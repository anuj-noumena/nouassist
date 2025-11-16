import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../core/utils/extensions.dart';
import '../../../data/models/project_dto.dart';
import '../../../providers/project_provider.dart';
import '../../../providers/project_delete_provider.dart';
import '../../../providers/project_workflow_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import 'project_form_screen.dart';
import 'widgets/project_card.dart';

class ProjectDetailsScreen extends ConsumerWidget {
  final String projectId;

  const ProjectDetailsScreen({super.key, required this.projectId});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final projectAsync = ref.watch(projectProvider(projectId));
    final childProjectsAsync = ref.watch(childProjectsProvider(projectId));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Project Details'),
        actions: [
          IconButton(
            icon: const Icon(Icons.edit),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => ProjectFormScreen(projectId: projectId),
                ),
              );
            },
          ),
          IconButton(
            icon: const Icon(Icons.delete),
            onPressed: () => _confirmDelete(context, ref),
          ),
        ],
      ),
      body: projectAsync.when(
        data: (project) => _buildProjectDetails(context, ref, project, childProjectsAsync),
        loading: () => const AppLoadingIndicator(message: 'Loading project...'),
        error: (error, stack) => AppErrorWidget(
          error: error.toString(),
          onRetry: () => ref.invalidate(projectProvider(projectId)),
        ),
      ),
    );
  }

  Widget _buildProjectDetails(
    BuildContext context,
    WidgetRef ref,
    ProjectDto project,
    AsyncValue<List<ProjectDto>> childProjectsAsync,
  ) {
    final workflowAsync = ref.watch(projectWorkflowProvider(project.workflowId));

    return RefreshIndicator(
      onRefresh: () async {
        ref.invalidate(projectProvider(projectId));
        ref.invalidate(childProjectsProvider(projectId));
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildHeader(project),
            const SizedBox(height: AppSpacing.lg),
            _buildStatusCards(project),
            const SizedBox(height: AppSpacing.lg),
            _buildInfoSection('Description', project.description ?? 'No description provided'),
            const SizedBox(height: AppSpacing.lg),
            _buildDetailsGrid(project),
            const SizedBox(height: AppSpacing.lg),
            _buildProgressSection(project),
            const SizedBox(height: AppSpacing.lg),
            if (project.budget != null) _buildBudgetSection(project),
            const SizedBox(height: AppSpacing.lg),
            _buildTeamSection(project),
            const SizedBox(height: AppSpacing.lg),
            _buildWorkflowSection(workflowAsync),
            const SizedBox(height: AppSpacing.lg),
            _buildChildProjectsSection(context, childProjectsAsync),
            const SizedBox(height: AppSpacing.lg),
            _buildMetadataSection(project),
          ],
        ),
      ),
    );
  }

  Widget _buildHeader(ProjectDto project) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Container(
              width: 6,
              height: 48,
              decoration: BoxDecoration(
                color: Color(int.parse(project.color.replaceFirst('#', '0xFF'))),
                borderRadius: BorderRadius.circular(3),
              ),
            ),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: AppText(
                project.name,
                variant: TextVariant.h2,
              ),
            ),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        Row(
          children: [
            _buildStatusBadge(project.status),
            const SizedBox(width: AppSpacing.sm),
            _buildPriorityBadge(project.priority),
            const SizedBox(width: AppSpacing.sm),
            _buildViewTypeBadge(project.viewType),
          ],
        ),
      ],
    );
  }

  Widget _buildStatusCards(ProjectDto project) {
    return Row(
      children: [
        Expanded(
          child: _buildStatCard(
            'Progress',
            '${project.progress.toInt()}%',
            Icons.trending_up,
            _getProgressColor(project.progress),
          ),
        ),
        const SizedBox(width: AppSpacing.md),
        Expanded(
          child: _buildStatCard(
            'Team',
            '${project.teamMembers.length}',
            Icons.people,
            AppColors.info,
          ),
        ),
        const SizedBox(width: AppSpacing.md),
        Expanded(
          child: _buildStatCard(
            'Days Left',
            _getDaysLeft(project.endDate).toString(),
            Icons.calendar_today,
            project.endDate.isPast ? AppColors.error : AppColors.success,
          ),
        ),
      ],
    );
  }

  Widget _buildStatCard(String label, String value, IconData icon, Color color) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        border: Border.all(color: AppColors.border),
      ),
      child: Column(
        children: [
          Icon(icon, size: 32, color: color),
          const SizedBox(height: AppSpacing.sm),
          AppText(
            value,
            variant: TextVariant.h3,
            color: color,
          ),
          AppText(
            label,
            variant: TextVariant.bodySmall,
            color: AppColors.textSecondary,
          ),
        ],
      ),
    );
  }

  Widget _buildInfoSection(String title, String content) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          title,
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.sm),
        AppText(
          content,
          variant: TextVariant.bodyMedium,
          color: AppColors.textSecondary,
        ),
      ],
    );
  }

  Widget _buildDetailsGrid(ProjectDto project) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          'Project Details',
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.md),
        _buildDetailRow('Start Date', project.startDate.toShortDateString()),
        _buildDetailRow('End Date', project.endDate.toShortDateString()),
        _buildDetailRow(
          'Duration',
          '${project.endDate.difference(project.startDate).inDays} days',
        ),
        _buildDetailRow('Owner ID', project.ownerId),
        _buildDetailRow('Workflow ID', project.workflowId),
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
            width: 120,
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

  Widget _buildProgressSection(ProjectDto project) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Overall Progress',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            AppText(
              '${project.progress.toInt()}%',
              variant: TextVariant.h4,
              fontWeight: FontWeight.w600,
              color: _getProgressColor(project.progress),
            ),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        ClipRRect(
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          child: LinearProgressIndicator(
            value: project.progress / 100,
            minHeight: 12,
            backgroundColor: AppColors.border,
            valueColor: AlwaysStoppedAnimation<Color>(
              _getProgressColor(project.progress),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildBudgetSection(ProjectDto project) {
    final budgetUsed = project.actualCost ?? 0;
    final budgetRemaining = (project.budget ?? 0) - budgetUsed;
    final percentUsed = project.budget != null && project.budget! > 0
        ? (budgetUsed / project.budget!) * 100
        : 0.0;

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
            'Budget',
            variant: TextVariant.h5,
            fontWeight: FontWeight.w600,
          ),
          const SizedBox(height: AppSpacing.md),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  AppText(
                    'Total Budget',
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.xs),
                  AppText(
                    '\$${project.budget!.toStringAsFixed(0)}',
                    variant: TextVariant.h4,
                    fontWeight: FontWeight.w600,
                  ),
                ],
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  AppText(
                    'Spent',
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.xs),
                  AppText(
                    '\$${budgetUsed.toStringAsFixed(0)}',
                    variant: TextVariant.h4,
                    fontWeight: FontWeight.w600,
                    color: percentUsed > 100 ? AppColors.error : AppColors.success,
                  ),
                ],
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  AppText(
                    'Remaining',
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.xs),
                  AppText(
                    '\$${budgetRemaining.toStringAsFixed(0)}',
                    variant: TextVariant.h4,
                    fontWeight: FontWeight.w600,
                    color: budgetRemaining < 0 ? AppColors.error : AppColors.info,
                  ),
                ],
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.md),
          ClipRRect(
            borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            child: LinearProgressIndicator(
              value: (percentUsed / 100).clamp(0.0, 1.0),
              minHeight: 8,
              backgroundColor: AppColors.border,
              valueColor: AlwaysStoppedAnimation<Color>(
                percentUsed > 100 ? AppColors.error : AppColors.success,
              ),
            ),
          ),
          const SizedBox(height: AppSpacing.xs),
          AppText(
            '${percentUsed.toStringAsFixed(1)}% of budget used',
            variant: TextVariant.bodySmall,
            color: AppColors.textSecondary,
          ),
        ],
      ),
    );
  }

  Widget _buildTeamSection(ProjectDto project) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Team Members',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            AppText(
              '${project.teamMembers.length} members',
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        if (project.teamMembers.isEmpty)
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.surfaceVariant,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Center(
              child: AppText(
                'No team members assigned',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ),
          )
        else
          Wrap(
            spacing: AppSpacing.sm,
            runSpacing: AppSpacing.sm,
            children: project.teamMembers.map((userId) {
              return Container(
                padding: const EdgeInsets.symmetric(
                  horizontal: AppSpacing.md,
                  vertical: AppSpacing.sm,
                ),
                decoration: BoxDecoration(
                  color: AppColors.primary.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    CircleAvatar(
                      radius: 12,
                      backgroundColor: AppColors.primary,
                      child: Text(
                        userId.substring(userId.length - 1).toUpperCase(),
                        style: const TextStyle(
                          fontSize: 10,
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    Text(
                      'User $userId',
                      style: const TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ],
                ),
              );
            }).toList(),
          ),
      ],
    );
  }

  Widget _buildWorkflowSection(AsyncValue<dynamic> workflowAsync) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          'Workflow',
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.md),
        workflowAsync.when(
          data: (workflow) {
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
                    workflow.name,
                    variant: TextVariant.bodyLarge,
                    fontWeight: FontWeight.w600,
                  ),
                  if (workflow.description != null) ...[
                    const SizedBox(height: AppSpacing.xs),
                    AppText(
                      workflow.description!,
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                  const SizedBox(height: AppSpacing.md),
                  Wrap(
                    spacing: AppSpacing.sm,
                    runSpacing: AppSpacing.sm,
                    children: workflow.stages.map<Widget>((stage) {
                      return Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: AppSpacing.md,
                          vertical: AppSpacing.sm,
                        ),
                        decoration: BoxDecoration(
                          color: Color(int.parse(
                              stage.color.replaceFirst('#', '0xFF'))),
                          borderRadius:
                              BorderRadius.circular(AppSpacing.radiusSm),
                        ),
                        child: Text(
                          stage.name,
                          style: const TextStyle(
                            fontSize: 12,
                            color: Colors.white,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      );
                    }).toList(),
                  ),
                ],
              ),
            );
          },
          loading: () => const CircularProgressIndicator(),
          error: (_, __) => const Text('Failed to load workflow'),
        ),
      ],
    );
  }

  Widget _buildChildProjectsSection(
    BuildContext context,
    AsyncValue<List<ProjectDto>> childProjectsAsync,
  ) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          'Sub-Projects',
          variant: TextVariant.h5,
          fontWeight: FontWeight.w600,
        ),
        const SizedBox(height: AppSpacing.md),
        childProjectsAsync.when(
          data: (children) {
            if (children.isEmpty) {
              return Container(
                padding: const EdgeInsets.all(AppSpacing.lg),
                decoration: BoxDecoration(
                  color: AppColors.surfaceVariant,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                ),
                child: Center(
                  child: AppText(
                    'No sub-projects',
                    variant: TextVariant.bodyMedium,
                    color: AppColors.textSecondary,
                  ),
                ),
              );
            }

            return Column(
              children: children.map((child) {
                return ProjectCard(
                  project: child,
                  onTap: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(
                        builder: (_) => ProjectDetailsScreen(projectId: child.id),
                      ),
                    );
                  },
                );
              }).toList(),
            );
          },
          loading: () => const CircularProgressIndicator(),
          error: (_, __) => const Text('Failed to load sub-projects'),
        ),
      ],
    );
  }

  Widget _buildMetadataSection(ProjectDto project) {
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
          _buildMetadataRow('Created', project.creationTime.toDateTimeString()),
          if (project.lastModificationTime != null)
            _buildMetadataRow(
                'Last Modified', project.lastModificationTime!.toDateTimeString()),
          _buildMetadataRow('Project ID', project.id),
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
          Expanded(
            child: AppText(
              value,
              variant: TextVariant.bodySmall,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatusBadge(ProjectStatus status) {
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

  Widget _buildPriorityBadge(ProjectPriority priority) {
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
          Icon(
            Icons.flag,
            size: 14,
            color: _getPriorityColor(priority),
          ),
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

  Widget _buildViewTypeBadge(ProjectViewType viewType) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: AppColors.info.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
        border: Border.all(color: AppColors.info),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(
            _getViewIcon(viewType),
            size: 14,
            color: AppColors.info,
          ),
          const SizedBox(width: AppSpacing.xs),
          Text(
            viewType.displayName,
            style: const TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: AppColors.info,
            ),
          ),
        ],
      ),
    );
  }

  IconData _getViewIcon(ProjectViewType viewType) {
    switch (viewType) {
      case ProjectViewType.list:
        return Icons.view_list;
      case ProjectViewType.kanban:
        return Icons.view_column;
      case ProjectViewType.gantt:
        return Icons.view_timeline;
    }
  }

  Color _getStatusColor(ProjectStatus status) {
    switch (status) {
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

  Color _getPriorityColor(ProjectPriority priority) {
    switch (priority) {
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

  Color _getProgressColor(double progress) {
    if (progress < 30) {
      return AppColors.error;
    } else if (progress < 70) {
      return AppColors.warning;
    } else {
      return AppColors.success;
    }
  }

  int _getDaysLeft(DateTime endDate) {
    final now = DateTime.now();
    final difference = endDate.difference(now);
    return difference.inDays;
  }

  Future<void> _confirmDelete(BuildContext context, WidgetRef ref) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Project'),
        content: const Text(
            'Are you sure you want to delete this project? This will also delete all sub-projects. This action cannot be undone.'),
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
        await ref.read(deleteProjectProvider(projectId).future);
        if (context.mounted) {
          Navigator.of(context).pop();
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Project deleted successfully'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to delete project: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }
  }
}
