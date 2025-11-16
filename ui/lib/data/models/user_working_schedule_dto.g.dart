// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_working_schedule_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_UserWorkingScheduleDto _$UserWorkingScheduleDtoFromJson(
  Map<String, dynamic> json,
) => _UserWorkingScheduleDto(
  id: json['id'] as String,
  userId: json['userId'] as String,
  dayOfWeek: $enumDecode(_$DayOfWeekEnumMap, json['dayOfWeek']),
  isWorkingDay: json['isWorkingDay'] as bool? ?? true,
  startTime: json['startTime'] as String?,
  endTime: json['endTime'] as String?,
  breakStartTime: json['breakStartTime'] as String?,
  breakEndTime: json['breakEndTime'] as String?,
  timezone: json['timezone'] as String,
  effectiveFrom: DateTime.parse(json['effectiveFrom'] as String),
  effectiveTo: json['effectiveTo'] == null
      ? null
      : DateTime.parse(json['effectiveTo'] as String),
  isDefault: json['isDefault'] as bool? ?? false,
);

Map<String, dynamic> _$UserWorkingScheduleDtoToJson(
  _UserWorkingScheduleDto instance,
) => <String, dynamic>{
  'id': instance.id,
  'userId': instance.userId,
  'dayOfWeek': _$DayOfWeekEnumMap[instance.dayOfWeek]!,
  'isWorkingDay': instance.isWorkingDay,
  'startTime': instance.startTime,
  'endTime': instance.endTime,
  'breakStartTime': instance.breakStartTime,
  'breakEndTime': instance.breakEndTime,
  'timezone': instance.timezone,
  'effectiveFrom': instance.effectiveFrom.toIso8601String(),
  'effectiveTo': instance.effectiveTo?.toIso8601String(),
  'isDefault': instance.isDefault,
};

const _$DayOfWeekEnumMap = {
  DayOfWeek.monday: 'monday',
  DayOfWeek.tuesday: 'tuesday',
  DayOfWeek.wednesday: 'wednesday',
  DayOfWeek.thursday: 'thursday',
  DayOfWeek.friday: 'friday',
  DayOfWeek.saturday: 'saturday',
  DayOfWeek.sunday: 'sunday',
};

_CreateUpdateWorkingScheduleDto _$CreateUpdateWorkingScheduleDtoFromJson(
  Map<String, dynamic> json,
) => _CreateUpdateWorkingScheduleDto(
  userId: json['userId'] as String,
  dayOfWeek: $enumDecode(_$DayOfWeekEnumMap, json['dayOfWeek']),
  isWorkingDay: json['isWorkingDay'] as bool? ?? true,
  startTime: json['startTime'] as String?,
  endTime: json['endTime'] as String?,
  breakStartTime: json['breakStartTime'] as String?,
  breakEndTime: json['breakEndTime'] as String?,
  timezone: json['timezone'] as String,
  effectiveFrom: DateTime.parse(json['effectiveFrom'] as String),
  effectiveTo: json['effectiveTo'] == null
      ? null
      : DateTime.parse(json['effectiveTo'] as String),
  isDefault: json['isDefault'] as bool? ?? false,
);

Map<String, dynamic> _$CreateUpdateWorkingScheduleDtoToJson(
  _CreateUpdateWorkingScheduleDto instance,
) => <String, dynamic>{
  'userId': instance.userId,
  'dayOfWeek': _$DayOfWeekEnumMap[instance.dayOfWeek]!,
  'isWorkingDay': instance.isWorkingDay,
  'startTime': instance.startTime,
  'endTime': instance.endTime,
  'breakStartTime': instance.breakStartTime,
  'breakEndTime': instance.breakEndTime,
  'timezone': instance.timezone,
  'effectiveFrom': instance.effectiveFrom.toIso8601String(),
  'effectiveTo': instance.effectiveTo?.toIso8601String(),
  'isDefault': instance.isDefault,
};
