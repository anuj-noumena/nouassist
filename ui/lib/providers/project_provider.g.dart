// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(Project)
const projectProvider = ProjectFamily._();

final class ProjectProvider
    extends $AsyncNotifierProvider<Project, ProjectDto> {
  const ProjectProvider._({
    required ProjectFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'projectProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$projectHash();

  @override
  String toString() {
    return r'projectProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  Project create() => Project();

  @override
  bool operator ==(Object other) {
    return other is ProjectProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$projectHash() => r'2cf9c173420e0b9a66a5071b498fce2991d9507a';

final class ProjectFamily extends $Family
    with
        $ClassFamilyOverride<
          Project,
          AsyncValue<ProjectDto>,
          ProjectDto,
          FutureOr<ProjectDto>,
          String
        > {
  const ProjectFamily._()
    : super(
        retry: null,
        name: r'projectProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ProjectProvider call(String id) =>
      ProjectProvider._(argument: id, from: this);

  @override
  String toString() => r'projectProvider';
}

abstract class _$Project extends $AsyncNotifier<ProjectDto> {
  late final _$args = ref.$arg as String;
  String get id => _$args;

  FutureOr<ProjectDto> build(String id);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref = this.ref as $Ref<AsyncValue<ProjectDto>, ProjectDto>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<ProjectDto>, ProjectDto>,
              AsyncValue<ProjectDto>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(childProjects)
const childProjectsProvider = ChildProjectsFamily._();

final class ChildProjectsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<ProjectDto>>,
          List<ProjectDto>,
          FutureOr<List<ProjectDto>>
        >
    with $FutureModifier<List<ProjectDto>>, $FutureProvider<List<ProjectDto>> {
  const ChildProjectsProvider._({
    required ChildProjectsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'childProjectsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$childProjectsHash();

  @override
  String toString() {
    return r'childProjectsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<ProjectDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<ProjectDto>> create(Ref ref) {
    final argument = this.argument as String;
    return childProjects(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is ChildProjectsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$childProjectsHash() => r'91d84d7487eadf4a73c0fd9b2b6c83fd76f15401';

final class ChildProjectsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<ProjectDto>>, String> {
  const ChildProjectsFamily._()
    : super(
        retry: null,
        name: r'childProjectsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ChildProjectsProvider call(String parentId) =>
      ChildProjectsProvider._(argument: parentId, from: this);

  @override
  String toString() => r'childProjectsProvider';
}
