import 'package:freezed_annotation/freezed_annotation.dart';

part 'certificate_dto.freezed.dart';
part 'certificate_dto.g.dart';

/// Certificate DTO
@freezed
abstract class CertificateDto with _$CertificateDto {
  const factory CertificateDto({
    required String id,
    required String userId,
    required String userName,
    required String courseId,
    required String courseTitle,
    required String instructorName,
    required DateTime issuedDate,
    required String certificateNumber,
    @Default(100.0) double finalScore,
    DateTime? expiryDate,
    @Default(false) bool isVerified,
    String? verificationUrl,
  }) = _CertificateDto;

  factory CertificateDto.fromJson(Map<String, dynamic> json) =>
      _$CertificateDtoFromJson(json);
}
