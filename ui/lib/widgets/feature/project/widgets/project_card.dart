import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../core/utils/extensions.dart';
import '../../../../data/models/project_dto.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_chip.dart';

class ProjectCard extends StatelessWidget {
  final ProjectDto project;
  final VoidCallback? onTap;
  final VoidCallback? onEdit;
  final VoidCallback? onDelete;

  const ProjectCard({
    super.key,
    required this.project,
    this.onTap,
    this.onEdit,
    this.onDelete,
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
        child: Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            border: Border(
              left: BorderSide(
                color: Color(int.parse(project.color.replaceFirst('#', '0xFF'))),
                width: 4,
              ),
            ),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          AppText(
                            project.name,
                            variant: TextVariant.h5,
                            fontWeight: FontWeight.w600,
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis,
                          ),
                          if (project.description != null && project.description!.isNotEmpty) ...[
                            const SizedBox(height: AppSpacing.xs),
                            AppText(
                              project.description!,
                              variant: TextVariant.bodySmall,
                              color: AppColors.textSecondary,
                              maxLines: 2,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ],
                        ],
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
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    _buildStatusChip(),
                    const SizedBox(width: AppSpacing.xs),
                    _buildPriorityChip(),
                  ],
                ),
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    Icon(
                      Icons.calendar_today,
                      size: 14,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      '${project.startDate.toShortDateString()} - ${project.endDate.toShortDateString()}',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                    const Spacer(),
                    Icon(
                      Icons.people_outline,
                      size: 14,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      '${project.teamMembers.length} members',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              AppText(
                                'Progress',
                                variant: TextVariant.bodySmall,
                                color: AppColors.textSecondary,
                              ),
                              AppText(
                                '${project.progress.toInt()}%',
                                variant: TextVariant.bodySmall,
                                fontWeight: FontWeight.w600,
                                color: _getProgressColor(),
                              ),
                            ],
                          ),
                          const SizedBox(height: AppSpacing.xs),
                          ClipRRect(
                            borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                            child: LinearProgressIndicator(
                              value: project.progress / 100,
                              minHeight: 6,
                              backgroundColor: AppColors.border,
                              valueColor: AlwaysStoppedAnimation<Color>(
                                _getProgressColor(),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
                if (project.budget != null) ...[
                  const SizedBox(height: AppSpacing.sm),
                  Row(
                    children: [
                      Icon(
                        Icons.attach_money,
                        size: 14,
                        color: AppColors.textSecondary,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        'Budget: \$${project.budget!.toStringAsFixed(0)}',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                      if (project.actualCost != null) ...[
                        const SizedBox(width: AppSpacing.md),
                        AppText(
                          'Spent: \$${project.actualCost!.toStringAsFixed(0)}',
                          variant: TextVariant.bodySmall,
                          color: project.actualCost! > project.budget!
                              ? AppColors.error
                              : AppColors.success,
                          fontWeight: FontWeight.w500,
                        ),
                      ],
                    ],
                  ),
                ],
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildStatusChip() {
    return AppChip(
      label: project.status.displayName,
      backgroundColor: _getStatusColor().withOpacity(0.1),
      textColor: _getStatusColor(),
    );
  }

  Widget _buildPriorityChip() {
    return AppChip(
      label: project.priority.displayName,
      icon: Icons.flag,
      backgroundColor: _getPriorityColor().withOpacity(0.1),
      textColor: _getPriorityColor(),
    );
  }

  Color _getStatusColor() {
    switch (project.status) {
      case ProjectStatus.planning:
        return AppColors.info;
      case ProjectStatus.active:
        return AppColors.success;
      case ProjectStatus.onHold:
        return AppColors.warning;
      case ProjectStatus.completed:
        return AppColors.success;
      case ProjectStatus.cancelled:
        return AppColors.error;
    }
  }

  Color _getPriorityColor() {
    switch (project.priority) {
      case ProjectPriority.low:
        return AppColors.priorityLow;
      case ProjectPriority.medium:
        return AppColors.priorityMedium;
      case ProjectPriority.high:
        return AppColors.priorityHigh;
      case ProjectPriority.critical:
        return AppColors.priorityHighest;
    }
  }

  Color _getProgressColor() {
    if (project.progress < 30) {
      return AppColors.error;
    } else if (project.progress < 70) {
      return AppColors.warning;
    } else {
      return AppColors.success;
    }
  }
}
