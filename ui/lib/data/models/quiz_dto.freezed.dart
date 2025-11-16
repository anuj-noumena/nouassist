// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'quiz_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$QuizQuestionDto {

 String get id; String get question; QuestionType get questionType; List<String> get options; String get correctAnswer; String? get explanation; int get points; int get order;
/// Create a copy of QuizQuestionDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$QuizQuestionDtoCopyWith<QuizQuestionDto> get copyWith => _$QuizQuestionDtoCopyWithImpl<QuizQuestionDto>(this as QuizQuestionDto, _$identity);

  /// Serializes this QuizQuestionDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is QuizQuestionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.question, question) || other.question == question)&&(identical(other.questionType, questionType) || other.questionType == questionType)&&const DeepCollectionEquality().equals(other.options, options)&&(identical(other.correctAnswer, correctAnswer) || other.correctAnswer == correctAnswer)&&(identical(other.explanation, explanation) || other.explanation == explanation)&&(identical(other.points, points) || other.points == points)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,question,questionType,const DeepCollectionEquality().hash(options),correctAnswer,explanation,points,order);

@override
String toString() {
  return 'QuizQuestionDto(id: $id, question: $question, questionType: $questionType, options: $options, correctAnswer: $correctAnswer, explanation: $explanation, points: $points, order: $order)';
}


}

