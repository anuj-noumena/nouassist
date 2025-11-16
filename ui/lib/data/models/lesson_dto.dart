import 'package:freezed_annotation/freezed_annotation.dart';

part 'lesson_dto.freezed.dart';
part 'lesson_dto.g.dart';

/// Lesson type
enum LessonType {
  video,
  article,
  quiz,
  assignment,
  exercise,
  project;

  String get displayName {
    switch (this) {
      case LessonType.video:
        return 'Video';
      case LessonType.article:
        return 'Article';
      case LessonType.quiz:
        return 'Quiz';
      case LessonType.assignment:
        return 'Assignment';
      case LessonType.exercise:
        return 'Exercise';
      case LessonType.project:
        return 'Project';
    }
  }
}

/// Lesson DTO
@freezed
abstract class LessonDto with _$LessonDto {
  const factory LessonDto({
    required String id,
    required String courseId,
    required String title,
    required String description,
    required LessonType lessonType,
    required int order,
    required int duration,
    String? videoUrl,
    String? content,
    @Default([]) List<String> attachments,
    String? quizId,
    @Default(false) bool isFree,
    @Default(false) bool isCompleted,
    required DateTime creationTime,
  }) = _LessonDto;

  factory LessonDto.fromJson(Map<String, dynamic> json) =>
      _$LessonDtoFromJson(json);
}

/// Create/Update lesson DTO
@freezed
abstract class CreateUpdateLessonDto with _$CreateUpdateLessonDto {
  const factory CreateUpdateLessonDto({
    required String courseId,
    required String title,
    required String description,
    required LessonType lessonType,
    required int order,
    required int duration,
    String? videoUrl,
    String? content,
    @Default([]) List<String> attachments,
    String? quizId,
    @Default(false) bool isFree,
  }) = _CreateUpdateLessonDto;

  factory CreateUpdateLessonDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateLessonDtoFromJson(json);
}
