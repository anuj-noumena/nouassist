// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_workflow_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$WorkflowStageDto {

 String get id; String get name; int get order; String get color; bool get isInitial; bool get isFinal; List<String> get allowedTransitions;
/// Create a copy of WorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$WorkflowStageDtoCopyWith<WorkflowStageDto> get copyWith => _$WorkflowStageDtoCopyWithImpl<WorkflowStageDto>(this as WorkflowStageDto, _$identity);

  /// Serializes this WorkflowStageDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is WorkflowStageDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.order, order) || other.order == order)&&(identical(other.color, color) || other.color == color)&&(identical(other.isInitial, isInitial) || other.isInitial == isInitial)&&(identical(other.isFinal, isFinal) || other.isFinal == isFinal)&&const DeepCollectionEquality().equals(other.allowedTransitions, allowedTransitions));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,order,color,isInitial,isFinal,const DeepCollectionEquality().hash(allowedTransitions));

@override
String toString() {
  return 'WorkflowStageDto(id: $id, name: $name, order: $order, color: $color, isInitial: $isInitial, isFinal: $isFinal, allowedTransitions: $allowedTransitions)';
}


}

/// @nodoc
abstract mixin class $WorkflowStageDtoCopyWith<$Res>  {
  factory $WorkflowStageDtoCopyWith(WorkflowStageDto value, $Res Function(WorkflowStageDto) _then) = _$WorkflowStageDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, int order, String color, bool isInitial, bool isFinal, List<String> allowedTransitions
});




}
/// @nodoc
class _$WorkflowStageDtoCopyWithImpl<$Res>
    implements $WorkflowStageDtoCopyWith<$Res> {
  _$WorkflowStageDtoCopyWithImpl(this._self, this._then);

  final WorkflowStageDto _self;
  final $Res Function(WorkflowStageDto) _then;

/// Create a copy of WorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? order = null,Object? color = null,Object? isInitial = null,Object? isFinal = null,Object? allowedTransitions = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isInitial: null == isInitial ? _self.isInitial : isInitial // ignore: cast_nullable_to_non_nullable
as bool,isFinal: null == isFinal ? _self.isFinal : isFinal // ignore: cast_nullable_to_non_nullable
as bool,allowedTransitions: null == allowedTransitions ? _self.allowedTransitions : allowedTransitions // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}

}


