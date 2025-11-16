import '../models/gtd_inbox_dto.dart';

/// Mock GTD Inbox service
class MockGtdInboxService {
  static final List<GtdInboxDto> _inbox = _generateMockInbox();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get inbox items
  static Future<List<GtdInboxDto>> getByUser(
    String userId, {
    InboxStatus? status,
  }) async {
    await _simulateDelay();

    var filtered = _inbox.where((item) => item.userId == userId);

    if (status != null) {
      filtered = filtered.where((item) => item.status == status);
    }

    return filtered.toList()
      ..sort((a, b) => b.creationTime.compareTo(a.creationTime));
  }

  /// Get single inbox item
  static Future<GtdInboxDto> get(String id) async {
    await _simulateDelay();

    try {
      return _inbox.firstWhere((item) => item.id == id);
    } catch (e) {
      throw Exception('Inbox item not found');
    }
  }

  /// Create inbox item
  static Future<GtdInboxDto> create(CreateUpdateInboxDto input) async {
    await _simulateDelay();

    final newItem = GtdInboxDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      content: input.content,
      notes: input.notes,
      itemType: input.itemType,
      status: InboxStatus.unprocessed,
      tags: input.tags,
      source: input.source,
      creationTime: DateTime.now(),
    );

    _inbox.insert(0, newItem);
    return newItem;
  }

  /// Update inbox item
  static Future<GtdInboxDto> update(
    String id,
    CreateUpdateInboxDto input,
  ) async {
    await _simulateDelay();

    final index = _inbox.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Inbox item not found');
    }

    final updatedItem = _inbox[index].copyWith(
      content: input.content,
      notes: input.notes,
      itemType: input.itemType,
      tags: input.tags,
      source: input.source,
    );

    _inbox[index] = updatedItem;
    return updatedItem;
  }

  /// Mark as processed
  static Future<GtdInboxDto> markAsProcessed(
    String id,
    String processedAs,
  ) async {
    await _simulateDelay();

    final index = _inbox.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Inbox item not found');
    }

    final updatedItem = _inbox[index].copyWith(
      status: InboxStatus.processed,
      processedDate: DateTime.now(),
      processedAs: processedAs,
    );

    _inbox[index] = updatedItem;
    return updatedItem;
  }

  /// Delete inbox item
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initialLength = _inbox.length;

    _inbox.removeWhere((item) => item.id == id);
    if (_inbox.length == initialLength) {
      throw Exception('Inbox item not found');
    }
  }

  /// Generate mock inbox
  static List<GtdInboxDto> _generateMockInbox() {
    final now = DateTime.now();
    return [
      GtdInboxDto(
        id: 'inbox-1',
        userId: 'user-1',
        content: 'Read "Getting Things Done" book',
        notes: 'Recommended by colleague',
        itemType: InboxItemType.thought,
        status: InboxStatus.unprocessed,
        tags: ['reading', 'productivity'],
        creationTime: now.subtract(const Duration(hours: 2)),
      ),
      GtdInboxDto(
        id: 'inbox-2',
        userId: 'user-1',
        content: 'Schedule dentist appointment',
        itemType: InboxItemType.task,
        status: InboxStatus.unprocessed,
        tags: ['health'],
        creationTime: now.subtract(const Duration(hours: 5)),
      ),
      GtdInboxDto(
        id: 'inbox-3',
        userId: 'user-1',
        content: 'Review Q1 budget with team',
        notes: 'Meeting requested by manager',
        itemType: InboxItemType.meeting,
        status: InboxStatus.unprocessed,
        source: 'Email',
        creationTime: now.subtract(const Duration(days: 1)),
      ),
      GtdInboxDto(
        id: 'inbox-4',
        userId: 'user-1',
        content: 'Learn Spanish',
        notes: 'Planning trip to Spain next year',
        itemType: InboxItemType.idea,
        status: InboxStatus.unprocessed,
        tags: ['learning', 'travel'],
        creationTime: now.subtract(const Duration(days: 2)),
      ),
      GtdInboxDto(
        id: 'inbox-5',
        userId: 'user-1',
        content: 'Update customer database',
        itemType: InboxItemType.email,
        status: InboxStatus.processed,
        processedAs: 'Next Action',
        processedDate: now.subtract(const Duration(days: 3)),
        source: 'Email',
        creationTime: now.subtract(const Duration(days: 4)),
      ),
    ];
  }
}
