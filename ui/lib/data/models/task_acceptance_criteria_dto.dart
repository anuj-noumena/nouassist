import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_acceptance_criteria_dto.freezed.dart';
part 'task_acceptance_criteria_dto.g.dart';

/// Task acceptance criteria DTO
@freezed
abstract class TaskAcceptanceCriteriaDto with _$TaskAcceptanceCriteriaDto {
  const factory TaskAcceptanceCriteriaDto({
    required String id,
    required String taskId,
    required String criterion,
    @Default(false) bool isCompleted,
    required int order,
    required DateTime creationTime,
    DateTime? completionTime,
  }) = _TaskAcceptanceCriteriaDto;

  factory TaskAcceptanceCriteriaDto.fromJson(Map<String, dynamic> json) =>
      _$TaskAcceptanceCriteriaDtoFromJson(json);
}

@freezed
abstract class CreateUpdateCriteriaDto with _$CreateUpdateCriteriaDto {
  const factory CreateUpdateCriteriaDto({
    required String taskId,
    required String criterion,
    @Default(false) bool isCompleted,
    required int order,
  }) = _CreateUpdateCriteriaDto;

  factory CreateUpdateCriteriaDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateCriteriaDtoFromJson(json);
}
