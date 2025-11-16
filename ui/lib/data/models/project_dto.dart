import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_dto.freezed.dart';
part 'project_dto.g.dart';

/// Project status enum
enum ProjectStatus {
  planning,
  active,
  onHold,
  completed,
  cancelled;

  String get displayName {
    switch (this) {
      case ProjectStatus.planning:
        return 'Planning';
      case ProjectStatus.active:
        return 'Active';
      case ProjectStatus.onHold:
        return 'On Hold';
      case ProjectStatus.completed:
        return 'Completed';
      case ProjectStatus.cancelled:
        return 'Cancelled';
    }
  }
}

/// Project priority enum
enum ProjectPriority {
  low,
  medium,
  high,
  critical;

  String get displayName {
    switch (this) {
      case ProjectPriority.low:
        return 'Low';
      case ProjectPriority.medium:
        return 'Medium';
      case ProjectPriority.high:
        return 'High';
      case ProjectPriority.critical:
        return 'Critical';
    }
  }
}

/// Project view type enum
enum ProjectViewType {
  list,
  kanban,
  gantt;

  String get displayName {
    switch (this) {
      case ProjectViewType.list:
        return 'List';
      case ProjectViewType.kanban:
        return 'Kanban';
      case ProjectViewType.gantt:
        return 'Gantt';
    }
  }
}

/// Project data transfer object
@freezed
abstract class ProjectDto with _$ProjectDto {
  const factory ProjectDto({
    required String id,
    required String name,
    String? description,
    String? parentProjectId,
    required DateTime startDate,
    required DateTime endDate,
    required ProjectStatus status,
    required ProjectPriority priority,
    required ProjectViewType viewType,
    required List<String> teamMembers,
    required String ownerId,
    required String workflowId,
    required String color,
    required double progress,
    double? budget,
    double? actualCost,
    required DateTime creationTime,
    DateTime? lastModificationTime,
  }) = _ProjectDto;

  factory ProjectDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectDtoFromJson(json);
}

/// Create/Update project DTO
@freezed
abstract class CreateUpdateProjectDto with _$CreateUpdateProjectDto {
  const factory CreateUpdateProjectDto({
    required String name,
    String? description,
    String? parentProjectId,
    required DateTime startDate,
    required DateTime endDate,
    required ProjectStatus status,
    required ProjectPriority priority,
    required ProjectViewType viewType,
    @Default([]) List<String> teamMembers,
    required String ownerId,
    required String workflowId,
    @Default('#2196F3') String color,
    @Default(0.0) double progress,
    double? budget,
    double? actualCost,
  }) = _CreateUpdateProjectDto;

  factory CreateUpdateProjectDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateProjectDtoFromJson(json);
}
