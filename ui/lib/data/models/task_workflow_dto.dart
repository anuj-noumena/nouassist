import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_workflow_dto.freezed.dart';
part 'task_workflow_dto.g.dart';

/// Status category enum
enum StatusCategory {
  todo,
  inProgress,
  done;

  String get displayName {
    switch (this) {
      case StatusCategory.todo:
        return 'To Do';
      case StatusCategory.inProgress:
        return 'In Progress';
      case StatusCategory.done:
        return 'Done';
    }
  }
}

/// Task workflow stage DTO
@freezed
abstract class TaskWorkflowStageDto with _$TaskWorkflowStageDto {
  const factory TaskWorkflowStageDto({
    required String id,
    required String name,
    required int order,
    required String color,
    required StatusCategory statusCategory,
    @Default([]) List<String> allowedTransitions,
    @Default(false) bool requiresComment,
  }) = _TaskWorkflowStageDto;

  factory TaskWorkflowStageDto.fromJson(Map<String, dynamic> json) =>
      _$TaskWorkflowStageDtoFromJson(json);
}

/// Task workflow DTO
@freezed
abstract class TaskWorkflowDto with _$TaskWorkflowDto {
  const factory TaskWorkflowDto({
    required String id,
    required String name,
    String? description,
    required List<TaskWorkflowStageDto> stages,
    @Default([]) List<String> taskTypeIds,
    @Default(false) bool isDefault,
    required DateTime creationTime,
  }) = _TaskWorkflowDto;

  factory TaskWorkflowDto.fromJson(Map<String, dynamic> json) =>
      _$TaskWorkflowDtoFromJson(json);
}
