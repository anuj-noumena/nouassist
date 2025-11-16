import 'package:freezed_annotation/freezed_annotation.dart';

part 'task_delegation_dto.freezed.dart';
part 'task_delegation_dto.g.dart';

/// Delegation acceptance status
enum DelegationStatus {
  pending,
  accepted,
  rejected;

  String get displayName {
    switch (this) {
      case DelegationStatus.pending:
        return 'Pending';
      case DelegationStatus.accepted:
        return 'Accepted';
      case DelegationStatus.rejected:
        return 'Rejected';
    }
  }
}

/// Task delegation DTO
@freezed
abstract class TaskDelegationDto with _$TaskDelegationDto {
  const factory TaskDelegationDto({
    required String id,
    required String taskId,
    required String delegatedFrom,
    required String delegatedFromName,
    required String delegatedTo,
    required String delegatedToName,
    String? reason,
    required DateTime delegationDate,
    required DelegationStatus acceptanceStatus,
    DateTime? responseDate,
    String? responseComment,
  }) = _TaskDelegationDto;

  factory TaskDelegationDto.fromJson(Map<String, dynamic> json) =>
      _$TaskDelegationDtoFromJson(json);
}

@freezed
abstract class CreateDelegationDto with _$CreateDelegationDto {
  const factory CreateDelegationDto({
    required String taskId,
    required String delegatedTo,
    String? reason,
  }) = _CreateDelegationDto;

  factory CreateDelegationDto.fromJson(Map<String, dynamic> json) =>
      _$CreateDelegationDtoFromJson(json);
}
