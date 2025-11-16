import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/project_dto.dart';
import '../data/models/paged_result_dto.dart';
import '../data/mock/mock_project_service.dart';

part 'project_list_provider.g.dart';

@riverpod
class ProjectList extends _$ProjectList {
  int _skipCount = 0;
  int _maxResultCount = 10;
  String? _sorting;
  String? _filter;
  ProjectStatus? _statusFilter;
  ProjectPriority? _priorityFilter;
  String? _ownerFilter;

  @override
  Future<PagedResultDto<ProjectDto>> build() async {
    return _loadProjects();
  }

  Future<PagedResultDto<ProjectDto>> _loadProjects() async {
    return await MockProjectService.getList(
      skipCount: _skipCount,
      maxResultCount: _maxResultCount,
      sorting: _sorting,
      filter: _filter,
      statusFilter: _statusFilter,
      priorityFilter: _priorityFilter,
      ownerFilter: _ownerFilter,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadProjects());
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
      _skipCount = (_skipCount - _maxResultCount).clamp(0, double.infinity).toInt();
      await refresh();
    }
  }

  Future<void> search(String? query) async {
    _filter = query;
    _skipCount = 0;
    await refresh();
  }

  Future<void> filterByStatus(ProjectStatus? status) async {
    _statusFilter = status;
    _skipCount = 0;
    await refresh();
  }

  Future<void> filterByPriority(ProjectPriority? priority) async {
    _priorityFilter = priority;
    _skipCount = 0;
    await refresh();
  }

  Future<void> clearFilters() async {
    _statusFilter = null;
    _priorityFilter = null;
    _ownerFilter = null;
    _filter = null;
    _skipCount = 0;
    await refresh();
  }

  Future<void> sort(String field, bool ascending) async {
    _sorting = '$field ${ascending ? 'ASC' : 'DESC'}';
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
      _ownerFilter != null ||
      (_filter != null && _filter!.isNotEmpty);
}
