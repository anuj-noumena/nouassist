// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_review_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_ReviewChecklistItemDto _$ReviewChecklistItemDtoFromJson(
  Map<String, dynamic> json,
) => _ReviewChecklistItemDto(
  id: json['id'] as String,
  description: json['description'] as String,
  isCompleted: json['isCompleted'] as bool? ?? false,
  order: (json['order'] as num).toInt(),
);

Map<String, dynamic> _$ReviewChecklistItemDtoToJson(
  _ReviewChecklistItemDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'description': instance.description,
  'isCompleted': instance.isCompleted,
  'order': instance.order,
};

_GtdReviewDto _$GtdReviewDtoFromJson(Map<String, dynamic> json) =>
    _GtdReviewDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      reviewType: $enumDecode(_$ReviewTypeEnumMap, json['reviewType']),
      reviewDate: DateTime.parse(json['reviewDate'] as String),
      completedDate: json['completedDate'] == null
          ? null
          : DateTime.parse(json['completedDate'] as String),
      isCompleted: json['isCompleted'] as bool? ?? false,
      checklist: (json['checklist'] as List<dynamic>)
          .map(
            (e) => ReviewChecklistItemDto.fromJson(e as Map<String, dynamic>),
          )
          .toList(),
      notes: json['notes'] as String?,
      metrics: json['metrics'] as Map<String, dynamic>? ?? const {},
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdReviewDtoToJson(_GtdReviewDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'reviewType': _$ReviewTypeEnumMap[instance.reviewType]!,
      'reviewDate': instance.reviewDate.toIso8601String(),
      'completedDate': instance.completedDate?.toIso8601String(),
      'isCompleted': instance.isCompleted,
      'checklist': instance.checklist,
      'notes': instance.notes,
      'metrics': instance.metrics,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$ReviewTypeEnumMap = {
  ReviewType.daily: 'daily',
  ReviewType.weekly: 'weekly',
  ReviewType.monthly: 'monthly',
  ReviewType.quarterly: 'quarterly',
  ReviewType.yearly: 'yearly',
};
