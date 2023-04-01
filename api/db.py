import base64
import os
import sqlite3
from random import randint
from sqlite3 import Cursor


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
                fault_name STRING NOT NULL,
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
                """, (local_id, )
            )

    def __insert_into_categories(self, flat_id: int, category_name: str) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Categories` (flat_id, category_name) VALUES (?, ?)
                """, (flat_id, category_name)
            )

    def __insert_into_faults(
            self, category_id: int, fault_name: str, place: str, description: str
    ) -> Cursor:
        with self.connection:
            return self.cursor.execute(
                """
                INSERT INTO `Faults` (category_id, fault_name, place, description) VALUES (?, ?, ?, ?)
                """, (category_id, fault_name, place, description)
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
    def __get_image_name(image_name: str) -> str:
        name_chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        result_name = ""
        for i in range(30):
            slice_start = randint(0, len(name_chars) - 1)
            result_name += name_chars[slice_start: slice_start + 1]
        return "{0}.{1}".format(result_name, image_name.split(".")[-1])

    def get_flat_unique_id(self, flat_id: int) -> int:
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE local_id = ?""", (flat_id,)).fetchone()[0]

    def get_category_unique_ids(self, flat_id: int):
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE flat_id = ?""", (flat_id,)).fetchall()

    def get_faults_unique_ids(self, category_id: int):
        return self.cursor.execute("""SELECT `id` FROM `Flats` WHERE category_id = ?""", (category_id,)).fetchall()

    def get_images_paths(self, fault_id: int):
        return self.cursor.execute("""SELECT `image_path` FROM `Flats` WHERE fault_id = ?""", (fault_id,)).fetchall()

    def insert(self, json: dict):
        for flat in json.items():
            self.__insert_into_flats(flat[0])
            flat_id = self.__get_last_insert_id()
            for category in flat[1].items():
                self.__insert_into_categories(flat_id, category[0])
                category_id = self.__get_last_insert_id()
                for fault in category[1]:
                    self.__insert_into_faults(category_id, fault["name"], fault["place"], fault["description"])
                    fault_id = self.__get_last_insert_id()
                    for image in fault["images"]:
                        name = self.__get_image_name(image["filename"])
                        with open("db/images/{0}".format(name), "+wb") as file:
                            file.write(base64.b64decode(image["metadata"]))
                        self.__insert_into_images(fault_id, name)

