import 'package:freezed_annotation/freezed_annotation.dart';
import 'time_block_dto.dart';

part 'event_dto.freezed.dart';
part 'event_dto.g.dart';

/// Event type enum
enum EventType {
  meeting,
  appointment,
  reminder,
  deadline,
  personal,
  holiday;

  String get displayName {
    switch (this) {
      case EventType.meeting:
        return 'Meeting';
      case EventType.appointment:
        return 'Appointment';
      case EventType.reminder:
        return 'Reminder';
      case EventType.deadline:
        return 'Deadline';
      case EventType.personal:
        return 'Personal';
      case EventType.holiday:
        return 'Holiday';
    }
  }
}

/// Event visibility enum
enum EventVisibility {
  public,
  private,
  confidential;

  String get displayName {
    switch (this) {
      case EventVisibility.public:
        return 'Public';
      case EventVisibility.private:
        return 'Private';
      case EventVisibility.confidential:
        return 'Confidential';
    }
  }
}

/// Event status enum
enum EventStatus {
  tentative,
  confirmed,
  cancelled;

  String get displayName {
    switch (this) {
      case EventStatus.tentative:
        return 'Tentative';
      case EventStatus.confirmed:
        return 'Confirmed';
      case EventStatus.cancelled:
        return 'Cancelled';
    }
  }
}

/// Event DTO
@freezed
abstract class EventDto with _$EventDto {
  const factory EventDto({
    required String id,
    required String userId,
    required String title,
    String? description,
    required EventType eventType,
    required DateTime startDateTime,
    required DateTime endDateTime,
    @Default(false) bool isAllDay,
    String? location,
    String? meetingLink,
    @Default([]) List<String> attendees,
    @Default(false) bool isRecurring,
    RecurrencePatternDto? recurrencePattern,
    @Default([]) List<int> reminderMinutes,
    required String color,
    required EventVisibility visibility,
    required EventStatus status,
    String? projectId,
    String? taskId,
    String? timeBlockId,
    required DateTime creationTime,
    DateTime? lastModificationTime,
  }) = _EventDto;

  factory EventDto.fromJson(Map<String, dynamic> json) =>
      _$EventDtoFromJson(json);
}

/// Create/Update event DTO
@freezed
abstract class CreateUpdateEventDto with _$CreateUpdateEventDto {
  const factory CreateUpdateEventDto({
    required String userId,
    required String title,
    String? description,
    required EventType eventType,
    required DateTime startDateTime,
    required DateTime endDateTime,
    @Default(false) bool isAllDay,
    String? location,
    String? meetingLink,
    @Default([]) List<String> attendees,
    @Default(false) bool isRecurring,
    RecurrencePatternDto? recurrencePattern,
    @Default([]) List<int> reminderMinutes,
    @Default('#2196F3') String color,
    @Default(EventVisibility.public) EventVisibility visibility,
    @Default(EventStatus.confirmed) EventStatus status,
    String? projectId,
    String? taskId,
    String? timeBlockId,
  }) = _CreateUpdateEventDto;

  factory CreateUpdateEventDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateEventDtoFromJson(json);
}