/// @nodoc
abstract mixin class $QuizQuestionDtoCopyWith<$Res>  {
  factory $QuizQuestionDtoCopyWith(QuizQuestionDto value, $Res Function(QuizQuestionDto) _then) = _$QuizQuestionDtoCopyWithImpl;
@useResult
$Res call({
 String id, String question, QuestionType questionType, List<String> options, String correctAnswer, String? explanation, int points, int order
});




}
/// @nodoc
class _$QuizQuestionDtoCopyWithImpl<$Res>
    implements $QuizQuestionDtoCopyWith<$Res> {
  _$QuizQuestionDtoCopyWithImpl(this._self, this._then);

  final QuizQuestionDto _self;
  final $Res Function(QuizQuestionDto) _then;

/// Create a copy of QuizQuestionDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? question = null,Object? questionType = null,Object? options = null,Object? correctAnswer = null,Object? explanation = freezed,Object? points = null,Object? order = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,question: null == question ? _self.question : question // ignore: cast_nullable_to_non_nullable
as String,questionType: null == questionType ? _self.questionType : questionType // ignore: cast_nullable_to_non_nullable
as QuestionType,options: null == options ? _self.options : options // ignore: cast_nullable_to_non_nullable
as List<String>,correctAnswer: null == correctAnswer ? _self.correctAnswer : correctAnswer // ignore: cast_nullable_to_non_nullable
as String,explanation: freezed == explanation ? _self.explanation : explanation // ignore: cast_nullable_to_non_nullable
as String?,points: null == points ? _self.points : points // ignore: cast_nullable_to_non_nullable
as int,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [QuizQuestionDto].
extension QuizQuestionDtoPatterns on QuizQuestionDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _QuizQuestionDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _QuizQuestionDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _QuizQuestionDto value)  $default,){
final _that = this;
switch (_that) {
case _QuizQuestionDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _QuizQuestionDto value)?  $default,){
final _that = this;
switch (_that) {
case _QuizQuestionDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String question,  QuestionType questionType,  List<String> options,  String correctAnswer,  String? explanation,  int points,  int order)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _QuizQuestionDto() when $default != null:
return $default(_that.id,_that.question,_that.questionType,_that.options,_that.correctAnswer,_that.explanation,_that.points,_that.order);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String question,  QuestionType questionType,  List<String> options,  String correctAnswer,  String? explanation,  int points,  int order)  $default,) {final _that = this;
switch (_that) {
case _QuizQuestionDto():
return $default(_that.id,_that.question,_that.questionType,_that.options,_that.correctAnswer,_that.explanation,_that.points,_that.order);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String question,  QuestionType questionType,  List<String> options,  String correctAnswer,  String? explanation,  int points,  int order)?  $default,) {final _that = this;
switch (_that) {
case _QuizQuestionDto() when $default != null:
return $default(_that.id,_that.question,_that.questionType,_that.options,_that.correctAnswer,_that.explanation,_that.points,_that.order);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _QuizQuestionDto implements QuizQuestionDto {
  const _QuizQuestionDto({required this.id, required this.question, required this.questionType, required final  List<String> options, required this.correctAnswer, this.explanation, required this.points, required this.order}): _options = options;
  factory _QuizQuestionDto.fromJson(Map<String, dynamic> json) => _$QuizQuestionDtoFromJson(json);

@override final  String id;
@override final  String question;
@override final  QuestionType questionType;
 final  List<String> _options;
@override List<String> get options {
  if (_options is EqualUnmodifiableListView) return _options;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_options);
}

@override final  String correctAnswer;
@override final  String? explanation;
@override final  int points;
@override final  int order;

/// Create a copy of QuizQuestionDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$QuizQuestionDtoCopyWith<_QuizQuestionDto> get copyWith => __$QuizQuestionDtoCopyWithImpl<_QuizQuestionDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$QuizQuestionDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _QuizQuestionDto&&(identical(other.id, id) || other.id == id)&&(identical(other.question, question) || other.question == question)&&(identical(other.questionType, questionType) || other.questionType == questionType)&&const DeepCollectionEquality().equals(other._options, _options)&&(identical(other.correctAnswer, correctAnswer) || other.correctAnswer == correctAnswer)&&(identical(other.explanation, explanation) || other.explanation == explanation)&&(identical(other.points, points) || other.points == points)&&(identical(other.order, order) || other.order == order));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,question,questionType,const DeepCollectionEquality().hash(_options),correctAnswer,explanation,points,order);

@override
String toString() {
  return 'QuizQuestionDto(id: $id, question: $question, questionType: $questionType, options: $options, correctAnswer: $correctAnswer, explanation: $explanation, points: $points, order: $order)';
}


}

/// @nodoc
abstract mixin class _$QuizQuestionDtoCopyWith<$Res> implements $QuizQuestionDtoCopyWith<$Res> {
  factory _$QuizQuestionDtoCopyWith(_QuizQuestionDto value, $Res Function(_QuizQuestionDto) _then) = __$QuizQuestionDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String question, QuestionType questionType, List<String> options, String correctAnswer, String? explanation, int points, int order
});




}
/// @nodoc
class __$QuizQuestionDtoCopyWithImpl<$Res>
    implements _$QuizQuestionDtoCopyWith<$Res> {
  __$QuizQuestionDtoCopyWithImpl(this._self, this._then);

  final _QuizQuestionDto _self;
  final $Res Function(_QuizQuestionDto) _then;

/// Create a copy of QuizQuestionDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? question = null,Object? questionType = null,Object? options = null,Object? correctAnswer = null,Object? explanation = freezed,Object? points = null,Object? order = null,}) {
  return _then(_QuizQuestionDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,question: null == question ? _self.question : question // ignore: cast_nullable_to_non_nullable
as String,questionType: null == questionType ? _self.questionType : questionType // ignore: cast_nullable_to_non_nullable
as QuestionType,options: null == options ? _self._options : options // ignore: cast_nullable_to_non_nullable
as List<String>,correctAnswer: null == correctAnswer ? _self.correctAnswer : correctAnswer // ignore: cast_nullable_to_non_nullable
as String,explanation: freezed == explanation ? _self.explanation : explanation // ignore: cast_nullable_to_non_nullable
as String?,points: null == points ? _self.points : points // ignore: cast_nullable_to_non_nullable
as int,order: null == order ? _self.order : order // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}


/// @nodoc
mixin _$QuizDto {

 String get id; String get title; String get description; String get courseId; String? get lessonId; List<QuizQuestionDto> get questions; int get passingScore; int get timeLimit; int get maxAttempts; bool get shuffleQuestions; bool get showCorrectAnswers; DateTime get creationTime;
/// Create a copy of QuizDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$QuizDtoCopyWith<QuizDto> get copyWith => _$QuizDtoCopyWithImpl<QuizDto>(this as QuizDto, _$identity);

  /// Serializes this QuizDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is QuizDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.lessonId, lessonId) || other.lessonId == lessonId)&&const DeepCollectionEquality().equals(other.questions, questions)&&(identical(other.passingScore, passingScore) || other.passingScore == passingScore)&&(identical(other.timeLimit, timeLimit) || other.timeLimit == timeLimit)&&(identical(other.maxAttempts, maxAttempts) || other.maxAttempts == maxAttempts)&&(identical(other.shuffleQuestions, shuffleQuestions) || other.shuffleQuestions == shuffleQuestions)&&(identical(other.showCorrectAnswers, showCorrectAnswers) || other.showCorrectAnswers == showCorrectAnswers)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,title,description,courseId,lessonId,const DeepCollectionEquality().hash(questions),passingScore,timeLimit,maxAttempts,shuffleQuestions,showCorrectAnswers,creationTime);

@override
String toString() {
  return 'QuizDto(id: $id, title: $title, description: $description, courseId: $courseId, lessonId: $lessonId, questions: $questions, passingScore: $passingScore, timeLimit: $timeLimit, maxAttempts: $maxAttempts, shuffleQuestions: $shuffleQuestions, showCorrectAnswers: $showCorrectAnswers, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class $QuizDtoCopyWith<$Res>  {
  factory $QuizDtoCopyWith(QuizDto value, $Res Function(QuizDto) _then) = _$QuizDtoCopyWithImpl;
@useResult
$Res call({
 String id, String title, String description, String courseId, String? lessonId, List<QuizQuestionDto> questions, int passingScore, int timeLimit, int maxAttempts, bool shuffleQuestions, bool showCorrectAnswers, DateTime creationTime
});




}
/// @nodoc
class _$QuizDtoCopyWithImpl<$Res>
    implements $QuizDtoCopyWith<$Res> {
  _$QuizDtoCopyWithImpl(this._self, this._then);

  final QuizDto _self;
  final $Res Function(QuizDto) _then;

/// Create a copy of QuizDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? title = null,Object? description = null,Object? courseId = null,Object? lessonId = freezed,Object? questions = null,Object? passingScore = null,Object? timeLimit = null,Object? maxAttempts = null,Object? shuffleQuestions = null,Object? showCorrectAnswers = null,Object? creationTime = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,lessonId: freezed == lessonId ? _self.lessonId : lessonId // ignore: cast_nullable_to_non_nullable
as String?,questions: null == questions ? _self.questions : questions // ignore: cast_nullable_to_non_nullable
as List<QuizQuestionDto>,passingScore: null == passingScore ? _self.passingScore : passingScore // ignore: cast_nullable_to_non_nullable
as int,timeLimit: null == timeLimit ? _self.timeLimit : timeLimit // ignore: cast_nullable_to_non_nullable
as int,maxAttempts: null == maxAttempts ? _self.maxAttempts : maxAttempts // ignore: cast_nullable_to_non_nullable
as int,shuffleQuestions: null == shuffleQuestions ? _self.shuffleQuestions : shuffleQuestions // ignore: cast_nullable_to_non_nullable
as bool,showCorrectAnswers: null == showCorrectAnswers ? _self.showCorrectAnswers : showCorrectAnswers // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}

}


/// Adds pattern-matching-related methods to [QuizDto].
extension QuizDtoPatterns on QuizDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _QuizDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _QuizDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _QuizDto value)  $default,){
final _that = this;
switch (_that) {
case _QuizDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _QuizDto value)?  $default,){
final _that = this;
switch (_that) {
case _QuizDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String title,  String description,  String courseId,  String? lessonId,  List<QuizQuestionDto> questions,  int passingScore,  int timeLimit,  int maxAttempts,  bool shuffleQuestions,  bool showCorrectAnswers,  DateTime creationTime)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _QuizDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.courseId,_that.lessonId,_that.questions,_that.passingScore,_that.timeLimit,_that.maxAttempts,_that.shuffleQuestions,_that.showCorrectAnswers,_that.creationTime);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String title,  String description,  String courseId,  String? lessonId,  List<QuizQuestionDto> questions,  int passingScore,  int timeLimit,  int maxAttempts,  bool shuffleQuestions,  bool showCorrectAnswers,  DateTime creationTime)  $default,) {final _that = this;
switch (_that) {
case _QuizDto():
return $default(_that.id,_that.title,_that.description,_that.courseId,_that.lessonId,_that.questions,_that.passingScore,_that.timeLimit,_that.maxAttempts,_that.shuffleQuestions,_that.showCorrectAnswers,_that.creationTime);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String title,  String description,  String courseId,  String? lessonId,  List<QuizQuestionDto> questions,  int passingScore,  int timeLimit,  int maxAttempts,  bool shuffleQuestions,  bool showCorrectAnswers,  DateTime creationTime)?  $default,) {final _that = this;
switch (_that) {
case _QuizDto() when $default != null:
return $default(_that.id,_that.title,_that.description,_that.courseId,_that.lessonId,_that.questions,_that.passingScore,_that.timeLimit,_that.maxAttempts,_that.shuffleQuestions,_that.showCorrectAnswers,_that.creationTime);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _QuizDto implements QuizDto {
  const _QuizDto({required this.id, required this.title, required this.description, required this.courseId, this.lessonId, required final  List<QuizQuestionDto> questions, required this.passingScore, required this.timeLimit, this.maxAttempts = 1, this.shuffleQuestions = false, this.showCorrectAnswers = false, required this.creationTime}): _questions = questions;
  factory _QuizDto.fromJson(Map<String, dynamic> json) => _$QuizDtoFromJson(json);

@override final  String id;
@override final  String title;
@override final  String description;
@override final  String courseId;
@override final  String? lessonId;
 final  List<QuizQuestionDto> _questions;
@override List<QuizQuestionDto> get questions {
  if (_questions is EqualUnmodifiableListView) return _questions;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableListView(_questions);
}

@override final  int passingScore;
@override final  int timeLimit;
@override@JsonKey() final  int maxAttempts;
@override@JsonKey() final  bool shuffleQuestions;
@override@JsonKey() final  bool showCorrectAnswers;
@override final  DateTime creationTime;

/// Create a copy of QuizDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$QuizDtoCopyWith<_QuizDto> get copyWith => __$QuizDtoCopyWithImpl<_QuizDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$QuizDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _QuizDto&&(identical(other.id, id) || other.id == id)&&(identical(other.title, title) || other.title == title)&&(identical(other.description, description) || other.description == description)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.lessonId, lessonId) || other.lessonId == lessonId)&&const DeepCollectionEquality().equals(other._questions, _questions)&&(identical(other.passingScore, passingScore) || other.passingScore == passingScore)&&(identical(other.timeLimit, timeLimit) || other.timeLimit == timeLimit)&&(identical(other.maxAttempts, maxAttempts) || other.maxAttempts == maxAttempts)&&(identical(other.shuffleQuestions, shuffleQuestions) || other.shuffleQuestions == shuffleQuestions)&&(identical(other.showCorrectAnswers, showCorrectAnswers) || other.showCorrectAnswers == showCorrectAnswers)&&(identical(other.creationTime, creationTime) || other.creationTime == creationTime));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,title,description,courseId,lessonId,const DeepCollectionEquality().hash(_questions),passingScore,timeLimit,maxAttempts,shuffleQuestions,showCorrectAnswers,creationTime);

@override
String toString() {
  return 'QuizDto(id: $id, title: $title, description: $description, courseId: $courseId, lessonId: $lessonId, questions: $questions, passingScore: $passingScore, timeLimit: $timeLimit, maxAttempts: $maxAttempts, shuffleQuestions: $shuffleQuestions, showCorrectAnswers: $showCorrectAnswers, creationTime: $creationTime)';
}


}

/// @nodoc
abstract mixin class _$QuizDtoCopyWith<$Res> implements $QuizDtoCopyWith<$Res> {
  factory _$QuizDtoCopyWith(_QuizDto value, $Res Function(_QuizDto) _then) = __$QuizDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String title, String description, String courseId, String? lessonId, List<QuizQuestionDto> questions, int passingScore, int timeLimit, int maxAttempts, bool shuffleQuestions, bool showCorrectAnswers, DateTime creationTime
});




}
/// @nodoc
class __$QuizDtoCopyWithImpl<$Res>
    implements _$QuizDtoCopyWith<$Res> {
  __$QuizDtoCopyWithImpl(this._self, this._then);

  final _QuizDto _self;
  final $Res Function(_QuizDto) _then;

/// Create a copy of QuizDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? title = null,Object? description = null,Object? courseId = null,Object? lessonId = freezed,Object? questions = null,Object? passingScore = null,Object? timeLimit = null,Object? maxAttempts = null,Object? shuffleQuestions = null,Object? showCorrectAnswers = null,Object? creationTime = null,}) {
  return _then(_QuizDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,title: null == title ? _self.title : title // ignore: cast_nullable_to_non_nullable
as String,description: null == description ? _self.description : description // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,lessonId: freezed == lessonId ? _self.lessonId : lessonId // ignore: cast_nullable_to_non_nullable
as String?,questions: null == questions ? _self._questions : questions // ignore: cast_nullable_to_non_nullable
as List<QuizQuestionDto>,passingScore: null == passingScore ? _self.passingScore : passingScore // ignore: cast_nullable_to_non_nullable
as int,timeLimit: null == timeLimit ? _self.timeLimit : timeLimit // ignore: cast_nullable_to_non_nullable
as int,maxAttempts: null == maxAttempts ? _self.maxAttempts : maxAttempts // ignore: cast_nullable_to_non_nullable
as int,shuffleQuestions: null == shuffleQuestions ? _self.shuffleQuestions : shuffleQuestions // ignore: cast_nullable_to_non_nullable
as bool,showCorrectAnswers: null == showCorrectAnswers ? _self.showCorrectAnswers : showCorrectAnswers // ignore: cast_nullable_to_non_nullable
as bool,creationTime: null == creationTime ? _self.creationTime : creationTime // ignore: cast_nullable_to_non_nullable
as DateTime,
  ));
}


}


