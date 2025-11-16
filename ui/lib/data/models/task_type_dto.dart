import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_type_dto.freezed.dart';
part 'task_type_dto.g.dart';

/// Task type DTO (already defined in core, but expanding)
@freezed
abstract class TaskTypeDto with _$TaskTypeDto {
  const factory TaskTypeDto({
    required String id,
    required String name,
    required String icon,
    required String color,
    String? description,
    required String defaultWorkflowId,
    @Default(true) bool isActive,
    required DateTime creationTime,
  }) = _TaskTypeDto;

  factory TaskTypeDto.fromJson(Map<String, dynamic> json) =>
      _$TaskTypeDtoFromJson(json);
}

@freezed
abstract class CreateUpdateTaskTypeDto with _$CreateUpdateTaskTypeDto {
  const factory CreateUpdateTaskTypeDto({
    required String name,
    required String icon,
    required String color,
    String? description,
    required String defaultWorkflowId,
    @Default(true) bool isActive,
  }) = _CreateUpdateTaskTypeDto;

  factory CreateUpdateTaskTypeDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateTaskTypeDtoFromJson(json);
}
