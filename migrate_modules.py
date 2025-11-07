#!/usr/bin/env python3
"""
Script to migrate code from bak folder to proper bounded context submodules.
Handles file copying and namespace refactoring for ABP framework modules.
"""

import os
import re
import shutil
from pathlib import Path
from typing import Dict, Tuple

# Mapping from bak folder to (BoundedContext, SubModule)
MODULE_MAPPING = {
    'Goals': ('CoreProductivity', 'GoalTracking'),
    'Habits': ('CoreProductivity', 'HabitSystems'),
    'Projects': ('CoreProductivity', 'ProjectManagement'),
    'Tasks': ('CoreProductivity', 'TaskManagement'),
    'Calendar': ('CoreProductivity', 'TimeManagement'),
    'Contacts': ('CollaborationHub', 'ContactManagement'),
    'Comments': ('CollaborationHub', 'DiscussionThreads'),
    'Resources': ('CollaborationHub', 'Resources'),
    'Team': ('CollaborationHub', 'TeamCollaboration'),
    'Meetings': ('CollaborationHub', 'TeamCollaboration'),
    'Users': ('CollaborationHub', 'UserProfiles'),
    'Activity': ('ReflectionAndSupport', 'ActivityTracking'),
    'CustomFields': ('ReflectionAndSupport', 'CustomFields'),
    'Reminders': ('ReflectionAndSupport', 'Reminders'),
    'Reviews': ('ReflectionAndSupport', 'Reviews'),
    'Approval': ('WorkflowOrchestration', 'ApprovalWorkflows'),
    'GTD': ('WorkflowOrchestration', 'GTDSystem'),
    'Workflow': ('WorkflowOrchestration', 'WorkflowEngine'),
    'Notifications': ('Infrastructure', 'Notifications'),
    'Search': ('Infrastructure', 'SearchEngine')
}

BASE_DIR = Path('/home/user/nouassist')
BAK_DIR = BASE_DIR / 'bak'
MODULES_DIR = BASE_DIR / 'modules'


def get_old_namespace(bak_module: str) -> str:
    """Get the old namespace pattern from bak module name."""
    return f"NouAssist.{bak_module}"


def get_new_namespace(bounded_context: str, submodule: str) -> str:
    """Get the new namespace pattern."""
    return f"NouAssist.{bounded_context}.{submodule}"


def get_target_path(bounded_context: str, submodule: str) -> Path:
    """Get the target path for the submodule."""
    return MODULES_DIR / bounded_context / 'modules' / submodule


def refactor_file_content(content: str, old_namespace: str, new_namespace: str,
                         bak_module: str) -> str:
    """
    Refactor the content of a file by replacing namespaces and references.

    Args:
        content: File content to refactor
        old_namespace: Old namespace (e.g., NouAssist.Goals)
        new_namespace: New namespace (e.g., NouAssist.CoreProductivity.GoalTracking)
        bak_module: Original bak module name (e.g., Goals)

    Returns:
        Refactored content
    """
    # Replace namespace declarations
    content = re.sub(
        rf'\bnamespace\s+{re.escape(old_namespace)}\b',
        f'namespace {new_namespace}',
        content
    )

    # Replace using statements
    content = re.sub(
        rf'\busing\s+{re.escape(old_namespace)}',
        f'using {new_namespace}',
        content
    )

    # Replace any full namespace references (e.g., in attributes, comments, etc.)
    content = re.sub(
        rf'\b{re.escape(old_namespace)}\.',
        f'{new_namespace}.',
        content
    )

    # Replace resource references that might have the old module name
    # E.g., GoalsResource -> GoalTrackingResource
    if 'Resource' in content:
        old_resource = f"{bak_module}Resource"
        # Extract the last part of the new namespace as the new module name
        new_module_name = new_namespace.split('.')[-1]
        new_resource = f"{new_module_name}Resource"
        content = content.replace(old_resource, new_resource)

    # Replace module class names
    # E.g., GoalsApplicationModule -> GoalTrackingApplicationModule
    old_module_pattern = f"{bak_module}([A-Z][a-zA-Z]+Module)"
    new_module_name = new_namespace.split('.')[-1]
    content = re.sub(
        old_module_pattern,
        lambda m: f"{new_module_name}{m.group(1)}",
        content
    )

    return content


def refactor_folder_name(folder_name: str, old_namespace: str, new_namespace: str) -> str:
    """
    Refactor folder names from old to new namespace.

    Args:
        folder_name: Original folder name (e.g., NouAssist.Goals.Application)
        old_namespace: Old namespace (e.g., NouAssist.Goals)
        new_namespace: New namespace (e.g., NouAssist.CoreProductivity.GoalTracking)

    Returns:
        New folder name
    """
    if folder_name.startswith(old_namespace):
        return folder_name.replace(old_namespace, new_namespace, 1)
    return folder_name


