// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_action_step_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskActionStepDto _$TaskActionStepDtoFromJson(Map<String, dynamic> json) =>
    _TaskActionStepDto(
      id: json['id'] as String,
      taskId: json['taskId'] as String,
      description: json['description'] as String,
      isCompleted: json['isCompleted'] as bool? ?? false,
      assigneeId: json['assigneeId'] as String?,
      assigneeName: json['assigneeName'] as String?,
      dueDate: json['dueDate'] == null
          ? null
          : DateTime.parse(json['dueDate'] as String),
      order: (json['order'] as num).toInt(),
      creationTime: DateTime.parse(json['creationTime'] as String),
      completionTime: json['completionTime'] == null
          ? null
          : DateTime.parse(json['completionTime'] as String),
    );

Map<String, dynamic> _$TaskActionStepDtoToJson(_TaskActionStepDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'taskId': instance.taskId,
      'description': instance.description,
      'isCompleted': instance.isCompleted,
      'assigneeId': instance.assigneeId,
      'assigneeName': instance.assigneeName,
      'dueDate': instance.dueDate?.toIso8601String(),
      'order': instance.order,
      'creationTime': instance.creationTime.toIso8601String(),
      'completionTime': instance.completionTime?.toIso8601String(),
    };

_CreateUpdateActionStepDto _$CreateUpdateActionStepDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateActionStepDto(
  taskId: json['taskId'] as String,
  description: json['description'] as String,
  isCompleted: json['isCompleted'] as bool? ?? false,
  assigneeId: json['assigneeId'] as String?,
  dueDate: json['dueDate'] == null
      ? null
      : DateTime.parse(json['dueDate'] as String),
  order: (json['order'] as num).toInt(),
);

Map<String, dynamic> _$CreateUpdateActionStepDtoToJson(
  _CreateUpdateActionStepDto instance,
) => <String, dynamic>{
  'taskId': instance.taskId,
  'description': instance.description,
  'isCompleted': instance.isCompleted,
  'assigneeId': instance.assigneeId,
  'dueDate': instance.dueDate?.toIso8601String(),
  'order': instance.order,
};
