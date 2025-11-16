// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_waiting_for_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdWaitingForDto {

 String get id; String get userId; String get description; String get waitingFor; String? get notes; String? get projectId; String? get relatedActionId; WaitingForStatus get status; DateTime? get requestedDate; DateTime? get expectedDate; DateTime? get reminderDate; DateTime? get receivedDate; List<String> get tags; DateTime get creationTime;
/// Create a copy of GtdWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdWaitingForDtoCopyWith<GtdWaitingForDto> get copyWith => _$GtdWaitingForDtoCopyWithImpl<GtdWaitingForDto>(this as GtdWaitingForDto, _$identity);

  /// Serializes this GtdWaitingForDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdWaitingForDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.description, description) || other.description == description)&&(identical(other.waitingFor, waitingFor) || other.waitingFor == waitingFor)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.relatedActionId, relatedActionId) || other.relatedActionId == relatedActionId)&&(identical(other.status, status) || other.status == status)&&(identical(other.requestedDate, requestedDate) || other.requestedDate == requestedDate)&&(identical(other.expectedDate, expectedDate) || other.expectedDate == expectedDate)&&(identical(other.reminderDate, reminderDate) || other.reminderDate == reminderDate)&&(identical(other.receivedDate, receivedDate) || other.receivedDate == receivedDate)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,description,waitingFor,notes,projectId,relatedActionId,status,requestedDate,expectedDate,reminderDate,receivedDate,const DeepCollectionEquality().hash(tags),creationTime);

