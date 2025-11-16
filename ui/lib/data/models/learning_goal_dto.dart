import 'package:freezed_annotation/freezed_annotation.dart';

part 'learning_goal_dto.freezed.dart';
part 'learning_goal_dto.g.dart';

/// Learning goal type
enum GoalType {
  completeCourse,
  earnCertificate,
  studyTime,
  skillMastery;

  String get displayName {
    switch (this) {
      case GoalType.completeCourse:
        return 'Complete Course';
      case GoalType.earnCertificate:
        return 'Earn Certificate';
      case GoalType.studyTime:
        return 'Study Time';
      case GoalType.skillMastery:
        return 'Skill Mastery';
    }
  }
}

/// Goal status
enum GoalStatus {
  active,
  completed,
  abandoned;

  String get displayName {
    switch (this) {
      case GoalStatus.active:
        return 'Active';
      case GoalStatus.completed:
        return 'Completed';
      case GoalStatus.abandoned:
        return 'Abandoned';
    }
  }
}

/// Learning goal DTO
@freezed
abstract class LearningGoalDto with _$LearningGoalDto {
  const factory LearningGoalDto({
    required String id,
    required String userId,
    required String title,
    String? description,
    required GoalType goalType,
    required GoalStatus status,
    String? courseId,
    int? targetValue,
    int? currentValue,
    required DateTime startDate,
    DateTime? targetDate,
    DateTime? completionDate,
    required DateTime creationTime,
  }) = _LearningGoalDto;

  factory LearningGoalDto.fromJson(Map<String, dynamic> json) =>
      _$LearningGoalDtoFromJson(json);
}
