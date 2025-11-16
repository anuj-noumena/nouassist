// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'quiz_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_QuizQuestionDto _$QuizQuestionDtoFromJson(Map<String, dynamic> json) =>
    _QuizQuestionDto(
      id: json['id'] as String,
      question: json['question'] as String,
      questionType: $enumDecode(_$QuestionTypeEnumMap, json['questionType']),
      options: (json['options'] as List<dynamic>)
          .map((e) => e as String)
          .toList(),
      correctAnswer: json['correctAnswer'] as String,
      explanation: json['explanation'] as String?,
      points: (json['points'] as num).toInt(),
      order: (json['order'] as num).toInt(),
    );

Map<String, dynamic> _$QuizQuestionDtoToJson(_QuizQuestionDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'question': instance.question,
      'questionType': _$QuestionTypeEnumMap[instance.questionType]!,
      'options': instance.options,
      'correctAnswer': instance.correctAnswer,
      'explanation': instance.explanation,
      'points': instance.points,
      'order': instance.order,
    };

const _$QuestionTypeEnumMap = {
  QuestionType.multipleChoice: 'multipleChoice',
  QuestionType.trueFalse: 'trueFalse',
  QuestionType.shortAnswer: 'shortAnswer',
  QuestionType.essay: 'essay',
};

_QuizDto _$QuizDtoFromJson(Map<String, dynamic> json) => _QuizDto(
  id: json['id'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  courseId: json['courseId'] as String,
  lessonId: json['lessonId'] as String?,
  questions: (json['questions'] as List<dynamic>)
      .map((e) => QuizQuestionDto.fromJson(e as Map<String, dynamic>))
      .toList(),
  passingScore: (json['passingScore'] as num).toInt(),
  timeLimit: (json['timeLimit'] as num).toInt(),
  maxAttempts: (json['maxAttempts'] as num?)?.toInt() ?? 1,
  shuffleQuestions: json['shuffleQuestions'] as bool? ?? false,
  showCorrectAnswers: json['showCorrectAnswers'] as bool? ?? false,
  creationTime: DateTime.parse(json['creationTime'] as String),
);

Map<String, dynamic> _$QuizDtoToJson(_QuizDto instance) => <String, dynamic>{
  'id': instance.id,
  'title': instance.title,
  'description': instance.description,
  'courseId': instance.courseId,
  'lessonId': instance.lessonId,
  'questions': instance.questions,
  'passingScore': instance.passingScore,
  'timeLimit': instance.timeLimit,
  'maxAttempts': instance.maxAttempts,
  'shuffleQuestions': instance.shuffleQuestions,
  'showCorrectAnswers': instance.showCorrectAnswers,
  'creationTime': instance.creationTime.toIso8601String(),
};

_QuizAttemptDto _$QuizAttemptDtoFromJson(Map<String, dynamic> json) =>
    _QuizAttemptDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      quizId: json['quizId'] as String,
      courseId: json['courseId'] as String,
      answers: Map<String, String>.from(json['answers'] as Map),
      score: (json['score'] as num).toInt(),
      totalPoints: (json['totalPoints'] as num).toInt(),
      percentage: (json['percentage'] as num).toDouble(),
      passed: json['passed'] as bool? ?? false,
      attemptNumber: (json['attemptNumber'] as num).toInt(),
      startTime: DateTime.parse(json['startTime'] as String),
      endTime: json['endTime'] == null
          ? null
          : DateTime.parse(json['endTime'] as String),
      timeSpent: (json['timeSpent'] as num).toInt(),
    );

Map<String, dynamic> _$QuizAttemptDtoToJson(_QuizAttemptDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'quizId': instance.quizId,
      'courseId': instance.courseId,
      'answers': instance.answers,
      'score': instance.score,
      'totalPoints': instance.totalPoints,
      'percentage': instance.percentage,
      'passed': instance.passed,
      'attemptNumber': instance.attemptNumber,
      'startTime': instance.startTime.toIso8601String(),
      'endTime': instance.endTime?.toIso8601String(),
      'timeSpent': instance.timeSpent,
    };