@override
String toString() {
  return 'GtdWaitingForDto(id: $id, userId: $userId, description: $description, waitingFor: $waitingFor, notes: $notes, projectId: $projectId, relatedActionId: $relatedActionId, status: $status, requestedDate: $requestedDate, expectedDate: $expectedDate, reminderDate: $reminderDate, receivedDate: $receivedDate, tags: $tags, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdWaitingForDtoCopyWith<$Res>  {
  factory $GtdWaitingForDtoCopyWith(GtdWaitingForDto value, $Res Function(GtdWaitingForDto) _then) = _$GtdWaitingForDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String description, String waitingFor, String? notes, String? projectId, String? relatedActionId, WaitingForStatus status, DateTime? requestedDate, DateTime? expectedDate, DateTime? reminderDate, DateTime? receivedDate, List<String> tags, DateTime creationTime
});




}
/// @nodoc
class _$GtdWaitingForDtoCopyWithImpl<$Res>
    implements $GtdWaitingForDtoCopyWith<$Res> {
  _$GtdWaitingForDtoCopyWithImpl(this._self, this._then);

  final GtdWaitingForDto _self;
  final $Res Function(GtdWaitingForDto) _then;

/// Create a copy of GtdWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? description = null,Object? waitingFor = null,Object? notes = freezed,Object? projectId = freezed,Object? relatedActionId = freezed,Object? status = null,Object? requestedDate = freezed,Object? expectedDate = freezed,Object? reminderDate = freezed,Object? receivedDate = freezed,Object? tags = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,waitingFor: null == waitingFor ? _self.waitingFor : waitingFor // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,relatedActionId: freezed == relatedActionId ? _self.relatedActionId : relatedActionId // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as WaitingForStatus,requestedDate: freezed == requestedDate ? _self.requestedDate : requestedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,expectedDate: freezed == expectedDate ? _self.expectedDate : expectedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,reminderDate: freezed == reminderDate ? _self.reminderDate : reminderDate // ignore: cast_nullable_to_non_nullable
as DateTime?,receivedDate: freezed == receivedDate ? _self.receivedDate : receivedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdWaitingForDto].
extension GtdWaitingForDtoPatterns on GtdWaitingForDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdWaitingForDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdWaitingForDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdWaitingForDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdWaitingForDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdWaitingForDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdWaitingForDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  WaitingForStatus status,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  DateTime? receivedDate,  List<String> tags,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdWaitingForDto() when $default != null:
return $default(_that.id,_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.status,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.receivedDate,_that.tags,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  WaitingForStatus status,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  DateTime? receivedDate,  List<String> tags,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdWaitingForDto():
return $default(_that.id,_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.status,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.receivedDate,_that.tags,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  WaitingForStatus status,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  DateTime? receivedDate,  List<String> tags,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdWaitingForDto() when $default != null:
return $default(_that.id,_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.status,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.receivedDate,_that.tags,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdWaitingForDto implements GtdWaitingForDto {
  const _GtdWaitingForDto({required this.id, required this.userId, required this.description, required this.waitingFor, this.notes, this.projectId, this.relatedActionId, required this.status, this.requestedDate, this.expectedDate, this.reminderDate, this.receivedDate, final  List<String> tags = const [], required this.creationTime}): _tags = tags;
  factory _GtdWaitingForDto.fromJson(Map<String, dynamic> json) => _$GtdWaitingForDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String description;
@override final  String waitingFor;
@override final  String? notes;
@override final  String? projectId;
@override final  String? relatedActionId;
@override final  WaitingForStatus status;
@override final  DateTime? requestedDate;
@override final  DateTime? expectedDate;
@override final  DateTime? reminderDate;
@override final  DateTime? receivedDate;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  DateTime creationTime;

/// Create a copy of GtdWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdWaitingForDtoCopyWith<_GtdWaitingForDto> get copyWith => __$GtdWaitingForDtoCopyWithImpl<_GtdWaitingForDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdWaitingForDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdWaitingForDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.description, description) || other.description == description)&&(identical(other.waitingFor, waitingFor) || other.waitingFor == waitingFor)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.relatedActionId, relatedActionId) || other.relatedActionId == relatedActionId)&&(identical(other.status, status) || other.status == status)&&(identical(other.requestedDate, requestedDate) || other.requestedDate == requestedDate)&&(identical(other.expectedDate, expectedDate) || other.expectedDate == expectedDate)&&(identical(other.reminderDate, reminderDate) || other.reminderDate == reminderDate)&&(identical(other.receivedDate, receivedDate) || other.receivedDate == receivedDate)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,description,waitingFor,notes,projectId,relatedActionId,status,requestedDate,expectedDate,reminderDate,receivedDate,const DeepCollectionEquality().hash(_tags),creationTime);

@override
String toString() {
  return 'GtdWaitingForDto(id: $id, userId: $userId, description: $description, waitingFor: $waitingFor, notes: $notes, projectId: $projectId, relatedActionId: $relatedActionId, status: $status, requestedDate: $requestedDate, expectedDate: $expectedDate, reminderDate: $reminderDate, receivedDate: $receivedDate, tags: $tags, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdWaitingForDtoCopyWith<$Res> implements $GtdWaitingForDtoCopyWith<$Res> {
  factory _$GtdWaitingForDtoCopyWith(_GtdWaitingForDto value, $Res Function(_GtdWaitingForDto) _then) = __$GtdWaitingForDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String description, String waitingFor, String? notes, String? projectId, String? relatedActionId, WaitingForStatus status, DateTime? requestedDate, DateTime? expectedDate, DateTime? reminderDate, DateTime? receivedDate, List<String> tags, DateTime creationTime
});




}
/// @nodoc
class __$GtdWaitingForDtoCopyWithImpl<$Res>
    implements _$GtdWaitingForDtoCopyWith<$Res> {
  __$GtdWaitingForDtoCopyWithImpl(this._self, this._then);

  final _GtdWaitingForDto _self;
  final $Res Function(_GtdWaitingForDto) _then;

/// Create a copy of GtdWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? description = null,Object? waitingFor = null,Object? notes = freezed,Object? projectId = freezed,Object? relatedActionId = freezed,Object? status = null,Object? requestedDate = freezed,Object? expectedDate = freezed,Object? reminderDate = freezed,Object? receivedDate = freezed,Object? tags = null,Object? creationTime = null,}) {
  return _then(_GtdWaitingForDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,waitingFor: null == waitingFor ? _self.waitingFor : waitingFor // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,relatedActionId: freezed == relatedActionId ? _self.relatedActionId : relatedActionId // ignore: cast_nullable_to_non_nullable
as String?,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as WaitingForStatus,requestedDate: freezed == requestedDate ? _self.requestedDate : requestedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,expectedDate: freezed == expectedDate ? _self.expectedDate : expectedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,reminderDate: freezed == reminderDate ? _self.reminderDate : reminderDate // ignore: cast_nullable_to_non_nullable
as DateTime?,receivedDate: freezed == receivedDate ? _self.receivedDate : receivedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateWaitingForDto {

 String get userId; String get description; String get waitingFor; String? get notes; String? get projectId; String? get relatedActionId; DateTime? get requestedDate; DateTime? get expectedDate; DateTime? get reminderDate; List<String> get tags;
/// Create a copy of CreateUpdateWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateWaitingForDtoCopyWith<CreateUpdateWaitingForDto> get copyWith => _$CreateUpdateWaitingForDtoCopyWithImpl<CreateUpdateWaitingForDto>(this as CreateUpdateWaitingForDto, _$identity);

  /// Serializes this CreateUpdateWaitingForDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateWaitingForDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.description, description) || other.description == description)&&(identical(other.waitingFor, waitingFor) || other.waitingFor == waitingFor)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.relatedActionId, relatedActionId) || other.relatedActionId == relatedActionId)&&(identical(other.requestedDate, requestedDate) || other.requestedDate == requestedDate)&&(identical(other.expectedDate, expectedDate) || other.expectedDate == expectedDate)&&(identical(other.reminderDate, reminderDate) || other.reminderDate == reminderDate)&&const DeepCollectionEquality().equals(other.tags, tags));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,description,waitingFor,notes,projectId,relatedActionId,requestedDate,expectedDate,reminderDate,const DeepCollectionEquality().hash(tags));

@override
String toString() {
  return 'CreateUpdateWaitingForDto(userId: $userId, description: $description, waitingFor: $waitingFor, notes: $notes, projectId: $projectId, relatedActionId: $relatedActionId, requestedDate: $requestedDate, expectedDate: $expectedDate, reminderDate: $reminderDate, tags: $tags)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateWaitingForDtoCopyWith<$Res>  {
  factory $CreateUpdateWaitingForDtoCopyWith(CreateUpdateWaitingForDto value, $Res Function(CreateUpdateWaitingForDto) _then) = _$CreateUpdateWaitingForDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String description, String waitingFor, String? notes, String? projectId, String? relatedActionId, DateTime? requestedDate, DateTime? expectedDate, DateTime? reminderDate, List<String> tags
});




}
/// @nodoc
class _$CreateUpdateWaitingForDtoCopyWithImpl<$Res>
    implements $CreateUpdateWaitingForDtoCopyWith<$Res> {
  _$CreateUpdateWaitingForDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateWaitingForDto _self;
  final $Res Function(CreateUpdateWaitingForDto) _then;

/// Create a copy of CreateUpdateWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? description = null,Object? waitingFor = null,Object? notes = freezed,Object? projectId = freezed,Object? relatedActionId = freezed,Object? requestedDate = freezed,Object? expectedDate = freezed,Object? reminderDate = freezed,Object? tags = null,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,waitingFor: null == waitingFor ? _self.waitingFor : waitingFor // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,relatedActionId: freezed == relatedActionId ? _self.relatedActionId : relatedActionId // ignore: cast_nullable_to_non_nullable
as String?,requestedDate: freezed == requestedDate ? _self.requestedDate : requestedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,expectedDate: freezed == expectedDate ? _self.expectedDate : expectedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,reminderDate: freezed == reminderDate ? _self.reminderDate : reminderDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateWaitingForDto].
extension CreateUpdateWaitingForDtoPatterns on CreateUpdateWaitingForDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateWaitingForDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateWaitingForDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateWaitingForDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  List<String> tags)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto() when $default != null:
return $default(_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.tags);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  List<String> tags)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto():
return $default(_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.tags);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String description,  String waitingFor,  String? notes,  String? projectId,  String? relatedActionId,  DateTime? requestedDate,  DateTime? expectedDate,  DateTime? reminderDate,  List<String> tags)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateWaitingForDto() when $default != null:
return $default(_that.userId,_that.description,_that.waitingFor,_that.notes,_that.projectId,_that.relatedActionId,_that.requestedDate,_that.expectedDate,_that.reminderDate,_that.tags);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateWaitingForDto implements CreateUpdateWaitingForDto {
  const _CreateUpdateWaitingForDto({required this.userId, required this.description, required this.waitingFor, this.notes, this.projectId, this.relatedActionId, this.requestedDate, this.expectedDate, this.reminderDate, final  List<String> tags = const []}): _tags = tags;
  factory _CreateUpdateWaitingForDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateWaitingForDtoFromJson(json);

@override final  String userId;
@override final  String description;
@override final  String waitingFor;
@override final  String? notes;
@override final  String? projectId;
@override final  String? relatedActionId;
@override final  DateTime? requestedDate;
@override final  DateTime? expectedDate;
@override final  DateTime? reminderDate;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}


