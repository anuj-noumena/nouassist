// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_inbox_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdInboxDto _$GtdInboxDtoFromJson(Map<String, dynamic> json) => _GtdInboxDto(
  id: json['id'] as String,
  userId: json['userId'] as String,
  content: json['content'] as String,
  notes: json['notes'] as String?,
  itemType: $enumDecode(_$InboxItemTypeEnumMap, json['itemType']),
  status: $enumDecode(_$InboxStatusEnumMap, json['status']),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  source: json['source'] as String?,
  processedDate: json['processedDate'] == null
      ? null
      : DateTime.parse(json['processedDate'] as String),
  processedAs: json['processedAs'] as String?,
  creationTime: DateTime.parse(json['creationTime'] as String),
);

Map<String, dynamic> _$GtdInboxDtoToJson(_GtdInboxDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'content': instance.content,
      'notes': instance.notes,
      'itemType': _$InboxItemTypeEnumMap[instance.itemType]!,
      'status': _$InboxStatusEnumMap[instance.status]!,
      'tags': instance.tags,
      'source': instance.source,
      'processedDate': instance.processedDate?.toIso8601String(),
      'processedAs': instance.processedAs,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$InboxItemTypeEnumMap = {
  InboxItemType.thought: 'thought',
  InboxItemType.email: 'email',
  InboxItemType.note: 'note',
  InboxItemType.task: 'task',
  InboxItemType.idea: 'idea',
  InboxItemType.meeting: 'meeting',
  InboxItemType.other: 'other',
};

const _$InboxStatusEnumMap = {
  InboxStatus.unprocessed: 'unprocessed',
  InboxStatus.processing: 'processing',
  InboxStatus.processed: 'processed',
};

_CreateUpdateInboxDto _$CreateUpdateInboxDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateInboxDto(
  userId: json['userId'] as String,
  content: json['content'] as String,
  notes: json['notes'] as String?,
  itemType: $enumDecode(_$InboxItemTypeEnumMap, json['itemType']),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  source: json['source'] as String?,
);

Map<String, dynamic> _$CreateUpdateInboxDtoToJson(
  _CreateUpdateInboxDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'content': instance.content,
  'notes': instance.notes,
  'itemType': _$InboxItemTypeEnumMap[instance.itemType]!,
  'tags': instance.tags,
  'source': instance.source,
};
