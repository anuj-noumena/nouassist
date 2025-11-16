import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_delegation_dto.dart';
import '../data/mock/mock_task_delegation_service.dart';

part 'task_delegation_provider.g.dart';

@riverpod
Future<List<TaskDelegationDto>> taskDelegations(Ref ref,
  String taskId,
) async {
  return await MockTaskDelegationService.getByTask(taskId);
}

@riverpod
Future<List<TaskDelegationDto>> userDelegations(Ref ref,
  String userId, {
  bool? delegatedTo,
}) async {
  return await MockTaskDelegationService.getByUser(
    userId,
    delegatedTo: delegatedTo,
  );
}

@riverpod
Future<void> createDelegation(Ref ref,
  CreateDelegationDto input,
) async {
  await MockTaskDelegationService.create(input);
  ref.invalidate(taskDelegationsProvider(input.taskId));
  ref.invalidate(userDelegationsProvider('user-1'));
}

@riverpod
Future<void> acceptDelegation(Ref ref,
  String id,
  String taskId,
  String comment,
) async {
  await MockTaskDelegationService.acceptDelegation(id, comment);
  ref.invalidate(taskDelegationsProvider(taskId));
  ref.invalidate(userDelegationsProvider('user-1'));
}

@riverpod
Future<void> rejectDelegation(Ref ref,
  String id,
  String taskId,
  String comment,
) async {
  await MockTaskDelegationService.rejectDelegation(id, comment);
  ref.invalidate(taskDelegationsProvider(taskId));
  ref.invalidate(userDelegationsProvider('user-1'));
}
