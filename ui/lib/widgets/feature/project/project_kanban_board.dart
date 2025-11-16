import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/project_dto.dart';
import '../../../providers/project_list_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import 'project_details_screen.dart';

class ProjectKanbanBoard extends ConsumerWidget {
  const ProjectKanbanBoard({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final projectsAsync = ref.watch(projectListProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Project Kanban Board'),
      ),
      body: projectsAsync.when(
        data: (pagedResult) => _buildKanbanBoard(context, pagedResult.items),
        loading: () => const AppLoadingIndicator(message: 'Loading projects...'),
        error: (error, stack) => AppErrorWidget(
          error: error.toString(),
          onRetry: () => ref.invalidate(projectListProvider),
        ),
      ),
    );
  }

  Widget _buildKanbanBoard(BuildContext context, List<ProjectDto> projects) {
    final columns = ProjectStatus.values;

    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: columns.map((status) {
          final statusProjects = projects.where((p) => p.status == status).toList();
          return _buildKanbanColumn(context, status, statusProjects);
        }).toList(),
      ),
    );
  }

  Widget _buildKanbanColumn(
    BuildContext context,
    ProjectStatus status,
    List<ProjectDto> projects,
  ) {
    return Container(
      width: 300,
      margin: const EdgeInsets.all(AppSpacing.md),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            padding: const EdgeInsets.all(AppSpacing.md),
            decoration: BoxDecoration(
              color: _getStatusColor(status).withOpacity(0.1),
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Row(
              children: [
                Container(
                  width: 12,
                  height: 12,
                  decoration: BoxDecoration(
                    color: _getStatusColor(status),
                    shape: BoxShape.circle,
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                AppText(
                  status.displayName,
                  variant: TextVariant.bodyLarge,
                  fontWeight: FontWeight.w600,
                ),
                const Spacer(),
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: AppSpacing.sm,
                    vertical: AppSpacing.xxs,
                  ),
                  decoration: BoxDecoration(
                    color: _getStatusColor(status),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  ),
                  child: Text(
                    projects.length.toString(),
                    style: const TextStyle(
                      fontSize: 12,
                      color: Colors.white,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: AppSpacing.md),
          Expanded(
            child: ListView.builder(
              shrinkWrap: true,
              itemCount: projects.length,
              itemBuilder: (context, index) {
                final project = projects[index];
                return _buildKanbanCard(context, project);
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildKanbanCard(BuildContext context, ProjectDto project) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: InkWell(
        onTap: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (_) => ProjectDetailsScreen(projectId: project.id),
            ),
          );
        },
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
                AppText(
                  project.name,
                  variant: TextVariant.bodyLarge,
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
                const SizedBox(height: AppSpacing.sm),
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: AppSpacing.sm,
                    vertical: AppSpacing.xxs,
                  ),
                  decoration: BoxDecoration(
                    color: _getPriorityColor(project.priority).withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(
                        Icons.flag,
                        size: 12,
                        color: _getPriorityColor(project.priority),
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      Text(
                        project.priority.displayName,
                        style: TextStyle(
                          fontSize: 10,
                          fontWeight: FontWeight.w600,
                          color: _getPriorityColor(project.priority),
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    Icon(
                      Icons.people_outline,
                      size: 14,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      '${project.teamMembers.length}',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                    const Spacer(),
                    AppText(
                      '${project.progress.toInt()}%',
                      variant: TextVariant.bodySmall,
                      fontWeight: FontWeight.w600,
                      color: _getProgressColor(project.progress),
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.xs),
                ClipRRect(
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  child: LinearProgressIndicator(
                    value: project.progress / 100,
                    minHeight: 4,
                    backgroundColor: AppColors.border,
                    valueColor: AlwaysStoppedAnimation<Color>(
                      _getProgressColor(project.progress),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
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
}
