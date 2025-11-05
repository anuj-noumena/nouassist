import json
import re
import sys
import pyperclip

# This script reads a text file 'wbs.txt', processes its content to extract

def wbs_json(text_data):
    """
    This function reads a text file 'wbs.txt', processes its content to extract
    specific patterns, and converts the extracted data into a JSON format.
    """
    # Initialize an empty list to hold the processed data
    processed_list = []
    current_item = None
    
    for line in text_data.strip().split('\n'):
        line = line.strip()
        if not line:
            continue

        # Regex to match lines starting with '##', '###', or '*'
        # It captures the numeric key and the title text that follows.
        match = re.match(r'^(?:#{2,5})\s+([0-9.]+)\s+(.*)', line)
        
        if match:
            # Extract the key (group 1) and title (group 2) from the match
            key = match.group(1)
            title = match.group(2).strip()
            current_item = {"key": key, "name": title}
            # Append the extracted data as a dictionary to our list
            processed_list.append(current_item)
        elif current_item:

            # Regex to match lines like '**Description**:' or '**Context**:'
            desc_match = re.match(r'^- \*\*(Description|Context|Action Steps|Outcome)\*\*:', line)
            if desc_match:
                current_item['is_leaf'] = True
                # Extract the description type and the text that follows
                desc_type = desc_match.group(1).lower()
                if( desc_type == "action steps"):
                    current_item['action_steps'] = []
                else:
                    current_item[desc_type] = line[desc_match.end():].strip()
            elif 'action_steps' in current_item:
                # If the line starts with a number, break out of the action steps section
                action_match = re.match(r'^\s*\d+\.\s', line)
                if action_match:
                    current_item['action_steps'].append({
                        "step": line[action_match.end():].strip(),
                        "completed": False  # Default value for is_completed
                    })


        else:
            # If the line does not match any expected pattern, we can ignore it or handle it as needed
            continue

    # Convert the list of dictionaries into a JSON formatted string
    # 'indent=4' makes the JSON output human-readable
    json_output = json.dumps(processed_list, indent=4)

    return json_output

def copy_to_clipboard(text):
    """
    This function copies the given text to the clipboard.
    """
    try:
        pyperclip.copy(text)
        print("Text copied to clipboard.")
    except ImportError:
        print("pyperclip module is not installed. Please install it to use this feature.")
    except Exception as e:
        print(f"An error occurred while copying to clipboard: {e}")

if __name__ == "__main__":
    # get file name from command line arguments

    if len(sys.argv) > 1:
        input_file = sys.argv[1]

        # Read the content of the input file
        with open(input_file, 'r') as file:
            text_data = file.read()

            # Call the function to process the text data and convert it to JSON
            json_txt = wbs_json(text_data)

            # Copy the JSON output to the clipboard
            copy_to_clipboard(json_txt)