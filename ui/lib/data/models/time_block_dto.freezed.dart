// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'time_block_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$RecurrencePatternDto {

 RecurrenceFrequency get frequency; int get interval; List<int>? get daysOfWeek; int? get dayOfMonth; DateTime? get endDate; int? get occurrences;
/// Create a copy of RecurrencePatternDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<RecurrencePatternDto> get copyWith => _$RecurrencePatternDtoCopyWithImpl<RecurrencePatternDto>(this as RecurrencePatternDto, _$identity);

  /// Serializes this RecurrencePatternDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is RecurrencePatternDto&&(identical(other.frequency, frequency) || other.frequency == frequency)&&(identical(other.interval, interval) || other.interval == interval)&&const DeepCollectionEquality().equals(other.daysOfWeek, daysOfWeek)&&(identical(other.dayOfMonth, dayOfMonth) || other.dayOfMonth == dayOfMonth)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.occurrences, occurrences) || other.occurrences == occurrences));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,frequency,interval,const DeepCollectionEquality().hash(daysOfWeek),dayOfMonth,endDate,occurrences);

@override
String toString() {
  return 'RecurrencePatternDto(frequency: $frequency, interval: $interval, daysOfWeek: $daysOfWeek, dayOfMonth: $dayOfMonth, endDate: $endDate, occurrences: $occurrences)';
}


}

