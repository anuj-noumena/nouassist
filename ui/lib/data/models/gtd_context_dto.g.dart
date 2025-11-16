// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_context_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdContextDto _$GtdContextDtoFromJson(Map<String, dynamic> json) =>
    _GtdContextDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      name: json['name'] as String,
      description: json['description'] as String?,
      icon: json['icon'] as String,
      color: json['color'] as String,
      contextType: $enumDecode(_$ContextTypeEnumMap, json['contextType']),
      actionCount: (json['actionCount'] as num?)?.toInt() ?? 0,
      isActive: json['isActive'] as bool? ?? true,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdContextDtoToJson(_GtdContextDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'name': instance.name,
      'description': instance.description,
      'icon': instance.icon,
      'color': instance.color,
      'contextType': _$ContextTypeEnumMap[instance.contextType]!,
      'actionCount': instance.actionCount,
      'isActive': instance.isActive,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$ContextTypeEnumMap = {
  ContextType.location: 'location',
  ContextType.tool: 'tool',
  ContextType.person: 'person',
  ContextType.energy: 'energy',
  ContextType.time: 'time',
  ContextType.other: 'other',
};

_CreateUpdateContextDto _$CreateUpdateContextDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateContextDto(
  userId: json['userId'] as String,
  name: json['name'] as String,
  description: json['description'] as String?,
  icon: json['icon'] as String,
  color: json['color'] as String,
  contextType: $enumDecode(_$ContextTypeEnumMap, json['contextType']),
  isActive: json['isActive'] as bool? ?? true,
);

Map<String, dynamic> _$CreateUpdateContextDtoToJson(
  _CreateUpdateContextDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'name': instance.name,
  'description': instance.description,
  'icon': instance.icon,
  'color': instance.color,
  'contextType': _$ContextTypeEnumMap[instance.contextType]!,
  'isActive': instance.isActive,
};
