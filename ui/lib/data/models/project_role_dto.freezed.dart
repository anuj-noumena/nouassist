// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_role_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectRoleDto {

 String get id; String get name; String? get description; List<String> get permissions; bool get isSystemRole; DateTime get creationTime;
/// Create a copy of ProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectRoleDtoCopyWith<ProjectRoleDto> get copyWith => _$ProjectRoleDtoCopyWithImpl<ProjectRoleDto>(this as ProjectRoleDto, _$identity);

  /// Serializes this ProjectRoleDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectRoleDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other.permissions, permissions)&&(identical(other.isSystemRole, isSystemRole) || other.isSystemRole == isSystemRole)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(permissions),isSystemRole,creationTime);

@override
String toString() {
  return 'ProjectRoleDto(id: $id, name: $name, description: $description, permissions: $permissions, isSystemRole: $isSystemRole, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $ProjectRoleDtoCopyWith<$Res>  {
  factory $ProjectRoleDtoCopyWith(ProjectRoleDto value, $Res Function(ProjectRoleDto) _then) = _$ProjectRoleDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String? description, List<String> permissions, bool isSystemRole, DateTime creationTime
});




}
/// @nodoc
class _$ProjectRoleDtoCopyWithImpl<$Res>
    implements $ProjectRoleDtoCopyWith<$Res> {
  _$ProjectRoleDtoCopyWithImpl(this._self, this._then);

  final ProjectRoleDto _self;
  final $Res Function(ProjectRoleDto) _then;

/// Create a copy of ProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? permissions = null,Object? isSystemRole = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,permissions: null == permissions ? _self.permissions : permissions // ignore: cast_nullable_to_non_nullable
as List<String>,isSystemRole: null == isSystemRole ? _self.isSystemRole : isSystemRole // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectRoleDto].
extension ProjectRoleDtoPatterns on ProjectRoleDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectRoleDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectRoleDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectRoleDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectRoleDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectRoleDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectRoleDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<String> permissions,  bool isSystemRole,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectRoleDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.permissions,_that.isSystemRole,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<String> permissions,  bool isSystemRole,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectRoleDto():
return $default(_that.id,_that.name,_that.description,_that.permissions,_that.isSystemRole,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String? description,  List<String> permissions,  bool isSystemRole,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectRoleDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.permissions,_that.isSystemRole,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectRoleDto implements ProjectRoleDto {
  const _ProjectRoleDto({required this.id, required this.name, this.description, required final  List<String> permissions, this.isSystemRole = false, required this.creationTime}): _permissions = permissions;
  factory _ProjectRoleDto.fromJson(Map<String, dynamic> json) => _$ProjectRoleDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String? description;
 final  List<String> _permissions;
@override List<String> get permissions {
  if (_permissions is EqualUnmodifiableListView) return _permissions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_permissions);
}

@override@JsonKey() final  bool isSystemRole;
@override final  DateTime creationTime;

/// Create a copy of ProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectRoleDtoCopyWith<_ProjectRoleDto> get copyWith => __$ProjectRoleDtoCopyWithImpl<_ProjectRoleDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectRoleDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectRoleDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other._permissions, _permissions)&&(identical(other.isSystemRole, isSystemRole) || other.isSystemRole == isSystemRole)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(_permissions),isSystemRole,creationTime);

@override
String toString() {
  return 'ProjectRoleDto(id: $id, name: $name, description: $description, permissions: $permissions, isSystemRole: $isSystemRole, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectRoleDtoCopyWith<$Res> implements $ProjectRoleDtoCopyWith<$Res> {
  factory _$ProjectRoleDtoCopyWith(_ProjectRoleDto value, $Res Function(_ProjectRoleDto) _then) = __$ProjectRoleDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String? description, List<String> permissions, bool isSystemRole, DateTime creationTime
});




}
/// @nodoc
class __$ProjectRoleDtoCopyWithImpl<$Res>
    implements _$ProjectRoleDtoCopyWith<$Res> {
  __$ProjectRoleDtoCopyWithImpl(this._self, this._then);

  final _ProjectRoleDto _self;
  final $Res Function(_ProjectRoleDto) _then;

/// Create a copy of ProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? permissions = null,Object? isSystemRole = null,Object? creationTime = null,}) {
  return _then(_ProjectRoleDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,permissions: null == permissions ? _self._permissions : permissions // ignore: cast_nullable_to_non_nullable
as List<String>,isSystemRole: null == isSystemRole ? _self.isSystemRole : isSystemRole // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateProjectRoleDto {

 String get name; String? get description; List<String> get permissions;
/// Create a copy of CreateUpdateProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateProjectRoleDtoCopyWith<CreateUpdateProjectRoleDto> get copyWith => _$CreateUpdateProjectRoleDtoCopyWithImpl<CreateUpdateProjectRoleDto>(this as CreateUpdateProjectRoleDto, _$identity);

  /// Serializes this CreateUpdateProjectRoleDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateProjectRoleDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other.permissions, permissions));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,const DeepCollectionEquality().hash(permissions));

@override
String toString() {
  return 'CreateUpdateProjectRoleDto(name: $name, description: $description, permissions: $permissions)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateProjectRoleDtoCopyWith<$Res>  {
  factory $CreateUpdateProjectRoleDtoCopyWith(CreateUpdateProjectRoleDto value, $Res Function(CreateUpdateProjectRoleDto) _then) = _$CreateUpdateProjectRoleDtoCopyWithImpl;
@useResult
$Res call({
 String name, String? description, List<String> permissions
});




}
/// @nodoc
class _$CreateUpdateProjectRoleDtoCopyWithImpl<$Res>
    implements $CreateUpdateProjectRoleDtoCopyWith<$Res> {
  _$CreateUpdateProjectRoleDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateProjectRoleDto _self;
  final $Res Function(CreateUpdateProjectRoleDto) _then;

/// Create a copy of CreateUpdateProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? description = freezed,Object? permissions = null,}) {
  return _then(_self.copyWith(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,permissions: null == permissions ? _self.permissions : permissions // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateProjectRoleDto].
extension CreateUpdateProjectRoleDtoPatterns on CreateUpdateProjectRoleDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateProjectRoleDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateProjectRoleDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateProjectRoleDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String? description,  List<String> permissions)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto() when $default != null:
return $default(_that.name,_that.description,_that.permissions);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String? description,  List<String> permissions)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto():
return $default(_that.name,_that.description,_that.permissions);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String? description,  List<String> permissions)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateProjectRoleDto() when $default != null:
return $default(_that.name,_that.description,_that.permissions);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateProjectRoleDto implements CreateUpdateProjectRoleDto {
  const _CreateUpdateProjectRoleDto({required this.name, this.description, required final  List<String> permissions}): _permissions = permissions;
  factory _CreateUpdateProjectRoleDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateProjectRoleDtoFromJson(json);

@override final  String name;
@override final  String? description;
 final  List<String> _permissions;
@override List<String> get permissions {
  if (_permissions is EqualUnmodifiableListView) return _permissions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_permissions);
}


/// Create a copy of CreateUpdateProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateProjectRoleDtoCopyWith<_CreateUpdateProjectRoleDto> get copyWith => __$CreateUpdateProjectRoleDtoCopyWithImpl<_CreateUpdateProjectRoleDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateProjectRoleDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateProjectRoleDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other._permissions, _permissions));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,const DeepCollectionEquality().hash(_permissions));

@override
String toString() {
  return 'CreateUpdateProjectRoleDto(name: $name, description: $description, permissions: $permissions)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateProjectRoleDtoCopyWith<$Res> implements $CreateUpdateProjectRoleDtoCopyWith<$Res> {
  factory _$CreateUpdateProjectRoleDtoCopyWith(_CreateUpdateProjectRoleDto value, $Res Function(_CreateUpdateProjectRoleDto) _then) = __$CreateUpdateProjectRoleDtoCopyWithImpl;
@override @useResult
$Res call({
 String name, String? description, List<String> permissions
});




}
/// @nodoc
class __$CreateUpdateProjectRoleDtoCopyWithImpl<$Res>
    implements _$CreateUpdateProjectRoleDtoCopyWith<$Res> {
  __$CreateUpdateProjectRoleDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateProjectRoleDto _self;
  final $Res Function(_CreateUpdateProjectRoleDto) _then;

/// Create a copy of CreateUpdateProjectRoleDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? description = freezed,Object? permissions = null,}) {
  return _then(_CreateUpdateProjectRoleDto(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,permissions: null == permissions ? _self._permissions : permissions // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}


}

// dart format on
