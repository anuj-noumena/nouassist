import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_someday_maybe_dto.freezed.dart';
part 'gtd_someday_maybe_dto.g.dart';

/// Someday/Maybe category
enum SomedayMaybeCategory {
  project,
  skill,
  place,
  purchase,
  hobby,
  experience,
  other;

  String get displayName {
    switch (this) {
      case SomedayMaybeCategory.project:
        return 'Project';
      case SomedayMaybeCategory.skill:
        return 'Skill to Learn';
      case SomedayMaybeCategory.place:
        return 'Place to Visit';
      case SomedayMaybeCategory.purchase:
        return 'Thing to Buy';
      case SomedayMaybeCategory.hobby:
        return 'Hobby';
      case SomedayMaybeCategory.experience:
        return 'Experience';
      case SomedayMaybeCategory.other:
        return 'Other';
    }
  }
}

/// GTD Someday/Maybe DTO
@freezed
abstract class GtdSomedayMaybeDto with _$GtdSomedayMaybeDto {
  const factory GtdSomedayMaybeDto({
    required String id,
    required String userId,
    required String title,
    String? description,
    required SomedayMaybeCategory category,
    String? areaOfFocusId,
    @Default([]) List<String> tags,
    DateTime? reviewDate,
    DateTime? activatedDate,
    @Default(false) bool isActivated,
    String? activatedAsProjectId,
    required DateTime creationTime,
  }) = _GtdSomedayMaybeDto;

  factory GtdSomedayMaybeDto.fromJson(Map<String, dynamic> json) =>
      _$GtdSomedayMaybeDtoFromJson(json);
}

/// Create/Update someday/maybe DTO
@freezed
abstract class CreateUpdateSomedayMaybeDto with _$CreateUpdateSomedayMaybeDto {
  const factory CreateUpdateSomedayMaybeDto({
    required String userId,
    required String title,
    String? description,
    required SomedayMaybeCategory category,
    String? areaOfFocusId,
    @Default([]) List<String> tags,
    DateTime? reviewDate,
  }) = _CreateUpdateSomedayMaybeDto;

  factory CreateUpdateSomedayMaybeDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateSomedayMaybeDtoFromJson(json);
}
