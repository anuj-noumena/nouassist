// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_next_action_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdNextActionDto _$GtdNextActionDtoFromJson(Map<String, dynamic> json) =>
    _GtdNextActionDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      action: json['action'] as String,
      notes: json['notes'] as String?,
      contextIds: (json['contextIds'] as List<dynamic>)
          .map((e) => e as String)
          .toList(),
      projectId: json['projectId'] as String?,
      areaOfFocusId: json['areaOfFocusId'] as String?,
      energyLevel: $enumDecode(_$EnergyLevelEnumMap, json['energyLevel']),
      timeRequired: $enumDecode(_$TimeRequiredEnumMap, json['timeRequired']),
      dueDate: json['dueDate'] == null
          ? null
          : DateTime.parse(json['dueDate'] as String),
      scheduledDate: json['scheduledDate'] == null
          ? null
          : DateTime.parse(json['scheduledDate'] as String),
      isCompleted: json['isCompleted'] as bool? ?? false,
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      isStarred: json['isStarred'] as bool? ?? false,
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          const [],
      outcome: json['outcome'] as String?,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdNextActionDtoToJson(_GtdNextActionDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'action': instance.action,
      'notes': instance.notes,
      'contextIds': instance.contextIds,
      'projectId': instance.projectId,
      'areaOfFocusId': instance.areaOfFocusId,
      'energyLevel': _$EnergyLevelEnumMap[instance.energyLevel]!,
      'timeRequired': _$TimeRequiredEnumMap[instance.timeRequired]!,
      'dueDate': instance.dueDate?.toIso8601String(),
      'scheduledDate': instance.scheduledDate?.toIso8601String(),
      'isCompleted': instance.isCompleted,
      'completedDate': instance.completedDate?.toIso8601String(),
      'isStarred': instance.isStarred,
      'tags': instance.tags,
      'outcome': instance.outcome,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$EnergyLevelEnumMap = {
  EnergyLevel.high: 'high',
  EnergyLevel.medium: 'medium',
  EnergyLevel.low: 'low',
};

const _$TimeRequiredEnumMap = {
  TimeRequired.short: 'short',
  TimeRequired.medium: 'medium',
  TimeRequired.long: 'long',
};

_CreateUpdateNextActionDto _$CreateUpdateNextActionDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateNextActionDto(
  userId: json['userId'] as String,
  action: json['action'] as String,
  notes: json['notes'] as String?,
  contextIds:
      (json['contextIds'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  projectId: json['projectId'] as String?,
  areaOfFocusId: json['areaOfFocusId'] as String?,
  energyLevel: $enumDecode(_$EnergyLevelEnumMap, json['energyLevel']),
  timeRequired: $enumDecode(_$TimeRequiredEnumMap, json['timeRequired']),
  dueDate: json['dueDate'] == null
      ? null
      : DateTime.parse(json['dueDate'] as String),
  scheduledDate: json['scheduledDate'] == null
      ? null
      : DateTime.parse(json['scheduledDate'] as String),
  isStarred: json['isStarred'] as bool? ?? false,
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  outcome: json['outcome'] as String?,
);

Map<String, dynamic> _$CreateUpdateNextActionDtoToJson(
  _CreateUpdateNextActionDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'action': instance.action,
  'notes': instance.notes,
  'contextIds': instance.contextIds,
  'projectId': instance.projectId,
  'areaOfFocusId': instance.areaOfFocusId,
  'energyLevel': _$EnergyLevelEnumMap[instance.energyLevel]!,
  'timeRequired': _$TimeRequiredEnumMap[instance.timeRequired]!,
  'dueDate': instance.dueDate?.toIso8601String(),
  'scheduledDate': instance.scheduledDate?.toIso8601String(),
  'isStarred': instance.isStarred,
  'tags': instance.tags,
  'outcome': instance.outcome,
};
