import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/mock/mock_project_service.dart';
import 'project_list_provider.dart';
import 'project_provider.dart';

part 'project_delete_provider.g.dart';

@riverpod
Future<void> deleteProject(Ref ref, String projectId) async {
  await MockProjectService.delete(projectId);

  ref.invalidate(projectListProvider);
  ref.invalidate(projectProvider(projectId));
}
