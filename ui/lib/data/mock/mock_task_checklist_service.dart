import '../models/task_checklist_dto.dart';

/// Mock task checklist service
class MockTaskChecklistService {
  static final List<TaskChecklistDto> _checklists = _generateMockChecklists();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskChecklistDto>> getByTask(String taskId) async {
    await _simulateDelay();
    return _checklists.where((c) => c.taskId == taskId).toList();
  }

  static Future<TaskChecklistDto> get(String id) async {
    await _simulateDelay();
    try {
      return _checklists.firstWhere((c) => c.id == id);
    } catch (e) {
      throw Exception('Checklist not found');
    }
  }

  static Future<TaskChecklistDto> create(CreateUpdateChecklistDto input) async {
    await _simulateDelay();

    final newChecklist = TaskChecklistDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      taskId: input.taskId,
      title: input.title,
      items: input.items,
      progress: _calculateProgress(input.items),
      creationTime: DateTime.now(),
    );

    _checklists.insert(0, newChecklist);
    return newChecklist;
  }

  static Future<TaskChecklistDto> update(
      String id, CreateUpdateChecklistDto input) async {
    await _simulateDelay();

    final index = _checklists.indexWhere((c) => c.id == id);
    if (index == -1) {
      throw Exception('Checklist not found');
    }

    final updatedChecklist = _checklists[index].copyWith(
      title: input.title,
      items: input.items,
      progress: _calculateProgress(input.items),
    );

    _checklists[index] = updatedChecklist;
    return updatedChecklist;
  }

  static Future<TaskChecklistDto> toggleItem(
      String checklistId, String itemId) async {
    await _simulateDelay();

    final index = _checklists.indexWhere((c) => c.id == checklistId);
    if (index == -1) {
      throw Exception('Checklist not found');
    }

    final checklist = _checklists[index];
    final updatedItems = checklist.items.map((item) {
      if (item.id == itemId) {
        return item.copyWith(
          isCompleted: !item.isCompleted,
          completionTime: !item.isCompleted ? DateTime.now() : null,
          completedBy: !item.isCompleted ? 'user-1' : null,
        );
      }
      return item;
    }).toList();

    final updatedChecklist = checklist.copyWith(
      items: updatedItems,
      progress: _calculateProgress(updatedItems),
    );

    _checklists[index] = updatedChecklist;
    return updatedChecklist;
  }

  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _checklists.length;
    _checklists.removeWhere((c) => c.id == id);
    if (_checklists.length == initialLength) {
      throw Exception('Checklist not found');
    }
  }

  static double _calculateProgress(List<ChecklistItemDto> items) {
    if (items.isEmpty) return 0;
    final completed = items.where((item) => item.isCompleted).length;
    return (completed / items.length) * 100;
  }

  static List<TaskChecklistDto> _generateMockChecklists() {
    final now = DateTime.now();
    return [
      TaskChecklistDto(
        id: 'checklist-1',
        taskId: '1',
        title: 'Authentication Setup',
        items: [
          ChecklistItemDto(
            id: 'item-1',
            text: 'Set up JWT token generation',
            isCompleted: true,
            order: 1,
            completedBy: 'user-1',
            completionTime: now.subtract(const Duration(days: 2)),
          ),
          const ChecklistItemDto(
            id: 'item-2',
            text: 'Implement refresh token logic',
            isCompleted: false,
            order: 2,
          ),
          const ChecklistItemDto(
            id: 'item-3',
            text: 'Add password reset functionality',
            isCompleted: false,
            order: 3,
          ),
          ChecklistItemDto(
            id: 'item-4',
            text: 'Write unit tests',
            isCompleted: true,
            order: 4,
            completedBy: 'user-1',
            completionTime: now.subtract(const Duration(days: 1)),
          ),
        ],
        progress: 50.0,
        creationTime: now.subtract(const Duration(days: 5)),
      ),
      TaskChecklistDto(
        id: 'checklist-2',
        taskId: '3',
        title: 'Design Deliverables',
        items: [
          const ChecklistItemDto(
            id: 'item-5',
            text: 'Create wireframes',
            isCompleted: false,
            order: 1,
          ),
          const ChecklistItemDto(
            id: 'item-6',
            text: 'Design high-fidelity mockups',
            isCompleted: false,
            order: 2,
          ),
          const ChecklistItemDto(
            id: 'item-7',
            text: 'Create design system',
            isCompleted: false,
            order: 3,
          ),
        ],
        progress: 0.0,
        creationTime: now.subtract(const Duration(days: 1)),
      ),
    ];
  }
}
