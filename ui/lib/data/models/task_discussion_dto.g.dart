// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_discussion_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskDiscussionDto _$TaskDiscussionDtoFromJson(
  Map<String, dynamic> json,
) => _TaskDiscussionDto(
  id: json['id'] as String,
  taskId: json['taskId'] as String,
  parentDiscussionId: json['parentDiscussionId'] as String?,
  authorId: json['authorId'] as String,
  authorName: json['authorName'] as String,
  content: json['content'] as String,
  mentions:
      (json['mentions'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  attachmentIds:
      (json['attachmentIds'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  isEdited: json['isEdited'] as bool? ?? false,
  reactions:
      (json['reactions'] as Map<String, dynamic>?)?.map(
        (k, e) =>
            MapEntry(k, (e as List<dynamic>).map((e) => e as String).toList()),
      ) ??
      const {},
  creationTime: DateTime.parse(json['creationTime'] as String),
  lastEditTime: json['lastEditTime'] == null
      ? null
      : DateTime.parse(json['lastEditTime'] as String),
);

Map<String, dynamic> _$TaskDiscussionDtoToJson(_TaskDiscussionDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'taskId': instance.taskId,
      'parentDiscussionId': instance.parentDiscussionId,
      'authorId': instance.authorId,
      'authorName': instance.authorName,
      'content': instance.content,
      'mentions': instance.mentions,
      'attachmentIds': instance.attachmentIds,
      'isEdited': instance.isEdited,
      'reactions': instance.reactions,
      'creationTime': instance.creationTime.toIso8601String(),
      'lastEditTime': instance.lastEditTime?.toIso8601String(),
    };

_CreateUpdateDiscussionDto _$CreateUpdateDiscussionDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateDiscussionDto(
  taskId: json['taskId'] as String,
  parentDiscussionId: json['parentDiscussionId'] as String?,
  content: json['content'] as String,
  mentions:
      (json['mentions'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  attachmentIds:
      (json['attachmentIds'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
);

Map<String, dynamic> _$CreateUpdateDiscussionDtoToJson(
  _CreateUpdateDiscussionDto instance,
) => <String, dynamic>{
  'taskId': instance.taskId,
  'parentDiscussionId': instance.parentDiscussionId,
  'content': instance.content,
  'mentions': instance.mentions,
  'attachmentIds': instance.attachmentIds,
};
