// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_workflow_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskWorkflowStageDto {

 String get id; String get name; int get order; String get color; StatusCategory get statusCategory; List<String> get allowedTransitions; bool get requiresComment;
/// Create a copy of TaskWorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskWorkflowStageDtoCopyWith<TaskWorkflowStageDto> get copyWith => _$TaskWorkflowStageDtoCopyWithImpl<TaskWorkflowStageDto>(this as TaskWorkflowStageDto, _$identity);

  /// Serializes this TaskWorkflowStageDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskWorkflowStageDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.order, order) || other.order == order)&&(identical(other.color, color) || other.color == color)&&(identical(other.statusCategory, statusCategory) || other.statusCategory == statusCategory)&&const DeepCollectionEquality().equals(other.allowedTransitions, allowedTransitions)&&(identical(other.requiresComment, requiresComment) || other.requiresComment == requiresComment));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,order,color,statusCategory,const DeepCollectionEquality().hash(allowedTransitions),requiresComment);

@override
String toString() {
  return 'TaskWorkflowStageDto(id: $id, name: $name, order: $order, color: $color, statusCategory: $statusCategory, allowedTransitions: $allowedTransitions, requiresComment: $requiresComment)';
}


}

/// @nodoc
abstract mixin class $TaskWorkflowStageDtoCopyWith<$Res>  {
  factory $TaskWorkflowStageDtoCopyWith(TaskWorkflowStageDto value, $Res Function(TaskWorkflowStageDto) _then) = _$TaskWorkflowStageDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, int order, String color, StatusCategory statusCategory, List<String> allowedTransitions, bool requiresComment
});




}
/// @nodoc
class _$TaskWorkflowStageDtoCopyWithImpl<$Res>
    implements $TaskWorkflowStageDtoCopyWith<$Res> {
  _$TaskWorkflowStageDtoCopyWithImpl(this._self, this._then);

  final TaskWorkflowStageDto _self;
  final $Res Function(TaskWorkflowStageDto) _then;

/// Create a copy of TaskWorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? order = null,Object? color = null,Object? statusCategory = null,Object? allowedTransitions = null,Object? requiresComment = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,statusCategory: null == statusCategory ? _self.statusCategory : statusCategory // ignore: cast_nullable_to_non_nullable
as StatusCategory,allowedTransitions: null == allowedTransitions ? _self.allowedTransitions : allowedTransitions // ignore: cast_nullable_to_non_nullable
as List<String>,requiresComment: null == requiresComment ? _self.requiresComment : requiresComment // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskWorkflowStageDto].
extension TaskWorkflowStageDtoPatterns on TaskWorkflowStageDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskWorkflowStageDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskWorkflowStageDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskWorkflowStageDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskWorkflowStageDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskWorkflowStageDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskWorkflowStageDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  int order,  String color,  StatusCategory statusCategory,  List<String> allowedTransitions,  bool requiresComment)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskWorkflowStageDto() when $default != null:
return $default(_that.id,_that.name,_that.order,_that.color,_that.statusCategory,_that.allowedTransitions,_that.requiresComment);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  int order,  String color,  StatusCategory statusCategory,  List<String> allowedTransitions,  bool requiresComment)  $default,) {final _that = this;
switch (_that) {
case _TaskWorkflowStageDto():
return $default(_that.id,_that.name,_that.order,_that.color,_that.statusCategory,_that.allowedTransitions,_that.requiresComment);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  int order,  String color,  StatusCategory statusCategory,  List<String> allowedTransitions,  bool requiresComment)?  $default,) {final _that = this;
switch (_that) {
case _TaskWorkflowStageDto() when $default != null:
return $default(_that.id,_that.name,_that.order,_that.color,_that.statusCategory,_that.allowedTransitions,_that.requiresComment);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskWorkflowStageDto implements TaskWorkflowStageDto {
  const _TaskWorkflowStageDto({required this.id, required this.name, required this.order, required this.color, required this.statusCategory, final  List<String> allowedTransitions = const [], this.requiresComment = false}): _allowedTransitions = allowedTransitions;
  factory _TaskWorkflowStageDto.fromJson(Map<String, dynamic> json) => _$TaskWorkflowStageDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  int order;
@override final  String color;
@override final  StatusCategory statusCategory;
 final  List<String> _allowedTransitions;
@override@JsonKey() List<String> get allowedTransitions {
  if (_allowedTransitions is EqualUnmodifiableListView) return _allowedTransitions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_allowedTransitions);
}

@override@JsonKey() final  bool requiresComment;

/// Create a copy of TaskWorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskWorkflowStageDtoCopyWith<_TaskWorkflowStageDto> get copyWith => __$TaskWorkflowStageDtoCopyWithImpl<_TaskWorkflowStageDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskWorkflowStageDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskWorkflowStageDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.order, order) || other.order == order)&&(identical(other.color, color) || other.color == color)&&(identical(other.statusCategory, statusCategory) || other.statusCategory == statusCategory)&&const DeepCollectionEquality().equals(other._allowedTransitions, _allowedTransitions)&&(identical(other.requiresComment, requiresComment) || other.requiresComment == requiresComment));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,order,color,statusCategory,const DeepCollectionEquality().hash(_allowedTransitions),requiresComment);

@override
String toString() {
  return 'TaskWorkflowStageDto(id: $id, name: $name, order: $order, color: $color, statusCategory: $statusCategory, allowedTransitions: $allowedTransitions, requiresComment: $requiresComment)';
}


}

/// @nodoc
abstract mixin class _$TaskWorkflowStageDtoCopyWith<$Res> implements $TaskWorkflowStageDtoCopyWith<$Res> {
  factory _$TaskWorkflowStageDtoCopyWith(_TaskWorkflowStageDto value, $Res Function(_TaskWorkflowStageDto) _then) = __$TaskWorkflowStageDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, int order, String color, StatusCategory statusCategory, List<String> allowedTransitions, bool requiresComment
});




}
/// @nodoc
class __$TaskWorkflowStageDtoCopyWithImpl<$Res>
    implements _$TaskWorkflowStageDtoCopyWith<$Res> {
  __$TaskWorkflowStageDtoCopyWithImpl(this._self, this._then);

  final _TaskWorkflowStageDto _self;
  final $Res Function(_TaskWorkflowStageDto) _then;

/// Create a copy of TaskWorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? order = null,Object? color = null,Object? statusCategory = null,Object? allowedTransitions = null,Object? requiresComment = null,}) {
  return _then(_TaskWorkflowStageDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,statusCategory: null == statusCategory ? _self.statusCategory : statusCategory // ignore: cast_nullable_to_non_nullable
as StatusCategory,allowedTransitions: null == allowedTransitions ? _self._allowedTransitions : allowedTransitions // ignore: cast_nullable_to_non_nullable
as List<String>,requiresComment: null == requiresComment ? _self.requiresComment : requiresComment // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}


/// @nodoc
mixin _$TaskWorkflowDto {

 String get id; String get name; String? get description; List<TaskWorkflowStageDto> get stages; List<String> get taskTypeIds; bool get isDefault; DateTime get creationTime;
/// Create a copy of TaskWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskWorkflowDtoCopyWith<TaskWorkflowDto> get copyWith => _$TaskWorkflowDtoCopyWithImpl<TaskWorkflowDto>(this as TaskWorkflowDto, _$identity);

  /// Serializes this TaskWorkflowDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskWorkflowDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other.stages, stages)&&const DeepCollectionEquality().equals(other.taskTypeIds, taskTypeIds)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(stages),const DeepCollectionEquality().hash(taskTypeIds),isDefault,creationTime);

@override
String toString() {
  return 'TaskWorkflowDto(id: $id, name: $name, description: $description, stages: $stages, taskTypeIds: $taskTypeIds, isDefault: $isDefault, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TaskWorkflowDtoCopyWith<$Res>  {
  factory $TaskWorkflowDtoCopyWith(TaskWorkflowDto value, $Res Function(TaskWorkflowDto) _then) = _$TaskWorkflowDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String? description, List<TaskWorkflowStageDto> stages, List<String> taskTypeIds, bool isDefault, DateTime creationTime
});




}
/// @nodoc
class _$TaskWorkflowDtoCopyWithImpl<$Res>
    implements $TaskWorkflowDtoCopyWith<$Res> {
  _$TaskWorkflowDtoCopyWithImpl(this._self, this._then);

  final TaskWorkflowDto _self;
  final $Res Function(TaskWorkflowDto) _then;

/// Create a copy of TaskWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? stages = null,Object? taskTypeIds = null,Object? isDefault = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self.stages : stages // ignore: cast_nullable_to_non_nullable
as List<TaskWorkflowStageDto>,taskTypeIds: null == taskTypeIds ? _self.taskTypeIds : taskTypeIds // ignore: cast_nullable_to_non_nullable
as List<String>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskWorkflowDto].
extension TaskWorkflowDtoPatterns on TaskWorkflowDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskWorkflowDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskWorkflowDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskWorkflowDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskWorkflowDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<TaskWorkflowStageDto> stages,  List<String> taskTypeIds,  bool isDefault,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskWorkflowDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.stages,_that.taskTypeIds,_that.isDefault,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<TaskWorkflowStageDto> stages,  List<String> taskTypeIds,  bool isDefault,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskWorkflowDto():
return $default(_that.id,_that.name,_that.description,_that.stages,_that.taskTypeIds,_that.isDefault,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String? description,  List<TaskWorkflowStageDto> stages,  List<String> taskTypeIds,  bool isDefault,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskWorkflowDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.stages,_that.taskTypeIds,_that.isDefault,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskWorkflowDto implements TaskWorkflowDto {
  const _TaskWorkflowDto({required this.id, required this.name, this.description, required final  List<TaskWorkflowStageDto> stages, final  List<String> taskTypeIds = const [], this.isDefault = false, required this.creationTime}): _stages = stages,_taskTypeIds = taskTypeIds;
  factory _TaskWorkflowDto.fromJson(Map<String, dynamic> json) => _$TaskWorkflowDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String? description;
 final  List<TaskWorkflowStageDto> _stages;
@override List<TaskWorkflowStageDto> get stages {
  if (_stages is EqualUnmodifiableListView) return _stages;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_stages);
}

 final  List<String> _taskTypeIds;
@override@JsonKey() List<String> get taskTypeIds {
  if (_taskTypeIds is EqualUnmodifiableListView) return _taskTypeIds;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_taskTypeIds);
}

@override@JsonKey() final  bool isDefault;
@override final  DateTime creationTime;

/// Create a copy of TaskWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskWorkflowDtoCopyWith<_TaskWorkflowDto> get copyWith => __$TaskWorkflowDtoCopyWithImpl<_TaskWorkflowDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskWorkflowDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskWorkflowDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other._stages, _stages)&&const DeepCollectionEquality().equals(other._taskTypeIds, _taskTypeIds)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(_stages),const DeepCollectionEquality().hash(_taskTypeIds),isDefault,creationTime);

@override
String toString() {
  return 'TaskWorkflowDto(id: $id, name: $name, description: $description, stages: $stages, taskTypeIds: $taskTypeIds, isDefault: $isDefault, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskWorkflowDtoCopyWith<$Res> implements $TaskWorkflowDtoCopyWith<$Res> {
  factory _$TaskWorkflowDtoCopyWith(_TaskWorkflowDto value, $Res Function(_TaskWorkflowDto) _then) = __$TaskWorkflowDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String? description, List<TaskWorkflowStageDto> stages, List<String> taskTypeIds, bool isDefault, DateTime creationTime
});




}
/// @nodoc
class __$TaskWorkflowDtoCopyWithImpl<$Res>
    implements _$TaskWorkflowDtoCopyWith<$Res> {
  __$TaskWorkflowDtoCopyWithImpl(this._self, this._then);

  final _TaskWorkflowDto _self;
  final $Res Function(_TaskWorkflowDto) _then;

/// Create a copy of TaskWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? stages = null,Object? taskTypeIds = null,Object? isDefault = null,Object? creationTime = null,}) {
  return _then(_TaskWorkflowDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self._stages : stages // ignore: cast_nullable_to_non_nullable
as List<TaskWorkflowStageDto>,taskTypeIds: null == taskTypeIds ? _self._taskTypeIds : taskTypeIds // ignore: cast_nullable_to_non_nullable
as List<String>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}

// dart format on
