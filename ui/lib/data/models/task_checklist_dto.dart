import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_checklist_dto.freezed.dart';
part 'task_checklist_dto.g.dart';

/// Checklist item DTO
@freezed
abstract class ChecklistItemDto with _$ChecklistItemDto {
  const factory ChecklistItemDto({
    required String id,
    required String text,
    @Default(false) bool isCompleted,
    required int order,
    String? completedBy,
    DateTime? completionTime,
  }) = _ChecklistItemDto;

  factory ChecklistItemDto.fromJson(Map<String, dynamic> json) =>
      _$ChecklistItemDtoFromJson(json);
}

/// Task checklist DTO
@freezed
abstract class TaskChecklistDto with _$TaskChecklistDto {
  const factory TaskChecklistDto({
    required String id,
    required String taskId,
    required String title,
    required List<ChecklistItemDto> items,
    required double progress,
    required DateTime creationTime,
  }) = _TaskChecklistDto;

  factory TaskChecklistDto.fromJson(Map<String, dynamic> json) =>
      _$TaskChecklistDtoFromJson(json);
}

@freezed
abstract class CreateUpdateChecklistDto with _$CreateUpdateChecklistDto {
  const factory CreateUpdateChecklistDto({
    required String taskId,
    required String title,
    @Default([]) List<ChecklistItemDto> items,
  }) = _CreateUpdateChecklistDto;

  factory CreateUpdateChecklistDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateChecklistDtoFromJson(json);
}
