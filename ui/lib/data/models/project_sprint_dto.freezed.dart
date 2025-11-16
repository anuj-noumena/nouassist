// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_sprint_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectSprintDto {

 String get id; String get projectId; String get name; String? get goal; DateTime get startDate; DateTime get endDate; SprintStatus get status; int? get velocity; int? get capacity; double get actualHours; DateTime get creationTime;
/// Create a copy of ProjectSprintDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectSprintDtoCopyWith<ProjectSprintDto> get copyWith => _$ProjectSprintDtoCopyWithImpl<ProjectSprintDto>(this as ProjectSprintDto, _$identity);

  /// Serializes this ProjectSprintDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectSprintDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.name, name) || other.name == name)&&(identical(other.goal, goal) || other.goal == goal)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.velocity, velocity) || other.velocity == velocity)&&(identical(other.capacity, capacity) || other.capacity == capacity)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,name,goal,startDate,endDate,status,velocity,capacity,actualHours,creationTime);

@override
String toString() {
  return 'ProjectSprintDto(id: $id, projectId: $projectId, name: $name, goal: $goal, startDate: $startDate, endDate: $endDate, status: $status, velocity: $velocity, capacity: $capacity, actualHours: $actualHours, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $ProjectSprintDtoCopyWith<$Res>  {
  factory $ProjectSprintDtoCopyWith(ProjectSprintDto value, $Res Function(ProjectSprintDto) _then) = _$ProjectSprintDtoCopyWithImpl;
@useResult
$Res call({
 String id, String projectId, String name, String? goal, DateTime startDate, DateTime endDate, SprintStatus status, int? velocity, int? capacity, double actualHours, DateTime creationTime
});




}
/// @nodoc
class _$ProjectSprintDtoCopyWithImpl<$Res>
    implements $ProjectSprintDtoCopyWith<$Res> {
  _$ProjectSprintDtoCopyWithImpl(this._self, this._then);

  final ProjectSprintDto _self;
  final $Res Function(ProjectSprintDto) _then;

/// Create a copy of ProjectSprintDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? projectId = null,Object? name = null,Object? goal = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? velocity = freezed,Object? capacity = freezed,Object? actualHours = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,goal: freezed == goal ? _self.goal : goal // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SprintStatus,velocity: freezed == velocity ? _self.velocity : velocity // ignore: cast_nullable_to_non_nullable
as int?,capacity: freezed == capacity ? _self.capacity : capacity // ignore: cast_nullable_to_non_nullable
as int?,actualHours: null == actualHours ? _self.actualHours : actualHours // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectSprintDto].
extension ProjectSprintDtoPatterns on ProjectSprintDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectSprintDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectSprintDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectSprintDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectSprintDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectSprintDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectSprintDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  SprintStatus status,  int? velocity,  int? capacity,  double actualHours,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectSprintDto() when $default != null:
return $default(_that.id,_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.status,_that.velocity,_that.capacity,_that.actualHours,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  SprintStatus status,  int? velocity,  int? capacity,  double actualHours,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectSprintDto():
return $default(_that.id,_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.status,_that.velocity,_that.capacity,_that.actualHours,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  SprintStatus status,  int? velocity,  int? capacity,  double actualHours,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectSprintDto() when $default != null:
return $default(_that.id,_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.status,_that.velocity,_that.capacity,_that.actualHours,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectSprintDto implements ProjectSprintDto {
  const _ProjectSprintDto({required this.id, required this.projectId, required this.name, this.goal, required this.startDate, required this.endDate, required this.status, this.velocity, this.capacity, this.actualHours = 0.0, required this.creationTime});
  factory _ProjectSprintDto.fromJson(Map<String, dynamic> json) => _$ProjectSprintDtoFromJson(json);

@override final  String id;
@override final  String projectId;
@override final  String name;
@override final  String? goal;
@override final  DateTime startDate;
@override final  DateTime endDate;
@override final  SprintStatus status;
@override final  int? velocity;
@override final  int? capacity;
@override@JsonKey() final  double actualHours;
@override final  DateTime creationTime;

/// Create a copy of ProjectSprintDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectSprintDtoCopyWith<_ProjectSprintDto> get copyWith => __$ProjectSprintDtoCopyWithImpl<_ProjectSprintDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectSprintDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectSprintDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.name, name) || other.name == name)&&(identical(other.goal, goal) || other.goal == goal)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.velocity, velocity) || other.velocity == velocity)&&(identical(other.capacity, capacity) || other.capacity == capacity)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,name,goal,startDate,endDate,status,velocity,capacity,actualHours,creationTime);

@override
String toString() {
  return 'ProjectSprintDto(id: $id, projectId: $projectId, name: $name, goal: $goal, startDate: $startDate, endDate: $endDate, status: $status, velocity: $velocity, capacity: $capacity, actualHours: $actualHours, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectSprintDtoCopyWith<$Res> implements $ProjectSprintDtoCopyWith<$Res> {
  factory _$ProjectSprintDtoCopyWith(_ProjectSprintDto value, $Res Function(_ProjectSprintDto) _then) = __$ProjectSprintDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String projectId, String name, String? goal, DateTime startDate, DateTime endDate, SprintStatus status, int? velocity, int? capacity, double actualHours, DateTime creationTime
});




}
/// @nodoc
class __$ProjectSprintDtoCopyWithImpl<$Res>
    implements _$ProjectSprintDtoCopyWith<$Res> {
  __$ProjectSprintDtoCopyWithImpl(this._self, this._then);

  final _ProjectSprintDto _self;
  final $Res Function(_ProjectSprintDto) _then;

/// Create a copy of ProjectSprintDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? projectId = null,Object? name = null,Object? goal = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? velocity = freezed,Object? capacity = freezed,Object? actualHours = null,Object? creationTime = null,}) {
  return _then(_ProjectSprintDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,goal: freezed == goal ? _self.goal : goal // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SprintStatus,velocity: freezed == velocity ? _self.velocity : velocity // ignore: cast_nullable_to_non_nullable
as int?,capacity: freezed == capacity ? _self.capacity : capacity // ignore: cast_nullable_to_non_nullable
as int?,actualHours: null == actualHours ? _self.actualHours : actualHours // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateSprintDto {

 String get projectId; String get name; String? get goal; DateTime get startDate; DateTime get endDate; int? get velocity; int? get capacity;
/// Create a copy of CreateUpdateSprintDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateSprintDtoCopyWith<CreateUpdateSprintDto> get copyWith => _$CreateUpdateSprintDtoCopyWithImpl<CreateUpdateSprintDto>(this as CreateUpdateSprintDto, _$identity);

  /// Serializes this CreateUpdateSprintDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateSprintDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.name, name) || other.name == name)&&(identical(other.goal, goal) || other.goal == goal)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.velocity, velocity) || other.velocity == velocity)&&(identical(other.capacity, capacity) || other.capacity == capacity));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,name,goal,startDate,endDate,velocity,capacity);

@override
String toString() {
  return 'CreateUpdateSprintDto(projectId: $projectId, name: $name, goal: $goal, startDate: $startDate, endDate: $endDate, velocity: $velocity, capacity: $capacity)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateSprintDtoCopyWith<$Res>  {
  factory $CreateUpdateSprintDtoCopyWith(CreateUpdateSprintDto value, $Res Function(CreateUpdateSprintDto) _then) = _$CreateUpdateSprintDtoCopyWithImpl;
@useResult
$Res call({
 String projectId, String name, String? goal, DateTime startDate, DateTime endDate, int? velocity, int? capacity
});




}
/// @nodoc
class _$CreateUpdateSprintDtoCopyWithImpl<$Res>
    implements $CreateUpdateSprintDtoCopyWith<$Res> {
  _$CreateUpdateSprintDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateSprintDto _self;
  final $Res Function(CreateUpdateSprintDto) _then;

/// Create a copy of CreateUpdateSprintDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? projectId = null,Object? name = null,Object? goal = freezed,Object? startDate = null,Object? endDate = null,Object? velocity = freezed,Object? capacity = freezed,}) {
  return _then(_self.copyWith(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,goal: freezed == goal ? _self.goal : goal // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,velocity: freezed == velocity ? _self.velocity : velocity // ignore: cast_nullable_to_non_nullable
as int?,capacity: freezed == capacity ? _self.capacity : capacity // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateSprintDto].
extension CreateUpdateSprintDtoPatterns on CreateUpdateSprintDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateSprintDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateSprintDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateSprintDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSprintDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateSprintDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSprintDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  int? velocity,  int? capacity)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateSprintDto() when $default != null:
return $default(_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.velocity,_that.capacity);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  int? velocity,  int? capacity)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSprintDto():
return $default(_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.velocity,_that.capacity);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String projectId,  String name,  String? goal,  DateTime startDate,  DateTime endDate,  int? velocity,  int? capacity)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSprintDto() when $default != null:
return $default(_that.projectId,_that.name,_that.goal,_that.startDate,_that.endDate,_that.velocity,_that.capacity);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateSprintDto implements CreateUpdateSprintDto {
  const _CreateUpdateSprintDto({required this.projectId, required this.name, this.goal, required this.startDate, required this.endDate, this.velocity, this.capacity});
  factory _CreateUpdateSprintDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateSprintDtoFromJson(json);

@override final  String projectId;
@override final  String name;
@override final  String? goal;
@override final  DateTime startDate;
@override final  DateTime endDate;
@override final  int? velocity;
@override final  int? capacity;

/// Create a copy of CreateUpdateSprintDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateSprintDtoCopyWith<_CreateUpdateSprintDto> get copyWith => __$CreateUpdateSprintDtoCopyWithImpl<_CreateUpdateSprintDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateSprintDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateSprintDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.name, name) || other.name == name)&&(identical(other.goal, goal) || other.goal == goal)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.velocity, velocity) || other.velocity == velocity)&&(identical(other.capacity, capacity) || other.capacity == capacity));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,name,goal,startDate,endDate,velocity,capacity);

@override
String toString() {
  return 'CreateUpdateSprintDto(projectId: $projectId, name: $name, goal: $goal, startDate: $startDate, endDate: $endDate, velocity: $velocity, capacity: $capacity)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateSprintDtoCopyWith<$Res> implements $CreateUpdateSprintDtoCopyWith<$Res> {
  factory _$CreateUpdateSprintDtoCopyWith(_CreateUpdateSprintDto value, $Res Function(_CreateUpdateSprintDto) _then) = __$CreateUpdateSprintDtoCopyWithImpl;
@override @useResult
$Res call({
 String projectId, String name, String? goal, DateTime startDate, DateTime endDate, int? velocity, int? capacity
});




}
/// @nodoc
class __$CreateUpdateSprintDtoCopyWithImpl<$Res>
    implements _$CreateUpdateSprintDtoCopyWith<$Res> {
  __$CreateUpdateSprintDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateSprintDto _self;
  final $Res Function(_CreateUpdateSprintDto) _then;

/// Create a copy of CreateUpdateSprintDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? projectId = null,Object? name = null,Object? goal = freezed,Object? startDate = null,Object? endDate = null,Object? velocity = freezed,Object? capacity = freezed,}) {
  return _then(_CreateUpdateSprintDto(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,goal: freezed == goal ? _self.goal : goal // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,velocity: freezed == velocity ? _self.velocity : velocity // ignore: cast_nullable_to_non_nullable
as int?,capacity: freezed == capacity ? _self.capacity : capacity // ignore: cast_nullable_to_non_nullable
as int?,
  ));
}


}

// dart format on
