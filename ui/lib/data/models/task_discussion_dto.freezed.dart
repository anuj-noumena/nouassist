// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_discussion_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$TaskDiscussionDto {

 String get id; String get taskId; String? get parentDiscussionId; String get authorId; String get authorName; String get content; List<String> get mentions; List<String> get attachmentIds; bool get isEdited; Map<String, List<String>> get reactions; DateTime get creationTime; DateTime? get lastEditTime;
/// Create a copy of TaskDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskDiscussionDtoCopyWith<TaskDiscussionDto> get copyWith => _$TaskDiscussionDtoCopyWithImpl<TaskDiscussionDto>(this as TaskDiscussionDto, _$identity);

  /// Serializes this TaskDiscussionDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskDiscussionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.parentDiscussionId, parentDiscussionId) || other.parentDiscussionId == parentDiscussionId)&&(identical(other.authorId, authorId) || other.authorId == authorId)&&(identical(other.authorName, authorName) || other.authorName == authorName)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other.mentions, mentions)&&const DeepCollectionEquality().equals(other.attachmentIds, attachmentIds)&&(identical(other.isEdited, isEdited) || other.isEdited == isEdited)&&const DeepCollectionEquality().equals(other.reactions, reactions)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastEditTime, lastEditTime) || other.lastEditTime == lastEditTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,parentDiscussionId,authorId,authorName,content,const DeepCollectionEquality().hash(mentions),const DeepCollectionEquality().hash(attachmentIds),isEdited,const DeepCollectionEquality().hash(reactions),creationTime,lastEditTime);

@override
String toString() {
  return 'TaskDiscussionDto(id: $id, taskId: $taskId, parentDiscussionId: $parentDiscussionId, authorId: $authorId, authorName: $authorName, content: $content, mentions: $mentions, attachmentIds: $attachmentIds, isEdited: $isEdited, reactions: $reactions, creationTime: $creationTime, lastEditTime: $lastEditTime)';
}


}

