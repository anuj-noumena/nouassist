// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_delegation_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskDelegationDto _$TaskDelegationDtoFromJson(Map<String, dynamic> json) =>
    _TaskDelegationDto(
      id: json['id'] as String,
      taskId: json['taskId'] as String,
      delegatedFrom: json['delegatedFrom'] as String,
      delegatedFromName: json['delegatedFromName'] as String,
      delegatedTo: json['delegatedTo'] as String,
      delegatedToName: json['delegatedToName'] as String,
      reason: json['reason'] as String?,
      delegationDate: DateTime.parse(json['delegationDate'] as String),
      acceptanceStatus: $enumDecode(
        _$DelegationStatusEnumMap,
        json['acceptanceStatus'],
      ),
      responseDate: json['responseDate'] == null
          ? null
          : DateTime.parse(json['responseDate'] as String),
      responseComment: json['responseComment'] as String?,
    );

Map<String, dynamic> _$TaskDelegationDtoToJson(_TaskDelegationDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'taskId': instance.taskId,
      'delegatedFrom': instance.delegatedFrom,
      'delegatedFromName': instance.delegatedFromName,
      'delegatedTo': instance.delegatedTo,
      'delegatedToName': instance.delegatedToName,
      'reason': instance.reason,
      'delegationDate': instance.delegationDate.toIso8601String(),
      'acceptanceStatus': _$DelegationStatusEnumMap[instance.acceptanceStatus]!,
      'responseDate': instance.responseDate?.toIso8601String(),
      'responseComment': instance.responseComment,
    };

const _$DelegationStatusEnumMap = {
  DelegationStatus.pending: 'pending',
  DelegationStatus.accepted: 'accepted',
  DelegationStatus.rejected: 'rejected',
};

_CreateDelegationDto _$CreateDelegationDtoFromJson(Map<String, dynamic> json) =>
    _CreateDelegationDto(
      taskId: json['taskId'] as String,
      delegatedTo: json['delegatedTo'] as String,
      reason: json['reason'] as String?,
    );

Map<String, dynamic> _$CreateDelegationDtoToJson(
  _CreateDelegationDto instance,
) => <String, dynamic>{
  'taskId': instance.taskId,
  'delegatedTo': instance.delegatedTo,
  'reason': instance.reason,
};
