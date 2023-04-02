import base64

def get_file(filepath: str) -> str:
    return base64.encodebytes(open(filepath, "rb").read()).decode("utf-8")
