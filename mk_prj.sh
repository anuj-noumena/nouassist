#!/bin/bash

# Script to create the module structure for NouAssist based on the specified folder hierarchy
# Assumes ABP CLI and .NET SDK are installed and available
# Run from the root directory of the solution

set -e  # Exit on any error

# Ensure modules directory exists
mkdir -p modules

# Function to create a top-level module
create_top_level_module() {
    local module_name=$1
    local full_name="NouAssist.$module_name"
    echo "## Create $module_name module"
    mkdir -p "modules/$module_name"
    abp new-module "$full_name" -t module:ddd --no-ui --output-folder "modules/$module_name"
    rm "modules/$module_name/NouAssist.$module_name.abpsln"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.Application/NouAssist.$module_name.Application.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.Application.Contracts/NouAssist.$module_name.Application.Contracts.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.Domain/NouAssist.$module_name.Domain.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.Domain.Shared/NouAssist.$module_name.Domain.Shared.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.EntityFrameworkCore/NouAssist.$module_name.EntityFrameworkCore.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.HttpApi/NouAssist.$module_name.HttpApi.csproj" --solution-folder "modules/$module_name"
    dotnet sln "NouAssist.sln" add "modules/$module_name/src/NouAssist.$module_name.HttpApi.Client/NouAssist.$module_name.HttpApi.Client.csproj" --solution-folder "modules/$module_name"
}

# Function to create a sub-module and add it to parent's solution
create_sub_module() {
    local parent_dir=$1
    local parent_name=$2
    local sub_name=$3
    local full_sub_name="NouAssist.$parent_name.$sub_name"
    echo "## Create $sub_name sub-module under $parent_name"
    mkdir -p "$parent_dir/modules/$sub_name"
    abp new-module "$full_sub_name" -t module:ddd --no-ui --output-folder "$parent_dir/modules/$sub_name"
    rm "$parent_dir/modules/$sub_name/NouAssist.$parent_name.$sub_name.abpsln"
    # Add projects to parent's solution
    local parent_sln="$parent_dir/NouAssist.$parent_name.sln"
    local sub_solution_folder="modules/$sub_name"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Application/NouAssist.$parent_name.$sub_name.Application.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Application.Contracts/NouAssist.$parent_name.$sub_name.Application.Contracts.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Domain/NouAssist.$parent_name.$sub_name.Domain.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Domain.Shared/NouAssist.$parent_name.$sub_name.Domain.Shared.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.EntityFrameworkCore/NouAssist.$parent_name.$sub_name.EntityFrameworkCore.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.HttpApi/NouAssist.$parent_name.$sub_name.HttpApi.csproj" --solution-folder "$sub_solution_folder"
    dotnet sln "$parent_sln" add "$parent_dir/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.HttpApi.Client/NouAssist.$parent_name.$sub_name.HttpApi.Client.csproj" --solution-folder "$sub_solution_folder"
    cd ../..
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Application/NouAssist.$parent_name.$sub_name.Application.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Application.Contracts/NouAssist.$parent_name.$sub_name.Application.Contracts.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Domain/NouAssist.$parent_name.$sub_name.Domain.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.Domain.Shared/NouAssist.$parent_name.$sub_name.Domain.Shared.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.EntityFrameworkCore/NouAssist.$parent_name.$sub_name.EntityFrameworkCore.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.HttpApi/NouAssist.$parent_name.$sub_name.HttpApi.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    dotnet sln "NouAssist.sln" add "modules/$parent_name/modules/$sub_name/src/NouAssist.$parent_name.$sub_name.HttpApi.Client/NouAssist.$parent_name.$sub_name.HttpApi.Client.csproj" --solution-folder "modules/$parent_name/modules/$sub_name"
    cd "modules/$parent_name"
}

create_top_level_module "Integrations"