from pathlib import Path
import subprocess
import os # Needed for os.path.join

def get_folders_pathlib(directory_path: str) -> list[str]:

    parent_path = Path(directory_path)

    # Check if the provided path is an existing directory
    if not parent_path.is_dir():
        print(f"Error: Path '{directory_path}' is not a valid directory.")
        return []

    folders = []
    try:
        # Iterate over all entries in the directory
        for entry_path in parent_path.iterdir():
            # Check if the entry is a directory
            if entry_path.is_dir() and "NouAssist" in entry_path.name:
                folders.append(str(entry_path)) # Convert Path object to string
    except OSError as e:
        print(f"Error accessing directory {directory_path}: {e}")
        return []

    return folders

target_dir = '../../Packages' # Get folders in the current directory

print(f"\nScanning directory: {Path(target_dir).resolve()} (using pathlib)") # Show absolute path

subfolders_pathlib = get_folders_pathlib(target_dir)

if subfolders_pathlib:
    print(f"\nFound {len(subfolders_pathlib)} folders:")
    for folder in subfolders_pathlib:
        modules_folders = get_folders_pathlib(f"{folder}/Modules")
        if modules_folders:
            package = Path(folder).name
            print(f"  - {package}")
            for modules_folder in modules_folders:
                module = Path(modules_folder).name
                print(f"    - {module}")
                result = subprocess.run(
                    ['abp', 'install-local-module', f'{modules_folder}/{module}.abpmdl', '-t', f'{package}.abpmdl'],
                    cwd=folder,
                    capture_output=True,
                    text=True,
                    check=False  # Set to True if you want an exception on error
                )
                if result.stdout:
                    print("\n--- Standard Output ---")
                    print(result.stdout)
                if result.stderr:
                    print("\n--- Standard Error ---")
                    print(result.stderr)


        else:
            print(f"  - {folder} (no modules found)")
else:
    print(f"\nNo folders found directly within '{target_dir}'.")

# To get just the folder names instead of full paths:
# Replace folders.append(str(entry_path)) with folders.append(entry_path.name)