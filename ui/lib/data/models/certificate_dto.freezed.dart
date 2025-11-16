// GENERATED CODE - DO NOT MODIFY BY HAND
// coverage:ignore-file
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'certificate_dto.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

// dart format off
T _$identity<T>(T value) => value;

/// @nodoc
mixin _$CertificateDto {

 String get id; String get userId; String get userName; String get courseId; String get courseTitle; String get instructorName; DateTime get issuedDate; String get certificateNumber; double get finalScore; DateTime? get expiryDate; bool get isVerified; String? get verificationUrl;
/// Create a copy of CertificateDto
/// with the given fields replaced by the non-null parameter values.
@JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
$CertificateDtoCopyWith<CertificateDto> get copyWith => _$CertificateDtoCopyWithImpl<CertificateDto>(this as CertificateDto, _$identity);

  /// Serializes this CertificateDto to a JSON map.
  Map<String, dynamic> toJson();


@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is CertificateDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.userName, userName) || other.userName == userName)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.courseTitle, courseTitle) || other.courseTitle == courseTitle)&&(identical(other.instructorName, instructorName) || other.instructorName == instructorName)&&(identical(other.issuedDate, issuedDate) || other.issuedDate == issuedDate)&&(identical(other.certificateNumber, certificateNumber) || other.certificateNumber == certificateNumber)&&(identical(other.finalScore, finalScore) || other.finalScore == finalScore)&&(identical(other.expiryDate, expiryDate) || other.expiryDate == expiryDate)&&(identical(other.isVerified, isVerified) || other.isVerified == isVerified)&&(identical(other.verificationUrl, verificationUrl) || other.verificationUrl == verificationUrl));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,userName,courseId,courseTitle,instructorName,issuedDate,certificateNumber,finalScore,expiryDate,isVerified,verificationUrl);

@override
String toString() {
  return 'CertificateDto(id: $id, userId: $userId, userName: $userName, courseId: $courseId, courseTitle: $courseTitle, instructorName: $instructorName, issuedDate: $issuedDate, certificateNumber: $certificateNumber, finalScore: $finalScore, expiryDate: $expiryDate, isVerified: $isVerified, verificationUrl: $verificationUrl)';
}


}

