import '../models/time_block_dto.dart';

/// Mock time block service
class MockTimeBlockService {
  static final List<TimeBlockDto> _timeBlocks = _generateMockTimeBlocks();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get time blocks by user and date range
  static Future<List<TimeBlockDto>> getByUserAndDateRange({
    required String userId,
    required DateTime startDate,
    required DateTime endDate,
    TimeBlockType? typeFilter,
  }) async {
    await _simulateDelay();

    var filtered = _timeBlocks.where((block) {
      return block.userId == userId &&
          block.startDateTime.isAfter(startDate.subtract(const Duration(days: 1))) &&
          block.startDateTime.isBefore(endDate.add(const Duration(days: 1)));
    }).toList();

    if (typeFilter != null) {
      filtered = filtered.where((block) => block.blockType == typeFilter).toList();
    }

    filtered.sort((a, b) => a.startDateTime.compareTo(b.startDateTime));
    return filtered;
  }

  /// Get time blocks for a specific date
  static Future<List<TimeBlockDto>> getByDate({
    required String userId,
    required DateTime date,
  }) async {
    await _simulateDelay();

    final startOfDay = DateTime(date.year, date.month, date.day);
    final endOfDay = startOfDay.add(const Duration(days: 1));

    return _timeBlocks.where((block) {
      return block.userId == userId &&
          block.startDateTime.isAfter(startOfDay.subtract(const Duration(seconds: 1))) &&
          block.startDateTime.isBefore(endOfDay);
    }).toList()
      ..sort((a, b) => a.startDateTime.compareTo(b.startDateTime));
  }

  /// Get single time block
  static Future<TimeBlockDto> get(String id) async {
    await _simulateDelay();

    try {
      return _timeBlocks.firstWhere((block) => block.id == id);
    } catch (e) {
      throw Exception('Time block not found');
    }
  }

  /// Create time block
  static Future<TimeBlockDto> create(CreateUpdateTimeBlockDto input) async {
    await _simulateDelay();

    if (input.startDateTime.isAfter(input.endDateTime)) {
      throw Exception('End time must be after start time');
    }

    final newBlock = TimeBlockDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      title: input.title,
      description: input.description,
      startDateTime: input.startDateTime,
      endDateTime: input.endDateTime,
      blockType: input.blockType,
      color: input.color,
      isRecurring: input.isRecurring,
      recurrencePattern: input.recurrencePattern,
      taskId: input.taskId,
      projectId: input.projectId,
      location: input.location,
      isFlexible: input.isFlexible,
      reminderMinutes: input.reminderMinutes,
      status: TimeBlockStatus.scheduled,
      creationTime: DateTime.now(),
    );

