import '../models/task_tag_dto.dart';

/// Mock task tag service
class MockTaskTagService {
  static final List<TaskTagDto> _tags = _generateMockTags();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskTagDto>> getList({String? search}) async {
    await _simulateDelay();

    if (search != null && search.isNotEmpty) {
      return _tags
          .where((tag) =>
              tag.name.toLowerCase().contains(search.toLowerCase()))
          .toList();
    }

    return List.from(_tags);
  }

  static Future<TaskTagDto> get(String id) async {
    await _simulateDelay();
    try {
      return _tags.firstWhere((tag) => tag.id == id);
    } catch (e) {
      throw Exception('Tag not found');
    }
  }

  static Future<TaskTagDto> create(CreateUpdateTaskTagDto input) async {
    await _simulateDelay();

    final newTag = TaskTagDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      name: input.name,
      color: input.color,
      description: input.description,
      category: input.category,
      usageCount: 0,
      creationTime: DateTime.now(),
    );

    _tags.insert(0, newTag);
    return newTag;
  }

  static Future<TaskTagDto> update(String id, CreateUpdateTaskTagDto input) async {
    await _simulateDelay();

    final index = _tags.indexWhere((tag) => tag.id == id);
    if (index == -1) {
      throw Exception('Tag not found');
    }

    final updatedTag = _tags[index].copyWith(
      name: input.name,
      color: input.color,
      description: input.description,
      category: input.category,
    );

    _tags[index] = updatedTag;
    return updatedTag;
  }

  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _tags.length;
    _tags.removeWhere((tag) => tag.id == id);
    if (_tags.length == initialLength) {
      throw Exception('Tag not found');
    }
  }

  static List<TaskTagDto> _generateMockTags() {
    final now = DateTime.now();
    return [
      TaskTagDto(
        id: 'tag-1',
        name: 'frontend',
        color: '#2196F3',
        description: 'Frontend development tasks',
        category: 'technology',
        usageCount: 45,
        creationTime: now.subtract(const Duration(days: 200)),
      ),
      TaskTagDto(
        id: 'tag-2',
        name: 'backend',
        color: '#4CAF50',
        description: 'Backend development tasks',
        category: 'technology',
        usageCount: 38,
        creationTime: now.subtract(const Duration(days: 200)),
      ),
      TaskTagDto(
        id: 'tag-3',
        name: 'urgent',
        color: '#F44336',
        description: 'Urgent tasks requiring immediate attention',
        category: 'priority',
        usageCount: 12,
        creationTime: now.subtract(const Duration(days: 180)),
      ),
      TaskTagDto(
        id: 'tag-4',
        name: 'api',
        color: '#FF9800',
        description: 'API related tasks',
        category: 'technology',
        usageCount: 28,
        creationTime: now.subtract(const Duration(days: 150)),
      ),
      TaskTagDto(
        id: 'tag-5',
        name: 'mobile',
        color: '#9C27B0',
        description: 'Mobile application tasks',
        category: 'platform',
        usageCount: 22,
        creationTime: now.subtract(const Duration(days: 150)),
      ),
      TaskTagDto(
        id: 'tag-6',
        name: 'design',
        color: '#E91E63',
        description: 'Design and UI/UX tasks',
        category: 'department',
        usageCount: 31,
        creationTime: now.subtract(const Duration(days: 120)),
      ),
      TaskTagDto(
        id: 'tag-7',
        name: 'security',
        color: '#795548',
        description: 'Security related tasks',
        category: 'technology',
        usageCount: 15,
        creationTime: now.subtract(const Duration(days: 100)),
      ),
      TaskTagDto(
        id: 'tag-8',
        name: 'performance',
        color: '#FF5722',
        description: 'Performance optimization tasks',
        category: 'quality',
        usageCount: 19,
        creationTime: now.subtract(const Duration(days: 90)),
      ),
    ];
  }
}
