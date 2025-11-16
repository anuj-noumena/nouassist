import '../models/certificate_dto.dart';

/// Mock certificate service
class MockCertificateService {
  static final List<CertificateDto> _certificates = _generateMockCertificates();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get certificates by user
  static Future<List<CertificateDto>> getByUser(String userId) async {
    await _simulateDelay();
    return _certificates.where((c) => c.userId == userId).toList()
      ..sort((a, b) => b.issuedDate.compareTo(a.issuedDate));
  }

  /// Get single certificate
  static Future<CertificateDto> get(String id) async {
    await _simulateDelay();

    try {
      return _certificates.firstWhere((cert) => cert.id == id);
    } catch (e) {
      throw Exception('Certificate not found');
    }
  }

  /// Issue certificate
  static Future<CertificateDto> issue({
    required String userId,
    required String userName,
    required String courseId,
    required String courseTitle,
    required String instructorName,
    required double finalScore,
  }) async {
    await _simulateDelay();

    // Check if certificate already exists
    final existing = _certificates.where(
      (c) => c.userId == userId && c.courseId == courseId,
    );

    if (existing.isNotEmpty) {
      throw Exception('Certificate already issued for this course');
    }

    final certificate = CertificateDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: userId,
      userName: userName,
      courseId: courseId,
      courseTitle: courseTitle,
      instructorName: instructorName,
      issuedDate: DateTime.now(),
      certificateNumber: _generateCertificateNumber(),
      finalScore: finalScore,
      isVerified: true,
      verificationUrl: 'https://verify.example.com/cert/${DateTime.now().millisecondsSinceEpoch}',
    );

    _certificates.insert(0, certificate);
    return certificate;
  }

  /// Generate certificate number
  static String _generateCertificateNumber() {
    final now = DateTime.now();
    return 'CERT-${now.year}-${now.millisecondsSinceEpoch}';
  }

  /// Generate mock certificates
  static List<CertificateDto> _generateMockCertificates() {
    final now = DateTime.now();
    return [
      CertificateDto(
        id: 'cert-1',
        userId: 'user-1',
        userName: 'John Doe',
        courseId: 'course-3',
        courseTitle: 'Python for Data Science',
        instructorName: 'Dr. Emily Watson',
        issuedDate: now.subtract(const Duration(days: 5)),
        certificateNumber: 'CERT-2024-1234567890',
        finalScore: 95.5,
        isVerified: true,
        verificationUrl: 'https://verify.example.com/cert/1234567890',
      ),
      CertificateDto(
        id: 'cert-2',
        userId: 'user-1',
        userName: 'John Doe',
        courseId: 'course-4',
        courseTitle: 'UI/UX Design Principles',
        instructorName: 'Alex Rivera',
        issuedDate: now.subtract(const Duration(days: 45)),
        certificateNumber: 'CERT-2024-1234567891',
        finalScore: 88.0,
        isVerified: true,
        verificationUrl: 'https://verify.example.com/cert/1234567891',
      ),
    ];
  }
}
