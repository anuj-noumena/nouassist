// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_area_focus_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdAreaFocusDto _$GtdAreaFocusDtoFromJson(Map<String, dynamic> json) =>
    _GtdAreaFocusDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      name: json['name'] as String,
      description: json['description'] as String?,
      color: json['color'] as String,
      icon: json['icon'] as String?,
      projectCount: (json['projectCount'] as num?)?.toInt() ?? 0,
      actionCount: (json['actionCount'] as num?)?.toInt() ?? 0,
      isActive: json['isActive'] as bool? ?? true,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdAreaFocusDtoToJson(_GtdAreaFocusDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'name': instance.name,
      'description': instance.description,
      'color': instance.color,
      'icon': instance.icon,
      'projectCount': instance.projectCount,
      'actionCount': instance.actionCount,
      'isActive': instance.isActive,
      'creationTime': instance.creationTime.toIso8601String(),
    };
