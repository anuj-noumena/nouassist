import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_action_step_dto.freezed.dart';
part 'task_action_step_dto.g.dart';

/// Task action step DTO
@freezed
abstract class TaskActionStepDto with _$TaskActionStepDto {
  const factory TaskActionStepDto({
    required String id,
    required String taskId,
    required String description,
    @Default(false) bool isCompleted,
    String? assigneeId,
    String? assigneeName,
    DateTime? dueDate,
    required int order,
    required DateTime creationTime,
    DateTime? completionTime,
  }) = _TaskActionStepDto;

  factory TaskActionStepDto.fromJson(Map<String, dynamic> json) =>
      _$TaskActionStepDtoFromJson(json);
}

@freezed
abstract class CreateUpdateActionStepDto with _$CreateUpdateActionStepDto {
  const factory CreateUpdateActionStepDto({
    required String taskId,
    required String description,
    @Default(false) bool isCompleted,
    String? assigneeId,
    DateTime? dueDate,
    required int order,
  }) = _CreateUpdateActionStepDto;

  factory CreateUpdateActionStepDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateActionStepDtoFromJson(json);
}
