// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'task_checklist_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$ChecklistItemDto {

 String get id; String get text; bool get isCompleted; int get order; String? get completedBy; DateTime? get completionTime;
/// Create a copy of ChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$ChecklistItemDtoCopyWith<ChecklistItemDto> get copyWith => _$ChecklistItemDtoCopyWithImpl<ChecklistItemDto>(this as ChecklistItemDto, _$identity);

  /// Serializes this ChecklistItemDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is ChecklistItemDto&&(identical(other.id, id) || other.id == id)&&(identical(other.text, text) || other.text == text)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order)&&(identical(other.completedBy, completedBy) || other.completedBy == completedBy)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,text,isCompleted,order,completedBy,completionTime);

@override
String toString() {
  return 'ChecklistItemDto(id: $id, text: $text, isCompleted: $isCompleted, order: $order, completedBy: $completedBy, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class $ChecklistItemDtoCopyWith<$Res>  {
  factory $ChecklistItemDtoCopyWith(ChecklistItemDto value, $Res Function(ChecklistItemDto) _then) = _$ChecklistItemDtoCopyWithImpl;
@useResult
$Res call({
 String id, String text, bool isCompleted, int order, String? completedBy, DateTime? completionTime
});




}
/// @nodoc
class _$ChecklistItemDtoCopyWithImpl<$Res>
    implements $ChecklistItemDtoCopyWith<$Res> {
  _$ChecklistItemDtoCopyWithImpl(this._self, this._then);

  final ChecklistItemDto _self;
  final $Res Function(ChecklistItemDto) _then;

/// Create a copy of ChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? text = null,Object? isCompleted = null,Object? order = null,Object? completedBy = freezed,Object? completionTime = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,text: null == text ? _self.text : text // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,completedBy: freezed == completedBy ? _self.completedBy : completedBy // ignore: cast_nullable_to_non_nullable
as String?,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}

}


