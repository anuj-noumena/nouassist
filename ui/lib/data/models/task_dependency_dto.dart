import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_dependency_dto.freezed.dart';
part 'task_dependency_dto.g.dart';

/// Dependency type enum
enum DependencyType {
  finishToStart,
  startToStart,
  finishToFinish,
  startToFinish;

  String get displayName {
    switch (this) {
      case DependencyType.finishToStart:
        return 'Finish to Start';
      case DependencyType.startToStart:
        return 'Start to Start';
      case DependencyType.finishToFinish:
        return 'Finish to Finish';
      case DependencyType.startToFinish:
        return 'Start to Finish';
    }
  }
}

/// Task dependency DTO
@freezed
abstract class TaskDependencyDto with _$TaskDependencyDto {
  const factory TaskDependencyDto({
    required String id,
    required String predecessorTaskId,
    required String predecessorTaskTitle,
    required String successorTaskId,
    required String successorTaskTitle,
    required DependencyType dependencyType,
    @Default(0) int lagDays,
    required DateTime creationTime,
  }) = _TaskDependencyDto;

  factory TaskDependencyDto.fromJson(Map<String, dynamic> json) =>
      _$TaskDependencyDtoFromJson(json);
}

@freezed
abstract class CreateDependencyDto with _$CreateDependencyDto {
  const factory CreateDependencyDto({
    required String predecessorTaskId,
    required String successorTaskId,
    required DependencyType dependencyType,
    @Default(0) int lagDays,
  }) = _CreateDependencyDto;

  factory CreateDependencyDto.fromJson(Map<String, dynamic> json) =>
      _$CreateDependencyDtoFromJson(json);
}
