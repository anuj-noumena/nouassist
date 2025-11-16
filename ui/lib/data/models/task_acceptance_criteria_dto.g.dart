// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_acceptance_criteria_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskAcceptanceCriteriaDto _$TaskAcceptanceCriteriaDtoFromJson(
  Map<String, dynamic> json,
) => _TaskAcceptanceCriteriaDto(
  id: json['id'] as String,
  taskId: json['taskId'] as String,
  criterion: json['criterion'] as String,
  isCompleted: json['isCompleted'] as bool? ?? false,
  order: (json['order'] as num).toInt(),
  creationTime: DateTime.parse(json['creationTime'] as String),
  completionTime: json['completionTime'] == null
      ? null
      : DateTime.parse(json['completionTime'] as String),
);

Map<String, dynamic> _$TaskAcceptanceCriteriaDtoToJson(
  _TaskAcceptanceCriteriaDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'taskId': instance.taskId,
  'criterion': instance.criterion,
  'isCompleted': instance.isCompleted,
  'order': instance.order,
  'creationTime': instance.creationTime.toIso8601String(),
  'completionTime': instance.completionTime?.toIso8601String(),
};

_CreateUpdateCriteriaDto _$CreateUpdateCriteriaDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateCriteriaDto(
  taskId: json['taskId'] as String,
  criterion: json['criterion'] as String,
  isCompleted: json['isCompleted'] as bool? ?? false,
  order: (json['order'] as num).toInt(),
);

Map<String, dynamic> _$CreateUpdateCriteriaDtoToJson(
  _CreateUpdateCriteriaDto instance,
) => <String, dynamic>{
  'taskId': instance.taskId,
  'criterion': instance.criterion,
  'isCompleted': instance.isCompleted,
  'order': instance.order,
};
