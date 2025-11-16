// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_type_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskTypeDto {

 String get id; String get name; String get icon; String get color; String? get description; String get defaultWorkflowId; bool get isActive; DateTime get creationTime;
/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskTypeDtoCopyWith<TaskTypeDto> get copyWith => _$TaskTypeDtoCopyWithImpl<TaskTypeDto>(this as TaskTypeDto, _$identity);

  /// Serializes this TaskTypeDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskTypeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.defaultWorkflowId, defaultWorkflowId) || other.defaultWorkflowId == defaultWorkflowId)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,icon,color,description,defaultWorkflowId,isActive,creationTime);

@override
String toString() {
  return 'TaskTypeDto(id: $id, name: $name, icon: $icon, color: $color, description: $description, defaultWorkflowId: $defaultWorkflowId, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TaskTypeDtoCopyWith<$Res>  {
  factory $TaskTypeDtoCopyWith(TaskTypeDto value, $Res Function(TaskTypeDto) _then) = _$TaskTypeDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String icon, String color, String? description, String defaultWorkflowId, bool isActive, DateTime creationTime
});




}
/// @nodoc
class _$TaskTypeDtoCopyWithImpl<$Res>
    implements $TaskTypeDtoCopyWith<$Res> {
  _$TaskTypeDtoCopyWithImpl(this._self, this._then);

  final TaskTypeDto _self;
  final $Res Function(TaskTypeDto) _then;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,Object? defaultWorkflowId = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,defaultWorkflowId: null == defaultWorkflowId ? _self.defaultWorkflowId : defaultWorkflowId // ignore: cast_nullable_to_non_nullable
as String,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskTypeDto].
extension TaskTypeDtoPatterns on TaskTypeDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskTypeDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskTypeDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskTypeDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskTypeDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskTypeDto():
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskTypeDto implements TaskTypeDto {
  const _TaskTypeDto({required this.id, required this.name, required this.icon, required this.color, this.description, required this.defaultWorkflowId, this.isActive = true, required this.creationTime});
  factory _TaskTypeDto.fromJson(Map<String, dynamic> json) => _$TaskTypeDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String icon;
@override final  String color;
@override final  String? description;
@override final  String defaultWorkflowId;
@override@JsonKey() final  bool isActive;
@override final  DateTime creationTime;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskTypeDtoCopyWith<_TaskTypeDto> get copyWith => __$TaskTypeDtoCopyWithImpl<_TaskTypeDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskTypeDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskTypeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.defaultWorkflowId, defaultWorkflowId) || other.defaultWorkflowId == defaultWorkflowId)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,icon,color,description,defaultWorkflowId,isActive,creationTime);

