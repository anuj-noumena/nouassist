// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'course_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(CourseList)
const courseListProvider = CourseListProvider._();

final class CourseListProvider
    extends $AsyncNotifierProvider<CourseList, PagedResultDto<CourseDto>> {
  const CourseListProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'courseListProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$courseListHash();

  @$internal
  @override
  CourseList create() => CourseList();
}

String _$courseListHash() => r'b93ea85016d417901d8cf5d72671175192d89875';

abstract class _$CourseList extends $AsyncNotifier<PagedResultDto<CourseDto>> {
  FutureOr<PagedResultDto<CourseDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref
            as $Ref<
              AsyncValue<PagedResultDto<CourseDto>>,
              PagedResultDto<CourseDto>
            >;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<PagedResultDto<CourseDto>>,
                PagedResultDto<CourseDto>
              >,
              AsyncValue<PagedResultDto<CourseDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(course)
const courseProvider = CourseFamily._();

final class CourseProvider
    extends
        $FunctionalProvider<
          AsyncValue<CourseDto>,
          CourseDto,
          FutureOr<CourseDto>
        >
    with $FutureModifier<CourseDto>, $FutureProvider<CourseDto> {
  const CourseProvider._({
    required CourseFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'courseProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$courseHash();

  @override
  String toString() {
    return r'courseProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<CourseDto> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<CourseDto> create(Ref ref) {
    final argument = this.argument as String;
    return course(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CourseProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$courseHash() => r'40bbcea1420ba9029d16c48f234c3f1ec9382d09';

final class CourseFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<CourseDto>, String> {
  const CourseFamily._()
    : super(
        retry: null,
        name: r'courseProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CourseProvider call(String id) => CourseProvider._(argument: id, from: this);

  @override
  String toString() => r'courseProvider';
}

@ProviderFor(featuredCourses)
const featuredCoursesProvider = FeaturedCoursesProvider._();

final class FeaturedCoursesProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<CourseDto>>,
          List<CourseDto>,
          FutureOr<List<CourseDto>>
        >
    with $FutureModifier<List<CourseDto>>, $FutureProvider<List<CourseDto>> {
  const FeaturedCoursesProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'featuredCoursesProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$featuredCoursesHash();

  @$internal
  @override
  $FutureProviderElement<List<CourseDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<CourseDto>> create(Ref ref) {
    return featuredCourses(ref);
  }
}

String _$featuredCoursesHash() => r'169b1af7fcde0290f367f71387198e77ac9f0d6f';

@ProviderFor(createCourse)
const createCourseProvider = CreateCourseFamily._();

final class CreateCourseProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateCourseProvider._({
    required CreateCourseFamily super.from,
    required CreateUpdateCourseDto super.argument,
  }) : super(
         retry: null,
         name: r'createCourseProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createCourseHash();

  @override
  String toString() {
    return r'createCourseProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateCourseDto;
    return createCourse(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateCourseProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createCourseHash() => r'86cda94bf2bd0607141201a7e9b0c2071a137ba9';

final class CreateCourseFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateCourseDto> {
  const CreateCourseFamily._()
    : super(
        retry: null,
        name: r'createCourseProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateCourseProvider call(CreateUpdateCourseDto input) =>
      CreateCourseProvider._(argument: input, from: this);

  @override
  String toString() => r'createCourseProvider';
}

@ProviderFor(deleteCourse)
const deleteCourseProvider = DeleteCourseFamily._();

final class DeleteCourseProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteCourseProvider._({
    required DeleteCourseFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteCourseProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteCourseHash();

  @override
  String toString() {
    return r'deleteCourseProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as String;
    return deleteCourse(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteCourseProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteCourseHash() => r'10f4a6682d5da48a651e1a216c90ce7646b38d32';

final class DeleteCourseFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteCourseFamily._()
    : super(
        retry: null,
        name: r'deleteCourseProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteCourseProvider call(String id) =>
      DeleteCourseProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteCourseProvider';
}
