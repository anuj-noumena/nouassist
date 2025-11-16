import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/project_dto.dart';
import '../data/mock/mock_project_service.dart';

part 'project_provider.g.dart';

@riverpod
class Project extends _$Project {
  @override
  Future<ProjectDto> build(String id) async {
    return await MockProjectService.get(id);
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => MockProjectService.get(id));
  }
}

@riverpod
Future<List<ProjectDto>> childProjects(Ref ref, String parentId) async {
  return await MockProjectService.getChildProjects(parentId);
}
