import 'package:freezed_annotation/freezed_annotation.dart';

part 'course_dto.freezed.dart';
part 'course_dto.g.dart';

/// Course difficulty level
enum CourseDifficulty {
  beginner,
  intermediate,
  advanced,
  expert;

  String get displayName {
    switch (this) {
      case CourseDifficulty.beginner:
        return 'Beginner';
      case CourseDifficulty.intermediate:
        return 'Intermediate';
      case CourseDifficulty.advanced:
        return 'Advanced';
      case CourseDifficulty.expert:
        return 'Expert';
    }
  }
}

/// Course status
enum CourseStatus {
  draft,
  published,
  archived;

  String get displayName {
    switch (this) {
      case CourseStatus.draft:
        return 'Draft';
      case CourseStatus.published:
        return 'Published';
      case CourseStatus.archived:
        return 'Archived';
    }
  }
}

/// Course category
enum CourseCategory {
  programming,
  design,
  business,
  marketing,
  dataScience,
  devOps,
  mobile,
  web,
  other;

  String get displayName {
    switch (this) {
      case CourseCategory.programming:
        return 'Programming';
      case CourseCategory.design:
        return 'Design';
      case CourseCategory.business:
        return 'Business';
      case CourseCategory.marketing:
        return 'Marketing';
      case CourseCategory.dataScience:
        return 'Data Science';
      case CourseCategory.devOps:
        return 'DevOps';
      case CourseCategory.mobile:
        return 'Mobile Development';
      case CourseCategory.web:
        return 'Web Development';
      case CourseCategory.other:
        return 'Other';
    }
  }
}

/// Course DTO
@freezed
abstract class CourseDto with _$CourseDto {
  const factory CourseDto({
    required String id,
    required String title,
    required String description,
    String? thumbnail,
    required String instructorId,
    required String instructorName,
    required CourseCategory category,
    required CourseDifficulty difficulty,
    required CourseStatus status,
    @Default([]) List<String> tags,
    @Default([]) List<String> prerequisites,
    required int estimatedDuration,
    required int lessonCount,
    @Default(0) int enrollmentCount,
    @Default(0.0) double rating,
    @Default(0) int reviewCount,
    @Default(false) bool isFeatured,
    @Default(false) bool isCertified,
    double? price,
    @Default(false) bool isFree,
    required DateTime creationTime,
    DateTime? lastModificationTime,
    DateTime? publishedDate,
  }) = _CourseDto;

  factory CourseDto.fromJson(Map<String, dynamic> json) =>
      _$CourseDtoFromJson(json);
}

/// Create/Update course DTO
@freezed
abstract class CreateUpdateCourseDto with _$CreateUpdateCourseDto {
  const factory CreateUpdateCourseDto({
    required String title,
    required String description,
    String? thumbnail,
    required String instructorId,
    required CourseCategory category,
    required CourseDifficulty difficulty,
    @Default([]) List<String> tags,
    @Default([]) List<String> prerequisites,
    required int estimatedDuration,
    @Default(false) bool isFeatured,
    @Default(false) bool isCertified,
    double? price,
    @Default(false) bool isFree,
  }) = _CreateUpdateCourseDto;

  factory CreateUpdateCourseDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateCourseDtoFromJson(json);
}
