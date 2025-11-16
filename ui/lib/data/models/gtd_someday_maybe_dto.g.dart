// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_someday_maybe_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_GtdSomedayMaybeDto _$GtdSomedayMaybeDtoFromJson(Map<String, dynamic> json) =>
    _GtdSomedayMaybeDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      title: json['title'] as String,
      description: json['description'] as String?,
      category: $enumDecode(_$SomedayMaybeCategoryEnumMap, json['category']),
      areaOfFocusId: json['areaOfFocusId'] as String?,
      tags:
          (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
          const [],
      reviewDate: json['reviewDate'] == null
          ? null
          : DateTime.parse(json['reviewDate'] as String),
      activatedDate: json['activatedDate'] == null
          ? null
          : DateTime.parse(json['activatedDate'] as String),
      isActivated: json['isActivated'] as bool? ?? false,
      activatedAsProjectId: json['activatedAsProjectId'] as String?,
      creationTime: DateTime.parse(json['creationTime'] as String),
    );

Map<String, dynamic> _$GtdSomedayMaybeDtoToJson(_GtdSomedayMaybeDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'title': instance.title,
      'description': instance.description,
      'category': _$SomedayMaybeCategoryEnumMap[instance.category]!,
      'areaOfFocusId': instance.areaOfFocusId,
      'tags': instance.tags,
      'reviewDate': instance.reviewDate?.toIso8601String(),
      'activatedDate': instance.activatedDate?.toIso8601String(),
      'isActivated': instance.isActivated,
      'activatedAsProjectId': instance.activatedAsProjectId,
      'creationTime': instance.creationTime.toIso8601String(),
    };

const _$SomedayMaybeCategoryEnumMap = {
  SomedayMaybeCategory.project: 'project',
  SomedayMaybeCategory.skill: 'skill',
  SomedayMaybeCategory.place: 'place',
  SomedayMaybeCategory.purchase: 'purchase',
  SomedayMaybeCategory.hobby: 'hobby',
  SomedayMaybeCategory.experience: 'experience',
  SomedayMaybeCategory.other: 'other',
};

_CreateUpdateSomedayMaybeDto _$CreateUpdateSomedayMaybeDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateSomedayMaybeDto(
  userId: json['userId'] as String,
  title: json['title'] as String,
  description: json['description'] as String?,
  category: $enumDecode(_$SomedayMaybeCategoryEnumMap, json['category']),
  areaOfFocusId: json['areaOfFocusId'] as String?,
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  reviewDate: json['reviewDate'] == null
      ? null
      : DateTime.parse(json['reviewDate'] as String),
);

Map<String, dynamic> _$CreateUpdateSomedayMaybeDtoToJson(
  _CreateUpdateSomedayMaybeDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'title': instance.title,
  'description': instance.description,
  'category': _$SomedayMaybeCategoryEnumMap[instance.category]!,
  'areaOfFocusId': instance.areaOfFocusId,
  'tags': instance.tags,
  'reviewDate': instance.reviewDate?.toIso8601String(),
};
