import 'package:freezed_annotation/freezed_annotation.dart';

part 'paged_result_dto.freezed.dart';
part 'paged_result_dto.g.dart';

/// Generic paged result DTO for list operations
@Freezed(genericArgumentFactories: true)
abstract class PagedResultDto<T> with _$PagedResultDto<T> {
  const factory PagedResultDto({
    required List<T> items,
    required int totalCount,
  }) = _PagedResultDto<T>;

  factory PagedResultDto.fromJson(
    Map<String, dynamic> json,
    T Function(Object?) fromJsonT,
  ) =>
      _$PagedResultDtoFromJson(json, fromJsonT);
}
