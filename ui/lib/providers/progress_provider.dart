import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/learning_progress_dto.dart';
import '../data/mock/mock_progress_service.dart';

part 'progress_provider.g.dart';

@riverpod
Future<List<CourseProgressDto>> userCourseProgress(Ref ref,
  String userId,
) async {
  return await MockProgressService.getCourseProgressByUser(userId);
}

@riverpod
Future<CourseProgressDto?> courseProgress(Ref ref,
  String userId,
  String courseId,
) async {
  return await MockProgressService.getCourseProgress(userId, courseId);
}

@riverpod
Future<void> updateProgress(Ref ref,
  String userId,
  String courseId, {
  required int completedLessons,
  required int totalLessons,
  required int timeSpent,
}) async {
  await MockProgressService.updateCourseProgress(
    userId,
    courseId,
    completedLessons: completedLessons,
    totalLessons: totalLessons,
    timeSpent: timeSpent,
  );

  ref.invalidate(userCourseProgressProvider(userId));
  ref.invalidate(courseProgressProvider(userId, courseId));
}

@riverpod
Future<void> markLessonComplete(Ref ref,
  String userId,
  String lessonId,
  String courseId,
  int timeSpent,
) async {
  await MockProgressService.markLessonCompleted(
    userId,
    lessonId,
    courseId,
    timeSpent,
  );

  ref.invalidate(courseProgressProvider(userId, courseId));
  ref.invalidate(userCourseProgressProvider(userId));
}
