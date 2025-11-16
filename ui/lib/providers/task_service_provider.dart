import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/mock/mock_task_service.dart';

part 'task_service_provider.g.dart';

@riverpod
MockTaskService taskService(Ref ref) {
  return MockTaskService();
}
