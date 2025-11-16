// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'learning_progress_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$CourseProgressDto {

 String get id; String get userId; String get courseId; String get courseTitle; ProgressStatus get status; double get completionPercentage; int get completedLessons; int get totalLessons; int get timeSpent; DateTime? get enrollmentDate; DateTime? get completionDate; DateTime? get lastAccessedDate; String? get certificateId;
/// Create a copy of CourseProgressDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CourseProgressDtoCopyWith<CourseProgressDto> get copyWith => _$CourseProgressDtoCopyWithImpl<CourseProgressDto>(this as CourseProgressDto, _$identity);

  /// Serializes this CourseProgressDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CourseProgressDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.courseTitle, courseTitle) || other.courseTitle == courseTitle)&&(identical(other.status, status) || other.status == status)&&(identical(other.completionPercentage, completionPercentage) || other.completionPercentage == completionPercentage)&&(identical(other.completedLessons, completedLessons) || other.completedLessons == completedLessons)&&(identical(other.totalLessons, totalLessons) || other.totalLessons == totalLessons)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent)&&(identical(other.enrollmentDate, enrollmentDate) || other.enrollmentDate == enrollmentDate)&&(identical(other.completionDate, completionDate) || other.completionDate == completionDate)&&(identical(other.lastAccessedDate, lastAccessedDate) || other.lastAccessedDate == lastAccessedDate)&&(identical(other.certificateId, certificateId) || other.certificateId == certificateId));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,courseId,courseTitle,status,completionPercentage,completedLessons,totalLessons,timeSpent,enrollmentDate,completionDate,lastAccessedDate,certificateId);

@override
String toString() {
  return 'CourseProgressDto(id: $id, userId: $userId, courseId: $courseId, courseTitle: $courseTitle, status: $status, completionPercentage: $completionPercentage, completedLessons: $completedLessons, totalLessons: $totalLessons, timeSpent: $timeSpent, enrollmentDate: $enrollmentDate, completionDate: $completionDate, lastAccessedDate: $lastAccessedDate, certificateId: $certificateId)';
}


}

