// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_tag_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskTagDto {

 String get id; String get name; String get color; String? get description; String? get category; int get usageCount; DateTime get creationTime;
/// Create a copy of TaskTagDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskTagDtoCopyWith<TaskTagDto> get copyWith => _$TaskTagDtoCopyWithImpl<TaskTagDto>(this as TaskTagDto, _$identity);

  /// Serializes this TaskTagDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskTagDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.usageCount, usageCount) || other.usageCount == usageCount)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,color,description,category,usageCount,creationTime);

@override
String toString() {
  return 'TaskTagDto(id: $id, name: $name, color: $color, description: $description, category: $category, usageCount: $usageCount, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TaskTagDtoCopyWith<$Res>  {
  factory $TaskTagDtoCopyWith(TaskTagDto value, $Res Function(TaskTagDto) _then) = _$TaskTagDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String color, String? description, String? category, int usageCount, DateTime creationTime
});




}
/// @nodoc
class _$TaskTagDtoCopyWithImpl<$Res>
    implements $TaskTagDtoCopyWith<$Res> {
  _$TaskTagDtoCopyWithImpl(this._self, this._then);

  final TaskTagDto _self;
  final $Res Function(TaskTagDto) _then;

/// Create a copy of TaskTagDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? color = null,Object? description = freezed,Object? category = freezed,Object? usageCount = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,usageCount: null == usageCount ? _self.usageCount : usageCount // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskTagDto].
extension TaskTagDtoPatterns on TaskTagDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskTagDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskTagDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskTagDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskTagDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskTagDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskTagDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String color,  String? description,  String? category,  int usageCount,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskTagDto() when $default != null:
return $default(_that.id,_that.name,_that.color,_that.description,_that.category,_that.usageCount,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String color,  String? description,  String? category,  int usageCount,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskTagDto():
return $default(_that.id,_that.name,_that.color,_that.description,_that.category,_that.usageCount,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String color,  String? description,  String? category,  int usageCount,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskTagDto() when $default != null:
return $default(_that.id,_that.name,_that.color,_that.description,_that.category,_that.usageCount,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskTagDto implements TaskTagDto {
  const _TaskTagDto({required this.id, required this.name, required this.color, this.description, this.category, this.usageCount = 0, required this.creationTime});
  factory _TaskTagDto.fromJson(Map<String, dynamic> json) => _$TaskTagDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String color;
@override final  String? description;
@override final  String? category;
@override@JsonKey() final  int usageCount;
@override final  DateTime creationTime;

/// Create a copy of TaskTagDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskTagDtoCopyWith<_TaskTagDto> get copyWith => __$TaskTagDtoCopyWithImpl<_TaskTagDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskTagDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskTagDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.usageCount, usageCount) || other.usageCount == usageCount)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,color,description,category,usageCount,creationTime);

@override
String toString() {
  return 'TaskTagDto(id: $id, name: $name, color: $color, description: $description, category: $category, usageCount: $usageCount, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskTagDtoCopyWith<$Res> implements $TaskTagDtoCopyWith<$Res> {
  factory _$TaskTagDtoCopyWith(_TaskTagDto value, $Res Function(_TaskTagDto) _then) = __$TaskTagDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String color, String? description, String? category, int usageCount, DateTime creationTime
});




}
/// @nodoc
class __$TaskTagDtoCopyWithImpl<$Res>
    implements _$TaskTagDtoCopyWith<$Res> {
  __$TaskTagDtoCopyWithImpl(this._self, this._then);

  final _TaskTagDto _self;
  final $Res Function(_TaskTagDto) _then;

/// Create a copy of TaskTagDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? color = null,Object? description = freezed,Object? category = freezed,Object? usageCount = null,Object? creationTime = null,}) {
  return _then(_TaskTagDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,usageCount: null == usageCount ? _self.usageCount : usageCount // ignore: cast_nullable_to_non_nullable
as int,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateTaskTagDto {

 String get name; String get color; String? get description; String? get category;
/// Create a copy of CreateUpdateTaskTagDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateTaskTagDtoCopyWith<CreateUpdateTaskTagDto> get copyWith => _$CreateUpdateTaskTagDtoCopyWithImpl<CreateUpdateTaskTagDto>(this as CreateUpdateTaskTagDto, _$identity);

  /// Serializes this CreateUpdateTaskTagDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateTaskTagDto&&(identical(other.name, name) || other.name == name)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,color,description,category);

@override
String toString() {
  return 'CreateUpdateTaskTagDto(name: $name, color: $color, description: $description, category: $category)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateTaskTagDtoCopyWith<$Res>  {
  factory $CreateUpdateTaskTagDtoCopyWith(CreateUpdateTaskTagDto value, $Res Function(CreateUpdateTaskTagDto) _then) = _$CreateUpdateTaskTagDtoCopyWithImpl;
@useResult
$Res call({
 String name, String color, String? description, String? category
});




}
/// @nodoc
class _$CreateUpdateTaskTagDtoCopyWithImpl<$Res>
    implements $CreateUpdateTaskTagDtoCopyWith<$Res> {
  _$CreateUpdateTaskTagDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateTaskTagDto _self;
  final $Res Function(CreateUpdateTaskTagDto) _then;

/// Create a copy of CreateUpdateTaskTagDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? color = null,Object? description = freezed,Object? category = freezed,}) {
  return _then(_self.copyWith(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateTaskTagDto].
extension CreateUpdateTaskTagDtoPatterns on CreateUpdateTaskTagDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateTaskTagDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateTaskTagDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateTaskTagDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String color,  String? description,  String? category)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto() when $default != null:
return $default(_that.name,_that.color,_that.description,_that.category);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String color,  String? description,  String? category)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto():
return $default(_that.name,_that.color,_that.description,_that.category);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String color,  String? description,  String? category)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskTagDto() when $default != null:
return $default(_that.name,_that.color,_that.description,_that.category);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateTaskTagDto implements CreateUpdateTaskTagDto {
  const _CreateUpdateTaskTagDto({required this.name, required this.color, this.description, this.category});
  factory _CreateUpdateTaskTagDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateTaskTagDtoFromJson(json);

@override final  String name;
@override final  String color;
@override final  String? description;
@override final  String? category;

/// Create a copy of CreateUpdateTaskTagDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateTaskTagDtoCopyWith<_CreateUpdateTaskTagDto> get copyWith => __$CreateUpdateTaskTagDtoCopyWithImpl<_CreateUpdateTaskTagDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateTaskTagDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateTaskTagDto&&(identical(other.name, name) || other.name == name)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,color,description,category);

@override
String toString() {
  return 'CreateUpdateTaskTagDto(name: $name, color: $color, description: $description, category: $category)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateTaskTagDtoCopyWith<$Res> implements $CreateUpdateTaskTagDtoCopyWith<$Res> {
  factory _$CreateUpdateTaskTagDtoCopyWith(_CreateUpdateTaskTagDto value, $Res Function(_CreateUpdateTaskTagDto) _then) = __$CreateUpdateTaskTagDtoCopyWithImpl;
@override @useResult
$Res call({
 String name, String color, String? description, String? category
});




}
/// @nodoc
class __$CreateUpdateTaskTagDtoCopyWithImpl<$Res>
    implements _$CreateUpdateTaskTagDtoCopyWith<$Res> {
  __$CreateUpdateTaskTagDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateTaskTagDto _self;
  final $Res Function(_CreateUpdateTaskTagDto) _then;

/// Create a copy of CreateUpdateTaskTagDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? color = null,Object? description = freezed,Object? category = freezed,}) {
  return _then(_CreateUpdateTaskTagDto(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