/// @nodoc
abstract mixin class $RecurrencePatternDtoCopyWith<$Res>  {
  factory $RecurrencePatternDtoCopyWith(RecurrencePatternDto value, $Res Function(RecurrencePatternDto) _then) = _$RecurrencePatternDtoCopyWithImpl;
@useResult
$Res call({
 RecurrenceFrequency frequency, int interval, List<int>? daysOfWeek, int? dayOfMonth, DateTime? endDate, int? occurrences
});




}
/// @nodoc
class _$RecurrencePatternDtoCopyWithImpl<$Res>
    implements $RecurrencePatternDtoCopyWith<$Res> {
  _$RecurrencePatternDtoCopyWithImpl(this._self, this._then);

  final RecurrencePatternDto _self;
  final $Res Function(RecurrencePatternDto) _then;

/// Create a copy of RecurrencePatternDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? frequency = null,Object? interval = null,Object? daysOfWeek = freezed,Object? dayOfMonth = freezed,Object? endDate = freezed,Object? occurrences = freezed,}) {
  return _then(_self.copyWith(
frequency: null == frequency ? _self.frequency : frequency // ignore: cast_nullable_to_non_nullable
as RecurrenceFrequency,interval: null == interval ? _self.interval : interval // ignore: cast_nullable_to_non_nullable
as int,daysOfWeek: freezed == daysOfWeek ? _self.daysOfWeek : daysOfWeek // ignore: cast_nullable_to_non_nullable
as List<int>?,dayOfMonth: freezed == dayOfMonth ? _self.dayOfMonth : dayOfMonth // ignore: cast_nullable_to_non_nullable
as int?,endDate: freezed == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime?,occurrences: freezed == occurrences ? _self.occurrences : occurrences // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}

}


/// Adds pattern-matching-related methods to [RecurrencePatternDto].
extension RecurrencePatternDtoPatterns on RecurrencePatternDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _RecurrencePatternDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _RecurrencePatternDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _RecurrencePatternDto value)  $default,){
final _that = this;
switch (_that) {
case _RecurrencePatternDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _RecurrencePatternDto value)?  $default,){
final _that = this;
switch (_that) {
case _RecurrencePatternDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( RecurrenceFrequency frequency,  int interval,  List<int>? daysOfWeek,  int? dayOfMonth,  DateTime? endDate,  int? occurrences)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _RecurrencePatternDto() when $default != null:
return $default(_that.frequency,_that.interval,_that.daysOfWeek,_that.dayOfMonth,_that.endDate,_that.occurrences);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( RecurrenceFrequency frequency,  int interval,  List<int>? daysOfWeek,  int? dayOfMonth,  DateTime? endDate,  int? occurrences)  $default,) {final _that = this;
switch (_that) {
case _RecurrencePatternDto():
return $default(_that.frequency,_that.interval,_that.daysOfWeek,_that.dayOfMonth,_that.endDate,_that.occurrences);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( RecurrenceFrequency frequency,  int interval,  List<int>? daysOfWeek,  int? dayOfMonth,  DateTime? endDate,  int? occurrences)?  $default,) {final _that = this;
switch (_that) {
case _RecurrencePatternDto() when $default != null:
return $default(_that.frequency,_that.interval,_that.daysOfWeek,_that.dayOfMonth,_that.endDate,_that.occurrences);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _RecurrencePatternDto implements RecurrencePatternDto {
  const _RecurrencePatternDto({required this.frequency, required this.interval, final  List<int>? daysOfWeek, this.dayOfMonth, this.endDate, this.occurrences}): _daysOfWeek = daysOfWeek;
  factory _RecurrencePatternDto.fromJson(Map<String, dynamic> json) => _$RecurrencePatternDtoFromJson(json);

@override final  RecurrenceFrequency frequency;
@override final  int interval;
 final  List<int>? _daysOfWeek;
@override List<int>? get daysOfWeek {
  final value = _daysOfWeek;
  if (value == null) return null;
  if (_daysOfWeek is EqualUnmodifiableListView) return _daysOfWeek;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(value);
}

@override final  int? dayOfMonth;
@override final  DateTime? endDate;
@override final  int? occurrences;

/// Create a copy of RecurrencePatternDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$RecurrencePatternDtoCopyWith<_RecurrencePatternDto> get copyWith => __$RecurrencePatternDtoCopyWithImpl<_RecurrencePatternDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$RecurrencePatternDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _RecurrencePatternDto&&(identical(other.frequency, frequency) || other.frequency == frequency)&&(identical(other.interval, interval) || other.interval == interval)&&const DeepCollectionEquality().equals(other._daysOfWeek, _daysOfWeek)&&(identical(other.dayOfMonth, dayOfMonth) || other.dayOfMonth == dayOfMonth)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.occurrences, occurrences) || other.occurrences == occurrences));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,frequency,interval,const DeepCollectionEquality().hash(_daysOfWeek),dayOfMonth,endDate,occurrences);

@override
String toString() {
  return 'RecurrencePatternDto(frequency: $frequency, interval: $interval, daysOfWeek: $daysOfWeek, dayOfMonth: $dayOfMonth, endDate: $endDate, occurrences: $occurrences)';
}


}

/// @nodoc
abstract mixin class _$RecurrencePatternDtoCopyWith<$Res> implements $RecurrencePatternDtoCopyWith<$Res> {
  factory _$RecurrencePatternDtoCopyWith(_RecurrencePatternDto value, $Res Function(_RecurrencePatternDto) _then) = __$RecurrencePatternDtoCopyWithImpl;
@override @useResult
$Res call({
 RecurrenceFrequency frequency, int interval, List<int>? daysOfWeek, int? dayOfMonth, DateTime? endDate, int? occurrences
});




}
/// @nodoc
class __$RecurrencePatternDtoCopyWithImpl<$Res>
    implements _$RecurrencePatternDtoCopyWith<$Res> {
  __$RecurrencePatternDtoCopyWithImpl(this._self, this._then);

  final _RecurrencePatternDto _self;
  final $Res Function(_RecurrencePatternDto) _then;

/// Create a copy of RecurrencePatternDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? frequency = null,Object? interval = null,Object? daysOfWeek = freezed,Object? dayOfMonth = freezed,Object? endDate = freezed,Object? occurrences = freezed,}) {
  return _then(_RecurrencePatternDto(
frequency: null == frequency ? _self.frequency : frequency // ignore: cast_nullable_to_non_nullable
as RecurrenceFrequency,interval: null == interval ? _self.interval : interval // ignore: cast_nullable_to_non_nullable
as int,daysOfWeek: freezed == daysOfWeek ? _self._daysOfWeek : daysOfWeek // ignore: cast_nullable_to_non_nullable
as List<int>?,dayOfMonth: freezed == dayOfMonth ? _self.dayOfMonth : dayOfMonth // ignore: cast_nullable_to_non_nullable
as int?,endDate: freezed == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime?,occurrences: freezed == occurrences ? _self.occurrences : occurrences // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}


}


/// @nodoc
mixin _$TimeBlockDto {

 String get id; String get userId; String get title; String? get description; DateTime get startDateTime; DateTime get endDateTime; TimeBlockType get blockType; String get color; bool get isRecurring; RecurrencePatternDto? get recurrencePattern; String? get taskId; String? get projectId; String? get location; bool get isFlexible; int? get reminderMinutes; DateTime? get actualStartTime; DateTime? get actualEndTime; TimeBlockStatus get status; DateTime get creationTime;
/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TimeBlockDtoCopyWith<TimeBlockDto> get copyWith => _$TimeBlockDtoCopyWithImpl<TimeBlockDto>(this as TimeBlockDto, _$identity);

  /// Serializes this TimeBlockDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TimeBlockDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.blockType, blockType) || other.blockType == blockType)&&(identical(other.color, color) || other.color == color)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.location, location) || other.location == location)&&(identical(other.isFlexible, isFlexible) || other.isFlexible == isFlexible)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.actualStartTime, actualStartTime) || other.actualStartTime == actualStartTime)&&(identical(other.actualEndTime, actualEndTime) || other.actualEndTime == actualEndTime)&&(identical(other.status, status) || other.status == status)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,userId,title,description,startDateTime,endDateTime,blockType,color,isRecurring,recurrencePattern,taskId,projectId,location,isFlexible,reminderMinutes,actualStartTime,actualEndTime,status,creationTime]);

@override
String toString() {
  return 'TimeBlockDto(id: $id, userId: $userId, title: $title, description: $description, startDateTime: $startDateTime, endDateTime: $endDateTime, blockType: $blockType, color: $color, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, taskId: $taskId, projectId: $projectId, location: $location, isFlexible: $isFlexible, reminderMinutes: $reminderMinutes, actualStartTime: $actualStartTime, actualEndTime: $actualEndTime, status: $status, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TimeBlockDtoCopyWith<$Res>  {
  factory $TimeBlockDtoCopyWith(TimeBlockDto value, $Res Function(TimeBlockDto) _then) = _$TimeBlockDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String title, String? description, DateTime startDateTime, DateTime endDateTime, TimeBlockType blockType, String color, bool isRecurring, RecurrencePatternDto? recurrencePattern, String? taskId, String? projectId, String? location, bool isFlexible, int? reminderMinutes, DateTime? actualStartTime, DateTime? actualEndTime, TimeBlockStatus status, DateTime creationTime
});


$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class _$TimeBlockDtoCopyWithImpl<$Res>
    implements $TimeBlockDtoCopyWith<$Res> {
  _$TimeBlockDtoCopyWithImpl(this._self, this._then);

  final TimeBlockDto _self;
  final $Res Function(TimeBlockDto) _then;

/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? startDateTime = null,Object? endDateTime = null,Object? blockType = null,Object? color = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? taskId = freezed,Object? projectId = freezed,Object? location = freezed,Object? isFlexible = null,Object? reminderMinutes = freezed,Object? actualStartTime = freezed,Object? actualEndTime = freezed,Object? status = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,blockType: null == blockType ? _self.blockType : blockType // ignore: cast_nullable_to_non_nullable
as TimeBlockType,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isFlexible: null == isFlexible ? _self.isFlexible : isFlexible // ignore: cast_nullable_to_non_nullable
as bool,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,actualStartTime: freezed == actualStartTime ? _self.actualStartTime : actualStartTime // ignore: cast_nullable_to_non_nullable
as DateTime?,actualEndTime: freezed == actualEndTime ? _self.actualEndTime : actualEndTime // ignore: cast_nullable_to_non_nullable
as DateTime?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as TimeBlockStatus,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}
/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}


/// Adds pattern-matching-related methods to [TimeBlockDto].
extension TimeBlockDtoPatterns on TimeBlockDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TimeBlockDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TimeBlockDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TimeBlockDto value)  $default,){
final _that = this;
switch (_that) {
case _TimeBlockDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TimeBlockDto value)?  $default,){
final _that = this;
switch (_that) {
case _TimeBlockDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes,  DateTime? actualStartTime,  DateTime? actualEndTime,  TimeBlockStatus status,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TimeBlockDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes,_that.actualStartTime,_that.actualEndTime,_that.status,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes,  DateTime? actualStartTime,  DateTime? actualEndTime,  TimeBlockStatus status,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TimeBlockDto():
return $default(_that.id,_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes,_that.actualStartTime,_that.actualEndTime,_that.status,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes,  DateTime? actualStartTime,  DateTime? actualEndTime,  TimeBlockStatus status,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TimeBlockDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes,_that.actualStartTime,_that.actualEndTime,_that.status,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TimeBlockDto implements TimeBlockDto {
  const _TimeBlockDto({required this.id, required this.userId, required this.title, this.description, required this.startDateTime, required this.endDateTime, required this.blockType, required this.color, this.isRecurring = false, this.recurrencePattern, this.taskId, this.projectId, this.location, this.isFlexible = false, this.reminderMinutes, this.actualStartTime, this.actualEndTime, required this.status, required this.creationTime});
  factory _TimeBlockDto.fromJson(Map<String, dynamic> json) => _$TimeBlockDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override final  TimeBlockType blockType;
@override final  String color;
@override@JsonKey() final  bool isRecurring;
@override final  RecurrencePatternDto? recurrencePattern;
@override final  String? taskId;
@override final  String? projectId;
@override final  String? location;
@override@JsonKey() final  bool isFlexible;
@override final  int? reminderMinutes;
@override final  DateTime? actualStartTime;
@override final  DateTime? actualEndTime;
@override final  TimeBlockStatus status;
@override final  DateTime creationTime;

/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TimeBlockDtoCopyWith<_TimeBlockDto> get copyWith => __$TimeBlockDtoCopyWithImpl<_TimeBlockDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TimeBlockDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TimeBlockDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.blockType, blockType) || other.blockType == blockType)&&(identical(other.color, color) || other.color == color)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.location, location) || other.location == location)&&(identical(other.isFlexible, isFlexible) || other.isFlexible == isFlexible)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.actualStartTime, actualStartTime) || other.actualStartTime == actualStartTime)&&(identical(other.actualEndTime, actualEndTime) || other.actualEndTime == actualEndTime)&&(identical(other.status, status) || other.status == status)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,userId,title,description,startDateTime,endDateTime,blockType,color,isRecurring,recurrencePattern,taskId,projectId,location,isFlexible,reminderMinutes,actualStartTime,actualEndTime,status,creationTime]);

