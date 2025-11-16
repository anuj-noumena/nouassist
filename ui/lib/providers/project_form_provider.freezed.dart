// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_form_provider.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;
/// @nodoc
mixin _$ProjectFormState {

 String get name; String? get description; String? get parentProjectId; DateTime get startDate; DateTime get endDate; ProjectStatus get status; ProjectPriority get priority; ProjectViewType get viewType; List<String> get teamMembers; String get ownerId; String get workflowId; String get color; double get progress; double? get budget; double? get actualCost; bool get isDirty; bool get isSubmitting; Map<String, String> get errors; String? get submitError;
/// Create a copy of ProjectFormState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectFormStateCopyWith<ProjectFormState> get copyWith => _$ProjectFormStateCopyWithImpl<ProjectFormState>(this as ProjectFormState, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectFormState&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other.teamMembers, teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost)&&(identical(other.isDirty, isDirty) || other.isDirty == isDirty)&&(identical(other.isSubmitting, isSubmitting) || other.isSubmitting == isSubmitting)&&const DeepCollectionEquality().equals(other.errors, errors)&&(identical(other.submitError, submitError) || other.submitError == submitError));
}


@override
int get hashCode => Object.hashAll([runtimeType,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(teamMembers),ownerId,workflowId,color,progress,budget,actualCost,isDirty,isSubmitting,const DeepCollectionEquality().hash(errors),submitError]);

@override
String toString() {
  return 'ProjectFormState(name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost, isDirty: $isDirty, isSubmitting: $isSubmitting, errors: $errors, submitError: $submitError)';
}


}

/// @nodoc
abstract mixin class $ProjectFormStateCopyWith<$Res>  {
  factory $ProjectFormStateCopyWith(ProjectFormState value, $Res Function(ProjectFormState) _then) = _$ProjectFormStateCopyWithImpl;
@useResult
$Res call({
 String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost, bool isDirty, bool isSubmitting, Map<String, String> errors, String? submitError
});




}
/// @nodoc
class _$ProjectFormStateCopyWithImpl<$Res>
    implements $ProjectFormStateCopyWith<$Res> {
  _$ProjectFormStateCopyWithImpl(this._self, this._then);

  final ProjectFormState _self;
  final $Res Function(ProjectFormState) _then;

/// Create a copy of ProjectFormState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,Object? isDirty = null,Object? isSubmitting = null,Object? errors = null,Object? submitError = freezed,}) {
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
as double?,isDirty: null == isDirty ? _self.isDirty : isDirty // ignore: cast_nullable_to_non_nullable
as bool,isSubmitting: null == isSubmitting ? _self.isSubmitting : isSubmitting // ignore: cast_nullable_to_non_nullable
as bool,errors: null == errors ? _self.errors : errors // ignore: cast_nullable_to_non_nullable
as Map<String, String>,submitError: freezed == submitError ? _self.submitError : submitError // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectFormState].
extension ProjectFormStatePatterns on ProjectFormState {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectFormState value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectFormState() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectFormState value)  $default,){
final _that = this;
switch (_that) {
case _ProjectFormState():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectFormState value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectFormState() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectFormState() when $default != null:
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)  $default,) {final _that = this;
switch (_that) {
case _ProjectFormState():
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String name,  String? description,  String? parentProjectId,  DateTime startDate,  DateTime endDate,  ProjectStatus status,  ProjectPriority priority,  ProjectViewType viewType,  List<String> teamMembers,  String ownerId,  String workflowId,  String color,  double progress,  double? budget,  double? actualCost,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)?  $default,) {final _that = this;
switch (_that) {
case _ProjectFormState() when $default != null:
return $default(_that.name,_that.description,_that.parentProjectId,_that.startDate,_that.endDate,_that.status,_that.priority,_that.viewType,_that.teamMembers,_that.ownerId,_that.workflowId,_that.color,_that.progress,_that.budget,_that.actualCost,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
  return null;

}
}

}

/// @nodoc