/// @nodoc
mixin _$QuizAttemptDto {

 String get id; String get userId; String get quizId; String get courseId; Map<String, String> get answers; int get score; int get totalPoints; double get percentage; bool get passed; int get attemptNumber; DateTime get startTime; DateTime? get endTime; int get timeSpent;
/// Create a copy of QuizAttemptDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$QuizAttemptDtoCopyWith<QuizAttemptDto> get copyWith => _$QuizAttemptDtoCopyWithImpl<QuizAttemptDto>(this as QuizAttemptDto, _$identity);

  /// Serializes this QuizAttemptDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is QuizAttemptDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&const DeepCollectionEquality().equals(other.answers, answers)&&(identical(other.score, score) || other.score == score)&&(identical(other.totalPoints, totalPoints) || other.totalPoints == totalPoints)&&(identical(other.percentage, percentage) || other.percentage == percentage)&&(identical(other.passed, passed) || other.passed == passed)&&(identical(other.attemptNumber, attemptNumber) || other.attemptNumber == attemptNumber)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,quizId,courseId,const DeepCollectionEquality().hash(answers),score,totalPoints,percentage,passed,attemptNumber,startTime,endTime,timeSpent);

@override
String toString() {
  return 'QuizAttemptDto(id: $id, userId: $userId, quizId: $quizId, courseId: $courseId, answers: $answers, score: $score, totalPoints: $totalPoints, percentage: $percentage, passed: $passed, attemptNumber: $attemptNumber, startTime: $startTime, endTime: $endTime, timeSpent: $timeSpent)';
}


}

