// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'course_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$CourseDto {

 String get id; String get title; String get description; String? get thumbnail; String get instructorId; String get instructorName; CourseCategory get category; CourseDifficulty get difficulty; CourseStatus get status; List<String> get tags; List<String> get prerequisites; int get estimatedDuration; int get lessonCount; int get enrollmentCount; double get rating; int get reviewCount; bool get isFeatured; bool get isCertified; double? get price; bool get isFree; DateTime get creationTime; DateTime? get lastModificationTime; DateTime? get publishedDate;
/// Create a copy of CourseDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CourseDtoCopyWith<CourseDto> get copyWith => _$CourseDtoCopyWithImpl<CourseDto>(this as CourseDto, _$identity);

  /// Serializes this CourseDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CourseDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.thumbnail, thumbnail) || other.thumbnail == thumbnail)&&(identical(other.instructorId, instructorId) || other.instructorId == instructorId)&&(identical(other.instructorName, instructorName) || other.instructorName == instructorName)&&(identical(other.category, category) || other.category == category)&&(identical(other.difficulty, difficulty) || other.difficulty == difficulty)&&(identical(other.status, status) || other.status == status)&&const DeepCollectionEquality().equals(other.tags, tags)&&const DeepCollectionEquality().equals(other.prerequisites, prerequisites)&&(identical(other.estimatedDuration, estimatedDuration) || other.estimatedDuration == estimatedDuration)&&(identical(other.lessonCount, lessonCount) || other.lessonCount == lessonCount)&&(identical(other.enrollmentCount, enrollmentCount) || other.enrollmentCount == enrollmentCount)&&(identical(other.rating, rating) || other.rating == rating)&&(identical(other.reviewCount, reviewCount) || other.reviewCount == reviewCount)&&(identical(other.isFeatured, isFeatured) || other.isFeatured == isFeatured)&&(identical(other.isCertified, isCertified) || other.isCertified == isCertified)&&(identical(other.price, price) || other.price == price)&&(identical(other.isFree, isFree) || other.isFree == isFree)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime)&&(identical(other.publishedDate, publishedDate) || other.publishedDate == publishedDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,title,description,thumbnail,instructorId,instructorName,category,difficulty,status,const DeepCollectionEquality().hash(tags),const DeepCollectionEquality().hash(prerequisites),estimatedDuration,lessonCount,enrollmentCount,rating,reviewCount,isFeatured,isCertified,price,isFree,creationTime,lastModificationTime,publishedDate]);

@override
String toString() {
  return 'CourseDto(id: $id, title: $title, description: $description, thumbnail: $thumbnail, instructorId: $instructorId, instructorName: $instructorName, category: $category, difficulty: $difficulty, status: $status, tags: $tags, prerequisites: $prerequisites, estimatedDuration: $estimatedDuration, lessonCount: $lessonCount, enrollmentCount: $enrollmentCount, rating: $rating, reviewCount: $reviewCount, isFeatured: $isFeatured, isCertified: $isCertified, price: $price, isFree: $isFree, creationTime: $creationTime, lastModificationTime: $lastModificationTime, publishedDate: $publishedDate)';
}


}

