// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskDto _$TaskDtoFromJson(Map<String, dynamic> json) => _TaskDto(
  id: json['id'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  parentTaskId: json['parentTaskId'] as String?,
  projectId: json['projectId'] as String?,
  sprintId: json['sprintId'] as String?,
  taskTypeId: json['taskTypeId'] as String,
  workflowStageId: json['workflowStageId'] as String,
  assigneeId: json['assigneeId'] as String?,
  reporterId: json['reporterId'] as String,
  priority: $enumDecode(_$TaskPriorityEnumMap, json['priority']),
  status: $enumDecode(_$TaskStatusEnumMap, json['status']),
  estimatedHours: (json['estimatedHours'] as num?)?.toDouble(),
  actualHours: (json['actualHours'] as num?)?.toDouble(),
  storyPoints: (json['storyPoints'] as num?)?.toInt(),
  startDate: json['startDate'] == null
      ? null
      : DateTime.parse(json['startDate'] as String),
  dueDate: json['dueDate'] == null
      ? null
      : DateTime.parse(json['dueDate'] as String),
  completedDate: json['completedDate'] == null
      ? null
      : DateTime.parse(json['completedDate'] as String),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  watchers:
      (json['watchers'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  isRecurring: json['isRecurring'] as bool? ?? false,
  progress: (json['progress'] as num?)?.toDouble() ?? 0.0,
  creationTime: DateTime.parse(json['creationTime'] as String),
  lastModificationTime: json['lastModificationTime'] == null
      ? null
      : DateTime.parse(json['lastModificationTime'] as String),
);

Map<String, dynamic> _$TaskDtoToJson(_TaskDto instance) => <String, dynamic>{
  'id': instance.id,
  'title': instance.title,
  'description': instance.description,
  'parentTaskId': instance.parentTaskId,
  'projectId': instance.projectId,
  'sprintId': instance.sprintId,
  'taskTypeId': instance.taskTypeId,
  'workflowStageId': instance.workflowStageId,
  'assigneeId': instance.assigneeId,
  'reporterId': instance.reporterId,
  'priority': _$TaskPriorityEnumMap[instance.priority]!,
  'status': _$TaskStatusEnumMap[instance.status]!,
  'estimatedHours': instance.estimatedHours,
  'actualHours': instance.actualHours,
  'storyPoints': instance.storyPoints,
  'startDate': instance.startDate?.toIso8601String(),
  'dueDate': instance.dueDate?.toIso8601String(),
  'completedDate': instance.completedDate?.toIso8601String(),
  'tags': instance.tags,
  'watchers': instance.watchers,
  'isRecurring': instance.isRecurring,
  'progress': instance.progress,
  'creationTime': instance.creationTime.toIso8601String(),
  'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
};

const _$TaskPriorityEnumMap = {
  TaskPriority.lowest: 'lowest',
  TaskPriority.low: 'low',
  TaskPriority.medium: 'medium',
  TaskPriority.high: 'high',
  TaskPriority.highest: 'highest',
};

const _$TaskStatusEnumMap = {
  TaskStatus.todo: 'todo',
  TaskStatus.inProgress: 'inProgress',
  TaskStatus.inReview: 'inReview',
  TaskStatus.done: 'done',
  TaskStatus.blocked: 'blocked',
};

_CreateUpdateTaskDto _$CreateUpdateTaskDtoFromJson(Map<String, dynamic> json) =>
    _CreateUpdateTaskDto(
      title: json['title'] as String,
      description: json['description'] as String?,
      parentTaskId: json['parentTaskId'] as String?,
      projectId: json['projectId'] as String?,
      sprintId: json['sprintId'] as String?,
      taskTypeId: json['taskTypeId'] as String,
      workflowStageId: json['workflowStageId'] as String,
      assigneeId: json['assigneeId'] as String?,
      reporterId: json['reporterId'] as String,
      priority: $enumDecode(_$TaskPriorityEnumMap, json['priority']),
      status: $enumDecode(_$TaskStatusEnumMap, json['status']),
      estimatedHours: (json['estimatedHours'] as num?)?.toDouble(),
      actualHours: (json['actualHours'] as num?)?.toDouble(),
      storyPoints: (json['storyPoints'] as num?)?.toInt(),
      startDate: json['startDate'] == null
          ? null
          : DateTime.parse(json['startDate'] as String),
      dueDate: json['dueDate'] == null
          ? null
          : DateTime.parse(json['dueDate'] as String),
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          const [],
      watchers:
          (json['watchers'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList() ??
          const [],
      isRecurring: json['isRecurring'] as bool? ?? false,
      progress: (json['progress'] as num?)?.toDouble() ?? 0.0,
    );

Map<String, dynamic> _$CreateUpdateTaskDtoToJson(
  _CreateUpdateTaskDto instance,
) => <String, dynamic>{
  'title': instance.title,
  'description': instance.description,
  'parentTaskId': instance.parentTaskId,
  'projectId': instance.projectId,
  'sprintId': instance.sprintId,
  'taskTypeId': instance.taskTypeId,
  'workflowStageId': instance.workflowStageId,
  'assigneeId': instance.assigneeId,
  'reporterId': instance.reporterId,
  'priority': _$TaskPriorityEnumMap[instance.priority]!,
  'status': _$TaskStatusEnumMap[instance.status]!,
  'estimatedHours': instance.estimatedHours,
  'actualHours': instance.actualHours,
  'storyPoints': instance.storyPoints,
  'startDate': instance.startDate?.toIso8601String(),
  'dueDate': instance.dueDate?.toIso8601String(),
  'tags': instance.tags,
  'watchers': instance.watchers,
  'isRecurring': instance.isRecurring,
  'progress': instance.progress,
};

_TaskTypeDto _$TaskTypeDtoFromJson(Map<String, dynamic> json) => _TaskTypeDto(
  id: json['id'] as String,
  name: json['name'] as String,
  icon: json['icon'] as String,
  color: json['color'] as String,
  description: json['description'] as String?,
);

Map<String, dynamic> _$TaskTypeDtoToJson(_TaskTypeDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'icon': instance.icon,
      'color': instance.color,
      'description': instance.description,
    };
