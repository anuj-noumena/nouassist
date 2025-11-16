// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_checklist_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ChecklistItemDto _$ChecklistItemDtoFromJson(Map<String, dynamic> json) =>
    _ChecklistItemDto(
      id: json['id'] as String,
      text: json['text'] as String,
      isCompleted: json['isCompleted'] as bool? ?? false,
      order: (json['order'] as num).toInt(),
      completedBy: json['completedBy'] as String?,
      completionTime: json['completionTime'] == null
          ? null
          : DateTime.parse(json['completionTime'] as String),
    );

Map<String, dynamic> _$ChecklistItemDtoToJson(_ChecklistItemDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'text': instance.text,
      'isCompleted': instance.isCompleted,
      'order': instance.order,
      'completedBy': instance.completedBy,
      'completionTime': instance.completionTime?.toIso8601String(),
    };

_TaskChecklistDto _$TaskChecklistDtoFromJson(Map<String, dynamic> json) =>
    _TaskChecklistDto(
      id: json['id'] as String,
      taskId: json['taskId'] as String,
      title: json['title'] as String,
      items: (json['items'] as List<dynamic>)
          .map((e) => ChecklistItemDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      progress: (json['progress'] as num).toDouble(),
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$TaskChecklistDtoToJson(_TaskChecklistDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'taskId': instance.taskId,
      'title': instance.title,
      'items': instance.items,
      'progress': instance.progress,
      'creationTime': instance.creationTime.toIso8601String(),
    };

_CreateUpdateChecklistDto _$CreateUpdateChecklistDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateChecklistDto(
  taskId: json['taskId'] as String,
  title: json['title'] as String,
  items:
      (json['items'] as List<dynamic>?)
          ?.map((e) => ChecklistItemDto.fromJson(e as Map<String, dynamic>))
          .toList() ??
      const [],
);

Map<String, dynamic> _$CreateUpdateChecklistDtoToJson(
  _CreateUpdateChecklistDto instance,
) => <String, dynamic>{
  'taskId': instance.taskId,
  'title': instance.title,
  'items': instance.items,
};
