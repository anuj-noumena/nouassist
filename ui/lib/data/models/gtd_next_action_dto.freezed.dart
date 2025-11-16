// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_next_action_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdNextActionDto {

 String get id; String get userId; String get action; String? get notes; List<String> get contextIds; String? get projectId; String? get areaOfFocusId; EnergyLevel get energyLevel; TimeRequired get timeRequired; DateTime? get dueDate; DateTime? get scheduledDate; bool get isCompleted; DateTime? get completedDate; bool get isStarred; List<String> get tags; String? get outcome; DateTime get creationTime;
/// Create a copy of GtdNextActionDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdNextActionDtoCopyWith<GtdNextActionDto> get copyWith => _$GtdNextActionDtoCopyWithImpl<GtdNextActionDto>(this as GtdNextActionDto, _$identity);

  /// Serializes this GtdNextActionDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdNextActionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.action, action) || other.action == action)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other.contextIds, contextIds)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&(identical(other.energyLevel, energyLevel) || other.energyLevel == energyLevel)&&(identical(other.timeRequired, timeRequired) || other.timeRequired == timeRequired)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.scheduledDate, scheduledDate) || other.scheduledDate == scheduledDate)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.isStarred, isStarred) || other.isStarred == isStarred)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.outcome, outcome) || other.outcome == outcome)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,action,notes,const DeepCollectionEquality().hash(contextIds),projectId,areaOfFocusId,energyLevel,timeRequired,dueDate,scheduledDate,isCompleted,completedDate,isStarred,const DeepCollectionEquality().hash(tags),outcome,creationTime);

