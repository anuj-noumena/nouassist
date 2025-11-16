// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_reference_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdReferenceDto {

 String get id; String get userId; String get title; String? get content; ReferenceType get referenceType; String? get url; List<String> get attachments; List<String> get tags; String? get category; DateTime get creationTime;
/// Create a copy of GtdReferenceDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdReferenceDtoCopyWith<GtdReferenceDto> get copyWith => _$GtdReferenceDtoCopyWithImpl<GtdReferenceDto>(this as GtdReferenceDto, _$identity);

  /// Serializes this GtdReferenceDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdReferenceDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.content, content) || other.content == content)&&(identical(other.referenceType, referenceType) || other.referenceType == referenceType)&&(identical(other.url, url) || other.url == url)&&const DeepCollectionEquality().equals(other.attachments, attachments)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.category, category) || other.category == category)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,content,referenceType,url,const DeepCollectionEquality().hash(attachments),const DeepCollectionEquality().hash(tags),category,creationTime);

@override
String toString() {
  return 'GtdReferenceDto(id: $id, userId: $userId, title: $title, content: $content, referenceType: $referenceType, url: $url, attachments: $attachments, tags: $tags, category: $category, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdReferenceDtoCopyWith<$Res>  {
  factory $GtdReferenceDtoCopyWith(GtdReferenceDto value, $Res Function(GtdReferenceDto) _then) = _$GtdReferenceDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String title, String? content, ReferenceType referenceType, String? url, List<String> attachments, List<String> tags, String? category, DateTime creationTime
});




}
/// @nodoc
class _$GtdReferenceDtoCopyWithImpl<$Res>
    implements $GtdReferenceDtoCopyWith<$Res> {
  _$GtdReferenceDtoCopyWithImpl(this._self, this._then);

  final GtdReferenceDto _self;
  final $Res Function(GtdReferenceDto) _then;

/// Create a copy of GtdReferenceDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? content = freezed,Object? referenceType = null,Object? url = freezed,Object? attachments = null,Object? tags = null,Object? category = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,referenceType: null == referenceType ? _self.referenceType : referenceType // ignore: cast_nullable_to_non_nullable
as ReferenceType,url: freezed == url ? _self.url : url // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self.attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdReferenceDto].
extension GtdReferenceDtoPatterns on GtdReferenceDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdReferenceDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdReferenceDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdReferenceDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdReferenceDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdReferenceDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdReferenceDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? content,  ReferenceType referenceType,  String? url,  List<String> attachments,  List<String> tags,  String? category,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdReferenceDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.content,_that.referenceType,_that.url,_that.attachments,_that.tags,_that.category,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? content,  ReferenceType referenceType,  String? url,  List<String> attachments,  List<String> tags,  String? category,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdReferenceDto():
return $default(_that.id,_that.userId,_that.title,_that.content,_that.referenceType,_that.url,_that.attachments,_that.tags,_that.category,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String title,  String? content,  ReferenceType referenceType,  String? url,  List<String> attachments,  List<String> tags,  String? category,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdReferenceDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.content,_that.referenceType,_that.url,_that.attachments,_that.tags,_that.category,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdReferenceDto implements GtdReferenceDto {
  const _GtdReferenceDto({required this.id, required this.userId, required this.title, this.content, required this.referenceType, this.url, final  List<String> attachments = const [], final  List<String> tags = const [], this.category, required this.creationTime}): _attachments = attachments,_tags = tags;
  factory _GtdReferenceDto.fromJson(Map<String, dynamic> json) => _$GtdReferenceDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String title;
@override final  String? content;
@override final  ReferenceType referenceType;
@override final  String? url;
 final  List<String> _attachments;
@override@JsonKey() List<String> get attachments {
  if (_attachments is EqualUnmodifiableListView) return _attachments;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_attachments);
}

 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  String? category;
@override final  DateTime creationTime;

/// Create a copy of GtdReferenceDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdReferenceDtoCopyWith<_GtdReferenceDto> get copyWith => __$GtdReferenceDtoCopyWithImpl<_GtdReferenceDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdReferenceDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdReferenceDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.content, content) || other.content == content)&&(identical(other.referenceType, referenceType) || other.referenceType == referenceType)&&(identical(other.url, url) || other.url == url)&&const DeepCollectionEquality().equals(other._attachments, _attachments)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.category, category) || other.category == category)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,content,referenceType,url,const DeepCollectionEquality().hash(_attachments),const DeepCollectionEquality().hash(_tags),category,creationTime);

@override
String toString() {
  return 'GtdReferenceDto(id: $id, userId: $userId, title: $title, content: $content, referenceType: $referenceType, url: $url, attachments: $attachments, tags: $tags, category: $category, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdReferenceDtoCopyWith<$Res> implements $GtdReferenceDtoCopyWith<$Res> {
  factory _$GtdReferenceDtoCopyWith(_GtdReferenceDto value, $Res Function(_GtdReferenceDto) _then) = __$GtdReferenceDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String title, String? content, ReferenceType referenceType, String? url, List<String> attachments, List<String> tags, String? category, DateTime creationTime
});




}
/// @nodoc
class __$GtdReferenceDtoCopyWithImpl<$Res>
    implements _$GtdReferenceDtoCopyWith<$Res> {
  __$GtdReferenceDtoCopyWithImpl(this._self, this._then);

  final _GtdReferenceDto _self;
  final $Res Function(_GtdReferenceDto) _then;

/// Create a copy of GtdReferenceDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? content = freezed,Object? referenceType = null,Object? url = freezed,Object? attachments = null,Object? tags = null,Object? category = freezed,Object? creationTime = null,}) {
  return _then(_GtdReferenceDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,content: freezed == content ? _self.content : content // ignore: cast_nullable_to_non_nullable
as String?,referenceType: null == referenceType ? _self.referenceType : referenceType // ignore: cast_nullable_to_non_nullable
as ReferenceType,url: freezed == url ? _self.url : url // ignore: cast_nullable_to_non_nullable
as String?,attachments: null == attachments ? _self._attachments : attachments // ignore: cast_nullable_to_non_nullable
as List<String>,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,category: freezed == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}

// dart format on