/// @nodoc
abstract mixin class $QuizAttemptDtoCopyWith<$Res>  {
  factory $QuizAttemptDtoCopyWith(QuizAttemptDto value, $Res Function(QuizAttemptDto) _then) = _$QuizAttemptDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String quizId, String courseId, Map<String, String> answers, int score, int totalPoints, double percentage, bool passed, int attemptNumber, DateTime startTime, DateTime? endTime, int timeSpent
});




}
/// @nodoc
class _$QuizAttemptDtoCopyWithImpl<$Res>
    implements $QuizAttemptDtoCopyWith<$Res> {
  _$QuizAttemptDtoCopyWithImpl(this._self, this._then);

  final QuizAttemptDto _self;
  final $Res Function(QuizAttemptDto) _then;

/// Create a copy of QuizAttemptDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? quizId = null,Object? courseId = null,Object? answers = null,Object? score = null,Object? totalPoints = null,Object? percentage = null,Object? passed = null,Object? attemptNumber = null,Object? startTime = null,Object? endTime = freezed,Object? timeSpent = null,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,quizId: null == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,answers: null == answers ? _self.answers : answers // ignore: cast_nullable_to_non_nullable
as Map<String, String>,score: null == score ? _self.score : score // ignore: cast_nullable_to_non_nullable
as int,totalPoints: null == totalPoints ? _self.totalPoints : totalPoints // ignore: cast_nullable_to_non_nullable
as int,percentage: null == percentage ? _self.percentage : percentage // ignore: cast_nullable_to_non_nullable
as double,passed: null == passed ? _self.passed : passed // ignore: cast_nullable_to_non_nullable
as bool,attemptNumber: null == attemptNumber ? _self.attemptNumber : attemptNumber // ignore: cast_nullable_to_non_nullable
as int,startTime: null == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as DateTime,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as DateTime?,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,
  ));
}

}


