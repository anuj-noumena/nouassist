import os
import pyperclip
import sys


def recursively_get_content(content, source_dir):
    for root, _, files in os.walk(source_dir):
        for file in files:
            file_path = os.path.join(root, file)
            with open(file_path, "r") as infile:
                content.append(f'// {file_path.replace(source_dir, "src")}\n')

def process(source_directory):   
    content = []
    for source in source_directory:
        recursively_get_content(content, source)
    content_str = "\n\n".join(content)

    pyperclip.copy(content_str)

if __name__ == "__main__":
    if(len(sys.argv) > 1):
        source_directory =  sys.argv[1].splitlines()
        process(source_directory)