@override
String toString() {
  return 'TaskTypeDto(id: $id, name: $name, icon: $icon, color: $color, description: $description, defaultWorkflowId: $defaultWorkflowId, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskTypeDtoCopyWith<$Res> implements $TaskTypeDtoCopyWith<$Res> {
  factory _$TaskTypeDtoCopyWith(_TaskTypeDto value, $Res Function(_TaskTypeDto) _then) = __$TaskTypeDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String icon, String color, String? description, String defaultWorkflowId, bool isActive, DateTime creationTime
});




}
/// @nodoc
class __$TaskTypeDtoCopyWithImpl<$Res>
    implements _$TaskTypeDtoCopyWith<$Res> {
  __$TaskTypeDtoCopyWithImpl(this._self, this._then);

  final _TaskTypeDto _self;
  final $Res Function(_TaskTypeDto) _then;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,Object? defaultWorkflowId = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_TaskTypeDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,defaultWorkflowId: null == defaultWorkflowId ? _self.defaultWorkflowId : defaultWorkflowId // ignore: cast_nullable_to_non_nullable
as String,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateTaskTypeDto {

 String get name; String get icon; String get color; String? get description; String get defaultWorkflowId; bool get isActive;
/// Create a copy of CreateUpdateTaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateTaskTypeDtoCopyWith<CreateUpdateTaskTypeDto> get copyWith => _$CreateUpdateTaskTypeDtoCopyWithImpl<CreateUpdateTaskTypeDto>(this as CreateUpdateTaskTypeDto, _$identity);

  /// Serializes this CreateUpdateTaskTypeDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateTaskTypeDto&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.defaultWorkflowId, defaultWorkflowId) || other.defaultWorkflowId == defaultWorkflowId)&&(identical(other.isActive, isActive) || other.isActive == isActive));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,icon,color,description,defaultWorkflowId,isActive);

@override
String toString() {
  return 'CreateUpdateTaskTypeDto(name: $name, icon: $icon, color: $color, description: $description, defaultWorkflowId: $defaultWorkflowId, isActive: $isActive)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateTaskTypeDtoCopyWith<$Res>  {
  factory $CreateUpdateTaskTypeDtoCopyWith(CreateUpdateTaskTypeDto value, $Res Function(CreateUpdateTaskTypeDto) _then) = _$CreateUpdateTaskTypeDtoCopyWithImpl;
@useResult
$Res call({
 String name, String icon, String color, String? description, String defaultWorkflowId, bool isActive
});




}
/// @nodoc
class _$CreateUpdateTaskTypeDtoCopyWithImpl<$Res>
    implements $CreateUpdateTaskTypeDtoCopyWith<$Res> {
  _$CreateUpdateTaskTypeDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateTaskTypeDto _self;
  final $Res Function(CreateUpdateTaskTypeDto) _then;

/// Create a copy of CreateUpdateTaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,Object? defaultWorkflowId = null,Object? isActive = null,}) {
  return _then(_self.copyWith(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,defaultWorkflowId: null == defaultWorkflowId ? _self.defaultWorkflowId : defaultWorkflowId // ignore: cast_nullable_to_non_nullable
as String,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateTaskTypeDto].
extension CreateUpdateTaskTypeDtoPatterns on CreateUpdateTaskTypeDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateTaskTypeDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateTaskTypeDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateTaskTypeDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto() when $default != null:
return $default(_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto():
return $default(_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String icon,  String color,  String? description,  String defaultWorkflowId,  bool isActive)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTypeDto() when $default != null:
return $default(_that.name,_that.icon,_that.color,_that.description,_that.defaultWorkflowId,_that.isActive);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateTaskTypeDto implements CreateUpdateTaskTypeDto {
  const _CreateUpdateTaskTypeDto({required this.name, required this.icon, required this.color, this.description, required this.defaultWorkflowId, this.isActive = true});
  factory _CreateUpdateTaskTypeDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateTaskTypeDtoFromJson(json);

@override final  String name;
@override final  String icon;
@override final  String color;
@override final  String? description;
@override final  String defaultWorkflowId;
@override@JsonKey() final  bool isActive;

/// Create a copy of CreateUpdateTaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateTaskTypeDtoCopyWith<_CreateUpdateTaskTypeDto> get copyWith => __$CreateUpdateTaskTypeDtoCopyWithImpl<_CreateUpdateTaskTypeDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateTaskTypeDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateTaskTypeDto&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.defaultWorkflowId, defaultWorkflowId) || other.defaultWorkflowId == defaultWorkflowId)&&(identical(other.isActive, isActive) || other.isActive == isActive));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,icon,color,description,defaultWorkflowId,isActive);

@override
String toString() {
  return 'CreateUpdateTaskTypeDto(name: $name, icon: $icon, color: $color, description: $description, defaultWorkflowId: $defaultWorkflowId, isActive: $isActive)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateTaskTypeDtoCopyWith<$Res> implements $CreateUpdateTaskTypeDtoCopyWith<$Res> {
  factory _$CreateUpdateTaskTypeDtoCopyWith(_CreateUpdateTaskTypeDto value, $Res Function(_CreateUpdateTaskTypeDto) _then) = __$CreateUpdateTaskTypeDtoCopyWithImpl;
@override @useResult
$Res call({
 String name, String icon, String color, String? description, String defaultWorkflowId, bool isActive
});




}
/// @nodoc
class __$CreateUpdateTaskTypeDtoCopyWithImpl<$Res>
    implements _$CreateUpdateTaskTypeDtoCopyWith<$Res> {
  __$CreateUpdateTaskTypeDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateTaskTypeDto _self;
  final $Res Function(_CreateUpdateTaskTypeDto) _then;

/// Create a copy of CreateUpdateTaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,Object? defaultWorkflowId = null,Object? isActive = null,}) {
  return _then(_CreateUpdateTaskTypeDto(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,defaultWorkflowId: null == defaultWorkflowId ? _self.defaultWorkflowId : defaultWorkflowId // ignore: cast_nullable_to_non_nullable
as String,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

// dart format on
