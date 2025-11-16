// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'lesson_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$LessonDto {

 String get id; String get courseId; String get title; String get description; LessonType get lessonType; int get order; int get duration; String? get videoUrl; String? get content; List<String> get attachments; String? get quizId; bool get isFree; bool get isCompleted; DateTime get creationTime;
/// Create a copy of LessonDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$LessonDtoCopyWith<LessonDto> get copyWith => _$LessonDtoCopyWithImpl<LessonDto>(this as LessonDto, _$identity);

  /// Serializes this LessonDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is LessonDto&&(identical(other.id, id) || other.id == id)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.lessonType, lessonType) || other.lessonType == lessonType)&&(identical(other.order, order) || other.order == order)&&(identical(other.duration, duration) || other.duration == duration)&&(identical(other.videoUrl, videoUrl) || other.videoUrl == videoUrl)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other.attachments, attachments)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.isFree, isFree) || other.isFree == isFree)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,courseId,title,description,lessonType,order,duration,videoUrl,content,const DeepCollectionEquality().hash(attachments),quizId,isFree,isCompleted,creationTime);

@override
String toString() {
  return 'LessonDto(id: $id, courseId: $courseId, title: $title, description: $description, lessonType: $lessonType, order: $order, duration: $duration, videoUrl: $videoUrl, content: $content, attachments: $attachments, quizId: $quizId, isFree: $isFree, isCompleted: $isCompleted, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $LessonDtoCopyWith<$Res>  {
  factory $LessonDtoCopyWith(LessonDto value, $Res Function(LessonDto) _then) = _$LessonDtoCopyWithImpl;
@useResult
$Res call({
 String id, String courseId, String title, String description, LessonType lessonType, int order, int duration, String? videoUrl, String? content, List<String> attachments, String? quizId, bool isFree, bool isCompleted, DateTime creationTime
});




}
/// @nodoc
class _$LessonDtoCopyWithImpl<$Res>
    implements $LessonDtoCopyWith<$Res> {
  _$LessonDtoCopyWithImpl(this._self, this._then);

  final LessonDto _self;
  final $Res Function(LessonDto) _then;

/// Create a copy of LessonDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? courseId = null,Object? title = null,Object? description = null,Object? lessonType = null,Object? order = null,Object? duration = null,Object? videoUrl = freezed,Object? content = freezed,Object? attachments = null,Object? quizId = freezed,Object? isFree = null,Object? isCompleted = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,lessonType: null == lessonType ? _self.lessonType : lessonType // ignore: cast_nullable_to_non_nullable
as LessonType,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,duration: null == duration ? _self.duration : duration // ignore: cast_nullable_to_non_nullable
as int,videoUrl: freezed == videoUrl ? _self.videoUrl : videoUrl // ignore: cast_nullable_to_non_nullable
as String?,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self.attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,quizId: freezed == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [LessonDto].
extension LessonDtoPatterns on LessonDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _LessonDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _LessonDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _LessonDto value)  $default,){
final _that = this;
switch (_that) {
case _LessonDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _LessonDto value)?  $default,){
final _that = this;
switch (_that) {
case _LessonDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree,  bool isCompleted,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _LessonDto() when $default != null:
return $default(_that.id,_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree,_that.isCompleted,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree,  bool isCompleted,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _LessonDto():
return $default(_that.id,_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree,_that.isCompleted,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree,  bool isCompleted,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _LessonDto() when $default != null:
return $default(_that.id,_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree,_that.isCompleted,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _LessonDto implements LessonDto {
  const _LessonDto({required this.id, required this.courseId, required this.title, required this.description, required this.lessonType, required this.order, required this.duration, this.videoUrl, this.content, final  List<String> attachments = const [], this.quizId, this.isFree = false, this.isCompleted = false, required this.creationTime}): _attachments = attachments;
  factory _LessonDto.fromJson(Map<String, dynamic> json) => _$LessonDtoFromJson(json);

@override final  String id;
@override final  String courseId;
@override final  String title;
@override final  String description;
@override final  LessonType lessonType;
@override final  int order;
@override final  int duration;
@override final  String? videoUrl;
@override final  String? content;
 final  List<String> _attachments;
@override@JsonKey() List<String> get attachments {
  if (_attachments is EqualUnmodifiableListView) return _attachments;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attachments);
}

@override final  String? quizId;
@override@JsonKey() final  bool isFree;
@override@JsonKey() final  bool isCompleted;
@override final  DateTime creationTime;

/// Create a copy of LessonDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$LessonDtoCopyWith<_LessonDto> get copyWith => __$LessonDtoCopyWithImpl<_LessonDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$LessonDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _LessonDto&&(identical(other.id, id) || other.id == id)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.lessonType, lessonType) || other.lessonType == lessonType)&&(identical(other.order, order) || other.order == order)&&(identical(other.duration, duration) || other.duration == duration)&&(identical(other.videoUrl, videoUrl) || other.videoUrl == videoUrl)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other._attachments, _attachments)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.isFree, isFree) || other.isFree == isFree)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,courseId,title,description,lessonType,order,duration,videoUrl,content,const DeepCollectionEquality().hash(_attachments),quizId,isFree,isCompleted,creationTime);

@override
String toString() {
  return 'LessonDto(id: $id, courseId: $courseId, title: $title, description: $description, lessonType: $lessonType, order: $order, duration: $duration, videoUrl: $videoUrl, content: $content, attachments: $attachments, quizId: $quizId, isFree: $isFree, isCompleted: $isCompleted, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$LessonDtoCopyWith<$Res> implements $LessonDtoCopyWith<$Res> {
  factory _$LessonDtoCopyWith(_LessonDto value, $Res Function(_LessonDto) _then) = __$LessonDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String courseId, String title, String description, LessonType lessonType, int order, int duration, String? videoUrl, String? content, List<String> attachments, String? quizId, bool isFree, bool isCompleted, DateTime creationTime
});




}
/// @nodoc
class __$LessonDtoCopyWithImpl<$Res>
    implements _$LessonDtoCopyWith<$Res> {
  __$LessonDtoCopyWithImpl(this._self, this._then);

  final _LessonDto _self;
  final $Res Function(_LessonDto) _then;

/// Create a copy of LessonDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? courseId = null,Object? title = null,Object? description = null,Object? lessonType = null,Object? order = null,Object? duration = null,Object? videoUrl = freezed,Object? content = freezed,Object? attachments = null,Object? quizId = freezed,Object? isFree = null,Object? isCompleted = null,Object? creationTime = null,}) {
  return _then(_LessonDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,lessonType: null == lessonType ? _self.lessonType : lessonType // ignore: cast_nullable_to_non_nullable
as LessonType,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,duration: null == duration ? _self.duration : duration // ignore: cast_nullable_to_non_nullable
as int,videoUrl: freezed == videoUrl ? _self.videoUrl : videoUrl // ignore: cast_nullable_to_non_nullable
as String?,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self._attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,quizId: freezed == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateLessonDto {

 String get courseId; String get title; String get description; LessonType get lessonType; int get order; int get duration; String? get videoUrl; String? get content; List<String> get attachments; String? get quizId; bool get isFree;
/// Create a copy of CreateUpdateLessonDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateLessonDtoCopyWith<CreateUpdateLessonDto> get copyWith => _$CreateUpdateLessonDtoCopyWithImpl<CreateUpdateLessonDto>(this as CreateUpdateLessonDto, _$identity);

  /// Serializes this CreateUpdateLessonDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateLessonDto&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.lessonType, lessonType) || other.lessonType == lessonType)&&(identical(other.order, order) || other.order == order)&&(identical(other.duration, duration) || other.duration == duration)&&(identical(other.videoUrl, videoUrl) || other.videoUrl == videoUrl)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other.attachments, attachments)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.isFree, isFree) || other.isFree == isFree));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,courseId,title,description,lessonType,order,duration,videoUrl,content,const DeepCollectionEquality().hash(attachments),quizId,isFree);

@override
String toString() {
  return 'CreateUpdateLessonDto(courseId: $courseId, title: $title, description: $description, lessonType: $lessonType, order: $order, duration: $duration, videoUrl: $videoUrl, content: $content, attachments: $attachments, quizId: $quizId, isFree: $isFree)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateLessonDtoCopyWith<$Res>  {
  factory $CreateUpdateLessonDtoCopyWith(CreateUpdateLessonDto value, $Res Function(CreateUpdateLessonDto) _then) = _$CreateUpdateLessonDtoCopyWithImpl;
@useResult
$Res call({
 String courseId, String title, String description, LessonType lessonType, int order, int duration, String? videoUrl, String? content, List<String> attachments, String? quizId, bool isFree
});




}
/// @nodoc
class _$CreateUpdateLessonDtoCopyWithImpl<$Res>
    implements $CreateUpdateLessonDtoCopyWith<$Res> {
  _$CreateUpdateLessonDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateLessonDto _self;
  final $Res Function(CreateUpdateLessonDto) _then;

/// Create a copy of CreateUpdateLessonDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? courseId = null,Object? title = null,Object? description = null,Object? lessonType = null,Object? order = null,Object? duration = null,Object? videoUrl = freezed,Object? content = freezed,Object? attachments = null,Object? quizId = freezed,Object? isFree = null,}) {
  return _then(_self.copyWith(
courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,lessonType: null == lessonType ? _self.lessonType : lessonType // ignore: cast_nullable_to_non_nullable
as LessonType,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,duration: null == duration ? _self.duration : duration // ignore: cast_nullable_to_non_nullable
as int,videoUrl: freezed == videoUrl ? _self.videoUrl : videoUrl // ignore: cast_nullable_to_non_nullable
as String?,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self.attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,quizId: freezed == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateLessonDto].
extension CreateUpdateLessonDtoPatterns on CreateUpdateLessonDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateLessonDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateLessonDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateLessonDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateLessonDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateLessonDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateLessonDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateLessonDto() when $default != null:
return $default(_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateLessonDto():
return $default(_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String courseId,  String title,  String description,  LessonType lessonType,  int order,  int duration,  String? videoUrl,  String? content,  List<String> attachments,  String? quizId,  bool isFree)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateLessonDto() when $default != null:
return $default(_that.courseId,_that.title,_that.description,_that.lessonType,_that.order,_that.duration,_that.videoUrl,_that.content,_that.attachments,_that.quizId,_that.isFree);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateLessonDto implements CreateUpdateLessonDto {
  const _CreateUpdateLessonDto({required this.courseId, required this.title, required this.description, required this.lessonType, required this.order, required this.duration, this.videoUrl, this.content, final  List<String> attachments = const [], this.quizId, this.isFree = false}): _attachments = attachments;
  factory _CreateUpdateLessonDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateLessonDtoFromJson(json);

@override final  String courseId;
@override final  String title;
@override final  String description;
@override final  LessonType lessonType;
@override final  int order;
@override final  int duration;
@override final  String? videoUrl;
@override final  String? content;
 final  List<String> _attachments;
@override@JsonKey() List<String> get attachments {
  if (_attachments is EqualUnmodifiableListView) return _attachments;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attachments);
}

@override final  String? quizId;
@override@JsonKey() final  bool isFree;

/// Create a copy of CreateUpdateLessonDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateLessonDtoCopyWith<_CreateUpdateLessonDto> get copyWith => __$CreateUpdateLessonDtoCopyWithImpl<_CreateUpdateLessonDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateLessonDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateLessonDto&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.lessonType, lessonType) || other.lessonType == lessonType)&&(identical(other.order, order) || other.order == order)&&(identical(other.duration, duration) || other.duration == duration)&&(identical(other.videoUrl, videoUrl) || other.videoUrl == videoUrl)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other._attachments, _attachments)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.isFree, isFree) || other.isFree == isFree));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,courseId,title,description,lessonType,order,duration,videoUrl,content,const DeepCollectionEquality().hash(_attachments),quizId,isFree);

@override
String toString() {
  return 'CreateUpdateLessonDto(courseId: $courseId, title: $title, description: $description, lessonType: $lessonType, order: $order, duration: $duration, videoUrl: $videoUrl, content: $content, attachments: $attachments, quizId: $quizId, isFree: $isFree)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateLessonDtoCopyWith<$Res> implements $CreateUpdateLessonDtoCopyWith<$Res> {
  factory _$CreateUpdateLessonDtoCopyWith(_CreateUpdateLessonDto value, $Res Function(_CreateUpdateLessonDto) _then) = __$CreateUpdateLessonDtoCopyWithImpl;
@override @useResult
$Res call({
 String courseId, String title, String description, LessonType lessonType, int order, int duration, String? videoUrl, String? content, List<String> attachments, String? quizId, bool isFree
});




}
/// @nodoc
class __$CreateUpdateLessonDtoCopyWithImpl<$Res>
    implements _$CreateUpdateLessonDtoCopyWith<$Res> {
  __$CreateUpdateLessonDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateLessonDto _self;
  final $Res Function(_CreateUpdateLessonDto) _then;

/// Create a copy of CreateUpdateLessonDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? courseId = null,Object? title = null,Object? description = null,Object? lessonType = null,Object? order = null,Object? duration = null,Object? videoUrl = freezed,Object? content = freezed,Object? attachments = null,Object? quizId = freezed,Object? isFree = null,}) {
  return _then(_CreateUpdateLessonDto(
courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,lessonType: null == lessonType ? _self.lessonType : lessonType // ignore: cast_nullable_to_non_nullable
as LessonType,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,duration: null == duration ? _self.duration : duration // ignore: cast_nullable_to_non_nullable
as int,videoUrl: freezed == videoUrl ? _self.videoUrl : videoUrl // ignore: cast_nullable_to_non_nullable
as String?,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self._attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,quizId: freezed == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

// dart format on