@override
String toString() {
  return 'GtdNextActionDto(id: $id, userId: $userId, action: $action, notes: $notes, contextIds: $contextIds, projectId: $projectId, areaOfFocusId: $areaOfFocusId, energyLevel: $energyLevel, timeRequired: $timeRequired, dueDate: $dueDate, scheduledDate: $scheduledDate, isCompleted: $isCompleted, completedDate: $completedDate, isStarred: $isStarred, tags: $tags, outcome: $outcome, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdNextActionDtoCopyWith<$Res>  {
  factory $GtdNextActionDtoCopyWith(GtdNextActionDto value, $Res Function(GtdNextActionDto) _then) = _$GtdNextActionDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String action, String? notes, List<String> contextIds, String? projectId, String? areaOfFocusId, EnergyLevel energyLevel, TimeRequired timeRequired, DateTime? dueDate, DateTime? scheduledDate, bool isCompleted, DateTime? completedDate, bool isStarred, List<String> tags, String? outcome, DateTime creationTime
});




}
/// @nodoc
class _$GtdNextActionDtoCopyWithImpl<$Res>
    implements $GtdNextActionDtoCopyWith<$Res> {
  _$GtdNextActionDtoCopyWithImpl(this._self, this._then);

  final GtdNextActionDto _self;
  final $Res Function(GtdNextActionDto) _then;

/// Create a copy of GtdNextActionDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? action = null,Object? notes = freezed,Object? contextIds = null,Object? projectId = freezed,Object? areaOfFocusId = freezed,Object? energyLevel = null,Object? timeRequired = null,Object? dueDate = freezed,Object? scheduledDate = freezed,Object? isCompleted = null,Object? completedDate = freezed,Object? isStarred = null,Object? tags = null,Object? outcome = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,action: null == action ? _self.action : action // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,contextIds: null == contextIds ? _self.contextIds : contextIds // ignore: cast_nullable_to_non_nullable
as List<String>,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,energyLevel: null == energyLevel ? _self.energyLevel : energyLevel // ignore: cast_nullable_to_non_nullable
as EnergyLevel,timeRequired: null == timeRequired ? _self.timeRequired : timeRequired // ignore: cast_nullable_to_non_nullable
as TimeRequired,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,scheduledDate: freezed == scheduledDate ? _self.scheduledDate : scheduledDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isStarred: null == isStarred ? _self.isStarred : isStarred // ignore: cast_nullable_to_non_nullable
as bool,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,outcome: freezed == outcome ? _self.outcome : outcome // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdNextActionDto].
extension GtdNextActionDtoPatterns on GtdNextActionDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdNextActionDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdNextActionDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdNextActionDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdNextActionDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdNextActionDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdNextActionDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isCompleted,  DateTime? completedDate,  bool isStarred,  List<String> tags,  String? outcome,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdNextActionDto() when $default != null:
return $default(_that.id,_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isCompleted,_that.completedDate,_that.isStarred,_that.tags,_that.outcome,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isCompleted,  DateTime? completedDate,  bool isStarred,  List<String> tags,  String? outcome,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdNextActionDto():
return $default(_that.id,_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isCompleted,_that.completedDate,_that.isStarred,_that.tags,_that.outcome,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isCompleted,  DateTime? completedDate,  bool isStarred,  List<String> tags,  String? outcome,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdNextActionDto() when $default != null:
return $default(_that.id,_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isCompleted,_that.completedDate,_that.isStarred,_that.tags,_that.outcome,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdNextActionDto implements GtdNextActionDto {
  const _GtdNextActionDto({required this.id, required this.userId, required this.action, this.notes, required final  List<String> contextIds, this.projectId, this.areaOfFocusId, required this.energyLevel, required this.timeRequired, this.dueDate, this.scheduledDate, this.isCompleted = false, this.completedDate, this.isStarred = false, final  List<String> tags = const [], this.outcome, required this.creationTime}): _contextIds = contextIds,_tags = tags;
  factory _GtdNextActionDto.fromJson(Map<String, dynamic> json) => _$GtdNextActionDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String action;
@override final  String? notes;
 final  List<String> _contextIds;
@override List<String> get contextIds {
  if (_contextIds is EqualUnmodifiableListView) return _contextIds;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_contextIds);
}

@override final  String? projectId;
@override final  String? areaOfFocusId;
@override final  EnergyLevel energyLevel;
@override final  TimeRequired timeRequired;
@override final  DateTime? dueDate;
@override final  DateTime? scheduledDate;
@override@JsonKey() final  bool isCompleted;
@override final  DateTime? completedDate;
@override@JsonKey() final  bool isStarred;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String? outcome;
@override final  DateTime creationTime;

/// Create a copy of GtdNextActionDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdNextActionDtoCopyWith<_GtdNextActionDto> get copyWith => __$GtdNextActionDtoCopyWithImpl<_GtdNextActionDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdNextActionDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdNextActionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.action, action) || other.action == action)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other._contextIds, _contextIds)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&(identical(other.energyLevel, energyLevel) || other.energyLevel == energyLevel)&&(identical(other.timeRequired, timeRequired) || other.timeRequired == timeRequired)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.scheduledDate, scheduledDate) || other.scheduledDate == scheduledDate)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.isStarred, isStarred) || other.isStarred == isStarred)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.outcome, outcome) || other.outcome == outcome)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,action,notes,const DeepCollectionEquality().hash(_contextIds),projectId,areaOfFocusId,energyLevel,timeRequired,dueDate,scheduledDate,isCompleted,completedDate,isStarred,const DeepCollectionEquality().hash(_tags),outcome,creationTime);

@override
String toString() {
  return 'GtdNextActionDto(id: $id, userId: $userId, action: $action, notes: $notes, contextIds: $contextIds, projectId: $projectId, areaOfFocusId: $areaOfFocusId, energyLevel: $energyLevel, timeRequired: $timeRequired, dueDate: $dueDate, scheduledDate: $scheduledDate, isCompleted: $isCompleted, completedDate: $completedDate, isStarred: $isStarred, tags: $tags, outcome: $outcome, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdNextActionDtoCopyWith<$Res> implements $GtdNextActionDtoCopyWith<$Res> {
  factory _$GtdNextActionDtoCopyWith(_GtdNextActionDto value, $Res Function(_GtdNextActionDto) _then) = __$GtdNextActionDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String action, String? notes, List<String> contextIds, String? projectId, String? areaOfFocusId, EnergyLevel energyLevel, TimeRequired timeRequired, DateTime? dueDate, DateTime? scheduledDate, bool isCompleted, DateTime? completedDate, bool isStarred, List<String> tags, String? outcome, DateTime creationTime
});




}
/// @nodoc
class __$GtdNextActionDtoCopyWithImpl<$Res>
    implements _$GtdNextActionDtoCopyWith<$Res> {
  __$GtdNextActionDtoCopyWithImpl(this._self, this._then);

  final _GtdNextActionDto _self;
  final $Res Function(_GtdNextActionDto) _then;

/// Create a copy of GtdNextActionDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? action = null,Object? notes = freezed,Object? contextIds = null,Object? projectId = freezed,Object? areaOfFocusId = freezed,Object? energyLevel = null,Object? timeRequired = null,Object? dueDate = freezed,Object? scheduledDate = freezed,Object? isCompleted = null,Object? completedDate = freezed,Object? isStarred = null,Object? tags = null,Object? outcome = freezed,Object? creationTime = null,}) {
  return _then(_GtdNextActionDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,action: null == action ? _self.action : action // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,contextIds: null == contextIds ? _self._contextIds : contextIds // ignore: cast_nullable_to_non_nullable
as List<String>,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,energyLevel: null == energyLevel ? _self.energyLevel : energyLevel // ignore: cast_nullable_to_non_nullable
as EnergyLevel,timeRequired: null == timeRequired ? _self.timeRequired : timeRequired // ignore: cast_nullable_to_non_nullable
as TimeRequired,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,scheduledDate: freezed == scheduledDate ? _self.scheduledDate : scheduledDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isStarred: null == isStarred ? _self.isStarred : isStarred // ignore: cast_nullable_to_non_nullable
as bool,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,outcome: freezed == outcome ? _self.outcome : outcome // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateNextActionDto {

 String get userId; String get action; String? get notes; List<String> get contextIds; String? get projectId; String? get areaOfFocusId; EnergyLevel get energyLevel; TimeRequired get timeRequired; DateTime? get dueDate; DateTime? get scheduledDate; bool get isStarred; List<String> get tags; String? get outcome;
/// Create a copy of CreateUpdateNextActionDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateNextActionDtoCopyWith<CreateUpdateNextActionDto> get copyWith => _$CreateUpdateNextActionDtoCopyWithImpl<CreateUpdateNextActionDto>(this as CreateUpdateNextActionDto, _$identity);

  /// Serializes this CreateUpdateNextActionDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateNextActionDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.action, action) || other.action == action)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other.contextIds, contextIds)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&(identical(other.energyLevel, energyLevel) || other.energyLevel == energyLevel)&&(identical(other.timeRequired, timeRequired) || other.timeRequired == timeRequired)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.scheduledDate, scheduledDate) || other.scheduledDate == scheduledDate)&&(identical(other.isStarred, isStarred) || other.isStarred == isStarred)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.outcome, outcome) || other.outcome == outcome));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,action,notes,const DeepCollectionEquality().hash(contextIds),projectId,areaOfFocusId,energyLevel,timeRequired,dueDate,scheduledDate,isStarred,const DeepCollectionEquality().hash(tags),outcome);

@override
String toString() {
  return 'CreateUpdateNextActionDto(userId: $userId, action: $action, notes: $notes, contextIds: $contextIds, projectId: $projectId, areaOfFocusId: $areaOfFocusId, energyLevel: $energyLevel, timeRequired: $timeRequired, dueDate: $dueDate, scheduledDate: $scheduledDate, isStarred: $isStarred, tags: $tags, outcome: $outcome)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateNextActionDtoCopyWith<$Res>  {
  factory $CreateUpdateNextActionDtoCopyWith(CreateUpdateNextActionDto value, $Res Function(CreateUpdateNextActionDto) _then) = _$CreateUpdateNextActionDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String action, String? notes, List<String> contextIds, String? projectId, String? areaOfFocusId, EnergyLevel energyLevel, TimeRequired timeRequired, DateTime? dueDate, DateTime? scheduledDate, bool isStarred, List<String> tags, String? outcome
});




}
/// @nodoc
class _$CreateUpdateNextActionDtoCopyWithImpl<$Res>
    implements $CreateUpdateNextActionDtoCopyWith<$Res> {
  _$CreateUpdateNextActionDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateNextActionDto _self;
  final $Res Function(CreateUpdateNextActionDto) _then;

/// Create a copy of CreateUpdateNextActionDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? action = null,Object? notes = freezed,Object? contextIds = null,Object? projectId = freezed,Object? areaOfFocusId = freezed,Object? energyLevel = null,Object? timeRequired = null,Object? dueDate = freezed,Object? scheduledDate = freezed,Object? isStarred = null,Object? tags = null,Object? outcome = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,action: null == action ? _self.action : action // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,contextIds: null == contextIds ? _self.contextIds : contextIds // ignore: cast_nullable_to_non_nullable
as List<String>,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,energyLevel: null == energyLevel ? _self.energyLevel : energyLevel // ignore: cast_nullable_to_non_nullable
as EnergyLevel,timeRequired: null == timeRequired ? _self.timeRequired : timeRequired // ignore: cast_nullable_to_non_nullable
as TimeRequired,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,scheduledDate: freezed == scheduledDate ? _self.scheduledDate : scheduledDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isStarred: null == isStarred ? _self.isStarred : isStarred // ignore: cast_nullable_to_non_nullable
as bool,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,outcome: freezed == outcome ? _self.outcome : outcome // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateNextActionDto].
extension CreateUpdateNextActionDtoPatterns on CreateUpdateNextActionDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateNextActionDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateNextActionDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateNextActionDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isStarred,  List<String> tags,  String? outcome)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto() when $default != null:
return $default(_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isStarred,_that.tags,_that.outcome);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isStarred,  List<String> tags,  String? outcome)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto():
return $default(_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isStarred,_that.tags,_that.outcome);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String action,  String? notes,  List<String> contextIds,  String? projectId,  String? areaOfFocusId,  EnergyLevel energyLevel,  TimeRequired timeRequired,  DateTime? dueDate,  DateTime? scheduledDate,  bool isStarred,  List<String> tags,  String? outcome)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateNextActionDto() when $default != null:
return $default(_that.userId,_that.action,_that.notes,_that.contextIds,_that.projectId,_that.areaOfFocusId,_that.energyLevel,_that.timeRequired,_that.dueDate,_that.scheduledDate,_that.isStarred,_that.tags,_that.outcome);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateNextActionDto implements CreateUpdateNextActionDto {
  const _CreateUpdateNextActionDto({required this.userId, required this.action, this.notes, final  List<String> contextIds = const [], this.projectId, this.areaOfFocusId, required this.energyLevel, required this.timeRequired, this.dueDate, this.scheduledDate, this.isStarred = false, final  List<String> tags = const [], this.outcome}): _contextIds = contextIds,_tags = tags;
  factory _CreateUpdateNextActionDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateNextActionDtoFromJson(json);

@override final  String userId;
@override final  String action;
@override final  String? notes;
 final  List<String> _contextIds;
@override@JsonKey() List<String> get contextIds {
  if (_contextIds is EqualUnmodifiableListView) return _contextIds;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_contextIds);
}