def migrate_module(bak_module: str, bounded_context: str, submodule: str):
    """
    Migrate a module from bak to its proper location.

    Args:
        bak_module: Name in bak folder (e.g., Goals)
        bounded_context: Target bounded context (e.g., CoreProductivity)
        submodule: Target submodule (e.g., GoalTracking)
    """
    print(f"\n{'='*80}")
    print(f"Migrating {bak_module} -> {bounded_context}/{submodule}")
    print(f"{'='*80}")

    source_path = BAK_DIR / bak_module
    target_path = get_target_path(bounded_context, submodule)

    if not source_path.exists():
        print(f"⚠️  Source path not found: {source_path}")
        return

    if not target_path.exists():
        print(f"⚠️  Target path not found: {target_path}")
        return

    old_namespace = get_old_namespace(bak_module)
    new_namespace = get_new_namespace(bounded_context, submodule)

    print(f"📁 Source: {source_path}")
    print(f"📁 Target: {target_path}")
    print(f"🔄 Namespace: {old_namespace} -> {new_namespace}")

    # Process src folder
    source_src = source_path / 'src'
    target_src = target_path / 'src'

    if source_src.exists():
        migrate_src_folder(source_src, target_src, old_namespace, new_namespace, bak_module)

    # Process test folder
    source_test = source_path / 'test'
    target_test = target_path / 'test'

    if source_test.exists():
        migrate_test_folder(source_test, target_test, old_namespace, new_namespace, bak_module)

    print(f"✅ Completed migration of {bak_module}")


def migrate_src_folder(source_src: Path, target_src: Path, old_namespace: str,
                      new_namespace: str, bak_module: str):
    """Migrate the src folder contents."""
    print(f"\n📦 Processing src folder...")

    # Get all project folders in source
    for source_project in source_src.iterdir():
        if not source_project.is_dir():
            continue

        # Get the corresponding target project folder name
        target_project_name = refactor_folder_name(
            source_project.name, old_namespace, new_namespace
        )
        target_project = target_src / target_project_name

        print(f"  📂 Processing {source_project.name} -> {target_project_name}")

        if not target_project.exists():
            print(f"    ⚠️  Target project folder not found, creating: {target_project}")
            target_project.mkdir(parents=True, exist_ok=True)

        # Copy all files from source to target
        copy_and_refactor_files(
            source_project, target_project, old_namespace, new_namespace, bak_module
        )


def migrate_test_folder(source_test: Path, target_test: Path, old_namespace: str,
                       new_namespace: str, bak_module: str):
    """Migrate the test folder contents."""
    print(f"\n🧪 Processing test folder...")

    # Get all test project folders in source
    for source_project in source_test.iterdir():
        if not source_project.is_dir():
            continue

        # Get the corresponding target project folder name
        target_project_name = refactor_folder_name(
            source_project.name, old_namespace, new_namespace
        )
        target_project = target_test / target_project_name

        print(f"  📂 Processing {source_project.name} -> {target_project_name}")

        if not target_project.exists():
            print(f"    ⚠️  Target test folder not found, creating: {target_project}")
            target_project.mkdir(parents=True, exist_ok=True)

        # Copy all files from source to target
        copy_and_refactor_files(
            source_project, target_project, old_namespace, new_namespace, bak_module
        )


def copy_and_refactor_files(source_dir: Path, target_dir: Path, old_namespace: str,
                           new_namespace: str, bak_module: str):
    """
    Recursively copy and refactor all files from source to target.

    Args:
        source_dir: Source directory
        target_dir: Target directory
        old_namespace: Old namespace
        new_namespace: New namespace
        bak_module: Original bak module name
    """
    for source_item in source_dir.rglob('*'):
        if not source_item.is_file():
            continue

        # Skip certain files
        if source_item.name in ['.DS_Store', 'obj', 'bin']:
            continue

        # Calculate relative path
        rel_path = source_item.relative_to(source_dir)
        target_item = target_dir / rel_path

        # Create parent directory if needed
        target_item.parent.mkdir(parents=True, exist_ok=True)

        # Process based on file type
        if source_item.suffix in ['.cs', '.csproj', '.json', '.xml', '.md', '.razor', '.cshtml']:
            # Text files that need refactoring
            try:
                with open(source_item, 'r', encoding='utf-8') as f:
                    content = f.read()

                # Refactor content
                refactored_content = refactor_file_content(
                    content, old_namespace, new_namespace, bak_module
                )

                # Write to target
                with open(target_item, 'w', encoding='utf-8') as f:
                    f.write(refactored_content)

                print(f"    ✅ Refactored: {rel_path}")
            except Exception as e:
                print(f"    ❌ Error processing {rel_path}: {e}")
        else:
            # Binary or other files, just copy
            shutil.copy2(source_item, target_item)
            print(f"    📄 Copied: {rel_path}")


def main():
    """Main migration function."""
    print("🚀 Starting module migration from bak to proper bounded contexts")
    print(f"📁 Base directory: {BASE_DIR}")
    print(f"📁 Bak directory: {BAK_DIR}")
    print(f"📁 Modules directory: {MODULES_DIR}")
    print(f"\n📋 Total modules to migrate: {len(MODULE_MAPPING)}")

    # Verify directories exist
    if not BAK_DIR.exists():
        print(f"❌ Bak directory not found: {BAK_DIR}")
        return

    if not MODULES_DIR.exists():
        print(f"❌ Modules directory not found: {MODULES_DIR}")
        return

    # Process each module
    for bak_module, (bounded_context, submodule) in MODULE_MAPPING.items():
        try:
            migrate_module(bak_module, bounded_context, submodule)
        except Exception as e:
            print(f"❌ Error migrating {bak_module}: {e}")
            import traceback
            traceback.print_exc()

    print(f"\n{'='*80}")
    print("🎉 Migration completed!")
    print(f"{'='*80}")


if __name__ == '__main__':
    main()
