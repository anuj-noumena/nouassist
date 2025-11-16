// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_dependency_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskDependencyDto _$TaskDependencyDtoFromJson(Map<String, dynamic> json) =>
    _TaskDependencyDto(
      id: json['id'] as String,
      predecessorTaskId: json['predecessorTaskId'] as String,
      predecessorTaskTitle: json['predecessorTaskTitle'] as String,
      successorTaskId: json['successorTaskId'] as String,
      successorTaskTitle: json['successorTaskTitle'] as String,
      dependencyType: $enumDecode(
        _$DependencyTypeEnumMap,
        json['dependencyType'],
      ),
      lagDays: (json['lagDays'] as num?)?.toInt() ?? 0,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$TaskDependencyDtoToJson(_TaskDependencyDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'predecessorTaskId': instance.predecessorTaskId,
      'predecessorTaskTitle': instance.predecessorTaskTitle,
      'successorTaskId': instance.successorTaskId,
      'successorTaskTitle': instance.successorTaskTitle,
      'dependencyType': _$DependencyTypeEnumMap[instance.dependencyType]!,
      'lagDays': instance.lagDays,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$DependencyTypeEnumMap = {
  DependencyType.finishToStart: 'finishToStart',
  DependencyType.startToStart: 'startToStart',
  DependencyType.finishToFinish: 'finishToFinish',
  DependencyType.startToFinish: 'startToFinish',
};

_CreateDependencyDto _$CreateDependencyDtoFromJson(Map<String, dynamic> json) =>
    _CreateDependencyDto(
      predecessorTaskId: json['predecessorTaskId'] as String,
      successorTaskId: json['successorTaskId'] as String,
      dependencyType: $enumDecode(
        _$DependencyTypeEnumMap,
        json['dependencyType'],
      ),
      lagDays: (json['lagDays'] as num?)?.toInt() ?? 0,
    );

Map<String, dynamic> _$CreateDependencyDtoToJson(
  _CreateDependencyDto instance,
) => <String, dynamic>{
  'predecessorTaskId': instance.predecessorTaskId,
  'successorTaskId': instance.successorTaskId,
  'dependencyType': _$DependencyTypeEnumMap[instance.dependencyType]!,
  'lagDays': instance.lagDays,
};
