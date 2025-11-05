import glob
import os # Needed for os.path.join
import subprocess

def find_files_glob(directory: str, extension: str) -> list[str]:
    # Remove leading dot if present
    if extension.startswith('.'):
        extension = extension[1:]

    # Check if the directory exists (glob doesn't error, just returns empty)
    if not os.path.isdir(directory):
         print(f"Error: Directory not found: {directory}")
         return []

    # Create the search pattern
    # Note: '**' enables recursive search
    search_pattern = os.path.join(directory, '**', f'*.{extension}')

    # Use glob with recursive=True
    # Need to filter results to ensure they are files, not directories
    found_files = [f for f in glob.glob(search_pattern, recursive=True) if os.path.isfile(f)]

    return found_files

search_dir = '../../Packages'
file_extension = '.sln' # Find markdown files

sln_files = find_files_glob(search_dir, file_extension)

if sln_files:
    print(f"Found {len(sln_files)} '{file_extension}' files (using glob):")
    for f in sln_files:
        dir_path = os.path.dirname(f)
        result = subprocess.run(
            ['abp', 'clean'],
            cwd=dir_path,
            capture_output=True,
            text=True,
            check=False  # Set to True if you want an exception on error
        )

        print(dir_path)
else:
    print(f"No '{file_extension}' files found in '{search_dir}' or its subdirectories (using glob).")