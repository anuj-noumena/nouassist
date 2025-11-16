import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/lesson_dto.dart';
import '../data/mock/mock_lesson_service.dart';

part 'lesson_provider.g.dart';

@riverpod
Future<List<LessonDto>> courseLessons(Ref ref,
  String courseId,
) async {
  return await MockLessonService.getByCourse(courseId);
}

@riverpod
Future<LessonDto> lesson(Ref ref, String id) async {
  return await MockLessonService.get(id);
}

@riverpod
Future<void> createLesson(Ref ref,
  CreateUpdateLessonDto input,
) async {
  await MockLessonService.create(input);
  ref.invalidate(courseLessonsProvider(input.courseId));
}

@riverpod
Future<void> deleteLesson(Ref ref,
  String id,
  String courseId,
) async {
  await MockLessonService.delete(id);
  ref.invalidate(courseLessonsProvider(courseId));
}
