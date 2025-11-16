// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'event_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$EventDto {

 String get id; String get userId; String get title; String? get description; EventType get eventType; DateTime get startDateTime; DateTime get endDateTime; bool get isAllDay; String? get location; String? get meetingLink; List<String> get attendees; bool get isRecurring; RecurrencePatternDto? get recurrencePattern; List<int> get reminderMinutes; String get color; EventVisibility get visibility; EventStatus get status; String? get projectId; String? get taskId; String? get timeBlockId; DateTime get creationTime; DateTime? get lastModificationTime;
/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$EventDtoCopyWith<EventDto> get copyWith => _$EventDtoCopyWithImpl<EventDto>(this as EventDto, _$identity);

  /// Serializes this EventDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is EventDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&(identical(other.location, location) || other.location == location)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&const DeepCollectionEquality().equals(other.attendees, attendees)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&const DeepCollectionEquality().equals(other.reminderMinutes, reminderMinutes)&&(identical(other.color, color) || other.color == color)&&(identical(other.visibility, visibility) || other.visibility == visibility)&&(identical(other.status, status) || other.status == status)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.timeBlockId, timeBlockId) || other.timeBlockId == timeBlockId)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,userId,title,description,eventType,startDateTime,endDateTime,isAllDay,location,meetingLink,const DeepCollectionEquality().hash(attendees),isRecurring,recurrencePattern,const DeepCollectionEquality().hash(reminderMinutes),color,visibility,status,projectId,taskId,timeBlockId,creationTime,lastModificationTime]);

@override
String toString() {
  return 'EventDto(id: $id, userId: $userId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, isAllDay: $isAllDay, location: $location, meetingLink: $meetingLink, attendees: $attendees, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, reminderMinutes: $reminderMinutes, color: $color, visibility: $visibility, status: $status, projectId: $projectId, taskId: $taskId, timeBlockId: $timeBlockId, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class $EventDtoCopyWith<$Res>  {
  factory $EventDtoCopyWith(EventDto value, $Res Function(EventDto) _then) = _$EventDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String title, String? description, EventType eventType, DateTime startDateTime, DateTime endDateTime, bool isAllDay, String? location, String? meetingLink, List<String> attendees, bool isRecurring, RecurrencePatternDto? recurrencePattern, List<int> reminderMinutes, String color, EventVisibility visibility, EventStatus status, String? projectId, String? taskId, String? timeBlockId, DateTime creationTime, DateTime? lastModificationTime
});


$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class _$EventDtoCopyWithImpl<$Res>
    implements $EventDtoCopyWith<$Res> {
  _$EventDtoCopyWithImpl(this._self, this._then);

  final EventDto _self;
  final $Res Function(EventDto) _then;

/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? isAllDay = null,Object? location = freezed,Object? meetingLink = freezed,Object? attendees = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? reminderMinutes = null,Object? color = null,Object? visibility = null,Object? status = null,Object? projectId = freezed,Object? taskId = freezed,Object? timeBlockId = freezed,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as EventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,attendees: null == attendees ? _self.attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,reminderMinutes: null == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as List<int>,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,visibility: null == visibility ? _self.visibility : visibility // ignore: cast_nullable_to_non_nullable
as EventVisibility,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as EventStatus,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,timeBlockId: freezed == timeBlockId ? _self.timeBlockId : timeBlockId // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}
/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}


