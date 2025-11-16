// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectDto _$ProjectDtoFromJson(Map<String, dynamic> json) => _ProjectDto(
  id: json['id'] as String,
  name: json['name'] as String,
  description: json['description'] as String?,
  parentProjectId: json['parentProjectId'] as String?,
  startDate: DateTime.parse(json['startDate'] as String),
  endDate: DateTime.parse(json['endDate'] as String),
  status: $enumDecode(_$ProjectStatusEnumMap, json['status']),
  priority: $enumDecode(_$ProjectPriorityEnumMap, json['priority']),
  viewType: $enumDecode(_$ProjectViewTypeEnumMap, json['viewType']),
  teamMembers: (json['teamMembers'] as List<dynamic>)
      .map((e) => e as String)
      .toList(),
  ownerId: json['ownerId'] as String,
  workflowId: json['workflowId'] as String,
  color: json['color'] as String,
  progress: (json['progress'] as num).toDouble(),
  budget: (json['budget'] as num?)?.toDouble(),
  actualCost: (json['actualCost'] as num?)?.toDouble(),
  creationTime: DateTime.parse(json['creationTime'] as String),
  lastModificationTime: json['lastModificationTime'] == null
      ? null
      : DateTime.parse(json['lastModificationTime'] as String),
);

Map<String, dynamic> _$ProjectDtoToJson(_ProjectDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'description': instance.description,
      'parentProjectId': instance.parentProjectId,
      'startDate': instance.startDate.toIso8601String(),
      'endDate': instance.endDate.toIso8601String(),
      'status': _$ProjectStatusEnumMap[instance.status]!,
      'priority': _$ProjectPriorityEnumMap[instance.priority]!,
      'viewType': _$ProjectViewTypeEnumMap[instance.viewType]!,
      'teamMembers': instance.teamMembers,
      'ownerId': instance.ownerId,
      'workflowId': instance.workflowId,
      'color': instance.color,
      'progress': instance.progress,
      'budget': instance.budget,
      'actualCost': instance.actualCost,
      'creationTime': instance.creationTime.toIso8601String(),
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
    };

const _$ProjectStatusEnumMap = {
  ProjectStatus.planning: 'planning',
  ProjectStatus.active: 'active',
  ProjectStatus.onHold: 'onHold',
  ProjectStatus.completed: 'completed',
  ProjectStatus.cancelled: 'cancelled',
};

const _$ProjectPriorityEnumMap = {
  ProjectPriority.low: 'low',
  ProjectPriority.medium: 'medium',
  ProjectPriority.high: 'high',
  ProjectPriority.critical: 'critical',
};

const _$ProjectViewTypeEnumMap = {
  ProjectViewType.list: 'list',
  ProjectViewType.kanban: 'kanban',
  ProjectViewType.gantt: 'gantt',
};

_CreateUpdateProjectDto _$CreateUpdateProjectDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateProjectDto(
  name: json['name'] as String,
  description: json['description'] as String?,
  parentProjectId: json['parentProjectId'] as String?,
  startDate: DateTime.parse(json['startDate'] as String),
  endDate: DateTime.parse(json['endDate'] as String),
  status: $enumDecode(_$ProjectStatusEnumMap, json['status']),
  priority: $enumDecode(_$ProjectPriorityEnumMap, json['priority']),
  viewType: $enumDecode(_$ProjectViewTypeEnumMap, json['viewType']),
  teamMembers:
      (json['teamMembers'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  ownerId: json['ownerId'] as String,
  workflowId: json['workflowId'] as String,
  color: json['color'] as String? ?? '#2196F3',
  progress: (json['progress'] as num?)?.toDouble() ?? 0.0,
  budget: (json['budget'] as num?)?.toDouble(),
  actualCost: (json['actualCost'] as num?)?.toDouble(),
);

Map<String, dynamic> _$CreateUpdateProjectDtoToJson(
  _CreateUpdateProjectDto instance,
) => <String, dynamic>{
  'name': instance.name,
  'description': instance.description,
  'parentProjectId': instance.parentProjectId,
  'startDate': instance.startDate.toIso8601String(),
  'endDate': instance.endDate.toIso8601String(),
  'status': _$ProjectStatusEnumMap[instance.status]!,
  'priority': _$ProjectPriorityEnumMap[instance.priority]!,
  'viewType': _$ProjectViewTypeEnumMap[instance.viewType]!,
  'teamMembers': instance.teamMembers,
  'ownerId': instance.ownerId,
  'workflowId': instance.workflowId,
  'color': instance.color,
  'progress': instance.progress,
  'budget': instance.budget,
  'actualCost': instance.actualCost,
};