/// @nodoc
abstract mixin class $CertificateDtoCopyWith<$Res>  {
  factory $CertificateDtoCopyWith(CertificateDto value, $Res Function(CertificateDto) _then) = _$CertificateDtoCopyWithImpl;
@useResult
$Res call({
 String id, String userId, String userName, String courseId, String courseTitle, String instructorName, DateTime issuedDate, String certificateNumber, double finalScore, DateTime? expiryDate, bool isVerified, String? verificationUrl
});




}
/// @nodoc
class _$CertificateDtoCopyWithImpl<$Res>
    implements $CertificateDtoCopyWith<$Res> {
  _$CertificateDtoCopyWithImpl(this._self, this._then);

  final CertificateDto _self;
  final $Res Function(CertificateDto) _then;

/// Create a copy of CertificateDto
/// with the given fields replaced by the non-null parameter values.
@pragma('vm:prefer-inline') @override $Res call({Object? id = null,Object? userId = null,Object? userName = null,Object? courseId = null,Object? courseTitle = null,Object? instructorName = null,Object? issuedDate = null,Object? certificateNumber = null,Object? finalScore = null,Object? expiryDate = freezed,Object? isVerified = null,Object? verificationUrl = freezed,}) {
  return _then(_self.copyWith(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,userName: null == userName ? _self.userName : userName // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,courseTitle: null == courseTitle ? _self.courseTitle : courseTitle // ignore: cast_nullable_to_non_nullable
as String,instructorName: null == instructorName ? _self.instructorName : instructorName // ignore: cast_nullable_to_non_nullable
as String,issuedDate: null == issuedDate ? _self.issuedDate : issuedDate // ignore: cast_nullable_to_non_nullable
as DateTime,certificateNumber: null == certificateNumber ? _self.certificateNumber : certificateNumber // ignore: cast_nullable_to_non_nullable
as String,finalScore: null == finalScore ? _self.finalScore : finalScore // ignore: cast_nullable_to_non_nullable
as double,expiryDate: freezed == expiryDate ? _self.expiryDate : expiryDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isVerified: null == isVerified ? _self.isVerified : isVerified // ignore: cast_nullable_to_non_nullable
as bool,verificationUrl: freezed == verificationUrl ? _self.verificationUrl : verificationUrl // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}

}


/// Adds pattern-matching-related methods to [CertificateDto].
extension CertificateDtoPatterns on CertificateDto {
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

@optionalTypeArgs TResult maybeMap<TResult extends Object?>(TResult Function( _CertificateDto value)?  $default,{required TResult orElse(),}){
final _that = this;
switch (_that) {
case _CertificateDto() when $default != null:
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

@optionalTypeArgs TResult map<TResult extends Object?>(TResult Function( _CertificateDto value)  $default,){
final _that = this;
switch (_that) {
case _CertificateDto():
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

@optionalTypeArgs TResult? mapOrNull<TResult extends Object?>(TResult? Function( _CertificateDto value)?  $default,){
final _that = this;
switch (_that) {
case _CertificateDto() when $default != null:
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

@optionalTypeArgs TResult maybeWhen<TResult extends Object?>(TResult Function( String id,  String userId,  String userName,  String courseId,  String courseTitle,  String instructorName,  DateTime issuedDate,  String certificateNumber,  double finalScore,  DateTime? expiryDate,  bool isVerified,  String? verificationUrl)?  $default,{required TResult orElse(),}) {final _that = this;
switch (_that) {
case _CertificateDto() when $default != null:
return $default(_that.id,_that.userId,_that.userName,_that.courseId,_that.courseTitle,_that.instructorName,_that.issuedDate,_that.certificateNumber,_that.finalScore,_that.expiryDate,_that.isVerified,_that.verificationUrl);case _:
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

@optionalTypeArgs TResult when<TResult extends Object?>(TResult Function( String id,  String userId,  String userName,  String courseId,  String courseTitle,  String instructorName,  DateTime issuedDate,  String certificateNumber,  double finalScore,  DateTime? expiryDate,  bool isVerified,  String? verificationUrl)  $default,) {final _that = this;
switch (_that) {
case _CertificateDto():
return $default(_that.id,_that.userId,_that.userName,_that.courseId,_that.courseTitle,_that.instructorName,_that.issuedDate,_that.certificateNumber,_that.finalScore,_that.expiryDate,_that.isVerified,_that.verificationUrl);case _:
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

@optionalTypeArgs TResult? whenOrNull<TResult extends Object?>(TResult? Function( String id,  String userId,  String userName,  String courseId,  String courseTitle,  String instructorName,  DateTime issuedDate,  String certificateNumber,  double finalScore,  DateTime? expiryDate,  bool isVerified,  String? verificationUrl)?  $default,) {final _that = this;
switch (_that) {
case _CertificateDto() when $default != null:
return $default(_that.id,_that.userId,_that.userName,_that.courseId,_that.courseTitle,_that.instructorName,_that.issuedDate,_that.certificateNumber,_that.finalScore,_that.expiryDate,_that.isVerified,_that.verificationUrl);case _:
  return null;

}
}

}

/// @nodoc
@JsonSerializable()

class _CertificateDto implements CertificateDto {
  const _CertificateDto({required this.id, required this.userId, required this.userName, required this.courseId, required this.courseTitle, required this.instructorName, required this.issuedDate, required this.certificateNumber, this.finalScore = 100.0, this.expiryDate, this.isVerified = false, this.verificationUrl});
  factory _CertificateDto.fromJson(Map<String, dynamic> json) => _$CertificateDtoFromJson(json);

@override final  String id;
@override final  String userId;
@override final  String userName;
@override final  String courseId;
@override final  String courseTitle;
@override final  String instructorName;
@override final  DateTime issuedDate;
@override final  String certificateNumber;
@override@JsonKey() final  double finalScore;
@override final  DateTime? expiryDate;
@override@JsonKey() final  bool isVerified;
@override final  String? verificationUrl;

/// Create a copy of CertificateDto
/// with the given fields replaced by the non-null parameter values.
@override @JsonKey(includeFromJson: false, includeToJson: false)
@pragma('vm:prefer-inline')
_$CertificateDtoCopyWith<_CertificateDto> get copyWith => __$CertificateDtoCopyWithImpl<_CertificateDto>(this, _$identity);

@override
Map<String, dynamic> toJson() {
  return _$CertificateDtoToJson(this, );
}

@override
bool operator ==(Object other) {
  return identical(this, other) || (other.runtimeType == runtimeType&&other is _CertificateDto&&(identical(other.id, id) || other.id == id)&&(identical(other.userId, userId) || other.userId == userId)&&(identical(other.userName, userName) || other.userName == userName)&&(identical(other.courseId, courseId) || other.courseId == courseId)&&(identical(other.courseTitle, courseTitle) || other.courseTitle == courseTitle)&&(identical(other.instructorName, instructorName) || other.instructorName == instructorName)&&(identical(other.issuedDate, issuedDate) || other.issuedDate == issuedDate)&&(identical(other.certificateNumber, certificateNumber) || other.certificateNumber == certificateNumber)&&(identical(other.finalScore, finalScore) || other.finalScore == finalScore)&&(identical(other.expiryDate, expiryDate) || other.expiryDate == expiryDate)&&(identical(other.isVerified, isVerified) || other.isVerified == isVerified)&&(identical(other.verificationUrl, verificationUrl) || other.verificationUrl == verificationUrl));
}

@JsonKey(includeFromJson: false, includeToJson: false)
@override
int get hashCode => Object.hash(runtimeType,id,userId,userName,courseId,courseTitle,instructorName,issuedDate,certificateNumber,finalScore,expiryDate,isVerified,verificationUrl);

@override
String toString() {
  return 'CertificateDto(id: $id, userId: $userId, userName: $userName, courseId: $courseId, courseTitle: $courseTitle, instructorName: $instructorName, issuedDate: $issuedDate, certificateNumber: $certificateNumber, finalScore: $finalScore, expiryDate: $expiryDate, isVerified: $isVerified, verificationUrl: $verificationUrl)';
}


}

/// @nodoc
abstract mixin class _$CertificateDtoCopyWith<$Res> implements $CertificateDtoCopyWith<$Res> {
  factory _$CertificateDtoCopyWith(_CertificateDto value, $Res Function(_CertificateDto) _then) = __$CertificateDtoCopyWithImpl;
@override @useResult
$Res call({
 String id, String userId, String userName, String courseId, String courseTitle, String instructorName, DateTime issuedDate, String certificateNumber, double finalScore, DateTime? expiryDate, bool isVerified, String? verificationUrl
});




}
/// @nodoc
class __$CertificateDtoCopyWithImpl<$Res>
    implements _$CertificateDtoCopyWith<$Res> {
  __$CertificateDtoCopyWithImpl(this._self, this._then);

  final _CertificateDto _self;
  final $Res Function(_CertificateDto) _then;

/// Create a copy of CertificateDto
/// with the given fields replaced by the non-null parameter values.
@override @pragma('vm:prefer-inline') $Res call({Object? id = null,Object? userId = null,Object? userName = null,Object? courseId = null,Object? courseTitle = null,Object? instructorName = null,Object? issuedDate = null,Object? certificateNumber = null,Object? finalScore = null,Object? expiryDate = freezed,Object? isVerified = null,Object? verificationUrl = freezed,}) {
  return _then(_CertificateDto(
id: null == id ? _self.id : id // ignore: cast_nullable_to_non_nullable
as String,userId: null == userId ? _self.userId : userId // ignore: cast_nullable_to_non_nullable
as String,userName: null == userName ? _self.userName : userName // ignore: cast_nullable_to_non_nullable
as String,courseId: null == courseId ? _self.courseId : courseId // ignore: cast_nullable_to_non_nullable
as String,courseTitle: null == courseTitle ? _self.courseTitle : courseTitle // ignore: cast_nullable_to_non_nullable
as String,instructorName: null == instructorName ? _self.instructorName : instructorName // ignore: cast_nullable_to_non_nullable
as String,issuedDate: null == issuedDate ? _self.issuedDate : issuedDate // ignore: cast_nullable_to_non_nullable
as DateTime,certificateNumber: null == certificateNumber ? _self.certificateNumber : certificateNumber // ignore: cast_nullable_to_non_nullable
as String,finalScore: null == finalScore ? _self.finalScore : finalScore // ignore: cast_nullable_to_non_nullable
as double,expiryDate: freezed == expiryDate ? _self.expiryDate : expiryDate // ignore: cast_nullable_to_non_nullable
as DateTime?,isVerified: null == isVerified ? _self.isVerified : isVerified // ignore: cast_nullable_to_non_nullable
as bool,verificationUrl: freezed == verificationUrl ? _self.verificationUrl : verificationUrl // ignore: cast_nullable_to_non_nullable
as String?,
  ));
}


}

// dart format on