/// Adds pattern-matching-related methods to [ChecklistItemDto].
extension ChecklistItemDtoPatterns on ChecklistItemDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _ChecklistItemDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _ChecklistItemDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _ChecklistItemDto value)  $default,){
final _that = this;
switch (_that) {
case _ChecklistItemDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _ChecklistItemDto value)?  $default,){
final _that = this;
switch (_that) {
case _ChecklistItemDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String text,  bool isCompleted,  int order,  String? completedBy,  DateTime? completionTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _ChecklistItemDto() when $default != null:
return $default(_that.id,_that.text,_that.isCompleted,_that.order,_that.completedBy,_that.completionTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String text,  bool isCompleted,  int order,  String? completedBy,  DateTime? completionTime)  $default,) {final _that = this;
switch (_that) {
case _ChecklistItemDto():
return $default(_that.id,_that.text,_that.isCompleted,_that.order,_that.completedBy,_that.completionTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String text,  bool isCompleted,  int order,  String? completedBy,  DateTime? completionTime)?  $default,) {final _that = this;
switch (_that) {
case _ChecklistItemDto() when $default != null:
return $default(_that.id,_that.text,_that.isCompleted,_that.order,_that.completedBy,_that.completionTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _ChecklistItemDto implements ChecklistItemDto {
  const _ChecklistItemDto({required this.id, required this.text, this.isCompleted = false, required this.order, this.completedBy, this.completionTime});
  factory _ChecklistItemDto.fromJson(Map<String, dynamic> json) => _$ChecklistItemDtoFromJson(json);

@override final  String id;
@override final  String text;
@override@JsonKey() final  bool isCompleted;
@override final  int order;
@override final  String? completedBy;
@override final  DateTime? completionTime;

/// Create a copy of ChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$ChecklistItemDtoCopyWith<_ChecklistItemDto> get copyWith => __$ChecklistItemDtoCopyWithImpl<_ChecklistItemDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$ChecklistItemDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _ChecklistItemDto&&(identical(other.id, id) || other.id == id)&&(identical(other.text, text) || other.text == text)&&(identical(other.isCompleted, isCompleted) || other.isCompleted == isCompleted)&&(identical(other.order, order) || other.order == order)&&(identical(other.completedBy, completedBy) || other.completedBy == completedBy)&&(identical(other.completionTime, completionTime) || other.completionTime == completionTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,text,isCompleted,order,completedBy,completionTime);

@override
String toString() {
  return 'ChecklistItemDto(id: $id, text: $text, isCompleted: $isCompleted, order: $order, completedBy: $completedBy, completionTime: $completionTime)';
}


}

/// @nodoc
abstract mixin class _$ChecklistItemDtoCopyWith<$Res> implements $ChecklistItemDtoCopyWith<$Res> {
  factory _$ChecklistItemDtoCopyWith(_ChecklistItemDto value, $Res Function(_ChecklistItemDto) _then) = __$ChecklistItemDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String text, bool isCompleted, int order, String? completedBy, DateTime? completionTime
});




}
/// @nodoc
class __$ChecklistItemDtoCopyWithImpl<$Res>
    implements _$ChecklistItemDtoCopyWith<$Res> {
  __$ChecklistItemDtoCopyWithImpl(this._self, this._then);

  final _ChecklistItemDto _self;
  final $Res Function(_ChecklistItemDto) _then;

/// Create a copy of ChecklistItemDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? text = null,Object? isCompleted = null,Object? order = null,Object? completedBy = freezed,Object? completionTime = freezed,}) {
  return _then(_ChecklistItemDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,text: null == text ? _self.text : text // ignore: cast_nullable_to_non_nullable
as String,isCompleted: null == isCompleted ? _self.isCompleted : isCompleted // ignore: cast_nullable_to_non_nullable
as bool,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,completedBy: freezed == completedBy ? _self.completedBy : completedBy // ignore: cast_nullable_to_non_nullable
as String?,completionTime: freezed == completionTime ? _self.completionTime : completionTime // ignore: cast_nullable_to_non_nullable
as DateTime?,
  ));
}


}


/// @nodoc
mixin _$TaskChecklistDto {

 String get id; String get taskId; String get title; List<ChecklistItemDto> get items; double get progress; DateTime get creationTime;
/// Create a copy of TaskChecklistDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$TaskChecklistDtoCopyWith<TaskChecklistDto> get copyWith => _$TaskChecklistDtoCopyWithImpl<TaskChecklistDto>(this as TaskChecklistDto, _$identity);

  /// Serializes this TaskChecklistDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is TaskChecklistDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.title, title) || other.title == title)&&const DeepCollectionEquality().equals(other.items, items)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,title,const DeepCollectionEquality().hash(items),progress,creationTime);

@override
String toString() {
  return 'TaskChecklistDto(id: $id, taskId: $taskId, title: $title, items: $items, progress: $progress, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $TaskChecklistDtoCopyWith<$Res>  {
  factory $TaskChecklistDtoCopyWith(TaskChecklistDto value, $Res Function(TaskChecklistDto) _then) = _$TaskChecklistDtoCopyWithImpl;
@useResult
$Res call({
 String id, String taskId, String title, List<ChecklistItemDto> items, double progress, DateTime creationTime
});




}
/// @nodoc
class _$TaskChecklistDtoCopyWithImpl<$Res>
    implements $TaskChecklistDtoCopyWith<$Res> {
  _$TaskChecklistDtoCopyWithImpl(this._self, this._then);

  final TaskChecklistDto _self;
  final $Res Function(TaskChecklistDto) _then;

/// Create a copy of TaskChecklistDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? taskId = null,Object? title = null,Object? items = null,Object? progress = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,items: null == items ? _self.items : items // ignore: cast_nullable_to_non_nullable
as List<ChecklistItemDto>,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [TaskChecklistDto].
extension TaskChecklistDtoPatterns on TaskChecklistDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _TaskChecklistDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _TaskChecklistDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _TaskChecklistDto value)  $default,){
final _that = this;
switch (_that) {
case _TaskChecklistDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _TaskChecklistDto value)?  $default,){
final _that = this;
switch (_that) {
case _TaskChecklistDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String taskId,  String title,  List<ChecklistItemDto> items,  double progress,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _TaskChecklistDto() when $default != null:
return $default(_that.id,_that.taskId,_that.title,_that.items,_that.progress,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String taskId,  String title,  List<ChecklistItemDto> items,  double progress,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _TaskChecklistDto():
return $default(_that.id,_that.taskId,_that.title,_that.items,_that.progress,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String taskId,  String title,  List<ChecklistItemDto> items,  double progress,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _TaskChecklistDto() when $default != null:
return $default(_that.id,_that.taskId,_that.title,_that.items,_that.progress,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _TaskChecklistDto implements TaskChecklistDto {
  const _TaskChecklistDto({required this.id, required this.taskId, required this.title, required final  List<ChecklistItemDto> items, required this.progress, required this.creationTime}): _items = items;
  factory _TaskChecklistDto.fromJson(Map<String, dynamic> json) => _$TaskChecklistDtoFromJson(json);

@override final  String id;
@override final  String taskId;
@override final  String title;
 final  List<ChecklistItemDto> _items;
@override List<ChecklistItemDto> get items {
  if (_items is EqualUnmodifiableListView) return _items;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_items);
}

@override final  double progress;
@override final  DateTime creationTime;

/// Create a copy of TaskChecklistDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$TaskChecklistDtoCopyWith<_TaskChecklistDto> get copyWith => __$TaskChecklistDtoCopyWithImpl<_TaskChecklistDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$TaskChecklistDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _TaskChecklistDto&&(identical(other.id, id) || other.id == id)&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.title, title) || other.title == title)&&const DeepCollectionEquality().equals(other._items, _items)&&(identical(other.progress, progress) || other.progress == progress)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,taskId,title,const DeepCollectionEquality().hash(_items),progress,creationTime);

@override
String toString() {
  return 'TaskChecklistDto(id: $id, taskId: $taskId, title: $title, items: $items, progress: $progress, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$TaskChecklistDtoCopyWith<$Res> implements $TaskChecklistDtoCopyWith<$Res> {
  factory _$TaskChecklistDtoCopyWith(_TaskChecklistDto value, $Res Function(_TaskChecklistDto) _then) = __$TaskChecklistDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String taskId, String title, List<ChecklistItemDto> items, double progress, DateTime creationTime
});




}
/// @nodoc
class __$TaskChecklistDtoCopyWithImpl<$Res>
    implements _$TaskChecklistDtoCopyWith<$Res> {
  __$TaskChecklistDtoCopyWithImpl(this._self, this._then);

  final _TaskChecklistDto _self;
  final $Res Function(_TaskChecklistDto) _then;

/// Create a copy of TaskChecklistDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? taskId = null,Object? title = null,Object? items = null,Object? progress = null,Object? creationTime = null,}) {
  return _then(_TaskChecklistDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,items: null == items ? _self._items : items // ignore: cast_nullable_to_non_nullable
as List<ChecklistItemDto>,progress: null == progress ? _self.progress : progress // ignore: cast_nullable_to_non_nullable
as double,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$CreateUpdateChecklistDto {

 String get taskId; String get title; List<ChecklistItemDto> get items;
/// Create a copy of CreateUpdateChecklistDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CreateUpdateChecklistDtoCopyWith<CreateUpdateChecklistDto> get copyWith => _$CreateUpdateChecklistDtoCopyWithImpl<CreateUpdateChecklistDto>(this as CreateUpdateChecklistDto, _$identity);

  /// Serializes this CreateUpdateChecklistDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CreateUpdateChecklistDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.title, title) || other.title == title)&&const DeepCollectionEquality().equals(other.items, items));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,title,const DeepCollectionEquality().hash(items));

@override
String toString() {
  return 'CreateUpdateChecklistDto(taskId: $taskId, title: $title, items: $items)';
}


}

/// @nodoc
abstract mixin class $CreateUpdateChecklistDtoCopyWith<$Res>  {
  factory $CreateUpdateChecklistDtoCopyWith(CreateUpdateChecklistDto value, $Res Function(CreateUpdateChecklistDto) _then) = _$CreateUpdateChecklistDtoCopyWithImpl;
@useResult
$Res call({
 String taskId, String title, List<ChecklistItemDto> items
});




}
/// @nodoc
class _$CreateUpdateChecklistDtoCopyWithImpl<$Res>
    implements $CreateUpdateChecklistDtoCopyWith<$Res> {
  _$CreateUpdateChecklistDtoCopyWithImpl(this._self, this._then);

  final CreateUpdateChecklistDto _self;
  final $Res Function(CreateUpdateChecklistDto) _then;

/// Create a copy of CreateUpdateChecklistDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? taskId = null,Object? title = null,Object? items = null,}) {
  return _then(_self.copyWith(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,items: null == items ? _self.items : items // ignore: cast_nullable_to_non_nullable
as List<ChecklistItemDto>,
  ));
}

}


/// Adds pattern-matching-related methods to [CreateUpdateChecklistDto].
extension CreateUpdateChecklistDtoPatterns on CreateUpdateChecklistDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CreateUpdateChecklistDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CreateUpdateChecklistDto value)  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CreateUpdateChecklistDto value)?  $default,){
final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String taskId,  String title,  List<ChecklistItemDto> items)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto() when $default != null:
return $default(_that.taskId,_that.title,_that.items);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String taskId,  String title,  List<ChecklistItemDto> items)  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto():
return $default(_that.taskId,_that.title,_that.items);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String taskId,  String title,  List<ChecklistItemDto> items)?  $default,) {final _that = this;
switch (_that) {
case _CreateUpdateChecklistDto() when $default != null:
return $default(_that.taskId,_that.title,_that.items);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CreateUpdateChecklistDto implements CreateUpdateChecklistDto {
  const _CreateUpdateChecklistDto({required this.taskId, required this.title, final  List<ChecklistItemDto> items = const []}): _items = items;
  factory _CreateUpdateChecklistDto.fromJson(Map<String, dynamic> json) => _$CreateUpdateChecklistDtoFromJson(json);

@override final  String taskId;
@override final  String title;
 final  List<ChecklistItemDto> _items;
@override@JsonKey() List<ChecklistItemDto> get items {
  if (_items is EqualUnmodifiableListView) return _items;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_items);
}


/// Create a copy of CreateUpdateChecklistDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CreateUpdateChecklistDtoCopyWith<_CreateUpdateChecklistDto> get copyWith => __$CreateUpdateChecklistDtoCopyWithImpl<_CreateUpdateChecklistDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CreateUpdateChecklistDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CreateUpdateChecklistDto&&(identical(other.taskId, taskId) || other.taskId == taskId)&&(identical(other.title, title) || other.title == title)&&const DeepCollectionEquality().equals(other._items, _items));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,taskId,title,const DeepCollectionEquality().hash(_items));

@override
String toString() {
  return 'CreateUpdateChecklistDto(taskId: $taskId, title: $title, items: $items)';
}


}

/// @nodoc
abstract mixin class _$CreateUpdateChecklistDtoCopyWith<$Res> implements $CreateUpdateChecklistDtoCopyWith<$Res> {
  factory _$CreateUpdateChecklistDtoCopyWith(_CreateUpdateChecklistDto value, $Res Function(_CreateUpdateChecklistDto) _then) = __$CreateUpdateChecklistDtoCopyWithImpl;
@override @useResult
$Res call({
 String taskId, String title, List<ChecklistItemDto> items
});




}
/// @nodoc
class __$CreateUpdateChecklistDtoCopyWithImpl<$Res>
    implements _$CreateUpdateChecklistDtoCopyWith<$Res> {
  __$CreateUpdateChecklistDtoCopyWithImpl(this._self, this._then);

  final _CreateUpdateChecklistDto _self;
  final $Res Function(_CreateUpdateChecklistDto) _then;

/// Create a copy of CreateUpdateChecklistDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? taskId = null,Object? title = null,Object? items = null,}) {
  return _then(_CreateUpdateChecklistDto(
taskId: null == taskId ? _self.taskId : taskId // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,items: null == items ? _self._items : items // ignore: cast_nullable_to_non_nullable
as List<ChecklistItemDto>,
  ));
}


}

// dart format on
