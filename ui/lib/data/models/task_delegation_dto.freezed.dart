// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_delegation_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskDelegationDto {

 String get id; String get taskId; String get delegatedFrom; String get delegatedFromName; String get delegatedTo; String get delegatedToName; String? get reason; DateTime get delegationDate; DelegationStatus get acceptanceStatus; DateTime? get responseDate; String? get responseComment;
/// Create a copy of TaskDelegationDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskDelegationDtoCopyWith<TaskDelegationDto> get copyWith => _$TaskDelegationDtoCopyWithImpl<TaskDelegationDto>(this as TaskDelegationDto, _$identity);

  /// Serializes this TaskDelegationDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskDelegationDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.delegatedFrom, delegatedFrom) || other.delegatedFrom == delegatedFrom)&&(identical(other.delegatedFromName, delegatedFromName) || other.delegatedFromName == delegatedFromName)&&(identical(other.delegatedTo, delegatedTo) || other.delegatedTo == delegatedTo)&&(identical(other.delegatedToName, delegatedToName) || other.delegatedToName == delegatedToName)&&(identical(other.reason, reason) || other.reason == reason)&&(identical(other.delegationDate, delegationDate) || other.delegationDate == delegationDate)&&(identical(other.acceptanceStatus, acceptanceStatus) || other.acceptanceStatus == acceptanceStatus)&&(identical(other.responseDate, responseDate) || other.responseDate == responseDate)&&(identical(other.responseComment, responseComment) || other.responseComment == responseComment));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,delegatedFrom,delegatedFromName,delegatedTo,delegatedToName,reason,delegationDate,acceptanceStatus,responseDate,responseComment);

@override
String toString() {
  return 'TaskDelegationDto(id: $id, taskId: $taskId, delegatedFrom: $delegatedFrom, delegatedFromName: $delegatedFromName, delegatedTo: $delegatedTo, delegatedToName: $delegatedToName, reason: $reason, delegationDate: $delegationDate, acceptanceStatus: $acceptanceStatus, responseDate: $responseDate, responseComment: $responseComment)';
}


}

