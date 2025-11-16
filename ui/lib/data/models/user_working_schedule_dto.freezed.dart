// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'user_working_schedule_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$UserWorkingScheduleDto {

 String get id; String get userId; DayOfWeek get dayOfWeek; bool get isWorkingDay; String? get startTime; String? get endTime; String? get breakStartTime; String? get breakEndTime; String get timezone; DateTime get effectiveFrom; DateTime? get effectiveTo; bool get isDefault;
/// Create a copy of UserWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$UserWorkingScheduleDtoCopyWith<UserWorkingScheduleDto> get copyWith => _$UserWorkingScheduleDtoCopyWithImpl<UserWorkingScheduleDto>(this as UserWorkingScheduleDto, _$identity);

  /// Serializes this UserWorkingScheduleDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is UserWorkingScheduleDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.dayOfWeek, dayOfWeek) || other.dayOfWeek == dayOfWeek)&&(identical(other.isWorkingDay, isWorkingDay) || other.isWorkingDay == isWorkingDay)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.breakStartTime, breakStartTime) || other.breakStartTime == breakStartTime)&&(identical(other.breakEndTime, breakEndTime) || other.breakEndTime == breakEndTime)&&(identical(other.timezone, timezone) || other.timezone == timezone)&&(identical(other.effectiveFrom, effectiveFrom) || other.effectiveFrom == effectiveFrom)&&(identical(other.effectiveTo, effectiveTo) || other.effectiveTo == effectiveTo)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,dayOfWeek,isWorkingDay,startTime,endTime,breakStartTime,breakEndTime,timezone,effectiveFrom,effectiveTo,isDefault);

@override
String toString() {
  return 'UserWorkingScheduleDto(id: $id, userId: $userId, dayOfWeek: $dayOfWeek, isWorkingDay: $isWorkingDay, startTime: $startTime, endTime: $endTime, breakStartTime: $breakStartTime, breakEndTime: $breakEndTime, timezone: $timezone, effectiveFrom: $effectiveFrom, effectiveTo: $effectiveTo, isDefault: $isDefault)';
}


}

