import '../models/gtd_context_dto.dart';

/// Mock GTD Context service
class MockGtdContextService {
  static final List<GtdContextDto> _contexts = _generateMockContexts();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get contexts
  static Future<List<GtdContextDto>> getByUser(String userId) async {
    await _simulateDelay();
    return _contexts.where((c) => c.userId == userId).toList()
      ..sort((a, b) => a.name.compareTo(b.name));
  }

  /// Get single context
  static Future<GtdContextDto> get(String id) async {
    await _simulateDelay();

    try {
      return _contexts.firstWhere((context) => context.id == id);
    } catch (e) {
      throw Exception('Context not found');
    }
  }

  /// Create context
  static Future<GtdContextDto> create(CreateUpdateContextDto input) async {
    await _simulateDelay();

    final newContext = GtdContextDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      name: input.name,
      description: input.description,
      icon: input.icon,
      color: input.color,
      contextType: input.contextType,
      isActive: input.isActive,
      creationTime: DateTime.now(),
    );

    _contexts.insert(0, newContext);
    return newContext;
  }

  /// Update context
  static Future<GtdContextDto> update(
    String id,
    CreateUpdateContextDto input,
  ) async {
    await _simulateDelay();

    final index = _contexts.indexWhere((c) => c.id == id);
    if (index == -1) {
      throw Exception('Context not found');
    }

    final updatedContext = _contexts[index].copyWith(
      name: input.name,
      description: input.description,
      icon: input.icon,
      color: input.color,
      contextType: input.contextType,
      isActive: input.isActive,
    );

    _contexts[index] = updatedContext;
    return updatedContext;
  }

  /// Delete context
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initialLength = _contexts.length;
     _contexts.removeWhere((c) => c.id == id);
    if (_contexts.length == initialLength) {
      throw Exception('Context not found');
    }
  }

  /// Generate mock contexts
  static List<GtdContextDto> _generateMockContexts() {
    final now = DateTime.now();
    return [
      GtdContextDto(
        id: 'context-1',
        userId: 'user-1',
        name: '@Phone',
        description: 'Tasks requiring phone calls',
        icon: 'phone',
        color: '#2196F3',
        contextType: ContextType.tool,
        actionCount: 3,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdContextDto(
        id: 'context-2',
        userId: 'user-1',
        name: '@Errands',
        description: 'Things to do while out',
        icon: 'shopping_cart',
        color: '#4CAF50',
        contextType: ContextType.location,
        actionCount: 2,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdContextDto(
        id: 'context-3',
        userId: 'user-1',
        name: '@Computer',
        description: 'Tasks requiring computer',
        icon: 'computer',
        color: '#FF9800',
        contextType: ContextType.tool,
        actionCount: 5,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdContextDto(
        id: 'context-4',
        userId: 'user-1',
        name: '@Office',
        description: 'Tasks to do at the office',
        icon: 'business',
        color: '#9C27B0',
        contextType: ContextType.location,
        actionCount: 4,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdContextDto(
        id: 'context-5',
        userId: 'user-1',
        name: '@Home',
        description: 'Tasks to do at home',
        icon: 'home',
        color: '#F44336',
        contextType: ContextType.location,
        actionCount: 3,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdContextDto(
        id: 'context-6',
        userId: 'user-1',
        name: '@High Energy',
        description: 'Tasks requiring focus and energy',
        icon: 'bolt',
        color: '#FFEB3B',
        contextType: ContextType.energy,
        actionCount: 2,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
    ];
  }
}
