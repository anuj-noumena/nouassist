import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_area_focus_dto.freezed.dart';
part 'gtd_area_focus_dto.g.dart';

/// GTD Area of Focus DTO
@freezed
abstract class GtdAreaFocusDto with _$GtdAreaFocusDto {
  const factory GtdAreaFocusDto({
    required String id,
    required String userId,
    required String name,
    String? description,
    required String color,
    String? icon,
    @Default(0) int projectCount,
    @Default(0) int actionCount,
    @Default(true) bool isActive,
    required DateTime creationTime,
  }) = _GtdAreaFocusDto;

  factory GtdAreaFocusDto.fromJson(Map<String, dynamic> json) =>
      _$GtdAreaFocusDtoFromJson(json);
}
