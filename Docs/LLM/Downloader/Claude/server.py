from flask import Flask, request, jsonify, abort
from flask_cors import CORS  # Import the CORS extension
import json
import os

app = Flask(__name__)
CORS(app)  # Enable CORS for all routes


# --- Configuration (Adjust as needed) ---
SAVE_DIR = "received_data"  # Directory to save the JSON files
# --- End Configuration ---


# --- Helper Functions ---
def ensure_directory_exists(directory):
    """Creates the directory if it doesn't exist."""
    if not os.path.exists(directory):
        os.makedirs(directory)

def save_json_to_file(data, filename):
    """Saves the given JSON data to a file."""
    filepath = os.path.join(SAVE_DIR, filename)
    try:
        with open(filepath, 'w') as f:
            json.dump(data, f, indent=4)  # Use indent for pretty printing
        return True
    except Exception as e:
        print(f"Error saving file: {e}")  # Log the error
        return False

# --- End Helper Functions ---


@app.route('/', methods=['POST'])
def receive_data():
    """Receives POSTed JSON data and saves it to a file."""
    if not request.is_json:
        abort(400, description="Request must be JSON")  # Use abort for proper HTTP errors

    try:
        data = request.get_json()  # Get the JSON data from the request
    except Exception as e:
        abort(400, description=f"Invalid JSON: {e}") #More specific error.

    filename = request.args.get('filename')

    if save_json_to_file(data, filename):
        return jsonify({"message": "Data received and saved successfully!", "filename": filename}), 201  # 201 Created
    else:
        abort(500, description="Error saving data to file")   # 500 Internal Server Error

@app.route('/', methods=['GET'])
def list_saved_files():
  """Lists the files saved in the SAVE_DIR."""
  ensure_directory_exists(SAVE_DIR)  # Ensure directory exists before listing
  try:
    files = [f for f in os.listdir(SAVE_DIR) if os.path.isfile(os.path.join(SAVE_DIR, f))]
    return jsonify({"files": files}), 200
  except Exception as e:
    abort(500, description=f"Error listing files: {e}")

@app.route('/<filename>', methods=['GET'])
def get_saved_file(filename):
    """Retrieves a specific saved JSON file."""
    filepath = os.path.join(SAVE_DIR, filename)

    if not os.path.exists(filepath):
        abort(404, description=f"File '{filename}' not found")  # 404 Not Found

    try:
        with open(filepath, 'r') as f:
            data = json.load(f)
            return jsonify(data), 200  # Return the JSON data directly
    except Exception as e:
        abort(500, description=f"Error reading file: {e}")

@app.errorhandler(400)
def bad_request(error):
    return jsonify({'error': 'Bad Request', 'message': error.description}), 400

@app.errorhandler(404)
def not_found(error):
  return jsonify({'error': 'Not Found', 'message': error.description}), 404
  
@app.errorhandler(500)
def internal_error(error):
  return jsonify({'error': 'Internal Server Error', 'message': error.description}), 500


if __name__ == '__main__':
    ensure_directory_exists(SAVE_DIR) #create save directory on startup.
    app.run(debug=True, host='0.0.0.0', port=5000) #added host and port for wider access and clarity.