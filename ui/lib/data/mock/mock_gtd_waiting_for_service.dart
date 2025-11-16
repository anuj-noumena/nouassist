import '../models/gtd_waiting_for_dto.dart';

/// Mock GTD Waiting For service
class MockGtdWaitingForService {
  static final List<GtdWaitingForDto> _waitingFor = _generateMockWaitingFor();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get waiting for items
  static Future<List<GtdWaitingForDto>> getByUser(
    String userId, {
    WaitingForStatus? status,
  }) async {
    await _simulateDelay();

    var filtered = _waitingFor.where((item) => item.userId == userId);

    if (status != null) {
      filtered = filtered.where((item) => item.status == status);
    }

    return filtered.toList()
      ..sort((a, b) {
        if (a.expectedDate != null && b.expectedDate != null) {
          return a.expectedDate!.compareTo(b.expectedDate!);
        }
        if (a.expectedDate != null) return -1;
        if (b.expectedDate != null) return 1;
        return b.creationTime.compareTo(a.creationTime);
      });
  }

  /// Get single waiting for item
  static Future<GtdWaitingForDto> get(String id) async {
    await _simulateDelay();

    try {
      return _waitingFor.firstWhere((item) => item.id == id);
    } catch (e) {
      throw Exception('Waiting for item not found');
    }
  }

  /// Create waiting for item
  static Future<GtdWaitingForDto> create(CreateUpdateWaitingForDto input) async {
    await _simulateDelay();

    final newItem = GtdWaitingForDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      description: input.description,
      waitingFor: input.waitingFor,
      notes: input.notes,
      projectId: input.projectId,
      relatedActionId: input.relatedActionId,
      status: WaitingForStatus.waiting,
      requestedDate: input.requestedDate ?? DateTime.now(),
      expectedDate: input.expectedDate,
      reminderDate: input.reminderDate,
      tags: input.tags,
      creationTime: DateTime.now(),
    );

    _waitingFor.insert(0, newItem);
    return newItem;
  }

  /// Update waiting for item
  static Future<GtdWaitingForDto> update(
    String id,
    CreateUpdateWaitingForDto input,
  ) async {
    await _simulateDelay();

    final index = _waitingFor.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Waiting for item not found');
    }

    final updatedItem = _waitingFor[index].copyWith(
      description: input.description,
      waitingFor: input.waitingFor,
      notes: input.notes,
      projectId: input.projectId,
      relatedActionId: input.relatedActionId,
      requestedDate: input.requestedDate,
      expectedDate: input.expectedDate,
      reminderDate: input.reminderDate,
      tags: input.tags,
    );

    _waitingFor[index] = updatedItem;
    return updatedItem;
  }

  /// Mark as received
  static Future<GtdWaitingForDto> markAsReceived(String id) async {
    await _simulateDelay();

    final index = _waitingFor.indexWhere((item) => item.id == id);
    if (index == -1) {
      throw Exception('Waiting for item not found');
    }

    final updatedItem = _waitingFor[index].copyWith(
      status: WaitingForStatus.received,
      receivedDate: DateTime.now(),
    );

    _waitingFor[index] = updatedItem;
    return updatedItem;
  }

  /// Delete waiting for item
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initalLength = _waitingFor.length;
    _waitingFor.removeWhere((item) => item.id == id);
    if (_waitingFor.length == initalLength) {
      throw Exception('Waiting for item not found');
    }
  }

  /// Generate mock waiting for items
  static List<GtdWaitingForDto> _generateMockWaitingFor() {
    final now = DateTime.now();
    return [
      GtdWaitingForDto(
        id: 'waiting-1',
        userId: 'user-1',
        description: 'Proposal review from client',
        waitingFor: 'John Smith (Client)',
        notes: 'Sent proposal last week',
        status: WaitingForStatus.waiting,
        requestedDate: now.subtract(const Duration(days: 7)),
        expectedDate: now.add(const Duration(days: 3)),
        tags: ['client', 'proposal'],
        creationTime: now.subtract(const Duration(days: 7)),
      ),
      GtdWaitingForDto(
        id: 'waiting-2',
        userId: 'user-1',
        description: 'Hardware approval from IT',
        waitingFor: 'IT Department',
        notes: 'Requested new laptop',
        status: WaitingForStatus.reminded,
        requestedDate: now.subtract(const Duration(days: 14)),
        expectedDate: now.add(const Duration(days: 7)),
        reminderDate: now.add(const Duration(days: 2)),
        tags: ['IT', 'equipment'],
        creationTime: now.subtract(const Duration(days: 14)),
      ),
      GtdWaitingForDto(
        id: 'waiting-3',
        userId: 'user-1',
        description: 'Budget approval',
        waitingFor: 'Finance Team',
        status: WaitingForStatus.waiting,
        requestedDate: now.subtract(const Duration(days: 3)),
        expectedDate: now.add(const Duration(days: 10)),
        tags: ['finance', 'approval'],
        creationTime: now.subtract(const Duration(days: 3)),
      ),
      GtdWaitingForDto(
        id: 'waiting-4',
        userId: 'user-1',
        description: 'Contract signed',
        waitingFor: 'Legal Department',
        status: WaitingForStatus.received,
        requestedDate: now.subtract(const Duration(days: 20)),
        receivedDate: now.subtract(const Duration(days: 2)),
        tags: ['legal', 'contract'],
        creationTime: now.subtract(const Duration(days: 20)),
      ),
    ];
  }
}
