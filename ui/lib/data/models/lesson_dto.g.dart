// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'lesson_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_LessonDto _$LessonDtoFromJson(Map<String, dynamic> json) => _LessonDto(
  id: json['id'] as String,
  courseId: json['courseId'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  lessonType: $enumDecode(_$LessonTypeEnumMap, json['lessonType']),
  order: (json['order'] as num).toInt(),
  duration: (json['duration'] as num).toInt(),
  videoUrl: json['videoUrl'] as String?,
  content: json['content'] as String?,
  attachments:
      (json['attachments'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  quizId: json['quizId'] as String?,
  isFree: json['isFree'] as bool? ?? false,
  isCompleted: json['isCompleted'] as bool? ?? false,
  creationTime: DateTime.parse(json['creationTime'] as String),
);

Map<String, dynamic> _$LessonDtoToJson(_LessonDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'courseId': instance.courseId,
      'title': instance.title,
      'description': instance.description,
      'lessonType': _$LessonTypeEnumMap[instance.lessonType]!,
      'order': instance.order,
      'duration': instance.duration,
      'videoUrl': instance.videoUrl,
      'content': instance.content,
      'attachments': instance.attachments,
      'quizId': instance.quizId,
      'isFree': instance.isFree,
      'isCompleted': instance.isCompleted,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$LessonTypeEnumMap = {
  LessonType.video: 'video',
  LessonType.article: 'article',
  LessonType.quiz: 'quiz',
  LessonType.assignment: 'assignment',
  LessonType.exercise: 'exercise',
  LessonType.project: 'project',
};

_CreateUpdateLessonDto _$CreateUpdateLessonDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateLessonDto(
  courseId: json['courseId'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  lessonType: $enumDecode(_$LessonTypeEnumMap, json['lessonType']),
  order: (json['order'] as num).toInt(),
  duration: (json['duration'] as num).toInt(),
  videoUrl: json['videoUrl'] as String?,
  content: json['content'] as String?,
  attachments:
      (json['attachments'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  quizId: json['quizId'] as String?,
  isFree: json['isFree'] as bool? ?? false,
);

Map<String, dynamic> _$CreateUpdateLessonDtoToJson(
  _CreateUpdateLessonDto instance,
) => <String, dynamic>{
  'courseId': instance.courseId,
  'title': instance.title,
  'description': instance.description,
  'lessonType': _$LessonTypeEnumMap[instance.lessonType]!,
  'order': instance.order,
  'duration': instance.duration,
  'videoUrl': instance.videoUrl,
  'content': instance.content,
  'attachments': instance.attachments,
  'quizId': instance.quizId,
  'isFree': instance.isFree,
};
