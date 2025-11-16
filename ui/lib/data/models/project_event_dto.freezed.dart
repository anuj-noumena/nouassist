// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'project_event_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ProjectEventDto {

 String get id; String get projectId; String get title; String? get description; ProjectEventType get eventType; DateTime get startDateTime; DateTime get endDateTime; String? get location; bool get isAllDay; List<String> get attendees; int? get reminderMinutes; String? get meetingLink; DateTime get creationTime;
/// Create a copy of ProjectEventDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ProjectEventDtoCopyWith<ProjectEventDto> get copyWith => _$ProjectEventDtoCopyWithImpl<ProjectEventDto>(this as ProjectEventDto, _$identity);

  /// Serializes this ProjectEventDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ProjectEventDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.location, location) || other.location == location)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&const DeepCollectionEquality().equals(other.attendees, attendees)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,title,description,eventType,startDateTime,endDateTime,location,isAllDay,const DeepCollectionEquality().hash(attendees),reminderMinutes,meetingLink,creationTime);

@override
String toString() {
  return 'ProjectEventDto(id: $id, projectId: $projectId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, location: $location, isAllDay: $isAllDay, attendees: $attendees, reminderMinutes: $reminderMinutes, meetingLink: $meetingLink, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $ProjectEventDtoCopyWith<$Res>  {
  factory $ProjectEventDtoCopyWith(ProjectEventDto value, $Res Function(ProjectEventDto) _then) = _$ProjectEventDtoCopyWithImpl;
@useResult
$Res call({
 String id, String projectId, String title, String? description, ProjectEventType eventType, DateTime startDateTime, DateTime endDateTime, String? location, bool isAllDay, List<String> attendees, int? reminderMinutes, String? meetingLink, DateTime creationTime
});




}
/// @nodoc
class _$ProjectEventDtoCopyWithImpl<$Res>
    implements $ProjectEventDtoCopyWith<$Res> {
  _$ProjectEventDtoCopyWithImpl(this._self, this._then);

  final ProjectEventDto _self;
  final $Res Function(ProjectEventDto) _then;

/// Create a copy of ProjectEventDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? projectId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? location = freezed,Object? isAllDay = null,Object? attendees = null,Object? reminderMinutes = freezed,Object? meetingLink = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as ProjectEventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,attendees: null == attendees ? _self.attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [ProjectEventDto].
extension ProjectEventDtoPatterns on ProjectEventDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ProjectEventDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ProjectEventDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ProjectEventDto value)  $default,){
final _that = this;
switch (_that) {
case _ProjectEventDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ProjectEventDto value)?  $default,){
final _that = this;
switch (_that) {
case _ProjectEventDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ProjectEventDto() when $default != null:
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _ProjectEventDto():
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _ProjectEventDto() when $default != null:
return $default(_that.id,_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ProjectEventDto implements ProjectEventDto {
  const _ProjectEventDto({required this.id, required this.projectId, required this.title, this.description, required this.eventType, required this.startDateTime, required this.endDateTime, this.location, this.isAllDay = false, final  List<String> attendees = const [], this.reminderMinutes, this.meetingLink, required this.creationTime}): _attendees = attendees;
  factory _ProjectEventDto.fromJson(Map<String, dynamic> json) => _$ProjectEventDtoFromJson(json);

@override final  String id;
@override final  String projectId;
@override final  String title;
@override final  String? description;
@override final  ProjectEventType eventType;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override final  String? location;
@override@JsonKey() final  bool isAllDay;
 final  List<String> _attendees;
@override@JsonKey() List<String> get attendees {
  if (_attendees is EqualUnmodifiableListView) return _attendees;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attendees);
}

@override final  int? reminderMinutes;
@override final  String? meetingLink;
@override final  DateTime creationTime;

/// Create a copy of ProjectEventDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ProjectEventDtoCopyWith<_ProjectEventDto> get copyWith => __$ProjectEventDtoCopyWithImpl<_ProjectEventDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ProjectEventDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ProjectEventDto&&(identical(other.id, id) || other.id == id)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.location, location) || other.location == location)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&const DeepCollectionEquality().equals(other._attendees, _attendees)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,projectId,title,description,eventType,startDateTime,endDateTime,location,isAllDay,const DeepCollectionEquality().hash(_attendees),reminderMinutes,meetingLink,creationTime);

@override
String toString() {
  return 'ProjectEventDto(id: $id, projectId: $projectId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, location: $location, isAllDay: $isAllDay, attendees: $attendees, reminderMinutes: $reminderMinutes, meetingLink: $meetingLink, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$ProjectEventDtoCopyWith<$Res> implements $ProjectEventDtoCopyWith<$Res> {
  factory _$ProjectEventDtoCopyWith(_ProjectEventDto value, $Res Function(_ProjectEventDto) _then) = __$ProjectEventDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String projectId, String title, String? description, ProjectEventType eventType, DateTime startDateTime, DateTime endDateTime, String? location, bool isAllDay, List<String> attendees, int? reminderMinutes, String? meetingLink, DateTime creationTime
});




}
/// @nodoc
class __$ProjectEventDtoCopyWithImpl<$Res>
    implements _$ProjectEventDtoCopyWith<$Res> {
  __$ProjectEventDtoCopyWithImpl(this._self, this._then);

  final _ProjectEventDto _self;
  final $Res Function(_ProjectEventDto) _then;

/// Create a copy of ProjectEventDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? projectId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? location = freezed,Object? isAllDay = null,Object? attendees = null,Object? reminderMinutes = freezed,Object? meetingLink = freezed,Object? creationTime = null,}) {
  return _then(_ProjectEventDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as ProjectEventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,attendees: null == attendees ? _self._attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateEventDto {

 String get projectId; String get title; String? get description; ProjectEventType get eventType; DateTime get startDateTime; DateTime get endDateTime; String? get location; bool get isAllDay; List<String> get attendees; int? get reminderMinutes; String? get meetingLink;
/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateEventDtoCopyWith<CreateUpdateEventDto> get copyWith => _$CreateUpdateEventDtoCopyWithImpl<CreateUpdateEventDto>(this as CreateUpdateEventDto, _$identity);

  /// Serializes this CreateUpdateEventDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateEventDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.location, location) || other.location == location)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&const DeepCollectionEquality().equals(other.attendees, attendees)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,title,description,eventType,startDateTime,endDateTime,location,isAllDay,const DeepCollectionEquality().hash(attendees),reminderMinutes,meetingLink);

@override
String toString() {
  return 'CreateUpdateEventDto(projectId: $projectId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, location: $location, isAllDay: $isAllDay, attendees: $attendees, reminderMinutes: $reminderMinutes, meetingLink: $meetingLink)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateEventDtoCopyWith<$Res>  {
  factory $CreateUpdateEventDtoCopyWith(CreateUpdateEventDto value, $Res Function(CreateUpdateEventDto) _then) = _$CreateUpdateEventDtoCopyWithImpl;
@useResult
$Res call({
 String projectId, String title, String? description, ProjectEventType eventType, DateTime startDateTime, DateTime endDateTime, String? location, bool isAllDay, List<String> attendees, int? reminderMinutes, String? meetingLink
});




}
/// @nodoc
class _$CreateUpdateEventDtoCopyWithImpl<$Res>
    implements $CreateUpdateEventDtoCopyWith<$Res> {
  _$CreateUpdateEventDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateEventDto _self;
  final $Res Function(CreateUpdateEventDto) _then;

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? projectId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? location = freezed,Object? isAllDay = null,Object? attendees = null,Object? reminderMinutes = freezed,Object? meetingLink = freezed,}) {
  return _then(_self.copyWith(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as ProjectEventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,attendees: null == attendees ? _self.attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateEventDto].
extension CreateUpdateEventDtoPatterns on CreateUpdateEventDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateEventDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateEventDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateEventDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateEventDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
return $default(_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto():
return $default(_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String projectId,  String title,  String? description,  ProjectEventType eventType,  DateTime startDateTime,  DateTime endDateTime,  String? location,  bool isAllDay,  List<String> attendees,  int? reminderMinutes,  String? meetingLink)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
return $default(_that.projectId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.location,_that.isAllDay,_that.attendees,_that.reminderMinutes,_that.meetingLink);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateEventDto implements CreateUpdateEventDto {
  const _CreateUpdateEventDto({required this.projectId, required this.title, this.description, required this.eventType, required this.startDateTime, required this.endDateTime, this.location, this.isAllDay = false, final  List<String> attendees = const [], this.reminderMinutes, this.meetingLink}): _attendees = attendees;
  factory _CreateUpdateEventDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateEventDtoFromJson(json);

@override final  String projectId;
@override final  String title;
@override final  String? description;
@override final  ProjectEventType eventType;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override final  String? location;
@override@JsonKey() final  bool isAllDay;
 final  List<String> _attendees;
@override@JsonKey() List<String> get attendees {
  if (_attendees is EqualUnmodifiableListView) return _attendees;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attendees);
}

@override final  int? reminderMinutes;
@override final  String? meetingLink;

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateEventDtoCopyWith<_CreateUpdateEventDto> get copyWith => __$CreateUpdateEventDtoCopyWithImpl<_CreateUpdateEventDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateEventDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateEventDto&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.location, location) || other.location == location)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&const DeepCollectionEquality().equals(other._attendees, _attendees)&&(identical(other.reminderMinutes, reminderMinutes) || other.reminderMinutes == reminderMinutes)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,projectId,title,description,eventType,startDateTime,endDateTime,location,isAllDay,const DeepCollectionEquality().hash(_attendees),reminderMinutes,meetingLink);

@override
String toString() {
  return 'CreateUpdateEventDto(projectId: $projectId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, location: $location, isAllDay: $isAllDay, attendees: $attendees, reminderMinutes: $reminderMinutes, meetingLink: $meetingLink)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateEventDtoCopyWith<$Res> implements $CreateUpdateEventDtoCopyWith<$Res> {
  factory _$CreateUpdateEventDtoCopyWith(_CreateUpdateEventDto value, $Res Function(_CreateUpdateEventDto) _then) = __$CreateUpdateEventDtoCopyWithImpl;
@override @useResult
$Res call({
 String projectId, String title, String? description, ProjectEventType eventType, DateTime startDateTime, DateTime endDateTime, String? location, bool isAllDay, List<String> attendees, int? reminderMinutes, String? meetingLink
});




}
/// @nodoc
class __$CreateUpdateEventDtoCopyWithImpl<$Res>
    implements _$CreateUpdateEventDtoCopyWith<$Res> {
  __$CreateUpdateEventDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateEventDto _self;
  final $Res Function(_CreateUpdateEventDto) _then;

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? projectId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? location = freezed,Object? isAllDay = null,Object? attendees = null,Object? reminderMinutes = freezed,Object? meetingLink = freezed,}) {
  return _then(_CreateUpdateEventDto(
projectId: null == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as ProjectEventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,attendees: null == attendees ? _self._attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,reminderMinutes: freezed == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as int?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
