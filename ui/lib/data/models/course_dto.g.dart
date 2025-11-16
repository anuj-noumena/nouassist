// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'course_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_CourseDto _$CourseDtoFromJson(Map<String, dynamic> json) => _CourseDto(
  id: json['id'] as String,
  title: json['title'] as String,
  description: json['description'] as String,
  thumbnail: json['thumbnail'] as String?,
  instructorId: json['instructorId'] as String,
  instructorName: json['instructorName'] as String,
  category: $enumDecode(_$CourseCategoryEnumMap, json['category']),
  difficulty: $enumDecode(_$CourseDifficultyEnumMap, json['difficulty']),
  status: $enumDecode(_$CourseStatusEnumMap, json['status']),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  prerequisites:
      (json['prerequisites'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  estimatedDuration: (json['estimatedDuration'] as num).toInt(),
  lessonCount: (json['lessonCount'] as num).toInt(),
  enrollmentCount: (json['enrollmentCount'] as num?)?.toInt() ?? 0,
  rating: (json['rating'] as num?)?.toDouble() ?? 0.0,
  reviewCount: (json['reviewCount'] as num?)?.toInt() ?? 0,
  isFeatured: json['isFeatured'] as bool? ?? false,
  isCertified: json['isCertified'] as bool? ?? false,
  price: (json['price'] as num?)?.toDouble(),
  isFree: json['isFree'] as bool? ?? false,
  creationTime: DateTime.parse(json['creationTime'] as String),
  lastModificationTime: json['lastModificationTime'] == null
      ? null
      : DateTime.parse(json['lastModificationTime'] as String),
  publishedDate: json['publishedDate'] == null
      ? null
      : DateTime.parse(json['publishedDate'] as String),
);

Map<String, dynamic> _$CourseDtoToJson(_CourseDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'title': instance.title,
      'description': instance.description,
      'thumbnail': instance.thumbnail,
      'instructorId': instance.instructorId,
      'instructorName': instance.instructorName,
      'category': _$CourseCategoryEnumMap[instance.category]!,
      'difficulty': _$CourseDifficultyEnumMap[instance.difficulty]!,
      'status': _$CourseStatusEnumMap[instance.status]!,
      'tags': instance.tags,
      'prerequisites': instance.prerequisites,
      'estimatedDuration': instance.estimatedDuration,
      'lessonCount': instance.lessonCount,
      'enrollmentCount': instance.enrollmentCount,
      'rating': instance.rating,
      'reviewCount': instance.reviewCount,
      'isFeatured': instance.isFeatured,
      'isCertified': instance.isCertified,
      'price': instance.price,
      'isFree': instance.isFree,
      'creationTime': instance.creationTime.toIso8601String(),
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'publishedDate': instance.publishedDate?.toIso8601String(),
    };

const _$CourseCategoryEnumMap = {
  CourseCategory.programming: 'programming',
  CourseCategory.design: 'design',
  CourseCategory.business: 'business',
  CourseCategory.marketing: 'marketing',
  CourseCategory.dataScience: 'dataScience',
  CourseCategory.devOps: 'devOps',
  CourseCategory.mobile: 'mobile',
  CourseCategory.web: 'web',
  CourseCategory.other: 'other',
};

const _$CourseDifficultyEnumMap = {
  CourseDifficulty.beginner: 'beginner',
  CourseDifficulty.intermediate: 'intermediate',
  CourseDifficulty.advanced: 'advanced',
  CourseDifficulty.expert: 'expert',
};

const _$CourseStatusEnumMap = {
  CourseStatus.draft: 'draft',
  CourseStatus.published: 'published',
  CourseStatus.archived: 'archived',
};

_CreateUpdateCourseDto _$CreateUpdateCourseDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateCourseDto(
  title: json['title'] as String,
  description: json['description'] as String,
  thumbnail: json['thumbnail'] as String?,
  instructorId: json['instructorId'] as String,
  category: $enumDecode(_$CourseCategoryEnumMap, json['category']),
  difficulty: $enumDecode(_$CourseDifficultyEnumMap, json['difficulty']),
  tags:
      (json['tags'] as List<dynamic>?)?.map((e) => e as String).toList() ??
      const [],
  prerequisites:
      (json['prerequisites'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList() ??
      const [],
  estimatedDuration: (json['estimatedDuration'] as num).toInt(),
  isFeatured: json['isFeatured'] as bool? ?? false,
  isCertified: json['isCertified'] as bool? ?? false,
  price: (json['price'] as num?)?.toDouble(),
  isFree: json['isFree'] as bool? ?? false,
);

Map<String, dynamic> _$CreateUpdateCourseDtoToJson(
  _CreateUpdateCourseDto instance,
) => <String, dynamic>{
  'title': instance.title,
  'description': instance.description,
  'thumbnail': instance.thumbnail,
  'instructorId': instance.instructorId,
  'category': _$CourseCategoryEnumMap[instance.category]!,
  'difficulty': _$CourseDifficultyEnumMap[instance.difficulty]!,
  'tags': instance.tags,
  'prerequisites': instance.prerequisites,
  'estimatedDuration': instance.estimatedDuration,
  'isFeatured': instance.isFeatured,
  'isCertified': instance.isCertified,
  'price': instance.price,
  'isFree': instance.isFree,
};
