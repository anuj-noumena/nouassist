import '../models/gtd_someday_maybe_dto.dart';

/// Mock GTD Someday/Maybe service
class MockGtdSomedayMaybeService {
  static final List<GtdSomedayMaybeDto> _items = _generateMockItems();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get someday/maybe items
  static Future<List<GtdSomedayMaybeDto>> getByUser(
    String userId, {
    SomedayMaybeCategory? category,
    bool? isActivated,
  }) async {
    await _simulateDelay();

    var filtered = _items.where((item) => item.userId == userId);

    if (category != null) {
      filtered = filtered.where((item) => item.category == category);
    }

    if (isActivated != null) {
      filtered = filtered.where((item) => item.isActivated == isActivated);
    }

    return filtered.toList()
      ..sort((a, b) => b.creationTime.compareTo(a.creationTime));
  }

  /// Get single item
  static Future<GtdSomedayMaybeDto> get(String id) async {
    await _simulateDelay();

    try {
      return _items.firstWhere((item) => item.id == id);
    } catch (e) {
      throw Exception('Someday/Maybe item not found');
    }
  }

  /// Create item
  static Future<GtdSomedayMaybeDto> create(
    CreateUpdateSomedayMaybeDto input,
  ) async {
    await _simulateDelay();

    final newItem = GtdSomedayMaybeDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      title: input.title,
      description: input.description,
      category: input.category,
      areaOfFocusId: input.areaOfFocusId,
      tags: input.tags,
      reviewDate: input.reviewDate,
      creationTime: DateTime.now(),
    );

    _items.insert(0, newItem);
    return newItem;
  }

  /// Update item
  static Future<GtdSomedayMaybeDto> update(
    String id,
    CreateUpdateSomedayMaybeDto input,
  ) async {
    await _simulateDelay();

    final index = _items.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Someday/Maybe item not found');
    }

    final updatedItem = _items[index].copyWith(
      title: input.title,
      description: input.description,
      category: input.category,
      areaOfFocusId: input.areaOfFocusId,
      tags: input.tags,
      reviewDate: input.reviewDate,
    );

    _items[index] = updatedItem;
    return updatedItem;
  }

  /// Activate item
  static Future<GtdSomedayMaybeDto> activate(String id, String projectId) async {
    await _simulateDelay();

    final index = _items.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Someday/Maybe item not found');
    }

    final updatedItem = _items[index].copyWith(
      isActivated: true,
      activatedDate: DateTime.now(),
      activatedAsProjectId: projectId,
    );

    _items[index] = updatedItem;
    return updatedItem;
  }

  /// Delete item
  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _items.length;
    _items.removeWhere((item) => item.id == id);
    if (_items.length == initialLength) {
      throw Exception('Someday/Maybe item not found');
    }
  }

  /// Generate mock items
  static List<GtdSomedayMaybeDto> _generateMockItems() {
    final now = DateTime.now();
    return [
      GtdSomedayMaybeDto(
        id: 'someday-1',
        userId: 'user-1',
        title: 'Learn machine learning',
        description: 'Take online course and build some projects',
        category: SomedayMaybeCategory.skill,
        tags: ['learning', 'tech', 'AI'],
        creationTime: now.subtract(const Duration(days: 45)),
      ),
      GtdSomedayMaybeDto(
        id: 'someday-2',
        userId: 'user-1',
        title: 'Visit Japan',
        description: 'Tokyo, Kyoto, and Mount Fuji',
        category: SomedayMaybeCategory.place,
        tags: ['travel', 'vacation'],
        creationTime: now.subtract(const Duration(days: 60)),
      ),
      GtdSomedayMaybeDto(
        id: 'someday-3',
        userId: 'user-1',
        title: 'Write a book',
        description: 'Book about productivity and time management',
        category: SomedayMaybeCategory.project,
        tags: ['writing', 'productivity'],
        creationTime: now.subtract(const Duration(days: 90)),
      ),
      GtdSomedayMaybeDto(
        id: 'someday-4',
        userId: 'user-1',
        title: 'Learn to play guitar',
        description: 'Take lessons and practice regularly',
        category: SomedayMaybeCategory.hobby,
        tags: ['music', 'hobby'],
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      GtdSomedayMaybeDto(
        id: 'someday-5',
        userId: 'user-1',
        title: 'Buy standing desk',
        description: 'Ergonomic desk for home office',
        category: SomedayMaybeCategory.purchase,
        tags: ['home', 'office'],
        creationTime: now.subtract(const Duration(days: 15)),
      ),
      GtdSomedayMaybeDto(
        id: 'someday-6',
        userId: 'user-1',
        title: 'Start a podcast',
        description: 'Podcast about technology and innovation',
        category: SomedayMaybeCategory.project,
        tags: ['content', 'tech'],
        isActivated: true,
        activatedDate: now.subtract(const Duration(days: 5)),
        activatedAsProjectId: 'project-1',
        creationTime: now.subtract(const Duration(days: 120)),
      ),
    ];
  }
}
