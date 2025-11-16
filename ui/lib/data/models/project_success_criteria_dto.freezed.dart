// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_success_criteria_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectSuccessCriteriaDto {

 String get id; String get projectId; String get criterion; String? get description; MeasurementType get measurementType; String? get targetValue; String? get actualValue; String? get unit; SuccessCriteriaStatus get status; double get weight; String? get verificationMethod; DateTime get creationTime; DateTime? get achievementTime;
/// Create a copy of ProjectSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectSuccessCriteriaDtoCopyWith<ProjectSuccessCriteriaDto> get copyWith => _$ProjectSuccessCriteriaDtoCopyWithImpl<ProjectSuccessCriteriaDto>(this as ProjectSuccessCriteriaDto, _$identity);

  /// Serializes this ProjectSuccessCriteriaDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectSuccessCriteriaDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.description, description) || other.description == description)&&(identical(other.measurementType, measurementType) || other.measurementType == measurementType)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.actualValue, actualValue) || other.actualValue == actualValue)&&(identical(other.unit, unit) || other.unit == unit)&&(identical(other.status, status) || other.status == status)&&(identical(other.weight, weight) || other.weight == weight)&&(identical(other.verificationMethod, verificationMethod) || other.verificationMethod == verificationMethod)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.achievementTime, achievementTime) || other.achievementTime == achievementTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,criterion,description,measurementType,targetValue,actualValue,unit,status,weight,verificationMethod,creationTime,achievementTime);

@override
String toString() {
  return 'ProjectSuccessCriteriaDto(id: $id, projectId: $projectId, criterion: $criterion, description: $description, measurementType: $measurementType, targetValue: $targetValue, actualValue: $actualValue, unit: $unit, status: $status, weight: $weight, verificationMethod: $verificationMethod, creationTime: $creationTime, achievementTime: $achievementTime)';
}


}