/// @nodoc
abstract mixin class $CourseDtoCopyWith<$Res>  {
  factory $CourseDtoCopyWith(CourseDto value, $Res Function(CourseDto) _then) = _$CourseDtoCopyWithImpl;
@useResult
$Res call({
 String id, String title, String description, String? thumbnail, String instructorId, String instructorName, CourseCategory category, CourseDifficulty difficulty, CourseStatus status, List<String> tags, List<String> prerequisites, int estimatedDuration, int lessonCount, int enrollmentCount, double rating, int reviewCount, bool isFeatured, bool isCertified, double? price, bool isFree, DateTime creationTime, DateTime? lastModificationTime, DateTime? publishedDate
});




}
/// @nodoc
class _$CourseDtoCopyWithImpl<$Res>
    implements $CourseDtoCopyWith<$Res> {
  _$CourseDtoCopyWithImpl(this._self, this._then);

  final CourseDto _self;
  final $Res Function(CourseDto) _then;

/// Create a copy of CourseDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? title = null,Object? description = null,Object? thumbnail = freezed,Object? instructorId = null,Object? instructorName = null,Object? category = null,Object? difficulty = null,Object? status = null,Object? tags = null,Object? prerequisites = null,Object? estimatedDuration = null,Object? lessonCount = null,Object? enrollmentCount = null,Object? rating = null,Object? reviewCount = null,Object? isFeatured = null,Object? isCertified = null,Object? price = freezed,Object? isFree = null,Object? creationTime = null,Object? lastModificationTime = freezed,Object? publishedDate = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,thumbnail: freezed == thumbnail ? _self.thumbnail : thumbnail // ignore: cast_nullable_to_non_nullable
as String?,instructorId: null == instructorId ? _self.instructorId : instructorId // ignore: cast_nullable_to_non_nullable
as String,instructorName: null == instructorName ? _self.instructorName : instructorName // ignore: cast_nullable_to_non_nullable
as String,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as CourseCategory,difficulty: null == difficulty ? _self.difficulty : difficulty // ignore: cast_nullable_to_non_nullable
as CourseDifficulty,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as CourseStatus,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,prerequisites: null == prerequisites ? _self.prerequisites : prerequisites // ignore: cast_nullable_to_non_nullable
as List<String>,estimatedDuration: null == estimatedDuration ? _self.estimatedDuration : estimatedDuration // ignore: cast_nullable_to_non_nullable
as int,lessonCount: null == lessonCount ? _self.lessonCount : lessonCount // ignore: cast_nullable_to_non_nullable
as int,enrollmentCount: null == enrollmentCount ? _self.enrollmentCount : enrollmentCount // ignore: cast_nullable_to_non_nullable
as int,rating: null == rating ? _self.rating : rating // ignore: cast_nullable_to_non_nullable
as double,reviewCount: null == reviewCount ? _self.reviewCount : reviewCount // ignore: cast_nullable_to_non_nullable
as int,isFeatured: null == isFeatured ? _self.isFeatured : isFeatured // ignore: cast_nullable_to_non_nullable
as bool,isCertified: null == isCertified ? _self.isCertified : isCertified // ignore: cast_nullable_to_non_nullable
as bool,price: freezed == price ? _self.price : price // ignore: cast_nullable_to_non_nullable
as double?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,publishedDate: freezed == publishedDate ? _self.publishedDate : publishedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [CourseDto].
extension CourseDtoPatterns on CourseDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CourseDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CourseDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CourseDto value)  $default,){
final _that = this;
switch (_that) {
case _CourseDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CourseDto value)?  $default,){
final _that = this;
switch (_that) {
case _CourseDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String title,  String description,  String? thumbnail,  String instructorId,  String instructorName,  CourseCategory category,  CourseDifficulty difficulty,  CourseStatus status,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  int lessonCount,  int enrollmentCount,  double rating,  int reviewCount,  bool isFeatured,  bool isCertified,  double? price,  bool isFree,  DateTime creationTime,  DateTime? lastModificationTime,  DateTime? publishedDate)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CourseDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.instructorName,_that.category,_that.difficulty,_that.status,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.lessonCount,_that.enrollmentCount,_that.rating,_that.reviewCount,_that.isFeatured,_that.isCertified,_that.price,_that.isFree,_that.creationTime,_that.lastModificationTime,_that.publishedDate);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String title,  String description,  String? thumbnail,  String instructorId,  String instructorName,  CourseCategory category,  CourseDifficulty difficulty,  CourseStatus status,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  int lessonCount,  int enrollmentCount,  double rating,  int reviewCount,  bool isFeatured,  bool isCertified,  double? price,  bool isFree,  DateTime creationTime,  DateTime? lastModificationTime,  DateTime? publishedDate)  $default,) {final _that = this;
switch (_that) {
case _CourseDto():
return $default(_that.id,_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.instructorName,_that.category,_that.difficulty,_that.status,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.lessonCount,_that.enrollmentCount,_that.rating,_that.reviewCount,_that.isFeatured,_that.isCertified,_that.price,_that.isFree,_that.creationTime,_that.lastModificationTime,_that.publishedDate);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String title,  String description,  String? thumbnail,  String instructorId,  String instructorName,  CourseCategory category,  CourseDifficulty difficulty,  CourseStatus status,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  int lessonCount,  int enrollmentCount,  double rating,  int reviewCount,  bool isFeatured,  bool isCertified,  double? price,  bool isFree,  DateTime creationTime,  DateTime? lastModificationTime,  DateTime? publishedDate)?  $default,) {final _that = this;
switch (_that) {
case _CourseDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.instructorName,_that.category,_that.difficulty,_that.status,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.lessonCount,_that.enrollmentCount,_that.rating,_that.reviewCount,_that.isFeatured,_that.isCertified,_that.price,_that.isFree,_that.creationTime,_that.lastModificationTime,_that.publishedDate);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CourseDto implements CourseDto {
  const _CourseDto({required this.id, required this.title, required this.description, this.thumbnail, required this.instructorId, required this.instructorName, required this.category, required this.difficulty, required this.status, final  List<String> tags = const [], final  List<String> prerequisites = const [], required this.estimatedDuration, required this.lessonCount, this.enrollmentCount = 0, this.rating = 0.0, this.reviewCount = 0, this.isFeatured = false, this.isCertified = false, this.price, this.isFree = false, required this.creationTime, this.lastModificationTime, this.publishedDate}): _tags = tags,_prerequisites = prerequisites;
  factory _CourseDto.fromJson(Map<String, dynamic> json) => _$CourseDtoFromJson(json);

@override final  String id;
@override final  String title;
@override final  String description;
@override final  String? thumbnail;
@override final  String instructorId;
@override final  String instructorName;
@override final  CourseCategory category;
@override final  CourseDifficulty difficulty;
@override final  CourseStatus status;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

 final  List<String> _prerequisites;
@override@JsonKey() List<String> get prerequisites {
  if (_prerequisites is EqualUnmodifiableListView) return _prerequisites;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_prerequisites);
}

@override final  int estimatedDuration;
@override final  int lessonCount;
@override@JsonKey() final  int enrollmentCount;
@override@JsonKey() final  double rating;
@override@JsonKey() final  int reviewCount;
@override@JsonKey() final  bool isFeatured;
@override@JsonKey() final  bool isCertified;
@override final  double? price;
@override@JsonKey() final  bool isFree;
@override final  DateTime creationTime;
@override final  DateTime? lastModificationTime;
@override final  DateTime? publishedDate;

/// Create a copy of CourseDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CourseDtoCopyWith<_CourseDto> get copyWith => __$CourseDtoCopyWithImpl<_CourseDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CourseDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CourseDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.thumbnail, thumbnail) || other.thumbnail == thumbnail)&&(identical(other.instructorId, instructorId) || other.instructorId == instructorId)&&(identical(other.instructorName, instructorName) || other.instructorName == instructorName)&&(identical(other.category, category) || other.category == category)&&(identical(other.difficulty, difficulty) || other.difficulty == difficulty)&&(identical(other.status, status) || other.status == status)&&const DeepCollectionEquality().equals(other._tags, _tags)&&const DeepCollectionEquality().equals(other._prerequisites, _prerequisites)&&(identical(other.estimatedDuration, estimatedDuration) || other.estimatedDuration == estimatedDuration)&&(identical(other.lessonCount, lessonCount) || other.lessonCount == lessonCount)&&(identical(other.enrollmentCount, enrollmentCount) || other.enrollmentCount == enrollmentCount)&&(identical(other.rating, rating) || other.rating == rating)&&(identical(other.reviewCount, reviewCount) || other.reviewCount == reviewCount)&&(identical(other.isFeatured, isFeatured) || other.isFeatured == isFeatured)&&(identical(other.isCertified, isCertified) || other.isCertified == isCertified)&&(identical(other.price, price) || other.price == price)&&(identical(other.isFree, isFree) || other.isFree == isFree)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime)&&(identical(other.lastModificationTime, lastModificationTime) || other.lastModificationTime == lastModificationTime)&&(identical(other.publishedDate, publishedDate) || other.publishedDate == publishedDate));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hashAll([runtimeType,id,title,description,thumbnail,instructorId,instructorName,category,difficulty,status,const DeepCollectionEquality().hash(_tags),const DeepCollectionEquality().hash(_prerequisites),estimatedDuration,lessonCount,enrollmentCount,rating,reviewCount,isFeatured,isCertified,price,isFree,creationTime,lastModificationTime,publishedDate]);

@override
String toString() {
  return 'CourseDto(id: $id, title: $title, description: $description, thumbnail: $thumbnail, instructorId: $instructorId, instructorName: $instructorName, category: $category, difficulty: $difficulty, status: $status, tags: $tags, prerequisites: $prerequisites, estimatedDuration: $estimatedDuration, lessonCount: $lessonCount, enrollmentCount: $enrollmentCount, rating: $rating, reviewCount: $reviewCount, isFeatured: $isFeatured, isCertified: $isCertified, price: $price, isFree: $isFree, creationTime: $creationTime, lastModificationTime: $lastModificationTime, publishedDate: $publishedDate)';
}


}

