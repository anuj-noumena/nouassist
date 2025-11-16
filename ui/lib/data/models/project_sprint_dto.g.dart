// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_sprint_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectSprintDto _$ProjectSprintDtoFromJson(Map<String, dynamic> json) =>
    _ProjectSprintDto(
      id: json['id'] as String,
      projectId: json['projectId'] as String,
      name: json['name'] as String,
      goal: json['goal'] as String?,
      startDate: DateTime.parse(json['startDate'] as String),
      endDate: DateTime.parse(json['endDate'] as String),
      status: $enumDecode(_$SprintStatusEnumMap, json['status']),
      velocity: (json['velocity'] as num?)?.toInt(),
      capacity: (json['capacity'] as num?)?.toInt(),
      actualHours: (json['actualHours'] as num?)?.toDouble() ?? 0.0,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$ProjectSprintDtoToJson(_ProjectSprintDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'projectId': instance.projectId,
      'name': instance.name,
      'goal': instance.goal,
      'startDate': instance.startDate.toIso8601String(),
      'endDate': instance.endDate.toIso8601String(),
      'status': _$SprintStatusEnumMap[instance.status]!,
      'velocity': instance.velocity,
      'capacity': instance.capacity,
      'actualHours': instance.actualHours,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$SprintStatusEnumMap = {
  SprintStatus.planning: 'planning',
  SprintStatus.active: 'active',
  SprintStatus.completed: 'completed',
  SprintStatus.cancelled: 'cancelled',
};

_CreateUpdateSprintDto _$CreateUpdateSprintDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateSprintDto(
  projectId: json['projectId'] as String,
  name: json['name'] as String,
  goal: json['goal'] as String?,
  startDate: DateTime.parse(json['startDate'] as String),
  endDate: DateTime.parse(json['endDate'] as String),
  velocity: (json['velocity'] as num?)?.toInt(),
  capacity: (json['capacity'] as num?)?.toInt(),
);

Map<String, dynamic> _$CreateUpdateSprintDtoToJson(
  _CreateUpdateSprintDto instance,
) => <String, dynamic>{
  'projectId': instance.projectId,
  'name': instance.name,
  'goal': instance.goal,
  'startDate': instance.startDate.toIso8601String(),
  'endDate': instance.endDate.toIso8601String(),
  'velocity': instance.velocity,
  'capacity': instance.capacity,
};
