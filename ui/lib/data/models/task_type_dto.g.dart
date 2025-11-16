// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_type_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskTypeDto _$TaskTypeDtoFromJson(Map<String, dynamic> json) => _TaskTypeDto(
  id: json['id'] as String,
  name: json['name'] as String,
  icon: json['icon'] as String,
  color: json['color'] as String,
  description: json['description'] as String?,
  defaultWorkflowId: json['defaultWorkflowId'] as String,
  isActive: json['isActive'] as bool? ?? true,
  creationTime: DateTime.parse(json['creationTime'] as String),
);

Map<String, dynamic> _$TaskTypeDtoToJson(_TaskTypeDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'icon': instance.icon,
      'color': instance.color,
      'description': instance.description,
      'defaultWorkflowId': instance.defaultWorkflowId,
      'isActive': instance.isActive,
      'creationTime': instance.creationTime.toIso8601String(),
    };

_CreateUpdateTaskTypeDto _$CreateUpdateTaskTypeDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateTaskTypeDto(
  name: json['name'] as String,
  icon: json['icon'] as String,
  color: json['color'] as String,
  description: json['description'] as String?,
  defaultWorkflowId: json['defaultWorkflowId'] as String,
  isActive: json['isActive'] as bool? ?? true,
);

Map<String, dynamic> _$CreateUpdateTaskTypeDtoToJson(
  _CreateUpdateTaskTypeDto instance,
) => <String, dynamic>{
  'name': instance.name,
  'icon': instance.icon,
  'color': instance.color,
  'description': instance.description,
  'defaultWorkflowId': instance.defaultWorkflowId,
  'isActive': instance.isActive,
};
