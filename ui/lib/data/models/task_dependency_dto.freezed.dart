// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_dependency_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskDependencyDto {

 String get id; String get predecessorTaskId; String get predecessorTaskTitle; String get successorTaskId; String get successorTaskTitle; DependencyType get dependencyType; int get lagDays; DateTime get creationTime;
/// Create a copy of TaskDependencyDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskDependencyDtoCopyWith<TaskDependencyDto> get copyWith => _$TaskDependencyDtoCopyWithImpl<TaskDependencyDto>(this as TaskDependencyDto, _$identity);

  /// Serializes this TaskDependencyDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskDependencyDto&&(identical(other.id, id) || other.id == id)&&(identical(other.predecessorTaskId, predecessorTaskId) || other.predecessorTaskId == predecessorTaskId)&&(identical(other.predecessorTaskTitle, predecessorTaskTitle) || other.predecessorTaskTitle == predecessorTaskTitle)&&(identical(other.successorTaskId, successorTaskId) || other.successorTaskId == successorTaskId)&&(identical(other.successorTaskTitle, successorTaskTitle) || other.successorTaskTitle == successorTaskTitle)&&(identical(other.dependencyType, dependencyType) || other.dependencyType == dependencyType)&&(identical(other.lagDays, lagDays) || other.lagDays == lagDays)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,predecessorTaskId,predecessorTaskTitle,successorTaskId,successorTaskTitle,dependencyType,lagDays,creationTime);

