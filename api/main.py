from flask import Flask, jsonify, make_response, request, abort
from flask_httpauth import HTTPBasicAuth

from api.db import Database

api = Flask(__name__)
auth = HTTPBasicAuth()
db = Database("server.db")

@auth.get_password
def get_password(username):
    if username == "shu_sha":
        return "antony_car"
    return None

@auth.error_handler
def unauthorized():
    return make_response(jsonify({'error': 'Unauthorized access'}), 401)

@api.route("/app/api/v1.0/db/detections/<detection_id>", methods=["GET"])
@auth.login_required
def get_detection(detection_id):
    # TODO: response from db
    if not detection_id and request.json:
        abort(404)
    return make_response(jsonify({"jopa": 123}), 201)

@api.route("/app/api/v1.0/db/detections", methods=["POST"])
@auth.login_required
def post_detection():
    if not request.json:
        abort(400)
    try:
        db.insert(request.json)
    except Exception as ex:
        return {"success": False, "error": str(ex)}, 400
    return {"success": True}, 201

@api.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)


if __name__ == "__main__":
    api.run(debug=True)