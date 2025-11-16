// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_someday_maybe_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$GtdSomedayMaybeDto {

 String get id; String get userId; String get title; String? get description; SomedayMaybeCategory get category; String? get areaOfFocusId; List<String> get tags; DateTime? get reviewDate; DateTime? get activatedDate; bool get isActivated; String? get activatedAsProjectId; DateTime get creationTime;
/// Create a copy of GtdSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdSomedayMaybeDtoCopyWith<GtdSomedayMaybeDto> get copyWith => _$GtdSomedayMaybeDtoCopyWithImpl<GtdSomedayMaybeDto>(this as GtdSomedayMaybeDto, _$identity);

  /// Serializes this GtdSomedayMaybeDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdSomedayMaybeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate)&&(identical(other.activatedDate, activatedDate) || other.activatedDate == activatedDate)&&(identical(other.isActivated, isActivated) || other.isActivated == isActivated)&&(identical(other.activatedAsProjectId, activatedAsProjectId) || other.activatedAsProjectId == activatedAsProjectId)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,description,category,areaOfFocusId,const DeepCollectionEquality().hash(tags),reviewDate,activatedDate,isActivated,activatedAsProjectId,creationTime);

@override
String toString() {
  return 'GtdSomedayMaybeDto(id: $id, userId: $userId, title: $title, description: $description, category: $category, areaOfFocusId: $areaOfFocusId, tags: $tags, reviewDate: $reviewDate, activatedDate: $activatedDate, isActivated: $isActivated, activatedAsProjectId: $activatedAsProjectId, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdSomedayMaybeDtoCopyWith<$Res>  {
  factory $GtdSomedayMaybeDtoCopyWith(GtdSomedayMaybeDto value, $Res Function(GtdSomedayMaybeDto) _then) = _$GtdSomedayMaybeDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String title, String? description, SomedayMaybeCategory category, String? areaOfFocusId, List<String> tags, DateTime? reviewDate, DateTime? activatedDate, bool isActivated, String? activatedAsProjectId, DateTime creationTime
});




}
/// @nodoc
class _$GtdSomedayMaybeDtoCopyWithImpl<$Res>
    implements $GtdSomedayMaybeDtoCopyWith<$Res> {
  _$GtdSomedayMaybeDtoCopyWithImpl(this._self, this._then);

  final GtdSomedayMaybeDto _self;
  final $Res Function(GtdSomedayMaybeDto) _then;

/// Create a copy of GtdSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? category = null,Object? areaOfFocusId = freezed,Object? tags = null,Object? reviewDate = freezed,Object? activatedDate = freezed,Object? isActivated = null,Object? activatedAsProjectId = freezed,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as SomedayMaybeCategory,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,reviewDate: freezed == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime?,activatedDate: freezed == activatedDate ? _self.activatedDate : activatedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isActivated: null == isActivated ? _self.isActivated : isActivated // ignore: cast_nullable_to_non_nullable
as bool,activatedAsProjectId: freezed == activatedAsProjectId ? _self.activatedAsProjectId : activatedAsProjectId // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdSomedayMaybeDto].
extension GtdSomedayMaybeDtoPatterns on GtdSomedayMaybeDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdSomedayMaybeDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdSomedayMaybeDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdSomedayMaybeDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate,  DateTime? activatedDate,  bool isActivated,  String? activatedAsProjectId,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate,_that.activatedDate,_that.isActivated,_that.activatedAsProjectId,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate,  DateTime? activatedDate,  bool isActivated,  String? activatedAsProjectId,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto():
return $default(_that.id,_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate,_that.activatedDate,_that.isActivated,_that.activatedAsProjectId,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate,  DateTime? activatedDate,  bool isActivated,  String? activatedAsProjectId,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdSomedayMaybeDto() when $default != null:
return $default(_that.id,_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate,_that.activatedDate,_that.isActivated,_that.activatedAsProjectId,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdSomedayMaybeDto implements GtdSomedayMaybeDto {
  const _GtdSomedayMaybeDto({required this.id, required this.userId, required this.title, this.description, required this.category, this.areaOfFocusId, final  List<String> tags = const [], this.reviewDate, this.activatedDate, this.isActivated = false, this.activatedAsProjectId, required this.creationTime}): _tags = tags;
  factory _GtdSomedayMaybeDto.fromJson(Map<String, dynamic> json) => _$GtdSomedayMaybeDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  SomedayMaybeCategory category;
@override final  String? areaOfFocusId;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  DateTime? reviewDate;
@override final  DateTime? activatedDate;
@override@JsonKey() final  bool isActivated;
@override final  String? activatedAsProjectId;
@override final  DateTime creationTime;

/// Create a copy of GtdSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdSomedayMaybeDtoCopyWith<_GtdSomedayMaybeDto> get copyWith => __$GtdSomedayMaybeDtoCopyWithImpl<_GtdSomedayMaybeDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdSomedayMaybeDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdSomedayMaybeDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate)&&(identical(other.activatedDate, activatedDate) || other.activatedDate == activatedDate)&&(identical(other.isActivated, isActivated) || other.isActivated == isActivated)&&(identical(other.activatedAsProjectId, activatedAsProjectId) || other.activatedAsProjectId == activatedAsProjectId)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,title,description,category,areaOfFocusId,const DeepCollectionEquality().hash(_tags),reviewDate,activatedDate,isActivated,activatedAsProjectId,creationTime);

@override
String toString() {
  return 'GtdSomedayMaybeDto(id: $id, userId: $userId, title: $title, description: $description, category: $category, areaOfFocusId: $areaOfFocusId, tags: $tags, reviewDate: $reviewDate, activatedDate: $activatedDate, isActivated: $isActivated, activatedAsProjectId: $activatedAsProjectId, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdSomedayMaybeDtoCopyWith<$Res> implements $GtdSomedayMaybeDtoCopyWith<$Res> {
  factory _$GtdSomedayMaybeDtoCopyWith(_GtdSomedayMaybeDto value, $Res Function(_GtdSomedayMaybeDto) _then) = __$GtdSomedayMaybeDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String title, String? description, SomedayMaybeCategory category, String? areaOfFocusId, List<String> tags, DateTime? reviewDate, DateTime? activatedDate, bool isActivated, String? activatedAsProjectId, DateTime creationTime
});




}
/// @nodoc
class __$GtdSomedayMaybeDtoCopyWithImpl<$Res>
    implements _$GtdSomedayMaybeDtoCopyWith<$Res> {
  __$GtdSomedayMaybeDtoCopyWithImpl(this._self, this._then);

  final _GtdSomedayMaybeDto _self;
  final $Res Function(_GtdSomedayMaybeDto) _then;

/// Create a copy of GtdSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? title = null,Object? description = freezed,Object? category = null,Object? areaOfFocusId = freezed,Object? tags = null,Object? reviewDate = freezed,Object? activatedDate = freezed,Object? isActivated = null,Object? activatedAsProjectId = freezed,Object? creationTime = null,}) {
  return _then(_GtdSomedayMaybeDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as SomedayMaybeCategory,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,reviewDate: freezed == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime?,activatedDate: freezed == activatedDate ? _self.activatedDate : activatedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isActivated: null == isActivated ? _self.isActivated : isActivated // ignore: cast_nullable_to_non_nullable
as bool,activatedAsProjectId: freezed == activatedAsProjectId ? _self.activatedAsProjectId : activatedAsProjectId // ignore: cast_nullable_to_non_nullable
as String?,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateSomedayMaybeDto {

 String get userId; String get title; String? get description; SomedayMaybeCategory get category; String? get areaOfFocusId; List<String> get tags; DateTime? get reviewDate;
/// Create a copy of CreateUpdateSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateSomedayMaybeDtoCopyWith<CreateUpdateSomedayMaybeDto> get copyWith => _$CreateUpdateSomedayMaybeDtoCopyWithImpl<CreateUpdateSomedayMaybeDto>(this as CreateUpdateSomedayMaybeDto, _$identity);

  /// Serializes this CreateUpdateSomedayMaybeDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateSomedayMaybeDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&const DeepCollectionEquality().equals(other.tags, tags)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,title,description,category,areaOfFocusId,const DeepCollectionEquality().hash(tags),reviewDate);

@override
String toString() {
  return 'CreateUpdateSomedayMaybeDto(userId: $userId, title: $title, description: $description, category: $category, areaOfFocusId: $areaOfFocusId, tags: $tags, reviewDate: $reviewDate)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateSomedayMaybeDtoCopyWith<$Res>  {
  factory $CreateUpdateSomedayMaybeDtoCopyWith(CreateUpdateSomedayMaybeDto value, $Res Function(CreateUpdateSomedayMaybeDto) _then) = _$CreateUpdateSomedayMaybeDtoCopyWithImpl;
@useResult
$Res call({
 String userId, String title, String? description, SomedayMaybeCategory category, String? areaOfFocusId, List<String> tags, DateTime? reviewDate
});




}
/// @nodoc
class _$CreateUpdateSomedayMaybeDtoCopyWithImpl<$Res>
    implements $CreateUpdateSomedayMaybeDtoCopyWith<$Res> {
  _$CreateUpdateSomedayMaybeDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateSomedayMaybeDto _self;
  final $Res Function(CreateUpdateSomedayMaybeDto) _then;

/// Create a copy of CreateUpdateSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? category = null,Object? areaOfFocusId = freezed,Object? tags = null,Object? reviewDate = freezed,}) {
  return _then(_self.copyWith(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as SomedayMaybeCategory,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,reviewDate: freezed == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateSomedayMaybeDto].
extension CreateUpdateSomedayMaybeDtoPatterns on CreateUpdateSomedayMaybeDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateSomedayMaybeDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateSomedayMaybeDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateSomedayMaybeDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto():
return $default(_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String userId,  String title,  String? description,  SomedayMaybeCategory category,  String? areaOfFocusId,  List<String> tags,  DateTime? reviewDate)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateSomedayMaybeDto() when $default != null:
return $default(_that.userId,_that.title,_that.description,_that.category,_that.areaOfFocusId,_that.tags,_that.reviewDate);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateSomedayMaybeDto implements CreateUpdateSomedayMaybeDto {
  const _CreateUpdateSomedayMaybeDto({required this.userId, required this.title, this.description, required this.category, this.areaOfFocusId, final  List<String> tags = const [], this.reviewDate}): _tags = tags;
  factory _CreateUpdateSomedayMaybeDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateSomedayMaybeDtoFromJson(json);

@override final  String userId;
@override final  String title;
@override final  String? description;
@override final  SomedayMaybeCategory category;
@override final  String? areaOfFocusId;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

@override final  DateTime? reviewDate;

/// Create a copy of CreateUpdateSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateSomedayMaybeDtoCopyWith<_CreateUpdateSomedayMaybeDto> get copyWith => __$CreateUpdateSomedayMaybeDtoCopyWithImpl<_CreateUpdateSomedayMaybeDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateSomedayMaybeDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateSomedayMaybeDto&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.category, category) || other.category == category)&&(identical(other.areaOfFocusId, areaOfFocusId) || other.areaOfFocusId == areaOfFocusId)&&const DeepCollectionEquality().equals(other._tags, _tags)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,userId,title,description,category,areaOfFocusId,const DeepCollectionEquality().hash(_tags),reviewDate);

@override
String toString() {
  return 'CreateUpdateSomedayMaybeDto(userId: $userId, title: $title, description: $description, category: $category, areaOfFocusId: $areaOfFocusId, tags: $tags, reviewDate: $reviewDate)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateSomedayMaybeDtoCopyWith<$Res> implements $CreateUpdateSomedayMaybeDtoCopyWith<$Res> {
  factory _$CreateUpdateSomedayMaybeDtoCopyWith(_CreateUpdateSomedayMaybeDto value, $Res Function(_CreateUpdateSomedayMaybeDto) _then) = __$CreateUpdateSomedayMaybeDtoCopyWithImpl;
@override @useResult
$Res call({
 String userId, String title, String? description, SomedayMaybeCategory category, String? areaOfFocusId, List<String> tags, DateTime? reviewDate
});




}
/// @nodoc
class __$CreateUpdateSomedayMaybeDtoCopyWithImpl<$Res>
    implements _$CreateUpdateSomedayMaybeDtoCopyWith<$Res> {
  __$CreateUpdateSomedayMaybeDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateSomedayMaybeDto _self;
  final $Res Function(_CreateUpdateSomedayMaybeDto) _then;

/// Create a copy of CreateUpdateSomedayMaybeDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? userId = null,Object? title = null,Object? description = freezed,Object? category = null,Object? areaOfFocusId = freezed,Object? tags = null,Object? reviewDate = freezed,}) {
  return _then(_CreateUpdateSomedayMaybeDto(
userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: freezed == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String?,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as SomedayMaybeCategory,areaOfFocusId: freezed == areaOfFocusId ? _self.areaOfFocusId : areaOfFocusId // ignore: cast_nullable_to_non_nullable
as String?,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,reviewDate: freezed == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}

// dart format on
