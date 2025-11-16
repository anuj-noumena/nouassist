import 'package:freezed_annotation/freezed_annotation.dart';

part 'project_success_criteria_dto.freezed.dart';
part 'project_success_criteria_dto.g.dart';

/// Measurement type enum
enum MeasurementType {
  quantitative,
  qualitative,
  binary;

  String get displayName {
    switch (this) {
      case MeasurementType.quantitative:
        return 'Quantitative';
      case MeasurementType.qualitative:
        return 'Qualitative';
      case MeasurementType.binary:
        return 'Binary';
    }
  }
}

/// Success criteria status enum
enum SuccessCriteriaStatus {
  notStarted,
  inProgress,
  achieved,
  notAchieved;

  String get displayName {
    switch (this) {
      case SuccessCriteriaStatus.notStarted:
        return 'Not Started';
      case SuccessCriteriaStatus.inProgress:
        return 'In Progress';
      case SuccessCriteriaStatus.achieved:
        return 'Achieved';
      case SuccessCriteriaStatus.notAchieved:
        return 'Not Achieved';
    }
  }
}

/// Project success criteria DTO
@freezed
abstract class ProjectSuccessCriteriaDto with _$ProjectSuccessCriteriaDto {
  const factory ProjectSuccessCriteriaDto({
    required String id,
    required String projectId,
    required String criterion,
    String? description,
    required MeasurementType measurementType,
    String? targetValue,
    String? actualValue,
    String? unit,
    required SuccessCriteriaStatus status,
    required double weight,
    String? verificationMethod,
    required DateTime creationTime,
    DateTime? achievementTime,
  }) = _ProjectSuccessCriteriaDto;

  factory ProjectSuccessCriteriaDto.fromJson(Map<String, dynamic> json) =>
      _$ProjectSuccessCriteriaDtoFromJson(json);
}

/// Create/Update success criteria DTO
@freezed
abstract class CreateUpdateSuccessCriteriaDto with _$CreateUpdateSuccessCriteriaDto {
  const factory CreateUpdateSuccessCriteriaDto({
    required String projectId,
    required String criterion,
    String? description,
    required MeasurementType measurementType,
    String? targetValue,
    String? actualValue,
    String? unit,
    @Default(SuccessCriteriaStatus.notStarted) SuccessCriteriaStatus status,
    @Default(1.0) double weight,
    String? verificationMethod,
  }) = _CreateUpdateSuccessCriteriaDto;

  factory CreateUpdateSuccessCriteriaDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateSuccessCriteriaDtoFromJson(json);
}
