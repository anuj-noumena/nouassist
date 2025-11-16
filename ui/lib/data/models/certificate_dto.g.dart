// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'certificate_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_CertificateDto _$CertificateDtoFromJson(Map<String, dynamic> json) =>
    _CertificateDto(
      id: json['id'] as String,
      userId: json['userId'] as String,
      userName: json['userName'] as String,
      courseId: json['courseId'] as String,
      courseTitle: json['courseTitle'] as String,
      instructorName: json['instructorName'] as String,
      issuedDate: DateTime.parse(json['issuedDate'] as String),
      certificateNumber: json['certificateNumber'] as String,
      finalScore: (json['finalScore'] as num?)?.toDouble() ?? 100.0,
      expiryDate: json['expiryDate'] == null
          ? null
          : DateTime.parse(json['expiryDate'] as String),
      isVerified: json['isVerified'] as bool? ?? false,
      verificationUrl: json['verificationUrl'] as String?,
    );

Map<String, dynamic> _$CertificateDtoToJson(_CertificateDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'userId': instance.userId,
      'userName': instance.userName,
      'courseId': instance.courseId,
      'courseTitle': instance.courseTitle,
      'instructorName': instance.instructorName,
      'issuedDate': instance.issuedDate.toIso8601String(),
      'certificateNumber': instance.certificateNumber,
      'finalScore': instance.finalScore,
      'expiryDate': instance.expiryDate?.toIso8601String(),
      'isVerified': instance.isVerified,
      'verificationUrl': instance.verificationUrl,
    };