    _timeBlocks.insert(0, newBlock);
    return newBlock;
  }

  /// Update time block
  static Future<TimeBlockDto> update(String id, CreateUpdateTimeBlockDto input) async {
    await _simulateDelay();

    final index = _timeBlocks.indexWhere((block) => block.id == id);
    if (index == -1) {
      throw Exception('Time block not found');
    }

    if (input.startDateTime.isAfter(input.endDateTime)) {
      throw Exception('End time must be after start time');
    }

    final updatedBlock = _timeBlocks[index].copyWith(
      title: input.title,
      description: input.description,
      startDateTime: input.startDateTime,
      endDateTime: input.endDateTime,
      blockType: input.blockType,
      color: input.color,
      isRecurring: input.isRecurring,
      recurrencePattern: input.recurrencePattern,
      taskId: input.taskId,
      projectId: input.projectId,
      location: input.location,
      isFlexible: input.isFlexible,
      reminderMinutes: input.reminderMinutes,
    );

    _timeBlocks[index] = updatedBlock;
    return updatedBlock;
  }

  /// Delete time block
  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _timeBlocks.length;
    _timeBlocks.removeWhere((block) => block.id == id);
    if (_timeBlocks.length == initialLength) {
      throw Exception('Time block not found');
    }
  }

  /// Update status
  static Future<TimeBlockDto> updateStatus(String id, TimeBlockStatus status) async {
    await _simulateDelay();

    final index = _timeBlocks.indexWhere((block) => block.id == id);
    if (index == -1) {
      throw Exception('Time block not found');
    }

    final updatedBlock = _timeBlocks[index].copyWith(
      status: status,
      actualStartTime: status == TimeBlockStatus.inProgress ? DateTime.now() : null,
      actualEndTime: status == TimeBlockStatus.completed ? DateTime.now() : null,
    );

    _timeBlocks[index] = updatedBlock;
    return updatedBlock;
  }

  /// Generate mock time blocks
  static List<TimeBlockDto> _generateMockTimeBlocks() {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final blocks = <TimeBlockDto>[];

    final colors = {
      TimeBlockType.work: '#2196F3',
      TimeBlockType.personal: '#4CAF50',
      TimeBlockType.breakTime: '#FF9800',
      TimeBlockType.meeting: '#9C27B0',
      TimeBlockType.focus: '#F44336',
      TimeBlockType.admin: '#795548',
      TimeBlockType.learning: '#00BCD4',
    };

    // Generate blocks for the past week
    for (int day = -7; day <= 7; day++) {
      final date = today.add(Duration(days: day));

      // Skip weekends for work blocks
      if (date.weekday == DateTime.saturday || date.weekday == DateTime.sunday) {
        continue;
      }

      // Morning focus time
      blocks.add(TimeBlockDto(
        id: 'block-${blocks.length + 1}',
        userId: 'user-1',
        title: 'Deep Work - Development',
        description: 'Focused coding session',
        startDateTime: date.add(const Duration(hours: 9)),
        endDateTime: date.add(const Duration(hours: 11)),
        blockType: TimeBlockType.focus,
        color: colors[TimeBlockType.focus]!,
        status: day < 0 ? TimeBlockStatus.completed : TimeBlockStatus.scheduled,
        creationTime: date.subtract(const Duration(days: 7)),
      ));

      // Mid-morning meeting
      blocks.add(TimeBlockDto(
        id: 'block-${blocks.length + 1}',
        userId: 'user-1',
        title: 'Team Standup',
        description: 'Daily team sync',
        startDateTime: date.add(const Duration(hours: 11)),
        endDateTime: date.add(const Duration(hours: 11, minutes: 30)),
        blockType: TimeBlockType.meeting,
        color: colors[TimeBlockType.meeting]!,
        location: 'Conference Room A',
        status: day < 0 ? TimeBlockStatus.completed : TimeBlockStatus.scheduled,
        creationTime: date.subtract(const Duration(days: 7)),
      ));

      // Lunch break
      blocks.add(TimeBlockDto(
        id: 'block-${blocks.length + 1}',
        userId: 'user-1',
        title: 'Lunch Break',
        startDateTime: date.add(const Duration(hours: 12)),
        endDateTime: date.add(const Duration(hours: 13)),
        blockType: TimeBlockType.breakTime,
        color: colors[TimeBlockType.breakTime]!,
        status: day < 0 ? TimeBlockStatus.completed : TimeBlockStatus.scheduled,
        creationTime: date.subtract(const Duration(days: 7)),
      ));

      // Afternoon work
      blocks.add(TimeBlockDto(
        id: 'block-${blocks.length + 1}',
        userId: 'user-1',
        title: 'Code Review & Testing',
        description: 'Review pull requests and test features',
        startDateTime: date.add(const Duration(hours: 14)),
        endDateTime: date.add(const Duration(hours: 16)),
        blockType: TimeBlockType.work,
        color: colors[TimeBlockType.work]!,
        taskId: '4',
        status: day < 0 ? TimeBlockStatus.completed : TimeBlockStatus.scheduled,
        creationTime: date.subtract(const Duration(days: 7)),
      ));

      // Admin time
      blocks.add(TimeBlockDto(
        id: 'block-${blocks.length + 1}',
        userId: 'user-1',
        title: 'Email & Administrative Tasks',
        startDateTime: date.add(const Duration(hours: 16)),
        endDateTime: date.add(const Duration(hours: 17)),
        blockType: TimeBlockType.admin,
        color: colors[TimeBlockType.admin]!,
        status: day < 0 ? TimeBlockStatus.completed : TimeBlockStatus.scheduled,
        creationTime: date.subtract(const Duration(days: 7)),
      ));
    }

    // Add some personal blocks for today
    blocks.add(TimeBlockDto(
      id: 'block-${blocks.length + 1}',
      userId: 'user-1',
      title: 'Gym Workout',
      startDateTime: today.add(const Duration(hours: 18)),
      endDateTime: today.add(const Duration(hours: 19)),
      blockType: TimeBlockType.personal,
      color: colors[TimeBlockType.personal]!,
      location: 'Local Gym',
      status: TimeBlockStatus.scheduled,
      creationTime: today.subtract(const Duration(days: 1)),
    ));

    // Add learning block
    blocks.add(TimeBlockDto(
      id: 'block-${blocks.length + 1}',
      userId: 'user-1',
      title: 'Online Course - Flutter Advanced',
      description: 'Continue learning advanced Flutter concepts',
      startDateTime: today.add(const Duration(hours: 20)),
      endDateTime: today.add(const Duration(hours: 21)),
      blockType: TimeBlockType.learning,
      color: colors[TimeBlockType.learning]!,
      status: TimeBlockStatus.scheduled,
      creationTime: today.subtract(const Duration(days: 1)),
    ));

    return blocks;
  }
}
