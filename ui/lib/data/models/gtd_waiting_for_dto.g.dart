// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_waiting_for_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdWaitingForDto _$GtdWaitingForDtoFromJson(Map<String, dynamic> json) =>
    _GtdWaitingForDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      description: json['description'] as String,
      waitingFor: json['waitingFor'] as String,
      notes: json['notes'] as String?,
      projectId: json['projectId'] as String?,
      relatedActionId: json['relatedActionId'] as String?,
      status: $enumDecode(_$WaitingForStatusEnumMap, json['status']),
      requestedDate: json['requestedDate'] == null
          ? null
          : DateTime.parse(json['requestedDate'] as String),
      expectedDate: json['expectedDate'] == null
          ? null
          : DateTime.parse(json['expectedDate'] as String),
      reminderDate: json['reminderDate'] == null
          ? null
          : DateTime.parse(json['reminderDate'] as String),
      receivedDate: json['receivedDate'] == null
          ? null
          : DateTime.parse(json['receivedDate'] as String),
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          const [],
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdWaitingForDtoToJson(_GtdWaitingForDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'description': instance.description,
      'waitingFor': instance.waitingFor,
      'notes': instance.notes,
      'projectId': instance.projectId,
      'relatedActionId': instance.relatedActionId,
      'status': _$WaitingForStatusEnumMap[instance.status]!,
      'requestedDate': instance.requestedDate?.toIso8601String(),
      'expectedDate': instance.expectedDate?.toIso8601String(),
      'reminderDate': instance.reminderDate?.toIso8601String(),
      'receivedDate': instance.receivedDate?.toIso8601String(),
      'tags': instance.tags,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$WaitingForStatusEnumMap = {
  WaitingForStatus.waiting: 'waiting',
  WaitingForStatus.reminded: 'reminded',
  WaitingForStatus.received: 'received',
  WaitingForStatus.cancelled: 'cancelled',
};

_CreateUpdateWaitingForDto _$CreateUpdateWaitingForDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateWaitingForDto(
  userId: json['userId'] as String,
  description: json['description'] as String,
  waitingFor: json['waitingFor'] as String,
  notes: json['notes'] as String?,
  projectId: json['projectId'] as String?,
  relatedActionId: json['relatedActionId'] as String?,
  requestedDate: json['requestedDate'] == null
      ? null
      : DateTime.parse(json['requestedDate'] as String),
  expectedDate: json['expectedDate'] == null
      ? null
      : DateTime.parse(json['expectedDate'] as String),
  reminderDate: json['reminderDate'] == null
      ? null
      : DateTime.parse(json['reminderDate'] as String),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
);

Map<String, dynamic> _$CreateUpdateWaitingForDtoToJson(
  _CreateUpdateWaitingForDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'description': instance.description,
  'waitingFor': instance.waitingFor,
  'notes': instance.notes,
  'projectId': instance.projectId,
  'relatedActionId': instance.relatedActionId,
  'requestedDate': instance.requestedDate?.toIso8601String(),
  'expectedDate': instance.expectedDate?.toIso8601String(),
  'reminderDate': instance.reminderDate?.toIso8601String(),
  'tags': instance.tags,
};