/// @nodoc
abstract mixin class $CourseProgressDtoCopyWith<$Res>  {
  factory $CourseProgressDtoCopyWith(CourseProgressDto value, $Res Function(CourseProgressDto) _then) = _$CourseProgressDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String courseId, String courseTitle, ProgressStatus status, double completionPercentage, int completedLessons, int totalLessons, int timeSpent, DateTime? enrollmentDate, DateTime? completionDate, DateTime? lastAccessedDate, String? certificateId
});




}
/// @nodoc
class _$CourseProgressDtoCopyWithImpl<$Res>
    implements $CourseProgressDtoCopyWith<$Res> {
  _$CourseProgressDtoCopyWithImpl(this._self, this._then);

  final CourseProgressDto _self;
  final $Res Function(CourseProgressDto) _then;

/// Create a copy of CourseProgressDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? courseId = null,Object? courseTitle = null,Object? status = null,Object? completionPercentage = null,Object? completedLessons = null,Object? totalLessons = null,Object? timeSpent = null,Object? enrollmentDate = freezed,Object? completionDate = freezed,Object? lastAccessedDate = freezed,Object? certificateId = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,courseTitle: null == courseTitle ? _self.courseTitle : courseTitle // ignore: cast_nullable_to_non_nullable
as String,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProgressStatus,completionPercentage: null == completionPercentage ? _self.completionPercentage : completionPercentage // ignore: cast_nullable_to_non_nullable
as double,completedLessons: null == completedLessons ? _self.completedLessons : completedLessons // ignore: cast_nullable_to_non_nullable
as int,totalLessons: null == totalLessons ? _self.totalLessons : totalLessons // ignore: cast_nullable_to_non_nullable
as int,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,enrollmentDate: freezed == enrollmentDate ? _self.enrollmentDate : enrollmentDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completionDate: freezed == completionDate ? _self.completionDate : completionDate // ignore: cast_nullable_to_non_nullable
as DateTime?,lastAccessedDate: freezed == lastAccessedDate ? _self.lastAccessedDate : lastAccessedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,certificateId: freezed == certificateId ? _self.certificateId : certificateId // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CourseProgressDto].
extension CourseProgressDtoPatterns on CourseProgressDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CourseProgressDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CourseProgressDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CourseProgressDto value)  $default,){
final _that = this;
switch (_that) {
case _CourseProgressDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CourseProgressDto value)?  $default,){
final _that = this;
switch (_that) {
case _CourseProgressDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String courseId,  String courseTitle,  ProgressStatus status,  double completionPercentage,  int completedLessons,  int totalLessons,  int timeSpent,  DateTime? enrollmentDate,  DateTime? completionDate,  DateTime? lastAccessedDate,  String? certificateId)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CourseProgressDto() when $default != null:
return $default(_that.id,_that.userId,_that.courseId,_that.courseTitle,_that.status,_that.completionPercentage,_that.completedLessons,_that.totalLessons,_that.timeSpent,_that.enrollmentDate,_that.completionDate,_that.lastAccessedDate,_that.certificateId);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String courseId,  String courseTitle,  ProgressStatus status,  double completionPercentage,  int completedLessons,  int totalLessons,  int timeSpent,  DateTime? enrollmentDate,  DateTime? completionDate,  DateTime? lastAccessedDate,  String? certificateId)  $default,) {final _that = this;
switch (_that) {
case _CourseProgressDto():
return $default(_that.id,_that.userId,_that.courseId,_that.courseTitle,_that.status,_that.completionPercentage,_that.completedLessons,_that.totalLessons,_that.timeSpent,_that.enrollmentDate,_that.completionDate,_that.lastAccessedDate,_that.certificateId);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String courseId,  String courseTitle,  ProgressStatus status,  double completionPercentage,  int completedLessons,  int totalLessons,  int timeSpent,  DateTime? enrollmentDate,  DateTime? completionDate,  DateTime? lastAccessedDate,  String? certificateId)?  $default,) {final _that = this;
switch (_that) {
case _CourseProgressDto() when $default != null:
return $default(_that.id,_that.userId,_that.courseId,_that.courseTitle,_that.status,_that.completionPercentage,_that.completedLessons,_that.totalLessons,_that.timeSpent,_that.enrollmentDate,_that.completionDate,_that.lastAccessedDate,_that.certificateId);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CourseProgressDto implements CourseProgressDto {
  const _CourseProgressDto({required this.id, required this.userId, required this.courseId, required this.courseTitle, required this.status, required this.completionPercentage, required this.completedLessons, required this.totalLessons, required this.timeSpent, this.enrollmentDate, this.completionDate, this.lastAccessedDate, this.certificateId});
  factory _CourseProgressDto.fromJson(Map<String, dynamic> json) => _$CourseProgressDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String courseId;
@override final  String courseTitle;
@override final  ProgressStatus status;
@override final  double completionPercentage;
@override final  int completedLessons;
@override final  int totalLessons;
@override final  int timeSpent;
@override final  DateTime? enrollmentDate;
@override final  DateTime? completionDate;
@override final  DateTime? lastAccessedDate;
@override final  String? certificateId;

/// Create a copy of CourseProgressDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CourseProgressDtoCopyWith<_CourseProgressDto> get copyWith => __$CourseProgressDtoCopyWithImpl<_CourseProgressDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CourseProgressDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CourseProgressDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.courseTitle, courseTitle) || other.courseTitle == courseTitle)&&(identical(other.status, status) || other.status == status)&&(identical(other.completionPercentage, completionPercentage) || other.completionPercentage == completionPercentage)&&(identical(other.completedLessons, completedLessons) || other.completedLessons == completedLessons)&&(identical(other.totalLessons, totalLessons) || other.totalLessons == totalLessons)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent)&&(identical(other.enrollmentDate, enrollmentDate) || other.enrollmentDate == enrollmentDate)&&(identical(other.completionDate, completionDate) || other.completionDate == completionDate)&&(identical(other.lastAccessedDate, lastAccessedDate) || other.lastAccessedDate == lastAccessedDate)&&(identical(other.certificateId, certificateId) || other.certificateId == certificateId));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,courseId,courseTitle,status,completionPercentage,completedLessons,totalLessons,timeSpent,enrollmentDate,completionDate,lastAccessedDate,certificateId);

@override
String toString() {
  return 'CourseProgressDto(id: $id, userId: $userId, courseId: $courseId, courseTitle: $courseTitle, status: $status, completionPercentage: $completionPercentage, completedLessons: $completedLessons, totalLessons: $totalLessons, timeSpent: $timeSpent, enrollmentDate: $enrollmentDate, completionDate: $completionDate, lastAccessedDate: $lastAccessedDate, certificateId: $certificateId)';
}


}

/// @nodoc
abstract mixin class _$CourseProgressDtoCopyWith<$Res> implements $CourseProgressDtoCopyWith<$Res> {
  factory _$CourseProgressDtoCopyWith(_CourseProgressDto value, $Res Function(_CourseProgressDto) _then) = __$CourseProgressDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String courseId, String courseTitle, ProgressStatus status, double completionPercentage, int completedLessons, int totalLessons, int timeSpent, DateTime? enrollmentDate, DateTime? completionDate, DateTime? lastAccessedDate, String? certificateId
});




}
/// @nodoc
class __$CourseProgressDtoCopyWithImpl<$Res>
    implements _$CourseProgressDtoCopyWith<$Res> {
  __$CourseProgressDtoCopyWithImpl(this._self, this._then);

  final _CourseProgressDto _self;
  final $Res Function(_CourseProgressDto) _then;

/// Create a copy of CourseProgressDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? courseId = null,Object? courseTitle = null,Object? status = null,Object? completionPercentage = null,Object? completedLessons = null,Object? totalLessons = null,Object? timeSpent = null,Object? enrollmentDate = freezed,Object? completionDate = freezed,Object? lastAccessedDate = freezed,Object? certificateId = freezed,}) {
  return _then(_CourseProgressDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,courseTitle: null == courseTitle ? _self.courseTitle : courseTitle // ignore: cast_nullable_to_non_nullable
as String,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as ProgressStatus,completionPercentage: null == completionPercentage ? _self.completionPercentage : completionPercentage // ignore: cast_nullable_to_non_nullable
as double,completedLessons: null == completedLessons ? _self.completedLessons : completedLessons // ignore: cast_nullable_to_non_nullable
as int,totalLessons: null == totalLessons ? _self.totalLessons : totalLessons // ignore: cast_nullable_to_non_nullable
as int,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,enrollmentDate: freezed == enrollmentDate ? _self.enrollmentDate : enrollmentDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completionDate: freezed == completionDate ? _self.completionDate : completionDate // ignore: cast_nullable_to_non_nullable
as DateTime?,lastAccessedDate: freezed == lastAccessedDate ? _self.lastAccessedDate : lastAccessedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,certificateId: freezed == certificateId ? _self.certificateId : certificateId // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}


/// @nodoc
mixin _$LessonProgressDto {

 String get id; String get userId; String get lessonId; String get courseId; bool get isCompleted; int get timeSpent; int? get videoProgress; DateTime? get startedDate; DateTime? get completedDate; DateTime? get lastAccessedDate;
/// Create a copy of LessonProgressDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$LessonProgressDtoCopyWith<LessonProgressDto> get copyWith => _$LessonProgressDtoCopyWithImpl<LessonProgressDto>(this as LessonProgressDto, _$identity);

  /// Serializes this LessonProgressDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is LessonProgressDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.lessonId, lessonId) || other.lessonId == lessonId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent)&&(identical(other.videoProgress, videoProgress) || other.videoProgress == videoProgress)&&(identical(other.startedDate, startedDate) || other.startedDate == startedDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.lastAccessedDate, lastAccessedDate) || other.lastAccessedDate == lastAccessedDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,lessonId,courseId,isCompleted,timeSpent,videoProgress,startedDate,completedDate,lastAccessedDate);

@override
String toString() {
  return 'LessonProgressDto(id: $id, userId: $userId, lessonId: $lessonId, courseId: $courseId, isCompleted: $isCompleted, timeSpent: $timeSpent, videoProgress: $videoProgress, startedDate: $startedDate, completedDate: $completedDate, lastAccessedDate: $lastAccessedDate)';
}


}

/// @nodoc
abstract mixin class $LessonProgressDtoCopyWith<$Res>  {
  factory $LessonProgressDtoCopyWith(LessonProgressDto value, $Res Function(LessonProgressDto) _then) = _$LessonProgressDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String lessonId, String courseId, bool isCompleted, int timeSpent, int? videoProgress, DateTime? startedDate, DateTime? completedDate, DateTime? lastAccessedDate
});




}
/// @nodoc
class _$LessonProgressDtoCopyWithImpl<$Res>
    implements $LessonProgressDtoCopyWith<$Res> {
  _$LessonProgressDtoCopyWithImpl(this._self, this._then);

  final LessonProgressDto _self;
  final $Res Function(LessonProgressDto) _then;

/// Create a copy of LessonProgressDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? lessonId = null,Object? courseId = null,Object? isCompleted = null,Object? timeSpent = null,Object? videoProgress = freezed,Object? startedDate = freezed,Object? completedDate = freezed,Object? lastAccessedDate = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,lessonId: null == lessonId ? _self.lessonId : lessonId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,videoProgress: freezed == videoProgress ? _self.videoProgress : videoProgress // ignore: cast_nullable_to_non_nullable
as int?,startedDate: freezed == startedDate ? _self.startedDate : startedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,lastAccessedDate: freezed == lastAccessedDate ? _self.lastAccessedDate : lastAccessedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [LessonProgressDto].
extension LessonProgressDtoPatterns on LessonProgressDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _LessonProgressDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _LessonProgressDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _LessonProgressDto value)  $default,){
final _that = this;
switch (_that) {
case _LessonProgressDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _LessonProgressDto value)?  $default,){
final _that = this;
switch (_that) {
case _LessonProgressDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String lessonId,  String courseId,  bool isCompleted,  int timeSpent,  int? videoProgress,  DateTime? startedDate,  DateTime? completedDate,  DateTime? lastAccessedDate)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _LessonProgressDto() when $default != null:
return $default(_that.id,_that.userId,_that.lessonId,_that.courseId,_that.isCompleted,_that.timeSpent,_that.videoProgress,_that.startedDate,_that.completedDate,_that.lastAccessedDate);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String lessonId,  String courseId,  bool isCompleted,  int timeSpent,  int? videoProgress,  DateTime? startedDate,  DateTime? completedDate,  DateTime? lastAccessedDate)  $default,) {final _that = this;
switch (_that) {
case _LessonProgressDto():
return $default(_that.id,_that.userId,_that.lessonId,_that.courseId,_that.isCompleted,_that.timeSpent,_that.videoProgress,_that.startedDate,_that.completedDate,_that.lastAccessedDate);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String lessonId,  String courseId,  bool isCompleted,  int timeSpent,  int? videoProgress,  DateTime? startedDate,  DateTime? completedDate,  DateTime? lastAccessedDate)?  $default,) {final _that = this;
switch (_that) {
case _LessonProgressDto() when $default != null:
return $default(_that.id,_that.userId,_that.lessonId,_that.courseId,_that.isCompleted,_that.timeSpent,_that.videoProgress,_that.startedDate,_that.completedDate,_that.lastAccessedDate);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _LessonProgressDto implements LessonProgressDto {
  const _LessonProgressDto({required this.id, required this.userId, required this.lessonId, required this.courseId, this.isCompleted = false, required this.timeSpent, this.videoProgress, this.startedDate, this.completedDate, this.lastAccessedDate});
  factory _LessonProgressDto.fromJson(Map<String, dynamic> json) => _$LessonProgressDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String lessonId;
@override final  String courseId;
@override@JsonKey() final  bool isCompleted;
@override final  int timeSpent;
@override final  int? videoProgress;
@override final  DateTime? startedDate;
@override final  DateTime? completedDate;
@override final  DateTime? lastAccessedDate;

/// Create a copy of LessonProgressDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$LessonProgressDtoCopyWith<_LessonProgressDto> get copyWith => __$LessonProgressDtoCopyWithImpl<_LessonProgressDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$LessonProgressDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _LessonProgressDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.lessonId, lessonId) || other.lessonId == lessonId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent)&&(identical(other.videoProgress, videoProgress) || other.videoProgress == videoProgress)&&(identical(other.startedDate, startedDate) || other.startedDate == startedDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.lastAccessedDate, lastAccessedDate) || other.lastAccessedDate == lastAccessedDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,lessonId,courseId,isCompleted,timeSpent,videoProgress,startedDate,completedDate,lastAccessedDate);

@override
String toString() {
  return 'LessonProgressDto(id: $id, userId: $userId, lessonId: $lessonId, courseId: $courseId, isCompleted: $isCompleted, timeSpent: $timeSpent, videoProgress: $videoProgress, startedDate: $startedDate, completedDate: $completedDate, lastAccessedDate: $lastAccessedDate)';
}


}

/// @nodoc
abstract mixin class _$LessonProgressDtoCopyWith<$Res> implements $LessonProgressDtoCopyWith<$Res> {
  factory _$LessonProgressDtoCopyWith(_LessonProgressDto value, $Res Function(_LessonProgressDto) _then) = __$LessonProgressDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String lessonId, String courseId, bool isCompleted, int timeSpent, int? videoProgress, DateTime? startedDate, DateTime? completedDate, DateTime? lastAccessedDate
});




}
/// @nodoc
class __$LessonProgressDtoCopyWithImpl<$Res>
    implements _$LessonProgressDtoCopyWith<$Res> {
  __$LessonProgressDtoCopyWithImpl(this._self, this._then);

  final _LessonProgressDto _self;
  final $Res Function(_LessonProgressDto) _then;

/// Create a copy of LessonProgressDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? lessonId = null,Object? courseId = null,Object? isCompleted = null,Object? timeSpent = null,Object? videoProgress = freezed,Object? startedDate = freezed,Object? completedDate = freezed,Object? lastAccessedDate = freezed,}) {
  return _then(_LessonProgressDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,lessonId: null == lessonId ? _self.lessonId : lessonId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,videoProgress: freezed == videoProgress ? _self.videoProgress : videoProgress // ignore: cast_nullable_to_non_nullable
as int?,startedDate: freezed == startedDate ? _self.startedDate : startedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,lastAccessedDate: freezed == lastAccessedDate ? _self.lastAccessedDate : lastAccessedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}

// dart format on
