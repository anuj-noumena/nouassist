// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_event_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectEventDto _$ProjectEventDtoFromJson(Map<String, dynamic> json) =>
    _ProjectEventDto(
      id: json['id'] as String,
      projectId: json['projectId'] as String,
      title: json['title'] as String,
      description: json['description'] as String?,
      eventType: $enumDecode(_$ProjectEventTypeEnumMap, json['eventType']),
      startDateTime: DateTime.parse(json['startDateTime'] as String),
      endDateTime: DateTime.parse(json['endDateTime'] as String),
      location: json['location'] as String?,
      isAllDay: json['isAllDay'] as bool? ?? false,
      attendees:
          (json['attendees'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList() ??
          const [],
      reminderMinutes: (json['reminderMinutes'] as num?)?.toInt(),
      meetingLink: json['meetingLink'] as String?,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$ProjectEventDtoToJson(_ProjectEventDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'projectId': instance.projectId,
      'title': instance.title,
      'description': instance.description,
      'eventType': _$ProjectEventTypeEnumMap[instance.eventType]!,
      'startDateTime': instance.startDateTime.toIso8601String(),
      'endDateTime': instance.endDateTime.toIso8601String(),
      'location': instance.location,
      'isAllDay': instance.isAllDay,
      'attendees': instance.attendees,
      'reminderMinutes': instance.reminderMinutes,
      'meetingLink': instance.meetingLink,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$ProjectEventTypeEnumMap = {
  ProjectEventType.meeting: 'meeting',
  ProjectEventType.milestone: 'milestone',
  ProjectEventType.deadline: 'deadline',
  ProjectEventType.review: 'review',
  ProjectEventType.demo: 'demo',
};

_CreateUpdateEventDto _$CreateUpdateEventDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateEventDto(
  projectId: json['projectId'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  eventType: $enumDecode(_$ProjectEventTypeEnumMap, json['eventType']),
  startDateTime: DateTime.parse(json['startDateTime'] as String),
  endDateTime: DateTime.parse(json['endDateTime'] as String),
  location: json['location'] as String?,
  isAllDay: json['isAllDay'] as bool? ?? false,
  attendees:
      (json['attendees'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  reminderMinutes: (json['reminderMinutes'] as num?)?.toInt(),
  meetingLink: json['meetingLink'] as String?,
);

Map<String, dynamic> _$CreateUpdateEventDtoToJson(
  _CreateUpdateEventDto instance,
) => <String, dynamic>{
  'projectId': instance.projectId,
  'title': instance.title,
  'description': instance.description,
  'eventType': _$ProjectEventTypeEnumMap[instance.eventType]!,
  'startDateTime': instance.startDateTime.toIso8601String(),
  'endDateTime': instance.endDateTime.toIso8601String(),
  'location': instance.location,
  'isAllDay': instance.isAllDay,
  'attendees': instance.attendees,
  'reminderMinutes': instance.reminderMinutes,
  'meetingLink': instance.meetingLink,
};
