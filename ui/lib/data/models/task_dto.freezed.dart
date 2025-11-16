// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskDto {

 String get id; String get title; String? get description; String? get parentTaskId; String? get projectId; String? get sprintId; String get taskTypeId; String get workflowStageId; String? get assigneeId; String get reporterId; TaskPriority get priority; TaskStatus get status; double? get estimatedHours; double? get actualHours; int? get storyPoints; DateTime? get startDate; DateTime? get dueDate; DateTime? get completedDate; List<String> get tags; List<String> get watchers; bool get isRecurring; double get progress; DateTime get creationTime; DateTime? get lastModificationTime;
/// Create a copy of TaskDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskDtoCopyWith<TaskDto> get copyWith => _$TaskDtoCopyWithImpl<TaskDto>(this as TaskDto, _$identity);

  /// Serializes this TaskDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentTaskId, parentTaskId) || other.parentTaskId == parentTaskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.sprintId, sprintId) || other.sprintId == sprintId)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&const DeepCollectionEquality().equals(other.tags, tags)&&const DeepCollectionEquality().equals(other.watchers, watchers)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,title,description,parentTaskId,projectId,sprintId,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,completedDate,const DeepCollectionEquality().hash(tags),const DeepCollectionEquality().hash(watchers),isRecurring,progress,creationTime,lastModificationTime]);

