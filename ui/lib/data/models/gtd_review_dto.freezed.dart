// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'gtd_review_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ReviewChecklistItemDto {

 String get id; String get description; bool get isCompleted; int get order;
/// Create a copy of ReviewChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ReviewChecklistItemDtoCopyWith<ReviewChecklistItemDto> get copyWith => _$ReviewChecklistItemDtoCopyWithImpl<ReviewChecklistItemDto>(this as ReviewChecklistItemDto, _$identity);

  /// Serializes this ReviewChecklistItemDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ReviewChecklistItemDto&&(identical(other.id, id) || other.id == id)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,description,isCompleted,order);

@override
String toString() {
  return 'ReviewChecklistItemDto(id: $id, description: $description, isCompleted: $isCompleted, order: $order)';
}


}

/// @nodoc
abstract mixin class $ReviewChecklistItemDtoCopyWith<$Res>  {
  factory $ReviewChecklistItemDtoCopyWith(ReviewChecklistItemDto value, $Res Function(ReviewChecklistItemDto) _then) = _$ReviewChecklistItemDtoCopyWithImpl;
@useResult
$Res call({
 String id, String description, bool isCompleted, int order
});




}
/// @nodoc
class _$ReviewChecklistItemDtoCopyWithImpl<$Res>
    implements $ReviewChecklistItemDtoCopyWith<$Res> {
  _$ReviewChecklistItemDtoCopyWithImpl(this._self, this._then);

  final ReviewChecklistItemDto _self;
  final $Res Function(ReviewChecklistItemDto) _then;

/// Create a copy of ReviewChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? description = null,Object? isCompleted = null,Object? order = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [ReviewChecklistItemDto].
extension ReviewChecklistItemDtoPatterns on ReviewChecklistItemDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ReviewChecklistItemDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ReviewChecklistItemDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ReviewChecklistItemDto value)  $default,){
final _that = this;
switch (_that) {
case _ReviewChecklistItemDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ReviewChecklistItemDto value)?  $default,){
final _that = this;
switch (_that) {
case _ReviewChecklistItemDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String description,  bool isCompleted,  int order)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ReviewChecklistItemDto() when $default != null:
return $default(_that.id,_that.description,_that.isCompleted,_that.order);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String description,  bool isCompleted,  int order)  $default,) {final _that = this;
switch (_that) {
case _ReviewChecklistItemDto():
return $default(_that.id,_that.description,_that.isCompleted,_that.order);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String description,  bool isCompleted,  int order)?  $default,) {final _that = this;
switch (_that) {
case _ReviewChecklistItemDto() when $default != null:
return $default(_that.id,_that.description,_that.isCompleted,_that.order);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ReviewChecklistItemDto implements ReviewChecklistItemDto {
  const _ReviewChecklistItemDto({required this.id, required this.description, this.isCompleted = false, required this.order});
  factory _ReviewChecklistItemDto.fromJson(Map<String, dynamic> json) => _$ReviewChecklistItemDtoFromJson(json);

@override final  String id;
@override final  String description;
@override@JsonKey() final  bool isCompleted;
@override final  int order;

/// Create a copy of ReviewChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ReviewChecklistItemDtoCopyWith<_ReviewChecklistItemDto> get copyWith => __$ReviewChecklistItemDtoCopyWithImpl<_ReviewChecklistItemDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ReviewChecklistItemDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ReviewChecklistItemDto&&(identical(other.id, id) || other.id == id)&&(identical(other.description, description) || other.description == description)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,description,isCompleted,order);

@override
String toString() {
  return 'ReviewChecklistItemDto(id: $id, description: $description, isCompleted: $isCompleted, order: $order)';
}


}

/// @nodoc
abstract mixin class _$ReviewChecklistItemDtoCopyWith<$Res> implements $ReviewChecklistItemDtoCopyWith<$Res> {
  factory _$ReviewChecklistItemDtoCopyWith(_ReviewChecklistItemDto value, $Res Function(_ReviewChecklistItemDto) _then) = __$ReviewChecklistItemDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String description, bool isCompleted, int order
});




}
/// @nodoc
class __$ReviewChecklistItemDtoCopyWithImpl<$Res>
    implements _$ReviewChecklistItemDtoCopyWith<$Res> {
  __$ReviewChecklistItemDtoCopyWithImpl(this._self, this._then);

  final _ReviewChecklistItemDto _self;
  final $Res Function(_ReviewChecklistItemDto) _then;

/// Create a copy of ReviewChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? description = null,Object? isCompleted = null,Object? order = null,}) {
  return _then(_ReviewChecklistItemDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}


/// @nodoc
mixin _$GtdReviewDto {

 String get id; String get userId; ReviewType get reviewType; DateTime get reviewDate; DateTime? get completedDate; bool get isCompleted; List<ReviewChecklistItemDto> get checklist; String? get notes; Map<String, dynamic> get metrics; DateTime get creationTime;
/// Create a copy of GtdReviewDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$GtdReviewDtoCopyWith<GtdReviewDto> get copyWith => _$GtdReviewDtoCopyWithImpl<GtdReviewDto>(this as GtdReviewDto, _$identity);

  /// Serializes this GtdReviewDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is GtdReviewDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.reviewType, reviewType) || other.reviewType == reviewType)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&const DeepCollectionEquality().equals(other.checklist, checklist)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other.metrics, metrics)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,reviewType,reviewDate,completedDate,isCompleted,const DeepCollectionEquality().hash(checklist),notes,const DeepCollectionEquality().hash(metrics),creationTime);

@override
String toString() {
  return 'GtdReviewDto(id: $id, userId: $userId, reviewType: $reviewType, reviewDate: $reviewDate, completedDate: $completedDate, isCompleted: $isCompleted, checklist: $checklist, notes: $notes, metrics: $metrics, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $GtdReviewDtoCopyWith<$Res>  {
  factory $GtdReviewDtoCopyWith(GtdReviewDto value, $Res Function(GtdReviewDto) _then) = _$GtdReviewDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, ReviewType reviewType, DateTime reviewDate, DateTime? completedDate, bool isCompleted, List<ReviewChecklistItemDto> checklist, String? notes, Map<String, dynamic> metrics, DateTime creationTime
});




}
/// @nodoc
class _$GtdReviewDtoCopyWithImpl<$Res>
    implements $GtdReviewDtoCopyWith<$Res> {
  _$GtdReviewDtoCopyWithImpl(this._self, this._then);

  final GtdReviewDto _self;
  final $Res Function(GtdReviewDto) _then;

/// Create a copy of GtdReviewDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? reviewType = null,Object? reviewDate = null,Object? completedDate = freezed,Object? isCompleted = null,Object? checklist = null,Object? notes = freezed,Object? metrics = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,reviewType: null == reviewType ? _self.reviewType : reviewType // ignore: cast_nullable_to_non_nullable
as ReviewType,reviewDate: null == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,checklist: null == checklist ? _self.checklist : checklist // ignore: cast_nullable_to_non_nullable
as List<ReviewChecklistItemDto>,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,metrics: null == metrics ? _self.metrics : metrics // ignore: cast_nullable_to_non_nullable
as Map<String, dynamic>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [GtdReviewDto].
extension GtdReviewDtoPatterns on GtdReviewDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _GtdReviewDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _GtdReviewDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _GtdReviewDto value)  $default,){
final _that = this;
switch (_that) {
case _GtdReviewDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _GtdReviewDto value)?  $default,){
final _that = this;
switch (_that) {
case _GtdReviewDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  ReviewType reviewType,  DateTime reviewDate,  DateTime? completedDate,  bool isCompleted,  List<ReviewChecklistItemDto> checklist,  String? notes,  Map<String, dynamic> metrics,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _GtdReviewDto() when $default != null:
return $default(_that.id,_that.userId,_that.reviewType,_that.reviewDate,_that.completedDate,_that.isCompleted,_that.checklist,_that.notes,_that.metrics,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  ReviewType reviewType,  DateTime reviewDate,  DateTime? completedDate,  bool isCompleted,  List<ReviewChecklistItemDto> checklist,  String? notes,  Map<String, dynamic> metrics,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _GtdReviewDto():
return $default(_that.id,_that.userId,_that.reviewType,_that.reviewDate,_that.completedDate,_that.isCompleted,_that.checklist,_that.notes,_that.metrics,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  ReviewType reviewType,  DateTime reviewDate,  DateTime? completedDate,  bool isCompleted,  List<ReviewChecklistItemDto> checklist,  String? notes,  Map<String, dynamic> metrics,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _GtdReviewDto() when $default != null:
return $default(_that.id,_that.userId,_that.reviewType,_that.reviewDate,_that.completedDate,_that.isCompleted,_that.checklist,_that.notes,_that.metrics,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _GtdReviewDto implements GtdReviewDto {
  const _GtdReviewDto({required this.id, required this.userId, required this.reviewType, required this.reviewDate, this.completedDate, this.isCompleted = false, required final  List<ReviewChecklistItemDto> checklist, this.notes, final  Map<String, dynamic> metrics = const {}, required this.creationTime}): _checklist = checklist,_metrics = metrics;
  factory _GtdReviewDto.fromJson(Map<String, dynamic> json) => _$GtdReviewDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  ReviewType reviewType;
@override final  DateTime reviewDate;
@override final  DateTime? completedDate;
@override@JsonKey() final  bool isCompleted;
 final  List<ReviewChecklistItemDto> _checklist;
@override List<ReviewChecklistItemDto> get checklist {
  if (_checklist is EqualUnmodifiableListView) return _checklist;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_checklist);
}

@override final  String? notes;
 final  Map<String, dynamic> _metrics;
@override@JsonKey() Map<String, dynamic> get metrics {
  if (_metrics is EqualUnmodifiableMapView) return _metrics;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableMapView(_metrics);
}

@override final  DateTime creationTime;

/// Create a copy of GtdReviewDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$GtdReviewDtoCopyWith<_GtdReviewDto> get copyWith => __$GtdReviewDtoCopyWithImpl<_GtdReviewDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$GtdReviewDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _GtdReviewDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.reviewType, reviewType) || other.reviewType == reviewType)&&(identical(other.reviewDate, reviewDate) || other.reviewDate == reviewDate)&&(identical(other.completedDate, completedDate) || other.completedDate == completedDate)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&const DeepCollectionEquality().equals(other._checklist, _checklist)&&(identical(other.notes, notes) || other.notes == notes)&&const DeepCollectionEquality().equals(other._metrics, _metrics)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,reviewType,reviewDate,completedDate,isCompleted,const DeepCollectionEquality().hash(_checklist),notes,const DeepCollectionEquality().hash(_metrics),creationTime);

@override
String toString() {
  return 'GtdReviewDto(id: $id, userId: $userId, reviewType: $reviewType, reviewDate: $reviewDate, completedDate: $completedDate, isCompleted: $isCompleted, checklist: $checklist, notes: $notes, metrics: $metrics, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$GtdReviewDtoCopyWith<$Res> implements $GtdReviewDtoCopyWith<$Res> {
  factory _$GtdReviewDtoCopyWith(_GtdReviewDto value, $Res Function(_GtdReviewDto) _then) = __$GtdReviewDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, ReviewType reviewType, DateTime reviewDate, DateTime? completedDate, bool isCompleted, List<ReviewChecklistItemDto> checklist, String? notes, Map<String, dynamic> metrics, DateTime creationTime
});




}
/// @nodoc
class __$GtdReviewDtoCopyWithImpl<$Res>
    implements _$GtdReviewDtoCopyWith<$Res> {
  __$GtdReviewDtoCopyWithImpl(this._self, this._then);

  final _GtdReviewDto _self;
  final $Res Function(_GtdReviewDto) _then;

/// Create a copy of GtdReviewDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? reviewType = null,Object? reviewDate = null,Object? completedDate = freezed,Object? isCompleted = null,Object? checklist = null,Object? notes = freezed,Object? metrics = null,Object? creationTime = null,}) {
  return _then(_GtdReviewDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,reviewType: null == reviewType ? _self.reviewType : reviewType // ignore: cast_nullable_to_non_nullable
as ReviewType,reviewDate: null == reviewDate ? _self.reviewDate : reviewDate // ignore: cast_nullable_to_non_nullable
as DateTime,completedDate: freezed == completedDate ? _self.completedDate : completedDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,checklist: null == checklist ? _self._checklist : checklist // ignore: cast_nullable_to_non_nullable
as List<ReviewChecklistItemDto>,notes: freezed == notes ? _self.notes : notes // ignore: cast_nullable_to_non_nullable
as String?,metrics: null == metrics ? _self._metrics : metrics // ignore: cast_nullable_to_non_nullable
as Map<String, dynamic>,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}

// dart format on
