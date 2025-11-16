import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_team_dto.freezed.dart';
part 'project_team_dto.g.dart';

/// Project team member DTO
@freezed
abstract class ProjectTeamDto with _$ProjectTeamDto {
  const factory ProjectTeamDto({
    required String id,
    required String projectId,
    required String userId,
    required String userName,
    required String roleId,
    required String roleName,
    required DateTime assignedDate,
    double? billableRate,
  }) = _ProjectTeamDto;

  factory ProjectTeamDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectTeamDtoFromJson(json);
}

/// Add team member DTO
@freezed
abstract class AddTeamMemberDto with _$AddTeamMemberDto {
  const factory AddTeamMemberDto({
    required String userId,
    required String roleId,
    double? billableRate,
  }) = _AddTeamMemberDto;

  factory AddTeamMemberDto.fromJson(Map<String, dynamic> json) =>
      _$AddTeamMemberDtoFromJson(json);
}