@override
String toString() {
  return 'TimeBlockDto(id: $id, userId: $userId, title: $title, description: $description, startDateTime: $startDateTime, endDateTime: $endDateTime, blockType: $blockType, color: $color, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, taskId: $taskId, projectId: $projectId, location: $location, isFlexible: $isFlexible, reminderMinutes: $reminderMinutes, actualStartTime: $actualStartTime, actualEndTime: $actualEndTime, status: $status, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TimeBlockDtoCopyWith<$Res> implements $TimeBlockDtoCopyWith<$Res> {
  factory _$TimeBlockDtoCopyWith(_TimeBlockDto value, $Res Function(_TimeBlockDto) _then) = __$TimeBlockDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String title, String? description, DateTime startDateTime, DateTime endDateTime, TimeBlockType blockType, String color, bool isRecurring, RecurrencePatternDto? recurrencePattern, String? taskId, String? projectId, String? location, bool isFlexible, int? reminderMinutes, DateTime? actualStartTime, DateTime? actualEndTime, TimeBlockStatus status, DateTime creationTime
});


@override $RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class __$TimeBlockDtoCopyWithImpl<$Res>
    implements _$TimeBlockDtoCopyWith<$Res> {
  __$TimeBlockDtoCopyWithImpl(this._self, this._then);

  final _TimeBlockDto _self;
  final $Res Function(_TimeBlockDto) _then;

/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? startDateTime = null,Object? endDateTime = null,Object? blockType = null,Object? color = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? taskId = freezed,Object? projectId = freezed,Object? location = freezed,Object? isFlexible = null,Object? reminderMinutes = freezed,Object? actualStartTime = freezed,Object? actualEndTime = freezed,Object? status = null,Object? creationTime = null,}) {
  return _then(_TimeBlockDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,blockType: null == blockType ? _self.blockType : blockType // ignore: cast_nullable_to_non_nullable
as TimeBlockType,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isFlexible: null == isFlexible ? _self.isFlexible : isFlexible // ignore: cast_nullable_to_non_nullable
as bool,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,actualStartTime: freezed == actualStartTime ? _self.actualStartTime : actualStartTime // ignore: cast_nullable_to_non_nullable
as DateTime?,actualEndTime: freezed == actualEndTime ? _self.actualEndTime : actualEndTime // ignore: cast_nullable_to_non_nullable
as DateTime?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as TimeBlockStatus,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

/// Create a copy of TimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}


/// @nodoc
mixin _$CreateUpdateTimeBlockDto {

 String get userId; String get title; String? get description; DateTime get startDateTime; DateTime get endDateTime; TimeBlockType get blockType; String get color; bool get isRecurring; RecurrencePatternDto? get recurrencePattern; String? get taskId; String? get projectId; String? get location; bool get isFlexible; int? get reminderMinutes;
/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateTimeBlockDtoCopyWith<CreateUpdateTimeBlockDto> get copyWith => _$CreateUpdateTimeBlockDtoCopyWithImpl<CreateUpdateTimeBlockDto>(this as CreateUpdateTimeBlockDto, _$identity);

  /// Serializes this CreateUpdateTimeBlockDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateTimeBlockDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.blockType, blockType) || other.blockType == blockType)&&(identical(other.color, color) || other.color == color)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.location, location) || other.location == location)&&(identical(other.isFlexible, isFlexible) || other.isFlexible == isFlexible)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,title,description,startDateTime,endDateTime,blockType,color,isRecurring,recurrencePattern,taskId,projectId,location,isFlexible,reminderMinutes);

@override
String toString() {
  return 'CreateUpdateTimeBlockDto(userId: $userId, title: $title, description: $description, startDateTime: $startDateTime, endDateTime: $endDateTime, blockType: $blockType, color: $color, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, taskId: $taskId, projectId: $projectId, location: $location, isFlexible: $isFlexible, reminderMinutes: $reminderMinutes)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateTimeBlockDtoCopyWith<$Res>  {
  factory $CreateUpdateTimeBlockDtoCopyWith(CreateUpdateTimeBlockDto value, $Res Function(CreateUpdateTimeBlockDto) _then) = _$CreateUpdateTimeBlockDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String title, String? description, DateTime startDateTime, DateTime endDateTime, TimeBlockType blockType, String color, bool isRecurring, RecurrencePatternDto? recurrencePattern, String? taskId, String? projectId, String? location, bool isFlexible, int? reminderMinutes
});


$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class _$CreateUpdateTimeBlockDtoCopyWithImpl<$Res>
    implements $CreateUpdateTimeBlockDtoCopyWith<$Res> {
  _$CreateUpdateTimeBlockDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateTimeBlockDto _self;
  final $Res Function(CreateUpdateTimeBlockDto) _then;

/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? startDateTime = null,Object? endDateTime = null,Object? blockType = null,Object? color = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? taskId = freezed,Object? projectId = freezed,Object? location = freezed,Object? isFlexible = null,Object? reminderMinutes = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,blockType: null == blockType ? _self.blockType : blockType // ignore: cast_nullable_to_non_nullable
as TimeBlockType,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isFlexible: null == isFlexible ? _self.isFlexible : isFlexible // ignore: cast_nullable_to_non_nullable
as bool,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}
/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}


/// Adds pattern-matching-related methods to [CreateUpdateTimeBlockDto].
extension CreateUpdateTimeBlockDtoPatterns on CreateUpdateTimeBlockDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateTimeBlockDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateTimeBlockDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateTimeBlockDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto():
return $default(_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String title,  String? description,  DateTime startDateTime,  DateTime endDateTime,  TimeBlockType blockType,  String color,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  String? taskId,  String? projectId,  String? location,  bool isFlexible,  int? reminderMinutes)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTimeBlockDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.startDateTime,_that.endDateTime,_that.blockType,_that.color,_that.isRecurring,_that.recurrencePattern,_that.taskId,_that.projectId,_that.location,_that.isFlexible,_that.reminderMinutes);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateTimeBlockDto implements CreateUpdateTimeBlockDto {
  const _CreateUpdateTimeBlockDto({required this.userId, required this.title, this.description, required this.startDateTime, required this.endDateTime, required this.blockType, this.color = '#2196F3', this.isRecurring = false, this.recurrencePattern, this.taskId, this.projectId, this.location, this.isFlexible = false, this.reminderMinutes});
  factory _CreateUpdateTimeBlockDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateTimeBlockDtoFromJson(json);

@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override final  TimeBlockType blockType;
@override@JsonKey() final  String color;
@override@JsonKey() final  bool isRecurring;
@override final  RecurrencePatternDto? recurrencePattern;
@override final  String? taskId;
@override final  String? projectId;
@override final  String? location;
@override@JsonKey() final  bool isFlexible;
@override final  int? reminderMinutes;

/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateTimeBlockDtoCopyWith<_CreateUpdateTimeBlockDto> get copyWith => __$CreateUpdateTimeBlockDtoCopyWithImpl<_CreateUpdateTimeBlockDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateTimeBlockDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateTimeBlockDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.blockType, blockType) || other.blockType == blockType)&&(identical(other.color, color) || other.color == color)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.location, location) || other.location == location)&&(identical(other.isFlexible, isFlexible) || other.isFlexible == isFlexible)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,title,description,startDateTime,endDateTime,blockType,color,isRecurring,recurrencePattern,taskId,projectId,location,isFlexible,reminderMinutes);

