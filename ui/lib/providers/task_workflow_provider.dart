import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_workflow_dto.dart';
import '../data/mock/mock_task_workflow_service.dart';

part 'task_workflow_provider.g.dart';

@riverpod
Future<List<TaskWorkflowDto>> taskWorkflows(Ref ref) async {
  return await MockTaskWorkflowService.getList();
}

@riverpod
Future<TaskWorkflowDto> taskWorkflow(Ref ref, String id) async {
  return await MockTaskWorkflowService.get(id);
}
