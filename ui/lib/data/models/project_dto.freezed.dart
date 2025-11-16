// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectDto {

 String get id; String get name; String? get description; String? get parentProjectId; DateTime get startDate; DateTime get endDate; ProjectStatus get status; ProjectPriority get priority; ProjectViewType get viewType; List<String> get teamMembers; String get ownerId; String get workflowId; String get color; double get progress; double? get budget; double? get actualCost; DateTime get creationTime; DateTime? get lastModificationTime;
/// Create a copy of ProjectDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectDtoCopyWith<ProjectDto> get copyWith => _$ProjectDtoCopyWithImpl<ProjectDto>(this as ProjectDto, _$identity);

  /// Serializes this ProjectDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other.teamMembers, teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(teamMembers),ownerId,workflowId,color,progress,budget,actualCost,creationTime,lastModificationTime);

@override
String toString() {
  return 'ProjectDto(id: $id, name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class $ProjectDtoCopyWith<$Res>  {
  factory $ProjectDtoCopyWith(ProjectDto value, $Res Function(ProjectDto) _then) = _$ProjectDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost, DateTime creationTime, DateTime? lastModificationTime
});




}
/// @nodoc
class _$ProjectDtoCopyWithImpl<$Res>
    implements $ProjectDtoCopyWith<$Res> {
  _$ProjectDtoCopyWithImpl(this._self, this._then);

  final ProjectDto _self;
  final $Res Function(ProjectDto) _then;

/// Create a copy of ProjectDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentProjectId: freezed == parentProjectId ? _self.parentProjectId : parentProjectId // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProjectStatus,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as ProjectPriority,viewType: null == viewType ? _self.viewType : viewType // ignore: cast_nullable_to_non_nullable
as ProjectViewType,teamMembers: null == teamMembers ? _self.teamMembers : teamMembers // ignore: cast_nullable_to_non_nullable
as List<String>,ownerId: null == ownerId ? _self.ownerId : ownerId // ignore: cast_nullable_to_non_nullable
as String,workflowId: null == workflowId ? _self.workflowId : workflowId // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,budget: freezed == budget ? _self.budget : budget // ignore: cast_nullable_to_non_nullable
as double?,actualCost: freezed == actualCost ? _self.actualCost : actualCost // ignore: cast_nullable_to_non_nullable
as double?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectDto].
extension ProjectDtoPatterns on ProjectDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  DateTime creationTime,  DateTime? lastModificationTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectDto():
return $default(_that.id,_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectDto() when $default != null:
return $default(_that.id,_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.creationTime,_that.lastModificationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectDto implements ProjectDto {
  const _ProjectDto({required this.id, required this.name, this.description, this.parentProjectId, required this.startDate, required this.endDate, required this.status, required this.priority, required this.viewType, required final  List<String> teamMembers, required this.ownerId, required this.workflowId, required this.color, required this.progress, this.budget, this.actualCost, required this.creationTime, this.lastModificationTime}): _teamMembers = teamMembers;
  factory _ProjectDto.fromJson(Map<String, dynamic> json) => _$ProjectDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String? description;
@override final  String? parentProjectId;
@override final  DateTime startDate;
@override final  DateTime endDate;
@override final  ProjectStatus status;
@override final  ProjectPriority priority;
@override final  ProjectViewType viewType;
 final  List<String> _teamMembers;
@override List<String> get teamMembers {
  if (_teamMembers is EqualUnmodifiableListView) return _teamMembers;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_teamMembers);
}

@override final  String ownerId;
@override final  String workflowId;
@override final  String color;
@override final  double progress;
@override final  double? budget;
@override final  double? actualCost;
@override final  DateTime creationTime;
@override final  DateTime? lastModificationTime;

/// Create a copy of ProjectDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectDtoCopyWith<_ProjectDto> get copyWith => __$ProjectDtoCopyWithImpl<_ProjectDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other._teamMembers, _teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(_teamMembers),ownerId,workflowId,color,progress,budget,actualCost,creationTime,lastModificationTime);

@override
String toString() {
  return 'ProjectDto(id: $id, name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectDtoCopyWith<$Res> implements $ProjectDtoCopyWith<$Res> {
  factory _$ProjectDtoCopyWith(_ProjectDto value, $Res Function(_ProjectDto) _then) = __$ProjectDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost, DateTime creationTime, DateTime? lastModificationTime
});




}
/// @nodoc
class __$ProjectDtoCopyWithImpl<$Res>
    implements _$ProjectDtoCopyWith<$Res> {
  __$ProjectDtoCopyWithImpl(this._self, this._then);

  final _ProjectDto _self;
  final $Res Function(_ProjectDto) _then;

/// Create a copy of ProjectDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_ProjectDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentProjectId: freezed == parentProjectId ? _self.parentProjectId : parentProjectId // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProjectStatus,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as ProjectPriority,viewType: null == viewType ? _self.viewType : viewType // ignore: cast_nullable_to_non_nullable
as ProjectViewType,teamMembers: null == teamMembers ? _self._teamMembers : teamMembers // ignore: cast_nullable_to_non_nullable
as List<String>,ownerId: null == ownerId ? _self.ownerId : ownerId // ignore: cast_nullable_to_non_nullable
as String,workflowId: null == workflowId ? _self.workflowId : workflowId // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,budget: freezed == budget ? _self.budget : budget // ignore: cast_nullable_to_non_nullable
as double?,actualCost: freezed == actualCost ? _self.actualCost : actualCost // ignore: cast_nullable_to_non_nullable
as double?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateProjectDto {

 String get name; String? get description; String? get parentProjectId; DateTime get startDate; DateTime get endDate; ProjectStatus get status; ProjectPriority get priority; ProjectViewType get viewType; List<String> get teamMembers; String get ownerId; String get workflowId; String get color; double get progress; double? get budget; double? get actualCost;
/// Create a copy of CreateUpdateProjectDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateProjectDtoCopyWith<CreateUpdateProjectDto> get copyWith => _$CreateUpdateProjectDtoCopyWithImpl<CreateUpdateProjectDto>(this as CreateUpdateProjectDto, _$identity);

  /// Serializes this CreateUpdateProjectDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateProjectDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other.teamMembers, teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(teamMembers),ownerId,workflowId,color,progress,budget,actualCost);

@override
String toString() {
  return 'CreateUpdateProjectDto(name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateProjectDtoCopyWith<$Res>  {
  factory $CreateUpdateProjectDtoCopyWith(CreateUpdateProjectDto value, $Res Function(CreateUpdateProjectDto) _then) = _$CreateUpdateProjectDtoCopyWithImpl;
@useResult
$Res call({
 String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost
});




}
/// @nodoc
class _$CreateUpdateProjectDtoCopyWithImpl<$Res>
    implements $CreateUpdateProjectDtoCopyWith<$Res> {
  _$CreateUpdateProjectDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateProjectDto _self;
  final $Res Function(CreateUpdateProjectDto) _then;

/// Create a copy of CreateUpdateProjectDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,}) {
  return _then(_self.copyWith(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentProjectId: freezed == parentProjectId ? _self.parentProjectId : parentProjectId // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProjectStatus,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as ProjectPriority,viewType: null == viewType ? _self.viewType : viewType // ignore: cast_nullable_to_non_nullable
as ProjectViewType,teamMembers: null == teamMembers ? _self.teamMembers : teamMembers // ignore: cast_nullable_to_non_nullable
as List<String>,ownerId: null == ownerId ? _self.ownerId : ownerId // ignore: cast_nullable_to_non_nullable
as String,workflowId: null == workflowId ? _self.workflowId : workflowId // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,budget: freezed == budget ? _self.budget : budget // ignore: cast_nullable_to_non_nullable
as double?,actualCost: freezed == actualCost ? _self.actualCost : actualCost // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateProjectDto].
extension CreateUpdateProjectDtoPatterns on CreateUpdateProjectDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateProjectDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateProjectDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateProjectDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateProjectDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateProjectDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateProjectDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateProjectDto() when $default != null:
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateProjectDto():
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateProjectDto() when $default != null:
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateProjectDto implements CreateUpdateProjectDto {
  const _CreateUpdateProjectDto({required this.name, this.description, this.parentProjectId, required this.startDate, required this.endDate, required this.status, required this.priority, required this.viewType, final  List<String> teamMembers = const [], required this.ownerId, required this.workflowId, this.color = '#2196F3', this.progress = 0.0, this.budget, this.actualCost}): _teamMembers = teamMembers;
  factory _CreateUpdateProjectDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateProjectDtoFromJson(json);

@override final  String name;
@override final  String? description;
@override final  String? parentProjectId;
@override final  DateTime startDate;
@override final  DateTime endDate;
@override final  ProjectStatus status;
@override final  ProjectPriority priority;
@override final  ProjectViewType viewType;
 final  List<String> _teamMembers;
@override@JsonKey() List<String> get teamMembers {
  if (_teamMembers is EqualUnmodifiableListView) return _teamMembers;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_teamMembers);
}

@override final  String ownerId;
@override final  String workflowId;
@override@JsonKey() final  String color;
@override@JsonKey() final  double progress;
@override final  double? budget;
@override final  double? actualCost;

/// Create a copy of CreateUpdateProjectDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateProjectDtoCopyWith<_CreateUpdateProjectDto> get copyWith => __$CreateUpdateProjectDtoCopyWithImpl<_CreateUpdateProjectDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateProjectDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateProjectDto&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other._teamMembers, _teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(_teamMembers),ownerId,workflowId,color,progress,budget,actualCost);

@override
String toString() {
  return 'CreateUpdateProjectDto(name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateProjectDtoCopyWith<$Res> implements $CreateUpdateProjectDtoCopyWith<$Res> {
  factory _$CreateUpdateProjectDtoCopyWith(_CreateUpdateProjectDto value, $Res Function(_CreateUpdateProjectDto) _then) = __$CreateUpdateProjectDtoCopyWithImpl;
@override @useResult
$Res call({
 String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost
});




}
/// @nodoc
class __$CreateUpdateProjectDtoCopyWithImpl<$Res>
    implements _$CreateUpdateProjectDtoCopyWith<$Res> {
  __$CreateUpdateProjectDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateProjectDto _self;
  final $Res Function(_CreateUpdateProjectDto) _then;

/// Create a copy of CreateUpdateProjectDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,}) {
  return _then(_CreateUpdateProjectDto(
name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentProjectId: freezed == parentProjectId ? _self.parentProjectId : parentProjectId // ignore: cast_nullable_to_non_nullable
as String?,startDate: null == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime,endDate: null == endDate ? _self.endDate : endDate // ignore: cast_nullable_to_non_nullable
as DateTime,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProjectStatus,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as ProjectPriority,viewType: null == viewType ? _self.viewType : viewType // ignore: cast_nullable_to_non_nullable
as ProjectViewType,teamMembers: null == teamMembers ? _self._teamMembers : teamMembers // ignore: cast_nullable_to_non_nullable
as List<String>,ownerId: null == ownerId ? _self.ownerId : ownerId // ignore: cast_nullable_to_non_nullable
as String,workflowId: null == workflowId ? _self.workflowId : workflowId // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,budget: freezed == budget ? _self.budget : budget // ignore: cast_nullable_to_non_nullable
as double?,actualCost: freezed == actualCost ? _self.actualCost : actualCost // ignore: cast_nullable_to_non_nullable
as double?,
  ));
}


}

// dart format on