/// @nodoc
abstract mixin class $ProjectSuccessCriteriaDtoCopyWith<$Res>  {
  factory $ProjectSuccessCriteriaDtoCopyWith(ProjectSuccessCriteriaDto value, $Res Function(ProjectSuccessCriteriaDto) _then) = _$ProjectSuccessCriteriaDtoCopyWithImpl;
@useResult
$Res call({
 String id, String projectId, String criterion, String? description, MeasurementType measurementType, String? targetValue, String? actualValue, String? unit, SuccessCriteriaStatus status, double weight, String? verificationMethod, DateTime creationTime, DateTime? achievementTime
});




}
/// @nodoc
class _$ProjectSuccessCriteriaDtoCopyWithImpl<$Res>
    implements $ProjectSuccessCriteriaDtoCopyWith<$Res> {
  _$ProjectSuccessCriteriaDtoCopyWithImpl(this._self, this._then);

  final ProjectSuccessCriteriaDto _self;
  final $Res Function(ProjectSuccessCriteriaDto) _then;

/// Create a copy of ProjectSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? projectId = null,Object? criterion = null,Object? description = freezed,Object? measurementType = null,Object? targetValue = freezed,Object? actualValue = freezed,Object? unit = freezed,Object? status = null,Object? weight = null,Object? verificationMethod = freezed,Object? creationTime = null,Object? achievementTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,measurementType: null == measurementType ? _self.measurementType : measurementType // ignore: cast_nullable_to_non_nullable
as MeasurementType,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as String?,actualValue: freezed == actualValue ? _self.actualValue : actualValue // ignore: cast_nullable_to_non_nullable
as String?,unit: freezed == unit ? _self.unit : unit // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SuccessCriteriaStatus,weight: null == weight ? _self.weight : weight // ignore: cast_nullable_to_non_nullable
as double,verificationMethod: freezed == verificationMethod ? _self.verificationMethod : verificationMethod // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,achievementTime: freezed == achievementTime ? _self.achievementTime : achievementTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectSuccessCriteriaDto].
extension ProjectSuccessCriteriaDtoPatterns on ProjectSuccessCriteriaDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectSuccessCriteriaDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectSuccessCriteriaDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectSuccessCriteriaDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod,  DateTime creationTime,  DateTime? achievementTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto() when $default != null:
return $default(_that.id,_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod,_that.creationTime,_that.achievementTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod,  DateTime creationTime,  DateTime? achievementTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto():
return $default(_that.id,_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod,_that.creationTime,_that.achievementTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod,  DateTime creationTime,  DateTime? achievementTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectSuccessCriteriaDto() when $default != null:
return $default(_that.id,_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod,_that.creationTime,_that.achievementTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectSuccessCriteriaDto implements ProjectSuccessCriteriaDto {
  const _ProjectSuccessCriteriaDto({required this.id, required this.projectId, required this.criterion, this.description, required this.measurementType, this.targetValue, this.actualValue, this.unit, required this.status, required this.weight, this.verificationMethod, required this.creationTime, this.achievementTime});
  factory _ProjectSuccessCriteriaDto.fromJson(Map<String, dynamic> json) => _$ProjectSuccessCriteriaDtoFromJson(json);

@override final  String id;
@override final  String projectId;
@override final  String criterion;
@override final  String? description;
@override final  MeasurementType measurementType;
@override final  String? targetValue;
@override final  String? actualValue;
@override final  String? unit;
@override final  SuccessCriteriaStatus status;
@override final  double weight;
@override final  String? verificationMethod;
@override final  DateTime creationTime;
@override final  DateTime? achievementTime;

/// Create a copy of ProjectSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectSuccessCriteriaDtoCopyWith<_ProjectSuccessCriteriaDto> get copyWith => __$ProjectSuccessCriteriaDtoCopyWithImpl<_ProjectSuccessCriteriaDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectSuccessCriteriaDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectSuccessCriteriaDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.description, description) || other.description == description)&&(identical(other.measurementType, measurementType) || other.measurementType == measurementType)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.actualValue, actualValue) || other.actualValue == actualValue)&&(identical(other.unit, unit) || other.unit == unit)&&(identical(other.status, status) || other.status == status)&&(identical(other.weight, weight) || other.weight == weight)&&(identical(other.verificationMethod, verificationMethod) || other.verificationMethod == verificationMethod)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.achievementTime, achievementTime) || other.achievementTime == achievementTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,criterion,description,measurementType,targetValue,actualValue,unit,status,weight,verificationMethod,creationTime,achievementTime);

@override
String toString() {
  return 'ProjectSuccessCriteriaDto(id: $id, projectId: $projectId, criterion: $criterion, description: $description, measurementType: $measurementType, targetValue: $targetValue, actualValue: $actualValue, unit: $unit, status: $status, weight: $weight, verificationMethod: $verificationMethod, creationTime: $creationTime, achievementTime: $achievementTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectSuccessCriteriaDtoCopyWith<$Res> implements $ProjectSuccessCriteriaDtoCopyWith<$Res> {
  factory _$ProjectSuccessCriteriaDtoCopyWith(_ProjectSuccessCriteriaDto value, $Res Function(_ProjectSuccessCriteriaDto) _then) = __$ProjectSuccessCriteriaDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String projectId, String criterion, String? description, MeasurementType measurementType, String? targetValue, String? actualValue, String? unit, SuccessCriteriaStatus status, double weight, String? verificationMethod, DateTime creationTime, DateTime? achievementTime
});




}
/// @nodoc
class __$ProjectSuccessCriteriaDtoCopyWithImpl<$Res>
    implements _$ProjectSuccessCriteriaDtoCopyWith<$Res> {
  __$ProjectSuccessCriteriaDtoCopyWithImpl(this._self, this._then);

  final _ProjectSuccessCriteriaDto _self;
  final $Res Function(_ProjectSuccessCriteriaDto) _then;

/// Create a copy of ProjectSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? projectId = null,Object? criterion = null,Object? description = freezed,Object? measurementType = null,Object? targetValue = freezed,Object? actualValue = freezed,Object? unit = freezed,Object? status = null,Object? weight = null,Object? verificationMethod = freezed,Object? creationTime = null,Object? achievementTime = freezed,}) {
  return _then(_ProjectSuccessCriteriaDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,measurementType: null == measurementType ? _self.measurementType : measurementType // ignore: cast_nullable_to_non_nullable
as MeasurementType,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as String?,actualValue: freezed == actualValue ? _self.actualValue : actualValue // ignore: cast_nullable_to_non_nullable
as String?,unit: freezed == unit ? _self.unit : unit // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SuccessCriteriaStatus,weight: null == weight ? _self.weight : weight // ignore: cast_nullable_to_non_nullable
as double,verificationMethod: freezed == verificationMethod ? _self.verificationMethod : verificationMethod // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,achievementTime: freezed == achievementTime ? _self.achievementTime : achievementTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateSuccessCriteriaDto {

 String get projectId; String get criterion; String? get description; MeasurementType get measurementType; String? get targetValue; String? get actualValue; String? get unit; SuccessCriteriaStatus get status; double get weight; String? get verificationMethod;
/// Create a copy of CreateUpdateSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateSuccessCriteriaDtoCopyWith<CreateUpdateSuccessCriteriaDto> get copyWith => _$CreateUpdateSuccessCriteriaDtoCopyWithImpl<CreateUpdateSuccessCriteriaDto>(this as CreateUpdateSuccessCriteriaDto, _$identity);

  /// Serializes this CreateUpdateSuccessCriteriaDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateSuccessCriteriaDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.description, description) || other.description == description)&&(identical(other.measurementType, measurementType) || other.measurementType == measurementType)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.actualValue, actualValue) || other.actualValue == actualValue)&&(identical(other.unit, unit) || other.unit == unit)&&(identical(other.status, status) || other.status == status)&&(identical(other.weight, weight) || other.weight == weight)&&(identical(other.verificationMethod, verificationMethod) || other.verificationMethod == verificationMethod));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,criterion,description,measurementType,targetValue,actualValue,unit,status,weight,verificationMethod);

@override
String toString() {
  return 'CreateUpdateSuccessCriteriaDto(projectId: $projectId, criterion: $criterion, description: $description, measurementType: $measurementType, targetValue: $targetValue, actualValue: $actualValue, unit: $unit, status: $status, weight: $weight, verificationMethod: $verificationMethod)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateSuccessCriteriaDtoCopyWith<$Res>  {
  factory $CreateUpdateSuccessCriteriaDtoCopyWith(CreateUpdateSuccessCriteriaDto value, $Res Function(CreateUpdateSuccessCriteriaDto) _then) = _$CreateUpdateSuccessCriteriaDtoCopyWithImpl;
@useResult
$Res call({
 String projectId, String criterion, String? description, MeasurementType measurementType, String? targetValue, String? actualValue, String? unit, SuccessCriteriaStatus status, double weight, String? verificationMethod
});




}
/// @nodoc
class _$CreateUpdateSuccessCriteriaDtoCopyWithImpl<$Res>
    implements $CreateUpdateSuccessCriteriaDtoCopyWith<$Res> {
  _$CreateUpdateSuccessCriteriaDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateSuccessCriteriaDto _self;
  final $Res Function(CreateUpdateSuccessCriteriaDto) _then;

/// Create a copy of CreateUpdateSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? projectId = null,Object? criterion = null,Object? description = freezed,Object? measurementType = null,Object? targetValue = freezed,Object? actualValue = freezed,Object? unit = freezed,Object? status = null,Object? weight = null,Object? verificationMethod = freezed,}) {
  return _then(_self.copyWith(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,measurementType: null == measurementType ? _self.measurementType : measurementType // ignore: cast_nullable_to_non_nullable
as MeasurementType,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as String?,actualValue: freezed == actualValue ? _self.actualValue : actualValue // ignore: cast_nullable_to_non_nullable
as String?,unit: freezed == unit ? _self.unit : unit // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SuccessCriteriaStatus,weight: null == weight ? _self.weight : weight // ignore: cast_nullable_to_non_nullable
as double,verificationMethod: freezed == verificationMethod ? _self.verificationMethod : verificationMethod // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateSuccessCriteriaDto].
extension CreateUpdateSuccessCriteriaDtoPatterns on CreateUpdateSuccessCriteriaDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateSuccessCriteriaDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateSuccessCriteriaDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateSuccessCriteriaDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto() when $default != null:
return $default(_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto():
return $default(_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String projectId,  String criterion,  String? description,  MeasurementType measurementType,  String? targetValue,  String? actualValue,  String? unit,  SuccessCriteriaStatus status,  double weight,  String? verificationMethod)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSuccessCriteriaDto() when $default != null:
return $default(_that.projectId,_that.criterion,_that.description,_that.measurementType,_that.targetValue,_that.actualValue,_that.unit,_that.status,_that.weight,_that.verificationMethod);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateSuccessCriteriaDto implements CreateUpdateSuccessCriteriaDto {
  const _CreateUpdateSuccessCriteriaDto({required this.projectId, required this.criterion, this.description, required this.measurementType, this.targetValue, this.actualValue, this.unit, this.status = SuccessCriteriaStatus.notStarted, this.weight = 1.0, this.verificationMethod});
  factory _CreateUpdateSuccessCriteriaDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateSuccessCriteriaDtoFromJson(json);

@override final  String projectId;
@override final  String criterion;
@override final  String? description;
@override final  MeasurementType measurementType;
@override final  String? targetValue;
@override final  String? actualValue;
@override final  String? unit;
@override@JsonKey() final  SuccessCriteriaStatus status;
@override@JsonKey() final  double weight;
@override final  String? verificationMethod;

/// Create a copy of CreateUpdateSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateSuccessCriteriaDtoCopyWith<_CreateUpdateSuccessCriteriaDto> get copyWith => __$CreateUpdateSuccessCriteriaDtoCopyWithImpl<_CreateUpdateSuccessCriteriaDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateSuccessCriteriaDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateSuccessCriteriaDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.criterion, criterion) || other.criterion == criterion)&&(identical(other.description, description) || other.description == description)&&(identical(other.measurementType, measurementType) || other.measurementType == measurementType)&&(identical(other.targetValue, targetValue) || other.targetValue == targetValue)&&(identical(other.actualValue, actualValue) || other.actualValue == actualValue)&&(identical(other.unit, unit) || other.unit == unit)&&(identical(other.status, status) || other.status == status)&&(identical(other.weight, weight) || other.weight == weight)&&(identical(other.verificationMethod, verificationMethod) || other.verificationMethod == verificationMethod));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,criterion,description,measurementType,targetValue,actualValue,unit,status,weight,verificationMethod);

@override
String toString() {
  return 'CreateUpdateSuccessCriteriaDto(projectId: $projectId, criterion: $criterion, description: $description, measurementType: $measurementType, targetValue: $targetValue, actualValue: $actualValue, unit: $unit, status: $status, weight: $weight, verificationMethod: $verificationMethod)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateSuccessCriteriaDtoCopyWith<$Res> implements $CreateUpdateSuccessCriteriaDtoCopyWith<$Res> {
  factory _$CreateUpdateSuccessCriteriaDtoCopyWith(_CreateUpdateSuccessCriteriaDto value, $Res Function(_CreateUpdateSuccessCriteriaDto) _then) = __$CreateUpdateSuccessCriteriaDtoCopyWithImpl;
@override @useResult
$Res call({
 String projectId, String criterion, String? description, MeasurementType measurementType, String? targetValue, String? actualValue, String? unit, SuccessCriteriaStatus status, double weight, String? verificationMethod
});




}
/// @nodoc
class __$CreateUpdateSuccessCriteriaDtoCopyWithImpl<$Res>
    implements _$CreateUpdateSuccessCriteriaDtoCopyWith<$Res> {
  __$CreateUpdateSuccessCriteriaDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateSuccessCriteriaDto _self;
  final $Res Function(_CreateUpdateSuccessCriteriaDto) _then;

/// Create a copy of CreateUpdateSuccessCriteriaDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? projectId = null,Object? criterion = null,Object? description = freezed,Object? measurementType = null,Object? targetValue = freezed,Object? actualValue = freezed,Object? unit = freezed,Object? status = null,Object? weight = null,Object? verificationMethod = freezed,}) {
  return _then(_CreateUpdateSuccessCriteriaDto(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,criterion: null == criterion ? _self.criterion : criterion // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,measurementType: null == measurementType ? _self.measurementType : measurementType // ignore: cast_nullable_to_non_nullable
as MeasurementType,targetValue: freezed == targetValue ? _self.targetValue : targetValue // ignore: cast_nullable_to_non_nullable
as String?,actualValue: freezed == actualValue ? _self.actualValue : actualValue // ignore: cast_nullable_to_non_nullable
as String?,unit: freezed == unit ? _self.unit : unit // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as SuccessCriteriaStatus,weight: null == weight ? _self.weight : weight // ignore: cast_nullable_to_non_nullable
as double,verificationMethod: freezed == verificationMethod ? _self.verificationMethod : verificationMethod // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
