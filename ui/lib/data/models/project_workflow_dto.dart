import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_workflow_dto.freezed.dart';
part 'project_workflow_dto.g.dart';

/// Workflow stage DTO
@freezed
abstract class WorkflowStageDto with _$WorkflowStageDto {
  const factory WorkflowStageDto({
    required String id,
    required String name,
    required int order,
    required String color,
    @Default(false) bool isInitial,
    @Default(false) bool isFinal,
    @Default([]) List<String> allowedTransitions,
  }) = _WorkflowStageDto;

  factory WorkflowStageDto.fromJson(Map<String, dynamic> json) =>
      _$WorkflowStageDtoFromJson(json);
}

/// Project workflow DTO
@freezed
abstract class ProjectWorkflowDto with _$ProjectWorkflowDto {
  const factory ProjectWorkflowDto({
    required String id,
    required String name,
    String? description,
    required List<WorkflowStageDto> stages,
    @Default(false) bool isDefault,
    String? projectType,
    required DateTime creationTime,
  }) = _ProjectWorkflowDto;

  factory ProjectWorkflowDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectWorkflowDtoFromJson(json);
}

/// Create/Update workflow DTO
@freezed
abstract class CreateUpdateWorkflowDto with _$CreateUpdateWorkflowDto {
  const factory CreateUpdateWorkflowDto({
    required String name,
    String? description,
    required List<WorkflowStageDto> stages,
    @Default(false) bool isDefault,
    String? projectType,
  }) = _CreateUpdateWorkflowDto;

  factory CreateUpdateWorkflowDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateWorkflowDtoFromJson(json);
}