/// Adds pattern-matching-related methods to [EventDto].
extension EventDtoPatterns on EventDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _EventDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _EventDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _EventDto value)  $default,){
final _that = this;
switch (_that) {
case _EventDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _EventDto value)?  $default,){
final _that = this;
switch (_that) {
case _EventDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _EventDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId,  DateTime creationTime,  DateTime? lastModificationTime)  $default,) {final _that = this;
switch (_that) {
case _EventDto():
return $default(_that.id,_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId,_that.creationTime,_that.lastModificationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId,  DateTime creationTime,  DateTime? lastModificationTime)?  $default,) {final _that = this;
switch (_that) {
case _EventDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId,_that.creationTime,_that.lastModificationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _EventDto implements EventDto {
  const _EventDto({required this.id, required this.userId, required this.title, this.description, required this.eventType, required this.startDateTime, required this.endDateTime, this.isAllDay = false, this.location, this.meetingLink, final  List<String> attendees = const [], this.isRecurring = false, this.recurrencePattern, final  List<int> reminderMinutes = const [], required this.color, required this.visibility, required this.status, this.projectId, this.taskId, this.timeBlockId, required this.creationTime, this.lastModificationTime}): _attendees = attendees,_reminderMinutes = reminderMinutes;
  factory _EventDto.fromJson(Map<String, dynamic> json) => _$EventDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  EventType eventType;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override@JsonKey() final  bool isAllDay;
@override final  String? location;
@override final  String? meetingLink;
 final  List<String> _attendees;
@override@JsonKey() List<String> get attendees {
  if (_attendees is EqualUnmodifiableListView) return _attendees;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attendees);
}

@override@JsonKey() final  bool isRecurring;
@override final  RecurrencePatternDto? recurrencePattern;
 final  List<int> _reminderMinutes;
@override@JsonKey() List<int> get reminderMinutes {
  if (_reminderMinutes is EqualUnmodifiableListView) return _reminderMinutes;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_reminderMinutes);
}

@override final  String color;
@override final  EventVisibility visibility;
@override final  EventStatus status;
@override final  String? projectId;
@override final  String? taskId;
@override final  String? timeBlockId;
@override final  DateTime creationTime;
@override final  DateTime? lastModificationTime;

/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$EventDtoCopyWith<_EventDto> get copyWith => __$EventDtoCopyWithImpl<_EventDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$EventDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _EventDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&(identical(other.location, location) || other.location == location)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&const DeepCollectionEquality().equals(other._attendees, _attendees)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&const DeepCollectionEquality().equals(other._reminderMinutes, _reminderMinutes)&&(identical(other.color, color) || other.color == color)&&(identical(other.visibility, visibility) || other.visibility == visibility)&&(identical(other.status, status) || other.status == status)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.timeBlockId, timeBlockId) || other.timeBlockId == timeBlockId)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,userId,title,description,eventType,startDateTime,endDateTime,isAllDay,location,meetingLink,const DeepCollectionEquality().hash(_attendees),isRecurring,recurrencePattern,const DeepCollectionEquality().hash(_reminderMinutes),color,visibility,status,projectId,taskId,timeBlockId,creationTime,lastModificationTime]);

@override
String toString() {
  return 'EventDto(id: $id, userId: $userId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, isAllDay: $isAllDay, location: $location, meetingLink: $meetingLink, attendees: $attendees, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, reminderMinutes: $reminderMinutes, color: $color, visibility: $visibility, status: $status, projectId: $projectId, taskId: $taskId, timeBlockId: $timeBlockId, creationTime: $creationTime, lastModificationTime: $lastModificationTime)';
}


}

/// @nodoc
abstract mixin class _$EventDtoCopyWith<$Res> implements $EventDtoCopyWith<$Res> {
  factory _$EventDtoCopyWith(_EventDto value, $Res Function(_EventDto) _then) = __$EventDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String title, String? description, EventType eventType, DateTime startDateTime, DateTime endDateTime, bool isAllDay, String? location, String? meetingLink, List<String> attendees, bool isRecurring, RecurrencePatternDto? recurrencePattern, List<int> reminderMinutes, String color, EventVisibility visibility, EventStatus status, String? projectId, String? taskId, String? timeBlockId, DateTime creationTime, DateTime? lastModificationTime
});


