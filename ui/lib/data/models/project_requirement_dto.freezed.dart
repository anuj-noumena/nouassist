// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_requirement_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectRequirementDto {

 String get id; String get projectId; String get title; String get description; RequirementType get requirementType; RequirementPriority get priority; RequirementStatus get status; String? get acceptanceCriteria; double? get estimatedEffort; String? get parentRequirementId; List<String> get tags; String get createdBy; String? get approvedBy; DateTime get creationTime; DateTime? get approvalTime;
/// Create a copy of ProjectRequirementDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectRequirementDtoCopyWith<ProjectRequirementDto> get copyWith => _$ProjectRequirementDtoCopyWithImpl<ProjectRequirementDto>(this as ProjectRequirementDto, _$identity);

  /// Serializes this ProjectRequirementDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectRequirementDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.requirementType, requirementType) || other.requirementType == requirementType)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.acceptanceCriteria, acceptanceCriteria) || other.acceptanceCriteria == acceptanceCriteria)&&(identical(other.estimatedEffort, estimatedEffort) || other.estimatedEffort == estimatedEffort)&&(identical(other.parentRequirementId, parentRequirementId) || other.parentRequirementId == parentRequirementId)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.createdBy, createdBy) || other.createdBy == createdBy)&&(identical(other.approvedBy, approvedBy) || other.approvedBy == approvedBy)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.approvalTime, approvalTime) || other.approvalTime == approvalTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,title,description,requirementType,priority,status,acceptanceCriteria,estimatedEffort,parentRequirementId,const DeepCollectionEquality().hash(tags),createdBy,approvedBy,creationTime,approvalTime);

@override
String toString() {
  return 'ProjectRequirementDto(id: $id, projectId: $projectId, title: $title, description: $description, requirementType: $requirementType, priority: $priority, status: $status, acceptanceCriteria: $acceptanceCriteria, estimatedEffort: $estimatedEffort, parentRequirementId: $parentRequirementId, tags: $tags, createdBy: $createdBy, approvedBy: $approvedBy, creationTime: $creationTime, approvalTime: $approvalTime)';
}


}

