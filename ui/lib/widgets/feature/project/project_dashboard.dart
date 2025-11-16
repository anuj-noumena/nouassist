import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/project_dto.dart';
import '../../../providers/project_list_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import 'widgets/project_stats_card.dart';
import 'project_list_view.dart';
import 'project_kanban_board.dart';

class ProjectDashboard extends ConsumerWidget {
  const ProjectDashboard({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final projectsAsync = ref.watch(projectListProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Project Dashboard'),
        actions: [
          IconButton(
            icon: const Icon(Icons.view_column),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => const ProjectKanbanBoard(),
                ),
              );
            },
            tooltip: 'Kanban Board',
          ),
          IconButton(
            icon: const Icon(Icons.list),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => const ProjectListView(),
                ),
              );
            },
            tooltip: 'Project List',
          ),
        ],
      ),
      body: projectsAsync.when(
        data: (pagedResult) => _buildDashboard(context, pagedResult.items),
        loading: () => const AppLoadingIndicator(message: 'Loading dashboard...'),
        error: (error, stack) => AppErrorWidget(
          error: error.toString(),
          onRetry: () => ref.invalidate(projectListProvider),
        ),
      ),
    );
  }

  Widget _buildDashboard(BuildContext context, List<ProjectDto> projects) {
    final stats = _calculateStats(projects);

    return RefreshIndicator(
      onRefresh: () async {
        // Refresh logic
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              'Overview',
              variant: TextVariant.h3,
              fontWeight: FontWeight.bold,
            ),
            const SizedBox(height: AppSpacing.lg),
            _buildStatsGrid(stats),
            const SizedBox(height: AppSpacing.xl),
            AppText(
              'Projects by Status',
              variant: TextVariant.h4,
              fontWeight: FontWeight.w600,
            ),
            const SizedBox(height: AppSpacing.md),
            _buildStatusBreakdown(stats),
            const SizedBox(height: AppSpacing.xl),
            AppText(
              'Projects by Priority',
              variant: TextVariant.h4,
              fontWeight: FontWeight.w600,
            ),
            const SizedBox(height: AppSpacing.md),
            _buildPriorityBreakdown(stats),
            const SizedBox(height: AppSpacing.xl),
            AppText(
              'Budget Overview',
              variant: TextVariant.h4,
              fontWeight: FontWeight.w600,
            ),
            const SizedBox(height: AppSpacing.md),
            _buildBudgetOverview(stats),
            const SizedBox(height: AppSpacing.xl),
            AppText(
              'Recent Activity',
              variant: TextVariant.h4,
              fontWeight: FontWeight.w600,
            ),
            const SizedBox(height: AppSpacing.md),
            _buildRecentActivity(projects),
          ],
        ),
      ),
    );
  }

  Widget _buildStatsGrid(DashboardStats stats) {
    return GridView.count(
      crossAxisCount: 2,
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      mainAxisSpacing: AppSpacing.md,
      crossAxisSpacing: AppSpacing.md,
      childAspectRatio: 1.5,
      children: [
        ProjectStatsCard(
          title: 'Total Projects',
          value: stats.totalProjects.toString(),
          icon: Icons.folder,
          color: AppColors.primary,
        ),
        ProjectStatsCard(
          title: 'Active Projects',
          value: stats.activeProjects.toString(),
          icon: Icons.play_circle,
          color: AppColors.success,
          subtitle: '${((stats.activeProjects / stats.totalProjects) * 100).toStringAsFixed(0)}% of total',
        ),
        ProjectStatsCard(
          title: 'Avg. Progress',
          value: '${stats.averageProgress.toStringAsFixed(0)}%',
          icon: Icons.trending_up,
          color: AppColors.info,
        ),
        ProjectStatsCard(
          title: 'On Hold',
          value: stats.onHoldProjects.toString(),
          icon: Icons.pause_circle,
          color: AppColors.warning,
        ),
      ],
    );
  }

  Widget _buildStatusBreakdown(DashboardStats stats) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          children: [
            _buildStatusRow(
              'Planning',
              stats.planningProjects,
              stats.totalProjects,
              AppColors.info,
            ),
            _buildStatusRow(
              'Active',
              stats.activeProjects,
              stats.totalProjects,
              AppColors.success,
            ),
            _buildStatusRow(
              'On Hold',
              stats.onHoldProjects,
              stats.totalProjects,
              AppColors.warning,
            ),
            _buildStatusRow(
              'Completed',
              stats.completedProjects,
              stats.totalProjects,
              AppColors.success,
            ),
            _buildStatusRow(
              'Cancelled',
              stats.cancelledProjects,
              stats.totalProjects,
              AppColors.error,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusRow(String label, int count, int total, Color color) {
    final percentage = total > 0 ? (count / total) * 100 : 0.0;

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: AppSpacing.sm),
      child: Column(
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Row(
                children: [
                  Container(
                    width: 12,
                    height: 12,
                    decoration: BoxDecoration(
                      color: color,
                      shape: BoxShape.circle,
                    ),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  AppText(
                    label,
                    variant: TextVariant.bodyMedium,
                  ),
                ],
              ),
              AppText(
                '$count (${percentage.toStringAsFixed(0)}%)',
                variant: TextVariant.bodyMedium,
                fontWeight: FontWeight.w600,
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.xs),
          ClipRRect(
            borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            child: LinearProgressIndicator(
              value: percentage / 100,
              minHeight: 6,
              backgroundColor: AppColors.border,
              valueColor: AlwaysStoppedAnimation<Color>(color),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPriorityBreakdown(DashboardStats stats) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          children: [
            _buildPriorityRow(
              'Critical',
              stats.criticalProjects,
              AppColors.priorityHighest,
            ),
            _buildPriorityRow(
              'High',
              stats.highProjects,
              AppColors.priorityHigh,
            ),
            _buildPriorityRow(
              'Medium',
              stats.mediumProjects,
              AppColors.priorityMedium,
            ),
            _buildPriorityRow(
              'Low',
              stats.lowProjects,
              AppColors.priorityLow,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPriorityRow(String label, int count, Color color) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: AppSpacing.sm),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Row(
            children: [
              Icon(Icons.flag, size: 16, color: color),
              const SizedBox(width: AppSpacing.sm),
              AppText(
                label,
                variant: TextVariant.bodyMedium,
              ),
            ],
          ),
          Container(
            padding: const EdgeInsets.symmetric(
              horizontal: AppSpacing.md,
              vertical: AppSpacing.xs,
            ),
            decoration: BoxDecoration(
              color: color.withOpacity(0.1),
              borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            ),
            child: Text(
              count.toString(),
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildBudgetOverview(DashboardStats stats) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
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
                      '\$${stats.totalBudget.toStringAsFixed(0)}',
                      variant: TextVariant.h3,
                      fontWeight: FontWeight.bold,
                    ),
                  ],
                ),
                Column(
                  crossAxisAlignment: CrossAxisAlignment.end,
                  children: [
                    AppText(
                      'Total Spent',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(height: AppSpacing.xs),
                    AppText(
                      '\$${stats.totalSpent.toStringAsFixed(0)}',
                      variant: TextVariant.h3,
                      fontWeight: FontWeight.bold,
                      color: stats.totalSpent > stats.totalBudget
                          ? AppColors.error
                          : AppColors.success,
                    ),
                  ],
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            ClipRRect(
              borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
              child: LinearProgressIndicator(
                value: stats.totalBudget > 0
                    ? (stats.totalSpent / stats.totalBudget).clamp(0.0, 1.0)
                    : 0.0,
                minHeight: 8,
                backgroundColor: AppColors.border,
                valueColor: AlwaysStoppedAnimation<Color>(
                  stats.totalSpent > stats.totalBudget
                      ? AppColors.error
                      : AppColors.success,
                ),
              ),
            ),
            const SizedBox(height: AppSpacing.sm),
            AppText(
              stats.totalBudget > 0
                  ? '${((stats.totalSpent / stats.totalBudget) * 100).toStringAsFixed(1)}% of budget used'
                  : 'No budget set',
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildRecentActivity(List<ProjectDto> projects) {
    final recentProjects = List<ProjectDto>.from(projects)
      ..sort((a, b) => b.creationTime.compareTo(a.creationTime))
      ..take(5);

    return Card(
      child: ListView.separated(
        shrinkWrap: true,
        physics: const NeverScrollableScrollPhysics(),
        itemCount: recentProjects.length,
        separatorBuilder: (_, __) => const Divider(height: 1),
        itemBuilder: (context, index) {
          final project = recentProjects.elementAt(index);
          return ListTile(
            leading: Container(
              width: 4,
              height: 40,
              decoration: BoxDecoration(
                color: Color(int.parse(project.color.replaceFirst('#', '0xFF'))),
                borderRadius: BorderRadius.circular(2),
              ),
            ),
            title: AppText(
              project.name,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w600,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
            subtitle: AppText(
              project.status.displayName,
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
            trailing: AppText(
              '${project.progress.toInt()}%',
              variant: TextVariant.bodySmall,
              fontWeight: FontWeight.w600,
            ),
          );
        },
      ),
    );
  }

  DashboardStats _calculateStats(List<ProjectDto> projects) {
    return DashboardStats(
      totalProjects: projects.length,
      planningProjects:
          projects.where((p) => p.status == ProjectStatus.planning).length,
      activeProjects:
          projects.where((p) => p.status == ProjectStatus.active).length,
      onHoldProjects:
          projects.where((p) => p.status == ProjectStatus.onHold).length,
      completedProjects:
          projects.where((p) => p.status == ProjectStatus.completed).length,
      cancelledProjects:
          projects.where((p) => p.status == ProjectStatus.cancelled).length,
      criticalProjects:
          projects.where((p) => p.priority == ProjectPriority.critical).length,
      highProjects:
          projects.where((p) => p.priority == ProjectPriority.high).length,
      mediumProjects:
          projects.where((p) => p.priority == ProjectPriority.medium).length,
      lowProjects:
          projects.where((p) => p.priority == ProjectPriority.low).length,
      averageProgress: projects.isEmpty
          ? 0
          : projects.map((p) => p.progress).reduce((a, b) => a + b) /
              projects.length,
      totalBudget:
          projects.fold(0.0, (sum, p) => sum + (p.budget ?? 0)),
      totalSpent:
          projects.fold(0.0, (sum, p) => sum + (p.actualCost ?? 0)),
    );
  }
}

class DashboardStats {
  final int totalProjects;
  final int planningProjects;
  final int activeProjects;
  final int onHoldProjects;
  final int completedProjects;
  final int cancelledProjects;
  final int criticalProjects;
  final int highProjects;
  final int mediumProjects;
  final int lowProjects;
  final double averageProgress;
  final double totalBudget;
  final double totalSpent;

  DashboardStats({
    required this.totalProjects,
    required this.planningProjects,
    required this.activeProjects,
    required this.onHoldProjects,
    required this.completedProjects,
    required this.cancelledProjects,
    required this.criticalProjects,
    required this.highProjects,
    required this.mediumProjects,
    required this.lowProjects,
    required this.averageProgress,
    required this.totalBudget,
    required this.totalSpent,
  });
}
