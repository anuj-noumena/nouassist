import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/task_dto.dart';
import '../../../providers/task_list_provider.dart';
import '../../../providers/task_delete_provider.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../composite/app_empty_state.dart';
import '../../composite/app_pagination_bar.dart';
import '../../composite/app_search_bar.dart';
import '../../composite/app_filter_chip_group.dart';
import '../../composite/app_sort_menu.dart';
import 'widgets/task_list_tile.dart';
import 'task_form_screen.dart';
import 'task_details_screen.dart';

class TaskListView extends ConsumerStatefulWidget {
  const TaskListView({super.key});

  @override
  ConsumerState<TaskListView> createState() => _TaskListViewState();
}

class _TaskListViewState extends ConsumerState<TaskListView> {
  bool _showFilters = false;

  @override
  Widget build(BuildContext context) {
    final tasksAsync = ref.watch(taskListProvider);
    final taskListNotifier = ref.read(taskListProvider.notifier);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Tasks'),
        actions: [
          IconButton(
            icon: Icon(_showFilters ? Icons.filter_alt : Icons.filter_alt_outlined),
            onPressed: () {
              setState(() {
                _showFilters = !_showFilters;
              });
            },
          ),
          AppSortMenu(
            options: const [
              SortOption(label: 'Title', field: 'title'),
              SortOption(label: 'Priority', field: 'priority'),
              SortOption(label: 'Status', field: 'status'),
              SortOption(label: 'Due Date', field: 'duedate'),
              SortOption(label: 'Created', field: 'creationtime'),
            ],
            currentSortField: null,
            currentSortAscending: true,
            onSortChanged: (option) {
              taskListNotifier.sort(option.field, option.ascending);
            },
          ),
          IconButton(
            icon: const Icon(Icons.add),
            onPressed: () => _navigateToCreateTask(context),
          ),
        ],
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: AppSearchBar(
              hint: 'Search tasks...',
              onChanged: (query) {
                taskListNotifier.search(query.isEmpty ? null : query);
              },
            ),
          ),
          if (_showFilters) _buildFilters(taskListNotifier),
          if (taskListNotifier.hasFilters) _buildActiveFiltersBar(taskListNotifier),
          Expanded(
            child: tasksAsync.when(
              data: (pagedResult) {
                if (pagedResult.items.isEmpty) {
                  return AppEmptyState(
                    message: taskListNotifier.hasFilters
                        ? 'No tasks match your filters'
                        : 'No tasks found',
                    description: taskListNotifier.hasFilters
                        ? 'Try adjusting your filters'
                        : 'Create your first task to get started',
                    icon: Icons.task_outlined,
                    action: AppButton(
                      label: taskListNotifier.hasFilters ? 'Clear Filters' : 'Create Task',
                      onPressed: () {
                        if (taskListNotifier.hasFilters) {
                          taskListNotifier.clearFilters();
                        } else {
                          _navigateToCreateTask(context);
                        }
                      },
                      isFullWidth: false,
                    ),
                  );
                }

                return RefreshIndicator(
                  onRefresh: () => taskListNotifier.refresh(),
                  child: ListView.builder(
                    itemCount: pagedResult.items.length,
                    padding: const EdgeInsets.only(bottom: AppSpacing.md),
                    itemBuilder: (context, index) {
                      final task = pagedResult.items[index];
                      return TaskListTile(
                        task: task,
                        onTap: () => _navigateToTaskDetails(context, task.id),
                        onEdit: () => _navigateToEditTask(context, task.id),
                        onDelete: () => _confirmDelete(context, ref, task.id, task.title),
                      );
                    },
                  ),
                );
              },
              loading: () => const AppLoadingIndicator(message: 'Loading tasks...'),
              error: (error, stack) => AppErrorWidget(
                error: error.toString(),
                onRetry: () => ref.invalidate(taskListProvider),
              ),
            ),
          ),
          if (tasksAsync.hasValue && tasksAsync.value!.items.isNotEmpty)
            AppPaginationBar(
              currentPage: taskListNotifier.currentPage,
              totalPages: taskListNotifier.getTotalPages(tasksAsync.value?.totalCount),
              onPreviousPage: () => taskListNotifier.previousPage(),
              onNextPage: () => taskListNotifier.nextPage(),
            ),
        ],
      ),
    );
  }

  Widget _buildFilters(TaskList taskListNotifier) {
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
          AppFilterChipGroup<TaskStatus>(
            chips: TaskStatus.values.map((status) {
              return FilterChipData<TaskStatus>(
                label: status.displayName,
                value: status,
                selectedColor: _getStatusColor(status),
                selectedTextColor: Colors.white,
              );
            }).toList(),
            selectedValue: null, // You can track selected status in state
            onSelected: (status) {
              taskListNotifier.filterByStatus(status);
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
          AppFilterChipGroup<TaskPriority>(
            chips: TaskPriority.values.map((priority) {
              return FilterChipData<TaskPriority>(
                label: priority.displayName,
                value: priority,
                selectedColor: _getPriorityColor(priority),
                selectedTextColor: Colors.white,
              );
            }).toList(),
            selectedValue: null, // You can track selected priority in state
            onSelected: (priority) {
              taskListNotifier.filterByPriority(priority);
            },
          ),
        ],
      ),
    );
  }

  Widget _buildActiveFiltersBar(TaskList taskListNotifier) {
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
            onPressed: () => taskListNotifier.clearFilters(),
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

  void _navigateToCreateTask(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => const TaskFormScreen(),
      ),
    );
  }

  void _navigateToEditTask(BuildContext context, String taskId) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => TaskFormScreen(taskId: taskId),
      ),
    );
  }

  void _navigateToTaskDetails(BuildContext context, String taskId) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => TaskDetailsScreen(taskId: taskId),
      ),
    );
  }

  Future<void> _confirmDelete(
    BuildContext context,
    WidgetRef ref,
    String taskId,
    String taskTitle,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Task'),
        content: Text('Are you sure you want to delete "$taskTitle"?'),
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
}