/// @nodoc
abstract mixin class $TaskDiscussionDtoCopyWith<$Res>  {
  factory $TaskDiscussionDtoCopyWith(TaskDiscussionDto value, $Res Function(TaskDiscussionDto) _then) = _$TaskDiscussionDtoCopyWithImpl;
@useResult
$Res call({
 String id, String taskId, String? parentDiscussionId, String authorId, String authorName, String content, List<String> mentions, List<String> attachmentIds, bool isEdited, Map<String, List<String>> reactions, DateTime creationTime, DateTime? lastEditTime
});




}
/// @nodoc
class _$TaskDiscussionDtoCopyWithImpl<$Res>
    implements $TaskDiscussionDtoCopyWith<$Res> {
  _$TaskDiscussionDtoCopyWithImpl(this._self, this._then);

  final TaskDiscussionDto _self;
  final $Res Function(TaskDiscussionDto) _then;

/// Create a copy of TaskDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? taskId = null,Object? parentDiscussionId = freezed,Object? authorId = null,Object? authorName = null,Object? content = null,Object? mentions = null,Object? attachmentIds = null,Object? isEdited = null,Object? reactions = null,Object? creationTime = null,Object? lastEditTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,parentDiscussionId: freezed == parentDiscussionId ? _self.parentDiscussionId : parentDiscussionId // ignore: cast_nullable_to_non_nullable
as String?,authorId: null == authorId ? _self.authorId : authorId // ignore: cast_nullable_to_non_nullable
as String,authorName: null == authorName ? _self.authorName : authorName // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,mentions: null == mentions ? _self.mentions : mentions // ignore: cast_nullable_to_non_nullable
as List<String>,attachmentIds: null == attachmentIds ? _self.attachmentIds : attachmentIds // ignore: cast_nullable_to_non_nullable
as List<String>,isEdited: null == isEdited ? _self.isEdited : isEdited // ignore: cast_nullable_to_non_nullable
as bool,reactions: null == reactions ? _self.reactions : reactions // ignore: cast_nullable_to_non_nullable
as Map<String, List<String>>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastEditTime: freezed == lastEditTime ? _self.lastEditTime : lastEditTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskDiscussionDto].
extension TaskDiscussionDtoPatterns on TaskDiscussionDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskDiscussionDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskDiscussionDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskDiscussionDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskDiscussionDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskDiscussionDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskDiscussionDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String taskId,  String? parentDiscussionId,  String authorId,  String authorName,  String content,  List<String> mentions,  List<String> attachmentIds,  bool isEdited,  Map<String, List<String>> reactions,  DateTime creationTime,  DateTime? lastEditTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskDiscussionDto() when $default != null:
return $default(_that.id,_that.taskId,_that.parentDiscussionId,_that.authorId,_that.authorName,_that.content,_that.mentions,_that.attachmentIds,_that.isEdited,_that.reactions,_that.creationTime,_that.lastEditTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String taskId,  String? parentDiscussionId,  String authorId,  String authorName,  String content,  List<String> mentions,  List<String> attachmentIds,  bool isEdited,  Map<String, List<String>> reactions,  DateTime creationTime,  DateTime? lastEditTime)  $default,) {final _that = this;
switch (_that) {
case _TaskDiscussionDto():
return $default(_that.id,_that.taskId,_that.parentDiscussionId,_that.authorId,_that.authorName,_that.content,_that.mentions,_that.attachmentIds,_that.isEdited,_that.reactions,_that.creationTime,_that.lastEditTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String taskId,  String? parentDiscussionId,  String authorId,  String authorName,  String content,  List<String> mentions,  List<String> attachmentIds,  bool isEdited,  Map<String, List<String>> reactions,  DateTime creationTime,  DateTime? lastEditTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskDiscussionDto() when $default != null:
return $default(_that.id,_that.taskId,_that.parentDiscussionId,_that.authorId,_that.authorName,_that.content,_that.mentions,_that.attachmentIds,_that.isEdited,_that.reactions,_that.creationTime,_that.lastEditTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskDiscussionDto implements TaskDiscussionDto {
  const _TaskDiscussionDto({required this.id, required this.taskId, this.parentDiscussionId, required this.authorId, required this.authorName, required this.content, final  List<String> mentions = const [], final  List<String> attachmentIds = const [], this.isEdited = false, final  Map<String, List<String>> reactions = const {}, required this.creationTime, this.lastEditTime}): _mentions = mentions,_attachmentIds = attachmentIds,_reactions = reactions;
  factory _TaskDiscussionDto.fromJson(Map<String, dynamic> json) => _$TaskDiscussionDtoFromJson(json);

@override final  String id;
@override final  String taskId;
@override final  String? parentDiscussionId;
@override final  String authorId;
@override final  String authorName;
@override final  String content;
 final  List<String> _mentions;
@override@JsonKey() List<String> get mentions {
  if (_mentions is EqualUnmodifiableListView) return _mentions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_mentions);
}

 final  List<String> _attachmentIds;
@override@JsonKey() List<String> get attachmentIds {
  if (_attachmentIds is EqualUnmodifiableListView) return _attachmentIds;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attachmentIds);
}

@override@JsonKey() final  bool isEdited;
 final  Map<String, List<String>> _reactions;
@override@JsonKey() Map<String, List<String>> get reactions {
  if (_reactions is EqualUnmodifiableMapView) return _reactions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableMapView(_reactions);
}

@override final  DateTime creationTime;
@override final  DateTime? lastEditTime;

/// Create a copy of TaskDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskDiscussionDtoCopyWith<_TaskDiscussionDto> get copyWith => __$TaskDiscussionDtoCopyWithImpl<_TaskDiscussionDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskDiscussionDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskDiscussionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.parentDiscussionId, parentDiscussionId) || other.parentDiscussionId == parentDiscussionId)&&(identical(other.authorId, authorId) || other.authorId == authorId)&&(identical(other.authorName, authorName) || other.authorName == authorName)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other._mentions, _mentions)&&const DeepCollectionEquality().equals(other._attachmentIds, _attachmentIds)&&(identical(other.isEdited, isEdited) || other.isEdited == isEdited)&&const DeepCollectionEquality().equals(other._reactions, _reactions)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastEditTime, lastEditTime) || other.lastEditTime == lastEditTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,parentDiscussionId,authorId,authorName,content,const DeepCollectionEquality().hash(_mentions),const DeepCollectionEquality().hash(_attachmentIds),isEdited,const DeepCollectionEquality().hash(_reactions),creationTime,lastEditTime);

@override
String toString() {
  return 'TaskDiscussionDto(id: $id, taskId: $taskId, parentDiscussionId: $parentDiscussionId, authorId: $authorId, authorName: $authorName, content: $content, mentions: $mentions, attachmentIds: $attachmentIds, isEdited: $isEdited, reactions: $reactions, creationTime: $creationTime, lastEditTime: $lastEditTime)';
}


}

/// @nodoc
abstract mixin class _$TaskDiscussionDtoCopyWith<$Res> implements $TaskDiscussionDtoCopyWith<$Res> {
  factory _$TaskDiscussionDtoCopyWith(_TaskDiscussionDto value, $Res Function(_TaskDiscussionDto) _then) = __$TaskDiscussionDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String taskId, String? parentDiscussionId, String authorId, String authorName, String content, List<String> mentions, List<String> attachmentIds, bool isEdited, Map<String, List<String>> reactions, DateTime creationTime, DateTime? lastEditTime
});




}
/// @nodoc
class __$TaskDiscussionDtoCopyWithImpl<$Res>
    implements _$TaskDiscussionDtoCopyWith<$Res> {
  __$TaskDiscussionDtoCopyWithImpl(this._self, this._then);

  final _TaskDiscussionDto _self;
  final $Res Function(_TaskDiscussionDto) _then;

/// Create a copy of TaskDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? taskId = null,Object? parentDiscussionId = freezed,Object? authorId = null,Object? authorName = null,Object? content = null,Object? mentions = null,Object? attachmentIds = null,Object? isEdited = null,Object? reactions = null,Object? creationTime = null,Object? lastEditTime = freezed,}) {
  return _then(_TaskDiscussionDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,parentDiscussionId: freezed == parentDiscussionId ? _self.parentDiscussionId : parentDiscussionId // ignore: cast_nullable_to_non_nullable
as String?,authorId: null == authorId ? _self.authorId : authorId // ignore: cast_nullable_to_non_nullable
as String,authorName: null == authorName ? _self.authorName : authorName // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,mentions: null == mentions ? _self._mentions : mentions // ignore: cast_nullable_to_non_nullable
as List<String>,attachmentIds: null == attachmentIds ? _self._attachmentIds : attachmentIds // ignore: cast_nullable_to_non_nullable
as List<String>,isEdited: null == isEdited ? _self.isEdited : isEdited // ignore: cast_nullable_to_non_nullable
as bool,reactions: null == reactions ? _self._reactions : reactions // ignore: cast_nullable_to_non_nullable
as Map<String, List<String>>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastEditTime: freezed == lastEditTime ? _self.lastEditTime : lastEditTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateDiscussionDto {

 String get taskId; String? get parentDiscussionId; String get content; List<String> get mentions; List<String> get attachmentIds;
/// Create a copy of CreateUpdateDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateDiscussionDtoCopyWith<CreateUpdateDiscussionDto> get copyWith => _$CreateUpdateDiscussionDtoCopyWithImpl<CreateUpdateDiscussionDto>(this as CreateUpdateDiscussionDto, _$identity);

  /// Serializes this CreateUpdateDiscussionDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateDiscussionDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.parentDiscussionId, parentDiscussionId) || other.parentDiscussionId == parentDiscussionId)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other.mentions, mentions)&&const DeepCollectionEquality().equals(other.attachmentIds, attachmentIds));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,parentDiscussionId,content,const DeepCollectionEquality().hash(mentions),const DeepCollectionEquality().hash(attachmentIds));

@override
String toString() {
  return 'CreateUpdateDiscussionDto(taskId: $taskId, parentDiscussionId: $parentDiscussionId, content: $content, mentions: $mentions, attachmentIds: $attachmentIds)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateDiscussionDtoCopyWith<$Res>  {
  factory $CreateUpdateDiscussionDtoCopyWith(CreateUpdateDiscussionDto value, $Res Function(CreateUpdateDiscussionDto) _then) = _$CreateUpdateDiscussionDtoCopyWithImpl;
@useResult
$Res call({
 String taskId, String? parentDiscussionId, String content, List<String> mentions, List<String> attachmentIds
});




}
/// @nodoc
class _$CreateUpdateDiscussionDtoCopyWithImpl<$Res>
    implements $CreateUpdateDiscussionDtoCopyWith<$Res> {
  _$CreateUpdateDiscussionDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateDiscussionDto _self;
  final $Res Function(CreateUpdateDiscussionDto) _then;

/// Create a copy of CreateUpdateDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? taskId = null,Object? parentDiscussionId = freezed,Object? content = null,Object? mentions = null,Object? attachmentIds = null,}) {
  return _then(_self.copyWith(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,parentDiscussionId: freezed == parentDiscussionId ? _self.parentDiscussionId : parentDiscussionId // ignore: cast_nullable_to_non_nullable
as String?,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,mentions: null == mentions ? _self.mentions : mentions // ignore: cast_nullable_to_non_nullable
as List<String>,attachmentIds: null == attachmentIds ? _self.attachmentIds : attachmentIds // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateDiscussionDto].
extension CreateUpdateDiscussionDtoPatterns on CreateUpdateDiscussionDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateDiscussionDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateDiscussionDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateDiscussionDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String taskId,  String? parentDiscussionId,  String content,  List<String> mentions,  List<String> attachmentIds)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto() when $default != null:
return $default(_that.taskId,_that.parentDiscussionId,_that.content,_that.mentions,_that.attachmentIds);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String taskId,  String? parentDiscussionId,  String content,  List<String> mentions,  List<String> attachmentIds)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto():
return $default(_that.taskId,_that.parentDiscussionId,_that.content,_that.mentions,_that.attachmentIds);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String taskId,  String? parentDiscussionId,  String content,  List<String> mentions,  List<String> attachmentIds)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateDiscussionDto() when $default != null:
return $default(_that.taskId,_that.parentDiscussionId,_that.content,_that.mentions,_that.attachmentIds);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateDiscussionDto implements CreateUpdateDiscussionDto {
  const _CreateUpdateDiscussionDto({required this.taskId, this.parentDiscussionId, required this.content, final  List<String> mentions = const [], final  List<String> attachmentIds = const []}): _mentions = mentions,_attachmentIds = attachmentIds;
  factory _CreateUpdateDiscussionDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateDiscussionDtoFromJson(json);

@override final  String taskId;
@override final  String? parentDiscussionId;
@override final  String content;
 final  List<String> _mentions;
@override@JsonKey() List<String> get mentions {
  if (_mentions is EqualUnmodifiableListView) return _mentions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_mentions);
}

 final  List<String> _attachmentIds;
@override@JsonKey() List<String> get attachmentIds {
  if (_attachmentIds is EqualUnmodifiableListView) return _attachmentIds;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attachmentIds);
}


/// Create a copy of CreateUpdateDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateDiscussionDtoCopyWith<_CreateUpdateDiscussionDto> get copyWith => __$CreateUpdateDiscussionDtoCopyWithImpl<_CreateUpdateDiscussionDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateDiscussionDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateDiscussionDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.parentDiscussionId, parentDiscussionId) || other.parentDiscussionId == parentDiscussionId)&&(identical(other.content, content) || other.content == content)&&const DeepCollectionEquality().equals(other._mentions, _mentions)&&const DeepCollectionEquality().equals(other._attachmentIds, _attachmentIds));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,parentDiscussionId,content,const DeepCollectionEquality().hash(_mentions),const DeepCollectionEquality().hash(_attachmentIds));

@override
String toString() {
  return 'CreateUpdateDiscussionDto(taskId: $taskId, parentDiscussionId: $parentDiscussionId, content: $content, mentions: $mentions, attachmentIds: $attachmentIds)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateDiscussionDtoCopyWith<$Res> implements $CreateUpdateDiscussionDtoCopyWith<$Res> {
  factory _$CreateUpdateDiscussionDtoCopyWith(_CreateUpdateDiscussionDto value, $Res Function(_CreateUpdateDiscussionDto) _then) = __$CreateUpdateDiscussionDtoCopyWithImpl;
@override @useResult
$Res call({
 String taskId, String? parentDiscussionId, String content, List<String> mentions, List<String> attachmentIds
});




}
/// @nodoc
class __$CreateUpdateDiscussionDtoCopyWithImpl<$Res>
    implements _$CreateUpdateDiscussionDtoCopyWith<$Res> {
  __$CreateUpdateDiscussionDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateDiscussionDto _self;
  final $Res Function(_CreateUpdateDiscussionDto) _then;

/// Create a copy of CreateUpdateDiscussionDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? taskId = null,Object? parentDiscussionId = freezed,Object? content = null,Object? mentions = null,Object? attachmentIds = null,}) {
  return _then(_CreateUpdateDiscussionDto(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,parentDiscussionId: freezed == parentDiscussionId ? _self.parentDiscussionId : parentDiscussionId // ignore: cast_nullable_to_non_nullable
as String?,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,mentions: null == mentions ? _self._mentions : mentions // ignore: cast_nullable_to_non_nullable
as List<String>,attachmentIds: null == attachmentIds ? _self._attachmentIds : attachmentIds // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}


}

// dart format on
