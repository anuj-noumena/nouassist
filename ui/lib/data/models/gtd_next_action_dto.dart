import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_next_action_dto.freezed.dart';
part 'gtd_next_action_dto.g.dart';

/// Next action energy level
enum EnergyLevel {
  high,
  medium,
  low;

  String get displayName {
    switch (this) {
      case EnergyLevel.high:
        return 'High Energy';
      case EnergyLevel.medium:
        return 'Medium Energy';
      case EnergyLevel.low:
        return 'Low Energy';
    }
  }
}

/// Next action time required
enum TimeRequired {
  short,
  medium,
  long;

  String get displayName {
    switch (this) {
      case TimeRequired.short:
        return '< 15 min';
      case TimeRequired.medium:
        return '15-60 min';
      case TimeRequired.long:
        return '> 1 hour';
    }
  }
}

/// GTD Next Action DTO
@freezed
abstract class GtdNextActionDto with _$GtdNextActionDto {
  const factory GtdNextActionDto({
    required String id,
    required String userId,
    required String action,
    String? notes,
    required List<String> contextIds,
    String? projectId,
    String? areaOfFocusId,
    required EnergyLevel energyLevel,
    required TimeRequired timeRequired,
    DateTime? dueDate,
    DateTime? scheduledDate,
    @Default(false) bool isCompleted,
    DateTime? completedDate,
    @Default(false) bool isStarred,
    @Default([]) List<String> tags,
    String? outcome,
    required DateTime creationTime,
  }) = _GtdNextActionDto;

  factory GtdNextActionDto.fromJson(Map<String, dynamic> json) =>
      _$GtdNextActionDtoFromJson(json);
}

/// Create/Update next action DTO
@freezed
abstract class CreateUpdateNextActionDto with _$CreateUpdateNextActionDto {
  const factory CreateUpdateNextActionDto({
    required String userId,
    required String action,
    String? notes,
    @Default([]) List<String> contextIds,
    String? projectId,
    String? areaOfFocusId,
    required EnergyLevel energyLevel,
    required TimeRequired timeRequired,
    DateTime? dueDate,
    DateTime? scheduledDate,
    @Default(false) bool isStarred,
    @Default([]) List<String> tags,
    String? outcome,
  }) = _CreateUpdateNextActionDto;

  factory CreateUpdateNextActionDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateNextActionDtoFromJson(json);
}
