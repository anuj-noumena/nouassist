import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_tag_dto.dart';
import '../data/mock/mock_task_tag_service.dart';

part 'task_tag_provider.g.dart';

@riverpod
class TaskTagList extends _$TaskTagList {
  String? _searchQuery;

  @override
  Future<List<TaskTagDto>> build() async {
    return _loadTags();
  }

  Future<List<TaskTagDto>> _loadTags() async {
    return await MockTaskTagService.getList(search: _searchQuery);
  }

  Future<void> search(String? query) async {
    _searchQuery = query;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadTags());
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadTags());
  }
}

@riverpod
Future<TaskTagDto> taskTag(Ref ref, String id) async {
  return await MockTaskTagService.get(id);
}

@riverpod
Future<void> createTaskTag(Ref ref,
  CreateUpdateTaskTagDto input,
) async {
  await MockTaskTagService.create(input);
  ref.invalidate(taskTagListProvider);
}

@riverpod
Future<void> deleteTaskTag(Ref ref, String id) async {
  await MockTaskTagService.delete(id);
  ref.invalidate(taskTagListProvider);
}
