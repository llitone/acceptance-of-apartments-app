from flask import Flask, jsonify, make_response

api = Flask(__name__)

@api.route("/app/api/v1.0/generator_status", methods=["GET"])
def get_status():
    return make_response(jsonify({"status_code": 201}), 201)

if __name__ == "__main__":
    api.run(debug=True)