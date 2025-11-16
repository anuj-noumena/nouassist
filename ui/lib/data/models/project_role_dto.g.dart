// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_role_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ProjectRoleDto _$ProjectRoleDtoFromJson(Map<String, dynamic> json) =>
    _ProjectRoleDto(
      id: json['id'] as String,
      name: json['name'] as String,
      description: json['description'] as String?,
      permissions: (json['permissions'] as List<dynamic>)
          .map((e) => e as String)
          .toList(),
      isSystemRole: json['isSystemRole'] as bool? ?? false,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$ProjectRoleDtoToJson(_ProjectRoleDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'description': instance.description,
      'permissions': instance.permissions,
      'isSystemRole': instance.isSystemRole,
      'creationTime': instance.creationTime.toIso8601String(),
    };

_CreateUpdateProjectRoleDto _$CreateUpdateProjectRoleDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateProjectRoleDto(
  name: json['name'] as String,
  description: json['description'] as String?,
  permissions: (json['permissions'] as List<dynamic>)
      .map((e) => e as String)
      .toList(),
);

Map<String, dynamic> _$CreateUpdateProjectRoleDtoToJson(
  _CreateUpdateProjectRoleDto instance,
) => <String, dynamic>{
  'name': instance.name,
  'description': instance.description,
  'permissions': instance.permissions,
};
