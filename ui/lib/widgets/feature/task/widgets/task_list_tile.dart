import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../core/utils/extensions.dart';
import '../../../../data/models/task_dto.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_chip.dart';

class TaskListTile extends StatelessWidget {
  final TaskDto task;
  final VoidCallback? onTap;
  final VoidCallback? onDelete;
  final VoidCallback? onEdit;

  const TaskListTile({
    super.key,
    required this.task,
    this.onTap,
    this.onDelete,
    this.onEdit,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.xs,
      ),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  _buildPriorityIndicator(),
                  const SizedBox(width: AppSpacing.sm),
                  Expanded(
                    child: AppText(
                      task.title,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                  PopupMenuButton<String>(
                    onSelected: (value) {
                      if (value == 'edit') {
                        onEdit?.call();
                      } else if (value == 'delete') {
                        onDelete?.call();
                      }
                    },
                    itemBuilder: (context) => [
                      const PopupMenuItem(
                        value: 'edit',
                        child: Row(
                          children: [
                            Icon(Icons.edit, size: 20),
                            SizedBox(width: 8),
                            Text('Edit'),
                          ],
                        ),
                      ),
                      const PopupMenuItem(
                        value: 'delete',
                        child: Row(
                          children: [
                            Icon(Icons.delete, size: 20, color: AppColors.error),
                            SizedBox(width: 8),
                            Text('Delete', style: TextStyle(color: AppColors.error)),
                          ],
                        ),
                      ),
                    ],
                  ),
                ],
              ),
              if (task.description != null && task.description!.isNotEmpty) ...[
                const SizedBox(height: AppSpacing.xs),
                AppText(
                  task.description!,
                  variant: TextVariant.bodySmall,
                  color: AppColors.textSecondary,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
              const SizedBox(height: AppSpacing.sm),
              Row(
                children: [
                  _buildStatusChip(),
                  const SizedBox(width: AppSpacing.xs),
                  if (task.dueDate != null) ...[
                    _buildDueDateChip(),
                    const SizedBox(width: AppSpacing.xs),
                  ],
                  if (task.storyPoints != null) ...[
                    AppChip(
                      label: '${task.storyPoints} SP',
                      icon: Icons.star_outline,
                      backgroundColor: AppColors.surfaceVariant,
                    ),
                  ],
                ],
              ),
              if (task.tags.isNotEmpty) ...[
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.xs,
                  runSpacing: AppSpacing.xs,
                  children: task.tags.take(3).map((tag) {
                    return AppChip(
                      label: tag,
                      backgroundColor: AppColors.surfaceVariant,
                    );
                  }).toList(),
                ),
              ],
              if (task.progress > 0) ...[
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    Expanded(
                      child: LinearProgressIndicator(
                        value: task.progress / 100,
                        backgroundColor: AppColors.border,
                        valueColor: AlwaysStoppedAnimation<Color>(
                          _getStatusColor(),
                        ),
                      ),
                    ),
                    const SizedBox(width: AppSpacing.sm),
                    AppText(
                      '${task.progress.toInt()}%',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildPriorityIndicator() {
    return Container(
      width: 4,
      height: 24,
      decoration: BoxDecoration(
        color: _getPriorityColor(),
        borderRadius: BorderRadius.circular(2),
      ),
    );
  }

  Widget _buildStatusChip() {
    return AppChip(
      label: task.status.displayName,
      backgroundColor: _getStatusColor().withOpacity(0.1),
      textColor: _getStatusColor(),
    );
  }

  Widget _buildDueDateChip() {
    final isOverdue = task.dueDate!.isPast && task.status != TaskStatus.done;
    return AppChip(
      label: task.dueDate!.toShortDateString(),
      icon: Icons.calendar_today,
      backgroundColor: isOverdue
          ? AppColors.error.withOpacity(0.1)
          : AppColors.surfaceVariant,
      textColor: isOverdue ? AppColors.error : AppColors.textSecondary,
    );
  }

  Color _getPriorityColor() {
    switch (task.priority) {
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

  Color _getStatusColor() {
    switch (task.status) {
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
}
