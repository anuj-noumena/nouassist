import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_discussion_dto.freezed.dart';
part 'task_discussion_dto.g.dart';

/// Task discussion/comment DTO
@freezed
abstract class TaskDiscussionDto with _$TaskDiscussionDto {
  const factory TaskDiscussionDto({
    required String id,
    required String taskId,
    String? parentDiscussionId,
    required String authorId,
    required String authorName,
    required String content,
    @Default([]) List<String> mentions,
    @Default([]) List<String> attachmentIds,
    @Default(false) bool isEdited,
    @Default({}) Map<String, List<String>> reactions,
    required DateTime creationTime,
    DateTime? lastEditTime,
  }) = _TaskDiscussionDto;

  factory TaskDiscussionDto.fromJson(Map<String, dynamic> json) =>
      _$TaskDiscussionDtoFromJson(json);
}

@freezed
abstract class CreateUpdateDiscussionDto with _$CreateUpdateDiscussionDto {
  const factory CreateUpdateDiscussionDto({
    required String taskId,
    String? parentDiscussionId,
    required String content,
    @Default([]) List<String> mentions,
    @Default([]) List<String> attachmentIds,
  }) = _CreateUpdateDiscussionDto;

  factory CreateUpdateDiscussionDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateDiscussionDtoFromJson(json);
}