@override $RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class __$EventDtoCopyWithImpl<$Res>
    implements _$EventDtoCopyWith<$Res> {
  __$EventDtoCopyWithImpl(this._self, this._then);

  final _EventDto _self;
  final $Res Function(_EventDto) _then;

/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? isAllDay = null,Object? location = freezed,Object? meetingLink = freezed,Object? attendees = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? reminderMinutes = null,Object? color = null,Object? visibility = null,Object? status = null,Object? projectId = freezed,Object? taskId = freezed,Object? timeBlockId = freezed,Object? creationTime = null,Object? lastModificationTime = freezed,}) {
  return _then(_EventDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as EventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,attendees: null == attendees ? _self._attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,reminderMinutes: null == reminderMinutes ? _self._reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as List<int>,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,visibility: null == visibility ? _self.visibility : visibility // ignore: cast_nullable_to_non_nullable
as EventVisibility,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as EventStatus,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,timeBlockId: freezed == timeBlockId ? _self.timeBlockId : timeBlockId // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

/// Create a copy of EventDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}


/// @nodoc
mixin _$CreateUpdateEventDto {

 String get userId; String get title; String? get description; EventType get eventType; DateTime get startDateTime; DateTime get endDateTime; bool get isAllDay; String? get location; String? get meetingLink; List<String> get attendees; bool get isRecurring; RecurrencePatternDto? get recurrencePattern; List<int> get reminderMinutes; String get color; EventVisibility get visibility; EventStatus get status; String? get projectId; String? get taskId; String? get timeBlockId;
/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateEventDtoCopyWith<CreateUpdateEventDto> get copyWith => _$CreateUpdateEventDtoCopyWithImpl<CreateUpdateEventDto>(this as CreateUpdateEventDto, _$identity);

  /// Serializes this CreateUpdateEventDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateEventDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&(identical(other.location, location) || other.location == location)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&const DeepCollectionEquality().equals(other.attendees, attendees)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&const DeepCollectionEquality().equals(other.reminderMinutes, reminderMinutes)&&(identical(other.color, color) || other.color == color)&&(identical(other.visibility, visibility) || other.visibility == visibility)&&(identical(other.status, status) || other.status == status)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.timeBlockId, timeBlockId) || other.timeBlockId == timeBlockId));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,userId,title,description,eventType,startDateTime,endDateTime,isAllDay,location,meetingLink,const DeepCollectionEquality().hash(attendees),isRecurring,recurrencePattern,const DeepCollectionEquality().hash(reminderMinutes),color,visibility,status,projectId,taskId,timeBlockId]);

@override
String toString() {
  return 'CreateUpdateEventDto(userId: $userId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, isAllDay: $isAllDay, location: $location, meetingLink: $meetingLink, attendees: $attendees, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, reminderMinutes: $reminderMinutes, color: $color, visibility: $visibility, status: $status, projectId: $projectId, taskId: $taskId, timeBlockId: $timeBlockId)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateEventDtoCopyWith<$Res>  {
  factory $CreateUpdateEventDtoCopyWith(CreateUpdateEventDto value, $Res Function(CreateUpdateEventDto) _then) = _$CreateUpdateEventDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String title, String? description, EventType eventType, DateTime startDateTime, DateTime endDateTime, bool isAllDay, String? location, String? meetingLink, List<String> attendees, bool isRecurring, RecurrencePatternDto? recurrencePattern, List<int> reminderMinutes, String color, EventVisibility visibility, EventStatus status, String? projectId, String? taskId, String? timeBlockId
});


$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class _$CreateUpdateEventDtoCopyWithImpl<$Res>
    implements $CreateUpdateEventDtoCopyWith<$Res> {
  _$CreateUpdateEventDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateEventDto _self;
  final $Res Function(CreateUpdateEventDto) _then;

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? isAllDay = null,Object? location = freezed,Object? meetingLink = freezed,Object? attendees = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? reminderMinutes = null,Object? color = null,Object? visibility = null,Object? status = null,Object? projectId = freezed,Object? taskId = freezed,Object? timeBlockId = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as EventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,attendees: null == attendees ? _self.attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,reminderMinutes: null == reminderMinutes ? _self.reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as List<int>,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,visibility: null == visibility ? _self.visibility : visibility // ignore: cast_nullable_to_non_nullable
as EventVisibility,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as EventStatus,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,timeBlockId: freezed == timeBlockId ? _self.timeBlockId : timeBlockId // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}
/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto():
return $default(_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String title,  String? description,  EventType eventType,  DateTime startDateTime,  DateTime endDateTime,  bool isAllDay,  String? location,  String? meetingLink,  List<String> attendees,  bool isRecurring,  RecurrencePatternDto? recurrencePattern,  List<int> reminderMinutes,  String color,  EventVisibility visibility,  EventStatus status,  String? projectId,  String? taskId,  String? timeBlockId)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateEventDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.eventType,_that.startDateTime,_that.endDateTime,_that.isAllDay,_that.location,_that.meetingLink,_that.attendees,_that.isRecurring,_that.recurrencePattern,_that.reminderMinutes,_that.color,_that.visibility,_that.status,_that.projectId,_that.taskId,_that.timeBlockId);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateEventDto implements CreateUpdateEventDto {
  const _CreateUpdateEventDto({required this.userId, required this.title, this.description, required this.eventType, required this.startDateTime, required this.endDateTime, this.isAllDay = false, this.location, this.meetingLink, final  List<String> attendees = const [], this.isRecurring = false, this.recurrencePattern, final  List<int> reminderMinutes = const [], this.color = '#2196F3', this.visibility = EventVisibility.public, this.status = EventStatus.confirmed, this.projectId, this.taskId, this.timeBlockId}): _attendees = attendees,_reminderMinutes = reminderMinutes;
  factory _CreateUpdateEventDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateEventDtoFromJson(json);

@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  EventType eventType;
@override final  DateTime startDateTime;
@override final  DateTime endDateTime;
@override@JsonKey() final  bool isAllDay;
@override final  String? location;
@override final  String? meetingLink;
 final  List<String> _attendees;
@override@JsonKey() List<String> get attendees {
  if (_attendees is EqualUnmodifiableListView) return _attendees;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attendees);
}

@override@JsonKey() final  bool isRecurring;
@override final  RecurrencePatternDto? recurrencePattern;
 final  List<int> _reminderMinutes;
@override@JsonKey() List<int> get reminderMinutes {
  if (_reminderMinutes is EqualUnmodifiableListView) return _reminderMinutes;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_reminderMinutes);
}

@override@JsonKey() final  String color;
@override@JsonKey() final  EventVisibility visibility;
@override@JsonKey() final  EventStatus status;
@override final  String? projectId;
@override final  String? taskId;
@override final  String? timeBlockId;

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
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateEventDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.eventType, eventType) || other.eventType == eventType)&&(identical(other.startDateTime, startDateTime) || other.startDateTime == startDateTime)&&(identical(other.endDateTime, endDateTime) || other.endDateTime == endDateTime)&&(identical(other.isAllDay, isAllDay) || other.isAllDay == isAllDay)&&(identical(other.location, location) || other.location == location)&&(identical(other.meetingLink, meetingLink) || other.meetingLink == meetingLink)&&const DeepCollectionEquality().equals(other._attendees, _attendees)&&(identical(other.isRecurring, isRecurring) || other.isRecurring == isRecurring)&&(identical(other.recurrencePattern, recurrencePattern) || other.recurrencePattern == recurrencePattern)&&const DeepCollectionEquality().equals(other._reminderMinutes, _reminderMinutes)&&(identical(other.color, color) || other.color == color)&&(identical(other.visibility, visibility) || other.visibility == visibility)&&(identical(other.status, status) || other.status == status)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.timeBlockId, timeBlockId) || other.timeBlockId == timeBlockId));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,userId,title,description,eventType,startDateTime,endDateTime,isAllDay,location,meetingLink,const DeepCollectionEquality().hash(_attendees),isRecurring,recurrencePattern,const DeepCollectionEquality().hash(_reminderMinutes),color,visibility,status,projectId,taskId,timeBlockId]);

