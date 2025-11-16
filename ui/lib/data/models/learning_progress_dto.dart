import 'package:freezed_annotation/freezed_annotation.dart';

part 'learning_progress_dto.freezed.dart';
part 'learning_progress_dto.g.dart';

/// Learning progress status
enum ProgressStatus {
  notStarted,
  inProgress,
  completed;

  String get displayName {
    switch (this) {
      case ProgressStatus.notStarted:
        return 'Not Started';
      case ProgressStatus.inProgress:
        return 'In Progress';
      case ProgressStatus.completed:
        return 'Completed';
    }
  }
}

/// Course progress DTO
@freezed
abstract class CourseProgressDto with _$CourseProgressDto {
  const factory CourseProgressDto({
    required String id,
    required String userId,
    required String courseId,
    required String courseTitle,
    required ProgressStatus status,
    required double completionPercentage,
    required int completedLessons,
    required int totalLessons,
    required int timeSpent,
    DateTime? enrollmentDate,
    DateTime? completionDate,
    DateTime? lastAccessedDate,
    String? certificateId,
  }) = _CourseProgressDto;

  factory CourseProgressDto.fromJson(Map<String, dynamic> json) =>
      _$CourseProgressDtoFromJson(json);
}

/// Lesson progress DTO
@freezed
abstract class LessonProgressDto with _$LessonProgressDto {
  const factory LessonProgressDto({
    required String id,
    required String userId,
    required String lessonId,
    required String courseId,
    @Default(false) bool isCompleted,
    required int timeSpent,
    int? videoProgress,
    DateTime? startedDate,
    DateTime? completedDate,
    DateTime? lastAccessedDate,
  }) = _LessonProgressDto;

  factory LessonProgressDto.fromJson(Map<String, dynamic> json) =>
      _$LessonProgressDtoFromJson(json);
}
