import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/project_dto.dart';
import '../../../providers/project_list_provider.dart';
import '../../../providers/project_delete_provider.dart';
import '../../../providers/project_provider.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../composite/app_empty_state.dart';
import '../../composite/app_pagination_bar.dart';
import '../../composite/app_search_bar.dart';
import '../../composite/app_filter_chip_group.dart';
import '../../composite/app_sort_menu.dart';
import 'widgets/project_card.dart';
import 'widgets/project_tree_node.dart';
import 'project_form_screen.dart';
import 'project_details_screen.dart';
import 'project_dashboard.dart';

class ProjectListView extends ConsumerStatefulWidget {
  const ProjectListView({super.key});

  @override
  ConsumerState<ProjectListView> createState() => _ProjectListViewState();
}

class _ProjectListViewState extends ConsumerState<ProjectListView> {
  bool _showFilters = false;
  bool _treeView = false;

  @override
  Widget build(BuildContext context) {
    final projectsAsync = ref.watch(projectListProvider);
    final projectListNotifier = ref.read(projectListProvider.notifier);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Projects'),
        actions: [
          IconButton(
            icon: const Icon(Icons.dashboard_outlined),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => const ProjectDashboard(),
                ),
              );
            },
            tooltip: 'Dashboard',
          ),
          IconButton(
            icon: Icon(_treeView ? Icons.view_list : Icons.account_tree),
            onPressed: () {
              setState(() {
                _treeView = !_treeView;
              });
            },
            tooltip: _treeView ? 'List View' : 'Tree View',
          ),
          IconButton(
            icon: Icon(
                _showFilters ? Icons.filter_alt : Icons.filter_alt_outlined),
            onPressed: () {
              setState(() {
                _showFilters = !_showFilters;
              });
            },
          ),
          AppSortMenu(
            options: const [
              SortOption(label: 'Name', field: 'name'),
              SortOption(label: 'Priority', field: 'priority'),
              SortOption(label: 'Status', field: 'status'),
              SortOption(label: 'Start Date', field: 'startdate'),
              SortOption(label: 'Progress', field: 'progress'),
            ],
            currentSortField: null,
            currentSortAscending: true,
            onSortChanged: (option) {
              projectListNotifier.sort(option.field, option.ascending);
            },
          ),
          IconButton(
            icon: const Icon(Icons.add),
            onPressed: () => _navigateToCreateProject(context),
          ),
        ],
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: AppSearchBar(
              hint: 'Search projects...',
              onChanged: (query) {
                projectListNotifier.search(query.isEmpty ? null : query);
              },
            ),
          ),
          if (_showFilters) _buildFilters(projectListNotifier),
          if (projectListNotifier.hasFilters)
            _buildActiveFiltersBar(projectListNotifier),
          Expanded(
            child: projectsAsync.when(
              data: (pagedResult) {
                if (pagedResult.items.isEmpty) {
                  return AppEmptyState(
                    message: projectListNotifier.hasFilters
                        ? 'No projects match your filters'
                        : 'No projects found',
                    description: projectListNotifier.hasFilters
                        ? 'Try adjusting your filters'
                        : 'Create your first project to get started',
                    icon: Icons.folder_outlined,
                    action: AppButton(
                      label: projectListNotifier.hasFilters
                          ? 'Clear Filters'
                          : 'Create Project',
                      onPressed: () {
                        if (projectListNotifier.hasFilters) {
                          projectListNotifier.clearFilters();
                        } else {
                          _navigateToCreateProject(context);
                        }
                      },
                      isFullWidth: false,
                    ),
                  );
                }

                return RefreshIndicator(
                  onRefresh: () => projectListNotifier.refresh(),
                  child: _treeView
                      ? _buildTreeView(pagedResult.items)
                      : _buildListView(pagedResult.items),
                );
              },
              loading: () =>
                  const AppLoadingIndicator(message: 'Loading projects...'),
              error: (error, stack) => AppErrorWidget(
                error: error.toString(),
                onRetry: () => ref.invalidate(projectListProvider),
              ),
            ),
          ),
          if (projectsAsync.hasValue &&
              projectsAsync.value!.items.isNotEmpty &&
              !_treeView)
            AppPaginationBar(
              currentPage: projectListNotifier.currentPage,
              totalPages: projectListNotifier
                  .getTotalPages(projectsAsync.value?.totalCount),
              onPreviousPage: () => projectListNotifier.previousPage(),
              onNextPage: () => projectListNotifier.nextPage(),
            ),
        ],
      ),
    );
  }

  Widget _buildListView(List<ProjectDto> projects) {
    return ListView.builder(
      itemCount: projects.length,
      padding: const EdgeInsets.only(bottom: AppSpacing.md),
      itemBuilder: (context, index) {
        final project = projects[index];
        return ProjectCard(
          project: project,
          onTap: () => _navigateToProjectDetails(context, project.id),
          onEdit: () => _navigateToEditProject(context, project.id),
          onDelete: () =>
              _confirmDelete(context, ref, project.id, project.name),
        );
      },
    );
  }

  Widget _buildTreeView(List<ProjectDto> projects) {
    final rootProjects =
        projects.where((p) => p.parentProjectId == null).toList();

    return ListView.builder(
      itemCount: rootProjects.length,
      padding: const EdgeInsets.all(AppSpacing.md),
      itemBuilder: (context, index) {
        final project = rootProjects[index];
        final children =
            projects.where((p) => p.parentProjectId == project.id).toList();

        return ProjectTreeNode(
          project: project,
          children: children,
          onTap: () => _navigateToProjectDetails(context, project.id),
          onEdit: () => _navigateToEditProject(context, project.id),
          onDelete: () =>
              _confirmDelete(context, ref, project.id, project.name),
        );
      },
    );
  }

  Widget _buildFilters(ProjectList projectListNotifier) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.sm,
      ),
      decoration: const BoxDecoration(
        color: AppColors.surfaceVariant,
        border: Border(
          bottom: BorderSide(color: AppColors.divider),
        ),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            'Status',
            style: TextStyle(
              fontWeight: FontWeight.w600,
              fontSize: 12,
              color: AppColors.textSecondary,
            ),
          ),
          const SizedBox(height: AppSpacing.xs),
          AppFilterChipGroup<ProjectStatus>(
            chips: ProjectStatus.values.map((status) {
              return FilterChipData<ProjectStatus>(
                label: status.displayName,
                value: status,
                selectedColor: _getStatusColor(status),
                selectedTextColor: Colors.white,
              );
            }).toList(),
            selectedValue: null,
            onSelected: (status) {
              projectListNotifier.filterByStatus(status);
            },
          ),
          const SizedBox(height: AppSpacing.sm),
          const Text(
            'Priority',
            style: TextStyle(
              fontWeight: FontWeight.w600,
              fontSize: 12,
              color: AppColors.textSecondary,
            ),
          ),
          const SizedBox(height: AppSpacing.xs),
          AppFilterChipGroup<ProjectPriority>(
            chips: ProjectPriority.values.map((priority) {
              return FilterChipData<ProjectPriority>(
                label: priority.displayName,
                value: priority,
                selectedColor: _getPriorityColor(priority),
                selectedTextColor: Colors.white,
              );
            }).toList(),
            selectedValue: null,
            onSelected: (priority) {
              projectListNotifier.filterByPriority(priority);
            },
          ),
        ],
      ),
    );
  }

  Widget _buildActiveFiltersBar(ProjectList projectListNotifier) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.md,
        vertical: AppSpacing.sm,
      ),
      decoration: BoxDecoration(
        color: AppColors.info.withOpacity(0.1),
        border: const Border(
          bottom: BorderSide(color: AppColors.divider),
        ),
      ),
      child: Row(
        children: [
          const Icon(Icons.filter_alt, size: 16, color: AppColors.info),
          const SizedBox(width: AppSpacing.xs),
          const Text(
            'Filters active',
            style: TextStyle(
              fontSize: 12,
              color: AppColors.info,
              fontWeight: FontWeight.w500,
            ),
          ),
          const Spacer(),
          TextButton(
            onPressed: () => projectListNotifier.clearFilters(),
            style: TextButton.styleFrom(
              padding: const EdgeInsets.symmetric(horizontal: AppSpacing.sm),
              minimumSize: const Size(0, 32),
            ),
            child: const Text('Clear all'),
          ),
        ],
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

  void _navigateToCreateProject(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) =>  ProjectFormScreen(),
      ),
    );
  }

  void _navigateToEditProject(BuildContext context, String projectId) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => ProjectFormScreen(projectId: projectId),
      ),
    );
  }

  void _navigateToProjectDetails(BuildContext context, String projectId) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => ProjectDetailsScreen(projectId: projectId),
      ),
    );
  }

  Future<void> _confirmDelete(
    BuildContext context,
    WidgetRef ref,
    String projectId,
    String projectName,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Project'),
        content: Text(
            'Are you sure you want to delete "$projectName"? This will also delete all child projects.'),
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
