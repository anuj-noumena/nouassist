import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_dto.freezed.dart';
part 'task_dto.g.dart';

/// Priority levels for tasks
enum TaskPriority {
  lowest,
  low,
  medium,
  high,
  highest;

  String get displayName {
    switch (this) {
      case TaskPriority.lowest:
        return 'Lowest';
      case TaskPriority.low:
        return 'Low';
      case TaskPriority.medium:
        return 'Medium';
      case TaskPriority.high:
        return 'High';
      case TaskPriority.highest:
        return 'Highest';
    }
  }
}

/// Status of tasks
enum TaskStatus {
  todo,
  inProgress,
  inReview,
  done,
  blocked;

  String get displayName {
    switch (this) {
      case TaskStatus.todo:
        return 'To Do';
      case TaskStatus.inProgress:
        return 'In Progress';
      case TaskStatus.inReview:
        return 'In Review';
      case TaskStatus.done:
        return 'Done';
      case TaskStatus.blocked:
        return 'Blocked';
    }
  }
}

/// Task data transfer object
@freezed
abstract class TaskDto with _$TaskDto {
  const factory TaskDto({
    required String id,
    required String title,
    String? description,
    String? parentTaskId,
    String? projectId,
    String? sprintId,
    required String taskTypeId,
    required String workflowStageId,
    String? assigneeId,
    required String reporterId,
    required TaskPriority priority,
    required TaskStatus status,
    double? estimatedHours,
    double? actualHours,
    int? storyPoints,
    DateTime? startDate,
    DateTime? dueDate,
    DateTime? completedDate,
    @Default([]) List<String> tags,
    @Default([]) List<String> watchers,
    @Default(false) bool isRecurring,
    @Default(0.0) double progress,
    required DateTime creationTime,
    DateTime? lastModificationTime,
  }) = _TaskDto;

  factory TaskDto.fromJson(Map<String, dynamic> json) =>
      _$TaskDtoFromJson(json);
}

/// Create/Update task DTO
@freezed
abstract class CreateUpdateTaskDto with _$CreateUpdateTaskDto {
  const factory CreateUpdateTaskDto({
    required String title,
    String? description,
    String? parentTaskId,
    String? projectId,
    String? sprintId,
    required String taskTypeId,
    required String workflowStageId,
    String? assigneeId,
    required String reporterId,
    required TaskPriority priority,
    required TaskStatus status,
    double? estimatedHours,
    double? actualHours,
    int? storyPoints,
    DateTime? startDate,
    DateTime? dueDate,
    @Default([]) List<String> tags,
    @Default([]) List<String> watchers,
    @Default(false) bool isRecurring,
    @Default(0.0) double progress,
  }) = _CreateUpdateTaskDto;

  factory CreateUpdateTaskDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateTaskDtoFromJson(json);
}

/// Task Type DTO
@freezed
abstract class TaskTypeDto with _$TaskTypeDto {
  const factory TaskTypeDto({
    required String id,
    required String name,
    required String icon,
    required String color,
    String? description,
  }) = _TaskTypeDto;

  factory TaskTypeDto.fromJson(Map<String, dynamic> json) =>
      _$TaskTypeDtoFromJson(json);
}
