// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_requirement_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectRequirementDto _$ProjectRequirementDtoFromJson(
  Map<String, dynamic> json,
) => _ProjectRequirementDto(
  id: json['id'] as String,
  projectId: json['projectId'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  requirementType: $enumDecode(
    _$RequirementTypeEnumMap,
    json['requirementType'],
  ),
  priority: $enumDecode(_$RequirementPriorityEnumMap, json['priority']),
  status: $enumDecode(_$RequirementStatusEnumMap, json['status']),
  acceptanceCriteria: json['acceptanceCriteria'] as String?,
  estimatedEffort: (json['estimatedEffort'] as num?)?.toDouble(),
  parentRequirementId: json['parentRequirementId'] as String?,
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  createdBy: json['createdBy'] as String,
  approvedBy: json['approvedBy'] as String?,
  creationTime: DateTime.parse(json['creationTime'] as String),
  approvalTime: json['approvalTime'] == null
      ? null
      : DateTime.parse(json['approvalTime'] as String),
);

Map<String, dynamic> _$ProjectRequirementDtoToJson(
  _ProjectRequirementDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'projectId': instance.projectId,
  'title': instance.title,
  'description': instance.description,
  'requirementType': _$RequirementTypeEnumMap[instance.requirementType]!,
  'priority': _$RequirementPriorityEnumMap[instance.priority]!,
  'status': _$RequirementStatusEnumMap[instance.status]!,
  'acceptanceCriteria': instance.acceptanceCriteria,
  'estimatedEffort': instance.estimatedEffort,
  'parentRequirementId': instance.parentRequirementId,
  'tags': instance.tags,
  'createdBy': instance.createdBy,
  'approvedBy': instance.approvedBy,
  'creationTime': instance.creationTime.toIso8601String(),
  'approvalTime': instance.approvalTime?.toIso8601String(),
};

const _$RequirementTypeEnumMap = {
  RequirementType.functional: 'functional',
  RequirementType.nonFunctional: 'nonFunctional',
  RequirementType.business: 'business',
  RequirementType.technical: 'technical',
};

const _$RequirementPriorityEnumMap = {
  RequirementPriority.mustHave: 'mustHave',
  RequirementPriority.shouldHave: 'shouldHave',
  RequirementPriority.couldHave: 'couldHave',
  RequirementPriority.wontHave: 'wontHave',
};

const _$RequirementStatusEnumMap = {
  RequirementStatus.draft: 'draft',
  RequirementStatus.approved: 'approved',
  RequirementStatus.inProgress: 'inProgress',
  RequirementStatus.completed: 'completed',
  RequirementStatus.rejected: 'rejected',
};

_CreateUpdateRequirementDto _$CreateUpdateRequirementDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateRequirementDto(
  projectId: json['projectId'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  requirementType: $enumDecode(
    _$RequirementTypeEnumMap,
    json['requirementType'],
  ),
  priority: $enumDecode(_$RequirementPriorityEnumMap, json['priority']),
  status: $enumDecode(_$RequirementStatusEnumMap, json['status']),
  acceptanceCriteria: json['acceptanceCriteria'] as String?,
  estimatedEffort: (json['estimatedEffort'] as num?)?.toDouble(),
  parentRequirementId: json['parentRequirementId'] as String?,
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
);

Map<String, dynamic> _$CreateUpdateRequirementDtoToJson(
  _CreateUpdateRequirementDto instance,
) => <String, dynamic>{
  'projectId': instance.projectId,
  'title': instance.title,
  'description': instance.description,
  'requirementType': _$RequirementTypeEnumMap[instance.requirementType]!,
  'priority': _$RequirementPriorityEnumMap[instance.priority]!,
  'status': _$RequirementStatusEnumMap[instance.status]!,
  'acceptanceCriteria': instance.acceptanceCriteria,
  'estimatedEffort': instance.estimatedEffort,
  'parentRequirementId': instance.parentRequirementId,
  'tags': instance.tags,
};
