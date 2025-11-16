// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'learning_goal_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$LearningGoalDto {

 String get id; String get userId; String get title; String? get description; GoalType get goalType; GoalStatus get status; String? get courseId; int? get targetValue; int? get currentValue; DateTime get startDate; DateTime? get targetDate; DateTime? get completionDate; DateTime get creationTime;
/// Create a copy of LearningGoalDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$LearningGoalDtoCopyWith<LearningGoalDto> get copyWith => _$LearningGoalDtoCopyWithImpl<LearningGoalDto>(this as LearningGoalDto, _$identity);

  /// Serializes this LearningGoalDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is LearningGoalDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.goalType, goalType) || other.goalType == goalType)&&(identical(other.status, status) || other.status == status)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.currentValue, currentValue) || other.currentValue == currentValue)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.targetDate, targetDate) || other.targetDate == targetDate)&&(identical(other.completionDate, completionDate) || other.completionDate == completionDate)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,description,goalType,status,courseId,targetValue,currentValue,startDate,targetDate,completionDate,creationTime);

@override
String toString() {
  return 'LearningGoalDto(id: $id, userId: $userId, title: $title, description: $description, goalType: $goalType, status: $status, courseId: $courseId, targetValue: $targetValue, currentValue: $currentValue, startDate: $startDate, targetDate: $targetDate, completionDate: $completionDate, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $LearningGoalDtoCopyWith<$Res>  {
  factory $LearningGoalDtoCopyWith(LearningGoalDto value, $Res Function(LearningGoalDto) _then) = _$LearningGoalDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String title, String? description, GoalType goalType, GoalStatus status, String? courseId, int? targetValue, int? currentValue, DateTime startDate, DateTime? targetDate, DateTime? completionDate, DateTime creationTime
});




}
/// @nodoc
class _$LearningGoalDtoCopyWithImpl<$Res>
    implements $LearningGoalDtoCopyWith<$Res> {
  _$LearningGoalDtoCopyWithImpl(this._self, this._then);

  final LearningGoalDto _self;
  final $Res Function(LearningGoalDto) _then;

/// Create a copy of LearningGoalDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? goalType = null,Object? status = null,Object? courseId = freezed,Object? targetValue = freezed,Object? currentValue = freezed,Object? startDate = null,Object? targetDate = freezed,Object? completionDate = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,goalType: null == goalType ? _self.goalType : goalType // ignore: cast_nullable_to_non_nullable
as GoalType,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as GoalStatus,courseId: freezed == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String?,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as int?,currentValue: freezed == currentValue ? _self.currentValue : currentValue // ignore: cast_nullable_to_non_nullable
as int?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,targetDate: freezed == targetDate ? _self.targetDate : targetDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completionDate: freezed == completionDate ? _self.completionDate : completionDate // ignore: cast_nullable_to_non_nullable
as DateTime?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [LearningGoalDto].
extension LearningGoalDtoPatterns on LearningGoalDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _LearningGoalDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _LearningGoalDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _LearningGoalDto value)  $default,){
final _that = this;
switch (_that) {
case _LearningGoalDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _LearningGoalDto value)?  $default,){
final _that = this;
switch (_that) {
case _LearningGoalDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  GoalType goalType,  GoalStatus status,  String? courseId,  int? targetValue,  int? currentValue,  DateTime startDate,  DateTime? targetDate,  DateTime? completionDate,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _LearningGoalDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.goalType,_that.status,_that.courseId,_that.targetValue,_that.currentValue,_that.startDate,_that.targetDate,_that.completionDate,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  GoalType goalType,  GoalStatus status,  String? courseId,  int? targetValue,  int? currentValue,  DateTime startDate,  DateTime? targetDate,  DateTime? completionDate,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _LearningGoalDto():
return $default(_that.id,_that.userId,_that.title,_that.description,_that.goalType,_that.status,_that.courseId,_that.targetValue,_that.currentValue,_that.startDate,_that.targetDate,_that.completionDate,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String title,  String? description,  GoalType goalType,  GoalStatus status,  String? courseId,  int? targetValue,  int? currentValue,  DateTime startDate,  DateTime? targetDate,  DateTime? completionDate,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _LearningGoalDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.goalType,_that.status,_that.courseId,_that.targetValue,_that.currentValue,_that.startDate,_that.targetDate,_that.completionDate,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _LearningGoalDto implements LearningGoalDto {
  const _LearningGoalDto({required this.id, required this.userId, required this.title, this.description, required this.goalType, required this.status, this.courseId, this.targetValue, this.currentValue, required this.startDate, this.targetDate, this.completionDate, required this.creationTime});
  factory _LearningGoalDto.fromJson(Map<String, dynamic> json) => _$LearningGoalDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  GoalType goalType;
@override final  GoalStatus status;
@override final  String? courseId;
@override final  int? targetValue;
@override final  int? currentValue;
@override final  DateTime startDate;
@override final  DateTime? targetDate;
@override final  DateTime? completionDate;
@override final  DateTime creationTime;

/// Create a copy of LearningGoalDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$LearningGoalDtoCopyWith<_LearningGoalDto> get copyWith => __$LearningGoalDtoCopyWithImpl<_LearningGoalDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$LearningGoalDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _LearningGoalDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.goalType, goalType) || other.goalType == goalType)&&(identical(other.status, status) || other.status == status)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.currentValue, currentValue) || other.currentValue == currentValue)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.targetDate, targetDate) || other.targetDate == targetDate)&&(identical(other.completionDate, completionDate) || other.completionDate == completionDate)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,description,goalType,status,courseId,targetValue,currentValue,startDate,targetDate,completionDate,creationTime);

@override
String toString() {
  return 'LearningGoalDto(id: $id, userId: $userId, title: $title, description: $description, goalType: $goalType, status: $status, courseId: $courseId, targetValue: $targetValue, currentValue: $currentValue, startDate: $startDate, targetDate: $targetDate, completionDate: $completionDate, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$LearningGoalDtoCopyWith<$Res> implements $LearningGoalDtoCopyWith<$Res> {
  factory _$LearningGoalDtoCopyWith(_LearningGoalDto value, $Res Function(_LearningGoalDto) _then) = __$LearningGoalDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String title, String? description, GoalType goalType, GoalStatus status, String? courseId, int? targetValue, int? currentValue, DateTime startDate, DateTime? targetDate, DateTime? completionDate, DateTime creationTime
});




}
/// @nodoc
class __$LearningGoalDtoCopyWithImpl<$Res>
    implements _$LearningGoalDtoCopyWith<$Res> {
  __$LearningGoalDtoCopyWithImpl(this._self, this._then);

  final _LearningGoalDto _self;
  final $Res Function(_LearningGoalDto) _then;

/// Create a copy of LearningGoalDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? goalType = null,Object? status = null,Object? courseId = freezed,Object? targetValue = freezed,Object? currentValue = freezed,Object? startDate = null,Object? targetDate = freezed,Object? completionDate = freezed,Object? creationTime = null,}) {
  return _then(_LearningGoalDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,goalType: null == goalType ? _self.goalType : goalType // ignore: cast_nullable_to_non_nullable
as GoalType,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as GoalStatus,courseId: freezed == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String?,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as int?,currentValue: freezed == currentValue ? _self.currentValue : currentValue // ignore: cast_nullable_to_non_nullable
as int?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,targetDate: freezed == targetDate ? _self.targetDate : targetDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completionDate: freezed == completionDate ? _self.completionDate : completionDate // ignore: cast_nullable_to_non_nullable
as DateTime?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}

// dart format on
