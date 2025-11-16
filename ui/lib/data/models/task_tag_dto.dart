import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_tag_dto.freezed.dart';
part 'task_tag_dto.g.dart';

/// Task tag DTO
@freezed
abstract class TaskTagDto with _$TaskTagDto {
  const factory TaskTagDto({
    required String id,
    required String name,
    required String color,
    String? description,
    String? category,
    @Default(0) int usageCount,
    required DateTime creationTime,
  }) = _TaskTagDto;

  factory TaskTagDto.fromJson(Map<String, dynamic> json) =>
      _$TaskTagDtoFromJson(json);
}

@freezed
abstract class CreateUpdateTaskTagDto with _$CreateUpdateTaskTagDto {
  const factory CreateUpdateTaskTagDto({
    required String name,
    required String color,
    String? description,
    String? category,
  }) = _CreateUpdateTaskTagDto;

  factory CreateUpdateTaskTagDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateTaskTagDtoFromJson(json);
}
