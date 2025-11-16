// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_form_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(ProjectForm)
const projectFormProvider = ProjectFormFamily._();

final class ProjectFormProvider
    extends $NotifierProvider<ProjectForm, ProjectFormState> {
  const ProjectFormProvider._({
    required ProjectFormFamily super.from,
    required String? super.argument,
  }) : super(
         retry: null,
         name: r'projectFormProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$projectFormHash();

  @override
  String toString() {
    return r'projectFormProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  ProjectForm create() => ProjectForm();

  /// {@macro riverpod.override_with_value}
  Override overrideWithValue(ProjectFormState value) {
    return $ProviderOverride(
      origin: this,
      providerOverride: $SyncValueProvider<ProjectFormState>(value),
    );
  }

  @override
  bool operator ==(Object other) {
    return other is ProjectFormProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$projectFormHash() => r'c1ba3533fcc748147dd725848355c54646ca61c5';

final class ProjectFormFamily extends $Family
    with
        $ClassFamilyOverride<
          ProjectForm,
          ProjectFormState,
          ProjectFormState,
          ProjectFormState,
          String?
        > {
  const ProjectFormFamily._()
    : super(
        retry: null,
        name: r'projectFormProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ProjectFormProvider call({String? projectId}) =>
      ProjectFormProvider._(argument: projectId, from: this);

  @override
  String toString() => r'projectFormProvider';
}

abstract class _$ProjectForm extends $Notifier<ProjectFormState> {
  late final _$args = ref.$arg as String?;
  String? get projectId => _$args;

  ProjectFormState build({String? projectId});
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(projectId: _$args);
    final ref = this.ref as $Ref<ProjectFormState, ProjectFormState>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<ProjectFormState, ProjectFormState>,
              ProjectFormState,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}
