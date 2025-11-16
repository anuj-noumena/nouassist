// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'lesson_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(courseLessons)
const courseLessonsProvider = CourseLessonsFamily._();

final class CourseLessonsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<LessonDto>>,
          List<LessonDto>,
          FutureOr<List<LessonDto>>
        >
    with $FutureModifier<List<LessonDto>>, $FutureProvider<List<LessonDto>> {
  const CourseLessonsProvider._({
    required CourseLessonsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'courseLessonsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$courseLessonsHash();

  @override
  String toString() {
    return r'courseLessonsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<LessonDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<LessonDto>> create(Ref ref) {
    final argument = this.argument as String;
    return courseLessons(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CourseLessonsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$courseLessonsHash() => r'2ed64484a4808e68442696e8c10c8ab3c3e4c99f';

final class CourseLessonsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<LessonDto>>, String> {
  const CourseLessonsFamily._()
    : super(
        retry: null,
        name: r'courseLessonsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CourseLessonsProvider call(String courseId) =>
      CourseLessonsProvider._(argument: courseId, from: this);

  @override
  String toString() => r'courseLessonsProvider';
}

@ProviderFor(lesson)
const lessonProvider = LessonFamily._();

final class LessonProvider
    extends
        $FunctionalProvider<
          AsyncValue<LessonDto>,
          LessonDto,
          FutureOr<LessonDto>
        >
    with $FutureModifier<LessonDto>, $FutureProvider<LessonDto> {
  const LessonProvider._({
    required LessonFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'lessonProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$lessonHash();

  @override
  String toString() {
    return r'lessonProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<LessonDto> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<LessonDto> create(Ref ref) {
    final argument = this.argument as String;
    return lesson(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is LessonProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$lessonHash() => r'bcd81545423efb4dff50a57378534d4604bb86fc';

final class LessonFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<LessonDto>, String> {
  const LessonFamily._()
    : super(
        retry: null,
        name: r'lessonProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  LessonProvider call(String id) => LessonProvider._(argument: id, from: this);

  @override
  String toString() => r'lessonProvider';
}

@ProviderFor(createLesson)
const createLessonProvider = CreateLessonFamily._();

final class CreateLessonProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateLessonProvider._({
    required CreateLessonFamily super.from,
    required CreateUpdateLessonDto super.argument,
  }) : super(
         retry: null,
         name: r'createLessonProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createLessonHash();

  @override
  String toString() {
    return r'createLessonProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateLessonDto;
    return createLesson(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateLessonProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createLessonHash() => r'342a38949d2e06eb16cbd90131247792b8782266';

final class CreateLessonFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateLessonDto> {
  const CreateLessonFamily._()
    : super(
        retry: null,
        name: r'createLessonProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateLessonProvider call(CreateUpdateLessonDto input) =>
      CreateLessonProvider._(argument: input, from: this);

  @override
  String toString() => r'createLessonProvider';
}

@ProviderFor(deleteLesson)
const deleteLessonProvider = DeleteLessonFamily._();

final class DeleteLessonProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteLessonProvider._({
    required DeleteLessonFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteLessonProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteLessonHash();

  @override
  String toString() {
    return r'deleteLessonProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, String);
    return deleteLesson(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteLessonProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteLessonHash() => r'e6fd7330f3e2a9ab2d61a01ff5ebddc59d0f882b';

final class DeleteLessonFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteLessonFamily._()
    : super(
        retry: null,
        name: r'deleteLessonProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteLessonProvider call(String id, String courseId) =>
      DeleteLessonProvider._(argument: (id, courseId), from: this);

  @override
  String toString() => r'deleteLessonProvider';
}
