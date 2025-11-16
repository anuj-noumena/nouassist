import '../models/learning_progress_dto.dart';

/// Mock progress service
class MockProgressService {
  static final List<CourseProgressDto> _courseProgress = _generateMockCourseProgress();
  static final Map<String, List<LessonProgressDto>> _lessonProgress = {};

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get course progress by user
  static Future<List<CourseProgressDto>> getCourseProgressByUser(
    String userId,
  ) async {
    await _simulateDelay();
    return _courseProgress.where((p) => p.userId == userId).toList();
  }

  /// Get course progress
  static Future<CourseProgressDto?> getCourseProgress(
    String userId,
    String courseId,
  ) async {
    await _simulateDelay();

    try {
      return _courseProgress.firstWhere(
        (p) => p.userId == userId && p.courseId == courseId,
      );
    } catch (e) {
      return null;
    }
  }

  /// Update course progress
  static Future<CourseProgressDto> updateCourseProgress(
    String userId,
    String courseId,
    {
    required int completedLessons,
    required int totalLessons,
    required int timeSpent,
  }) async {
    await _simulateDelay();

    final percentage = (completedLessons / totalLessons * 100).clamp(0, 100);
    ProgressStatus status;

    if (completedLessons == 0) {
      status = ProgressStatus.notStarted;
    } else if (completedLessons >= totalLessons) {
      status = ProgressStatus.completed;
    } else {
      status = ProgressStatus.inProgress;
    }

    final index = _courseProgress.indexWhere(
      (p) => p.userId == userId && p.courseId == courseId,
    );

    if (index != -1) {
      final updated = _courseProgress[index].copyWith(
        status: status,
        completionPercentage: percentage.toDouble(),
        completedLessons: completedLessons,
        totalLessons: totalLessons,
        timeSpent: timeSpent,
        lastAccessedDate: DateTime.now(),
        completionDate:
            status == ProgressStatus.completed ? DateTime.now() : null,
      );

      _courseProgress[index] = updated;
      return updated;
    } else {
      final newProgress = CourseProgressDto(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        userId: userId,
        courseId: courseId,
        courseTitle: 'Course Title',
        status: status,
        completionPercentage: percentage.toDouble(),
        completedLessons: completedLessons,
        totalLessons: totalLessons,
        timeSpent: timeSpent,
        enrollmentDate: DateTime.now(),
        lastAccessedDate: DateTime.now(),
        completionDate:
            status == ProgressStatus.completed ? DateTime.now() : null,
      );

      _courseProgress.add(newProgress);
      return newProgress;
    }
  }

  /// Mark lesson as completed
  static Future<LessonProgressDto> markLessonCompleted(
    String userId,
    String lessonId,
    String courseId,
    int timeSpent,
  ) async {
    await _simulateDelay();

    final key = '$userId-$courseId';
    if (_lessonProgress[key] == null) {
      _lessonProgress[key] = [];
    }

    final index = _lessonProgress[key]!.indexWhere(
      (p) => p.lessonId == lessonId,
    );

    if (index != -1) {
      final updated = _lessonProgress[key]![index].copyWith(
        isCompleted: true,
        timeSpent: timeSpent,
        completedDate: DateTime.now(),
        lastAccessedDate: DateTime.now(),
      );

      _lessonProgress[key]![index] = updated;
      return updated;
    } else {
      final newProgress = LessonProgressDto(
        id: DateTime.now().millisecondsSinceEpoch.toString(),
        userId: userId,
        lessonId: lessonId,
        courseId: courseId,
        isCompleted: true,
        timeSpent: timeSpent,
        startedDate: DateTime.now(),
        completedDate: DateTime.now(),
        lastAccessedDate: DateTime.now(),
      );

      _lessonProgress[key]!.add(newProgress);
      return newProgress;
    }
  }

  /// Generate mock course progress
  static List<CourseProgressDto> _generateMockCourseProgress() {
    final now = DateTime.now();
    return [
      CourseProgressDto(
        id: 'progress-1',
        userId: 'user-1',
        courseId: 'course-1',
        courseTitle: 'Flutter Advanced Masterclass',
        status: ProgressStatus.inProgress,
        completionPercentage: 35.0,
        completedLessons: 17,
        totalLessons: 48,
        timeSpent: 720,
        enrollmentDate: now.subtract(const Duration(days: 30)),
        lastAccessedDate: now.subtract(const Duration(hours: 2)),
      ),
      CourseProgressDto(
        id: 'progress-2',
        userId: 'user-1',
        courseId: 'course-3',
        courseTitle: 'Python for Data Science',
        status: ProgressStatus.completed,
        completionPercentage: 100.0,
        completedLessons: 30,
        totalLessons: 30,
        timeSpent: 1500,
        enrollmentDate: now.subtract(const Duration(days: 60)),
        completionDate: now.subtract(const Duration(days: 5)),
        lastAccessedDate: now.subtract(const Duration(days: 5)),
        certificateId: 'cert-1',
      ),
      CourseProgressDto(
        id: 'progress-3',
        userId: 'user-1',
        courseId: 'course-6',
        courseTitle: 'JavaScript ES6+ Modern Features',
        status: ProgressStatus.notStarted,
        completionPercentage: 0.0,
        completedLessons: 0,
        totalLessons: 18,
        timeSpent: 0,
        enrollmentDate: now.subtract(const Duration(days: 2)),
      ),
    ];
  }
}
