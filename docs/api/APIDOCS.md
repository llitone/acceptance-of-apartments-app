<h1 align="center">API</h1>



## 1. Обзор

DefectDetect API — это API на основе JSON, которое компании могут развернуть на своих серверах.

## 2. Пример запросов на языке Python

### POST
#### Пример запроса в формате json
```python
{
    flat_unique_id: {
        problem_category_name: [
            {
                "name": "",
                "place": "",
                "description": "",
                "images": [
                    {
                        "filename": "",
                        "metadata": ""
                    }
                ]
            }
        ]
    }
}
```
#### Пример кодировки картинок для запроса (metadata)
```python
import base64

metadata = base64.encodebytes(open(f"test.png", "rb").read()).decode("utf-8")
```

#### Пример запроса

```python
import requests.auth

auth = requests.auth.HTTPBasicAuth(login, password)
response = requests.post("http://{your_domain_name}/app/api/v1.0/db/detections/", auth=auth, json=json_example)
```

### GET
#### Пример запроса
```python
import requests.auth

auth = requests.auth.HTTPBasicAuth(login, password)

response = requests.get("http://{your_domain_name}/app/api/v1.0/db/detections/{flat_unique_id}", auth=auth)
response = response.json()

with open(response["filename"], "+wb") as file:
    file.write(base64.b64decode(response["metadata"]))
```

### DELETE
#### Пример запроса
```python
import requests.auth

auth = requests.auth.HTTPBasicAuth(login, password)

response = requests.delete("http://{your_domain_name}/app/api/v1.0/db/detections/{flat_unique_id}", auth=auth)
```
