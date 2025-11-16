import 'package:freezed_annotation/freezed_annotation.dart';

part 'gtd_waiting_for_dto.freezed.dart';
part 'gtd_waiting_for_dto.g.dart';

/// Waiting for status
enum WaitingForStatus {
  waiting,
  reminded,
  received,
  cancelled;

  String get displayName {
    switch (this) {
      case WaitingForStatus.waiting:
        return 'Waiting';
      case WaitingForStatus.reminded:
        return 'Reminded';
      case WaitingForStatus.received:
        return 'Received';
      case WaitingForStatus.cancelled:
        return 'Cancelled';
    }
  }
}

/// GTD Waiting For DTO
@freezed
abstract class GtdWaitingForDto with _$GtdWaitingForDto {
  const factory GtdWaitingForDto({
    required String id,
    required String userId,
    required String description,
    required String waitingFor,
    String? notes,
    String? projectId,
    String? relatedActionId,
    required WaitingForStatus status,
    DateTime? requestedDate,
    DateTime? expectedDate,
    DateTime? reminderDate,
    DateTime? receivedDate,
    @Default([]) List<String> tags,
    required DateTime creationTime,
  }) = _GtdWaitingForDto;

  factory GtdWaitingForDto.fromJson(Map<String, dynamic> json) =>
      _$GtdWaitingForDtoFromJson(json);
}

/// Create/Update waiting for DTO
@freezed
abstract class CreateUpdateWaitingForDto with _$CreateUpdateWaitingForDto {
  const factory CreateUpdateWaitingForDto({
    required String userId,
    required String description,
    required String waitingFor,
    String? notes,
    String? projectId,
    String? relatedActionId,
    DateTime? requestedDate,
    DateTime? expectedDate,
    DateTime? reminderDate,
    @Default([]) List<String> tags,
  }) = _CreateUpdateWaitingForDto;

  factory CreateUpdateWaitingForDto.fromJson(Map<String, dynamic> json) =>
      _$CreateUpdateWaitingForDtoFromJson(json);
}