/// @nodoc
abstract mixin class $UserWorkingScheduleDtoCopyWith<$Res>  {
  factory $UserWorkingScheduleDtoCopyWith(UserWorkingScheduleDto value, $Res Function(UserWorkingScheduleDto) _then) = _$UserWorkingScheduleDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, DayOfWeek dayOfWeek, bool isWorkingDay, String? startTime, String? endTime, String? breakStartTime, String? breakEndTime, String timezone, DateTime effectiveFrom, DateTime? effectiveTo, bool isDefault
});




}
/// @nodoc
class _$UserWorkingScheduleDtoCopyWithImpl<$Res>
    implements $UserWorkingScheduleDtoCopyWith<$Res> {
  _$UserWorkingScheduleDtoCopyWithImpl(this._self, this._then);

  final UserWorkingScheduleDto _self;
  final $Res Function(UserWorkingScheduleDto) _then;

/// Create a copy of UserWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? dayOfWeek = null,Object? isWorkingDay = null,Object? startTime = freezed,Object? endTime = freezed,Object? breakStartTime = freezed,Object? breakEndTime = freezed,Object? timezone = null,Object? effectiveFrom = null,Object? effectiveTo = freezed,Object? isDefault = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,dayOfWeek: null == dayOfWeek ? _self.dayOfWeek : dayOfWeek // ignore: cast_nullable_to_non_nullable
as DayOfWeek,isWorkingDay: null == isWorkingDay ? _self.isWorkingDay : isWorkingDay // ignore: cast_nullable_to_non_nullable
as bool,startTime: freezed == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as String?,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as String?,breakStartTime: freezed == breakStartTime ? _self.breakStartTime : breakStartTime // ignore: cast_nullable_to_non_nullable
as String?,breakEndTime: freezed == breakEndTime ? _self.breakEndTime : breakEndTime // ignore: cast_nullable_to_non_nullable
as String?,timezone: null == timezone ? _self.timezone : timezone // ignore: cast_nullable_to_non_nullable
as String,effectiveFrom: null == effectiveFrom ? _self.effectiveFrom : effectiveFrom // ignore: cast_nullable_to_non_nullable
as DateTime,effectiveTo: freezed == effectiveTo ? _self.effectiveTo : effectiveTo // ignore: cast_nullable_to_non_nullable
as DateTime?,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [UserWorkingScheduleDto].
extension UserWorkingScheduleDtoPatterns on UserWorkingScheduleDto {
/// A variant of `map` that fallback to returning `orElse`.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case _:
///     return orElse();
/// }
/// ```

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _UserWorkingScheduleDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _UserWorkingScheduleDto() when $default != null:
return $default(_that);case _:
  return orElse();

}
}
/// A `switch`-like method, using callbacks.
///
/// Callbacks receives the raw object, upcasted.
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case final Subclass2 value:
///     return ...;
/// }
/// ```

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _UserWorkingScheduleDto value)  $default,){
final _that = this;
switch (_that) {
case _UserWorkingScheduleDto():
return $default(_that);case _:
  throw StateError('Unexpected subclass');

}
}
/// A variant of `map` that fallback to returning `null`.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case _:
///     return null;
/// }
/// ```

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _UserWorkingScheduleDto value)?  $default,){
final _that = this;
switch (_that) {
case _UserWorkingScheduleDto() when $default != null:
return $default(_that);case _:
  return null;

}
}
/// A variant of `when` that fallback to an `orElse` callback.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case _:
///     return orElse();
/// }
/// ```

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _UserWorkingScheduleDto() when $default != null:
return $default(_that.id,_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  return orElse();

}
}
/// A `switch`-like method, using callbacks.
///
/// As opposed to `map`, this offers destructuring.
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case Subclass2(:final field2):
///     return ...;
/// }
/// ```

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)  $default,) {final _that = this;
switch (_that) {
case _UserWorkingScheduleDto():
return $default(_that.id,_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  throw StateError('Unexpected subclass');

}
}
/// A variant of `when` that fallback to returning `null`
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case _:
///     return null;
/// }
/// ```

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)?  $default,) {final _that = this;
switch (_that) {
case _UserWorkingScheduleDto() when $default != null:
return $default(_that.id,_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _UserWorkingScheduleDto implements UserWorkingScheduleDto {
  const _UserWorkingScheduleDto({required this.id, required this.userId, required this.dayOfWeek, this.isWorkingDay = true, this.startTime, this.endTime, this.breakStartTime, this.breakEndTime, required this.timezone, required this.effectiveFrom, this.effectiveTo, this.isDefault = false});
  factory _UserWorkingScheduleDto.fromJson(Map<String, dynamic> json) => _$UserWorkingScheduleDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  DayOfWeek dayOfWeek;
@override@JsonKey() final  bool isWorkingDay;
@override final  String? startTime;
@override final  String? endTime;
@override final  String? breakStartTime;
@override final  String? breakEndTime;
@override final  String timezone;
@override final  DateTime effectiveFrom;
@override final  DateTime? effectiveTo;
@override@JsonKey() final  bool isDefault;

/// Create a copy of UserWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$UserWorkingScheduleDtoCopyWith<_UserWorkingScheduleDto> get copyWith => __$UserWorkingScheduleDtoCopyWithImpl<_UserWorkingScheduleDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$UserWorkingScheduleDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _UserWorkingScheduleDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.dayOfWeek, dayOfWeek) || other.dayOfWeek == dayOfWeek)&&(identical(other.isWorkingDay, isWorkingDay) || other.isWorkingDay == isWorkingDay)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.breakStartTime, breakStartTime) || other.breakStartTime == breakStartTime)&&(identical(other.breakEndTime, breakEndTime) || other.breakEndTime == breakEndTime)&&(identical(other.timezone, timezone) || other.timezone == timezone)&&(identical(other.effectiveFrom, effectiveFrom) || other.effectiveFrom == effectiveFrom)&&(identical(other.effectiveTo, effectiveTo) || other.effectiveTo == effectiveTo)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,dayOfWeek,isWorkingDay,startTime,endTime,breakStartTime,breakEndTime,timezone,effectiveFrom,effectiveTo,isDefault);

@override
String toString() {
  return 'UserWorkingScheduleDto(id: $id, userId: $userId, dayOfWeek: $dayOfWeek, isWorkingDay: $isWorkingDay, startTime: $startTime, endTime: $endTime, breakStartTime: $breakStartTime, breakEndTime: $breakEndTime, timezone: $timezone, effectiveFrom: $effectiveFrom, effectiveTo: $effectiveTo, isDefault: $isDefault)';
}


}

/// @nodoc
abstract mixin class _$UserWorkingScheduleDtoCopyWith<$Res> implements $UserWorkingScheduleDtoCopyWith<$Res> {
  factory _$UserWorkingScheduleDtoCopyWith(_UserWorkingScheduleDto value, $Res Function(_UserWorkingScheduleDto) _then) = __$UserWorkingScheduleDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, DayOfWeek dayOfWeek, bool isWorkingDay, String? startTime, String? endTime, String? breakStartTime, String? breakEndTime, String timezone, DateTime effectiveFrom, DateTime? effectiveTo, bool isDefault
});




}
/// @nodoc
class __$UserWorkingScheduleDtoCopyWithImpl<$Res>
    implements _$UserWorkingScheduleDtoCopyWith<$Res> {
  __$UserWorkingScheduleDtoCopyWithImpl(this._self, this._then);

  final _UserWorkingScheduleDto _self;
  final $Res Function(_UserWorkingScheduleDto) _then;

/// Create a copy of UserWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? dayOfWeek = null,Object? isWorkingDay = null,Object? startTime = freezed,Object? endTime = freezed,Object? breakStartTime = freezed,Object? breakEndTime = freezed,Object? timezone = null,Object? effectiveFrom = null,Object? effectiveTo = freezed,Object? isDefault = null,}) {
  return _then(_UserWorkingScheduleDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,dayOfWeek: null == dayOfWeek ? _self.dayOfWeek : dayOfWeek // ignore: cast_nullable_to_non_nullable
as DayOfWeek,isWorkingDay: null == isWorkingDay ? _self.isWorkingDay : isWorkingDay // ignore: cast_nullable_to_non_nullable
as bool,startTime: freezed == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as String?,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as String?,breakStartTime: freezed == breakStartTime ? _self.breakStartTime : breakStartTime // ignore: cast_nullable_to_non_nullable
as String?,breakEndTime: freezed == breakEndTime ? _self.breakEndTime : breakEndTime // ignore: cast_nullable_to_non_nullable
as String?,timezone: null == timezone ? _self.timezone : timezone // ignore: cast_nullable_to_non_nullable
as String,effectiveFrom: null == effectiveFrom ? _self.effectiveFrom : effectiveFrom // ignore: cast_nullable_to_non_nullable
as DateTime,effectiveTo: freezed == effectiveTo ? _self.effectiveTo : effectiveTo // ignore: cast_nullable_to_non_nullable
as DateTime?,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateWorkingScheduleDto {

 String get userId; DayOfWeek get dayOfWeek; bool get isWorkingDay; String? get startTime; String? get endTime; String? get breakStartTime; String? get breakEndTime; String get timezone; DateTime get effectiveFrom; DateTime? get effectiveTo; bool get isDefault;
/// Create a copy of CreateUpdateWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateWorkingScheduleDtoCopyWith<CreateUpdateWorkingScheduleDto> get copyWith => _$CreateUpdateWorkingScheduleDtoCopyWithImpl<CreateUpdateWorkingScheduleDto>(this as CreateUpdateWorkingScheduleDto, _$identity);

  /// Serializes this CreateUpdateWorkingScheduleDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateWorkingScheduleDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.dayOfWeek, dayOfWeek) || other.dayOfWeek == dayOfWeek)&&(identical(other.isWorkingDay, isWorkingDay) || other.isWorkingDay == isWorkingDay)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.breakStartTime, breakStartTime) || other.breakStartTime == breakStartTime)&&(identical(other.breakEndTime, breakEndTime) || other.breakEndTime == breakEndTime)&&(identical(other.timezone, timezone) || other.timezone == timezone)&&(identical(other.effectiveFrom, effectiveFrom) || other.effectiveFrom == effectiveFrom)&&(identical(other.effectiveTo, effectiveTo) || other.effectiveTo == effectiveTo)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,dayOfWeek,isWorkingDay,startTime,endTime,breakStartTime,breakEndTime,timezone,effectiveFrom,effectiveTo,isDefault);

@override
String toString() {
  return 'CreateUpdateWorkingScheduleDto(userId: $userId, dayOfWeek: $dayOfWeek, isWorkingDay: $isWorkingDay, startTime: $startTime, endTime: $endTime, breakStartTime: $breakStartTime, breakEndTime: $breakEndTime, timezone: $timezone, effectiveFrom: $effectiveFrom, effectiveTo: $effectiveTo, isDefault: $isDefault)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateWorkingScheduleDtoCopyWith<$Res>  {
  factory $CreateUpdateWorkingScheduleDtoCopyWith(CreateUpdateWorkingScheduleDto value, $Res Function(CreateUpdateWorkingScheduleDto) _then) = _$CreateUpdateWorkingScheduleDtoCopyWithImpl;
@useResult
$Res call({
 String userId, DayOfWeek dayOfWeek, bool isWorkingDay, String? startTime, String? endTime, String? breakStartTime, String? breakEndTime, String timezone, DateTime effectiveFrom, DateTime? effectiveTo, bool isDefault
});




}
/// @nodoc
class _$CreateUpdateWorkingScheduleDtoCopyWithImpl<$Res>
    implements $CreateUpdateWorkingScheduleDtoCopyWith<$Res> {
  _$CreateUpdateWorkingScheduleDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateWorkingScheduleDto _self;
  final $Res Function(CreateUpdateWorkingScheduleDto) _then;

/// Create a copy of CreateUpdateWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? dayOfWeek = null,Object? isWorkingDay = null,Object? startTime = freezed,Object? endTime = freezed,Object? breakStartTime = freezed,Object? breakEndTime = freezed,Object? timezone = null,Object? effectiveFrom = null,Object? effectiveTo = freezed,Object? isDefault = null,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,dayOfWeek: null == dayOfWeek ? _self.dayOfWeek : dayOfWeek // ignore: cast_nullable_to_non_nullable
as DayOfWeek,isWorkingDay: null == isWorkingDay ? _self.isWorkingDay : isWorkingDay // ignore: cast_nullable_to_non_nullable
as bool,startTime: freezed == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as String?,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as String?,breakStartTime: freezed == breakStartTime ? _self.breakStartTime : breakStartTime // ignore: cast_nullable_to_non_nullable
as String?,breakEndTime: freezed == breakEndTime ? _self.breakEndTime : breakEndTime // ignore: cast_nullable_to_non_nullable
as String?,timezone: null == timezone ? _self.timezone : timezone // ignore: cast_nullable_to_non_nullable
as String,effectiveFrom: null == effectiveFrom ? _self.effectiveFrom : effectiveFrom // ignore: cast_nullable_to_non_nullable
as DateTime,effectiveTo: freezed == effectiveTo ? _self.effectiveTo : effectiveTo // ignore: cast_nullable_to_non_nullable
as DateTime?,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateWorkingScheduleDto].
extension CreateUpdateWorkingScheduleDtoPatterns on CreateUpdateWorkingScheduleDto {
/// A variant of `map` that fallback to returning `orElse`.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case _:
///     return orElse();
/// }
/// ```

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateWorkingScheduleDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto() when $default != null:
return $default(_that);case _:
  return orElse();

}
}
/// A `switch`-like method, using callbacks.
///
/// Callbacks receives the raw object, upcasted.
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case final Subclass2 value:
///     return ...;
/// }
/// ```

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateWorkingScheduleDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto():
return $default(_that);case _:
  throw StateError('Unexpected subclass');

}
}
/// A variant of `map` that fallback to returning `null`.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case final Subclass value:
///     return ...;
///   case _:
///     return null;
/// }
/// ```

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateWorkingScheduleDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto() when $default != null:
return $default(_that);case _:
  return null;

}
}
/// A variant of `when` that fallback to an `orElse` callback.
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case _:
///     return orElse();
/// }
/// ```

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto() when $default != null:
return $default(_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  return orElse();

}
}
/// A `switch`-like method, using callbacks.
///
/// As opposed to `map`, this offers destructuring.
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case Subclass2(:final field2):
///     return ...;
/// }
/// ```

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto():
return $default(_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  throw StateError('Unexpected subclass');

}
}
/// A variant of `when` that fallback to returning `null`
///
/// It is equivalent to doing:
/// ```dart
/// switch (sealedClass) {
///   case Subclass(:final field):
///     return ...;
///   case _:
///     return null;
/// }
/// ```

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  DayOfWeek dayOfWeek,  bool isWorkingDay,  String? startTime,  String? endTime,  String? breakStartTime,  String? breakEndTime,  String timezone,  DateTime effectiveFrom,  DateTime? effectiveTo,  bool isDefault)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWorkingScheduleDto() when $default != null:
return $default(_that.userId,_that.dayOfWeek,_that.isWorkingDay,_that.startTime,_that.endTime,_that.breakStartTime,_that.breakEndTime,_that.timezone,_that.effectiveFrom,_that.effectiveTo,_that.isDefault);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateWorkingScheduleDto implements CreateUpdateWorkingScheduleDto {
  const _CreateUpdateWorkingScheduleDto({required this.userId, required this.dayOfWeek, this.isWorkingDay = true, this.startTime, this.endTime, this.breakStartTime, this.breakEndTime, required this.timezone, required this.effectiveFrom, this.effectiveTo, this.isDefault = false});
  factory _CreateUpdateWorkingScheduleDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateWorkingScheduleDtoFromJson(json);

@override final  String userId;
@override final  DayOfWeek dayOfWeek;
@override@JsonKey() final  bool isWorkingDay;
@override final  String? startTime;
@override final  String? endTime;
@override final  String? breakStartTime;
@override final  String? breakEndTime;
@override final  String timezone;
@override final  DateTime effectiveFrom;
@override final  DateTime? effectiveTo;
@override@JsonKey() final  bool isDefault;

/// Create a copy of CreateUpdateWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateWorkingScheduleDtoCopyWith<_CreateUpdateWorkingScheduleDto> get copyWith => __$CreateUpdateWorkingScheduleDtoCopyWithImpl<_CreateUpdateWorkingScheduleDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateWorkingScheduleDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateWorkingScheduleDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.dayOfWeek, dayOfWeek) || other.dayOfWeek == dayOfWeek)&&(identical(other.isWorkingDay, isWorkingDay) || other.isWorkingDay == isWorkingDay)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.breakStartTime, breakStartTime) || other.breakStartTime == breakStartTime)&&(identical(other.breakEndTime, breakEndTime) || other.breakEndTime == breakEndTime)&&(identical(other.timezone, timezone) || other.timezone == timezone)&&(identical(other.effectiveFrom, effectiveFrom) || other.effectiveFrom == effectiveFrom)&&(identical(other.effectiveTo, effectiveTo) || other.effectiveTo == effectiveTo)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,dayOfWeek,isWorkingDay,startTime,endTime,breakStartTime,breakEndTime,timezone,effectiveFrom,effectiveTo,isDefault);

@override
String toString() {
  return 'CreateUpdateWorkingScheduleDto(userId: $userId, dayOfWeek: $dayOfWeek, isWorkingDay: $isWorkingDay, startTime: $startTime, endTime: $endTime, breakStartTime: $breakStartTime, breakEndTime: $breakEndTime, timezone: $timezone, effectiveFrom: $effectiveFrom, effectiveTo: $effectiveTo, isDefault: $isDefault)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateWorkingScheduleDtoCopyWith<$Res> implements $CreateUpdateWorkingScheduleDtoCopyWith<$Res> {
  factory _$CreateUpdateWorkingScheduleDtoCopyWith(_CreateUpdateWorkingScheduleDto value, $Res Function(_CreateUpdateWorkingScheduleDto) _then) = __$CreateUpdateWorkingScheduleDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, DayOfWeek dayOfWeek, bool isWorkingDay, String? startTime, String? endTime, String? breakStartTime, String? breakEndTime, String timezone, DateTime effectiveFrom, DateTime? effectiveTo, bool isDefault
});




}
/// @nodoc
class __$CreateUpdateWorkingScheduleDtoCopyWithImpl<$Res>
    implements _$CreateUpdateWorkingScheduleDtoCopyWith<$Res> {
  __$CreateUpdateWorkingScheduleDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateWorkingScheduleDto _self;
  final $Res Function(_CreateUpdateWorkingScheduleDto) _then;

/// Create a copy of CreateUpdateWorkingScheduleDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? dayOfWeek = null,Object? isWorkingDay = null,Object? startTime = freezed,Object? endTime = freezed,Object? breakStartTime = freezed,Object? breakEndTime = freezed,Object? timezone = null,Object? effectiveFrom = null,Object? effectiveTo = freezed,Object? isDefault = null,}) {
  return _then(_CreateUpdateWorkingScheduleDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,dayOfWeek: null == dayOfWeek ? _self.dayOfWeek : dayOfWeek // ignore: cast_nullable_to_non_nullable
as DayOfWeek,isWorkingDay: null == isWorkingDay ? _self.isWorkingDay : isWorkingDay // ignore: cast_nullable_to_non_nullable
as bool,startTime: freezed == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as String?,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as String?,breakStartTime: freezed == breakStartTime ? _self.breakStartTime : breakStartTime // ignore: cast_nullable_to_non_nullable
as String?,breakEndTime: freezed == breakEndTime ? _self.breakEndTime : breakEndTime // ignore: cast_nullable_to_non_nullable
as String?,timezone: null == timezone ? _self.timezone : timezone // ignore: cast_nullable_to_non_nullable
as String,effectiveFrom: null == effectiveFrom ? _self.effectiveFrom : effectiveFrom // ignore: cast_nullable_to_non_nullable
as DateTime,effectiveTo: freezed == effectiveTo ? _self.effectiveTo : effectiveTo // ignore: cast_nullable_to_non_nullable
as DateTime?,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

// dart format on
