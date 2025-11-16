import '../models/task_delegation_dto.dart';

/// Mock task delegation service
class MockTaskDelegationService {
  static final List<TaskDelegationDto> _delegations = _generateMockDelegations();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskDelegationDto>> getByTask(String taskId) async {
    await _simulateDelay();
    return _delegations.where((d) => d.taskId == taskId).toList();
  }

  static Future<List<TaskDelegationDto>> getByUser(String userId,
      {bool? delegatedTo}) async {
    await _simulateDelay();

    if (delegatedTo == true) {
      return _delegations.where((d) => d.delegatedTo == userId).toList();
    } else if (delegatedTo == false) {
      return _delegations.where((d) => d.delegatedFrom == userId).toList();
    }

    return _delegations
        .where((d) => d.delegatedTo == userId || d.delegatedFrom == userId)
        .toList();
  }

  static Future<TaskDelegationDto> create(CreateDelegationDto input) async {
    await _simulateDelay();

    final newDelegation = TaskDelegationDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      taskId: input.taskId,
      delegatedFrom: 'user-1',
      delegatedFromName: 'Current User',
      delegatedTo: input.delegatedTo,
      delegatedToName: 'User ${input.delegatedTo}',
      reason: input.reason,
      delegationDate: DateTime.now(),
      acceptanceStatus: DelegationStatus.pending,
    );

    _delegations.insert(0, newDelegation);
    return newDelegation;
  }

  static Future<TaskDelegationDto> acceptDelegation(
      String id, String comment) async {
    await _simulateDelay();

    final index = _delegations.indexWhere((d) => d.id == id);
    if (index == -1) {
      throw Exception('Delegation not found');
    }

    final updatedDelegation = _delegations[index].copyWith(
      acceptanceStatus: DelegationStatus.accepted,
      responseDate: DateTime.now(),
      responseComment: comment,
    );

    _delegations[index] = updatedDelegation;
    return updatedDelegation;
  }

  static Future<TaskDelegationDto> rejectDelegation(
      String id, String comment) async {
    await _simulateDelay();

    final index = _delegations.indexWhere((d) => d.id == id);
    if (index == -1) {
      throw Exception('Delegation not found');
    }

    final updatedDelegation = _delegations[index].copyWith(
      acceptanceStatus: DelegationStatus.rejected,
      responseDate: DateTime.now(),
      responseComment: comment,
    );

    _delegations[index] = updatedDelegation;
    return updatedDelegation;
  }

  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _delegations.length;
    _delegations.removeWhere((d) => d.id == id);
    if (_delegations.length == initialLength) {
      throw Exception('Delegation not found');
    }
  }

  static List<TaskDelegationDto> _generateMockDelegations() {
    final now = DateTime.now();
    return [
      TaskDelegationDto(
        id: 'delegation-1',
        taskId: '2',
        delegatedFrom: 'user-1',
        delegatedFromName: 'John Doe',
        delegatedTo: 'user-2',
        delegatedToName: 'Jane Smith',
        reason: 'Better suited for frontend expertise',
        delegationDate: now.subtract(const Duration(days: 2)),
        acceptanceStatus: DelegationStatus.accepted,
        responseDate: now.subtract(const Duration(days: 1)),
        responseComment: 'Happy to take this on',
      ),
      TaskDelegationDto(
        id: 'delegation-2',
        taskId: '5',
        delegatedFrom: 'user-3',
        delegatedFromName: 'Bob Wilson',
        delegatedTo: 'user-1',
        delegatedToName: 'Current User',
        reason: 'Need help with API documentation',
        delegationDate: now.subtract(const Duration(hours: 8)),
        acceptanceStatus: DelegationStatus.pending,
      ),
      TaskDelegationDto(
        id: 'delegation-3',
        taskId: '7',
        delegatedFrom: 'user-2',
        delegatedFromName: 'Jane Smith',
        delegatedTo: 'user-3',
        delegatedToName: 'Bob Wilson',
        reason: 'Requires research expertise',
        delegationDate: now.subtract(const Duration(days: 4)),
        acceptanceStatus: DelegationStatus.rejected,
        responseDate: now.subtract(const Duration(days: 3)),
        responseComment: 'Currently overloaded with other tasks',
      ),
    ];
  }
}