@override
String toString() {
  return 'TaskDto(id: $id, title: $title, description: $description, parentTaskId: $parentTaskId, projectId: $projectId, sprintId: $sprintId, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, completedDate: $completedDate, tags: $tags, watchers: $watchers, isRecurring: $isRecurring, progress: $progress, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class $TaskDtoCopyWith<$Res>  {
  factory $TaskDtoCopyWith(TaskDto value, $Res Function(TaskDto) _then) = _$TaskDtoCopyWithImpl;
@useResult
$Res call({
 String id, String title, String? description, String? parentTaskId, String? projectId, String? sprintId, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, DateTime? completedDate, List<String> tags, List<String> watchers, bool isRecurring, double progress, DateTime creationTime, DateTime? lastModificationTime
});




}
/// @nodoc
class _$TaskDtoCopyWithImpl<$Res>
    implements $TaskDtoCopyWith<$Res> {
  _$TaskDtoCopyWithImpl(this._self, this._then);

  final TaskDto _self;
  final $Res Function(TaskDto) _then;

/// Create a copy of TaskDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? title = null,Object? description = freezed,Object? parentTaskId = freezed,Object? projectId = freezed,Object? sprintId = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? completedDate = freezed,Object? tags = null,Object? watchers = null,Object? isRecurring = null,Object? progress = null,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentTaskId: freezed == parentTaskId ? _self.parentTaskId : parentTaskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,sprintId: freezed == sprintId ? _self.sprintId : sprintId // ignore: cast_nullable_to_non_nullable
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
as DateTime?,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,watchers: null == watchers ? _self.watchers : watchers // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskDto].
extension TaskDtoPatterns on TaskDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  DateTime? completedDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.completedDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  DateTime? completedDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress,  DateTime creationTime,  DateTime? lastModificationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskDto():
return $default(_that.id,_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.completedDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  DateTime? completedDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.completedDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress,_that.creationTime,_that.lastModificationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskDto implements TaskDto {
  const _TaskDto({required this.id, required this.title, this.description, this.parentTaskId, this.projectId, this.sprintId, required this.taskTypeId, required this.workflowStageId, this.assigneeId, required this.reporterId, required this.priority, required this.status, this.estimatedHours, this.actualHours, this.storyPoints, this.startDate, this.dueDate, this.completedDate, final  List<String> tags = const [], final  List<String> watchers = const [], this.isRecurring = false, this.progress = 0.0, required this.creationTime, this.lastModificationTime}): _tags = tags,_watchers = watchers;
  factory _TaskDto.fromJson(Map<String, dynamic> json) => _$TaskDtoFromJson(json);

@override final  String id;
@override final  String title;
@override final  String? description;
@override final  String? parentTaskId;
@override final  String? projectId;
@override final  String? sprintId;
@override final  String taskTypeId;
@override final  String workflowStageId;
@override final  String? assigneeId;
@override final  String reporterId;
@override final  TaskPriority priority;
@override final  TaskStatus status;
@override final  double? estimatedHours;
@override final  double? actualHours;
@override final  int? storyPoints;
@override final  DateTime? startDate;
@override final  DateTime? dueDate;
@override final  DateTime? completedDate;
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

@override@JsonKey() final  bool isRecurring;
@override@JsonKey() final  double progress;
@override final  DateTime creationTime;
@override final  DateTime? lastModificationTime;

/// Create a copy of TaskDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskDtoCopyWith<_TaskDto> get copyWith => __$TaskDtoCopyWithImpl<_TaskDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentTaskId, parentTaskId) || other.parentTaskId == parentTaskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.sprintId, sprintId) || other.sprintId == sprintId)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&const DeepCollectionEquality().equals(other._tags, _tags)&&const DeepCollectionEquality().equals(other._watchers, _watchers)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,title,description,parentTaskId,projectId,sprintId,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,completedDate,const DeepCollectionEquality().hash(_tags),const DeepCollectionEquality().hash(_watchers),isRecurring,progress,creationTime,lastModificationTime]);

@override
String toString() {
  return 'TaskDto(id: $id, title: $title, description: $description, parentTaskId: $parentTaskId, projectId: $projectId, sprintId: $sprintId, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, completedDate: $completedDate, tags: $tags, watchers: $watchers, isRecurring: $isRecurring, progress: $progress, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskDtoCopyWith<$Res> implements $TaskDtoCopyWith<$Res> {
  factory _$TaskDtoCopyWith(_TaskDto value, $Res Function(_TaskDto) _then) = __$TaskDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String title, String? description, String? parentTaskId, String? projectId, String? sprintId, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, DateTime? completedDate, List<String> tags, List<String> watchers, bool isRecurring, double progress, DateTime creationTime, DateTime? lastModificationTime
});




}
/// @nodoc
class __$TaskDtoCopyWithImpl<$Res>
    implements _$TaskDtoCopyWith<$Res> {
  __$TaskDtoCopyWithImpl(this._self, this._then);

  final _TaskDto _self;
  final $Res Function(_TaskDto) _then;

/// Create a copy of TaskDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? title = null,Object? description = freezed,Object? parentTaskId = freezed,Object? projectId = freezed,Object? sprintId = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? completedDate = freezed,Object? tags = null,Object? watchers = null,Object? isRecurring = null,Object? progress = null,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_TaskDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentTaskId: freezed == parentTaskId ? _self.parentTaskId : parentTaskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,sprintId: freezed == sprintId ? _self.sprintId : sprintId // ignore: cast_nullable_to_non_nullable
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
as DateTime?,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,watchers: null == watchers ? _self._watchers : watchers // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateTaskDto {

 String get title; String? get description; String? get parentTaskId; String? get projectId; String? get sprintId; String get taskTypeId; String get workflowStageId; String? get assigneeId; String get reporterId; TaskPriority get priority; TaskStatus get status; double? get estimatedHours; double? get actualHours; int? get storyPoints; DateTime? get startDate; DateTime? get dueDate; List<String> get tags; List<String> get watchers; bool get isRecurring; double get progress;
/// Create a copy of CreateUpdateTaskDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateTaskDtoCopyWith<CreateUpdateTaskDto> get copyWith => _$CreateUpdateTaskDtoCopyWithImpl<CreateUpdateTaskDto>(this as CreateUpdateTaskDto, _$identity);

  /// Serializes this CreateUpdateTaskDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateTaskDto&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentTaskId, parentTaskId) || other.parentTaskId == parentTaskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.sprintId, sprintId) || other.sprintId == sprintId)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&const DeepCollectionEquality().equals(other.tags, tags)&&const DeepCollectionEquality().equals(other.watchers, watchers)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.progress, progress) || other.progress == progress));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,title,description,parentTaskId,projectId,sprintId,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,const DeepCollectionEquality().hash(tags),const DeepCollectionEquality().hash(watchers),isRecurring,progress]);

@override
String toString() {
  return 'CreateUpdateTaskDto(title: $title, description: $description, parentTaskId: $parentTaskId, projectId: $projectId, sprintId: $sprintId, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, tags: $tags, watchers: $watchers, isRecurring: $isRecurring, progress: $progress)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateTaskDtoCopyWith<$Res>  {
  factory $CreateUpdateTaskDtoCopyWith(CreateUpdateTaskDto value, $Res Function(CreateUpdateTaskDto) _then) = _$CreateUpdateTaskDtoCopyWithImpl;
@useResult
$Res call({
 String title, String? description, String? parentTaskId, String? projectId, String? sprintId, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, List<String> tags, List<String> watchers, bool isRecurring, double progress
});




}
/// @nodoc
class _$CreateUpdateTaskDtoCopyWithImpl<$Res>
    implements $CreateUpdateTaskDtoCopyWith<$Res> {
  _$CreateUpdateTaskDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateTaskDto _self;
  final $Res Function(CreateUpdateTaskDto) _then;

/// Create a copy of CreateUpdateTaskDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? title = null,Object? description = freezed,Object? parentTaskId = freezed,Object? projectId = freezed,Object? sprintId = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? tags = null,Object? watchers = null,Object? isRecurring = null,Object? progress = null,}) {
  return _then(_self.copyWith(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentTaskId: freezed == parentTaskId ? _self.parentTaskId : parentTaskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,sprintId: freezed == sprintId ? _self.sprintId : sprintId // ignore: cast_nullable_to_non_nullable
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
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateTaskDto].
extension CreateUpdateTaskDtoPatterns on CreateUpdateTaskDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateTaskDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateTaskDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateTaskDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateTaskDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateTaskDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateTaskDto() when $default != null:
return $default(_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskDto():
return $default(_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String title,  String? description,  String? parentTaskId,  String? projectId,  String? sprintId,  String taskTypeId,  String workflowStageId,  String? assigneeId,  String reporterId,  TaskPriority priority,  TaskStatus status,  double? estimatedHours,  double? actualHours,  int? storyPoints,  DateTime? startDate,  DateTime? dueDate,  List<String> tags,  List<String> watchers,  bool isRecurring,  double progress)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateTaskDto() when $default != null:
return $default(_that.title,_that.description,_that.parentTaskId,_that.projectId,_that.sprintId,_that.taskTypeId,_that.workflowStageId,_that.assigneeId,_that.reporterId,_that.priority,_that.status,_that.estimatedHours,_that.actualHours,_that.storyPoints,_that.startDate,_that.dueDate,_that.tags,_that.watchers,_that.isRecurring,_that.progress);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateTaskDto implements CreateUpdateTaskDto {
  const _CreateUpdateTaskDto({required this.title, this.description, this.parentTaskId, this.projectId, this.sprintId, required this.taskTypeId, required this.workflowStageId, this.assigneeId, required this.reporterId, required this.priority, required this.status, this.estimatedHours, this.actualHours, this.storyPoints, this.startDate, this.dueDate, final  List<String> tags = const [], final  List<String> watchers = const [], this.isRecurring = false, this.progress = 0.0}): _tags = tags,_watchers = watchers;
  factory _CreateUpdateTaskDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateTaskDtoFromJson(json);

@override final  String title;
@override final  String? description;
@override final  String? parentTaskId;
@override final  String? projectId;
@override final  String? sprintId;
@override final  String taskTypeId;
@override final  String workflowStageId;
@override final  String? assigneeId;
@override final  String reporterId;
@override final  TaskPriority priority;
@override final  TaskStatus status;
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

@override@JsonKey() final  bool isRecurring;
@override@JsonKey() final  double progress;

/// Create a copy of CreateUpdateTaskDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateTaskDtoCopyWith<_CreateUpdateTaskDto> get copyWith => __$CreateUpdateTaskDtoCopyWithImpl<_CreateUpdateTaskDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateTaskDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateTaskDto&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.parentTaskId, parentTaskId) || other.parentTaskId == parentTaskId)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.sprintId, sprintId) || other.sprintId == sprintId)&&(identical(other.taskTypeId, taskTypeId) || other.taskTypeId == taskTypeId)&&(identical(other.workflowStageId, workflowStageId) || other.workflowStageId == workflowStageId)&&(identical(other.assigneeId, assigneeId) || other.assigneeId == assigneeId)&&(identical(other.reporterId, reporterId) || other.reporterId == reporterId)&&(identical(other.priority, priority) || other.priority == priority)&&(identical(other.status, status) || other.status == status)&&(identical(other.estimatedHours, estimatedHours) || other.estimatedHours == estimatedHours)&&(identical(other.actualHours, actualHours) || other.actualHours == actualHours)&&(identical(other.storyPoints, storyPoints) || other.storyPoints == storyPoints)&&(identical(other.startDate, startDate) || other.startDate == startDate)&&(identical(other.dueDate, dueDate) || other.dueDate == dueDate)&&const DeepCollectionEquality().equals(other._tags, _tags)&&const DeepCollectionEquality().equals(other._watchers, _watchers)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.progress, progress) || other.progress == progress));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,title,description,parentTaskId,projectId,sprintId,taskTypeId,workflowStageId,assigneeId,reporterId,priority,status,estimatedHours,actualHours,storyPoints,startDate,dueDate,const DeepCollectionEquality().hash(_tags),const DeepCollectionEquality().hash(_watchers),isRecurring,progress]);

@override
String toString() {
  return 'CreateUpdateTaskDto(title: $title, description: $description, parentTaskId: $parentTaskId, projectId: $projectId, sprintId: $sprintId, taskTypeId: $taskTypeId, workflowStageId: $workflowStageId, assigneeId: $assigneeId, reporterId: $reporterId, priority: $priority, status: $status, estimatedHours: $estimatedHours, actualHours: $actualHours, storyPoints: $storyPoints, startDate: $startDate, dueDate: $dueDate, tags: $tags, watchers: $watchers, isRecurring: $isRecurring, progress: $progress)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateTaskDtoCopyWith<$Res> implements $CreateUpdateTaskDtoCopyWith<$Res> {
  factory _$CreateUpdateTaskDtoCopyWith(_CreateUpdateTaskDto value, $Res Function(_CreateUpdateTaskDto) _then) = __$CreateUpdateTaskDtoCopyWithImpl;
@override @useResult
$Res call({
 String title, String? description, String? parentTaskId, String? projectId, String? sprintId, String taskTypeId, String workflowStageId, String? assigneeId, String reporterId, TaskPriority priority, TaskStatus status, double? estimatedHours, double? actualHours, int? storyPoints, DateTime? startDate, DateTime? dueDate, List<String> tags, List<String> watchers, bool isRecurring, double progress
});




}
/// @nodoc
class __$CreateUpdateTaskDtoCopyWithImpl<$Res>
    implements _$CreateUpdateTaskDtoCopyWith<$Res> {
  __$CreateUpdateTaskDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateTaskDto _self;
  final $Res Function(_CreateUpdateTaskDto) _then;

/// Create a copy of CreateUpdateTaskDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? title = null,Object? description = freezed,Object? parentTaskId = freezed,Object? projectId = freezed,Object? sprintId = freezed,Object? taskTypeId = null,Object? workflowStageId = null,Object? assigneeId = freezed,Object? reporterId = null,Object? priority = null,Object? status = null,Object? estimatedHours = freezed,Object? actualHours = freezed,Object? storyPoints = freezed,Object? startDate = freezed,Object? dueDate = freezed,Object? tags = null,Object? watchers = null,Object? isRecurring = null,Object? progress = null,}) {
  return _then(_CreateUpdateTaskDto(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,parentTaskId: freezed == parentTaskId ? _self.parentTaskId : parentTaskId // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,sprintId: freezed == sprintId ? _self.sprintId : sprintId // ignore: cast_nullable_to_non_nullable
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
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,
  ));
}


}


/// @nodoc
mixin _$TaskTypeDto {

 String get id; String get name; String get icon; String get color; String? get description;
/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskTypeDtoCopyWith<TaskTypeDto> get copyWith => _$TaskTypeDtoCopyWithImpl<TaskTypeDto>(this as TaskTypeDto, _$identity);

  /// Serializes this TaskTypeDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskTypeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,icon,color,description);

@override
String toString() {
  return 'TaskTypeDto(id: $id, name: $name, icon: $icon, color: $color, description: $description)';
}


}

/// @nodoc
abstract mixin class $TaskTypeDtoCopyWith<$Res>  {
  factory $TaskTypeDtoCopyWith(TaskTypeDto value, $Res Function(TaskTypeDto) _then) = _$TaskTypeDtoCopyWithImpl;
@useResult
$Res call({
 String id, String name, String icon, String color, String? description
});




}
/// @nodoc
class _$TaskTypeDtoCopyWithImpl<$Res>
    implements $TaskTypeDtoCopyWith<$Res> {
  _$TaskTypeDtoCopyWithImpl(this._self, this._then);

  final TaskTypeDto _self;
  final $Res Function(TaskTypeDto) _then;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskTypeDto].
extension TaskTypeDtoPatterns on TaskTypeDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskTypeDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskTypeDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskTypeDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskTypeDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String name,  String icon,  String color,  String? description)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String name,  String icon,  String color,  String? description)  $default,) {final _that = this;
switch (_that) {
case _TaskTypeDto():
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String name,  String icon,  String color,  String? description)?  $default,) {final _that = this;
switch (_that) {
case _TaskTypeDto() when $default != null:
return $default(_that.id,_that.name,_that.icon,_that.color,_that.description);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskTypeDto implements TaskTypeDto {
  const _TaskTypeDto({required this.id, required this.name, required this.icon, required this.color, this.description});
  factory _TaskTypeDto.fromJson(Map<String, dynamic> json) => _$TaskTypeDtoFromJson(json);

@override final  String id;
@override final  String name;
@override final  String icon;
@override final  String color;
@override final  String? description;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskTypeDtoCopyWith<_TaskTypeDto> get copyWith => __$TaskTypeDtoCopyWithImpl<_TaskTypeDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskTypeDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskTypeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.name, name) || other.name == name)&&(identical(other.icon, icon) || other.icon == icon)&&(identical(other.color, color) || other.color == color)&&(identical(other.description, description) || other.description == description));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,name,icon,color,description);

@override
String toString() {
  return 'TaskTypeDto(id: $id, name: $name, icon: $icon, color: $color, description: $description)';
}


}

/// @nodoc
abstract mixin class _$TaskTypeDtoCopyWith<$Res> implements $TaskTypeDtoCopyWith<$Res> {
  factory _$TaskTypeDtoCopyWith(_TaskTypeDto value, $Res Function(_TaskTypeDto) _then) = __$TaskTypeDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String name, String icon, String color, String? description
});




}
/// @nodoc
class __$TaskTypeDtoCopyWithImpl<$Res>
    implements _$TaskTypeDtoCopyWith<$Res> {
  __$TaskTypeDtoCopyWithImpl(this._self, this._then);

  final _TaskTypeDto _self;
  final $Res Function(_TaskTypeDto) _then;

/// Create a copy of TaskTypeDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? name = null,Object? icon = null,Object? color = null,Object? description = freezed,}) {
  return _then(_TaskTypeDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,name: null == name ? _self.name : name // ignore: cast_nullable_to_non_nullable
as String,icon: null == icon ? _self.icon : icon // ignore: cast_nullable_to_non_nullable
as String,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