/// @nodoc
abstract mixin class $ProjectRequirementDtoCopyWith<$Res>  {
  factory $ProjectRequirementDtoCopyWith(ProjectRequirementDto value, $Res Function(ProjectRequirementDto) _then) = _$ProjectRequirementDtoCopyWithImpl;
@useResult
$Res call({
 String id, String projectId, String title, String description, RequirementType requirementType, RequirementPriority priority, RequirementStatus status, String? acceptanceCriteria, double? estimatedEffort, String? parentRequirementId, List<String> tags, String createdBy, String? approvedBy, DateTime creationTime, DateTime? approvalTime
});




}
/// @nodoc
class _$ProjectRequirementDtoCopyWithImpl<$Res>
    implements $ProjectRequirementDtoCopyWith<$Res> {
  _$ProjectRequirementDtoCopyWithImpl(this._self, this._then);

  final ProjectRequirementDto _self;
  final $Res Function(ProjectRequirementDto) _then;

/// Create a copy of ProjectRequirementDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? projectId = null,Object? title = null,Object? description = null,Object? requirementType = null,Object? priority = null,Object? status = null,Object? acceptanceCriteria = freezed,Object? estimatedEffort = freezed,Object? parentRequirementId = freezed,Object? tags = null,Object? createdBy = null,Object? approvedBy = freezed,Object? creationTime = null,Object? approvalTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,requirementType: null == requirementType ? _self.requirementType : requirementType // ignore: cast_nullable_to_non_nullable
as RequirementType,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as RequirementPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as RequirementStatus,acceptanceCriteria: freezed == acceptanceCriteria ? _self.acceptanceCriteria : acceptanceCriteria // ignore: cast_nullable_to_non_nullable
as String?,estimatedEffort: freezed == estimatedEffort ? _self.estimatedEffort : estimatedEffort // ignore: cast_nullable_to_non_nullable
as double?,parentRequirementId: freezed == parentRequirementId ? _self.parentRequirementId : parentRequirementId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,createdBy: null == createdBy ? _self.createdBy : createdBy // ignore: cast_nullable_to_non_nullable
as String,approvedBy: freezed == approvedBy ? _self.approvedBy : approvedBy // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,approvalTime: freezed == approvalTime ? _self.approvalTime : approvalTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectRequirementDto].
extension ProjectRequirementDtoPatterns on ProjectRequirementDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectRequirementDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectRequirementDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectRequirementDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectRequirementDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectRequirementDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectRequirementDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags,  String createdBy,  String? approvedBy,  DateTime creationTime,  DateTime? approvalTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectRequirementDto() when $default != null:
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags,_that.createdBy,_that.approvedBy,_that.creationTime,_that.approvalTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags,  String createdBy,  String? approvedBy,  DateTime creationTime,  DateTime? approvalTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectRequirementDto():
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags,_that.createdBy,_that.approvedBy,_that.creationTime,_that.approvalTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags,  String createdBy,  String? approvedBy,  DateTime creationTime,  DateTime? approvalTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectRequirementDto() when $default != null:
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags,_that.createdBy,_that.approvedBy,_that.creationTime,_that.approvalTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectRequirementDto implements ProjectRequirementDto {
  const _ProjectRequirementDto({required this.id, required this.projectId, required this.title, required this.description, required this.requirementType, required this.priority, required this.status, this.acceptanceCriteria, this.estimatedEffort, this.parentRequirementId, final  List<String> tags = const [], required this.createdBy, this.approvedBy, required this.creationTime, this.approvalTime}): _tags = tags;
  factory _ProjectRequirementDto.fromJson(Map<String, dynamic> json) => _$ProjectRequirementDtoFromJson(json);

@override final  String id;
@override final  String projectId;
@override final  String title;
@override final  String description;
@override final  RequirementType requirementType;
@override final  RequirementPriority priority;
@override final  RequirementStatus status;
@override final  String? acceptanceCriteria;
@override final  double? estimatedEffort;
@override final  String? parentRequirementId;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String createdBy;
@override final  String? approvedBy;
@override final  DateTime creationTime;
@override final  DateTime? approvalTime;

/// Create a copy of ProjectRequirementDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectRequirementDtoCopyWith<_ProjectRequirementDto> get copyWith => __$ProjectRequirementDtoCopyWithImpl<_ProjectRequirementDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectRequirementDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectRequirementDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.requirementType, requirementType) || other.requirementType == requirementType)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.acceptanceCriteria, acceptanceCriteria) || other.acceptanceCriteria == acceptanceCriteria)&&(identical(other.estimatedEffort, estimatedEffort) || other.estimatedEffort == estimatedEffort)&&(identical(other.parentRequirementId, parentRequirementId) || other.parentRequirementId == parentRequirementId)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.createdBy, createdBy) || other.createdBy == createdBy)&&(identical(other.approvedBy, approvedBy) || other.approvedBy == approvedBy)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.approvalTime, approvalTime) || other.approvalTime == approvalTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,title,description,requirementType,priority,status,acceptanceCriteria,estimatedEffort,parentRequirementId,const DeepCollectionEquality().hash(_tags),createdBy,approvedBy,creationTime,approvalTime);

@override
String toString() {
  return 'ProjectRequirementDto(id: $id, projectId: $projectId, title: $title, description: $description, requirementType: $requirementType, priority: $priority, status: $status, acceptanceCriteria: $acceptanceCriteria, estimatedEffort: $estimatedEffort, parentRequirementId: $parentRequirementId, tags: $tags, createdBy: $createdBy, approvedBy: $approvedBy, creationTime: $creationTime, approvalTime: $approvalTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectRequirementDtoCopyWith<$Res> implements $ProjectRequirementDtoCopyWith<$Res> {
  factory _$ProjectRequirementDtoCopyWith(_ProjectRequirementDto value, $Res Function(_ProjectRequirementDto) _then) = __$ProjectRequirementDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String projectId, String title, String description, RequirementType requirementType, RequirementPriority priority, RequirementStatus status, String? acceptanceCriteria, double? estimatedEffort, String? parentRequirementId, List<String> tags, String createdBy, String? approvedBy, DateTime creationTime, DateTime? approvalTime
});




}
/// @nodoc
class __$ProjectRequirementDtoCopyWithImpl<$Res>
    implements _$ProjectRequirementDtoCopyWith<$Res> {
  __$ProjectRequirementDtoCopyWithImpl(this._self, this._then);

  final _ProjectRequirementDto _self;
  final $Res Function(_ProjectRequirementDto) _then;

/// Create a copy of ProjectRequirementDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? projectId = null,Object? title = null,Object? description = null,Object? requirementType = null,Object? priority = null,Object? status = null,Object? acceptanceCriteria = freezed,Object? estimatedEffort = freezed,Object? parentRequirementId = freezed,Object? tags = null,Object? createdBy = null,Object? approvedBy = freezed,Object? creationTime = null,Object? approvalTime = freezed,}) {
  return _then(_ProjectRequirementDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,requirementType: null == requirementType ? _self.requirementType : requirementType // ignore: cast_nullable_to_non_nullable
as RequirementType,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as RequirementPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as RequirementStatus,acceptanceCriteria: freezed == acceptanceCriteria ? _self.acceptanceCriteria : acceptanceCriteria // ignore: cast_nullable_to_non_nullable
as String?,estimatedEffort: freezed == estimatedEffort ? _self.estimatedEffort : estimatedEffort // ignore: cast_nullable_to_non_nullable
as double?,parentRequirementId: freezed == parentRequirementId ? _self.parentRequirementId : parentRequirementId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,createdBy: null == createdBy ? _self.createdBy : createdBy // ignore: cast_nullable_to_non_nullable
as String,approvedBy: freezed == approvedBy ? _self.approvedBy : approvedBy // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,approvalTime: freezed == approvalTime ? _self.approvalTime : approvalTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateRequirementDto {

 String get projectId; String get title; String get description; RequirementType get requirementType; RequirementPriority get priority; RequirementStatus get status; String? get acceptanceCriteria; double? get estimatedEffort; String? get parentRequirementId; List<String> get tags;
/// Create a copy of CreateUpdateRequirementDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateRequirementDtoCopyWith<CreateUpdateRequirementDto> get copyWith => _$CreateUpdateRequirementDtoCopyWithImpl<CreateUpdateRequirementDto>(this as CreateUpdateRequirementDto, _$identity);

  /// Serializes this CreateUpdateRequirementDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateRequirementDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.requirementType, requirementType) || other.requirementType == requirementType)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.acceptanceCriteria, acceptanceCriteria) || other.acceptanceCriteria == acceptanceCriteria)&&(identical(other.estimatedEffort, estimatedEffort) || other.estimatedEffort == estimatedEffort)&&(identical(other.parentRequirementId, parentRequirementId) || other.parentRequirementId == parentRequirementId)&&const DeepCollectionEquality().equals(other.tags, tags));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,title,description,requirementType,priority,status,acceptanceCriteria,estimatedEffort,parentRequirementId,const DeepCollectionEquality().hash(tags));

@override
String toString() {
  return 'CreateUpdateRequirementDto(projectId: $projectId, title: $title, description: $description, requirementType: $requirementType, priority: $priority, status: $status, acceptanceCriteria: $acceptanceCriteria, estimatedEffort: $estimatedEffort, parentRequirementId: $parentRequirementId, tags: $tags)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateRequirementDtoCopyWith<$Res>  {
  factory $CreateUpdateRequirementDtoCopyWith(CreateUpdateRequirementDto value, $Res Function(CreateUpdateRequirementDto) _then) = _$CreateUpdateRequirementDtoCopyWithImpl;
@useResult
$Res call({
 String projectId, String title, String description, RequirementType requirementType, RequirementPriority priority, RequirementStatus status, String? acceptanceCriteria, double? estimatedEffort, String? parentRequirementId, List<String> tags
});




}
/// @nodoc
class _$CreateUpdateRequirementDtoCopyWithImpl<$Res>
    implements $CreateUpdateRequirementDtoCopyWith<$Res> {
  _$CreateUpdateRequirementDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateRequirementDto _self;
  final $Res Function(CreateUpdateRequirementDto) _then;

/// Create a copy of CreateUpdateRequirementDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? projectId = null,Object? title = null,Object? description = null,Object? requirementType = null,Object? priority = null,Object? status = null,Object? acceptanceCriteria = freezed,Object? estimatedEffort = freezed,Object? parentRequirementId = freezed,Object? tags = null,}) {
  return _then(_self.copyWith(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,requirementType: null == requirementType ? _self.requirementType : requirementType // ignore: cast_nullable_to_non_nullable
as RequirementType,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as RequirementPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as RequirementStatus,acceptanceCriteria: freezed == acceptanceCriteria ? _self.acceptanceCriteria : acceptanceCriteria // ignore: cast_nullable_to_non_nullable
as String?,estimatedEffort: freezed == estimatedEffort ? _self.estimatedEffort : estimatedEffort // ignore: cast_nullable_to_non_nullable
as double?,parentRequirementId: freezed == parentRequirementId ? _self.parentRequirementId : parentRequirementId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateRequirementDto].
extension CreateUpdateRequirementDtoPatterns on CreateUpdateRequirementDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateRequirementDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateRequirementDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateRequirementDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto() when $default != null:
return $default(_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto():
return $default(_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String projectId,  String title,  String description,  RequirementType requirementType,  RequirementPriority priority,  RequirementStatus status,  String? acceptanceCriteria,  double? estimatedEffort,  String? parentRequirementId,  List<String> tags)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateRequirementDto() when $default != null:
return $default(_that.projectId,_that.title,_that.description,_that.requirementType,_that.priority,_that.status,_that.acceptanceCriteria,_that.estimatedEffort,_that.parentRequirementId,_that.tags);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateRequirementDto implements CreateUpdateRequirementDto {
  const _CreateUpdateRequirementDto({required this.projectId, required this.title, required this.description, required this.requirementType, required this.priority, required this.status, this.acceptanceCriteria, this.estimatedEffort, this.parentRequirementId, final  List<String> tags = const []}): _tags = tags;
  factory _CreateUpdateRequirementDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateRequirementDtoFromJson(json);

@override final  String projectId;
@override final  String title;
@override final  String description;
@override final  RequirementType requirementType;
@override final  RequirementPriority priority;
@override final  RequirementStatus status;
@override final  String? acceptanceCriteria;
@override final  double? estimatedEffort;
@override final  String? parentRequirementId;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}


/// Create a copy of CreateUpdateRequirementDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateRequirementDtoCopyWith<_CreateUpdateRequirementDto> get copyWith => __$CreateUpdateRequirementDtoCopyWithImpl<_CreateUpdateRequirementDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateRequirementDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateRequirementDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.requirementType, requirementType) || other.requirementType == requirementType)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.acceptanceCriteria, acceptanceCriteria) || other.acceptanceCriteria == acceptanceCriteria)&&(identical(other.estimatedEffort, estimatedEffort) || other.estimatedEffort == estimatedEffort)&&(identical(other.parentRequirementId, parentRequirementId) || other.parentRequirementId == parentRequirementId)&&const DeepCollectionEquality().equals(other._tags, _tags));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,title,description,requirementType,priority,status,acceptanceCriteria,estimatedEffort,parentRequirementId,const DeepCollectionEquality().hash(_tags));

@override
String toString() {
  return 'CreateUpdateRequirementDto(projectId: $projectId, title: $title, description: $description, requirementType: $requirementType, priority: $priority, status: $status, acceptanceCriteria: $acceptanceCriteria, estimatedEffort: $estimatedEffort, parentRequirementId: $parentRequirementId, tags: $tags)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateRequirementDtoCopyWith<$Res> implements $CreateUpdateRequirementDtoCopyWith<$Res> {
  factory _$CreateUpdateRequirementDtoCopyWith(_CreateUpdateRequirementDto value, $Res Function(_CreateUpdateRequirementDto) _then) = __$CreateUpdateRequirementDtoCopyWithImpl;
@override @useResult
$Res call({
 String projectId, String title, String description, RequirementType requirementType, RequirementPriority priority, RequirementStatus status, String? acceptanceCriteria, double? estimatedEffort, String? parentRequirementId, List<String> tags
});




}
/// @nodoc
class __$CreateUpdateRequirementDtoCopyWithImpl<$Res>
    implements _$CreateUpdateRequirementDtoCopyWith<$Res> {
  __$CreateUpdateRequirementDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateRequirementDto _self;
  final $Res Function(_CreateUpdateRequirementDto) _then;

/// Create a copy of CreateUpdateRequirementDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? projectId = null,Object? title = null,Object? description = null,Object? requirementType = null,Object? priority = null,Object? status = null,Object? acceptanceCriteria = freezed,Object? estimatedEffort = freezed,Object? parentRequirementId = freezed,Object? tags = null,}) {
  return _then(_CreateUpdateRequirementDto(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,requirementType: null == requirementType ? _self.requirementType : requirementType // ignore: cast_nullable_to_non_nullable
as RequirementType,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as RequirementPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as RequirementStatus,acceptanceCriteria: freezed == acceptanceCriteria ? _self.acceptanceCriteria : acceptanceCriteria // ignore: cast_nullable_to_non_nullable
as String?,estimatedEffort: freezed == estimatedEffort ? _self.estimatedEffort : estimatedEffort // ignore: cast_nullable_to_non_nullable
as double?,parentRequirementId: freezed == parentRequirementId ? _self.parentRequirementId : parentRequirementId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}


}

// dart format on
