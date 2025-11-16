import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_dto.dart';
import '../data/mock/mock_task_service.dart';

part 'task_provider.g.dart';

@riverpod
class Task extends _$Task {
  @override
  Future<TaskDto> build(String id) async {
    return await MockTaskService.get(id);
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => MockTaskService.get(id));
  }
}