/// Create a copy of CreateUpdateWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateWaitingForDtoCopyWith<_CreateUpdateWaitingForDto> get copyWith => __$CreateUpdateWaitingForDtoCopyWithImpl<_CreateUpdateWaitingForDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateWaitingForDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateWaitingForDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.description, description) || other.description == description)&&(identical(other.waitingFor, waitingFor) || other.waitingFor == waitingFor)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.projectId, projectId) || other.projectId == projectId)&&(identical(other.relatedActionId, relatedActionId) || other.relatedActionId == relatedActionId)&&(identical(other.requestedDate, requestedDate) || other.requestedDate == requestedDate)&&(identical(other.expectedDate, expectedDate) || other.expectedDate == expectedDate)&&(identical(other.reminderDate, reminderDate) || other.reminderDate == reminderDate)&&const DeepCollectionEquality().equals(other._tags, _tags));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,description,waitingFor,notes,projectId,relatedActionId,requestedDate,expectedDate,reminderDate,const DeepCollectionEquality().hash(_tags));

@override
String toString() {
  return 'CreateUpdateWaitingForDto(userId: $userId, description: $description, waitingFor: $waitingFor, notes: $notes, projectId: $projectId, relatedActionId: $relatedActionId, requestedDate: $requestedDate, expectedDate: $expectedDate, reminderDate: $reminderDate, tags: $tags)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateWaitingForDtoCopyWith<$Res> implements $CreateUpdateWaitingForDtoCopyWith<$Res> {
  factory _$CreateUpdateWaitingForDtoCopyWith(_CreateUpdateWaitingForDto value, $Res Function(_CreateUpdateWaitingForDto) _then) = __$CreateUpdateWaitingForDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String description, String waitingFor, String? notes, String? projectId, String? relatedActionId, DateTime? requestedDate, DateTime? expectedDate, DateTime? reminderDate, List<String> tags
});




}
/// @nodoc
class __$CreateUpdateWaitingForDtoCopyWithImpl<$Res>
    implements _$CreateUpdateWaitingForDtoCopyWith<$Res> {
  __$CreateUpdateWaitingForDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateWaitingForDto _self;
  final $Res Function(_CreateUpdateWaitingForDto) _then;

/// Create a copy of CreateUpdateWaitingForDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? description = null,Object? waitingFor = null,Object? notes = freezed,Object? projectId = freezed,Object? relatedActionId = freezed,Object? requestedDate = freezed,Object? expectedDate = freezed,Object? reminderDate = freezed,Object? tags = null,}) {
  return _then(_CreateUpdateWaitingForDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,waitingFor: null == waitingFor ? _self.waitingFor : waitingFor // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,projectId: freezed == projectId ? _self.projectId : projectId // ignore: cast_nullable_to_non_nullable
as String?,relatedActionId: freezed == relatedActionId ? _self.relatedActionId : relatedActionId // ignore: cast_nullable_to_non_nullable
as String?,requestedDate: freezed == requestedDate ? _self.requestedDate : requestedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,expectedDate: freezed == expectedDate ? _self.expectedDate : expectedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,reminderDate: freezed == reminderDate ? _self.reminderDate : reminderDate // ignore: cast_nullable_to_non_nullable
as DateTime?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,
  ));
}


}

// dart format on
