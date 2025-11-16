// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_context_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdContextDto {

 String get id; String get userId; String get name; String? get description; String get icon; String get color; ContextType get contextType; int get actionCount; bool get isActive; DateTime get creationTime;
/// Create a copy of GtdContextDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdContextDtoCopyWith<GtdContextDto> get copyWith => _$GtdContextDtoCopyWithImpl<GtdContextDto>(this as GtdContextDto, _$identity);

  /// Serializes this GtdContextDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdContextDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.contextType, contextType) || other.contextType == contextType)&&(identical(other.actionCount, actionCount) || other.actionCount == actionCount)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,name,description,icon,color,contextType,actionCount,isActive,creationTime);

@override
String toString() {
  return 'GtdContextDto(id: $id, userId: $userId, name: $name, description: $description, icon: $icon, color: $color, contextType: $contextType, actionCount: $actionCount, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdContextDtoCopyWith<$Res>  {
  factory $GtdContextDtoCopyWith(GtdContextDto value, $Res Function(GtdContextDto) _then) = _$GtdContextDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String name, String? description, String icon, String color, ContextType contextType, int actionCount, bool isActive, DateTime creationTime
});




}
/// @nodoc
class _$GtdContextDtoCopyWithImpl<$Res>
    implements $GtdContextDtoCopyWith<$Res> {
  _$GtdContextDtoCopyWithImpl(this._self, this._then);

  final GtdContextDto _self;
  final $Res Function(GtdContextDto) _then;

/// Create a copy of GtdContextDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? name = null,Object? description = freezed,Object? icon = null,Object? color = null,Object? contextType = null,Object? actionCount = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,contextType: null == contextType ? _self.contextType : contextType // ignore: cast_nullable_to_non_nullable
as ContextType,actionCount: null == actionCount ? _self.actionCount : actionCount // ignore: cast_nullable_to_non_nullable
as int,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdContextDto].
extension GtdContextDtoPatterns on GtdContextDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdContextDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdContextDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdContextDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdContextDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdContextDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdContextDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  int actionCount,  bool isActive,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdContextDto() when $default != null:
return $default(_that.id,_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.actionCount,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  int actionCount,  bool isActive,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdContextDto():
return $default(_that.id,_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.actionCount,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  int actionCount,  bool isActive,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdContextDto() when $default != null:
return $default(_that.id,_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.actionCount,_that.isActive,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdContextDto implements GtdContextDto {
  const _GtdContextDto({required this.id, required this.userId, required this.name, this.description, required this.icon, required this.color, required this.contextType, this.actionCount = 0, this.isActive = true, required this.creationTime});
  factory _GtdContextDto.fromJson(Map<String, dynamic> json) => _$GtdContextDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String name;
@override final  String? description;
@override final  String icon;
@override final  String color;
@override final  ContextType contextType;
@override@JsonKey() final  int actionCount;
@override@JsonKey() final  bool isActive;
@override final  DateTime creationTime;

/// Create a copy of GtdContextDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdContextDtoCopyWith<_GtdContextDto> get copyWith => __$GtdContextDtoCopyWithImpl<_GtdContextDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdContextDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdContextDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.contextType, contextType) || other.contextType == contextType)&&(identical(other.actionCount, actionCount) || other.actionCount == actionCount)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,name,description,icon,color,contextType,actionCount,isActive,creationTime);

@override
String toString() {
  return 'GtdContextDto(id: $id, userId: $userId, name: $name, description: $description, icon: $icon, color: $color, contextType: $contextType, actionCount: $actionCount, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdContextDtoCopyWith<$Res> implements $GtdContextDtoCopyWith<$Res> {
  factory _$GtdContextDtoCopyWith(_GtdContextDto value, $Res Function(_GtdContextDto) _then) = __$GtdContextDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String name, String? description, String icon, String color, ContextType contextType, int actionCount, bool isActive, DateTime creationTime
});




}
/// @nodoc
class __$GtdContextDtoCopyWithImpl<$Res>
    implements _$GtdContextDtoCopyWith<$Res> {
  __$GtdContextDtoCopyWithImpl(this._self, this._then);

  final _GtdContextDto _self;
  final $Res Function(_GtdContextDto) _then;

/// Create a copy of GtdContextDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? name = null,Object? description = freezed,Object? icon = null,Object? color = null,Object? contextType = null,Object? actionCount = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_GtdContextDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,contextType: null == contextType ? _self.contextType : contextType // ignore: cast_nullable_to_non_nullable
as ContextType,actionCount: null == actionCount ? _self.actionCount : actionCount // ignore: cast_nullable_to_non_nullable
as int,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateContextDto {

 String get userId; String get name; String? get description; String get icon; String get color; ContextType get contextType; bool get isActive;
/// Create a copy of CreateUpdateContextDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateContextDtoCopyWith<CreateUpdateContextDto> get copyWith => _$CreateUpdateContextDtoCopyWithImpl<CreateUpdateContextDto>(this as CreateUpdateContextDto, _$identity);

  /// Serializes this CreateUpdateContextDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateContextDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.contextType, contextType) || other.contextType == contextType)&&(identical(other.isActive, isActive) || other.isActive == isActive));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,name,description,icon,color,contextType,isActive);

@override
String toString() {
  return 'CreateUpdateContextDto(userId: $userId, name: $name, description: $description, icon: $icon, color: $color, contextType: $contextType, isActive: $isActive)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateContextDtoCopyWith<$Res>  {
  factory $CreateUpdateContextDtoCopyWith(CreateUpdateContextDto value, $Res Function(CreateUpdateContextDto) _then) = _$CreateUpdateContextDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String name, String? description, String icon, String color, ContextType contextType, bool isActive
});




}
/// @nodoc
class _$CreateUpdateContextDtoCopyWithImpl<$Res>
    implements $CreateUpdateContextDtoCopyWith<$Res> {
  _$CreateUpdateContextDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateContextDto _self;
  final $Res Function(CreateUpdateContextDto) _then;

/// Create a copy of CreateUpdateContextDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? name = null,Object? description = freezed,Object? icon = null,Object? color = null,Object? contextType = null,Object? isActive = null,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,contextType: null == contextType ? _self.contextType : contextType // ignore: cast_nullable_to_non_nullable
as ContextType,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateContextDto].
extension CreateUpdateContextDtoPatterns on CreateUpdateContextDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateContextDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateContextDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateContextDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateContextDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateContextDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateContextDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  bool isActive)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateContextDto() when $default != null:
return $default(_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.isActive);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  bool isActive)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateContextDto():
return $default(_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.isActive);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String name,  String? description,  String icon,  String color,  ContextType contextType,  bool isActive)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateContextDto() when $default != null:
return $default(_that.userId,_that.name,_that.description,_that.icon,_that.color,_that.contextType,_that.isActive);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateContextDto implements CreateUpdateContextDto {
  const _CreateUpdateContextDto({required this.userId, required this.name, this.description, required this.icon, required this.color, required this.contextType, this.isActive = true});
  factory _CreateUpdateContextDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateContextDtoFromJson(json);

@override final  String userId;
@override final  String name;
@override final  String? description;
@override final  String icon;
@override final  String color;
@override final  ContextType contextType;
@override@JsonKey() final  bool isActive;

/// Create a copy of CreateUpdateContextDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateContextDtoCopyWith<_CreateUpdateContextDto> get copyWith => __$CreateUpdateContextDtoCopyWithImpl<_CreateUpdateContextDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateContextDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateContextDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.contextType, contextType) || other.contextType == contextType)&&(identical(other.isActive, isActive) || other.isActive == isActive));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,name,description,icon,color,contextType,isActive);

@override
String toString() {
  return 'CreateUpdateContextDto(userId: $userId, name: $name, description: $description, icon: $icon, color: $color, contextType: $contextType, isActive: $isActive)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateContextDtoCopyWith<$Res> implements $CreateUpdateContextDtoCopyWith<$Res> {
  factory _$CreateUpdateContextDtoCopyWith(_CreateUpdateContextDto value, $Res Function(_CreateUpdateContextDto) _then) = __$CreateUpdateContextDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String name, String? description, String icon, String color, ContextType contextType, bool isActive
});




}
/// @nodoc
class __$CreateUpdateContextDtoCopyWithImpl<$Res>
    implements _$CreateUpdateContextDtoCopyWith<$Res> {
  __$CreateUpdateContextDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateContextDto _self;
  final $Res Function(_CreateUpdateContextDto) _then;

/// Create a copy of CreateUpdateContextDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? name = null,Object? description = freezed,Object? icon = null,Object? color = null,Object? contextType = null,Object? isActive = null,}) {
  return _then(_CreateUpdateContextDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,contextType: null == contextType ? _self.contextType : contextType // ignore: cast_nullable_to_non_nullable
as ContextType,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

// dart format on
