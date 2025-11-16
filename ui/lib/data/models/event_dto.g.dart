// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'event_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_EventDto _$EventDtoFromJson(Map<String, dynamic> json) => _EventDto(
  id: json['id'] as String,
  userId: json['userId'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  eventType: $enumDecode(_$EventTypeEnumMap, json['eventType']),
  startDateTime: DateTime.parse(json['startDateTime'] as String),
  endDateTime: DateTime.parse(json['endDateTime'] as String),
  isAllDay: json['isAllDay'] as bool? ?? false,
  location: json['location'] as String?,
  meetingLink: json['meetingLink'] as String?,
  attendees:
      (json['attendees'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  isRecurring: json['isRecurring'] as bool? ?? false,
  recurrencePattern: json['recurrencePattern'] == null
      ? null
      : RecurrencePatternDto.fromJson(
          json['recurrencePattern'] as Map<String, dynamic>,
        ),
  reminderMinutes:
      (json['reminderMinutes'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      const [],
  color: json['color'] as String,
  visibility: $enumDecode(_$EventVisibilityEnumMap, json['visibility']),
  status: $enumDecode(_$EventStatusEnumMap, json['status']),
  projectId: json['projectId'] as String?,
  taskId: json['taskId'] as String?,
  timeBlockId: json['timeBlockId'] as String?,
  creationTime: DateTime.parse(json['creationTime'] as String),
  lastModificationTime: json['lastModificationTime'] == null
      ? null
      : DateTime.parse(json['lastModificationTime'] as String),
);

Map<String, dynamic> _$EventDtoToJson(_EventDto instance) => <String, dynamic>{
  'id': instance.id,
  'userId': instance.userId,
  'title': instance.title,
  'description': instance.description,
  'eventType': _$EventTypeEnumMap[instance.eventType]!,
  'startDateTime': instance.startDateTime.toIso8601String(),
  'endDateTime': instance.endDateTime.toIso8601String(),
  'isAllDay': instance.isAllDay,
  'location': instance.location,
  'meetingLink': instance.meetingLink,
  'attendees': instance.attendees,
  'isRecurring': instance.isRecurring,
  'recurrencePattern': instance.recurrencePattern,
  'reminderMinutes': instance.reminderMinutes,
  'color': instance.color,
  'visibility': _$EventVisibilityEnumMap[instance.visibility]!,
  'status': _$EventStatusEnumMap[instance.status]!,
  'projectId': instance.projectId,
  'taskId': instance.taskId,
  'timeBlockId': instance.timeBlockId,
  'creationTime': instance.creationTime.toIso8601String(),
  'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
};

const _$EventTypeEnumMap = {
  EventType.meeting: 'meeting',
  EventType.appointment: 'appointment',
  EventType.reminder: 'reminder',
  EventType.deadline: 'deadline',
  EventType.personal: 'personal',
  EventType.holiday: 'holiday',
};

const _$EventVisibilityEnumMap = {
  EventVisibility.public: 'public',
  EventVisibility.private: 'private',
  EventVisibility.confidential: 'confidential',
};

const _$EventStatusEnumMap = {
  EventStatus.tentative: 'tentative',
  EventStatus.confirmed: 'confirmed',
  EventStatus.cancelled: 'cancelled',
};

_CreateUpdateEventDto _$CreateUpdateEventDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateEventDto(
  userId: json['userId'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  eventType: $enumDecode(_$EventTypeEnumMap, json['eventType']),
  startDateTime: DateTime.parse(json['startDateTime'] as String),
  endDateTime: DateTime.parse(json['endDateTime'] as String),
  isAllDay: json['isAllDay'] as bool? ?? false,
  location: json['location'] as String?,
  meetingLink: json['meetingLink'] as String?,
  attendees:
      (json['attendees'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  isRecurring: json['isRecurring'] as bool? ?? false,
  recurrencePattern: json['recurrencePattern'] == null
      ? null
      : RecurrencePatternDto.fromJson(
          json['recurrencePattern'] as Map<String, dynamic>,
        ),
  reminderMinutes:
      (json['reminderMinutes'] as List<dynamic>?)
          ?.map((e) => (e as num).toInt())
          .toList() ??
      const [],
  color: json['color'] as String? ?? '#2196F3',
  visibility:
      $enumDecodeNullable(_$EventVisibilityEnumMap, json['visibility']) ??
      EventVisibility.public,
  status:
      $enumDecodeNullable(_$EventStatusEnumMap, json['status']) ??
      EventStatus.confirmed,
  projectId: json['projectId'] as String?,
  taskId: json['taskId'] as String?,
  timeBlockId: json['timeBlockId'] as String?,
);

Map<String, dynamic> _$CreateUpdateEventDtoToJson(
  _CreateUpdateEventDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'title': instance.title,
  'description': instance.description,
  'eventType': _$EventTypeEnumMap[instance.eventType]!,
  'startDateTime': instance.startDateTime.toIso8601String(),
  'endDateTime': instance.endDateTime.toIso8601String(),
  'isAllDay': instance.isAllDay,
  'location': instance.location,
  'meetingLink': instance.meetingLink,
  'attendees': instance.attendees,
  'isRecurring': instance.isRecurring,
  'recurrencePattern': instance.recurrencePattern,
  'reminderMinutes': instance.reminderMinutes,
  'color': instance.color,
  'visibility': _$EventVisibilityEnumMap[instance.visibility]!,
  'status': _$EventStatusEnumMap[instance.status]!,
  'projectId': instance.projectId,
  'taskId': instance.taskId,
  'timeBlockId': instance.timeBlockId,
};
