import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/mock/mock_task_service.dart';
import 'task_list_provider.dart';
import 'task_provider.dart';

part 'task_delete_provider.g.dart';

@riverpod
Future<void> deleteTask(Ref ref, String taskId) async {
  await MockTaskService.delete(taskId);

  ref.invalidate(taskListProvider);
  ref.invalidate(taskProvider(taskId));
}
