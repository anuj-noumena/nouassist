import 'package:freezed_annotation/freezed_annotation.dart';

part 'user_working_schedule_dto.freezed.dart';
part 'user_working_schedule_dto.g.dart';

/// Day of week enum
enum DayOfWeek {
  monday,
  tuesday,
  wednesday,
  thursday,
  friday,
  saturday,
  sunday;

  String get displayName {
    switch (this) {
      case DayOfWeek.monday:
        return 'Monday';
      case DayOfWeek.tuesday:
        return 'Tuesday';
      case DayOfWeek.wednesday:
        return 'Wednesday';
      case DayOfWeek.thursday:
        return 'Thursday';
      case DayOfWeek.friday:
        return 'Friday';
      case DayOfWeek.saturday:
        return 'Saturday';
      case DayOfWeek.sunday:
        return 'Sunday';
    }
  }
}

/// User working schedule DTO
@freezed
abstract class UserWorkingScheduleDto with _$UserWorkingScheduleDto {
  const factory UserWorkingScheduleDto({
    required String id,
    required String userId,
    required DayOfWeek dayOfWeek,
    @Default(true) bool isWorkingDay,
    String? startTime,
    String? endTime,
    String? breakStartTime,
    String? breakEndTime,
    required String timezone,
    required DateTime effectiveFrom,
    DateTime? effectiveTo,
    @Default(false) bool isDefault,
  }) = _UserWorkingScheduleDto;

  factory UserWorkingScheduleDto.fromJson(Map<String, dynamic> json) =>
      _$UserWorkingScheduleDtoFromJson(json);
}

/// Create/Update working schedule DTO
@freezed
abstract class CreateUpdateWorkingScheduleDto with _$CreateUpdateWorkingScheduleDto {
  const factory CreateUpdateWorkingScheduleDto({
    required String userId,
    required DayOfWeek dayOfWeek,
    @Default(true) bool isWorkingDay,
    String? startTime,
    String? endTime,
    String? breakStartTime,
    String? breakEndTime,
    required String timezone,
    required DateTime effectiveFrom,
    DateTime? effectiveTo,
    @Default(false) bool isDefault,
  }) = _CreateUpdateWorkingScheduleDto;

  factory CreateUpdateWorkingScheduleDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateWorkingScheduleDtoFromJson(json);
}
