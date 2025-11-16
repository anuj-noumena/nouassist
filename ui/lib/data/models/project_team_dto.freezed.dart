// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_team_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectTeamDto {

 String get id; String get projectId; String get userId; String get userName; String get roleId; String get roleName; DateTime get assignedDate; double? get billableRate;
/// Create a copy of ProjectTeamDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectTeamDtoCopyWith<ProjectTeamDto> get copyWith => _$ProjectTeamDtoCopyWithImpl<ProjectTeamDto>(this as ProjectTeamDto, _$identity);

  /// Serializes this ProjectTeamDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectTeamDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.userName, userName) || other.userName == userName)&&(identical(other.roleId, roleId) || other.roleId == roleId)&&(identical(other.roleName, roleName) || other.roleName == roleName)&&(identical(other.assignedDate, assignedDate) || other.assignedDate == assignedDate)&&(identical(other.billableRate, billableRate) || other.billableRate == billableRate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,userId,userName,roleId,roleName,assignedDate,billableRate);

@override
String toString() {
  return 'ProjectTeamDto(id: $id, projectId: $projectId, userId: $userId, userName: $userName, roleId: $roleId, roleName: $roleName, assignedDate: $assignedDate, billableRate: $billableRate)';
}


}

/// @nodoc
abstract mixin class $ProjectTeamDtoCopyWith<$Res>  {
  factory $ProjectTeamDtoCopyWith(ProjectTeamDto value, $Res Function(ProjectTeamDto) _then) = _$ProjectTeamDtoCopyWithImpl;
@useResult
$Res call({
 String id, String projectId, String userId, String userName, String roleId, String roleName, DateTime assignedDate, double? billableRate
});




}
/// @nodoc
class _$ProjectTeamDtoCopyWithImpl<$Res>
    implements $ProjectTeamDtoCopyWith<$Res> {
  _$ProjectTeamDtoCopyWithImpl(this._self, this._then);

  final ProjectTeamDto _self;
  final $Res Function(ProjectTeamDto) _then;

/// Create a copy of ProjectTeamDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? projectId = null,Object? userId = null,Object? userName = null,Object? roleId = null,Object? roleName = null,Object? assignedDate = null,Object? billableRate = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,userName: null == userName ? _self.userName : userName // ignore: cast_nullable_to_non_nullable
as String,roleId: null == roleId ? _self.roleId : roleId // ignore: cast_nullable_to_non_nullable
as String,roleName: null == roleName ? _self.roleName : roleName // ignore: cast_nullable_to_non_nullable
as String,assignedDate: null == assignedDate ? _self.assignedDate : assignedDate // ignore: cast_nullable_to_non_nullable
as DateTime,billableRate: freezed == billableRate ? _self.billableRate : billableRate // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectTeamDto].
extension ProjectTeamDtoPatterns on ProjectTeamDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectTeamDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectTeamDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectTeamDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectTeamDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectTeamDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectTeamDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String projectId,  String userId,  String userName,  String roleId,  String roleName,  DateTime assignedDate,  double? billableRate)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectTeamDto() when $default != null:
return $default(_that.id,_that.projectId,_that.userId,_that.userName,_that.roleId,_that.roleName,_that.assignedDate,_that.billableRate);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String projectId,  String userId,  String userName,  String roleId,  String roleName,  DateTime assignedDate,  double? billableRate)  $default,) {final _that = this;
switch (_that) {
case _ProjectTeamDto():
return $default(_that.id,_that.projectId,_that.userId,_that.userName,_that.roleId,_that.roleName,_that.assignedDate,_that.billableRate);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String projectId,  String userId,  String userName,  String roleId,  String roleName,  DateTime assignedDate,  double? billableRate)?  $default,) {final _that = this;
switch (_that) {
case _ProjectTeamDto() when $default != null:
return $default(_that.id,_that.projectId,_that.userId,_that.userName,_that.roleId,_that.roleName,_that.assignedDate,_that.billableRate);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectTeamDto implements ProjectTeamDto {
  const _ProjectTeamDto({required this.id, required this.projectId, required this.userId, required this.userName, required this.roleId, required this.roleName, required this.assignedDate, this.billableRate});
  factory _ProjectTeamDto.fromJson(Map<String, dynamic> json) => _$ProjectTeamDtoFromJson(json);

@override final  String id;
@override final  String projectId;
@override final  String userId;
@override final  String userName;
@override final  String roleId;
@override final  String roleName;
@override final  DateTime assignedDate;
@override final  double? billableRate;

/// Create a copy of ProjectTeamDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectTeamDtoCopyWith<_ProjectTeamDto> get copyWith => __$ProjectTeamDtoCopyWithImpl<_ProjectTeamDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectTeamDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectTeamDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.userName, userName) || other.userName == userName)&&(identical(other.roleId, roleId) || other.roleId == roleId)&&(identical(other.roleName, roleName) || other.roleName == roleName)&&(identical(other.assignedDate, assignedDate) || other.assignedDate == assignedDate)&&(identical(other.billableRate, billableRate) || other.billableRate == billableRate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,userId,userName,roleId,roleName,assignedDate,billableRate);

@override
String toString() {
  return 'ProjectTeamDto(id: $id, projectId: $projectId, userId: $userId, userName: $userName, roleId: $roleId, roleName: $roleName, assignedDate: $assignedDate, billableRate: $billableRate)';
}


}

/// @nodoc
abstract mixin class _$ProjectTeamDtoCopyWith<$Res> implements $ProjectTeamDtoCopyWith<$Res> {
  factory _$ProjectTeamDtoCopyWith(_ProjectTeamDto value, $Res Function(_ProjectTeamDto) _then) = __$ProjectTeamDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String projectId, String userId, String userName, String roleId, String roleName, DateTime assignedDate, double? billableRate
});




}
/// @nodoc
class __$ProjectTeamDtoCopyWithImpl<$Res>
    implements _$ProjectTeamDtoCopyWith<$Res> {
  __$ProjectTeamDtoCopyWithImpl(this._self, this._then);

  final _ProjectTeamDto _self;
  final $Res Function(_ProjectTeamDto) _then;

/// Create a copy of ProjectTeamDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? projectId = null,Object? userId = null,Object? userName = null,Object? roleId = null,Object? roleName = null,Object? assignedDate = null,Object? billableRate = freezed,}) {
  return _then(_ProjectTeamDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,userName: null == userName ? _self.userName : userName // ignore: cast_nullable_to_non_nullable
as String,roleId: null == roleId ? _self.roleId : roleId // ignore: cast_nullable_to_non_nullable
as String,roleName: null == roleName ? _self.roleName : roleName // ignore: cast_nullable_to_non_nullable
as String,assignedDate: null == assignedDate ? _self.assignedDate : assignedDate // ignore: cast_nullable_to_non_nullable
as DateTime,billableRate: freezed == billableRate ? _self.billableRate : billableRate // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}


}


/// @nodoc
mixin _$AddTeamMemberDto {

 String get userId; String get roleId; double? get billableRate;
/// Create a copy of AddTeamMemberDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$AddTeamMemberDtoCopyWith<AddTeamMemberDto> get copyWith => _$AddTeamMemberDtoCopyWithImpl<AddTeamMemberDto>(this as AddTeamMemberDto, _$identity);

  /// Serializes this AddTeamMemberDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is AddTeamMemberDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.roleId, roleId) || other.roleId == roleId)&&(identical(other.billableRate, billableRate) || other.billableRate == billableRate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,roleId,billableRate);

@override
String toString() {
  return 'AddTeamMemberDto(userId: $userId, roleId: $roleId, billableRate: $billableRate)';
}


}

/// @nodoc
abstract mixin class $AddTeamMemberDtoCopyWith<$Res>  {
  factory $AddTeamMemberDtoCopyWith(AddTeamMemberDto value, $Res Function(AddTeamMemberDto) _then) = _$AddTeamMemberDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String roleId, double? billableRate
});




}
/// @nodoc
class _$AddTeamMemberDtoCopyWithImpl<$Res>
    implements $AddTeamMemberDtoCopyWith<$Res> {
  _$AddTeamMemberDtoCopyWithImpl(this._self, this._then);

  final AddTeamMemberDto _self;
  final $Res Function(AddTeamMemberDto) _then;

/// Create a copy of AddTeamMemberDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? roleId = null,Object? billableRate = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,roleId: null == roleId ? _self.roleId : roleId // ignore: cast_nullable_to_non_nullable
as String,billableRate: freezed == billableRate ? _self.billableRate : billableRate // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}

}


