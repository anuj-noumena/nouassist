import '../models/lesson_dto.dart';

/// Mock lesson service
class MockLessonService {
  static final Map<String, List<LessonDto>> _lessonsByCourse =
      _generateMockLessons();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get lessons by course
  static Future<List<LessonDto>> getByCourse(String courseId) async {
    await _simulateDelay();
    return _lessonsByCourse[courseId] ?? [];
  }

  /// Get single lesson
  static Future<LessonDto> get(String id) async {
    await _simulateDelay();

    for (var lessons in _lessonsByCourse.values) {
      try {
        return lessons.firstWhere((lesson) => lesson.id == id);
      } catch (_) {}
    }

    throw Exception('Lesson not found');
  }

  /// Create lesson
  static Future<LessonDto> create(CreateUpdateLessonDto input) async {
    await _simulateDelay();

    final newLesson = LessonDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      courseId: input.courseId,
      title: input.title,
      description: input.description,
      lessonType: input.lessonType,
      order: input.order,
      duration: input.duration,
      videoUrl: input.videoUrl,
      content: input.content,
      attachments: input.attachments,
      quizId: input.quizId,
      isFree: input.isFree,
      creationTime: DateTime.now(),
    );

    if (_lessonsByCourse[input.courseId] == null) {
      _lessonsByCourse[input.courseId] = [];
    }
    _lessonsByCourse[input.courseId]!.add(newLesson);

    return newLesson;
  }

  /// Update lesson
  static Future<LessonDto> update(String id, CreateUpdateLessonDto input) async {
    await _simulateDelay();

    for (var lessons in _lessonsByCourse.values) {
      final index = lessons.indexWhere((lesson) => lesson.id == id);
      if (index != -1) {
        final updatedLesson = lessons[index].copyWith(
          title: input.title,
          description: input.description,
          lessonType: input.lessonType,
          order: input.order,
          duration: input.duration,
          videoUrl: input.videoUrl,
          content: input.content,
          attachments: input.attachments,
          quizId: input.quizId,
          isFree: input.isFree,
        );

        lessons[index] = updatedLesson;
        return updatedLesson;
      }
    }

    throw Exception('Lesson not found');
  }

  /// Delete lesson
  static Future<void> delete(String id) async {
    await _simulateDelay();

    for (var lessons in _lessonsByCourse.values) {
      final initialLength = lessons.length;
      lessons.removeWhere((lesson) => lesson.id == id);
      if (lessons.length < initialLength) {
        return;
      }
    }

    throw Exception('Lesson not found');
  }

  /// Generate mock lessons
  static Map<String, List<LessonDto>> _generateMockLessons() {
    final now = DateTime.now();
    return {
      'course-1': [
        LessonDto(
          id: 'lesson-1-1',
          courseId: 'course-1',
          title: 'Introduction to Advanced Flutter',
          description: 'Overview of advanced Flutter concepts and what you will learn',
          lessonType: LessonType.video,
          order: 1,
          duration: 15,
          videoUrl: 'https://example.com/videos/intro.mp4',
          isFree: true,
          creationTime: now.subtract(const Duration(days: 180)),
        ),
        LessonDto(
          id: 'lesson-1-2',
          courseId: 'course-1',
          title: 'State Management Deep Dive',
          description: 'Understanding different state management approaches in Flutter',
          lessonType: LessonType.video,
          order: 2,
          duration: 45,
          videoUrl: 'https://example.com/videos/state-management.mp4',
          creationTime: now.subtract(const Duration(days: 179)),
        ),
        LessonDto(
          id: 'lesson-1-3',
          courseId: 'course-1',
          title: 'Riverpod Fundamentals',
          description: 'Learn Riverpod state management from scratch',
          lessonType: LessonType.video,
          order: 3,
          duration: 60,
          videoUrl: 'https://example.com/videos/riverpod.mp4',
          creationTime: now.subtract(const Duration(days: 178)),
        ),
        LessonDto(
          id: 'lesson-1-4',
          courseId: 'course-1',
          title: 'State Management Quiz',
          description: 'Test your knowledge of state management concepts',
          lessonType: LessonType.quiz,
          order: 4,
          duration: 30,
          quizId: 'quiz-1',
          creationTime: now.subtract(const Duration(days: 177)),
        ),
        LessonDto(
          id: 'lesson-1-5',
          courseId: 'course-1',
          title: 'Advanced Animations',
          description: 'Create smooth and complex animations in Flutter',
          lessonType: LessonType.video,
          order: 5,
          duration: 50,
          videoUrl: 'https://example.com/videos/animations.mp4',
          creationTime: now.subtract(const Duration(days: 176)),
        ),
      ],
      'course-2': [
        LessonDto(
          id: 'lesson-2-1',
          courseId: 'course-2',
          title: 'React Native Basics',
          description: 'Getting started with React Native development',
          lessonType: LessonType.video,
          order: 1,
          duration: 20,
          videoUrl: 'https://example.com/videos/rn-basics.mp4',
          isFree: true,
          creationTime: now.subtract(const Duration(days: 150)),
        ),
        LessonDto(
          id: 'lesson-2-2',
          courseId: 'course-2',
          title: 'Components and Props',
          description: 'Understanding React Native components',
          lessonType: LessonType.video,
          order: 2,
          duration: 35,
          videoUrl: 'https://example.com/videos/components.mp4',
          creationTime: now.subtract(const Duration(days: 149)),
        ),
        LessonDto(
          id: 'lesson-2-3',
          courseId: 'course-2',
          title: 'Navigation Patterns',
          description: 'Implementing navigation in React Native apps',
          lessonType: LessonType.video,
          order: 3,
          duration: 40,
          videoUrl: 'https://example.com/videos/navigation.mp4',
          creationTime: now.subtract(const Duration(days: 148)),
        ),
      ],
      'course-3': [
        LessonDto(
          id: 'lesson-3-1',
          courseId: 'course-3',
          title: 'Python Fundamentals',
          description: 'Learn Python programming basics',
          lessonType: LessonType.video,
          order: 1,
          duration: 25,
          videoUrl: 'https://example.com/videos/python-basics.mp4',
          isFree: true,
          creationTime: now.subtract(const Duration(days: 200)),
        ),
        LessonDto(
          id: 'lesson-3-2',
          courseId: 'course-3',
          title: 'Data Structures in Python',
          description: 'Lists, tuples, dictionaries, and sets',
          lessonType: LessonType.video,
          order: 2,
          duration: 40,
          videoUrl: 'https://example.com/videos/data-structures.mp4',
          isFree: true,
          creationTime: now.subtract(const Duration(days: 199)),
        ),
        LessonDto(
          id: 'lesson-3-3',
          courseId: 'course-3',
          title: 'Pandas Introduction',
          description: 'Getting started with pandas library',
          lessonType: LessonType.video,
          order: 3,
          duration: 45,
          videoUrl: 'https://example.com/videos/pandas.mp4',
          isFree: true,
          creationTime: now.subtract(const Duration(days: 198)),
        ),
      ],
    };
  }
}
