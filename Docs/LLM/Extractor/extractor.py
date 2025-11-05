#!/usr/bin/env python3

import os
import re
import argparse

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

def find_source_blocks(content):
    """
    Find blocks of the form:

    // src/path/to/file.ts
    ... code ...
    (stop at the next `// src/` or end of file)
    """
    pattern = r'// (?P<filename>src/[^\n]+)\n(?P<content>.*?)(?=(// src/|$))'
    regex = re.compile(pattern, re.DOTALL)
    matches = regex.finditer(content)

    blocks = []
    for match in matches:
        filename = match.group('filename').strip()
        code = match.group('content').strip()
        blocks.append({"filename": filename, "content": code})
    return blocks

def run_source_blocks_extraction(file_path, output_path, replace=False):
    """
    Extract code blocks that start with "// src/path/to/file" lines
    and write them to the corresponding file.
    """
    with open(file_path, 'r', encoding='utf-8') as f:
        text = f.read()

    blocks = find_source_blocks(text)
    
    file_mode = 'w' if replace else 'a'
    print(file_mode)
    for block in blocks:
        filename = block["filename"]
        content = block["content"]
        filename = os.path.join(output_path, filename)
        os.makedirs(os.path.dirname(filename), exist_ok=True)
        with open(filename, file_mode, encoding='utf-8') as outfile:
            outfile.write(content + "\n")


def main():
    parser = argparse.ArgumentParser(
        description="Extracts source code blocks from a text file and saves them to their respective file paths."
    )
    parser.add_argument(
        '--replace',
        action='store_true',
        help='If specified, existing files will be replaced instead of appended to.'
    )
    args = parser.parse_args()
    
    artifact = find_output_path()
    input_file = os.path.join(artifact, "output.txt")
    output_path = os.path.join(artifact, "Code")
    run_source_blocks_extraction(input_file, output_path, replace=args.replace)


if __name__ == "__main__":
    main()
