import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_dependency_dto.dart';
import '../data/mock/mock_task_dependency_service.dart';

part 'task_dependency_provider.g.dart';

@riverpod
Future<List<TaskDependencyDto>> taskDependencies(Ref ref,
  String taskId,
) async {
  return await MockTaskDependencyService.getByTask(taskId);
}

@riverpod
Future<List<TaskDependencyDto>> taskPredecessors(Ref ref,
  String taskId,
) async {
  return await MockTaskDependencyService.getPredecessors(taskId);
}

@riverpod
Future<List<TaskDependencyDto>> taskSuccessors(Ref ref,
  String taskId,
) async {
  return await MockTaskDependencyService.getSuccessors(taskId);
}

@riverpod
Future<void> createDependency(Ref ref,
  CreateDependencyDto input,
) async {
  await MockTaskDependencyService.create(input);
  ref.invalidate(taskDependenciesProvider(input.predecessorTaskId));
  ref.invalidate(taskDependenciesProvider(input.successorTaskId));
}

@riverpod
Future<void> deleteDependency(Ref ref,
  String id,
  String taskId,
) async {
  await MockTaskDependencyService.delete(id);
  ref.invalidate(taskDependenciesProvider(taskId));
}
