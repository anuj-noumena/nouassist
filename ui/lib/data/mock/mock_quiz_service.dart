import '../models/quiz_dto.dart';

/// Mock quiz service
class MockQuizService {
  static final Map<String, QuizDto> _quizzes = _generateMockQuizzes();
  static final List<QuizAttemptDto> _attempts = [];

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get quiz by ID
  static Future<QuizDto> get(String id) async {
    await _simulateDelay();

    if (_quizzes.containsKey(id)) {
      return _quizzes[id]!;
    }

    throw Exception('Quiz not found');
  }

  /// Get quizzes by course
  static Future<List<QuizDto>> getByCourse(String courseId) async {
    await _simulateDelay();

    return _quizzes.values
        .where((quiz) => quiz.courseId == courseId)
        .toList();
  }

  /// Submit quiz attempt
  static Future<QuizAttemptDto> submitAttempt({
    required String userId,
    required String quizId,
    required Map<String, String> answers,
  }) async {
    await _simulateDelay();

    final quiz = _quizzes[quizId];
    if (quiz == null) {
      throw Exception('Quiz not found');
    }

    // Calculate score
    int score = 0;
    int totalPoints = 0;

    for (var question in quiz.questions) {
      totalPoints += question.points;
      final userAnswer = answers[question.id];
      if (userAnswer == question.correctAnswer) {
        score += question.points;
      }
    }

    final percentage = (score / totalPoints * 100);
    final passed = percentage >= quiz.passingScore;

    // Get attempt number
    final previousAttempts = _attempts.where(
      (a) => a.userId == userId && a.quizId == quizId,
    ).length;

    final attempt = QuizAttemptDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: userId,
      quizId: quizId,
      courseId: quiz.courseId,
      answers: answers,
      score: score,
      totalPoints: totalPoints,
      percentage: percentage,
      passed: passed,
      attemptNumber: previousAttempts + 1,
      startTime: DateTime.now().subtract(const Duration(minutes: 15)),
      endTime: DateTime.now(),
      timeSpent: 15,
    );

    _attempts.add(attempt);
    return attempt;
  }

  /// Get quiz attempts
  static Future<List<QuizAttemptDto>> getAttempts({
    required String userId,
    String? quizId,
  }) async {
    await _simulateDelay();

    var filtered = _attempts.where((a) => a.userId == userId);

    if (quizId != null) {
      filtered = filtered.where((a) => a.quizId == quizId);
    }

    return filtered.toList()..sort((a, b) => b.startTime.compareTo(a.startTime));
  }

  /// Generate mock quizzes
  static Map<String, QuizDto> _generateMockQuizzes() {
    final now = DateTime.now();
    return {
      'quiz-1': QuizDto(
        id: 'quiz-1',
        title: 'State Management Quiz',
        description: 'Test your knowledge of Flutter state management',
        courseId: 'course-1',
        lessonId: 'lesson-1-4',
        questions: [
          const QuizQuestionDto(
            id: 'q1',
            question: 'What is the main purpose of state management in Flutter?',
            questionType: QuestionType.multipleChoice,
            options: [
              'To make apps look better',
              'To manage and share data across widgets',
              'To improve app performance',
              'To handle routing',
            ],
            correctAnswer: 'To manage and share data across widgets',
            explanation:
                'State management helps manage and share application state across different widgets efficiently.',
            points: 10,
            order: 1,
          ),
          const QuizQuestionDto(
            id: 'q2',
            question: 'Which of the following is NOT a state management solution in Flutter?',
            questionType: QuestionType.multipleChoice,
            options: [
              'Provider',
              'Riverpod',
              'Redux',
              'Express',
            ],
            correctAnswer: 'Express',
            explanation:
                'Express is a Node.js web framework, not a Flutter state management solution.',
            points: 10,
            order: 2,
          ),
          const QuizQuestionDto(
            id: 'q3',
            question: 'StatefulWidget always rebuilds when setState is called',
            questionType: QuestionType.trueFalse,
            options: ['True', 'False'],
            correctAnswer: 'True',
            explanation:
                'When setState is called, the framework schedules a rebuild of the widget.',
            points: 5,
            order: 3,
          ),
          const QuizQuestionDto(
            id: 'q4',
            question: 'What does Riverpod stand for?',
            questionType: QuestionType.multipleChoice,
            options: [
              'River Provider',
              'Provider in reverse',
              'Reactive Provider',
              'River Pod',
            ],
            correctAnswer: 'Provider in reverse',
            explanation:
                'Riverpod is literally "Provider" spelled in reverse, representing an improved version.',
            points: 10,
            order: 4,
          ),
          const QuizQuestionDto(
            id: 'q5',
            question: 'InheritedWidget is used for propagating information down the widget tree',
            questionType: QuestionType.trueFalse,
            options: ['True', 'False'],
            correctAnswer: 'True',
            explanation:
                'InheritedWidget is the base class for widgets that efficiently propagate information down the tree.',
            points: 5,
            order: 5,
          ),
        ],
        passingScore: 70,
        timeLimit: 30,
        maxAttempts: 3,
        shuffleQuestions: true,
        showCorrectAnswers: true,
        creationTime: now.subtract(const Duration(days: 177)),
      ),
      'quiz-2': QuizDto(
        id: 'quiz-2',
        title: 'Python Basics Assessment',
        description: 'Test your Python fundamentals knowledge',
        courseId: 'course-3',
        questions: [
          const QuizQuestionDto(
            id: 'q6',
            question: 'Which data type is mutable in Python?',
            questionType: QuestionType.multipleChoice,
            options: [
              'Tuple',
              'String',
              'List',
              'Integer',
            ],
            correctAnswer: 'List',
            explanation: 'Lists are mutable, meaning their contents can be changed after creation.',
            points: 10,
            order: 1,
          ),
          const QuizQuestionDto(
            id: 'q7',
            question: 'What is the output of: print(type([]))?',
            questionType: QuestionType.multipleChoice,
            options: [
              '<class \'list\'>',
              '<class \'array\'>',
              '<class \'tuple\'>',
              '<class \'dict\'>',
            ],
            correctAnswer: '<class \'list\'>',
            explanation: '[] creates an empty list in Python.',
            points: 10,
            order: 2,
          ),
          const QuizQuestionDto(
            id: 'q8',
            question: 'Python is a dynamically typed language',
            questionType: QuestionType.trueFalse,
            options: ['True', 'False'],
            correctAnswer: 'True',
            explanation:
                'Python is dynamically typed, meaning you don\'t need to declare variable types.',
            points: 5,
            order: 3,
          ),
        ],
        passingScore: 60,
        timeLimit: 20,
        maxAttempts: 5,
        shuffleQuestions: false,
        showCorrectAnswers: true,
        creationTime: now.subtract(const Duration(days: 199)),
      ),
    };
  }
}
