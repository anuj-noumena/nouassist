import json
import os
import re

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
        with open(file_path, 'r') as file:
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
    text_content = ''
    
    for message in data['chat_messages']:
        if message['sender'] == 'assistant':
            for content in message['content']:
                if content['type'] == 'text':
                    text_content += content['text'] + "\n"
                elif content['type'] == 'tool_use' and content.__contains__('input'):
                    if content['input'].__contains__('content'):
                        title = content['input']['title'] if content['input'].__contains__('title') else ''
                        text_content += f'<antArtifact title="{title}">\n'+content['input']['content']+'</antArtifact>' + "\n"
                    elif content['input'].__contains__('command') and content['input']['command'] == 'update' and content['input'].__contains__('new_str'):
                        text_content = text_content.replace(content['input']['old_str'], content['input']['new_str'])
                    

    return text_content

    
def extract_ant_artifacts(text):
    pattern = r'<antArtifact(?:\s+[^>]*)?>([\s\S]*?)<\/antArtifact>'

    # Find all matches in the sample_text.
    matches = re.findall(pattern, text, re.MULTILINE | re.DOTALL)
    extracted = [] 
    # Print each found match.
    for index, match in enumerate(matches):
        match = match.strip()
        extracted.append(match)
    

    return extracted


def main(artifact, file_path):
    data = load_json_file(file_path)
    print(isinstance(data, list))
    if isinstance(data, list):
        for item in data:
            if item["name"]:
                processed_data = process_data(item)
                output_path = os.path.join(artifact, "output.txt")
                # output_path = os.path.join(artifact, item["name"] + ".txt")
                with open(output_path, 'a') as f:
                    f.write(processed_data)
    else:
        processed_data = process_data(data)
        # extracted_content = extract_ant_artifacts(processed_data)
        output_path = os.path.join(artifact, "output.txt")
        # with open(output_path, 'w') as f:
        #     f.write("\n\n".join(extracted_content))
        with open(output_path, 'w') as f:
            f.write(processed_data)
    return processed_data

if __name__ == '__main__':
    artifact = find_output_path()

    input_file = os.path.join(artifact, "input.json")
    main(artifact, input_file)
    