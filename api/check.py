import base64
import requests.auth

json_example = {
    10: {
        "окна":
        [
            {
                "name": "Косяк",
                "place": "Коровник",
                "description": "очень косяк",
                "images": [
                    {
                        "filename": "test.png",
                        "metadata": base64.encodebytes(open(f"test.png", "rb").read()).decode("utf-8")
                    }
                ]
            }
        ]
    }
}

resp = requests.post(
    "http://127.0.0.1:5000/app/api/v1.0/db/detections/",
    json=json_example,
    auth=requests.auth.HTTPBasicAuth("shu_sha", "antony_car")
)
print(resp)
print(resp.json())
response = requests.get("http://127.0.0.1:5000/app/api/v1.0/db/detections/10", auth=requests.auth.HTTPBasicAuth("shu_sha", "antony_car"))
print(response)
response = response.json()
with open(response["filename"], "+wb") as file:
    file.write(base64.b64decode(response["metadata"]))
