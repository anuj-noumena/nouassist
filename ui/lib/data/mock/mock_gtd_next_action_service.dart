import '../models/gtd_next_action_dto.dart';

/// Mock GTD Next Action service
class MockGtdNextActionService {
  static final List<GtdNextActionDto> _actions = _generateMockActions();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get next actions
  static Future<List<GtdNextActionDto>> getByUser(
    String userId, {
    List<String>? contextIds,
    bool? isCompleted,
    bool? isStarred,
  }) async {
    await _simulateDelay();

    var filtered = _actions.where((action) => action.userId == userId);

    if (contextIds != null && contextIds.isNotEmpty) {
      filtered = filtered.where((action) {
        return action.contextIds.any((id) => contextIds.contains(id));
      });
    }

    if (isCompleted != null) {
      filtered = filtered.where((action) => action.isCompleted == isCompleted);
    }

    if (isStarred != null) {
      filtered = filtered.where((action) => action.isStarred == isStarred);
    }

    return filtered.toList()
      ..sort((a, b) {
        if (a.isStarred != b.isStarred) {
          return a.isStarred ? -1 : 1;
        }
        if (a.dueDate != null && b.dueDate != null) {
          return a.dueDate!.compareTo(b.dueDate!);
        }
        if (a.dueDate != null) return -1;
        if (b.dueDate != null) return 1;
        return b.creationTime.compareTo(a.creationTime);
      });
  }

  /// Get single action
  static Future<GtdNextActionDto> get(String id) async {
    await _simulateDelay();

    try {
      return _actions.firstWhere((action) => action.id == id);
    } catch (e) {
      throw Exception('Next action not found');
    }
  }

  /// Create next action
  static Future<GtdNextActionDto> create(CreateUpdateNextActionDto input) async {
    await _simulateDelay();

    final newAction = GtdNextActionDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      action: input.action,
      notes: input.notes,
      contextIds: input.contextIds,
      projectId: input.projectId,
      areaOfFocusId: input.areaOfFocusId,
      energyLevel: input.energyLevel,
      timeRequired: input.timeRequired,
      dueDate: input.dueDate,
      scheduledDate: input.scheduledDate,
      isStarred: input.isStarred,
      tags: input.tags,
      outcome: input.outcome,
      creationTime: DateTime.now(),
    );

    _actions.insert(0, newAction);
    return newAction;
  }

  /// Update next action
  static Future<GtdNextActionDto> update(
    String id,
    CreateUpdateNextActionDto input,
  ) async {
    await _simulateDelay();

    final index = _actions.indexWhere((action) => action.id == id);
    if (index == -1) {
      throw Exception('Next action not found');
    }

    final updatedAction = _actions[index].copyWith(
      action: input.action,
      notes: input.notes,
      contextIds: input.contextIds,
      projectId: input.projectId,
      areaOfFocusId: input.areaOfFocusId,
      energyLevel: input.energyLevel,
      timeRequired: input.timeRequired,
      dueDate: input.dueDate,
      scheduledDate: input.scheduledDate,
      isStarred: input.isStarred,
      tags: input.tags,
      outcome: input.outcome,
    );

    _actions[index] = updatedAction;
    return updatedAction;
  }

  /// Toggle completion
  static Future<GtdNextActionDto> toggleCompletion(String id) async {
    await _simulateDelay();

    final index = _actions.indexWhere((action) => action.id == id);
    if (index == -1) {
      throw Exception('Next action not found');
    }

    final updatedAction = _actions[index].copyWith(
      isCompleted: !_actions[index].isCompleted,
      completedDate:
          !_actions[index].isCompleted ? DateTime.now() : null,
    );

    _actions[index] = updatedAction;
    return updatedAction;
  }

  /// Toggle star
  static Future<GtdNextActionDto> toggleStar(String id) async {
    await _simulateDelay();

    final index = _actions.indexWhere((action) => action.id == id);
    if (index == -1) {
      throw Exception('Next action not found');
    }

    final updatedAction = _actions[index].copyWith(
      isStarred: !_actions[index].isStarred,
    );

    _actions[index] = updatedAction;
    return updatedAction;
  }

  /// Delete next action
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initialLength = _actions.length;
    _actions.removeWhere((action) => action.id == id);
    if (_actions.length == initialLength) {
      throw Exception('Next action not found');
    }
  }

  /// Generate mock actions
  static List<GtdNextActionDto> _generateMockActions() {
    final now = DateTime.now();
    return [
      GtdNextActionDto(
        id: 'action-1',
        userId: 'user-1',
        action: 'Call John about project proposal',
        notes: 'Discuss timeline and budget',
        contextIds: ['context-1'],
        energyLevel: EnergyLevel.medium,
        timeRequired: TimeRequired.short,
        dueDate: now.add(const Duration(days: 2)),
        isStarred: true,
        tags: ['important', 'work'],
        creationTime: now.subtract(const Duration(hours: 3)),
      ),
      GtdNextActionDto(
        id: 'action-2',
        userId: 'user-1',
        action: 'Buy groceries',
        notes: 'Milk, eggs, bread, vegetables',
        contextIds: ['context-2'],
        energyLevel: EnergyLevel.low,
        timeRequired: TimeRequired.medium,
        tags: ['personal', 'errands'],
        creationTime: now.subtract(const Duration(hours: 5)),
      ),
      GtdNextActionDto(
        id: 'action-3',
        userId: 'user-1',
        action: 'Review code for new feature',
        notes: 'Focus on security and performance',
        contextIds: ['context-3'],
        projectId: '1',
        energyLevel: EnergyLevel.high,
        timeRequired: TimeRequired.long,
        scheduledDate: now.add(const Duration(days: 1)),
        tags: ['coding', 'review'],
        creationTime: now.subtract(const Duration(days: 1)),
      ),
      GtdNextActionDto(
        id: 'action-4',
        userId: 'user-1',
        action: 'Reply to client email',
        contextIds: ['context-4'],
        energyLevel: EnergyLevel.medium,
        timeRequired: TimeRequired.short,
        isCompleted: true,
        completedDate: now.subtract(const Duration(hours: 2)),
        tags: ['communication'],
        creationTime: now.subtract(const Duration(days: 2)),
      ),
      GtdNextActionDto(
        id: 'action-5',
        userId: 'user-1',
        action: 'Schedule team meeting for Q2 planning',
        contextIds: ['context-4'],
        energyLevel: EnergyLevel.low,
        timeRequired: TimeRequired.short,
        dueDate: now.add(const Duration(days: 5)),
        tags: ['planning', 'team'],
        creationTime: now.subtract(const Duration(days: 3)),
      ),
    ];
  }
}
