// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_workflow_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_WorkflowStageDto _$WorkflowStageDtoFromJson(Map<String, dynamic> json) =>
    _WorkflowStageDto(
      id: json['id'] as String,
      name: json['name'] as String,
      order: (json['order'] as num).toInt(),
      color: json['color'] as String,
      isInitial: json['isInitial'] as bool? ?? false,
      isFinal: json['isFinal'] as bool? ?? false,
      allowedTransitions:
          (json['allowedTransitions'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList() ??
          const [],
    );

Map<String, dynamic> _$WorkflowStageDtoToJson(_WorkflowStageDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'order': instance.order,
      'color': instance.color,
      'isInitial': instance.isInitial,
      'isFinal': instance.isFinal,
      'allowedTransitions': instance.allowedTransitions,
    };

_ProjectWorkflowDto _$ProjectWorkflowDtoFromJson(Map<String, dynamic> json) =>
    _ProjectWorkflowDto(
      id: json['id'] as String,
      name: json['name'] as String,
      description: json['description'] as String?,
      stages: (json['stages'] as List<dynamic>)
          .map((e) => WorkflowStageDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      isDefault: json['isDefault'] as bool? ?? false,
      projectType: json['projectType'] as String?,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$ProjectWorkflowDtoToJson(_ProjectWorkflowDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'description': instance.description,
      'stages': instance.stages,
      'isDefault': instance.isDefault,
      'projectType': instance.projectType,
      'creationTime': instance.creationTime.toIso8601String(),
    };

_CreateUpdateWorkflowDto _$CreateUpdateWorkflowDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateWorkflowDto(
  name: json['name'] as String,
  description: json['description'] as String?,
  stages: (json['stages'] as List<dynamic>)
      .map((e) => WorkflowStageDto.fromJson(e as Map<String, dynamic>))
      .toList(),
  isDefault: json['isDefault'] as bool? ?? false,
  projectType: json['projectType'] as String?,
);

Map<String, dynamic> _$CreateUpdateWorkflowDtoToJson(
  _CreateUpdateWorkflowDto instance,
) => <String, dynamic>{
  'name': instance.name,
  'description': instance.description,
  'stages': instance.stages,
  'isDefault': instance.isDefault,
  'projectType': instance.projectType,
};
