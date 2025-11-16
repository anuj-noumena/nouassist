import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_dto.dart';
import '../data/models/paged_result_dto.dart';
import '../data/mock/mock_task_service.dart';

part 'task_list_provider.g.dart';

@riverpod
class TaskList extends _$TaskList {
  int _skipCount = 0;
  int _maxResultCount = 10;
  String? _sorting;
  String? _filter;
  TaskStatus? _statusFilter;
  TaskPriority? _priorityFilter;
  String? _assigneeFilter;

  @override
  Future<PagedResultDto<TaskDto>> build() async {
    return _loadTasks();
  }

  Future<PagedResultDto<TaskDto>> _loadTasks() async {
    return await MockTaskService.getList(
      skipCount: _skipCount,
      maxResultCount: _maxResultCount,
      sorting: _sorting,
      filter: _filter,
      statusFilter: _statusFilter,
      priorityFilter: _priorityFilter,
      assigneeFilter: _assigneeFilter,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadTasks());
  }

  Future<void> nextPage() async {
    final currentData = state.value;
    if (currentData == null) return;

    if (_skipCount + _maxResultCount < currentData.totalCount) {
      _skipCount += _maxResultCount;
      await refresh();
    }
  }

  Future<void> previousPage() async {
    if (_skipCount > 0) {
      _skipCount =
          (_skipCount - _maxResultCount).clamp(0, double.infinity).toInt();
      await refresh();
    }
  }

  Future<void> search(String? query) async {
    _filter = query;
    _skipCount = 0;
    await refresh();
  }

  Future<void> filterByStatus(TaskStatus? status) async {
    _statusFilter = status;
    _skipCount = 0;
    await refresh();
  }

  Future<void> filterByPriority(TaskPriority? priority) async {
    _priorityFilter = priority;
    _skipCount = 0;
    await refresh();
  }

  Future<void> filterByAssignee(String? assigneeId) async {
    _assigneeFilter = assigneeId;
    _skipCount = 0;
    await refresh();
  }

  Future<void> clearFilters() async {
    _statusFilter = null;
    _priorityFilter = null;
    _assigneeFilter = null;
    _filter = null;
    _skipCount = 0;
    await refresh();
  }

  Future<void> sort(String field, bool ascending) async {
    _sorting = '$field ${ascending ? 'ASC' : 'DESC'}';
    await refresh();
  }

  Future<void> setPageSize(int size) async {
    _maxResultCount = size;
    _skipCount = 0;
    await refresh();
  }

  int get currentPage => (_skipCount / _maxResultCount).floor() + 1;

  int getTotalPages(int? totalCount) {
    if (totalCount == null) return 0;
    return (totalCount / _maxResultCount).ceil();
  }

  bool get hasFilters =>
      _statusFilter != null ||
      _priorityFilter != null ||
      _assigneeFilter != null ||
      (_filter != null && _filter!.isNotEmpty);
}
