import json
import os

def find_output_path():
    start_path = os.getcwd()
    target_name = "artifacts"
    
    current_dir = start_path
    
    while True:
        # Get the parent directory of the current directory
        parent_dir = os.path.dirname(current_dir)
        target_path = os.path.join(parent_dir, target_name)
        if os.path.isdir(target_path):
            return target_path
       
        # Stop if we've reached the root directory
        if parent_dir == current_dir:
            break
        current_dir = parent_dir

    return None

def load_json_file(file_path):
    try:
        with open(file_path, "r") as file:
            data = json.load(file)
        return data
    except FileNotFoundError:
        print(f"Error: File '{file_path}' not found")
        return None
    except json.JSONDecodeError:
        print(f"Error: Invalid JSON format in '{file_path}'")
        return None


def process_data(data):
    if data is None:
        return None
    # Process data here
    text_content = []
    out = data["mapping"].items()
    for _, (i, message) in enumerate(data["mapping"].items()):
        print(message)
        if (
                message["message"] and message["message"]["author"]["role"] == "assistant"
                and message["message"]["content"]["content_type"] == "text"
        ):
            text_content.append("\n".join(message["message"]["content"]["parts"]))

    return "\n".join(text_content)


def main(file_path):
    data = load_json_file(file_path)
    processed_data = process_data(data)
    return processed_data


if __name__ == "__main__":
    input_file = "../../../../artifacts/input.json"
    artifact = find_output_path()
    output_path = os.path.join(artifact, "output.txt")
    out = main(input_file)
    
    with open(output_path, "w") as f:
        f.write(out)