/// @nodoc
abstract mixin class _$CourseDtoCopyWith<$Res> implements $CourseDtoCopyWith<$Res> {
  factory _$CourseDtoCopyWith(_CourseDto value, $Res Function(_CourseDto) _then) = __$CourseDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String title, String description, String? thumbnail, String instructorId, String instructorName, CourseCategory category, CourseDifficulty difficulty, CourseStatus status, List<String> tags, List<String> prerequisites, int estimatedDuration, int lessonCount, int enrollmentCount, double rating, int reviewCount, bool isFeatured, bool isCertified, double? price, bool isFree, DateTime creationTime, DateTime? lastModificationTime, DateTime? publishedDate
});




}
/// @nodoc
class __$CourseDtoCopyWithImpl<$Res>
    implements _$CourseDtoCopyWith<$Res> {
  __$CourseDtoCopyWithImpl(this._self, this._then);

  final _CourseDto _self;
  final $Res Function(_CourseDto) _then;

/// Create a copy of CourseDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? title = null,Object? description = null,Object? thumbnail = freezed,Object? instructorId = null,Object? instructorName = null,Object? category = null,Object? difficulty = null,Object? status = null,Object? tags = null,Object? prerequisites = null,Object? estimatedDuration = null,Object? lessonCount = null,Object? enrollmentCount = null,Object? rating = null,Object? reviewCount = null,Object? isFeatured = null,Object? isCertified = null,Object? price = freezed,Object? isFree = null,Object? creationTime = null,Object? lastModificationTime = freezed,Object? publishedDate = freezed,}) {
  return _then(_CourseDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,thumbnail: freezed == thumbnail ? _self.thumbnail : thumbnail // ignore: cast_nullable_to_non_nullable
as String?,instructorId: null == instructorId ? _self.instructorId : instructorId // ignore: cast_nullable_to_non_nullable
as String,instructorName: null == instructorName ? _self.instructorName : instructorName // ignore: cast_nullable_to_non_nullable
as String,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as CourseCategory,difficulty: null == difficulty ? _self.difficulty : difficulty // ignore: cast_nullable_to_non_nullable
as CourseDifficulty,status: null == status ? _self.status : status // ignore: cast_nullable_to_non_nullable
as CourseStatus,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,prerequisites: null == prerequisites ? _self._prerequisites : prerequisites // ignore: cast_nullable_to_non_nullable
as List<String>,estimatedDuration: null == estimatedDuration ? _self.estimatedDuration : estimatedDuration // ignore: cast_nullable_to_non_nullable
as int,lessonCount: null == lessonCount ? _self.lessonCount : lessonCount // ignore: cast_nullable_to_non_nullable
as int,enrollmentCount: null == enrollmentCount ? _self.enrollmentCount : enrollmentCount // ignore: cast_nullable_to_non_nullable
as int,rating: null == rating ? _self.rating : rating // ignore: cast_nullable_to_non_nullable
as double,reviewCount: null == reviewCount ? _self.reviewCount : reviewCount // ignore: cast_nullable_to_non_nullable
as int,isFeatured: null == isFeatured ? _self.isFeatured : isFeatured // ignore: cast_nullable_to_non_nullable
as bool,isCertified: null == isCertified ? _self.isCertified : isCertified // ignore: cast_nullable_to_non_nullable
as bool,price: freezed == price ? _self.price : price // ignore: cast_nullable_to_non_nullable
as double?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,lastModificationTime: freezed == lastModificationTime ? _self.lastModificationTime : lastModificationTime // ignore: cast_nullable_to_non_nullable
as DateTime?,publishedDate: freezed == publishedDate ? _self.publishedDate : publishedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateCourseDto {

 String get title; String get description; String? get thumbnail; String get instructorId; CourseCategory get category; CourseDifficulty get difficulty; List<String> get tags; List<String> get prerequisites; int get estimatedDuration; bool get isFeatured; bool get isCertified; double? get price; bool get isFree;
/// Create a copy of CreateUpdateCourseDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateCourseDtoCopyWith<CreateUpdateCourseDto> get copyWith => _$CreateUpdateCourseDtoCopyWithImpl<CreateUpdateCourseDto>(this as CreateUpdateCourseDto, _$identity);

  /// Serializes this CreateUpdateCourseDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateCourseDto&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.thumbnail, thumbnail) || other.thumbnail == thumbnail)&&(identical(other.instructorId, instructorId) || other.instructorId == instructorId)&&(identical(other.category, category) || other.category == category)&&(identical(other.difficulty, difficulty) || other.difficulty == difficulty)&&const DeepCollectionEquality().equals(other.tags, tags)&&const DeepCollectionEquality().equals(other.prerequisites, prerequisites)&&(identical(other.estimatedDuration, estimatedDuration) || other.estimatedDuration == estimatedDuration)&&(identical(other.isFeatured, isFeatured) || other.isFeatured == isFeatured)&&(identical(other.isCertified, isCertified) || other.isCertified == isCertified)&&(identical(other.price, price) || other.price == price)&&(identical(other.isFree, isFree) || other.isFree == isFree));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,title,description,thumbnail,instructorId,category,difficulty,const DeepCollectionEquality().hash(tags),const DeepCollectionEquality().hash(prerequisites),estimatedDuration,isFeatured,isCertified,price,isFree);

@override
String toString() {
  return 'CreateUpdateCourseDto(title: $title, description: $description, thumbnail: $thumbnail, instructorId: $instructorId, category: $category, difficulty: $difficulty, tags: $tags, prerequisites: $prerequisites, estimatedDuration: $estimatedDuration, isFeatured: $isFeatured, isCertified: $isCertified, price: $price, isFree: $isFree)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateCourseDtoCopyWith<$Res>  {
  factory $CreateUpdateCourseDtoCopyWith(CreateUpdateCourseDto value, $Res Function(CreateUpdateCourseDto) _then) = _$CreateUpdateCourseDtoCopyWithImpl;
@useResult
$Res call({
 String title, String description, String? thumbnail, String instructorId, CourseCategory category, CourseDifficulty difficulty, List<String> tags, List<String> prerequisites, int estimatedDuration, bool isFeatured, bool isCertified, double? price, bool isFree
});




}
/// @nodoc
class _$CreateUpdateCourseDtoCopyWithImpl<$Res>
    implements $CreateUpdateCourseDtoCopyWith<$Res> {
  _$CreateUpdateCourseDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateCourseDto _self;
  final $Res Function(CreateUpdateCourseDto) _then;

/// Create a copy of CreateUpdateCourseDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? title = null,Object? description = null,Object? thumbnail = freezed,Object? instructorId = null,Object? category = null,Object? difficulty = null,Object? tags = null,Object? prerequisites = null,Object? estimatedDuration = null,Object? isFeatured = null,Object? isCertified = null,Object? price = freezed,Object? isFree = null,}) {
  return _then(_self.copyWith(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,thumbnail: freezed == thumbnail ? _self.thumbnail : thumbnail // ignore: cast_nullable_to_non_nullable
as String?,instructorId: null == instructorId ? _self.instructorId : instructorId // ignore: cast_nullable_to_non_nullable
as String,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as CourseCategory,difficulty: null == difficulty ? _self.difficulty : difficulty // ignore: cast_nullable_to_non_nullable
as CourseDifficulty,tags: null == tags ? _self.tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,prerequisites: null == prerequisites ? _self.prerequisites : prerequisites // ignore: cast_nullable_to_non_nullable
as List<String>,estimatedDuration: null == estimatedDuration ? _self.estimatedDuration : estimatedDuration // ignore: cast_nullable_to_non_nullable
as int,isFeatured: null == isFeatured ? _self.isFeatured : isFeatured // ignore: cast_nullable_to_non_nullable
as bool,isCertified: null == isCertified ? _self.isCertified : isCertified // ignore: cast_nullable_to_non_nullable
as bool,price: freezed == price ? _self.price : price // ignore: cast_nullable_to_non_nullable
as double?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateCourseDto].
extension CreateUpdateCourseDtoPatterns on CreateUpdateCourseDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateCourseDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateCourseDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateCourseDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateCourseDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateCourseDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateCourseDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String title,  String description,  String? thumbnail,  String instructorId,  CourseCategory category,  CourseDifficulty difficulty,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  bool isFeatured,  bool isCertified,  double? price,  bool isFree)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateCourseDto() when $default != null:
return $default(_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.category,_that.difficulty,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.isFeatured,_that.isCertified,_that.price,_that.isFree);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String title,  String description,  String? thumbnail,  String instructorId,  CourseCategory category,  CourseDifficulty difficulty,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  bool isFeatured,  bool isCertified,  double? price,  bool isFree)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateCourseDto():
return $default(_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.category,_that.difficulty,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.isFeatured,_that.isCertified,_that.price,_that.isFree);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String title,  String description,  String? thumbnail,  String instructorId,  CourseCategory category,  CourseDifficulty difficulty,  List<String> tags,  List<String> prerequisites,  int estimatedDuration,  bool isFeatured,  bool isCertified,  double? price,  bool isFree)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateCourseDto() when $default != null:
return $default(_that.title,_that.description,_that.thumbnail,_that.instructorId,_that.category,_that.difficulty,_that.tags,_that.prerequisites,_that.estimatedDuration,_that.isFeatured,_that.isCertified,_that.price,_that.isFree);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateCourseDto implements CreateUpdateCourseDto {
  const _CreateUpdateCourseDto({required this.title, required this.description, this.thumbnail, required this.instructorId, required this.category, required this.difficulty, final  List<String> tags = const [], final  List<String> prerequisites = const [], required this.estimatedDuration, this.isFeatured = false, this.isCertified = false, this.price, this.isFree = false}): _tags = tags,_prerequisites = prerequisites;
  factory _CreateUpdateCourseDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateCourseDtoFromJson(json);

@override final  String title;
@override final  String description;
@override final  String? thumbnail;
@override final  String instructorId;
@override final  CourseCategory category;
@override final  CourseDifficulty difficulty;
 final  List<String> _tags;
@override@JsonKey() List<String> get tags {
  if (_tags is EqualUnmodifiableListView) return _tags;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_tags);
}

 final  List<String> _prerequisites;
@override@JsonKey() List<String> get prerequisites {
  if (_prerequisites is EqualUnmodifiableListView) return _prerequisites;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_prerequisites);
}

@override final  int estimatedDuration;
@override@JsonKey() final  bool isFeatured;
@override@JsonKey() final  bool isCertified;
@override final  double? price;
@override@JsonKey() final  bool isFree;

/// Create a copy of CreateUpdateCourseDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateCourseDtoCopyWith<_CreateUpdateCourseDto> get copyWith => __$CreateUpdateCourseDtoCopyWithImpl<_CreateUpdateCourseDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateCourseDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateCourseDto&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.thumbnail, thumbnail) || other.thumbnail == thumbnail)&&(identical(other.instructorId, instructorId) || other.instructorId == instructorId)&&(identical(other.category, category) || other.category == category)&&(identical(other.difficulty, difficulty) || other.difficulty == difficulty)&&const DeepCollectionEquality().equals(other._tags, _tags)&&const DeepCollectionEquality().equals(other._prerequisites, _prerequisites)&&(identical(other.estimatedDuration, estimatedDuration) || other.estimatedDuration == estimatedDuration)&&(identical(other.isFeatured, isFeatured) || other.isFeatured == isFeatured)&&(identical(other.isCertified, isCertified) || other.isCertified == isCertified)&&(identical(other.price, price) || other.price == price)&&(identical(other.isFree, isFree) || other.isFree == isFree));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,title,description,thumbnail,instructorId,category,difficulty,const DeepCollectionEquality().hash(_tags),const DeepCollectionEquality().hash(_prerequisites),estimatedDuration,isFeatured,isCertified,price,isFree);

@override
String toString() {
  return 'CreateUpdateCourseDto(title: $title, description: $description, thumbnail: $thumbnail, instructorId: $instructorId, category: $category, difficulty: $difficulty, tags: $tags, prerequisites: $prerequisites, estimatedDuration: $estimatedDuration, isFeatured: $isFeatured, isCertified: $isCertified, price: $price, isFree: $isFree)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateCourseDtoCopyWith<$Res> implements $CreateUpdateCourseDtoCopyWith<$Res> {
  factory _$CreateUpdateCourseDtoCopyWith(_CreateUpdateCourseDto value, $Res Function(_CreateUpdateCourseDto) _then) = __$CreateUpdateCourseDtoCopyWithImpl;
@override @useResult
$Res call({
 String title, String description, String? thumbnail, String instructorId, CourseCategory category, CourseDifficulty difficulty, List<String> tags, List<String> prerequisites, int estimatedDuration, bool isFeatured, bool isCertified, double? price, bool isFree
});




}
/// @nodoc
class __$CreateUpdateCourseDtoCopyWithImpl<$Res>
    implements _$CreateUpdateCourseDtoCopyWith<$Res> {
  __$CreateUpdateCourseDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateCourseDto _self;
  final $Res Function(_CreateUpdateCourseDto) _then;

/// Create a copy of CreateUpdateCourseDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? title = null,Object? description = null,Object? thumbnail = freezed,Object? instructorId = null,Object? category = null,Object? difficulty = null,Object? tags = null,Object? prerequisites = null,Object? estimatedDuration = null,Object? isFeatured = null,Object? isCertified = null,Object? price = freezed,Object? isFree = null,}) {
  return _then(_CreateUpdateCourseDto(
title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,thumbnail: freezed == thumbnail ? _self.thumbnail : thumbnail // ignore: cast_nullable_to_non_nullable
as String?,instructorId: null == instructorId ? _self.instructorId : instructorId // ignore: cast_nullable_to_non_nullable
as String,category: null == category ? _self.category : category // ignore: cast_nullable_to_non_nullable
as CourseCategory,difficulty: null == difficulty ? _self.difficulty : difficulty // ignore: cast_nullable_to_non_nullable
as CourseDifficulty,tags: null == tags ? _self._tags : tags // ignore: cast_nullable_to_non_nullable
as List<String>,prerequisites: null == prerequisites ? _self._prerequisites : prerequisites // ignore: cast_nullable_to_non_nullable
as List<String>,estimatedDuration: null == estimatedDuration ? _self.estimatedDuration : estimatedDuration // ignore: cast_nullable_to_non_nullable
as int,isFeatured: null == isFeatured ? _self.isFeatured : isFeatured // ignore: cast_nullable_to_non_nullable
as bool,isCertified: null == isCertified ? _self.isCertified : isCertified // ignore: cast_nullable_to_non_nullable
as bool,price: freezed == price ? _self.price : price // ignore: cast_nullable_to_non_nullable
as double?,isFree: null == isFree ? _self.isFree : isFree // ignore: cast_nullable_to_non_nullable
as bool,
  ));
}


}

// dart format on
