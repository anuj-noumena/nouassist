// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_action_step_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskActionStepDto {

 String get id; String get taskId; String get description; bool get isCompleted; String? get assigneeId; String? get assigneeName; DateTime? get dueDate; int get order; DateTime get creationTime; DateTime? get completionTime;
/// Create a copy of TaskActionStepDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskActionStepDtoCopyWith<TaskActionStepDto> get copyWith => _$TaskActionStepDtoCopyWithImpl<TaskActionStepDto>(this as TaskActionStepDto, _$identity);

  /// Serializes this TaskActionStepDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskActionStepDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.assigneeName, assigneeName) || other.assigneeName == assigneeName)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.order, order) || other.order == order)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,description,isCompleted,assigneeId,assigneeName,dueDate,order,creationTime,completionTime);

@override
String toString() {
  return 'TaskActionStepDto(id: $id, taskId: $taskId, description: $description, isCompleted: $isCompleted, assigneeId: $assigneeId, assigneeName: $assigneeName, dueDate: $dueDate, order: $order, creationTime: $creationTime, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class $TaskActionStepDtoCopyWith<$Res>  {
  factory $TaskActionStepDtoCopyWith(TaskActionStepDto value, $Res Function(TaskActionStepDto) _then) = _$TaskActionStepDtoCopyWithImpl;
@useResult
$Res call({
 String id, String taskId, String description, bool isCompleted, String? assigneeId, String? assigneeName, DateTime? dueDate, int order, DateTime creationTime, DateTime? completionTime
});




}
/// @nodoc
class _$TaskActionStepDtoCopyWithImpl<$Res>
    implements $TaskActionStepDtoCopyWith<$Res> {
  _$TaskActionStepDtoCopyWithImpl(this._self, this._then);

  final TaskActionStepDto _self;
  final $Res Function(TaskActionStepDto) _then;

/// Create a copy of TaskActionStepDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? taskId = null,Object? description = null,Object? isCompleted = null,Object? assigneeId = freezed,Object? assigneeName = freezed,Object? dueDate = freezed,Object? order = null,Object? creationTime = null,Object? completionTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,assigneeName: freezed == assigneeName ? _self.assigneeName : assigneeName // ignore: cast_nullable_to_non_nullable
as String?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskActionStepDto].
extension TaskActionStepDtoPatterns on TaskActionStepDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskActionStepDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskActionStepDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskActionStepDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskActionStepDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskActionStepDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskActionStepDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String taskId,  String description,  bool isCompleted,  String? assigneeId,  String? assigneeName,  DateTime? dueDate,  int order,  DateTime creationTime,  DateTime? completionTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskActionStepDto() when $default != null:
return $default(_that.id,_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.assigneeName,_that.dueDate,_that.order,_that.creationTime,_that.completionTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String taskId,  String description,  bool isCompleted,  String? assigneeId,  String? assigneeName,  DateTime? dueDate,  int order,  DateTime creationTime,  DateTime? completionTime)  $default,) {final _that = this;
switch (_that) {
case _TaskActionStepDto():
return $default(_that.id,_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.assigneeName,_that.dueDate,_that.order,_that.creationTime,_that.completionTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String taskId,  String description,  bool isCompleted,  String? assigneeId,  String? assigneeName,  DateTime? dueDate,  int order,  DateTime creationTime,  DateTime? completionTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskActionStepDto() when $default != null:
return $default(_that.id,_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.assigneeName,_that.dueDate,_that.order,_that.creationTime,_that.completionTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskActionStepDto implements TaskActionStepDto {
  const _TaskActionStepDto({required this.id, required this.taskId, required this.description, this.isCompleted = false, this.assigneeId, this.assigneeName, this.dueDate, required this.order, required this.creationTime, this.completionTime});
  factory _TaskActionStepDto.fromJson(Map<String, dynamic> json) => _$TaskActionStepDtoFromJson(json);

@override final  String id;
@override final  String taskId;
@override final  String description;
@override@JsonKey() final  bool isCompleted;
@override final  String? assigneeId;
@override final  String? assigneeName;
@override final  DateTime? dueDate;
@override final  int order;
@override final  DateTime creationTime;
@override final  DateTime? completionTime;

/// Create a copy of TaskActionStepDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskActionStepDtoCopyWith<_TaskActionStepDto> get copyWith => __$TaskActionStepDtoCopyWithImpl<_TaskActionStepDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskActionStepDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskActionStepDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.assigneeName, assigneeName) || other.assigneeName == assigneeName)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.order, order) || other.order == order)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,description,isCompleted,assigneeId,assigneeName,dueDate,order,creationTime,completionTime);

@override
String toString() {
  return 'TaskActionStepDto(id: $id, taskId: $taskId, description: $description, isCompleted: $isCompleted, assigneeId: $assigneeId, assigneeName: $assigneeName, dueDate: $dueDate, order: $order, creationTime: $creationTime, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class _$TaskActionStepDtoCopyWith<$Res> implements $TaskActionStepDtoCopyWith<$Res> {
  factory _$TaskActionStepDtoCopyWith(_TaskActionStepDto value, $Res Function(_TaskActionStepDto) _then) = __$TaskActionStepDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String taskId, String description, bool isCompleted, String? assigneeId, String? assigneeName, DateTime? dueDate, int order, DateTime creationTime, DateTime? completionTime
});




}
/// @nodoc
class __$TaskActionStepDtoCopyWithImpl<$Res>
    implements _$TaskActionStepDtoCopyWith<$Res> {
  __$TaskActionStepDtoCopyWithImpl(this._self, this._then);

  final _TaskActionStepDto _self;
  final $Res Function(_TaskActionStepDto) _then;

/// Create a copy of TaskActionStepDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? taskId = null,Object? description = null,Object? isCompleted = null,Object? assigneeId = freezed,Object? assigneeName = freezed,Object? dueDate = freezed,Object? order = null,Object? creationTime = null,Object? completionTime = freezed,}) {
  return _then(_TaskActionStepDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,assigneeName: freezed == assigneeName ? _self.assigneeName : assigneeName // ignore: cast_nullable_to_non_nullable
as String?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateActionStepDto {

 String get taskId; String get description; bool get isCompleted; String? get assigneeId; DateTime? get dueDate; int get order;
/// Create a copy of CreateUpdateActionStepDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateActionStepDtoCopyWith<CreateUpdateActionStepDto> get copyWith => _$CreateUpdateActionStepDtoCopyWithImpl<CreateUpdateActionStepDto>(this as CreateUpdateActionStepDto, _$identity);

  /// Serializes this CreateUpdateActionStepDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateActionStepDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,description,isCompleted,assigneeId,dueDate,order);

@override
String toString() {
  return 'CreateUpdateActionStepDto(taskId: $taskId, description: $description, isCompleted: $isCompleted, assigneeId: $assigneeId, dueDate: $dueDate, order: $order)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateActionStepDtoCopyWith<$Res>  {
  factory $CreateUpdateActionStepDtoCopyWith(CreateUpdateActionStepDto value, $Res Function(CreateUpdateActionStepDto) _then) = _$CreateUpdateActionStepDtoCopyWithImpl;
@useResult
$Res call({
 String taskId, String description, bool isCompleted, String? assigneeId, DateTime? dueDate, int order
});




}
/// @nodoc
class _$CreateUpdateActionStepDtoCopyWithImpl<$Res>
    implements $CreateUpdateActionStepDtoCopyWith<$Res> {
  _$CreateUpdateActionStepDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateActionStepDto _self;
  final $Res Function(CreateUpdateActionStepDto) _then;

/// Create a copy of CreateUpdateActionStepDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? taskId = null,Object? description = null,Object? isCompleted = null,Object? assigneeId = freezed,Object? dueDate = freezed,Object? order = null,}) {
  return _then(_self.copyWith(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateActionStepDto].
extension CreateUpdateActionStepDtoPatterns on CreateUpdateActionStepDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateActionStepDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateActionStepDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateActionStepDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String taskId,  String description,  bool isCompleted,  String? assigneeId,  DateTime? dueDate,  int order)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto() when $default != null:
return $default(_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.dueDate,_that.order);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String taskId,  String description,  bool isCompleted,  String? assigneeId,  DateTime? dueDate,  int order)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto():
return $default(_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.dueDate,_that.order);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String taskId,  String description,  bool isCompleted,  String? assigneeId,  DateTime? dueDate,  int order)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateActionStepDto() when $default != null:
return $default(_that.taskId,_that.description,_that.isCompleted,_that.assigneeId,_that.dueDate,_that.order);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateActionStepDto implements CreateUpdateActionStepDto {
  const _CreateUpdateActionStepDto({required this.taskId, required this.description, this.isCompleted = false, this.assigneeId, this.dueDate, required this.order});
  factory _CreateUpdateActionStepDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateActionStepDtoFromJson(json);

@override final  String taskId;
@override final  String description;
@override@JsonKey() final  bool isCompleted;
@override final  String? assigneeId;
@override final  DateTime? dueDate;
@override final  int order;

/// Create a copy of CreateUpdateActionStepDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateActionStepDtoCopyWith<_CreateUpdateActionStepDto> get copyWith => __$CreateUpdateActionStepDtoCopyWithImpl<_CreateUpdateActionStepDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateActionStepDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateActionStepDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,description,isCompleted,assigneeId,dueDate,order);

@override
String toString() {
  return 'CreateUpdateActionStepDto(taskId: $taskId, description: $description, isCompleted: $isCompleted, assigneeId: $assigneeId, dueDate: $dueDate, order: $order)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateActionStepDtoCopyWith<$Res> implements $CreateUpdateActionStepDtoCopyWith<$Res> {
  factory _$CreateUpdateActionStepDtoCopyWith(_CreateUpdateActionStepDto value, $Res Function(_CreateUpdateActionStepDto) _then) = __$CreateUpdateActionStepDtoCopyWithImpl;
@override @useResult
$Res call({
 String taskId, String description, bool isCompleted, String? assigneeId, DateTime? dueDate, int order
});




}
/// @nodoc
class __$CreateUpdateActionStepDtoCopyWithImpl<$Res>
    implements _$CreateUpdateActionStepDtoCopyWith<$Res> {
  __$CreateUpdateActionStepDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateActionStepDto _self;
  final $Res Function(_CreateUpdateActionStepDto) _then;

/// Create a copy of CreateUpdateActionStepDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? taskId = null,Object? description = null,Object? isCompleted = null,Object? assigneeId = freezed,Object? dueDate = freezed,Object? order = null,}) {
  return _then(_CreateUpdateActionStepDto(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}

// dart format on
