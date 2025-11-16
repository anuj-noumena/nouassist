// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'learning_goal_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_LearningGoalDto _$LearningGoalDtoFromJson(Map<String, dynamic> json) =>
    _LearningGoalDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      title: json['title'] as String,
      description: json['description'] as String?,
      goalType: $enumDecode(_$GoalTypeEnumMap, json['goalType']),
      status: $enumDecode(_$GoalStatusEnumMap, json['status']),
      courseId: json['courseId'] as String?,
      targetValue: (json['targetValue'] as num?)?.toInt(),
      currentValue: (json['currentValue'] as num?)?.toInt(),
      startDate: DateTime.parse(json['startDate'] as String),
      targetDate: json['targetDate'] == null
          ? null
          : DateTime.parse(json['targetDate'] as String),
      completionDate: json['completionDate'] == null
          ? null
          : DateTime.parse(json['completionDate'] as String),
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$LearningGoalDtoToJson(_LearningGoalDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'title': instance.title,
      'description': instance.description,
      'goalType': _$GoalTypeEnumMap[instance.goalType]!,
      'status': _$GoalStatusEnumMap[instance.status]!,
      'courseId': instance.courseId,
      'targetValue': instance.targetValue,
      'currentValue': instance.currentValue,
      'startDate': instance.startDate.toIso8601String(),
      'targetDate': instance.targetDate?.toIso8601String(),
      'completionDate': instance.completionDate?.toIso8601String(),
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$GoalTypeEnumMap = {
  GoalType.completeCourse: 'completeCourse',
  GoalType.earnCertificate: 'earnCertificate',
  GoalType.studyTime: 'studyTime',
  GoalType.skillMastery: 'skillMastery',
};

const _$GoalStatusEnumMap = {
  GoalStatus.active: 'active',
  GoalStatus.completed: 'completed',
  GoalStatus.abandoned: 'abandoned',
};
