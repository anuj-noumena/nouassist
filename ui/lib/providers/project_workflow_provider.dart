import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/project_workflow_dto.dart';
import '../data/mock/mock_project_workflow_service.dart';

part 'project_workflow_provider.g.dart';

@riverpod
Future<List<ProjectWorkflowDto>> projectWorkflows(Ref ref) async {
  return await MockProjectWorkflowService.getList();
}

@riverpod
Future<ProjectWorkflowDto> projectWorkflow(Ref ref, String id) async {
  return await MockProjectWorkflowService.get(id);
}