/// Adds pattern-matching-related methods to [WorkflowStageDto].
extension WorkflowStageDtoPatterns on WorkflowStageDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _WorkflowStageDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _WorkflowStageDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _WorkflowStageDto value)  $default,){
final _that = this;
switch (_that) {
case _WorkflowStageDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _WorkflowStageDto value)?  $default,){
final _that = this;
switch (_that) {
case _WorkflowStageDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  int order,  String color,  bool isInitial,  bool isFinal,  List<String> allowedTransitions)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _WorkflowStageDto() when $default != null:
return $default(_that.id,_that.name,_that.order,_that.color,_that.isInitial,_that.isFinal,_that.allowedTransitions);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  int order,  String color,  bool isInitial,  bool isFinal,  List<String> allowedTransitions)  $default,) {final _that = this;
switch (_that) {
case _WorkflowStageDto():
return $default(_that.id,_that.name,_that.order,_that.color,_that.isInitial,_that.isFinal,_that.allowedTransitions);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  int order,  String color,  bool isInitial,  bool isFinal,  List<String> allowedTransitions)?  $default,) {final _that = this;
switch (_that) {
case _WorkflowStageDto() when $default != null:
return $default(_that.id,_that.name,_that.order,_that.color,_that.isInitial,_that.isFinal,_that.allowedTransitions);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _WorkflowStageDto implements WorkflowStageDto {
  const _WorkflowStageDto({required this.id, required this.name, required this.order, required this.color, this.isInitial = false, this.isFinal = false, final  List<String> allowedTransitions = const []}): _allowedTransitions = allowedTransitions;
  factory _WorkflowStageDto.fromJson(Map<String, dynamic> json) => _$WorkflowStageDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  int order;
@override final  String color;
@override@JsonKey() final  bool isInitial;
@override@JsonKey() final  bool isFinal;
 final  List<String> _allowedTransitions;
@override@JsonKey() List<String> get allowedTransitions {
  if (_allowedTransitions is EqualUnmodifiableListView) return _allowedTransitions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_allowedTransitions);
}


/// Create a copy of WorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$WorkflowStageDtoCopyWith<_WorkflowStageDto> get copyWith => __$WorkflowStageDtoCopyWithImpl<_WorkflowStageDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$WorkflowStageDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _WorkflowStageDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.order, order) || other.order == order)&&(identical(other.color, color) || other.color == color)&&(identical(other.isInitial, isInitial) || other.isInitial == isInitial)&&(identical(other.isFinal, isFinal) || other.isFinal == isFinal)&&const DeepCollectionEquality().equals(other._allowedTransitions, _allowedTransitions));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,order,color,isInitial,isFinal,const DeepCollectionEquality().hash(_allowedTransitions));

@override
String toString() {
  return 'WorkflowStageDto(id: $id, name: $name, order: $order, color: $color, isInitial: $isInitial, isFinal: $isFinal, allowedTransitions: $allowedTransitions)';
}


}

/// @nodoc
abstract mixin class _$WorkflowStageDtoCopyWith<$Res> implements $WorkflowStageDtoCopyWith<$Res> {
  factory _$WorkflowStageDtoCopyWith(_WorkflowStageDto value, $Res Function(_WorkflowStageDto) _then) = __$WorkflowStageDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, int order, String color, bool isInitial, bool isFinal, List<String> allowedTransitions
});




}
/// @nodoc
class __$WorkflowStageDtoCopyWithImpl<$Res>
    implements _$WorkflowStageDtoCopyWith<$Res> {
  __$WorkflowStageDtoCopyWithImpl(this._self, this._then);

  final _WorkflowStageDto _self;
  final $Res Function(_WorkflowStageDto) _then;

/// Create a copy of WorkflowStageDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? order = null,Object? color = null,Object? isInitial = null,Object? isFinal = null,Object? allowedTransitions = null,}) {
  return _then(_WorkflowStageDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,isInitial: null == isInitial ? _self.isInitial : isInitial // ignore: cast_nullable_to_non_nullable
as bool,isFinal: null == isFinal ? _self.isFinal : isFinal // ignore: cast_nullable_to_non_nullable
as bool,allowedTransitions: null == allowedTransitions ? _self._allowedTransitions : allowedTransitions // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}


}


/// @nodoc
mixin _$ProjectWorkflowDto {

 String get id; String get name; String? get description; List<WorkflowStageDto> get stages; bool get isDefault; String? get projectType; DateTime get creationTime;
/// Create a copy of ProjectWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectWorkflowDtoCopyWith<ProjectWorkflowDto> get copyWith => _$ProjectWorkflowDtoCopyWithImpl<ProjectWorkflowDto>(this as ProjectWorkflowDto, _$identity);

  /// Serializes this ProjectWorkflowDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectWorkflowDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other.stages, stages)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.projectType, projectType) || other.projectType == projectType)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(stages),isDefault,projectType,creationTime);

@override
String toString() {
  return 'ProjectWorkflowDto(id: $id, name: $name, description: $description, stages: $stages, isDefault: $isDefault, projectType: $projectType, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $ProjectWorkflowDtoCopyWith<$Res>  {
  factory $ProjectWorkflowDtoCopyWith(ProjectWorkflowDto value, $Res Function(ProjectWorkflowDto) _then) = _$ProjectWorkflowDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String? description, List<WorkflowStageDto> stages, bool isDefault, String? projectType, DateTime creationTime
});




}
/// @nodoc
class _$ProjectWorkflowDtoCopyWithImpl<$Res>
    implements $ProjectWorkflowDtoCopyWith<$Res> {
  _$ProjectWorkflowDtoCopyWithImpl(this._self, this._then);

  final ProjectWorkflowDto _self;
  final $Res Function(ProjectWorkflowDto) _then;

/// Create a copy of ProjectWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? stages = null,Object? isDefault = null,Object? projectType = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self.stages : stages // ignore: cast_nullable_to_non_nullable
as List<WorkflowStageDto>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,projectType: freezed == projectType ? _self.projectType : projectType // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectWorkflowDto].
extension ProjectWorkflowDtoPatterns on ProjectWorkflowDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectWorkflowDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectWorkflowDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectWorkflowDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectWorkflowDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectWorkflowDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectWorkflowDto():
return $default(_that.id,_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectWorkflowDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectWorkflowDto implements ProjectWorkflowDto {
  const _ProjectWorkflowDto({required this.id, required this.name, this.description, required final  List<WorkflowStageDto> stages, this.isDefault = false, this.projectType, required this.creationTime}): _stages = stages;
  factory _ProjectWorkflowDto.fromJson(Map<String, dynamic> json) => _$ProjectWorkflowDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String? description;
 final  List<WorkflowStageDto> _stages;
@override List<WorkflowStageDto> get stages {
  if (_stages is EqualUnmodifiableListView) return _stages;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_stages);
}

@override@JsonKey() final  bool isDefault;
@override final  String? projectType;
@override final  DateTime creationTime;

/// Create a copy of ProjectWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectWorkflowDtoCopyWith<_ProjectWorkflowDto> get copyWith => __$ProjectWorkflowDtoCopyWithImpl<_ProjectWorkflowDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectWorkflowDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectWorkflowDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other._stages, _stages)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.projectType, projectType) || other.projectType == projectType)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,const DeepCollectionEquality().hash(_stages),isDefault,projectType,creationTime);

@override
String toString() {
  return 'ProjectWorkflowDto(id: $id, name: $name, description: $description, stages: $stages, isDefault: $isDefault, projectType: $projectType, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectWorkflowDtoCopyWith<$Res> implements $ProjectWorkflowDtoCopyWith<$Res> {
  factory _$ProjectWorkflowDtoCopyWith(_ProjectWorkflowDto value, $Res Function(_ProjectWorkflowDto) _then) = __$ProjectWorkflowDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String? description, List<WorkflowStageDto> stages, bool isDefault, String? projectType, DateTime creationTime
});




}
/// @nodoc
class __$ProjectWorkflowDtoCopyWithImpl<$Res>
    implements _$ProjectWorkflowDtoCopyWith<$Res> {
  __$ProjectWorkflowDtoCopyWithImpl(this._self, this._then);

  final _ProjectWorkflowDto _self;
  final $Res Function(_ProjectWorkflowDto) _then;

/// Create a copy of ProjectWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? stages = null,Object? isDefault = null,Object? projectType = freezed,Object? creationTime = null,}) {
  return _then(_ProjectWorkflowDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self._stages : stages // ignore: cast_nullable_to_non_nullable
as List<WorkflowStageDto>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,projectType: freezed == projectType ? _self.projectType : projectType // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateWorkflowDto {

 String get name; String? get description; List<WorkflowStageDto> get stages; bool get isDefault; String? get projectType;
/// Create a copy of CreateUpdateWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateWorkflowDtoCopyWith<CreateUpdateWorkflowDto> get copyWith => _$CreateUpdateWorkflowDtoCopyWithImpl<CreateUpdateWorkflowDto>(this as CreateUpdateWorkflowDto, _$identity);

  /// Serializes this CreateUpdateWorkflowDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateWorkflowDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other.stages, stages)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.projectType, projectType) || other.projectType == projectType));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,const DeepCollectionEquality().hash(stages),isDefault,projectType);

@override
String toString() {
  return 'CreateUpdateWorkflowDto(name: $name, description: $description, stages: $stages, isDefault: $isDefault, projectType: $projectType)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateWorkflowDtoCopyWith<$Res>  {
  factory $CreateUpdateWorkflowDtoCopyWith(CreateUpdateWorkflowDto value, $Res Function(CreateUpdateWorkflowDto) _then) = _$CreateUpdateWorkflowDtoCopyWithImpl;
@useResult
$Res call({
 String name, String? description, List<WorkflowStageDto> stages, bool isDefault, String? projectType
});




}
/// @nodoc
class _$CreateUpdateWorkflowDtoCopyWithImpl<$Res>
    implements $CreateUpdateWorkflowDtoCopyWith<$Res> {
  _$CreateUpdateWorkflowDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateWorkflowDto _self;
  final $Res Function(CreateUpdateWorkflowDto) _then;

/// Create a copy of CreateUpdateWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? description = freezed,Object? stages = null,Object? isDefault = null,Object? projectType = freezed,}) {
  return _then(_self.copyWith(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self.stages : stages // ignore: cast_nullable_to_non_nullable
as List<WorkflowStageDto>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,projectType: freezed == projectType ? _self.projectType : projectType // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateWorkflowDto].
extension CreateUpdateWorkflowDtoPatterns on CreateUpdateWorkflowDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateWorkflowDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateWorkflowDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateWorkflowDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto() when $default != null:
return $default(_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto():
return $default(_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String? description,  List<WorkflowStageDto> stages,  bool isDefault,  String? projectType)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWorkflowDto() when $default != null:
return $default(_that.name,_that.description,_that.stages,_that.isDefault,_that.projectType);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateWorkflowDto implements CreateUpdateWorkflowDto {
  const _CreateUpdateWorkflowDto({required this.name, this.description, required final  List<WorkflowStageDto> stages, this.isDefault = false, this.projectType}): _stages = stages;
  factory _CreateUpdateWorkflowDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateWorkflowDtoFromJson(json);

@override final  String name;
@override final  String? description;
 final  List<WorkflowStageDto> _stages;
@override List<WorkflowStageDto> get stages {
  if (_stages is EqualUnmodifiableListView) return _stages;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_stages);
}

@override@JsonKey() final  bool isDefault;
@override final  String? projectType;

/// Create a copy of CreateUpdateWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateWorkflowDtoCopyWith<_CreateUpdateWorkflowDto> get copyWith => __$CreateUpdateWorkflowDtoCopyWithImpl<_CreateUpdateWorkflowDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateWorkflowDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateWorkflowDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&const DeepCollectionEquality().equals(other._stages, _stages)&&(identical(other.isDefault, isDefault) || other.isDefault == isDefault)&&(identical(other.projectType, projectType) || other.projectType == projectType));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,const DeepCollectionEquality().hash(_stages),isDefault,projectType);

@override
String toString() {
  return 'CreateUpdateWorkflowDto(name: $name, description: $description, stages: $stages, isDefault: $isDefault, projectType: $projectType)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateWorkflowDtoCopyWith<$Res> implements $CreateUpdateWorkflowDtoCopyWith<$Res> {
  factory _$CreateUpdateWorkflowDtoCopyWith(_CreateUpdateWorkflowDto value, $Res Function(_CreateUpdateWorkflowDto) _then) = __$CreateUpdateWorkflowDtoCopyWithImpl;
@override @useResult
$Res call({
 String name, String? description, List<WorkflowStageDto> stages, bool isDefault, String? projectType
});




}
/// @nodoc
class __$CreateUpdateWorkflowDtoCopyWithImpl<$Res>
    implements _$CreateUpdateWorkflowDtoCopyWith<$Res> {
  __$CreateUpdateWorkflowDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateWorkflowDto _self;
  final $Res Function(_CreateUpdateWorkflowDto) _then;

/// Create a copy of CreateUpdateWorkflowDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? description = freezed,Object? stages = null,Object? isDefault = null,Object? projectType = freezed,}) {
  return _then(_CreateUpdateWorkflowDto(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,stages: null == stages ? _self._stages : stages // ignore: cast_nullable_to_non_nullable
as List<WorkflowStageDto>,isDefault: null == isDefault ? _self.isDefault : isDefault // ignore: cast_nullable_to_non_nullable
as bool,projectType: freezed == projectType ? _self.projectType : projectType // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
