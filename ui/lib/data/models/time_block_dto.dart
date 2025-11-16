import 'package:freezed_annotation/freezed_annotation.dart';

part 'time_block_dto.freezed.dart';
part 'time_block_dto.g.dart';

/// Time block type enum
enum TimeBlockType {
  work,
  personal,
  breakTime,
  meeting,
  focus,
  admin,
  learning;

  String get displayName {
    switch (this) {
      case TimeBlockType.work:
        return 'Work';
      case TimeBlockType.personal:
        return 'Personal';
      case TimeBlockType.breakTime:
        return 'Break';
      case TimeBlockType.meeting:
        return 'Meeting';
      case TimeBlockType.focus:
        return 'Focus Time';
      case TimeBlockType.admin:
        return 'Admin';
      case TimeBlockType.learning:
        return 'Learning';
    }
  }
}

/// Time block status enum
enum TimeBlockStatus {
  scheduled,
  inProgress,
  completed,
  cancelled;

  String get displayName {
    switch (this) {
      case TimeBlockStatus.scheduled:
        return 'Scheduled';
      case TimeBlockStatus.inProgress:
        return 'In Progress';
      case TimeBlockStatus.completed:
        return 'Completed';
      case TimeBlockStatus.cancelled:
        return 'Cancelled';
    }
  }
}

/// Recurrence frequency enum
enum RecurrenceFrequency {
  daily,
  weekly,
  monthly,
  yearly;

  String get displayName {
    switch (this) {
      case RecurrenceFrequency.daily:
        return 'Daily';
      case RecurrenceFrequency.weekly:
        return 'Weekly';
      case RecurrenceFrequency.monthly:
        return 'Monthly';
      case RecurrenceFrequency.yearly:
        return 'Yearly';
    }
  }
}

/// Recurrence pattern DTO
@freezed
abstract class RecurrencePatternDto with _$RecurrencePatternDto {
  const factory RecurrencePatternDto({
    required RecurrenceFrequency frequency,
    required int interval,
    List<int>? daysOfWeek,
    int? dayOfMonth,
    DateTime? endDate,
    int? occurrences,
  }) = _RecurrencePatternDto;

  factory RecurrencePatternDto.fromJson(Map<String, dynamic> json) =>
      _$RecurrencePatternDtoFromJson(json);
}

/// Time block DTO
@freezed
abstract class TimeBlockDto with _$TimeBlockDto {
  const factory TimeBlockDto({
    required String id,
    required String userId,
    required String title,
    String? description,
    required DateTime startDateTime,
    required DateTime endDateTime,
    required TimeBlockType blockType,
    required String color,
    @Default(false) bool isRecurring,
    RecurrencePatternDto? recurrencePattern,
    String? taskId,
    String? projectId,
    String? location,
    @Default(false) bool isFlexible,
    int? reminderMinutes,
    DateTime? actualStartTime,
    DateTime? actualEndTime,
    required TimeBlockStatus status,
    required DateTime creationTime,
  }) = _TimeBlockDto;

  factory TimeBlockDto.fromJson(Map<String, dynamic> json) =>
      _$TimeBlockDtoFromJson(json);
}

/// Create/Update time block DTO
@freezed
abstract class CreateUpdateTimeBlockDto with _$CreateUpdateTimeBlockDto {
  const factory CreateUpdateTimeBlockDto({
    required String userId,
    required String title,
    String? description,
    required DateTime startDateTime,
    required DateTime endDateTime,
    required TimeBlockType blockType,
    @Default('#2196F3') String color,
    @Default(false) bool isRecurring,
    RecurrencePatternDto? recurrencePattern,
    String? taskId,
    String? projectId,
    String? location,
    @Default(false) bool isFlexible,
    int? reminderMinutes,
  }) = _CreateUpdateTimeBlockDto;

  factory CreateUpdateTimeBlockDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateTimeBlockDtoFromJson(json);
}
