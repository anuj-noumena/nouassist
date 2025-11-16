// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_form_provider.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;
/// @nodoc
mixin _$TaskFormState {

 String get title; String? get description; String get taskTypeId; String get workflowStageId; String? get assigneeId; String get reporterId; TaskPriority get priority; TaskStatus get status; double? get estimatedHours; double? get actualHours; int? get storyPoints; DateTime? get startDate; DateTime? get dueDate; List<String> get tags; List<String> get watchers; double get progress; bool get isDirty; bool get isSubmitting; Map<String, String> get errors; String? get submitError;
/// Create a copy of TaskFormState
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskFormStateCopyWith<TaskFormState> get copyWith => _$TaskFormStateCopyWithImpl<TaskFormState>(this as TaskFormState, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskFormState&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&const DeepCollectionEquality().equals(other.tags, tags)&&const DeepCollectionEquality().equals(other.watchers, watchers)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.isDirty, isDirty) || other.isDirty == isDirty)&&(identical(other.isSubmitting, isSubmitting) || other.isSubmitting == isSubmitting)&&const DeepCollectionEquality().equals(other.errors, errors)&&(identical(other.submitError, submitError) || other.submitError == submitError));
}


@override
int get hashCode => Object.hashAll([runtimeType,title,description,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,const DeepCollectionEquality().hash(tags),const DeepCollectionEquality().hash(watchers),progress,isDirty,isSubmitting,const DeepCollectionEquality().hash(errors),submitError]);

@override
String toString() {
  return 'TaskFormState(title: $title, description: $description, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, tags: $tags, watchers: $watchers, progress: $progress, isDirty: $isDirty, isSubmitting: $isSubmitting, errors: $errors, submitError: $submitError)';
}


}

/// @nodoc
abstract mixin class $TaskFormStateCopyWith<$Res>  {
  factory $TaskFormStateCopyWith(TaskFormState value, $Res Function(TaskFormState) _then) = _$TaskFormStateCopyWithImpl;
@useResult
$Res call({
 String title, String? description, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, List<String> tags, List<String> watchers, double progress, bool isDirty, bool isSubmitting, Map<String, String> errors, String? submitError
});




}
/// @nodoc
class _$TaskFormStateCopyWithImpl<$Res>
    implements $TaskFormStateCopyWith<$Res> {
  _$TaskFormStateCopyWithImpl(this._self, this._then);

  final TaskFormState _self;
  final $Res Function(TaskFormState) _then;

/// Create a copy of TaskFormState
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? title = null,Object? description = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? tags = null,Object? watchers = null,Object? progress = null,Object? isDirty = null,Object? isSubmitting = null,Object? errors = null,Object? submitError = freezed,}) {
  return _then(_self.copyWith(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,taskTypeId: null == taskTypeId ? _self.taskTypeId : taskTypeId // ignore: cast_nullable_to_non_nullable
as String,workflowStageId: null == workflowStageId ? _self.workflowStageId : workflowStageId // ignore: cast_nullable_to_non_nullable
as String,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,reporterId: null == reporterId ? _self.reporterId : reporterId // ignore: cast_nullable_to_non_nullable
as String,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as TaskPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as TaskStatus,estimatedHours: freezed == estimatedHours ? _self.estimatedHours : estimatedHours // ignore: cast_nullable_to_non_nullable
as double?,actualHours: freezed == actualHours ? _self.actualHours : actualHours // ignore: cast_nullable_to_non_nullable
as double?,storyPoints: freezed == storyPoints ? _self.storyPoints : storyPoints // ignore: cast_nullable_to_non_nullable
as int?,startDate: freezed == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,watchers: null == watchers ? _self.watchers : watchers // ignore: cast_nullable_to_non_nullable
as List<String>,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,isDirty: null == isDirty ? _self.isDirty : isDirty // ignore: cast_nullable_to_non_nullable
as bool,isSubmitting: null == isSubmitting ? _self.isSubmitting : isSubmitting // ignore: cast_nullable_to_non_nullable
as bool,errors: null == errors ? _self.errors : errors // ignore: cast_nullable_to_non_nullable
as Map<String, String>,submitError: freezed == submitError ? _self.submitError : submitError // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskFormState].
extension TaskFormStatePatterns on TaskFormState {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskFormState value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskFormState() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskFormState value)  $default,){
final _that = this;
switch (_that) {
case _TaskFormState():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskFormState value)?  $default,){
final _that = this;
switch (_that) {
case _TaskFormState() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String title,  String? description,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  double progress,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskFormState() when $default != null:
return $default(_that.title,_that.description,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.progress,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String title,  String? description,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  double progress,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)  $default,) {final _that = this;
switch (_that) {
case _TaskFormState():
return $default(_that.title,_that.description,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.progress,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String title,  String? description,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  double progress,  bool isDirty,  bool isSubmitting,  Map<String, String> errors,  String? submitError)?  $default,) {final _that = this;
switch (_that) {
case _TaskFormState() when $default != null:
return $default(_that.title,_that.description,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.progress,_that.isDirty,_that.isSubmitting,_that.errors,_that.submitError);case _:
  return null;

}
}

}

/// @nodoc


class _TaskFormState implements TaskFormState {
  const _TaskFormState({this.title = '', this.description, this.taskTypeId = '', this.workflowStageId = '', this.assigneeId, this.reporterId = '', this.priority = TaskPriority.medium, this.status = TaskStatus.todo, this.estimatedHours, this.actualHours, this.storyPoints, this.startDate, this.dueDate, final  List<String> tags = const [], final  List<String> watchers = const [], this.progress = 0.0, this.isDirty = false, this.isSubmitting = false, final  Map<String, String> errors = const {}, this.submitError}): _tags = tags,_watchers = watchers,_errors = errors;
  

@override@JsonKey() final  String title;
@override final  String? description;
@override@JsonKey() final  String taskTypeId;
@override@JsonKey() final  String workflowStageId;
@override final  String? assigneeId;
@override@JsonKey() final  String reporterId;
@override@JsonKey() final  TaskPriority priority;
@override@JsonKey() final  TaskStatus status;
@override final  double? estimatedHours;
@override final  double? actualHours;
@override final  int? storyPoints;
@override final  DateTime? startDate;
@override final  DateTime? dueDate;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

 final  List<String> _watchers;
@override@JsonKey() List<String> get watchers {
  if (_watchers is EqualUnmodifiableListView) return _watchers;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_watchers);
}

@override@JsonKey() final  double progress;
@override@JsonKey() final  bool isDirty;
@override@JsonKey() final  bool isSubmitting;
 final  Map<String, String> _errors;
@override@JsonKey() Map<String, String> get errors {
  if (_errors is EqualUnmodifiableMapView) return _errors;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableMapView(_errors);
}

@override final  String? submitError;

/// Create a copy of TaskFormState
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskFormStateCopyWith<_TaskFormState> get copyWith => __$TaskFormStateCopyWithImpl<_TaskFormState>(this, _$identity);



@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskFormState&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&const DeepCollectionEquality().equals(other._tags, _tags)&&const DeepCollectionEquality().equals(other._watchers, _watchers)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.isDirty, isDirty) || other.isDirty == isDirty)&&(identical(other.isSubmitting, isSubmitting) || other.isSubmitting == isSubmitting)&&const DeepCollectionEquality().equals(other._errors, _errors)&&(identical(other.submitError, submitError) || other.submitError == submitError));
}


