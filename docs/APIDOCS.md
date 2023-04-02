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

**flat_unique_id** - уникальный id квартиры (Пример: 22) <br>
**problem_category_name** - название категории определённого дефекта (Пример: Оконные конструкции)<br>
**place** - комната квартиры, в которой обнаружен дефект (Пример: Спальня)<br>
**description** - описание дефекта и номера по ГОСТ (Пример: Царапины и загрязнения на профилях рамы и створок балконного блока и окна. (К) ГОСТ 30674-99 п.5.3.5)<br>
**images** - к каждому дефекту обязательно нужно изображение, и на сервер передаётся:<br>
    filename (название файла)<br>
    metadata (закодированные байты изображения в формате Base64)<br>

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

Выше в примере запроса упоминаются **login** и **password**. Они задаются не пользователем, а компанией, которая будет пользоваться этой api.<br>
**your_domain_name** - доменное имя компании (сейчас api развёрнут на домене [d1ffic00lt.com](d1ffic00lt.com))<br>

### GET
#### Пример запроса
```python
import requests.auth
import base64

auth = requests.auth.HTTPBasicAuth(login, password)

response = requests.get("http://{your_domain_name}/app/api/v1.0/db/detections/{flat_unique_id}", auth=auth)
response = response.json()

with open(response["filename"], "+wb") as file:
    file.write(base64.b64decode(response["metadata"]))
```

Выше указан пример с запросом файла отчёта о квартире.

### DELETE
#### Пример запроса
```python
import requests.auth

auth = requests.auth.HTTPBasicAuth(login, password)

response = requests.delete("http://{your_domain_name}/app/api/v1.0/db/detections/{flat_unique_id}", auth=auth)
```

Выше представлен пример удаления данных об отчёте.
