// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_workflow_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskWorkflowStageDto _$TaskWorkflowStageDtoFromJson(
  Map<String, dynamic> json,
) => _TaskWorkflowStageDto(
  id: json['id'] as String,
  name: json['name'] as String,
  order: (json['order'] as num).toInt(),
  color: json['color'] as String,
  statusCategory: $enumDecode(_$StatusCategoryEnumMap, json['statusCategory']),
  allowedTransitions:
      (json['allowedTransitions'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  requiresComment: json['requiresComment'] as bool? ?? false,
);

Map<String, dynamic> _$TaskWorkflowStageDtoToJson(
  _TaskWorkflowStageDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'name': instance.name,
  'order': instance.order,
  'color': instance.color,
  'statusCategory': _$StatusCategoryEnumMap[instance.statusCategory]!,
  'allowedTransitions': instance.allowedTransitions,
  'requiresComment': instance.requiresComment,
};

const _$StatusCategoryEnumMap = {
  StatusCategory.todo: 'todo',
  StatusCategory.inProgress: 'inProgress',
  StatusCategory.done: 'done',
};

_TaskWorkflowDto _$TaskWorkflowDtoFromJson(Map<String, dynamic> json) =>
    _TaskWorkflowDto(
      id: json['id'] as String,
      name: json['name'] as String,
      description: json['description'] as String?,
      stages: (json['stages'] as List<dynamic>)
          .map((e) => TaskWorkflowStageDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      taskTypeIds:
          (json['taskTypeIds'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList() ??
          const [],
      isDefault: json['isDefault'] as bool? ?? false,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$TaskWorkflowDtoToJson(_TaskWorkflowDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'description': instance.description,
      'stages': instance.stages,
      'taskTypeIds': instance.taskTypeIds,
      'isDefault': instance.isDefault,
      'creationTime': instance.creationTime.toIso8601String(),
    };