@override
String toString() {
  return 'CreateUpdateTimeBlockDto(userId: $userId, title: $title, description: $description, startDateTime: $startDateTime, endDateTime: $endDateTime, blockType: $blockType, color: $color, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, taskId: $taskId, projectId: $projectId, location: $location, isFlexible: $isFlexible, reminderMinutes: $reminderMinutes)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateTimeBlockDtoCopyWith<$Res> implements $CreateUpdateTimeBlockDtoCopyWith<$Res> {
  factory _$CreateUpdateTimeBlockDtoCopyWith(_CreateUpdateTimeBlockDto value, $Res Function(_CreateUpdateTimeBlockDto) _then) = __$CreateUpdateTimeBlockDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String title, String? description, DateTime startDateTime, DateTime endDateTime, TimeBlockType blockType, String color, bool isRecurring, RecurrencePatternDto? recurrencePattern, String? taskId, String? projectId, String? location, bool isFlexible, int? reminderMinutes
});


@override $RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class __$CreateUpdateTimeBlockDtoCopyWithImpl<$Res>
    implements _$CreateUpdateTimeBlockDtoCopyWith<$Res> {
  __$CreateUpdateTimeBlockDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateTimeBlockDto _self;
  final $Res Function(_CreateUpdateTimeBlockDto) _then;

/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? startDateTime = null,Object? endDateTime = null,Object? blockType = null,Object? color = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? taskId = freezed,Object? projectId = freezed,Object? location = freezed,Object? isFlexible = null,Object? reminderMinutes = freezed,}) {
  return _then(_CreateUpdateTimeBlockDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,blockType: null == blockType ? _self.blockType : blockType // ignore: cast_nullable_to_non_nullable
as TimeBlockType,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isFlexible: null == isFlexible ? _self.isFlexible : isFlexible // ignore: cast_nullable_to_non_nullable
as bool,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}

/// Create a copy of CreateUpdateTimeBlockDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}

// dart format on