/// @nodoc
abstract mixin class $TaskDelegationDtoCopyWith<$Res>  {
  factory $TaskDelegationDtoCopyWith(TaskDelegationDto value, $Res Function(TaskDelegationDto) _then) = _$TaskDelegationDtoCopyWithImpl;
@useResult
$Res call({
 String id, String taskId, String delegatedFrom, String delegatedFromName, String delegatedTo, String delegatedToName, String? reason, DateTime delegationDate, DelegationStatus acceptanceStatus, DateTime? responseDate, String? responseComment
});




}
/// @nodoc
class _$TaskDelegationDtoCopyWithImpl<$Res>
    implements $TaskDelegationDtoCopyWith<$Res> {
  _$TaskDelegationDtoCopyWithImpl(this._self, this._then);

  final TaskDelegationDto _self;
  final $Res Function(TaskDelegationDto) _then;

/// Create a copy of TaskDelegationDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? taskId = null,Object? delegatedFrom = null,Object? delegatedFromName = null,Object? delegatedTo = null,Object? delegatedToName = null,Object? reason = freezed,Object? delegationDate = null,Object? acceptanceStatus = null,Object? responseDate = freezed,Object? responseComment = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,delegatedFrom: null == delegatedFrom ? _self.delegatedFrom : delegatedFrom // ignore: cast_nullable_to_non_nullable
as String,delegatedFromName: null == delegatedFromName ? _self.delegatedFromName : delegatedFromName // ignore: cast_nullable_to_non_nullable
as String,delegatedTo: null == delegatedTo ? _self.delegatedTo : delegatedTo // ignore: cast_nullable_to_non_nullable
as String,delegatedToName: null == delegatedToName ? _self.delegatedToName : delegatedToName // ignore: cast_nullable_to_non_nullable
as String,reason: freezed == reason ? _self.reason : reason // ignore: cast_nullable_to_non_nullable
as String?,delegationDate: null == delegationDate ? _self.delegationDate : delegationDate // ignore: cast_nullable_to_non_nullable
as DateTime,acceptanceStatus: null == acceptanceStatus ? _self.acceptanceStatus : acceptanceStatus // ignore: cast_nullable_to_non_nullable
as DelegationStatus,responseDate: freezed == responseDate ? _self.responseDate : responseDate // ignore: cast_nullable_to_non_nullable
as DateTime?,responseComment: freezed == responseComment ? _self.responseComment : responseComment // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskDelegationDto].
extension TaskDelegationDtoPatterns on TaskDelegationDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskDelegationDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskDelegationDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskDelegationDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskDelegationDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskDelegationDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskDelegationDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String taskId,  String delegatedFrom,  String delegatedFromName,  String delegatedTo,  String delegatedToName,  String? reason,  DateTime delegationDate,  DelegationStatus acceptanceStatus,  DateTime? responseDate,  String? responseComment)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskDelegationDto() when $default != null:
return $default(_that.id,_that.taskId,_that.delegatedFrom,_that.delegatedFromName,_that.delegatedTo,_that.delegatedToName,_that.reason,_that.delegationDate,_that.acceptanceStatus,_that.responseDate,_that.responseComment);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String taskId,  String delegatedFrom,  String delegatedFromName,  String delegatedTo,  String delegatedToName,  String? reason,  DateTime delegationDate,  DelegationStatus acceptanceStatus,  DateTime? responseDate,  String? responseComment)  $default,) {final _that = this;
switch (_that) {
case _TaskDelegationDto():
return $default(_that.id,_that.taskId,_that.delegatedFrom,_that.delegatedFromName,_that.delegatedTo,_that.delegatedToName,_that.reason,_that.delegationDate,_that.acceptanceStatus,_that.responseDate,_that.responseComment);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String taskId,  String delegatedFrom,  String delegatedFromName,  String delegatedTo,  String delegatedToName,  String? reason,  DateTime delegationDate,  DelegationStatus acceptanceStatus,  DateTime? responseDate,  String? responseComment)?  $default,) {final _that = this;
switch (_that) {
case _TaskDelegationDto() when $default != null:
return $default(_that.id,_that.taskId,_that.delegatedFrom,_that.delegatedFromName,_that.delegatedTo,_that.delegatedToName,_that.reason,_that.delegationDate,_that.acceptanceStatus,_that.responseDate,_that.responseComment);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskDelegationDto implements TaskDelegationDto {
  const _TaskDelegationDto({required this.id, required this.taskId, required this.delegatedFrom, required this.delegatedFromName, required this.delegatedTo, required this.delegatedToName, this.reason, required this.delegationDate, required this.acceptanceStatus, this.responseDate, this.responseComment});
  factory _TaskDelegationDto.fromJson(Map<String, dynamic> json) => _$TaskDelegationDtoFromJson(json);

@override final  String id;
@override final  String taskId;
@override final  String delegatedFrom;
@override final  String delegatedFromName;
@override final  String delegatedTo;
@override final  String delegatedToName;
@override final  String? reason;
@override final  DateTime delegationDate;
@override final  DelegationStatus acceptanceStatus;
@override final  DateTime? responseDate;
@override final  String? responseComment;

/// Create a copy of TaskDelegationDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskDelegationDtoCopyWith<_TaskDelegationDto> get copyWith => __$TaskDelegationDtoCopyWithImpl<_TaskDelegationDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskDelegationDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskDelegationDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.delegatedFrom, delegatedFrom) || other.delegatedFrom == delegatedFrom)&&(identical(other.delegatedFromName, delegatedFromName) || other.delegatedFromName == delegatedFromName)&&(identical(other.delegatedTo, delegatedTo) || other.delegatedTo == delegatedTo)&&(identical(other.delegatedToName, delegatedToName) || other.delegatedToName == delegatedToName)&&(identical(other.reason, reason) || other.reason == reason)&&(identical(other.delegationDate, delegationDate) || other.delegationDate == delegationDate)&&(identical(other.acceptanceStatus, acceptanceStatus) || other.acceptanceStatus == acceptanceStatus)&&(identical(other.responseDate, responseDate) || other.responseDate == responseDate)&&(identical(other.responseComment, responseComment) || other.responseComment == responseComment));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,delegatedFrom,delegatedFromName,delegatedTo,delegatedToName,reason,delegationDate,acceptanceStatus,responseDate,responseComment);

@override
String toString() {
  return 'TaskDelegationDto(id: $id, taskId: $taskId, delegatedFrom: $delegatedFrom, delegatedFromName: $delegatedFromName, delegatedTo: $delegatedTo, delegatedToName: $delegatedToName, reason: $reason, delegationDate: $delegationDate, acceptanceStatus: $acceptanceStatus, responseDate: $responseDate, responseComment: $responseComment)';
}


}

/// @nodoc
abstract mixin class _$TaskDelegationDtoCopyWith<$Res> implements $TaskDelegationDtoCopyWith<$Res> {
  factory _$TaskDelegationDtoCopyWith(_TaskDelegationDto value, $Res Function(_TaskDelegationDto) _then) = __$TaskDelegationDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String taskId, String delegatedFrom, String delegatedFromName, String delegatedTo, String delegatedToName, String? reason, DateTime delegationDate, DelegationStatus acceptanceStatus, DateTime? responseDate, String? responseComment
});




}
/// @nodoc
class __$TaskDelegationDtoCopyWithImpl<$Res>
    implements _$TaskDelegationDtoCopyWith<$Res> {
  __$TaskDelegationDtoCopyWithImpl(this._self, this._then);

  final _TaskDelegationDto _self;
  final $Res Function(_TaskDelegationDto) _then;

/// Create a copy of TaskDelegationDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? taskId = null,Object? delegatedFrom = null,Object? delegatedFromName = null,Object? delegatedTo = null,Object? delegatedToName = null,Object? reason = freezed,Object? delegationDate = null,Object? acceptanceStatus = null,Object? responseDate = freezed,Object? responseComment = freezed,}) {
  return _then(_TaskDelegationDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,delegatedFrom: null == delegatedFrom ? _self.delegatedFrom : delegatedFrom // ignore: cast_nullable_to_non_nullable
as String,delegatedFromName: null == delegatedFromName ? _self.delegatedFromName : delegatedFromName // ignore: cast_nullable_to_non_nullable
as String,delegatedTo: null == delegatedTo ? _self.delegatedTo : delegatedTo // ignore: cast_nullable_to_non_nullable
as String,delegatedToName: null == delegatedToName ? _self.delegatedToName : delegatedToName // ignore: cast_nullable_to_non_nullable
as String,reason: freezed == reason ? _self.reason : reason // ignore: cast_nullable_to_non_nullable
as String?,delegationDate: null == delegationDate ? _self.delegationDate : delegationDate // ignore: cast_nullable_to_non_nullable
as DateTime,acceptanceStatus: null == acceptanceStatus ? _self.acceptanceStatus : acceptanceStatus // ignore: cast_nullable_to_non_nullable
as DelegationStatus,responseDate: freezed == responseDate ? _self.responseDate : responseDate // ignore: cast_nullable_to_non_nullable
as DateTime?,responseComment: freezed == responseComment ? _self.responseComment : responseComment // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}


/// @nodoc
mixin _$CreateDelegationDto {

 String get taskId; String get delegatedTo; String? get reason;
/// Create a copy of CreateDelegationDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateDelegationDtoCopyWith<CreateDelegationDto> get copyWith => _$CreateDelegationDtoCopyWithImpl<CreateDelegationDto>(this as CreateDelegationDto, _$identity);

  /// Serializes this CreateDelegationDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateDelegationDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.delegatedTo, delegatedTo) || other.delegatedTo == delegatedTo)&&(identical(other.reason, reason) || other.reason == reason));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,delegatedTo,reason);

@override
String toString() {
  return 'CreateDelegationDto(taskId: $taskId, delegatedTo: $delegatedTo, reason: $reason)';
}


}

/// @nodoc
abstract mixin class $CreateDelegationDtoCopyWith<$Res>  {
  factory $CreateDelegationDtoCopyWith(CreateDelegationDto value, $Res Function(CreateDelegationDto) _then) = _$CreateDelegationDtoCopyWithImpl;
@useResult
$Res call({
 String taskId, String delegatedTo, String? reason
});




}
/// @nodoc
class _$CreateDelegationDtoCopyWithImpl<$Res>
    implements $CreateDelegationDtoCopyWith<$Res> {
  _$CreateDelegationDtoCopyWithImpl(this._self, this._then);

  final CreateDelegationDto _self;
  final $Res Function(CreateDelegationDto) _then;

/// Create a copy of CreateDelegationDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? taskId = null,Object? delegatedTo = null,Object? reason = freezed,}) {
  return _then(_self.copyWith(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,delegatedTo: null == delegatedTo ? _self.delegatedTo : delegatedTo // ignore: cast_nullable_to_non_nullable
as String,reason: freezed == reason ? _self.reason : reason // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateDelegationDto].
extension CreateDelegationDtoPatterns on CreateDelegationDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateDelegationDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateDelegationDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateDelegationDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateDelegationDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateDelegationDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateDelegationDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String taskId,  String delegatedTo,  String? reason)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateDelegationDto() when $default != null:
return $default(_that.taskId,_that.delegatedTo,_that.reason);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String taskId,  String delegatedTo,  String? reason)  $default,) {final _that = this;
switch (_that) {
case _CreateDelegationDto():
return $default(_that.taskId,_that.delegatedTo,_that.reason);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String taskId,  String delegatedTo,  String? reason)?  $default,) {final _that = this;
switch (_that) {
case _CreateDelegationDto() when $default != null:
return $default(_that.taskId,_that.delegatedTo,_that.reason);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateDelegationDto implements CreateDelegationDto {
  const _CreateDelegationDto({required this.taskId, required this.delegatedTo, this.reason});
  factory _CreateDelegationDto.fromJson(Map<String, dynamic> json) => _$CreateDelegationDtoFromJson(json);

@override final  String taskId;
@override final  String delegatedTo;
@override final  String? reason;

/// Create a copy of CreateDelegationDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateDelegationDtoCopyWith<_CreateDelegationDto> get copyWith => __$CreateDelegationDtoCopyWithImpl<_CreateDelegationDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateDelegationDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateDelegationDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.delegatedTo, delegatedTo) || other.delegatedTo == delegatedTo)&&(identical(other.reason, reason) || other.reason == reason));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,delegatedTo,reason);

@override
String toString() {
  return 'CreateDelegationDto(taskId: $taskId, delegatedTo: $delegatedTo, reason: $reason)';
}


}

/// @nodoc
abstract mixin class _$CreateDelegationDtoCopyWith<$Res> implements $CreateDelegationDtoCopyWith<$Res> {
  factory _$CreateDelegationDtoCopyWith(_CreateDelegationDto value, $Res Function(_CreateDelegationDto) _then) = __$CreateDelegationDtoCopyWithImpl;
@override @useResult
$Res call({
 String taskId, String delegatedTo, String? reason
});




}
/// @nodoc
class __$CreateDelegationDtoCopyWithImpl<$Res>
    implements _$CreateDelegationDtoCopyWith<$Res> {
  __$CreateDelegationDtoCopyWithImpl(this._self, this._then);

  final _CreateDelegationDto _self;
  final $Res Function(_CreateDelegationDto) _then;

/// Create a copy of CreateDelegationDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? taskId = null,Object? delegatedTo = null,Object? reason = freezed,}) {
  return _then(_CreateDelegationDto(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,delegatedTo: null == delegatedTo ? _self.delegatedTo : delegatedTo // ignore: cast_nullable_to_non_nullable
as String,reason: freezed == reason ? _self.reason : reason // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
