// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'paged_result_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_PagedResultDto<T> _$PagedResultDtoFromJson<T>(
  Map<String, dynamic> json,
  T Function(Object? json) fromJsonT,
) => _PagedResultDto<T>(
  items: (json['items'] as List<dynamic>).map(fromJsonT).toList(),
  totalCount: (json['totalCount'] as num).toInt(),
);

Map<String, dynamic> _$PagedResultDtoToJson<T>(
  _PagedResultDto<T> instance,
  Object? Function(T value) toJsonT,
) => <String, dynamic>{
  'items': instance.items.map(toJsonT).toList(),
  'totalCount': instance.totalCount,
};
