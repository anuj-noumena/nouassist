// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_inbox_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdInboxDto {

 String get id; String get userId; String get content; String? get notes; InboxItemType get itemType; InboxStatus get status; List<String> get tags; String? get source; DateTime? get processedDate; String? get processedAs; DateTime get creationTime;
/// Create a copy of GtdInboxDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdInboxDtoCopyWith<GtdInboxDto> get copyWith => _$GtdInboxDtoCopyWithImpl<GtdInboxDto>(this as GtdInboxDto, _$identity);

  /// Serializes this GtdInboxDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdInboxDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.content, content) || other.content == content)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.itemType, itemType) || other.itemType == itemType)&&(identical(other.status, status) || other.status == status)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.source, source) || other.source == source)&&(identical(other.processedDate, processedDate) || other.processedDate == processedDate)&&(identical(other.processedAs, processedAs) || other.processedAs == processedAs)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,content,notes,itemType,status,const DeepCollectionEquality().hash(tags),source,processedDate,processedAs,creationTime);

@override
String toString() {
  return 'GtdInboxDto(id: $id, userId: $userId, content: $content, notes: $notes, itemType: $itemType, status: $status, tags: $tags, source: $source, processedDate: $processedDate, processedAs: $processedAs, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdInboxDtoCopyWith<$Res>  {
  factory $GtdInboxDtoCopyWith(GtdInboxDto value, $Res Function(GtdInboxDto) _then) = _$GtdInboxDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String content, String? notes, InboxItemType itemType, InboxStatus status, List<String> tags, String? source, DateTime? processedDate, String? processedAs, DateTime creationTime
});




}
/// @nodoc
class _$GtdInboxDtoCopyWithImpl<$Res>
    implements $GtdInboxDtoCopyWith<$Res> {
  _$GtdInboxDtoCopyWithImpl(this._self, this._then);

  final GtdInboxDto _self;
  final $Res Function(GtdInboxDto) _then;

/// Create a copy of GtdInboxDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? content = null,Object? notes = freezed,Object? itemType = null,Object? status = null,Object? tags = null,Object? source = freezed,Object? processedDate = freezed,Object? processedAs = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,itemType: null == itemType ? _self.itemType : itemType // ignore: cast_nullable_to_non_nullable
as InboxItemType,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as InboxStatus,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,source: freezed == source ? _self.source : source // ignore: cast_nullable_to_non_nullable
as String?,processedDate: freezed == processedDate ? _self.processedDate : processedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,processedAs: freezed == processedAs ? _self.processedAs : processedAs // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdInboxDto].
extension GtdInboxDtoPatterns on GtdInboxDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdInboxDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdInboxDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdInboxDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdInboxDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdInboxDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdInboxDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String content,  String? notes,  InboxItemType itemType,  InboxStatus status,  List<String> tags,  String? source,  DateTime? processedDate,  String? processedAs,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdInboxDto() when $default != null:
return $default(_that.id,_that.userId,_that.content,_that.notes,_that.itemType,_that.status,_that.tags,_that.source,_that.processedDate,_that.processedAs,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String content,  String? notes,  InboxItemType itemType,  InboxStatus status,  List<String> tags,  String? source,  DateTime? processedDate,  String? processedAs,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdInboxDto():
return $default(_that.id,_that.userId,_that.content,_that.notes,_that.itemType,_that.status,_that.tags,_that.source,_that.processedDate,_that.processedAs,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String content,  String? notes,  InboxItemType itemType,  InboxStatus status,  List<String> tags,  String? source,  DateTime? processedDate,  String? processedAs,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdInboxDto() when $default != null:
return $default(_that.id,_that.userId,_that.content,_that.notes,_that.itemType,_that.status,_that.tags,_that.source,_that.processedDate,_that.processedAs,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdInboxDto implements GtdInboxDto {
  const _GtdInboxDto({required this.id, required this.userId, required this.content, this.notes, required this.itemType, required this.status, final  List<String> tags = const [], this.source, this.processedDate, this.processedAs, required this.creationTime}): _tags = tags;
  factory _GtdInboxDto.fromJson(Map<String, dynamic> json) => _$GtdInboxDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String content;
@override final  String? notes;
@override final  InboxItemType itemType;
@override final  InboxStatus status;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String? source;
@override final  DateTime? processedDate;
@override final  String? processedAs;
@override final  DateTime creationTime;

/// Create a copy of GtdInboxDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdInboxDtoCopyWith<_GtdInboxDto> get copyWith => __$GtdInboxDtoCopyWithImpl<_GtdInboxDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdInboxDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdInboxDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.content, content) || other.content == content)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.itemType, itemType) || other.itemType == itemType)&&(identical(other.status, status) || other.status == status)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.source, source) || other.source == source)&&(identical(other.processedDate, processedDate) || other.processedDate == processedDate)&&(identical(other.processedAs, processedAs) || other.processedAs == processedAs)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,content,notes,itemType,status,const DeepCollectionEquality().hash(_tags),source,processedDate,processedAs,creationTime);

@override
String toString() {
  return 'GtdInboxDto(id: $id, userId: $userId, content: $content, notes: $notes, itemType: $itemType, status: $status, tags: $tags, source: $source, processedDate: $processedDate, processedAs: $processedAs, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdInboxDtoCopyWith<$Res> implements $GtdInboxDtoCopyWith<$Res> {
  factory _$GtdInboxDtoCopyWith(_GtdInboxDto value, $Res Function(_GtdInboxDto) _then) = __$GtdInboxDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String content, String? notes, InboxItemType itemType, InboxStatus status, List<String> tags, String? source, DateTime? processedDate, String? processedAs, DateTime creationTime
});




}
/// @nodoc
class __$GtdInboxDtoCopyWithImpl<$Res>
    implements _$GtdInboxDtoCopyWith<$Res> {
  __$GtdInboxDtoCopyWithImpl(this._self, this._then);

  final _GtdInboxDto _self;
  final $Res Function(_GtdInboxDto) _then;

/// Create a copy of GtdInboxDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? content = null,Object? notes = freezed,Object? itemType = null,Object? status = null,Object? tags = null,Object? source = freezed,Object? processedDate = freezed,Object? processedAs = freezed,Object? creationTime = null,}) {
  return _then(_GtdInboxDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,itemType: null == itemType ? _self.itemType : itemType // ignore: cast_nullable_to_non_nullable
as InboxItemType,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as InboxStatus,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,source: freezed == source ? _self.source : source // ignore: cast_nullable_to_non_nullable
as String?,processedDate: freezed == processedDate ? _self.processedDate : processedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,processedAs: freezed == processedAs ? _self.processedAs : processedAs // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateInboxDto {

 String get userId; String get content; String? get notes; InboxItemType get itemType; List<String> get tags; String? get source;
/// Create a copy of CreateUpdateInboxDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateInboxDtoCopyWith<CreateUpdateInboxDto> get copyWith => _$CreateUpdateInboxDtoCopyWithImpl<CreateUpdateInboxDto>(this as CreateUpdateInboxDto, _$identity);

  /// Serializes this CreateUpdateInboxDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateInboxDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.content, content) || other.content == content)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.itemType, itemType) || other.itemType == itemType)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.source, source) || other.source == source));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,content,notes,itemType,const DeepCollectionEquality().hash(tags),source);

@override
String toString() {
  return 'CreateUpdateInboxDto(userId: $userId, content: $content, notes: $notes, itemType: $itemType, tags: $tags, source: $source)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateInboxDtoCopyWith<$Res>  {
  factory $CreateUpdateInboxDtoCopyWith(CreateUpdateInboxDto value, $Res Function(CreateUpdateInboxDto) _then) = _$CreateUpdateInboxDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String content, String? notes, InboxItemType itemType, List<String> tags, String? source
});




}
/// @nodoc
class _$CreateUpdateInboxDtoCopyWithImpl<$Res>
    implements $CreateUpdateInboxDtoCopyWith<$Res> {
  _$CreateUpdateInboxDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateInboxDto _self;
  final $Res Function(CreateUpdateInboxDto) _then;

/// Create a copy of CreateUpdateInboxDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? content = null,Object? notes = freezed,Object? itemType = null,Object? tags = null,Object? source = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,itemType: null == itemType ? _self.itemType : itemType // ignore: cast_nullable_to_non_nullable
as InboxItemType,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,source: freezed == source ? _self.source : source // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateInboxDto].
extension CreateUpdateInboxDtoPatterns on CreateUpdateInboxDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateInboxDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateInboxDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateInboxDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateInboxDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateInboxDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateInboxDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String content,  String? notes,  InboxItemType itemType,  List<String> tags,  String? source)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateInboxDto() when $default != null:
return $default(_that.userId,_that.content,_that.notes,_that.itemType,_that.tags,_that.source);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String content,  String? notes,  InboxItemType itemType,  List<String> tags,  String? source)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateInboxDto():
return $default(_that.userId,_that.content,_that.notes,_that.itemType,_that.tags,_that.source);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String content,  String? notes,  InboxItemType itemType,  List<String> tags,  String? source)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateInboxDto() when $default != null:
return $default(_that.userId,_that.content,_that.notes,_that.itemType,_that.tags,_that.source);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateInboxDto implements CreateUpdateInboxDto {
  const _CreateUpdateInboxDto({required this.userId, required this.content, this.notes, required this.itemType, final  List<String> tags = const [], this.source}): _tags = tags;
  factory _CreateUpdateInboxDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateInboxDtoFromJson(json);

@override final  String userId;
@override final  String content;
@override final  String? notes;
@override final  InboxItemType itemType;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String? source;

/// Create a copy of CreateUpdateInboxDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateInboxDtoCopyWith<_CreateUpdateInboxDto> get copyWith => __$CreateUpdateInboxDtoCopyWithImpl<_CreateUpdateInboxDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateInboxDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateInboxDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.content, content) || other.content == content)&&(identical(other.notes, notes) || other.notes == notes)&&(identical(other.itemType, itemType) || other.itemType == itemType)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.source, source) || other.source == source));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,content,notes,itemType,const DeepCollectionEquality().hash(_tags),source);

@override
String toString() {
  return 'CreateUpdateInboxDto(userId: $userId, content: $content, notes: $notes, itemType: $itemType, tags: $tags, source: $source)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateInboxDtoCopyWith<$Res> implements $CreateUpdateInboxDtoCopyWith<$Res> {
  factory _$CreateUpdateInboxDtoCopyWith(_CreateUpdateInboxDto value, $Res Function(_CreateUpdateInboxDto) _then) = __$CreateUpdateInboxDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String content, String? notes, InboxItemType itemType, List<String> tags, String? source
});




}
/// @nodoc
class __$CreateUpdateInboxDtoCopyWithImpl<$Res>
    implements _$CreateUpdateInboxDtoCopyWith<$Res> {
  __$CreateUpdateInboxDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateInboxDto _self;
  final $Res Function(_CreateUpdateInboxDto) _then;

/// Create a copy of CreateUpdateInboxDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? content = null,Object? notes = freezed,Object? itemType = null,Object? tags = null,Object? source = freezed,}) {
  return _then(_CreateUpdateInboxDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,content: null == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,itemType: null == itemType ? _self.itemType : itemType // ignore: cast_nullable_to_non_nullable
as InboxItemType,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,source: freezed == source ? _self.source : source // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
