import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/certificate_dto.dart';
import '../data/mock/mock_certificate_service.dart';

part 'certificate_provider.g.dart';

@riverpod
Future<List<CertificateDto>> userCertificates(Ref ref,
  String userId,
) async {
  return await MockCertificateService.getByUser(userId);
}

@riverpod
Future<CertificateDto> certificate(Ref ref, String id) async {
  return await MockCertificateService.get(id);
}

@riverpod
Future<CertificateDto> issueCertificate(Ref ref, {
  required String userId,
  required String userName,
  required String courseId,
  required String courseTitle,
  required String instructorName,
  required double finalScore,
}) async {
  final cert = await MockCertificateService.issue(
    userId: userId,
    userName: userName,
    courseId: courseId,
    courseTitle: courseTitle,
    instructorName: instructorName,
    finalScore: finalScore,
  );

  ref.invalidate(userCertificatesProvider(userId));
  return cert;
}
