import '../models/user_working_schedule_dto.dart';

/// Mock working schedule service
class MockWorkingScheduleService {
  static final List<UserWorkingScheduleDto> _schedules = _generateMockSchedules();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get working schedule by user
  static Future<List<UserWorkingScheduleDto>> getByUser(String userId) async {
    await _simulateDelay();
    return _schedules.where((s) => s.userId == userId).toList()
      ..sort((a, b) => a.dayOfWeek.index.compareTo(b.dayOfWeek.index));
  }

  /// Get working schedule for specific day
  static Future<UserWorkingScheduleDto?> getByUserAndDay({
    required String userId,
    required DayOfWeek dayOfWeek,
  }) async {
    await _simulateDelay();

    try {
      return _schedules.firstWhere(
        (s) => s.userId == userId && s.dayOfWeek == dayOfWeek,
      );
    } catch (e) {
      return null;
    }
  }

  /// Create or update working schedule
  static Future<UserWorkingScheduleDto> createOrUpdate(
    CreateUpdateWorkingScheduleDto input,
  ) async {
    await _simulateDelay();

    final existingIndex = _schedules.indexWhere(
      (s) => s.userId == input.userId && s.dayOfWeek == input.dayOfWeek,
    );

    if (existingIndex != -1) {
      final updated = _schedules[existingIndex].copyWith(
        isWorkingDay: input.isWorkingDay,
        startTime: input.startTime,
        endTime: input.endTime,
        breakStartTime: input.breakStartTime,
        breakEndTime: input.breakEndTime,
        timezone: input.timezone,
        effectiveFrom: input.effectiveFrom,
        effectiveTo: input.effectiveTo,
        isDefault: input.isDefault,
      );

      _schedules[existingIndex] = updated;
      return updated;
    } else {
      final newSchedule = UserWorkingScheduleDto(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        userId: input.userId,
        dayOfWeek: input.dayOfWeek,
        isWorkingDay: input.isWorkingDay,
        startTime: input.startTime,
        endTime: input.endTime,
        breakStartTime: input.breakStartTime,
        breakEndTime: input.breakEndTime,
        timezone: input.timezone,
        effectiveFrom: input.effectiveFrom,
        effectiveTo: input.effectiveTo,
        isDefault: input.isDefault,
      );

      _schedules.add(newSchedule);
      return newSchedule;
    }
  }

  /// Delete working schedule
  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _schedules.length;
    _schedules.removeWhere((s) => s.id == id);
    if (_schedules.length == initialLength) {
      throw Exception('Schedule not found');
    }
  }

  /// Apply default schedule
  static Future<List<UserWorkingScheduleDto>> applyDefaultSchedule(
    String userId,
  ) async {
    await _simulateDelay();

    // Remove existing schedules
    _schedules.removeWhere((s) => s.userId == userId);

    // Create default 9-5 schedule
    final now = DateTime.now();
    final newSchedules = <UserWorkingScheduleDto>[];

    for (final day in DayOfWeek.values) {
      final isWorkingDay = day != DayOfWeek.saturday && day != DayOfWeek.sunday;

      final schedule = UserWorkingScheduleDto(
        id: '${userId}-${day.name}-${DateTime.now().millisecondsSinceEpoch}',
        userId: userId,
        dayOfWeek: day,
        isWorkingDay: isWorkingDay,
        startTime: isWorkingDay ? '09:00' : null,
        endTime: isWorkingDay ? '17:00' : null,
        breakStartTime: isWorkingDay ? '12:00' : null,
        breakEndTime: isWorkingDay ? '13:00' : null,
        timezone: 'UTC',
        effectiveFrom: now,
        isDefault: true,
      );

      _schedules.add(schedule);
      newSchedules.add(schedule);
    }

    return newSchedules;
  }

  /// Generate mock schedules
  static List<UserWorkingScheduleDto> _generateMockSchedules() {
    final now = DateTime.now();
    final schedules = <UserWorkingScheduleDto>[];

    // Create default schedule for user-1
    for (final day in DayOfWeek.values) {
      final isWorkingDay = day != DayOfWeek.saturday && day != DayOfWeek.sunday;

      schedules.add(UserWorkingScheduleDto(
        id: 'schedule-${day.name}',
        userId: 'user-1',
        dayOfWeek: day,
        isWorkingDay: isWorkingDay,
        startTime: isWorkingDay ? '09:00' : null,
        endTime: isWorkingDay ? '17:00' : null,
        breakStartTime: isWorkingDay ? '12:00' : null,
        breakEndTime: isWorkingDay ? '13:00' : null,
        timezone: 'UTC',
        effectiveFrom: now.subtract(const Duration(days: 365)),
        isDefault: true,
      ));
    }

    return schedules;
  }
}
