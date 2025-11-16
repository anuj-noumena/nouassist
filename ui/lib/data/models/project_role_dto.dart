import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_role_dto.freezed.dart';
part 'project_role_dto.g.dart';

/// Project role DTO
@freezed
abstract class ProjectRoleDto with _$ProjectRoleDto {
  const factory ProjectRoleDto({
    required String id,
    required String name,
    String? description,
    required List<String> permissions,
    @Default(false) bool isSystemRole,
    required DateTime creationTime,
  }) = _ProjectRoleDto;

  factory ProjectRoleDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectRoleDtoFromJson(json);
}

/// Create/Update project role DTO
@freezed
abstract class CreateUpdateProjectRoleDto with _$CreateUpdateProjectRoleDto {
  const factory CreateUpdateProjectRoleDto({
    required String name,
    String? description,
    required List<String> permissions,
  }) = _CreateUpdateProjectRoleDto;

  factory CreateUpdateProjectRoleDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateProjectRoleDtoFromJson(json);
}