@override
String toString() {
  return 'TaskDependencyDto(id: $id, predecessorTaskId: $predecessorTaskId, predecessorTaskTitle: $predecessorTaskTitle, successorTaskId: $successorTaskId, successorTaskTitle: $successorTaskTitle, dependencyType: $dependencyType, lagDays: $lagDays, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TaskDependencyDtoCopyWith<$Res>  {
  factory $TaskDependencyDtoCopyWith(TaskDependencyDto value, $Res Function(TaskDependencyDto) _then) = _$TaskDependencyDtoCopyWithImpl;
@useResult
$Res call({
 String id, String predecessorTaskId, String predecessorTaskTitle, String successorTaskId, String successorTaskTitle, DependencyType dependencyType, int lagDays, DateTime creationTime
});




}
/// @nodoc
class _$TaskDependencyDtoCopyWithImpl<$Res>
    implements $TaskDependencyDtoCopyWith<$Res> {
  _$TaskDependencyDtoCopyWithImpl(this._self, this._then);

  final TaskDependencyDto _self;
  final $Res Function(TaskDependencyDto) _then;

/// Create a copy of TaskDependencyDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? predecessorTaskId = null,Object? predecessorTaskTitle = null,Object? successorTaskId = null,Object? successorTaskTitle = null,Object? dependencyType = null,Object? lagDays = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,predecessorTaskId: null == predecessorTaskId ? _self.predecessorTaskId : predecessorTaskId // ignore: cast_nullable_to_non_nullable
as String,predecessorTaskTitle: null == predecessorTaskTitle ? _self.predecessorTaskTitle : predecessorTaskTitle // ignore: cast_nullable_to_non_nullable
as String,successorTaskId: null == successorTaskId ? _self.successorTaskId : successorTaskId // ignore: cast_nullable_to_non_nullable
as String,successorTaskTitle: null == successorTaskTitle ? _self.successorTaskTitle : successorTaskTitle // ignore: cast_nullable_to_non_nullable
as String,dependencyType: null == dependencyType ? _self.dependencyType : dependencyType // ignore: cast_nullable_to_non_nullable
as DependencyType,lagDays: null == lagDays ? _self.lagDays : lagDays // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskDependencyDto].
extension TaskDependencyDtoPatterns on TaskDependencyDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskDependencyDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskDependencyDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskDependencyDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskDependencyDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskDependencyDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskDependencyDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String predecessorTaskId,  String predecessorTaskTitle,  String successorTaskId,  String successorTaskTitle,  DependencyType dependencyType,  int lagDays,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskDependencyDto() when $default != null:
return $default(_that.id,_that.predecessorTaskId,_that.predecessorTaskTitle,_that.successorTaskId,_that.successorTaskTitle,_that.dependencyType,_that.lagDays,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String predecessorTaskId,  String predecessorTaskTitle,  String successorTaskId,  String successorTaskTitle,  DependencyType dependencyType,  int lagDays,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskDependencyDto():
return $default(_that.id,_that.predecessorTaskId,_that.predecessorTaskTitle,_that.successorTaskId,_that.successorTaskTitle,_that.dependencyType,_that.lagDays,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String predecessorTaskId,  String predecessorTaskTitle,  String successorTaskId,  String successorTaskTitle,  DependencyType dependencyType,  int lagDays,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskDependencyDto() when $default != null:
return $default(_that.id,_that.predecessorTaskId,_that.predecessorTaskTitle,_that.successorTaskId,_that.successorTaskTitle,_that.dependencyType,_that.lagDays,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskDependencyDto implements TaskDependencyDto {
  const _TaskDependencyDto({required this.id, required this.predecessorTaskId, required this.predecessorTaskTitle, required this.successorTaskId, required this.successorTaskTitle, required this.dependencyType, this.lagDays = 0, required this.creationTime});
  factory _TaskDependencyDto.fromJson(Map<String, dynamic> json) => _$TaskDependencyDtoFromJson(json);

@override final  String id;
@override final  String predecessorTaskId;
@override final  String predecessorTaskTitle;
@override final  String successorTaskId;
@override final  String successorTaskTitle;
@override final  DependencyType dependencyType;
@override@JsonKey() final  int lagDays;
@override final  DateTime creationTime;

/// Create a copy of TaskDependencyDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskDependencyDtoCopyWith<_TaskDependencyDto> get copyWith => __$TaskDependencyDtoCopyWithImpl<_TaskDependencyDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskDependencyDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskDependencyDto&&(identical(other.id, id) || other.id == id)&&(identical(other.predecessorTaskId, predecessorTaskId) || other.predecessorTaskId == predecessorTaskId)&&(identical(other.predecessorTaskTitle, predecessorTaskTitle) || other.predecessorTaskTitle == predecessorTaskTitle)&&(identical(other.successorTaskId, successorTaskId) || other.successorTaskId == successorTaskId)&&(identical(other.successorTaskTitle, successorTaskTitle) || other.successorTaskTitle == successorTaskTitle)&&(identical(other.dependencyType, dependencyType) || other.dependencyType == dependencyType)&&(identical(other.lagDays, lagDays) || other.lagDays == lagDays)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,predecessorTaskId,predecessorTaskTitle,successorTaskId,successorTaskTitle,dependencyType,lagDays,creationTime);

@override
String toString() {
  return 'TaskDependencyDto(id: $id, predecessorTaskId: $predecessorTaskId, predecessorTaskTitle: $predecessorTaskTitle, successorTaskId: $successorTaskId, successorTaskTitle: $successorTaskTitle, dependencyType: $dependencyType, lagDays: $lagDays, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskDependencyDtoCopyWith<$Res> implements $TaskDependencyDtoCopyWith<$Res> {
  factory _$TaskDependencyDtoCopyWith(_TaskDependencyDto value, $Res Function(_TaskDependencyDto) _then) = __$TaskDependencyDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String predecessorTaskId, String predecessorTaskTitle, String successorTaskId, String successorTaskTitle, DependencyType dependencyType, int lagDays, DateTime creationTime
});




}
/// @nodoc
class __$TaskDependencyDtoCopyWithImpl<$Res>
    implements _$TaskDependencyDtoCopyWith<$Res> {
  __$TaskDependencyDtoCopyWithImpl(this._self, this._then);

  final _TaskDependencyDto _self;
  final $Res Function(_TaskDependencyDto) _then;

/// Create a copy of TaskDependencyDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? predecessorTaskId = null,Object? predecessorTaskTitle = null,Object? successorTaskId = null,Object? successorTaskTitle = null,Object? dependencyType = null,Object? lagDays = null,Object? creationTime = null,}) {
  return _then(_TaskDependencyDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,predecessorTaskId: null == predecessorTaskId ? _self.predecessorTaskId : predecessorTaskId // ignore: cast_nullable_to_non_nullable
as String,predecessorTaskTitle: null == predecessorTaskTitle ? _self.predecessorTaskTitle : predecessorTaskTitle // ignore: cast_nullable_to_non_nullable
as String,successorTaskId: null == successorTaskId ? _self.successorTaskId : successorTaskId // ignore: cast_nullable_to_non_nullable
as String,successorTaskTitle: null == successorTaskTitle ? _self.successorTaskTitle : successorTaskTitle // ignore: cast_nullable_to_non_nullable
as String,dependencyType: null == dependencyType ? _self.dependencyType : dependencyType // ignore: cast_nullable_to_non_nullable
as DependencyType,lagDays: null == lagDays ? _self.lagDays : lagDays // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateDependencyDto {

 String get predecessorTaskId; String get successorTaskId; DependencyType get dependencyType; int get lagDays;
/// Create a copy of CreateDependencyDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateDependencyDtoCopyWith<CreateDependencyDto> get copyWith => _$CreateDependencyDtoCopyWithImpl<CreateDependencyDto>(this as CreateDependencyDto, _$identity);

  /// Serializes this CreateDependencyDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateDependencyDto&&(identical(other.predecessorTaskId, predecessorTaskId) || other.predecessorTaskId == predecessorTaskId)&&(identical(other.successorTaskId, successorTaskId) || other.successorTaskId == successorTaskId)&&(identical(other.dependencyType, dependencyType) || other.dependencyType == dependencyType)&&(identical(other.lagDays, lagDays) || other.lagDays == lagDays));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,predecessorTaskId,successorTaskId,dependencyType,lagDays);

@override
String toString() {
  return 'CreateDependencyDto(predecessorTaskId: $predecessorTaskId, successorTaskId: $successorTaskId, dependencyType: $dependencyType, lagDays: $lagDays)';
}


}

/// @nodoc
abstract mixin class $CreateDependencyDtoCopyWith<$Res>  {
  factory $CreateDependencyDtoCopyWith(CreateDependencyDto value, $Res Function(CreateDependencyDto) _then) = _$CreateDependencyDtoCopyWithImpl;
@useResult
$Res call({
 String predecessorTaskId, String successorTaskId, DependencyType dependencyType, int lagDays
});




}
/// @nodoc
class _$CreateDependencyDtoCopyWithImpl<$Res>
    implements $CreateDependencyDtoCopyWith<$Res> {
  _$CreateDependencyDtoCopyWithImpl(this._self, this._then);

  final CreateDependencyDto _self;
  final $Res Function(CreateDependencyDto) _then;

/// Create a copy of CreateDependencyDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? predecessorTaskId = null,Object? successorTaskId = null,Object? dependencyType = null,Object? lagDays = null,}) {
  return _then(_self.copyWith(
predecessorTaskId: null == predecessorTaskId ? _self.predecessorTaskId : predecessorTaskId // ignore: cast_nullable_to_non_nullable
as String,successorTaskId: null == successorTaskId ? _self.successorTaskId : successorTaskId // ignore: cast_nullable_to_non_nullable
as String,dependencyType: null == dependencyType ? _self.dependencyType : dependencyType // ignore: cast_nullable_to_non_nullable
as DependencyType,lagDays: null == lagDays ? _self.lagDays : lagDays // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateDependencyDto].
extension CreateDependencyDtoPatterns on CreateDependencyDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateDependencyDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateDependencyDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateDependencyDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateDependencyDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateDependencyDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateDependencyDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String predecessorTaskId,  String successorTaskId,  DependencyType dependencyType,  int lagDays)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateDependencyDto() when $default != null:
return $default(_that.predecessorTaskId,_that.successorTaskId,_that.dependencyType,_that.lagDays);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String predecessorTaskId,  String successorTaskId,  DependencyType dependencyType,  int lagDays)  $default,) {final _that = this;
switch (_that) {
case _CreateDependencyDto():
return $default(_that.predecessorTaskId,_that.successorTaskId,_that.dependencyType,_that.lagDays);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String predecessorTaskId,  String successorTaskId,  DependencyType dependencyType,  int lagDays)?  $default,) {final _that = this;
switch (_that) {
case _CreateDependencyDto() when $default != null:
return $default(_that.predecessorTaskId,_that.successorTaskId,_that.dependencyType,_that.lagDays);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateDependencyDto implements CreateDependencyDto {
  const _CreateDependencyDto({required this.predecessorTaskId, required this.successorTaskId, required this.dependencyType, this.lagDays = 0});
  factory _CreateDependencyDto.fromJson(Map<String, dynamic> json) => _$CreateDependencyDtoFromJson(json);

@override final  String predecessorTaskId;
@override final  String successorTaskId;
@override final  DependencyType dependencyType;
@override@JsonKey() final  int lagDays;

/// Create a copy of CreateDependencyDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateDependencyDtoCopyWith<_CreateDependencyDto> get copyWith => __$CreateDependencyDtoCopyWithImpl<_CreateDependencyDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateDependencyDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateDependencyDto&&(identical(other.predecessorTaskId, predecessorTaskId) || other.predecessorTaskId == predecessorTaskId)&&(identical(other.successorTaskId, successorTaskId) || other.successorTaskId == successorTaskId)&&(identical(other.dependencyType, dependencyType) || other.dependencyType == dependencyType)&&(identical(other.lagDays, lagDays) || other.lagDays == lagDays));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,predecessorTaskId,successorTaskId,dependencyType,lagDays);

@override
String toString() {
  return 'CreateDependencyDto(predecessorTaskId: $predecessorTaskId, successorTaskId: $successorTaskId, dependencyType: $dependencyType, lagDays: $lagDays)';
}


}

/// @nodoc
abstract mixin class _$CreateDependencyDtoCopyWith<$Res> implements $CreateDependencyDtoCopyWith<$Res> {
  factory _$CreateDependencyDtoCopyWith(_CreateDependencyDto value, $Res Function(_CreateDependencyDto) _then) = __$CreateDependencyDtoCopyWithImpl;
@override @useResult
$Res call({
 String predecessorTaskId, String successorTaskId, DependencyType dependencyType, int lagDays
});




}
/// @nodoc
class __$CreateDependencyDtoCopyWithImpl<$Res>
    implements _$CreateDependencyDtoCopyWith<$Res> {
  __$CreateDependencyDtoCopyWithImpl(this._self, this._then);

  final _CreateDependencyDto _self;
  final $Res Function(_CreateDependencyDto) _then;

/// Create a copy of CreateDependencyDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? predecessorTaskId = null,Object? successorTaskId = null,Object? dependencyType = null,Object? lagDays = null,}) {
  return _then(_CreateDependencyDto(
predecessorTaskId: null == predecessorTaskId ? _self.predecessorTaskId : predecessorTaskId // ignore: cast_nullable_to_non_nullable
as String,successorTaskId: null == successorTaskId ? _self.successorTaskId : successorTaskId // ignore: cast_nullable_to_non_nullable
as String,dependencyType: null == dependencyType ? _self.dependencyType : dependencyType // ignore: cast_nullable_to_non_nullable
as DependencyType,lagDays: null == lagDays ? _self.lagDays : lagDays // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}

// dart format on
