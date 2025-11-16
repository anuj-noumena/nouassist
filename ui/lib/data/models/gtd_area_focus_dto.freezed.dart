// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_area_focus_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdAreaFocusDto {

 String get id; String get userId; String get name; String? get description; String get color; String? get icon; int get projectCount; int get actionCount; bool get isActive; DateTime get creationTime;
/// Create a copy of GtdAreaFocusDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdAreaFocusDtoCopyWith<GtdAreaFocusDto> get copyWith => _$GtdAreaFocusDtoCopyWithImpl<GtdAreaFocusDto>(this as GtdAreaFocusDto, _$identity);

  /// Serializes this GtdAreaFocusDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdAreaFocusDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.color, color) || other.color == color)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.projectCount, projectCount) || other.projectCount == projectCount)&&(identical(other.actionCount, actionCount) || other.actionCount == actionCount)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,name,description,color,icon,projectCount,actionCount,isActive,creationTime);

@override
String toString() {
  return 'GtdAreaFocusDto(id: $id, userId: $userId, name: $name, description: $description, color: $color, icon: $icon, projectCount: $projectCount, actionCount: $actionCount, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdAreaFocusDtoCopyWith<$Res>  {
  factory $GtdAreaFocusDtoCopyWith(GtdAreaFocusDto value, $Res Function(GtdAreaFocusDto) _then) = _$GtdAreaFocusDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String name, String? description, String color, String? icon, int projectCount, int actionCount, bool isActive, DateTime creationTime
});




}
/// @nodoc
class _$GtdAreaFocusDtoCopyWithImpl<$Res>
    implements $GtdAreaFocusDtoCopyWith<$Res> {
  _$GtdAreaFocusDtoCopyWithImpl(this._self, this._then);

  final GtdAreaFocusDto _self;
  final $Res Function(GtdAreaFocusDto) _then;

/// Create a copy of GtdAreaFocusDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? name = null,Object? description = freezed,Object? color = null,Object? icon = freezed,Object? projectCount = null,Object? actionCount = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,icon: freezed == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String?,projectCount: null == projectCount ? _self.projectCount : projectCount // ignore: cast_nullable_to_non_nullable
as int,actionCount: null == actionCount ? _self.actionCount : actionCount // ignore: cast_nullable_to_non_nullable
as int,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdAreaFocusDto].
extension GtdAreaFocusDtoPatterns on GtdAreaFocusDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdAreaFocusDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdAreaFocusDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdAreaFocusDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdAreaFocusDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdAreaFocusDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdAreaFocusDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String name,  String? description,  String color,  String? icon,  int projectCount,  int actionCount,  bool isActive,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdAreaFocusDto() when $default != null:
return $default(_that.id,_that.userId,_that.name,_that.description,_that.color,_that.icon,_that.projectCount,_that.actionCount,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String name,  String? description,  String color,  String? icon,  int projectCount,  int actionCount,  bool isActive,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdAreaFocusDto():
return $default(_that.id,_that.userId,_that.name,_that.description,_that.color,_that.icon,_that.projectCount,_that.actionCount,_that.isActive,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String name,  String? description,  String color,  String? icon,  int projectCount,  int actionCount,  bool isActive,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdAreaFocusDto() when $default != null:
return $default(_that.id,_that.userId,_that.name,_that.description,_that.color,_that.icon,_that.projectCount,_that.actionCount,_that.isActive,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdAreaFocusDto implements GtdAreaFocusDto {
  const _GtdAreaFocusDto({required this.id, required this.userId, required this.name, this.description, required this.color, this.icon, this.projectCount = 0, this.actionCount = 0, this.isActive = true, required this.creationTime});
  factory _GtdAreaFocusDto.fromJson(Map<String, dynamic> json) => _$GtdAreaFocusDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String name;
@override final  String? description;
@override final  String color;
@override final  String? icon;
@override@JsonKey() final  int projectCount;
@override@JsonKey() final  int actionCount;
@override@JsonKey() final  bool isActive;
@override final  DateTime creationTime;

/// Create a copy of GtdAreaFocusDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdAreaFocusDtoCopyWith<_GtdAreaFocusDto> get copyWith => __$GtdAreaFocusDtoCopyWithImpl<_GtdAreaFocusDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdAreaFocusDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdAreaFocusDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.color, color) || other.color == color)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.projectCount, projectCount) || other.projectCount == projectCount)&&(identical(other.actionCount, actionCount) || other.actionCount == actionCount)&&(identical(other.isActive, isActive) || other.isActive == isActive)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,name,description,color,icon,projectCount,actionCount,isActive,creationTime);

@override
String toString() {
  return 'GtdAreaFocusDto(id: $id, userId: $userId, name: $name, description: $description, color: $color, icon: $icon, projectCount: $projectCount, actionCount: $actionCount, isActive: $isActive, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdAreaFocusDtoCopyWith<$Res> implements $GtdAreaFocusDtoCopyWith<$Res> {
  factory _$GtdAreaFocusDtoCopyWith(_GtdAreaFocusDto value, $Res Function(_GtdAreaFocusDto) _then) = __$GtdAreaFocusDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String name, String? description, String color, String? icon, int projectCount, int actionCount, bool isActive, DateTime creationTime
});




}
/// @nodoc
class __$GtdAreaFocusDtoCopyWithImpl<$Res>
    implements _$GtdAreaFocusDtoCopyWith<$Res> {
  __$GtdAreaFocusDtoCopyWithImpl(this._self, this._then);

  final _GtdAreaFocusDto _self;
  final $Res Function(_GtdAreaFocusDto) _then;

/// Create a copy of GtdAreaFocusDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? name = null,Object? description = freezed,Object? color = null,Object? icon = freezed,Object? projectCount = null,Object? actionCount = null,Object? isActive = null,Object? creationTime = null,}) {
  return _then(_GtdAreaFocusDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,icon: freezed == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String?,projectCount: null == projectCount ? _self.projectCount : projectCount // ignore: cast_nullable_to_non_nullable
as int,actionCount: null == actionCount ? _self.actionCount : actionCount // ignore: cast_nullable_to_non_nullable
as int,isActive: null == isActive ? _self.isActive : isActive // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}

// dart format on