@override
int get hashCode => Object.hashAll([runtimeType,title,description,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,const DeepCollectionEquality().hash(_tags),const DeepCollectionEquality().hash(_watchers),progress,isDirty,isSubmitting,const DeepCollectionEquality().hash(_errors),submitError]);

@override
String toString() {
  return 'TaskFormState(title: $title, description: $description, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, tags: $tags, watchers: $watchers, progress: $progress, isDirty: $isDirty, isSubmitting: $isSubmitting, errors: $errors, submitError: $submitError)';
}


}

/// @nodoc
abstract mixin class _$TaskFormStateCopyWith<$Res> implements $TaskFormStateCopyWith<$Res> {
  factory _$TaskFormStateCopyWith(_TaskFormState value, $Res Function(_TaskFormState) _then) = __$TaskFormStateCopyWithImpl;
@override @useResult
$Res call({
 String title, String? description, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, List<String> tags, List<String> watchers, double progress, bool isDirty, bool isSubmitting, Map<String, String> errors, String? submitError
});




}
/// @nodoc
class __$TaskFormStateCopyWithImpl<$Res>
    implements _$TaskFormStateCopyWith<$Res> {
  __$TaskFormStateCopyWithImpl(this._self, this._then);

  final _TaskFormState _self;
  final $Res Function(_TaskFormState) _then;

/// Create a copy of TaskFormState
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? title = null,Object? description = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? tags = null,Object? watchers = null,Object? progress = null,Object? isDirty = null,Object? isSubmitting = null,Object? errors = null,Object? submitError = freezed,}) {
  return _then(_TaskFormState(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,taskTypeId: null == taskTypeId ? _self.taskTypeId : taskTypeId // ignore: cast_nullable_to_non_nullable
as String,workflowStageId: null == workflowStageId ? _self.workflowStageId : workflowStageId // ignore: cast_nullable_to_non_nullable
as String,assigneeId: freezed == assigneeId ? _self.assigneeId : assigneeId // ignore: cast_nullable_to_non_nullable
as String?,reporterId: null == reporterId ? _self.reporterId : reporterId // ignore: cast_nullable_to_non_nullable
as String,priority: null == priority ? _self.priority : priority // ignore: cast_nullable_to_non_nullable
as TaskPriority,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as TaskStatus,estimatedHours: freezed == estimatedHours ? _self.estimatedHours : estimatedHours // ignore: cast_nullable_to_non_nullable
as double?,actualHours: freezed == actualHours ? _self.actualHours : actualHours // ignore: cast_nullable_to_non_nullable
as double?,storyPoints: freezed == storyPoints ? _self.storyPoints : storyPoints // ignore: cast_nullable_to_non_nullable
as int?,startDate: freezed == startDate ? _self.startDate : startDate // ignore: cast_nullable_to_non_nullable
as DateTime?,dueDate: freezed == dueDate ? _self.dueDate : dueDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,watchers: null == watchers ? _self._watchers : watchers // ignore: cast_nullable_to_non_nullable
as List<String>,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,isDirty: null == isDirty ? _self.isDirty : isDirty // ignore: cast_nullable_to_non_nullable
as bool,isSubmitting: null == isSubmitting ? _self.isSubmitting : isSubmitting // ignore: cast_nullable_to_non_nullable
as bool,errors: null == errors ? _self._errors : errors // ignore: cast_nullable_to_non_nullable
as Map<String, String>,submitError: freezed == submitError ? _self.submitError : submitError // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
