import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_sprint_dto.freezed.dart';
part 'project_sprint_dto.g.dart';

/// Sprint status enum
enum SprintStatus {
  planning,
  active,
  completed,
  cancelled;

  String get displayName {
    switch (this) {
      case SprintStatus.planning:
        return 'Planning';
      case SprintStatus.active:
        return 'Active';
      case SprintStatus.completed:
        return 'Completed';
      case SprintStatus.cancelled:
        return 'Cancelled';
    }
  }
}

/// Project sprint DTO
@freezed
abstract class ProjectSprintDto with _$ProjectSprintDto {
  const factory ProjectSprintDto({
    required String id,
    required String projectId,
    required String name,
    String? goal,
    required DateTime startDate,
    required DateTime endDate,
    required SprintStatus status,
    int? velocity,
    int? capacity,
    @Default(0.0) double actualHours,
    required DateTime creationTime,
  }) = _ProjectSprintDto;

  factory ProjectSprintDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectSprintDtoFromJson(json);
}

/// Create/Update sprint DTO
@freezed
abstract class CreateUpdateSprintDto with _$CreateUpdateSprintDto {
  const factory CreateUpdateSprintDto({
    required String projectId,
    required String name,
    String? goal,
    required DateTime startDate,
    required DateTime endDate,
    int? velocity,
    int? capacity,
  }) = _CreateUpdateSprintDto;

  factory CreateUpdateSprintDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateSprintDtoFromJson(json);
}
