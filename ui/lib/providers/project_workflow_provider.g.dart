// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_workflow_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(projectWorkflows)
const projectWorkflowsProvider = ProjectWorkflowsProvider._();

final class ProjectWorkflowsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<ProjectWorkflowDto>>,
          List<ProjectWorkflowDto>,
          FutureOr<List<ProjectWorkflowDto>>
        >
    with
        $FutureModifier<List<ProjectWorkflowDto>>,
        $FutureProvider<List<ProjectWorkflowDto>> {
  const ProjectWorkflowsProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'projectWorkflowsProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$projectWorkflowsHash();

  @$internal
  @override
  $FutureProviderElement<List<ProjectWorkflowDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<ProjectWorkflowDto>> create(Ref ref) {
    return projectWorkflows(ref);
  }
}

String _$projectWorkflowsHash() => r'57c03c546accdc86e2c201915072b121b1b43a7d';

@ProviderFor(projectWorkflow)
const projectWorkflowProvider = ProjectWorkflowFamily._();

final class ProjectWorkflowProvider
    extends
        $FunctionalProvider<
          AsyncValue<ProjectWorkflowDto>,
          ProjectWorkflowDto,
          FutureOr<ProjectWorkflowDto>
        >
    with
        $FutureModifier<ProjectWorkflowDto>,
        $FutureProvider<ProjectWorkflowDto> {
  const ProjectWorkflowProvider._({
    required ProjectWorkflowFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'projectWorkflowProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$projectWorkflowHash();

  @override
  String toString() {
    return r'projectWorkflowProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<ProjectWorkflowDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<ProjectWorkflowDto> create(Ref ref) {
    final argument = this.argument as String;
    return projectWorkflow(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is ProjectWorkflowProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$projectWorkflowHash() => r'618686605be2ae5666c523c0869569d91a8ea9b4';

final class ProjectWorkflowFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<ProjectWorkflowDto>, String> {
  const ProjectWorkflowFamily._()
    : super(
        retry: null,
        name: r'projectWorkflowProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ProjectWorkflowProvider call(String id) =>
      ProjectWorkflowProvider._(argument: id, from: this);

  @override
  String toString() => r'projectWorkflowProvider';
}
