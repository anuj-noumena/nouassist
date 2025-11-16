import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_event_dto.freezed.dart';
part 'project_event_dto.g.dart';

/// Event type enum
enum ProjectEventType {
  meeting,
  milestone,
  deadline,
  review,
  demo;

  String get displayName {
    switch (this) {
      case ProjectEventType.meeting:
        return 'Meeting';
      case ProjectEventType.milestone:
        return 'Milestone';
      case ProjectEventType.deadline:
        return 'Deadline';
      case ProjectEventType.review:
        return 'Review';
      case ProjectEventType.demo:
        return 'Demo';
    }
  }
}

/// Project event DTO
@freezed
abstract class ProjectEventDto with _$ProjectEventDto {
  const factory ProjectEventDto({
    required String id,
    required String projectId,
    required String title,
    String? description,
    required ProjectEventType eventType,
    required DateTime startDateTime,
    required DateTime endDateTime,
    String? location,
    @Default(false) bool isAllDay,
    @Default([]) List<String> attendees,
    int? reminderMinutes,
    String? meetingLink,
    required DateTime creationTime,
  }) = _ProjectEventDto;

  factory ProjectEventDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectEventDtoFromJson(json);
}

/// Create/Update event DTO
@freezed
abstract class CreateUpdateEventDto with _$CreateUpdateEventDto {
  const factory CreateUpdateEventDto({
    required String projectId,
    required String title,
    String? description,
    required ProjectEventType eventType,
    required DateTime startDateTime,
    required DateTime endDateTime,
    String? location,
    @Default(false) bool isAllDay,
    @Default([]) List<String> attendees,
    int? reminderMinutes,
    String? meetingLink,
  }) = _CreateUpdateEventDto;

  factory CreateUpdateEventDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateEventDtoFromJson(json);
}
