import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_reference_dto.freezed.dart';
part 'gtd_reference_dto.g.dart';

/// Reference type
enum ReferenceType {
  document,
  link,
  note,
  contact,
  other;

  String get displayName {
    switch (this) {
      case ReferenceType.document:
        return 'Document';
      case ReferenceType.link:
        return 'Link';
      case ReferenceType.note:
        return 'Note';
      case ReferenceType.contact:
        return 'Contact';
      case ReferenceType.other:
        return 'Other';
    }
  }
}

/// GTD Reference DTO
@freezed
abstract class GtdReferenceDto with _$GtdReferenceDto {
  const factory GtdReferenceDto({
    required String id,
    required String userId,
    required String title,
    String? content,
    required ReferenceType referenceType,
    String? url,
    @Default([]) List<String> attachments,
    @Default([]) List<String> tags,
    String? category,
    required DateTime creationTime,
  }) = _GtdReferenceDto;

  factory GtdReferenceDto.fromJson(Map<String, dynamic> json) =>
      _$GtdReferenceDtoFromJson(json);
}
