import '../models/task_dependency_dto.dart';

/// Mock task dependency service
class MockTaskDependencyService {
  static final List<TaskDependencyDto> _dependencies = _generateMockDependencies();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskDependencyDto>> getByTask(String taskId) async {
    await _simulateDelay();
    return _dependencies
        .where((d) =>
            d.predecessorTaskId == taskId || d.successorTaskId == taskId)
        .toList();
  }

  static Future<List<TaskDependencyDto>> getPredecessors(String taskId) async {
    await _simulateDelay();
    return _dependencies
        .where((d) => d.successorTaskId == taskId)
        .toList();
  }

  static Future<List<TaskDependencyDto>> getSuccessors(String taskId) async {
    await _simulateDelay();
    return _dependencies
        .where((d) => d.predecessorTaskId == taskId)
        .toList();
  }

  static Future<TaskDependencyDto> create(CreateDependencyDto input) async {
    await _simulateDelay();

    // Validate no circular dependencies
    if (input.predecessorTaskId == input.successorTaskId) {
      throw Exception('Cannot create dependency to itself');
    }

    // Check for existing dependency
    final exists = _dependencies.any((d) =>
        d.predecessorTaskId == input.predecessorTaskId &&
        d.successorTaskId == input.successorTaskId);

    if (exists) {
      throw Exception('Dependency already exists');
    }

    final newDependency = TaskDependencyDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      predecessorTaskId: input.predecessorTaskId,
      predecessorTaskTitle: 'Task ${input.predecessorTaskId}',
      successorTaskId: input.successorTaskId,
      successorTaskTitle: 'Task ${input.successorTaskId}',
      dependencyType: input.dependencyType,
      lagDays: input.lagDays,
      creationTime: DateTime.now(),
    );

    _dependencies.insert(0, newDependency);
    return newDependency;
  }

  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _dependencies.length;
    _dependencies.removeWhere((d) => d.id == id);
    if (_dependencies.length == initialLength) {
      throw Exception('Dependency not found');
    }
  }

  static List<TaskDependencyDto> _generateMockDependencies() {
    final now = DateTime.now();
    return [
      TaskDependencyDto(
        id: 'dep-1',
        predecessorTaskId: '1',
        predecessorTaskTitle: 'Implement user authentication system',
        successorTaskId: '4',
        successorTaskTitle: 'Optimize database queries for dashboard',
        dependencyType: DependencyType.finishToStart,
        lagDays: 0,
        creationTime: now.subtract(const Duration(days: 10)),
      ),
      TaskDependencyDto(
        id: 'dep-2',
        predecessorTaskId: '3',
        predecessorTaskTitle: 'Design new landing page mockups',
        successorTaskId: '9',
        successorTaskTitle: 'Add dark mode support',
        dependencyType: DependencyType.finishToStart,
        lagDays: 1,
        creationTime: now.subtract(const Duration(days: 5)),
      ),
      TaskDependencyDto(
        id: 'dep-3',
        predecessorTaskId: '7',
        predecessorTaskTitle: 'Research payment gateway options',
        successorTaskId: '6',
        successorTaskTitle: 'Implement real-time notifications',
        dependencyType: DependencyType.startToStart,
        lagDays: 0,
        creationTime: now.subtract(const Duration(days: 3)),
      ),
    ];
  }
}
