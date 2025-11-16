import '../models/task_type_dto.dart';

/// Mock task type service
class MockTaskTypeService {
  static final List<TaskTypeDto> _taskTypes = _generateMockTaskTypes();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskTypeDto>> getList() async {
    await _simulateDelay();
    return List.from(_taskTypes);
  }

  static Future<TaskTypeDto> get(String id) async {
    await _simulateDelay();
    try {
      return _taskTypes.firstWhere((type) => type.id == id);
    } catch (e) {
      throw Exception('Task type not found');
    }
  }

  static Future<TaskTypeDto> create(CreateUpdateTaskTypeDto input) async {
    await _simulateDelay();

    final newType = TaskTypeDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      name: input.name,
      icon: input.icon,
      color: input.color,
      description: input.description,
      defaultWorkflowId: input.defaultWorkflowId,
      isActive: input.isActive,
      creationTime: DateTime.now(),
    );

    _taskTypes.insert(0, newType);
    return newType;
  }

  static Future<TaskTypeDto> update(String id, CreateUpdateTaskTypeDto input) async {
    await _simulateDelay();

    final index = _taskTypes.indexWhere((type) => type.id == id);
    if (index == -1) {
      throw Exception('Task type not found');
    }

    final updatedType = _taskTypes[index].copyWith(
      name: input.name,
      icon: input.icon,
      color: input.color,
      description: input.description,
      defaultWorkflowId: input.defaultWorkflowId,
      isActive: input.isActive,
    );

    _taskTypes[index] = updatedType;
    return updatedType;
  }

  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _taskTypes.length;
    _taskTypes.removeWhere((type) => type.id == id);
    if (_taskTypes.length == initialLength) {
      throw Exception('Task type not found');
    }
  }

  static List<TaskTypeDto> _generateMockTaskTypes() {
    final now = DateTime.now();
    return [
      TaskTypeDto(
        id: 'type-1',
        name: 'Bug',
        icon: '🐛',
        color: '#D32F2F',
        description: 'Something that needs to be fixed',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-2',
        name: 'Feature',
        icon: '✨',
        color: '#1976D2',
        description: 'New feature or enhancement',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-3',
        name: 'Story',
        icon: '📖',
        color: '#388E3C',
        description: 'User story',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-4',
        name: 'Task',
        icon: '✓',
        color: '#757575',
        description: 'General task',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-5',
        name: 'Epic',
        icon: '🎯',
        color: '#7B1FA2',
        description: 'Large body of work',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-6',
        name: 'Improvement',
        icon: '🚀',
        color: '#F57C00',
        description: 'Enhancement or optimization',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-7',
        name: 'Documentation',
        icon: '📚',
        color: '#0097A7',
        description: 'Documentation task',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskTypeDto(
        id: 'type-8',
        name: 'Research',
        icon: '🔬',
        color: '#5D4037',
        description: 'Research or investigation',
        defaultWorkflowId: 'workflow-1',
        isActive: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
    ];
  }
}
