// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'time_block_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_RecurrencePatternDto _$RecurrencePatternDtoFromJson(
  Map<String, dynamic> json,
) => _RecurrencePatternDto(
  frequency: $enumDecode(_$RecurrenceFrequencyEnumMap, json['frequency']),
  interval: (json['interval'] as num).toInt(),
  daysOfWeek: (json['daysOfWeek'] as List<dynamic>?)
      ?.map((e) => (e as num).toInt())
      .toList(),
  dayOfMonth: (json['dayOfMonth'] as num?)?.toInt(),
  endDate: json['endDate'] == null
      ? null
      : DateTime.parse(json['endDate'] as String),
  occurrences: (json['occurrences'] as num?)?.toInt(),
);

Map<String, dynamic> _$RecurrencePatternDtoToJson(
  _RecurrencePatternDto instance,
) => <String, dynamic>{
  'frequency': _$RecurrenceFrequencyEnumMap[instance.frequency]!,
  'interval': instance.interval,
  'daysOfWeek': instance.daysOfWeek,
  'dayOfMonth': instance.dayOfMonth,
  'endDate': instance.endDate?.toIso8601String(),
  'occurrences': instance.occurrences,
};

const _$RecurrenceFrequencyEnumMap = {
  RecurrenceFrequency.daily: 'daily',
  RecurrenceFrequency.weekly: 'weekly',
  RecurrenceFrequency.monthly: 'monthly',
  RecurrenceFrequency.yearly: 'yearly',
};

_TimeBlockDto _$TimeBlockDtoFromJson(Map<String, dynamic> json) =>
    _TimeBlockDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      title: json['title'] as String,
      description: json['description'] as String?,
      startDateTime: DateTime.parse(json['startDateTime'] as String),
      endDateTime: DateTime.parse(json['endDateTime'] as String),
      blockType: $enumDecode(_$TimeBlockTypeEnumMap, json['blockType']),
      color: json['color'] as String,
      isRecurring: json['isRecurring'] as bool? ?? false,
      recurrencePattern: json['recurrencePattern'] == null
          ? null
          : RecurrencePatternDto.fromJson(
              json['recurrencePattern'] as Map<String, dynamic>,
            ),
      taskId: json['taskId'] as String?,
      projectId: json['projectId'] as String?,
      location: json['location'] as String?,
      isFlexible: json['isFlexible'] as bool? ?? false,
      reminderMinutes: (json['reminderMinutes'] as num?)?.toInt(),
      actualStartTime: json['actualStartTime'] == null
          ? null
          : DateTime.parse(json['actualStartTime'] as String),
      actualEndTime: json['actualEndTime'] == null
          ? null
          : DateTime.parse(json['actualEndTime'] as String),
      status: $enumDecode(_$TimeBlockStatusEnumMap, json['status']),
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$TimeBlockDtoToJson(_TimeBlockDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'title': instance.title,
      'description': instance.description,
      'startDateTime': instance.startDateTime.toIso8601String(),
      'endDateTime': instance.endDateTime.toIso8601String(),
      'blockType': _$TimeBlockTypeEnumMap[instance.blockType]!,
      'color': instance.color,
      'isRecurring': instance.isRecurring,
      'recurrencePattern': instance.recurrencePattern,
      'taskId': instance.taskId,
      'projectId': instance.projectId,
      'location': instance.location,
      'isFlexible': instance.isFlexible,
      'reminderMinutes': instance.reminderMinutes,
      'actualStartTime': instance.actualStartTime?.toIso8601String(),
      'actualEndTime': instance.actualEndTime?.toIso8601String(),
      'status': _$TimeBlockStatusEnumMap[instance.status]!,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$TimeBlockTypeEnumMap = {
  TimeBlockType.work: 'work',
  TimeBlockType.personal: 'personal',
  TimeBlockType.breakTime: 'breakTime',
  TimeBlockType.meeting: 'meeting',
  TimeBlockType.focus: 'focus',
  TimeBlockType.admin: 'admin',
  TimeBlockType.learning: 'learning',
};

const _$TimeBlockStatusEnumMap = {
  TimeBlockStatus.scheduled: 'scheduled',
  TimeBlockStatus.inProgress: 'inProgress',
  TimeBlockStatus.completed: 'completed',
  TimeBlockStatus.cancelled: 'cancelled',
};

_CreateUpdateTimeBlockDto _$CreateUpdateTimeBlockDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateTimeBlockDto(
  userId: json['userId'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  startDateTime: DateTime.parse(json['startDateTime'] as String),
  endDateTime: DateTime.parse(json['endDateTime'] as String),
  blockType: $enumDecode(_$TimeBlockTypeEnumMap, json['blockType']),
  color: json['color'] as String? ?? '#2196F3',
  isRecurring: json['isRecurring'] as bool? ?? false,
  recurrencePattern: json['recurrencePattern'] == null
      ? null
      : RecurrencePatternDto.fromJson(
          json['recurrencePattern'] as Map<String, dynamic>,
        ),
  taskId: json['taskId'] as String?,
  projectId: json['projectId'] as String?,
  location: json['location'] as String?,
  isFlexible: json['isFlexible'] as bool? ?? false,
  reminderMinutes: (json['reminderMinutes'] as num?)?.toInt(),
);

Map<String, dynamic> _$CreateUpdateTimeBlockDtoToJson(
  _CreateUpdateTimeBlockDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'title': instance.title,
  'description': instance.description,
  'startDateTime': instance.startDateTime.toIso8601String(),
  'endDateTime': instance.endDateTime.toIso8601String(),
  'blockType': _$TimeBlockTypeEnumMap[instance.blockType]!,
  'color': instance.color,
  'isRecurring': instance.isRecurring,
  'recurrencePattern': instance.recurrencePattern,
  'taskId': instance.taskId,
  'projectId': instance.projectId,
  'location': instance.location,
  'isFlexible': instance.isFlexible,
  'reminderMinutes': instance.reminderMinutes,
};
