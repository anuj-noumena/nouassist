import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_context_dto.freezed.dart';
part 'gtd_context_dto.g.dart';

/// Context type
enum ContextType {
  location,
  tool,
  person,
  energy,
  time,
  other;

  String get displayName {
    switch (this) {
      case ContextType.location:
        return 'Location';
      case ContextType.tool:
        return 'Tool';
      case ContextType.person:
        return 'Person';
      case ContextType.energy:
        return 'Energy';
      case ContextType.time:
        return 'Time';
      case ContextType.other:
        return 'Other';
    }
  }
}

/// GTD Context DTO
@freezed
abstract class GtdContextDto with _$GtdContextDto {
  const factory GtdContextDto({
    required String id,
    required String userId,
    required String name,
    String? description,
    required String icon,
    required String color,
    required ContextType contextType,
    @Default(0) int actionCount,
    @Default(true) bool isActive,
    required DateTime creationTime,
  }) = _GtdContextDto;

  factory GtdContextDto.fromJson(Map<String, dynamic> json) =>
      _$GtdContextDtoFromJson(json);
}

/// Create/Update context DTO
@freezed
abstract class CreateUpdateContextDto with _$CreateUpdateContextDto {
  const factory CreateUpdateContextDto({
    required String userId,
    required String name,
    String? description,
    required String icon,
    required String color,
    required ContextType contextType,
    @Default(true) bool isActive,
  }) = _CreateUpdateContextDto;

  factory CreateUpdateContextDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateContextDtoFromJson(json);
}
