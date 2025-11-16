import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_requirement_dto.freezed.dart';
part 'project_requirement_dto.g.dart';

/// Requirement type enum
enum RequirementType {
  functional,
  nonFunctional,
  business,
  technical;

  String get displayName {
    switch (this) {
      case RequirementType.functional:
        return 'Functional';
      case RequirementType.nonFunctional:
        return 'Non-Functional';
      case RequirementType.business:
        return 'Business';
      case RequirementType.technical:
        return 'Technical';
    }
  }
}

/// Requirement priority enum
enum RequirementPriority {
  mustHave,
  shouldHave,
  couldHave,
  wontHave;

  String get displayName {
    switch (this) {
      case RequirementPriority.mustHave:
        return 'Must Have';
      case RequirementPriority.shouldHave:
        return 'Should Have';
      case RequirementPriority.couldHave:
        return 'Could Have';
      case RequirementPriority.wontHave:
        return "Won't Have";
    }
  }
}

/// Requirement status enum
enum RequirementStatus {
  draft,
  approved,
  inProgress,
  completed,
  rejected;

  String get displayName {
    switch (this) {
      case RequirementStatus.draft:
        return 'Draft';
      case RequirementStatus.approved:
        return 'Approved';
      case RequirementStatus.inProgress:
        return 'In Progress';
      case RequirementStatus.completed:
        return 'Completed';
      case RequirementStatus.rejected:
        return 'Rejected';
    }
  }
}

/// Project requirement DTO
@freezed
abstract class ProjectRequirementDto with _$ProjectRequirementDto {
  const factory ProjectRequirementDto({
    required String id,
    required String projectId,
    required String title,
    required String description,
    required RequirementType requirementType,
    required RequirementPriority priority,
    required RequirementStatus status,
    String? acceptanceCriteria,
    double? estimatedEffort,
    String? parentRequirementId,
    @Default([]) List<String> tags,
    required String createdBy,
    String? approvedBy,
    required DateTime creationTime,
    DateTime? approvalTime,
  }) = _ProjectRequirementDto;

  factory ProjectRequirementDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectRequirementDtoFromJson(json);
}

/// Create/Update requirement DTO
@freezed
abstract class CreateUpdateRequirementDto with _$CreateUpdateRequirementDto {
  const factory CreateUpdateRequirementDto({
    required String projectId,
    required String title,
    required String description,
    required RequirementType requirementType,
    required RequirementPriority priority,
    required RequirementStatus status,
    String? acceptanceCriteria,
    double? estimatedEffort,
    String? parentRequirementId,
    @Default([]) List<String> tags,
  }) = _CreateUpdateRequirementDto;

  factory CreateUpdateRequirementDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateRequirementDtoFromJson(json);
}
