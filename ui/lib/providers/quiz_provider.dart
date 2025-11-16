import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/quiz_dto.dart';
import '../data/mock/mock_quiz_service.dart';

part 'quiz_provider.g.dart';

@riverpod
Future<QuizDto> quiz(Ref ref, String id) async {
  return await MockQuizService.get(id);
}

@riverpod
Future<List<QuizDto>> courseQuizzes(Ref ref,
  String courseId,
) async {
  return await MockQuizService.getByCourse(courseId);
}

@riverpod
Future<QuizAttemptDto> submitQuiz(Ref ref, {
  required String userId,
  required String quizId,
  required Map<String, String> answers,
}) async {
  return await MockQuizService.submitAttempt(
    userId: userId,
    quizId: quizId,
    answers: answers,
  );
}

@riverpod
Future<List<QuizAttemptDto>> quizAttempts(Ref ref,
  String userId, {
  String? quizId,
}) async {
  return await MockQuizService.getAttempts(
    userId: userId,
    quizId: quizId,
  );
}