/// Adds pattern-matching-related methods to [QuizAttemptDto].
extension QuizAttemptDtoPatterns on QuizAttemptDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _QuizAttemptDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _QuizAttemptDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _QuizAttemptDto value)  $default,){
final _that = this;
switch (_that) {
case _QuizAttemptDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _QuizAttemptDto value)?  $default,){
final _that = this;
switch (_that) {
case _QuizAttemptDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String quizId,  String courseId,  Map<String, String> answers,  int score,  int totalPoints,  double percentage,  bool passed,  int attemptNumber,  DateTime startTime,  DateTime? endTime,  int timeSpent)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _QuizAttemptDto() when $default != null:
return $default(_that.id,_that.userId,_that.quizId,_that.courseId,_that.answers,_that.score,_that.totalPoints,_that.percentage,_that.passed,_that.attemptNumber,_that.startTime,_that.endTime,_that.timeSpent);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String quizId,  String courseId,  Map<String, String> answers,  int score,  int totalPoints,  double percentage,  bool passed,  int attemptNumber,  DateTime startTime,  DateTime? endTime,  int timeSpent)  $default,) {final _that = this;
switch (_that) {
case _QuizAttemptDto():
return $default(_that.id,_that.userId,_that.quizId,_that.courseId,_that.answers,_that.score,_that.totalPoints,_that.percentage,_that.passed,_that.attemptNumber,_that.startTime,_that.endTime,_that.timeSpent);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String quizId,  String courseId,  Map<String, String> answers,  int score,  int totalPoints,  double percentage,  bool passed,  int attemptNumber,  DateTime startTime,  DateTime? endTime,  int timeSpent)?  $default,) {final _that = this;
switch (_that) {
case _QuizAttemptDto() when $default != null:
return $default(_that.id,_that.userId,_that.quizId,_that.courseId,_that.answers,_that.score,_that.totalPoints,_that.percentage,_that.passed,_that.attemptNumber,_that.startTime,_that.endTime,_that.timeSpent);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _QuizAttemptDto implements QuizAttemptDto {
  const _QuizAttemptDto({required this.id, required this.userId, required this.quizId, required this.courseId, required final  Map<String, String> answers, required this.score, required this.totalPoints, required this.percentage, this.passed = false, required this.attemptNumber, required this.startTime, this.endTime, required this.timeSpent}): _answers = answers;
  factory _QuizAttemptDto.fromJson(Map<String, dynamic> json) => _$QuizAttemptDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String quizId;
@override final  String courseId;
 final  Map<String, String> _answers;
@override Map<String, String> get answers {
  if (_answers is EqualUnmodifiableMapView) return _answers;
  // ignore: implicit_dynamic_type
  return EqualUnmodifiableMapView(_answers);
}

@override final  int score;
@override final  int totalPoints;
@override final  double percentage;
@override@JsonKey() final  bool passed;
@override final  int attemptNumber;
@override final  DateTime startTime;
@override final  DateTime? endTime;
@override final  int timeSpent;

/// Create a copy of QuizAttemptDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$QuizAttemptDtoCopyWith<_QuizAttemptDto> get copyWith => __$QuizAttemptDtoCopyWithImpl<_QuizAttemptDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$QuizAttemptDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _QuizAttemptDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.quizId, quizId) || other.quizId == quizId)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&const DeepCollectionEquality().equals(other._answers, _answers)&&(identical(other.score, score) || other.score == score)&&(identical(other.totalPoints, totalPoints) || other.totalPoints == totalPoints)&&(identical(other.percentage, percentage) || other.percentage == percentage)&&(identical(other.passed, passed) || other.passed == passed)&&(identical(other.attemptNumber, attemptNumber) || other.attemptNumber == attemptNumber)&&(identical(other.startTime, startTime) || other.startTime == startTime)&&(identical(other.endTime, endTime) || other.endTime == endTime)&&(identical(other.timeSpent, timeSpent) || other.timeSpent == timeSpent));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,quizId,courseId,const DeepCollectionEquality().hash(_answers),score,totalPoints,percentage,passed,attemptNumber,startTime,endTime,timeSpent);

@override
String toString() {
  return 'QuizAttemptDto(id: $id, userId: $userId, quizId: $quizId, courseId: $courseId, answers: $answers, score: $score, totalPoints: $totalPoints, percentage: $percentage, passed: $passed, attemptNumber: $attemptNumber, startTime: $startTime, endTime: $endTime, timeSpent: $timeSpent)';
}


}

/// @nodoc
abstract mixin class _$QuizAttemptDtoCopyWith<$Res> implements $QuizAttemptDtoCopyWith<$Res> {
  factory _$QuizAttemptDtoCopyWith(_QuizAttemptDto value, $Res Function(_QuizAttemptDto) _then) = __$QuizAttemptDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String quizId, String courseId, Map<String, String> answers, int score, int totalPoints, double percentage, bool passed, int attemptNumber, DateTime startTime, DateTime? endTime, int timeSpent
});




}
/// @nodoc
class __$QuizAttemptDtoCopyWithImpl<$Res>
    implements _$QuizAttemptDtoCopyWith<$Res> {
  __$QuizAttemptDtoCopyWithImpl(this._self, this._then);

  final _QuizAttemptDto _self;
  final $Res Function(_QuizAttemptDto) _then;

/// Create a copy of QuizAttemptDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? quizId = null,Object? courseId = null,Object? answers = null,Object? score = null,Object? totalPoints = null,Object? percentage = null,Object? passed = null,Object? attemptNumber = null,Object? startTime = null,Object? endTime = freezed,Object? timeSpent = null,}) {
  return _then(_QuizAttemptDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,quizId: null == quizId ? _self.quizId : quizId // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,answers: null == answers ? _self._answers : answers // ignore: cast_nullable_to_non_nullable
as Map<String, String>,score: null == score ? _self.score : score // ignore: cast_nullable_to_non_nullable
as int,totalPoints: null == totalPoints ? _self.totalPoints : totalPoints // ignore: cast_nullable_to_non_nullable
as int,percentage: null == percentage ? _self.percentage : percentage // ignore: cast_nullable_to_non_nullable
as double,passed: null == passed ? _self.passed : passed // ignore: cast_nullable_to_non_nullable
as bool,attemptNumber: null == attemptNumber ? _self.attemptNumber : attemptNumber // ignore: cast_nullable_to_non_nullable
as int,startTime: null == startTime ? _self.startTime : startTime // ignore: cast_nullable_to_non_nullable
as DateTime,endTime: freezed == endTime ? _self.endTime : endTime // ignore: cast_nullable_to_non_nullable
as DateTime?,timeSpent: null == timeSpent ? _self.timeSpent : timeSpent // ignore: cast_nullable_to_non_nullable
as int,
  ));
}


}

// dart format on
