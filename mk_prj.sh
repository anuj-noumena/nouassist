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

# # Create CoreProductivity
# create_top_level_module "CoreProductivity"
# cd modules/CoreProductivity
# # Sub-modules
# create_sub_module . "CoreProductivity" "TaskManagement"
# create_sub_module . "CoreProductivity" "ProjectManagement"
# create_sub_module . "CoreProductivity" "GoalTracking"
# create_sub_module . "CoreProductivity" "HabitSystems"
# create_sub_module . "CoreProductivity" "TimeManagement"
# cd ../..

# # Create WorkflowOrchestration
# create_top_level_module "WorkflowOrchestration"
# cd modules/WorkflowOrchestration
# # Sub-modules
# create_sub_module . "WorkflowOrchestration" "WorkflowEngine"
# create_sub_module . "WorkflowOrchestration" "ApprovalWorkflows"
# create_sub_module . "WorkflowOrchestration" "GTDSystem"
# cd ../..

# # Create CollaborationHub
# create_top_level_module "CollaborationHub"
# cd modules/CollaborationHub
# # Sub-modules (UserProfiles, ContactManagement, Resources are leaf; TeamCollaboration has sub-sub-modules)
# create_sub_module . "CollaborationHub" "UserProfiles"
# create_sub_module . "CollaborationHub" "TeamCollaboration"
# create_sub_module . "CollaborationHub" "DiscussionThreads"
# create_sub_module . "CollaborationHub" "ContactManagement"
# create_sub_module . "CollaborationHub" "Resources"

# cd ../..

# # Create ReflectionAndSupport
# create_top_level_module "ReflectionAndSupport"
# cd modules/ReflectionAndSupport
# # Sub-modules (all leaf)
# create_sub_module . "ReflectionAndSupport" "Reviews"
# create_sub_module . "ReflectionAndSupport" "Journaling"
# create_sub_module . "ReflectionAndSupport" "ActivityTracking"
# create_sub_module . "ReflectionAndSupport" "Reminders"
# create_sub_module . "ReflectionAndSupport" "CustomFields"
# cd ../..

# # Create Infrastructure
# create_top_level_module "Infrastructure"
# cd modules/Infrastructure
# # Sub-modules (all leaf)
# create_sub_module . "Infrastructure" "SearchEngine"
# create_sub_module . "Infrastructure" "Notifications"
# cd ../..

# # Create SharedKernel (no sub-modules)
# create_top_level_module "SharedKernel"

# echo "Module structure creation completed!"

# create_top_level_module "IdentityAccess"
# create_top_level_module "WorkManagement"
# create_top_level_module "FinancialManagement"
# create_top_level_module "TimeManagement"
# create_top_level_module "LearningDevelopment"
# create_top_level_module "AssetManagement"
# create_top_level_module "Collaboration"
# create_top_level_module "Notification"
# create_top_level_module "Customization"
# create_top_level_module "SharedKernel"
create_top_level_module "Infrastructure"