class _ProjectFormState implements ProjectFormState {
  const _ProjectFormState({this.name = '', this.description, this.parentProjectId, required this.startDate, required this.endDate, required this.status, required this.priority, required this.viewType, final  List<String> teamMembers = const [], required this.ownerId, required this.workflowId, this.color = '#2196F3', this.progress = 0.0, this.budget, this.actualCost, this.isDirty = false, this.isSubmitting = false, final  Map<String, String> errors = const {}, this.submitError}): _teamMembers = teamMembers,_errors = errors;
  

@override@JsonKey() final  String name;
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
@override@JsonKey() final  bool isDirty;
@override@JsonKey() final  bool isSubmitting;
 final  Map<String, String> _errors;
@override@JsonKey() Map<String, String> get errors {
  if (_errors is EqualUnmodifiableMapView) return _errors;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableMapView(_errors);
}

@override final  String? submitError;

/// Create a copy of ProjectFormState
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectFormStateCopyWith<_ProjectFormState> get copyWith => __$ProjectFormStateCopyWithImpl<_ProjectFormState>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectFormState&&(identical(other.name, name) || other.name == name)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentProjectId, parentProjectId) || other.parentProjectId == parentProjectId)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.endDate, endDate) || other.endDate == endDate)&&(identical(other.status, status) || other.status == status)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.viewType, viewType) || other.viewType == viewType)&&const DeepCollectionEquality().equals(other._teamMembers, _teamMembers)&&(identical(other.ownerId, ownerId) || other.ownerId == ownerId)&&(identical(other.workflowId, workflowId) || other.workflowId == workflowId)&&(identical(other.color, color) || other.color == color)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.budget, budget) || other.budget == budget)&&(identical(other.actualCost, actualCost) || other.actualCost == actualCost)&&(identical(other.isDirty, isDirty) || other.isDirty == isDirty)&&(identical(other.isSubmitting, isSubmitting) || other.isSubmitting == isSubmitting)&&const DeepCollectionEquality().equals(other._errors, _errors)&&(identical(other.submitError, submitError) || other.submitError == submitError));
}


@override
int get hashCode => Object.hashAll([runtimeType,name,description,parentProjectId,startDate,endDate,status,priority,viewType,const DeepCollectionEquality().hash(_teamMembers),ownerId,workflowId,color,progress,budget,actualCost,isDirty,isSubmitting,const DeepCollectionEquality().hash(_errors),submitError]);

@override
String toString() {
  return 'ProjectFormState(name: $name, description: $description, parentProjectId: $parentProjectId, startDate: $startDate, endDate: $endDate, status: $status, priority: $priority, viewType: $viewType, teamMembers: $teamMembers, ownerId: $ownerId, workflowId: $workflowId, color: $color, progress: $progress, budget: $budget, actualCost: $actualCost, isDirty: $isDirty, isSubmitting: $isSubmitting, errors: $errors, submitError: $submitError)';
}


}

/// @nodoc
abstract mixin class _$ProjectFormStateCopyWith<$Res> implements $ProjectFormStateCopyWith<$Res> {
  factory _$ProjectFormStateCopyWith(_ProjectFormState value, $Res Function(_ProjectFormState) _then) = __$ProjectFormStateCopyWithImpl;
@override @useResult
$Res call({
 String name, String? description, String? parentProjectId, DateTime startDate, DateTime endDate, ProjectStatus status, ProjectPriority priority, ProjectViewType viewType, List<String> teamMembers, String ownerId, String workflowId, String color, double progress, double? budget, double? actualCost, bool isDirty, bool isSubmitting, Map<String, String> errors, String? submitError
});




}
/// @nodoc
class __$ProjectFormStateCopyWithImpl<$Res>
    implements _$ProjectFormStateCopyWith<$Res> {
  __$ProjectFormStateCopyWithImpl(this._self, this._then);

  final _ProjectFormState _self;
  final $Res Function(_ProjectFormState) _then;

/// Create a copy of ProjectFormState
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? name = null,Object? description = freezed,Object? parentProjectId = freezed,Object? startDate = null,Object? endDate = null,Object? status = null,Object? priority = null,Object? viewType = null,Object? teamMembers = null,Object? ownerId = null,Object? workflowId = null,Object? color = null,Object? progress = null,Object? budget = freezed,Object? actualCost = freezed,Object? isDirty = null,Object? isSubmitting = null,Object? errors = null,Object? submitError = freezed,}) {
  return _then(_ProjectFormState(
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
as double?,isDirty: null == isDirty ? _self.isDirty : isDirty // ignore: cast_nullable_to_non_nullable
as bool,isSubmitting: null == isSubmitting ? _self.isSubmitting : isSubmitting // ignore: cast_nullable_to_non_nullable
as bool,errors: null == errors ? _self._errors : errors // ignore: cast_nullable_to_non_nullable
as Map<String, String>,submitError: freezed == submitError ? _self.submitError : submitError // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
