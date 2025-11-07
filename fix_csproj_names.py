#!/usr/bin/env python3
"""
Script to rename .csproj files to match the new namespace structure.
"""

import os
import re
import shutil
from pathlib import Path

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
MODULES_DIR = BASE_DIR / 'modules'


def get_old_namespace(bak_module: str) -> str:
    """Get the old namespace pattern from bak module name."""
    return f"NouAssist.{bak_module}"


def get_new_namespace(bounded_context: str, submodule: str) -> str:
    """Get the new namespace pattern."""
    return f"NouAssist.{bounded_context}.{submodule}"


def fix_module_files(bak_module: str, bounded_context: str, submodule: str):
    """
    Fix file names and references for a module.

    Args:
        bak_module: Name in bak folder (e.g., Goals)
        bounded_context: Target bounded context (e.g., CoreProductivity)
        submodule: Target submodule (e.g., GoalTracking)
    """
    print(f"\n{'='*80}")
    print(f"Fixing files for {bak_module} -> {bounded_context}/{submodule}")
    print(f"{'='*80}")

    old_namespace = get_old_namespace(bak_module)
    new_namespace = get_new_namespace(bounded_context, submodule)

    target_path = MODULES_DIR / bounded_context / 'modules' / submodule

    if not target_path.exists():
        print(f"⚠️  Target path not found: {target_path}")
        return

    # Find and rename all .csproj, .abppkg, and .sln files
    for root, dirs, files in os.walk(target_path):
        root_path = Path(root)

        for filename in files:
            if filename.startswith(old_namespace):
                old_file_path = root_path / filename
                new_filename = filename.replace(old_namespace, new_namespace, 1)
                new_file_path = root_path / new_filename

                print(f"  📝 Renaming: {filename} -> {new_filename}")
                shutil.move(str(old_file_path), str(new_file_path))

                # If it's a .csproj or .sln file, update references inside
                if new_filename.endswith(('.csproj', '.sln')):
                    update_file_references(new_file_path, old_namespace, new_namespace, bak_module)


def update_file_references(file_path: Path, old_namespace: str, new_namespace: str, bak_module: str):
    """
    Update references inside .csproj and .sln files.

    Args:
        file_path: Path to the file to update
        old_namespace: Old namespace
        new_namespace: New namespace
        bak_module: Original bak module name
    """
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()

        # Replace project references
        content = re.sub(
            rf'{re.escape(old_namespace)}\.([A-Za-z.]+)\.csproj',
            f'{new_namespace}.\\1.csproj',
            content
        )

        # Replace assembly names
        content = re.sub(
            rf'<AssemblyName>{re.escape(old_namespace)}',
            f'<AssemblyName>{new_namespace}',
            content
        )

        # Replace root namespace
        content = re.sub(
            rf'<RootNamespace>{re.escape(old_namespace)}',
            f'<RootNamespace>{new_namespace}',
            content
        )

        # Write back
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)

        print(f"    ✅ Updated references in {file_path.name}")
    except Exception as e:
        print(f"    ❌ Error updating {file_path}: {e}")


def main():
    """Main function."""
    print("🔧 Fixing .csproj and project file names")
    print(f"📁 Modules directory: {MODULES_DIR}")

    if not MODULES_DIR.exists():
        print(f"❌ Modules directory not found: {MODULES_DIR}")
        return

    # Process each module
    for bak_module, (bounded_context, submodule) in MODULE_MAPPING.items():
        try:
            fix_module_files(bak_module, bounded_context, submodule)
        except Exception as e:
            print(f"❌ Error fixing {bak_module}: {e}")
            import traceback
            traceback.print_exc()

    print(f"\n{'='*80}")
    print("🎉 File name fixing completed!")
    print(f"{'='*80}")


if __name__ == '__main__':
    main()
