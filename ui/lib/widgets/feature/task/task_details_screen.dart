import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'widgets/task_checklist_widget.dart';
import 'widgets/task_action_steps_widget.dart';
import 'widgets/task_acceptance_criteria_widget.dart';
import 'widgets/task_discussion_widget.dart';
import 'task_delegation_screen.dart';
import 'task_dependencies_screen.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../core/utils/extensions.dart';
import '../../../data/models/task_dto.dart';
import '../../../providers/task_provider.dart';
import '../../../providers/task_delete_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../atomic/app_chip.dart';
import '../../atomic/app_button.dart';
import 'task_form_screen.dart';

class TaskDetailsScreen extends ConsumerWidget {
  final String taskId;

  const TaskDetailsScreen({super.key, required this.taskId});

  @override
  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final taskAsync = ref.watch(taskProvider(taskId));

    return DefaultTabController(
      length: 6,
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Task Details'),
          actions: [
            IconButton(
              icon: const Icon(Icons.edit),
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (_) => TaskFormScreen(taskId: taskId),
                  ),
                );
              },
            ),
            IconButton(
              icon: const Icon(Icons.delete),
              onPressed: () => _confirmDelete(context, ref),
            ),
          ],
          bottom: const TabBar(
            isScrollable: true,
            tabs: [
              Tab(text: 'Details'),
              Tab(text: 'Checklist'),
              Tab(text: 'Actions'),
              Tab(text: 'Criteria'),
              Tab(text: 'Discussion'),
              Tab(text: 'More'),
            ],
          ),
        ),
        body: taskAsync.when(
          data: (task) => TabBarView(
            children: [
              _buildTaskDetails(context, task),
              SingleChildScrollView(
                padding: const EdgeInsets.all(AppSpacing.md),
                child: TaskChecklistWidget(taskId: taskId),
              ),
              SingleChildScrollView(
                padding: const EdgeInsets.all(AppSpacing.md),
                child: TaskActionStepsWidget(taskId: taskId),
              ),
              SingleChildScrollView(
                padding: const EdgeInsets.all(AppSpacing.md),
                child: TaskAcceptanceCriteriaWidget(taskId: taskId),
              ),
              SingleChildScrollView(
                padding: const EdgeInsets.all(AppSpacing.md),
                child: TaskDiscussionWidget(taskId: taskId),
              ),
              _buildMoreTab(context, task),
            ],
          ),
          loading: () => const AppLoadingIndicator(message: 'Loading task...'),
          error: (error, stack) => AppErrorWidget(
            error: error.toString(),
            onRetry: () => ref.invalidate(taskProvider(taskId)),
          ),
        ),
      ),
    );
  }

  Widget _buildTaskDetails(BuildContext context, TaskDto task) {
    return RefreshIndicator(
      onRefresh: () async {
        // Refresh task details
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildHeader(task),
            const SizedBox(height: AppSpacing.lg),
            _buildInfoSection(
              'Description',
              task.description ?? 'No description provided',
            ),
            const SizedBox(height: AppSpacing.lg),
            _buildDetailsGrid(task),
            const SizedBox(height: AppSpacing.lg),
            if (task.tags.isNotEmpty) _buildTagsSection(task),
            const SizedBox(height: AppSpacing.lg),
            _buildProgressSection(task),
            const SizedBox(height: AppSpacing.lg),
            _buildMetadataSection(task),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader(TaskDto task) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Container(
              width: 6,
              height: 40,
              decoration: BoxDecoration(
                color: _getPriorityColor(task.priority),
                borderRadius: BorderRadius.circular(3),
              ),
            ),
            const SizedBox(width: AppSpacing.md),
            Expanded(child: AppText(task.title, variant: TextVariant.h2)),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        Row(
          children: [
            _buildStatusBadge(task.status),
            const SizedBox(width: AppSpacing.sm),
            _buildPriorityBadge(task.priority),
          ],
        ),
      ],
    );
  }

  Widget _buildInfoSection(String title, String content) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(title, variant: TextVariant.h5, fontWeight: FontWeight.w600),
        const SizedBox(height: AppSpacing.sm),
        AppText(
          content,
          variant: TextVariant.bodyMedium,
          color: AppColors.textSecondary,
        ),
      ],
    );
  }

  Widget _buildDetailsGrid(TaskDto task) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          'Details',
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.md),
        _buildDetailRow(
          'Start Date',
          task.startDate?.toShortDateString() ?? 'Not set',
        ),
        _buildDetailRow(
          'Due Date',
          task.dueDate?.toShortDateString() ?? 'Not set',
        ),
        if (task.completedDate != null)
          _buildDetailRow('Completed', task.completedDate!.toShortDateString()),
        _buildDetailRow(
          'Estimated Hours',
          task.estimatedHours?.toString() ?? 'Not set',
        ),
        _buildDetailRow(
          'Actual Hours',
          task.actualHours?.toString() ?? 'Not set',
        ),
        _buildDetailRow(
          'Story Points',
          task.storyPoints?.toString() ?? 'Not set',
        ),
      ],
    );
  }

  Widget _buildDetailRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 140,
            child: AppText(
              label,
              variant: TextVariant.bodyMedium,
              color: AppColors.textSecondary,
            ),
          ),
          Expanded(
            child: AppText(
              value,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w500,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTagsSection(TaskDto task) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText('Tags', variant: TextVariant.h5, fontWeight: FontWeight.w600),
        const SizedBox(height: AppSpacing.sm),
        Wrap(
          spacing: AppSpacing.sm,
          runSpacing: AppSpacing.sm,
          children: task.tags.map((tag) {
            return AppChip(
              label: tag,
              backgroundColor: AppColors.primary.withOpacity(0.1),
              textColor: AppColors.primary,
            );
          }).toList(),
        ),
      ],
    );
  }

  Widget _buildProgressSection(TaskDto task) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Progress',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            AppText(
              '${task.progress.toInt()}%',
              variant: TextVariant.bodyLarge,
              fontWeight: FontWeight.w600,
              color: AppColors.primary,
            ),
          ],
        ),
        const SizedBox(height: AppSpacing.sm),
        ClipRRect(
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          child: LinearProgressIndicator(
            value: task.progress / 100,
            minHeight: 8,
            backgroundColor: AppColors.border,
            valueColor: AlwaysStoppedAnimation<Color>(
              _getStatusColor(task.status),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildMetadataSection(TaskDto task) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surfaceVariant,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          AppText(
            'Metadata',
            variant: TextVariant.labelSmall,
            color: AppColors.textSecondary,
          ),
          const SizedBox(height: AppSpacing.sm),
          _buildMetadataRow('Created', task.creationTime.toDateTimeString()),
          if (task.lastModificationTime != null)
            _buildMetadataRow(
              'Last Modified',
              task.lastModificationTime!.toDateTimeString(),
            ),
          _buildMetadataRow('Task ID', task.id),
        ],
      ),
    );
  }

  Widget _buildMetadataRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.xs),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 100,
            child: AppText(
              label,
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ),
          Expanded(child: AppText(value, variant: TextVariant.bodySmall)),
        ],
      ),
    );
  }

  Widget _buildStatusBadge(TaskStatus status) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: _getStatusColor(status).withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
        border: Border.all(color: _getStatusColor(status)),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 8,
            height: 8,
            decoration: BoxDecoration(
              color: _getStatusColor(status),
              shape: BoxShape.circle,
            ),
          ),
          const SizedBox(width: AppSpacing.xs),
          Text(
            status.displayName,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: _getStatusColor(status),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPriorityBadge(TaskPriority priority) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: _getPriorityColor(priority).withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
        border: Border.all(color: _getPriorityColor(priority)),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.flag, size: 14, color: _getPriorityColor(priority)),
          const SizedBox(width: AppSpacing.xs),
          Text(
            priority.displayName,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: _getPriorityColor(priority),
            ),
          ),
        ],
      ),
    );
  }

  Color _getPriorityColor(TaskPriority priority) {
    switch (priority) {
      case TaskPriority.lowest:
        return AppColors.priorityLowest;
      case TaskPriority.low:
        return AppColors.priorityLow;
      case TaskPriority.medium:
        return AppColors.priorityMedium;
      case TaskPriority.high:
        return AppColors.priorityHigh;
      case TaskPriority.highest:
        return AppColors.priorityHighest;
    }
  }

  Color _getStatusColor(TaskStatus status) {
    switch (status) {
      case TaskStatus.todo:
        return AppColors.statusTodo;
      case TaskStatus.inProgress:
        return AppColors.statusInProgress;
      case TaskStatus.inReview:
        return AppColors.statusInReview;
      case TaskStatus.done:
        return AppColors.statusDone;
      case TaskStatus.blocked:
        return AppColors.statusBlocked;
    }
  }

  Future<void> _confirmDelete(BuildContext context, WidgetRef ref) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Task'),
        content: const Text(
          'Are you sure you want to delete this task? This action cannot be undone.',
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () => Navigator.of(context).pop(true),
            style: TextButton.styleFrom(foregroundColor: AppColors.error),
            child: const Text('Delete'),
          ),
        ],
      ),
    );

    if (confirmed == true && context.mounted) {
      try {
        await ref.read(deleteTaskProvider(taskId).future);
        if (context.mounted) {
          Navigator.of(context).pop();
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Task deleted successfully'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to delete task: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }
  }

  Widget _buildMoreTab(BuildContext context, TaskDto task) {
    return ListView(
      padding: const EdgeInsets.all(AppSpacing.md),
      children: [
        ListTile(
          leading: const Icon(Icons.link),
          title: const Text('Dependencies'),
          subtitle: const Text('Manage task dependencies'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (_) => TaskDependenciesScreen(
                  taskId: task.id,
                  taskTitle: task.title,
                ),
              ),
            );
          },
        ),
        const Divider(),
        ListTile(
          leading: const Icon(Icons.people),
          title: const Text('Delegation'),
          subtitle: const Text('Delegate this task'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (_) => TaskDelegationScreen(
                  taskId: task.id,
                  taskTitle: task.title,
                ),
              ),
            );
          },
        ),
        const Divider(),
        ListTile(
          leading: const Icon(Icons.attach_file),
          title: const Text('Attachments'),
          subtitle: const Text('Manage task attachments'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {
            // Navigate to attachments screen
          },
        ),
        const Divider(),
        ListTile(
          leading: const Icon(Icons.history),
          title: const Text('Activity History'),
          subtitle: const Text('View all changes'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () {
            // Navigate to history screen
          },
        ),
      ],
    );
  }
}
