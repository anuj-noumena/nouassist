// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'certificate_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(userCertificates)
const userCertificatesProvider = UserCertificatesFamily._();

final class UserCertificatesProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<CertificateDto>>,
          List<CertificateDto>,
          FutureOr<List<CertificateDto>>
        >
    with
        $FutureModifier<List<CertificateDto>>,
        $FutureProvider<List<CertificateDto>> {
  const UserCertificatesProvider._({
    required UserCertificatesFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'userCertificatesProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$userCertificatesHash();

  @override
  String toString() {
    return r'userCertificatesProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<CertificateDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<CertificateDto>> create(Ref ref) {
    final argument = this.argument as String;
    return userCertificates(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is UserCertificatesProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$userCertificatesHash() => r'de320b9a4577226763f26f88995514635eaf02d2';

final class UserCertificatesFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<CertificateDto>>, String> {
  const UserCertificatesFamily._()
    : super(
        retry: null,
        name: r'userCertificatesProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UserCertificatesProvider call(String userId) =>
      UserCertificatesProvider._(argument: userId, from: this);

  @override
  String toString() => r'userCertificatesProvider';
}

@ProviderFor(certificate)
const certificateProvider = CertificateFamily._();

final class CertificateProvider
    extends
        $FunctionalProvider<
          AsyncValue<CertificateDto>,
          CertificateDto,
          FutureOr<CertificateDto>
        >
    with $FutureModifier<CertificateDto>, $FutureProvider<CertificateDto> {
  const CertificateProvider._({
    required CertificateFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'certificateProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$certificateHash();

  @override
  String toString() {
    return r'certificateProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<CertificateDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<CertificateDto> create(Ref ref) {
    final argument = this.argument as String;
    return certificate(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CertificateProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$certificateHash() => r'9be8cd9c44544031a7e368121cb5aa34208b96c8';

final class CertificateFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<CertificateDto>, String> {
  const CertificateFamily._()
    : super(
        retry: null,
        name: r'certificateProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CertificateProvider call(String id) =>
      CertificateProvider._(argument: id, from: this);

  @override
  String toString() => r'certificateProvider';
}

@ProviderFor(issueCertificate)
const issueCertificateProvider = IssueCertificateFamily._();

final class IssueCertificateProvider
    extends
        $FunctionalProvider<
          AsyncValue<CertificateDto>,
          CertificateDto,
          FutureOr<CertificateDto>
        >
    with $FutureModifier<CertificateDto>, $FutureProvider<CertificateDto> {
  const IssueCertificateProvider._({
    required IssueCertificateFamily super.from,
    required ({
      String userId,
      String userName,
      String courseId,
      String courseTitle,
      String instructorName,
      double finalScore,
    })
    super.argument,
  }) : super(
         retry: null,
         name: r'issueCertificateProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$issueCertificateHash();

  @override
  String toString() {
    return r'issueCertificateProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<CertificateDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<CertificateDto> create(Ref ref) {
    final argument =
        this.argument
            as ({
              String userId,
              String userName,
              String courseId,
              String courseTitle,
              String instructorName,
              double finalScore,
            });
    return issueCertificate(
      ref,
      userId: argument.userId,
      userName: argument.userName,
      courseId: argument.courseId,
      courseTitle: argument.courseTitle,
      instructorName: argument.instructorName,
      finalScore: argument.finalScore,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is IssueCertificateProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$issueCertificateHash() => r'94a03cbbedd33e21d9c327acca2c23c9a4b0e840';

final class IssueCertificateFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<CertificateDto>,
          ({
            String userId,
            String userName,
            String courseId,
            String courseTitle,
            String instructorName,
            double finalScore,
          })
        > {
  const IssueCertificateFamily._()
    : super(
        retry: null,
        name: r'issueCertificateProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  IssueCertificateProvider call({
    required String userId,
    required String userName,
    required String courseId,
    required String courseTitle,
    required String instructorName,
    required double finalScore,
  }) => IssueCertificateProvider._(
    argument: (
      userId: userId,
      userName: userName,
      courseId: courseId,
      courseTitle: courseTitle,
      instructorName: instructorName,
      finalScore: finalScore,
    ),
    from: this,
  );

  @override
  String toString() => r'issueCertificateProvider';
}
