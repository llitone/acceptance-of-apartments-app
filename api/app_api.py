import base64

from flask import Flask, jsonify, make_response, request, abort
from flask_httpauth import HTTPBasicAuth

from api.db import Database
from api.config import login, password

application = Flask(__name__)
auth = HTTPBasicAuth()
db = Database("server.db")


@auth.get_password
def get_password(username):
    if username == login:
        return password
    return None


@auth.error_handler
def unauthorized():
    return make_response(jsonify({'error': 'Unauthorized access'}), 401)


@application.route("/app/api/v1.0/db/detections/<detection_id>", methods=["GET"])
@auth.login_required
def get_detection(detection_id):
    if not detection_id and request.json:
        abort(404)
    try:
        flat_report = db.get_pdf(detection_id)
    except Exception as ex:
        return {"error": str(ex)}, 400
    return make_response(
        jsonify(
            {"filename": flat_report,
             "metadata": base64.encodebytes(open(f"./db/reports/{flat_report}", "rb").read()).decode("utf-8")}
        ),
        201
    )


@application.route("/app/api/v1.0/db/detections/", methods=["POST"])
@auth.login_required
def post_detection():
    if not request.json:
        abort(400)
    try:
        db.insert(request.json)
    except Exception as ex:
        return {"success": False, "error": str(ex)}, 400
    return {"success": True}, 201


@application.route("/app/api/v1.0/db/detections/<detection_id>", methods=["DELETE"])
@auth.login_required
def delete_detection(detection_id):
    if not detection_id and request.json:
        abort(404)
    try:
        db.delete(detection_id)
    except Exception as ex:
        return {"error": str(ex)}, 400
    return {"success": True}, 201


@application.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)


if __name__ == "__main__":
    application.run(debug=True)
