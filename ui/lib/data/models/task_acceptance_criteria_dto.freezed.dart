// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_acceptance_criteria_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskAcceptanceCriteriaDto {

 String get id; String get taskId; String get criterion; bool get isCompleted; int get order; DateTime get creationTime; DateTime? get completionTime;
/// Create a copy of TaskAcceptanceCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskAcceptanceCriteriaDtoCopyWith<TaskAcceptanceCriteriaDto> get copyWith => _$TaskAcceptanceCriteriaDtoCopyWithImpl<TaskAcceptanceCriteriaDto>(this as TaskAcceptanceCriteriaDto, _$identity);

  /// Serializes this TaskAcceptanceCriteriaDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskAcceptanceCriteriaDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,criterion,isCompleted,order,creationTime,completionTime);

@override
String toString() {
  return 'TaskAcceptanceCriteriaDto(id: $id, taskId: $taskId, criterion: $criterion, isCompleted: $isCompleted, order: $order, creationTime: $creationTime, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class $TaskAcceptanceCriteriaDtoCopyWith<$Res>  {
  factory $TaskAcceptanceCriteriaDtoCopyWith(TaskAcceptanceCriteriaDto value, $Res Function(TaskAcceptanceCriteriaDto) _then) = _$TaskAcceptanceCriteriaDtoCopyWithImpl;
@useResult
$Res call({
 String id, String taskId, String criterion, bool isCompleted, int order, DateTime creationTime, DateTime? completionTime
});




}
/// @nodoc
class _$TaskAcceptanceCriteriaDtoCopyWithImpl<$Res>
    implements $TaskAcceptanceCriteriaDtoCopyWith<$Res> {
  _$TaskAcceptanceCriteriaDtoCopyWithImpl(this._self, this._then);

  final TaskAcceptanceCriteriaDto _self;
  final $Res Function(TaskAcceptanceCriteriaDto) _then;

/// Create a copy of TaskAcceptanceCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? taskId = null,Object? criterion = null,Object? isCompleted = null,Object? order = null,Object? creationTime = null,Object? completionTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskAcceptanceCriteriaDto].
extension TaskAcceptanceCriteriaDtoPatterns on TaskAcceptanceCriteriaDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskAcceptanceCriteriaDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskAcceptanceCriteriaDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskAcceptanceCriteriaDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String taskId,  String criterion,  bool isCompleted,  int order,  DateTime creationTime,  DateTime? completionTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto() when $default != null:
return $default(_that.id,_that.taskId,_that.criterion,_that.isCompleted,_that.order,_that.creationTime,_that.completionTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String taskId,  String criterion,  bool isCompleted,  int order,  DateTime creationTime,  DateTime? completionTime)  $default,) {final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto():
return $default(_that.id,_that.taskId,_that.criterion,_that.isCompleted,_that.order,_that.creationTime,_that.completionTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String taskId,  String criterion,  bool isCompleted,  int order,  DateTime creationTime,  DateTime? completionTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskAcceptanceCriteriaDto() when $default != null:
return $default(_that.id,_that.taskId,_that.criterion,_that.isCompleted,_that.order,_that.creationTime,_that.completionTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskAcceptanceCriteriaDto implements TaskAcceptanceCriteriaDto {
  const _TaskAcceptanceCriteriaDto({required this.id, required this.taskId, required this.criterion, this.isCompleted = false, required this.order, required this.creationTime, this.completionTime});
  factory _TaskAcceptanceCriteriaDto.fromJson(Map<String, dynamic> json) => _$TaskAcceptanceCriteriaDtoFromJson(json);

@override final  String id;
@override final  String taskId;
@override final  String criterion;
@override@JsonKey() final  bool isCompleted;
@override final  int order;
@override final  DateTime creationTime;
@override final  DateTime? completionTime;

/// Create a copy of TaskAcceptanceCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskAcceptanceCriteriaDtoCopyWith<_TaskAcceptanceCriteriaDto> get copyWith => __$TaskAcceptanceCriteriaDtoCopyWithImpl<_TaskAcceptanceCriteriaDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskAcceptanceCriteriaDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskAcceptanceCriteriaDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,criterion,isCompleted,order,creationTime,completionTime);

@override
String toString() {
  return 'TaskAcceptanceCriteriaDto(id: $id, taskId: $taskId, criterion: $criterion, isCompleted: $isCompleted, order: $order, creationTime: $creationTime, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class _$TaskAcceptanceCriteriaDtoCopyWith<$Res> implements $TaskAcceptanceCriteriaDtoCopyWith<$Res> {
  factory _$TaskAcceptanceCriteriaDtoCopyWith(_TaskAcceptanceCriteriaDto value, $Res Function(_TaskAcceptanceCriteriaDto) _then) = __$TaskAcceptanceCriteriaDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String taskId, String criterion, bool isCompleted, int order, DateTime creationTime, DateTime? completionTime
});




}
/// @nodoc
class __$TaskAcceptanceCriteriaDtoCopyWithImpl<$Res>
    implements _$TaskAcceptanceCriteriaDtoCopyWith<$Res> {
  __$TaskAcceptanceCriteriaDtoCopyWithImpl(this._self, this._then);

  final _TaskAcceptanceCriteriaDto _self;
  final $Res Function(_TaskAcceptanceCriteriaDto) _then;

/// Create a copy of TaskAcceptanceCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? taskId = null,Object? criterion = null,Object? isCompleted = null,Object? order = null,Object? creationTime = null,Object? completionTime = freezed,}) {
  return _then(_TaskAcceptanceCriteriaDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateCriteriaDto {

 String get taskId; String get criterion; bool get isCompleted; int get order;
/// Create a copy of CreateUpdateCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateCriteriaDtoCopyWith<CreateUpdateCriteriaDto> get copyWith => _$CreateUpdateCriteriaDtoCopyWithImpl<CreateUpdateCriteriaDto>(this as CreateUpdateCriteriaDto, _$identity);

  /// Serializes this CreateUpdateCriteriaDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateCriteriaDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,criterion,isCompleted,order);

@override
String toString() {
  return 'CreateUpdateCriteriaDto(taskId: $taskId, criterion: $criterion, isCompleted: $isCompleted, order: $order)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateCriteriaDtoCopyWith<$Res>  {
  factory $CreateUpdateCriteriaDtoCopyWith(CreateUpdateCriteriaDto value, $Res Function(CreateUpdateCriteriaDto) _then) = _$CreateUpdateCriteriaDtoCopyWithImpl;
@useResult
$Res call({
 String taskId, String criterion, bool isCompleted, int order
});




}
/// @nodoc
class _$CreateUpdateCriteriaDtoCopyWithImpl<$Res>
    implements $CreateUpdateCriteriaDtoCopyWith<$Res> {
  _$CreateUpdateCriteriaDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateCriteriaDto _self;
  final $Res Function(CreateUpdateCriteriaDto) _then;

/// Create a copy of CreateUpdateCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? taskId = null,Object? criterion = null,Object? isCompleted = null,Object? order = null,}) {
  return _then(_self.copyWith(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateCriteriaDto].
extension CreateUpdateCriteriaDtoPatterns on CreateUpdateCriteriaDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateCriteriaDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateCriteriaDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateCriteriaDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String taskId,  String criterion,  bool isCompleted,  int order)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto() when $default != null:
return $default(_that.taskId,_that.criterion,_that.isCompleted,_that.order);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String taskId,  String criterion,  bool isCompleted,  int order)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto():
return $default(_that.taskId,_that.criterion,_that.isCompleted,_that.order);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String taskId,  String criterion,  bool isCompleted,  int order)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateCriteriaDto() when $default != null:
return $default(_that.taskId,_that.criterion,_that.isCompleted,_that.order);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateCriteriaDto implements CreateUpdateCriteriaDto {
  const _CreateUpdateCriteriaDto({required this.taskId, required this.criterion, this.isCompleted = false, required this.order});
  factory _CreateUpdateCriteriaDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateCriteriaDtoFromJson(json);

@override final  String taskId;
@override final  String criterion;
@override@JsonKey() final  bool isCompleted;
@override final  int order;

/// Create a copy of CreateUpdateCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateCriteriaDtoCopyWith<_CreateUpdateCriteriaDto> get copyWith => __$CreateUpdateCriteriaDtoCopyWithImpl<_CreateUpdateCriteriaDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateCriteriaDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateCriteriaDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,criterion,isCompleted,order);

@override
String toString() {
  return 'CreateUpdateCriteriaDto(taskId: $taskId, criterion: $criterion, isCompleted: $isCompleted, order: $order)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateCriteriaDtoCopyWith<$Res> implements $CreateUpdateCriteriaDtoCopyWith<$Res> {
  factory _$CreateUpdateCriteriaDtoCopyWith(_CreateUpdateCriteriaDto value, $Res Function(_CreateUpdateCriteriaDto) _then) = __$CreateUpdateCriteriaDtoCopyWithImpl;
@override @useResult
$Res call({
 String taskId, String criterion, bool isCompleted, int order
});




}
/// @nodoc
class __$CreateUpdateCriteriaDtoCopyWithImpl<$Res>
    implements _$CreateUpdateCriteriaDtoCopyWith<$Res> {
  __$CreateUpdateCriteriaDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateCriteriaDto _self;
  final $Res Function(_CreateUpdateCriteriaDto) _then;

/// Create a copy of CreateUpdateCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? taskId = null,Object? criterion = null,Object? isCompleted = null,Object? order = null,}) {
  return _then(_CreateUpdateCriteriaDto(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}

// dart format on
