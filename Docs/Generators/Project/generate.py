from pybars import Compiler
import os

compiler = Compiler()

packages = [
    "SharedKernel.Tagging"
]

template_file_names = [
    ("Domain", "Domain.csproj"),
    ("Domain", "DomainModule.cs"),
    ("Domain.Shared", "Domain.Shared.csproj"),
    ("Domain.Shared", "DomainSharedModule.cs"),
    ("Domain.Tests", "Domain.Tests.csproj"),
    ("Domain.Tests", "DomainTestModule.cs"),
    ("Application", "Application.csproj"),
    (
        "Application",
        "ApplicationModule.cs",
    ),
    (
        "Application.Contracts",
        "Application.Contracts.csproj",
    ),
    (
        "Application.Contracts",
        "ApplicationContractsModule.cs",
    ),
    (
        "Application.Tests",
        "Application.Tests.csproj",
    ),
    (
        "Application.Tests",
        "ApplicationTestModule.cs",
    ),
    ("Infrastructure", "Infrastructure.csproj"),
    (
        "Infrastructure",
        "InfrastructureModule.cs",
    ),
]


def generate_template(tmpl_path):
    """Generates a compiled Handlebars template from a file."""
    try:
        with open(tmpl_path, "r", encoding="utf-8") as file:
            content = file.read()
            return compiler.compile(content)
    except FileNotFoundError:
        print(f"Error: File '{tmpl_path}' not found.")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None


def generate_file(template, package_name, module_name, file_name):
    """Generates a file from a template and writes it to the output directory."""
    if template:
        output = template({"package": package_name, "module": module_name})
        
        with open(file_name, "w", encoding="utf-8") as file:
            file.write(output)


def create_project_files(package_name, module_name):
    """Initializes a project by generating multiple files from templates."""
    for tmpl_name in template_file_names:
        path_prefix, tmpl_name = tmpl_name
        path_prefix = f"./out/{package_name}/{path_prefix}"
        print(path_prefix)
        os.makedirs(path_prefix, exist_ok=True)
        template = generate_template(f"tmpl/{tmpl_name}.tmpl")
        file_name = f"{path_prefix}/{package_name}.{tmpl_name}"
        generate_file(template, package_name, module_name, file_name)


if __name__ == "__main__":
    for package in packages:
        full_package_name = "NouAssist." + package
        module_name = package.replace(".", "")
        create_project_files(full_package_name, module_name)