/// Adds pattern-matching-related methods to [AddTeamMemberDto].
extension AddTeamMemberDtoPatterns on AddTeamMemberDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _AddTeamMemberDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _AddTeamMemberDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _AddTeamMemberDto value)  $default,){
final _that = this;
switch (_that) {
case _AddTeamMemberDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _AddTeamMemberDto value)?  $default,){
final _that = this;
switch (_that) {
case _AddTeamMemberDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String roleId,  double? billableRate)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _AddTeamMemberDto() when $default != null:
return $default(_that.userId,_that.roleId,_that.billableRate);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String roleId,  double? billableRate)  $default,) {final _that = this;
switch (_that) {
case _AddTeamMemberDto():
return $default(_that.userId,_that.roleId,_that.billableRate);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String roleId,  double? billableRate)?  $default,) {final _that = this;
switch (_that) {
case _AddTeamMemberDto() when $default != null:
return $default(_that.userId,_that.roleId,_that.billableRate);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _AddTeamMemberDto implements AddTeamMemberDto {
  const _AddTeamMemberDto({required this.userId, required this.roleId, this.billableRate});
  factory _AddTeamMemberDto.fromJson(Map<String, dynamic> json) => _$AddTeamMemberDtoFromJson(json);

@override final  String userId;
@override final  String roleId;
@override final  double? billableRate;

/// Create a copy of AddTeamMemberDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$AddTeamMemberDtoCopyWith<_AddTeamMemberDto> get copyWith => __$AddTeamMemberDtoCopyWithImpl<_AddTeamMemberDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$AddTeamMemberDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _AddTeamMemberDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.roleId, roleId) || other.roleId == roleId)&&(identical(other.billableRate, billableRate) || other.billableRate == billableRate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,roleId,billableRate);

@override
String toString() {
  return 'AddTeamMemberDto(userId: $userId, roleId: $roleId, billableRate: $billableRate)';
}


}

/// @nodoc
abstract mixin class _$AddTeamMemberDtoCopyWith<$Res> implements $AddTeamMemberDtoCopyWith<$Res> {
  factory _$AddTeamMemberDtoCopyWith(_AddTeamMemberDto value, $Res Function(_AddTeamMemberDto) _then) = __$AddTeamMemberDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String roleId, double? billableRate
});




}
/// @nodoc
class __$AddTeamMemberDtoCopyWithImpl<$Res>
    implements _$AddTeamMemberDtoCopyWith<$Res> {
  __$AddTeamMemberDtoCopyWithImpl(this._self, this._then);

  final _AddTeamMemberDto _self;
  final $Res Function(_AddTeamMemberDto) _then;

/// Create a copy of AddTeamMemberDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? roleId = null,Object? billableRate = freezed,}) {
  return _then(_AddTeamMemberDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,roleId: null == roleId ? _self.roleId : roleId // ignore: cast_nullable_to_non_nullable
as String,billableRate: freezed == billableRate ? _self.billableRate : billableRate // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}


}

// dart format on
