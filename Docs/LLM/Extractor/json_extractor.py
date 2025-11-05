#!/usr/bin/env python3

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

def run_source_blocks_extraction(input_file, output_path):
    with open(input_file, "r") as file:
        parsed_data = json.load(file)
        
        # Iterate through the JSON array
        for item in parsed_data:
            if "content" in item:
                for content_item in item["content"]:
                    filename = content_item.get("filename")
                    code = content_item.get("code", "No code provided")
                    filename = os.path.join(output_path, filename)
                    os.makedirs(os.path.dirname(filename), exist_ok=True)
                    with open(filename, 'w', encoding='utf-8') as outfile:
                        outfile.write(code + "\n")

def main():
    artifact = find_output_path()
    input_file = os.path.join(artifact, "output.txt")
    output_path = os.path.join(artifact, "Code")
    run_source_blocks_extraction(input_file, output_path)


if __name__ == "__main__":
    main()