@override final  String? projectId;
@override final  String? areaOfFocusId;
@override final  EnergyLevel energyLevel;
@override final  TimeRequired timeRequired;
@override final  DateTime? dueDate;
@override final  DateTime? scheduledDate;
@override@JsonKey() final  bool isStarred;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String? outcome;

/// Create a copy of CreateUpdateNextActionDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateNextActionDtoCopyWith<_CreateUpdateNextActionDto> get copyWith => __$CreateUpdateNextActionDtoCopyWithImpl<_CreateUpdateNextActionDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateNextActionDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateNextActionDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.action, action) || other.action == action)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other._contextIds, _contextIds)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&(identical(other.energyLevel, energyLevel) || other.energyLevel == energyLevel)&&(identical(other.timeRequired, timeRequired) || other.timeRequired == timeRequired)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.scheduledDate, scheduledDate) || other.scheduledDate == scheduledDate)&&(identical(other.isStarred, isStarred) || other.isStarred == isStarred)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.outcome, outcome) || other.outcome == outcome));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,action,notes,const DeepCollectionEquality().hash(_contextIds),projectId,areaOfFocusId,energyLevel,timeRequired,dueDate,scheduledDate,isStarred,const DeepCollectionEquality().hash(_tags),outcome);

@override
String toString() {
  return 'CreateUpdateNextActionDto(userId: $userId, action: $action, notes: $notes, contextIds: $contextIds, projectId: $projectId, areaOfFocusId: $areaOfFocusId, energyLevel: $energyLevel, timeRequired: $timeRequired, dueDate: $dueDate, scheduledDate: $scheduledDate, isStarred: $isStarred, tags: $tags, outcome: $outcome)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateNextActionDtoCopyWith<$Res> implements $CreateUpdateNextActionDtoCopyWith<$Res> {
  factory _$CreateUpdateNextActionDtoCopyWith(_CreateUpdateNextActionDto value, $Res Function(_CreateUpdateNextActionDto) _then) = __$CreateUpdateNextActionDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String action, String? notes, List<String> contextIds, String? projectId, String? areaOfFocusId, EnergyLevel energyLevel, TimeRequired timeRequired, DateTime? dueDate, DateTime? scheduledDate, bool isStarred, List<String> tags, String? outcome
});




}
/// @nodoc
class __$CreateUpdateNextActionDtoCopyWithImpl<$Res>
    implements _$CreateUpdateNextActionDtoCopyWith<$Res> {
  __$CreateUpdateNextActionDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateNextActionDto _self;
  final $Res Function(_CreateUpdateNextActionDto) _then;

/// Create a copy of CreateUpdateNextActionDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? action = null,Object? notes = freezed,Object? contextIds = null,Object? projectId = freezed,Object? areaOfFocusId = freezed,Object? energyLevel = null,Object? timeRequired = null,Object? dueDate = freezed,Object? scheduledDate = freezed,Object? isStarred = null,Object? tags = null,Object? outcome = freezed,}) {
  return _then(_CreateUpdateNextActionDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,action: null == action ? _self.action : action // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,contextIds: null == contextIds ? _self._contextIds : contextIds // ignore: cast_nullable_to_non_nullable
as List<String>,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,energyLevel: null == energyLevel ? _self.energyLevel : energyLevel // ignore: cast_nullable_to_non_nullable
as EnergyLevel,timeRequired: null == timeRequired ? _self.timeRequired : timeRequired // ignore: cast_nullable_to_non_nullable
as TimeRequired,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,scheduledDate: freezed == scheduledDate ? _self.scheduledDate : scheduledDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isStarred: null == isStarred ? _self.isStarred : isStarred // ignore: cast_nullable_to_non_nullable
as bool,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,outcome: freezed == outcome ? _self.outcome : outcome // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
