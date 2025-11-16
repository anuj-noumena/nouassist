// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'progress_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(userCourseProgress)
const userCourseProgressProvider = UserCourseProgressFamily._();

final class UserCourseProgressProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<CourseProgressDto>>,
          List<CourseProgressDto>,
          FutureOr<List<CourseProgressDto>>
        >
    with
        $FutureModifier<List<CourseProgressDto>>,
        $FutureProvider<List<CourseProgressDto>> {
  const UserCourseProgressProvider._({
    required UserCourseProgressFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'userCourseProgressProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$userCourseProgressHash();

  @override
  String toString() {
    return r'userCourseProgressProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<CourseProgressDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<CourseProgressDto>> create(Ref ref) {
    final argument = this.argument as String;
    return userCourseProgress(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is UserCourseProgressProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$userCourseProgressHash() =>
    r'8dd36a547e1d9910e77247a29448971977860d60';

final class UserCourseProgressFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<CourseProgressDto>>, String> {
  const UserCourseProgressFamily._()
    : super(
        retry: null,
        name: r'userCourseProgressProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UserCourseProgressProvider call(String userId) =>
      UserCourseProgressProvider._(argument: userId, from: this);

  @override
  String toString() => r'userCourseProgressProvider';
}

@ProviderFor(courseProgress)
const courseProgressProvider = CourseProgressFamily._();

final class CourseProgressProvider
    extends
        $FunctionalProvider<
          AsyncValue<CourseProgressDto?>,
          CourseProgressDto?,
          FutureOr<CourseProgressDto?>
        >
    with
        $FutureModifier<CourseProgressDto?>,
        $FutureProvider<CourseProgressDto?> {
  const CourseProgressProvider._({
    required CourseProgressFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'courseProgressProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$courseProgressHash();

  @override
  String toString() {
    return r'courseProgressProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<CourseProgressDto?> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<CourseProgressDto?> create(Ref ref) {
    final argument = this.argument as (String, String);
    return courseProgress(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is CourseProgressProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$courseProgressHash() => r'5a5ff47b296079978d3f27e01c5f282f952fcccc';

final class CourseProgressFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<CourseProgressDto?>,
          (String, String)
        > {
  const CourseProgressFamily._()
    : super(
        retry: null,
        name: r'courseProgressProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CourseProgressProvider call(String userId, String courseId) =>
      CourseProgressProvider._(argument: (userId, courseId), from: this);

  @override
  String toString() => r'courseProgressProvider';
}

@ProviderFor(updateProgress)
const updateProgressProvider = UpdateProgressFamily._();

final class UpdateProgressProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const UpdateProgressProvider._({
    required UpdateProgressFamily super.from,
    required (
      String,
      String, {
      int completedLessons,
      int totalLessons,
      int timeSpent,
    })
    super.argument,
  }) : super(
         retry: null,
         name: r'updateProgressProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$updateProgressHash();

  @override
  String toString() {
    return r'updateProgressProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument =
        this.argument
            as (
              String,
              String, {
              int completedLessons,
              int totalLessons,
              int timeSpent,
            });
    return updateProgress(
      ref,
      argument.$1,
      argument.$2,
      completedLessons: argument.completedLessons,
      totalLessons: argument.totalLessons,
      timeSpent: argument.timeSpent,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is UpdateProgressProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$updateProgressHash() => r'97e5266fbe93cf7f72cfb2dc3cb2b0db49adc40f';

final class UpdateProgressFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<void>,
          (
            String,
            String, {
            int completedLessons,
            int totalLessons,
            int timeSpent,
          })
        > {
  const UpdateProgressFamily._()
    : super(
        retry: null,
        name: r'updateProgressProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UpdateProgressProvider call(
    String userId,
    String courseId, {
    required int completedLessons,
    required int totalLessons,
    required int timeSpent,
  }) => UpdateProgressProvider._(
    argument: (
      userId,
      courseId,
      completedLessons: completedLessons,
      totalLessons: totalLessons,
      timeSpent: timeSpent,
    ),
    from: this,
  );

  @override
  String toString() => r'updateProgressProvider';
}

@ProviderFor(markLessonComplete)
const markLessonCompleteProvider = MarkLessonCompleteFamily._();

final class MarkLessonCompleteProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const MarkLessonCompleteProvider._({
    required MarkLessonCompleteFamily super.from,
    required (String, String, String, int) super.argument,
  }) : super(
         retry: null,
         name: r'markLessonCompleteProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$markLessonCompleteHash();

  @override
  String toString() {
    return r'markLessonCompleteProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, String, String, int);
    return markLessonComplete(
      ref,
      argument.$1,
      argument.$2,
      argument.$3,
      argument.$4,
    );
  }

  @override
  bool operator ==(Object other) {
    return other is MarkLessonCompleteProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$markLessonCompleteHash() =>
    r'7c98eba3fbfa486d4d380f9bb4ab17ac3f581d40';

final class MarkLessonCompleteFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<void>,
          (String, String, String, int)
        > {
  const MarkLessonCompleteFamily._()
    : super(
        retry: null,
        name: r'markLessonCompleteProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  MarkLessonCompleteProvider call(
    String userId,
    String lessonId,
    String courseId,
    int timeSpent,
  ) => MarkLessonCompleteProvider._(
    argument: (userId, lessonId, courseId, timeSpent),
    from: this,
  );

  @override
  String toString() => r'markLessonCompleteProvider';
}
