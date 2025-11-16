// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_team_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectTeamDto _$ProjectTeamDtoFromJson(Map<String, dynamic> json) =>
    _ProjectTeamDto(
      id: json['id'] as String,
      projectId: json['projectId'] as String,
      userId: json['userId'] as String,
      userName: json['userName'] as String,
      roleId: json['roleId'] as String,
      roleName: json['roleName'] as String,
      assignedDate: DateTime.parse(json['assignedDate'] as String),
      billableRate: (json['billableRate'] as num?)?.toDouble(),
    );

Map<String, dynamic> _$ProjectTeamDtoToJson(_ProjectTeamDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'projectId': instance.projectId,
      'userId': instance.userId,
      'userName': instance.userName,
      'roleId': instance.roleId,
      'roleName': instance.roleName,
      'assignedDate': instance.assignedDate.toIso8601String(),
      'billableRate': instance.billableRate,
    };

_AddTeamMemberDto _$AddTeamMemberDtoFromJson(Map<String, dynamic> json) =>
    _AddTeamMemberDto(
      userId: json['userId'] as String,
      roleId: json['roleId'] as String,
      billableRate: (json['billableRate'] as num?)?.toDouble(),
    );

Map<String, dynamic> _$AddTeamMemberDtoToJson(_AddTeamMemberDto instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'roleId': instance.roleId,
      'billableRate': instance.billableRate,
    };
