// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'learning_progress_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_CourseProgressDto _$CourseProgressDtoFromJson(Map<String, dynamic> json) =>
    _CourseProgressDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      courseId: json['courseId'] as String,
      courseTitle: json['courseTitle'] as String,
      status: $enumDecode(_$ProgressStatusEnumMap, json['status']),
      completionPercentage: (json['completionPercentage'] as num).toDouble(),
      completedLessons: (json['completedLessons'] as num).toInt(),
      totalLessons: (json['totalLessons'] as num).toInt(),
      timeSpent: (json['timeSpent'] as num).toInt(),
      enrollmentDate: json['enrollmentDate'] == null
          ? null
          : DateTime.parse(json['enrollmentDate'] as String),
      completionDate: json['completionDate'] == null
          ? null
          : DateTime.parse(json['completionDate'] as String),
      lastAccessedDate: json['lastAccessedDate'] == null
          ? null
          : DateTime.parse(json['lastAccessedDate'] as String),
      certificateId: json['certificateId'] as String?,
    );

Map<String, dynamic> _$CourseProgressDtoToJson(_CourseProgressDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'courseId': instance.courseId,
      'courseTitle': instance.courseTitle,
      'status': _$ProgressStatusEnumMap[instance.status]!,
      'completionPercentage': instance.completionPercentage,
      'completedLessons': instance.completedLessons,
      'totalLessons': instance.totalLessons,
      'timeSpent': instance.timeSpent,
      'enrollmentDate': instance.enrollmentDate?.toIso8601String(),
      'completionDate': instance.completionDate?.toIso8601String(),
      'lastAccessedDate': instance.lastAccessedDate?.toIso8601String(),
      'certificateId': instance.certificateId,
    };

const _$ProgressStatusEnumMap = {
  ProgressStatus.notStarted: 'notStarted',
  ProgressStatus.inProgress: 'inProgress',
  ProgressStatus.completed: 'completed',
};

_LessonProgressDto _$LessonProgressDtoFromJson(Map<String, dynamic> json) =>
    _LessonProgressDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      lessonId: json['lessonId'] as String,
      courseId: json['courseId'] as String,
      isCompleted: json['isCompleted'] as bool? ?? false,
      timeSpent: (json['timeSpent'] as num).toInt(),
      videoProgress: (json['videoProgress'] as num?)?.toInt(),
      startedDate: json['startedDate'] == null
          ? null
          : DateTime.parse(json['startedDate'] as String),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      lastAccessedDate: json['lastAccessedDate'] == null
          ? null
          : DateTime.parse(json['lastAccessedDate'] as String),
    );

Map<String, dynamic> _$LessonProgressDtoToJson(_LessonProgressDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'lessonId': instance.lessonId,
      'courseId': instance.courseId,
      'isCompleted': instance.isCompleted,
      'timeSpent': instance.timeSpent,
      'videoProgress': instance.videoProgress,
      'startedDate': instance.startedDate?.toIso8601String(),
      'completedDate': instance.completedDate?.toIso8601String(),
      'lastAccessedDate': instance.lastAccessedDate?.toIso8601String(),
    };
