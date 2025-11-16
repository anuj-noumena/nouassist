import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_inbox_dto.freezed.dart';
part 'gtd_inbox_dto.g.dart';

/// GTD Inbox item status
enum InboxStatus {
  unprocessed,
  processing,
  processed;

  String get displayName {
    switch (this) {
      case InboxStatus.unprocessed:
        return 'Unprocessed';
      case InboxStatus.processing:
        return 'Processing';
      case InboxStatus.processed:
        return 'Processed';
    }
  }
}

/// GTD Inbox item type
enum InboxItemType {
  thought,
  email,
  note,
  task,
  idea,
  meeting,
  other;

  String get displayName {
    switch (this) {
      case InboxItemType.thought:
        return 'Thought';
      case InboxItemType.email:
        return 'Email';
      case InboxItemType.note:
        return 'Note';
      case InboxItemType.task:
        return 'Task';
      case InboxItemType.idea:
        return 'Idea';
      case InboxItemType.meeting:
        return 'Meeting';
      case InboxItemType.other:
        return 'Other';
    }
  }
}

/// GTD Inbox DTO
@freezed
abstract class GtdInboxDto with _$GtdInboxDto {
  const factory GtdInboxDto({
    required String id,
    required String userId,
    required String content,
    String? notes,
    required InboxItemType itemType,
    required InboxStatus status,
    @Default([]) List<String> tags,
    String? source,
    DateTime? processedDate,
    String? processedAs,
    required DateTime creationTime,
  }) = _GtdInboxDto;

  factory GtdInboxDto.fromJson(Map<String, dynamic> json) =>
      _$GtdInboxDtoFromJson(json);
}

/// Create/Update inbox item DTO
@freezed
abstract class CreateUpdateInboxDto with _$CreateUpdateInboxDto {
  const factory CreateUpdateInboxDto({
    required String userId,
    required String content,
    String? notes,
    required InboxItemType itemType,
    @Default([]) List<String> tags,
    String? source,
  }) = _CreateUpdateInboxDto;

  factory CreateUpdateInboxDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateInboxDtoFromJson(json);
}
