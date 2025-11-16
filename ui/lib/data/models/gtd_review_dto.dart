import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_review_dto.freezed.dart';
part 'gtd_review_dto.g.dart';

/// Review type
enum ReviewType {
  daily,
  weekly,
  monthly,
  quarterly,
  yearly;

  String get displayName {
    switch (this) {
      case ReviewType.daily:
        return 'Daily Review';
      case ReviewType.weekly:
        return 'Weekly Review';
      case ReviewType.monthly:
        return 'Monthly Review';
      case ReviewType.quarterly:
        return 'Quarterly Review';
      case ReviewType.yearly:
        return 'Yearly Review';
    }
  }
}

/// Review checklist item
@freezed
abstract class ReviewChecklistItemDto with _$ReviewChecklistItemDto {
  const factory ReviewChecklistItemDto({
    required String id,
    required String description,
    @Default(false) bool isCompleted,
    required int order,
  }) = _ReviewChecklistItemDto;

  factory ReviewChecklistItemDto.fromJson(Map<String, dynamic> json) =>
      _$ReviewChecklistItemDtoFromJson(json);
}

/// GTD Review DTO
@freezed
abstract class GtdReviewDto with _$GtdReviewDto {
  const factory GtdReviewDto({
    required String id,
    required String userId,
    required ReviewType reviewType,
    required DateTime reviewDate,
    DateTime? completedDate,
    @Default(false) bool isCompleted,
    required List<ReviewChecklistItemDto> checklist,
    String? notes,
    @Default({}) Map<String, dynamic> metrics,
    required DateTime creationTime,
  }) = _GtdReviewDto;

  factory GtdReviewDto.fromJson(Map<String, dynamic> json) =>
      _$GtdReviewDtoFromJson(json);
}
