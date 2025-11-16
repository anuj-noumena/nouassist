import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/task_type_dto.dart';
import '../data/mock/mock_task_type_service.dart';

part 'task_type_provider.g.dart';

@riverpod
Future<List<TaskTypeDto>> taskTypes(Ref ref) async {
  return await MockTaskTypeService.getList();
}

@riverpod
Future<TaskTypeDto> taskType(Ref ref, String id) async {
  return await MockTaskTypeService.get(id);
}
