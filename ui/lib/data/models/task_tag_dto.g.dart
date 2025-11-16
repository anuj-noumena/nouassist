// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_tag_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_TaskTagDto _$TaskTagDtoFromJson(Map<String, dynamic> json) => _TaskTagDto(
  id: json['id'] as String,
  name: json['name'] as String,
  color: json['color'] as String,
  description: json['description'] as String?,
  category: json['category'] as String?,
  usageCount: (json['usageCount'] as num?)?.toInt() ?? 0,
  creationTime: DateTime.parse(json['creationTime'] as String),
);

Map<String, dynamic> _$TaskTagDtoToJson(_TaskTagDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'color': instance.color,
      'description': instance.description,
      'category': instance.category,
      'usageCount': instance.usageCount,
      'creationTime': instance.creationTime.toIso8601String(),
    };

_CreateUpdateTaskTagDto _$CreateUpdateTaskTagDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateTaskTagDto(
  name: json['name'] as String,
  color: json['color'] as String,
  description: json['description'] as String?,
  category: json['category'] as String?,
);

Map<String, dynamic> _$CreateUpdateTaskTagDtoToJson(
  _CreateUpdateTaskTagDto instance,
) => <String, dynamic>{
  'name': instance.name,
  'color': instance.color,
  'description': instance.description,
  'category': instance.category,
};
