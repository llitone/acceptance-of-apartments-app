import base64
import os
import sqlite3

from random import randint
from sqlite3 import Cursor

from api.report_generator import ReportGenerator


class Database(object):
    def __init__(self, filename: str):
        self.filename = filename
        if not os.path.isdir("db"):
            os.mkdir("db")
        if not os.path.isdir("db/images"):
            os.mkdir("db/images")
        self.connection = sqlite3.connect("./db/" + self.filename, check_same_thread=False)
        self.cursor = self.connection.cursor()

        self.cursor.execute(
            """
                CREATE TABLE IF NOT EXISTS Flats (
                    id INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT,
                    local_id INTEGER UNIQUE NOT NULL
                )
            """
        )
        self.cursor.execute(
            """
                CREATE TABLE IF NOT EXISTS Categories (
                    id INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT, 
                    flat_id INTEGER NOT NULL, 
                    category_name STRING NOT NULL, 
                    FOREIGN KEY(flat_id) REFERENCES Flats(id)
                )
            """
        )
        self.cursor.execute(
            """
            CREATE TABLE IF NOT EXISTS Faults (
                id INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT,
                category_id INTEGER NOT NULL, 
                place STRING NOT NULL,
                description STRING NOT NULL,
                FOREIGN KEY(category_id) REFERENCES Categories(id)
            )
            """
        )
        self.cursor.execute(
            """
            CREATE TABLE IF NOT EXISTS Images (
                id INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT,
                fault_id INTEGER NOT NULL,
                image_path STRING NOT NULL UNIQUE,
                FOREIGN KEY(fault_id) REFERENCES Faults(id)
            )
            """
        )

    def __insert_into_flats(self, local_id: int) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Flats` (local_id) VALUES (?)
                """, (local_id,)
            )

    def __insert_into_categories(self, flat_id: int, category_name: str) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Categories` (flat_id, category_name) VALUES (?, ?)
                """, (flat_id, category_name)
            )

    def __insert_into_faults(
            self, category_id: int, place: str, description: str
    ) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Faults` (category_id, place, description) VALUES (?, ?, ?)
                """, (category_id, place, description)
            )

    def __insert_into_images(self, fault_id: int, image_path) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Images` (fault_id, image_path) VALUES (?, ?)
                """, (fault_id, image_path)
            )

    def __get_last_insert_id(self) -> int:
        return self.cursor.execute("SELECT last_insert_rowid()").fetchone()[0]

    @staticmethod
    def __get_name(image_name: str) -> str:
        name_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        result_name = ""
        for i in range(30):
            slice_start = randint(0, len(name_chars) - 1)
            result_name += name_chars[slice_start: slice_start + 1]
        return "{0}.{1}".format(result_name, image_name.split(".")[-1])

    def __get_flat_unique_id(self, local_id: int) -> int:
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE `local_id` = ?""", (local_id,)).fetchone()[0]

    def __get_category_unique_ids(self, flat_id: int) -> tuple:
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE `flat_id` = ?""", (flat_id,)).fetchall()

    def __get_faults_unique_ids(self, category_id: int) -> tuple:
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE `category_id` = ?""", (category_id,)).fetchall()

    def __get_images_paths(self, fault_id: int) -> tuple:
        return self.cursor.execute("""SELECT `image_path` FROM `Flats` WHERE fault_id = ?""", (fault_id,)).fetchall()

    def __delete_flat(self, local_id: int) -> Cursor:
        with self.connection:
            return self.cursor.execute("""DELETE FROM `Flats` WHERE `local_id` = ?""", (local_id,))

    def __delete_category(self, flat_id: int) -> Cursor:
        with self.connection:
            return self.cursor.execute("""DELETE FROM `Categories` WHERE `flat_id` = ?""", (flat_id,))

    def __delete_faults(self, category_id: int) -> Cursor:
        with self.connection:
            return self.cursor.execute("""DELETE FROM `Faults` WHERE `category_id` = ?""", (category_id,))

    def __delete_images(self, fault_id: int) -> Cursor:
        with self.connection:
            return self.cursor.execute("""DELETE FROM `Images` WHERE fault_id = ?""", (fault_id,))

    def insert(self, json: dict):
        for flat in json.items():
            self.__insert_into_flats(flat[0])
            flat_id = self.__get_last_insert_id()
            for category in flat[1].items():
                self.__insert_into_categories(flat_id, category[0])
                category_id = self.__get_last_insert_id()
                for fault in category[1]:
                    self.__insert_into_faults(category_id, fault["place"], fault["description"])
                    fault_id = self.__get_last_insert_id()
                    for image in fault["images"]:
                        name = self.__get_name(image["filename"])
                        with open("db/images/{0}".format(name), "+wb") as file:
                            file.write(base64.b64decode(image["metadata"]))
                        self.__insert_into_images(fault_id, name)

    def delete(self, flat_id: int):
        local_flat_id = self.__select_from_flats(flat_id)[0][0]
        for category in self.__select_from_categories(local_flat_id):
            for fault in self.__select_from_faults(category[0]):
                self.__delete_images(fault[0])
            self.__delete_faults(category[0])
        self.__delete_category(local_flat_id)
        self.__delete_flat(flat_id)

    def __select_from_flats(self, flat_id: int):
        return self.cursor.execute("""SELECT * FROM `Flats` WHERE `local_id` = ?""", (flat_id,)).fetchall()

    def __select_from_categories(self, flat_id: int):
        return self.cursor.execute("""SELECT * FROM `Categories` WHERE `flat_id` = ?""", (flat_id,)).fetchall()

    def __select_from_faults(self, category_id: int):
        return self.cursor.execute("""SELECT * FROM `Faults` WHERE `category_id` = ?""", (category_id,)).fetchall()

    def __select_from_images(self, fault_id: int):
        return self.cursor.execute("""SELECT * FROM `Images` WHERE `fault_id` = ?""", (fault_id,)).fetchall()

    def __get_flat(self, flat_id: int) -> dict:
        data = {flat_id: {}}
        local_flat_id = self.__select_from_flats(flat_id)[0][0]
        for category in self.__select_from_categories(local_flat_id):
            data[flat_id][category[2]] = []
            for fault in self.__select_from_faults(category[0]):
                fault_data = {
                    "place": fault[2],
                    "description": fault[3],
                    "images": []
                }
                for image in self.__select_from_images(fault[0]):
                    fault_data["images"].append(f"./db/images/" + image[2])
                data[flat_id][category[2]].append(fault_data)
        return data

    def get_pdf(self, flat_id: int) -> str:
        name = self.__get_name("output.docx")
        generator = ReportGenerator(name, self.__get_flat(flat_id))
        generator.convert_json()
        generator.save()
        return name
