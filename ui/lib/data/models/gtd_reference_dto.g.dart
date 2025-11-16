// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_reference_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdReferenceDto _$GtdReferenceDtoFromJson(Map<String, dynamic> json) =>
    _GtdReferenceDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      title: json['title'] as String,
      content: json['content'] as String?,
      referenceType: $enumDecode(_$ReferenceTypeEnumMap, json['referenceType']),
      url: json['url'] as String?,
      attachments:
          (json['attachments'] as List<dynamic>?)
              ?.map((e) => e as String)
              .toList() ??
          const [],
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          const [],
      category: json['category'] as String?,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdReferenceDtoToJson(_GtdReferenceDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'title': instance.title,
      'content': instance.content,
      'referenceType': _$ReferenceTypeEnumMap[instance.referenceType]!,
      'url': instance.url,
      'attachments': instance.attachments,
      'tags': instance.tags,
      'category': instance.category,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$ReferenceTypeEnumMap = {
  ReferenceType.document: 'document',
  ReferenceType.link: 'link',
  ReferenceType.note: 'note',
  ReferenceType.contact: 'contact',
  ReferenceType.other: 'other',
};
