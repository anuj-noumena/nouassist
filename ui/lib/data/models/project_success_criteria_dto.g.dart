// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_success_criteria_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectSuccessCriteriaDto _$ProjectSuccessCriteriaDtoFromJson(
  Map<String, dynamic> json,
) => _ProjectSuccessCriteriaDto(
  id: json['id'] as String,
  projectId: json['projectId'] as String,
  criterion: json['criterion'] as String,
  description: json['description'] as String?,
  measurementType: $enumDecode(
    _$MeasurementTypeEnumMap,
    json['measurementType'],
  ),
  targetValue: json['targetValue'] as String?,
  actualValue: json['actualValue'] as String?,
  unit: json['unit'] as String?,
  status: $enumDecode(_$SuccessCriteriaStatusEnumMap, json['status']),
  weight: (json['weight'] as num).toDouble(),
  verificationMethod: json['verificationMethod'] as String?,
  creationTime: DateTime.parse(json['creationTime'] as String),
  achievementTime: json['achievementTime'] == null
      ? null
      : DateTime.parse(json['achievementTime'] as String),
);

Map<String, dynamic> _$ProjectSuccessCriteriaDtoToJson(
  _ProjectSuccessCriteriaDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'projectId': instance.projectId,
  'criterion': instance.criterion,
  'description': instance.description,
  'measurementType': _$MeasurementTypeEnumMap[instance.measurementType]!,
  'targetValue': instance.targetValue,
  'actualValue': instance.actualValue,
  'unit': instance.unit,
  'status': _$SuccessCriteriaStatusEnumMap[instance.status]!,
  'weight': instance.weight,
  'verificationMethod': instance.verificationMethod,
  'creationTime': instance.creationTime.toIso8601String(),
  'achievementTime': instance.achievementTime?.toIso8601String(),
};

const _$MeasurementTypeEnumMap = {
  MeasurementType.quantitative: 'quantitative',
  MeasurementType.qualitative: 'qualitative',
  MeasurementType.binary: 'binary',
};

const _$SuccessCriteriaStatusEnumMap = {
  SuccessCriteriaStatus.notStarted: 'notStarted',
  SuccessCriteriaStatus.inProgress: 'inProgress',
  SuccessCriteriaStatus.achieved: 'achieved',
  SuccessCriteriaStatus.notAchieved: 'notAchieved',
};

_CreateUpdateSuccessCriteriaDto _$CreateUpdateSuccessCriteriaDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateSuccessCriteriaDto(
  projectId: json['projectId'] as String,
  criterion: json['criterion'] as String,
  description: json['description'] as String?,
  measurementType: $enumDecode(
    _$MeasurementTypeEnumMap,
    json['measurementType'],
  ),
  targetValue: json['targetValue'] as String?,
  actualValue: json['actualValue'] as String?,
  unit: json['unit'] as String?,
  status:
      $enumDecodeNullable(_$SuccessCriteriaStatusEnumMap, json['status']) ??
      SuccessCriteriaStatus.notStarted,
  weight: (json['weight'] as num?)?.toDouble() ?? 1.0,
  verificationMethod: json['verificationMethod'] as String?,
);

Map<String, dynamic> _$CreateUpdateSuccessCriteriaDtoToJson(
  _CreateUpdateSuccessCriteriaDto instance,
) => <String, dynamic>{
  'projectId': instance.projectId,
  'criterion': instance.criterion,
  'description': instance.description,
  'measurementType': _$MeasurementTypeEnumMap[instance.measurementType]!,
  'targetValue': instance.targetValue,
  'actualValue': instance.actualValue,
  'unit': instance.unit,
  'status': _$SuccessCriteriaStatusEnumMap[instance.status]!,
  'weight': instance.weight,
  'verificationMethod': instance.verificationMethod,
};