@override
String toString() {
  return 'CreateUpdateEventDto(userId: $userId, title: $title, description: $description, eventType: $eventType, startDateTime: $startDateTime, endDateTime: $endDateTime, isAllDay: $isAllDay, location: $location, meetingLink: $meetingLink, attendees: $attendees, isRecurring: $isRecurring, recurrencePattern: $recurrencePattern, reminderMinutes: $reminderMinutes, color: $color, visibility: $visibility, status: $status, projectId: $projectId, taskId: $taskId, timeBlockId: $timeBlockId)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateEventDtoCopyWith<$Res> implements $CreateUpdateEventDtoCopyWith<$Res> {
  factory _$CreateUpdateEventDtoCopyWith(_CreateUpdateEventDto value, $Res Function(_CreateUpdateEventDto) _then) = __$CreateUpdateEventDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String title, String? description, EventType eventType, DateTime startDateTime, DateTime endDateTime, bool isAllDay, String? location, String? meetingLink, List<String> attendees, bool isRecurring, RecurrencePatternDto? recurrencePattern, List<int> reminderMinutes, String color, EventVisibility visibility, EventStatus status, String? projectId, String? taskId, String? timeBlockId
});


@override $RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern;

}
/// @nodoc
class __$CreateUpdateEventDtoCopyWithImpl<$Res>
    implements _$CreateUpdateEventDtoCopyWith<$Res> {
  __$CreateUpdateEventDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateEventDto _self;
  final $Res Function(_CreateUpdateEventDto) _then;

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? eventType = null,Object? startDateTime = null,Object? endDateTime = null,Object? isAllDay = null,Object? location = freezed,Object? meetingLink = freezed,Object? attendees = null,Object? isRecurring = null,Object? recurrencePattern = freezed,Object? reminderMinutes = null,Object? color = null,Object? visibility = null,Object? status = null,Object? projectId = freezed,Object? taskId = freezed,Object? timeBlockId = freezed,}) {
  return _then(_CreateUpdateEventDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,eventType: null == eventType ? _self.eventType : eventType // ignore: cast_nullable_to_non_nullable
as EventType,startDateTime: null == startDateTime ? _self.startDateTime : startDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,endDateTime: null == endDateTime ? _self.endDateTime : endDateTime // ignore: cast_nullable_to_non_nullable
as DateTime,isAllDay: null == isAllDay ? _self.isAllDay : isAllDay // ignore: cast_nullable_to_non_nullable
as bool,location: freezed == location ? _self.location : location // ignore: cast_nullable_to_non_nullable
as String?,meetingLink: freezed == meetingLink ? _self.meetingLink : meetingLink // ignore: cast_nullable_to_non_nullable
as String?,attendees: null == attendees ? _self._attendees : attendees // ignore: cast_nullable_to_non_nullable
as List<String>,isRecurring: null == isRecurring ? _self.isRecurring : isRecurring // ignore: cast_nullable_to_non_nullable
as bool,recurrencePattern: freezed == recurrencePattern ? _self.recurrencePattern : recurrencePattern // ignore: cast_nullable_to_non_nullable
as RecurrencePatternDto?,reminderMinutes: null == reminderMinutes ? _self._reminderMinutes : reminderMinutes // ignore: cast_nullable_to_non_nullable
as List<int>,color: null == color ? _self.color : color // ignore: cast_nullable_to_non_nullable
as String,visibility: null == visibility ? _self.visibility : visibility // ignore: cast_nullable_to_non_nullable
as EventVisibility,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as EventStatus,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,taskId: freezed == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String?,timeBlockId: freezed == timeBlockId ? _self.timeBlockId : timeBlockId // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

/// Create a copy of CreateUpdateEventDto
/// with the given fields replaced by the non-null parameter values.
@override
@pragma('vm:prefer-inline')
$RecurrencePatternDtoCopyWith<$Res>? get recurrencePattern {
    if (_self.recurrencePattern == null) {
    return null;
  }

  return $RecurrencePatternDtoCopyWith<$Res>(_self.recurrencePattern!, (value) {
    return _then(_self.copyWith(recurrencePattern: value));
  });
}
}

// dart format on
