import base64

class ReportGenerator(object):
    def __init__(self, json):
        self.json = json

json_example = {
    1337: {
        "Личная жизнь":
        [
            {
                "name": "Московкин Александр",
                "place": "it-квадрат",
                "description": "МОСКВА ЖЕНАТ НА АЛЁНЕ И НЕ МОЖЕТ ЖЕНИТЬСЯ НА АНТОНЕ",
                "images":
                    [
                        {
                            "filename": "IMG_4162.HEIC",
                            "metadata": base64.b64encode(open("db/IMG_4162.HEIC", "rb").read()).decode("utf-8")
                        },
                        {
                            "filename": "IMG_4149.heic",
                            "metadata": base64.b64encode(open("db/IMG_4149.heic", "rb").read()).decode("utf-8")
                        }
                    ]
            },
            {
                "name": "Личная жизнь антона петлина",
                "place": "весь мир",
                "description": "её нет",
                "images": []
            }
        ],
        "fuck": [
            {
                "name": "Физика",
                "place": "школа",
                "description": "у максона 4...",
                "images": []
            }
        ]
    }
}

import requests.auth

print(
    requests.post(
        "http://127.0.0.1:5000/app/api/v1.0/db/detections",
        json=json_example,
        auth=requests.auth.HTTPBasicAuth("shu_sha", "antony_car")
    ).json()
)