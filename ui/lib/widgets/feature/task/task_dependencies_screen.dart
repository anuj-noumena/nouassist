import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/task_dependency_dto.dart';
import '../../../providers/task_dependency_provider.dart';
import '../../../providers/task_list_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_dropdown.dart';

class TaskDependenciesScreen extends ConsumerWidget {
  final String taskId;
  final String taskTitle;

  const TaskDependenciesScreen({
    super.key,
    required this.taskId,
    required this.taskTitle,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final dependenciesAsync = ref.watch(taskDependenciesProvider(taskId));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Task Dependencies'),
      ),
      body: dependenciesAsync.when(
        data: (dependencies) => _buildDependenciesView(context, ref, dependencies),
        loading: () => const AppLoadingIndicator(message: 'Loading...'),
        error: (error, _) => AppErrorWidget(error: error.toString()),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => _showAddDependencyDialog(context, ref),
        icon: const Icon(Icons.add_link),
        label: const Text('Add Dependency'),
      ),
    );
  }

  Widget _buildDependenciesView(
    BuildContext context,
    WidgetRef ref,
    List<TaskDependencyDto> dependencies,
  ) {
    final predecessors =
        dependencies.where((d) => d.successorTaskId == taskId).toList();
    final successors =
        dependencies.where((d) => d.predecessorTaskId == taskId).toList();

    return SingleChildScrollView(
      padding: const EdgeInsets.all(AppSpacing.md),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            padding: const EdgeInsets.all(AppSpacing.md),
            decoration: BoxDecoration(
              color: AppColors.info.withOpacity(0.1),
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
              border: Border.all(color: AppColors.info),
            ),
            child: Row(
              children: [
                const Icon(Icons.info_outline, color: AppColors.info),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: AppText(
                    'Dependencies help organize task execution order',
                    variant: TextVariant.bodySmall,
                    color: AppColors.info,
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: AppSpacing.xl),
          _buildSection(
            context,
            ref,
            'Blocked By',
            'This task depends on:',
            predecessors,
            true,
          ),
          const SizedBox(height: AppSpacing.xl),
          _buildSection(
            context,
            ref,
            'Blocking',
            'These tasks depend on this:',
            successors,
            false,
          ),
        ],
      ),
    );
  }

  Widget _buildSection(
    BuildContext context,
    WidgetRef ref,
    String title,
    String subtitle,
    List<TaskDependencyDto> dependencies,
    bool isPredecessor,
  ) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          title,
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.xs),
        AppText(
          subtitle,
          variant: TextVariant.bodySmall,
          color: AppColors.textSecondary,
        ),
        const SizedBox(height: AppSpacing.md),
        if (dependencies.isEmpty)
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.surfaceVariant,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Center(
              child: AppText(
                'No dependencies',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ),
          )
        else
          ...dependencies.map((dependency) {
            return _buildDependencyCard(context, ref, dependency, isPredecessor);
          }).toList(),
      ],
    );
  }

  Widget _buildDependencyCard(
    BuildContext context,
    WidgetRef ref,
    TaskDependencyDto dependency,
    bool isPredecessor,
  ) {
    final taskTitle = isPredecessor
        ? dependency.predecessorTaskTitle
        : dependency.successorTaskTitle;

    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(AppSpacing.sm),
                  decoration: BoxDecoration(
                    color: AppColors.primary.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  ),
                  child: Icon(
                    isPredecessor ? Icons.arrow_back : Icons.arrow_forward,
                    color: AppColors.primary,
                    size: 20,
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppText(
                    taskTitle,
                    variant: TextVariant.bodyMedium,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.delete_outline, size: 20),
                  color: AppColors.error,
                  onPressed: () => _confirmDelete(context, ref, dependency),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.sm),
            Row(
              children: [
                _buildDependencyTypeBadge(dependency.dependencyType),
                if (dependency.lagDays != 0) ...[
                  const SizedBox(width: AppSpacing.sm),
                  Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: AppSpacing.sm,
                      vertical: AppSpacing.xs,
                    ),
                    decoration: BoxDecoration(
                      color: AppColors.warning.withOpacity(0.1),
                      borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                    ),
                    child: Text(
                      '${dependency.lagDays > 0 ? '+' : ''}${dependency.lagDays} days',
                      style: const TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.w600,
                        color: AppColors.warning,
                      ),
                    ),
                  ),
                ],
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildDependencyTypeBadge(DependencyType type) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: AppColors.info.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        type.displayName,
        style: const TextStyle(
          fontSize: 12,
          fontWeight: FontWeight.w600,
          color: AppColors.info,
        ),
      ),
    );
  }

  Future<void> _showAddDependencyDialog(BuildContext context, WidgetRef ref) async {
    String? selectedTaskId;
    DependencyType selectedType = DependencyType.finishToStart;
    int lagDays = 0;

    final tasksAsync = await ref.read(taskListProvider.future);
    final availableTasks = tasksAsync.items.where((t) => t.id != taskId).toList();

    if (!context.mounted) return;

    final result = await showDialog<bool>(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('Add Dependency'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                AppText(
                  'Current task: $taskTitle',
                  variant: TextVariant.bodyMedium,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.md),
                DropdownButtonFormField<String>(
                  decoration: const InputDecoration(
                    labelText: 'Depends on task',
                    border: OutlineInputBorder(),
                  ),
                  items: availableTasks
                      .map((task) => DropdownMenuItem(
                            value: task.id,
                            child: Text(
                              task.title,
                              maxLines: 1,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ))
                      .toList(),
                  onChanged: (value) {
                    setState(() {
                      selectedTaskId = value;
                    });
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                AppDropdown<DependencyType>(
                  label: 'Dependency Type',
                  value: selectedType,
                  items: DependencyType.values
                      .map((type) => DropdownMenuItem(
                            value: type,
                            child: Text(type.displayName),
                          ))
                      .toList(),
                  onChanged: (value) {
                    if (value != null) {
                      setState(() {
                        selectedType = value;
                      });
                    }
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                TextField(
                  decoration: const InputDecoration(
                    labelText: 'Lag Days',
                    helperText: 'Positive for delay, negative for lead time',
                    border: OutlineInputBorder(),
                  ),
                  keyboardType: TextInputType.number,
                  onChanged: (value) {
                    setState(() {
                      lagDays = int.tryParse(value) ?? 0;
                    });
                  },
                ),
              ],
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context, false),
              child: const Text('Cancel'),
            ),
            ElevatedButton(
              onPressed: selectedTaskId != null
                  ? () => Navigator.pop(context, true)
                  : null,
              child: const Text('Add'),
            ),
          ],
        ),
      ),
    );

    if (result == true && selectedTaskId != null && context.mounted) {
      try {
        await ref.read(
          createDependencyProvider(
            CreateDependencyDto(
              predecessorTaskId: selectedTaskId!,
              successorTaskId: taskId,
              dependencyType: selectedType,
              lagDays: lagDays,
            ),
          ).future,
        );

        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Dependency added successfully'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to add dependency: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }
  }

  Future<void> _confirmDelete(
    BuildContext context,
    WidgetRef ref,
    TaskDependencyDto dependency,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Remove Dependency'),
        content: const Text('Are you sure you want to remove this dependency?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            style: TextButton.styleFrom(foregroundColor: AppColors.error),
            child: const Text('Remove'),
          ),
        ],
      ),
    );

    if (confirmed == true && context.mounted) {
      try {
        await ref.read(
          deleteDependencyProvider(dependency.id, taskId).future,
        );

        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Dependency removed'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to remove: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }
  }
}
