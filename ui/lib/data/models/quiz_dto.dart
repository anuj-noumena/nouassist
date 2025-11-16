import 'package:freezed_annotation/freezed_annotation.dart';

part 'quiz_dto.freezed.dart';
part 'quiz_dto.g.dart';

/// Question type
enum QuestionType {
  multipleChoice,
  trueFalse,
  shortAnswer,
  essay;

  String get displayName {
    switch (this) {
      case QuestionType.multipleChoice:
        return 'Multiple Choice';
      case QuestionType.trueFalse:
        return 'True/False';
      case QuestionType.shortAnswer:
        return 'Short Answer';
      case QuestionType.essay:
        return 'Essay';
    }
  }
}

/// Quiz question DTO
@freezed
abstract class QuizQuestionDto with _$QuizQuestionDto {
  const factory QuizQuestionDto({
    required String id,
    required String question,
    required QuestionType questionType,
    required List<String> options,
    required String correctAnswer,
    String? explanation,
    required int points,
    required int order,
  }) = _QuizQuestionDto;

  factory QuizQuestionDto.fromJson(Map<String, dynamic> json) =>
      _$QuizQuestionDtoFromJson(json);
}

/// Quiz DTO
@freezed
abstract class QuizDto with _$QuizDto {
  const factory QuizDto({
    required String id,
    required String title,
    required String description,
    required String courseId,
    String? lessonId,
    required List<QuizQuestionDto> questions,
    required int passingScore,
    required int timeLimit,
    @Default(1) int maxAttempts,
    @Default(false) bool shuffleQuestions,
    @Default(false) bool showCorrectAnswers,
    required DateTime creationTime,
  }) = _QuizDto;

  factory QuizDto.fromJson(Map<String, dynamic> json) =>
      _$QuizDtoFromJson(json);
}

/// Quiz attempt DTO
@freezed
abstract class QuizAttemptDto with _$QuizAttemptDto {
  const factory QuizAttemptDto({
    required String id,
    required String userId,
    required String quizId,
    required String courseId,
    required Map<String, String> answers,
    required int score,
    required int totalPoints,
    required double percentage,
    @Default(false) bool passed,
    required int attemptNumber,
    required DateTime startTime,
    DateTime? endTime,
    required int timeSpent,
  }) = _QuizAttemptDto;

  factory QuizAttemptDto.fromJson(Map<String, dynamic> json) =>
      _$QuizAttemptDtoFromJson(json);
}
