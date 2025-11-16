import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_checklist_dto.dart';
import '../data/mock/mock_task_checklist_service.dart';

part 'task_checklist_provider.g.dart';

@riverpod
Future<List<TaskChecklistDto>> taskChecklists(Ref ref,
  String taskId,
) async {
  return await MockTaskChecklistService.getByTask(taskId);
}

@riverpod
class TaskChecklist extends _$TaskChecklist {
  @override
  Future<TaskChecklistDto> build(String id) async {
    return await MockTaskChecklistService.get(id);
  }

  Future<void> toggleItem(String itemId) async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(
      () => MockTaskChecklistService.toggleItem(id, itemId),
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => MockTaskChecklistService.get(id));
  }
}

@riverpod
Future<void> createChecklist(Ref ref,
  CreateUpdateChecklistDto input,
) async {
  await MockTaskChecklistService.create(input);
  ref.invalidate(taskChecklistsProvider(input.taskId));
}

@riverpod
Future<void> deleteChecklist(Ref ref,
  String id,
  String taskId,
) async {
  await MockTaskChecklistService.delete(id);
  ref.invalidate(taskChecklistsProvider(taskId));
}
