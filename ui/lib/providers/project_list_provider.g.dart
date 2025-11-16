// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_list_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(ProjectList)
const projectListProvider = ProjectListProvider._();

final class ProjectListProvider
    extends $AsyncNotifierProvider<ProjectList, PagedResultDto<ProjectDto>> {
  const ProjectListProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'projectListProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$projectListHash();

  @$internal
  @override
  ProjectList create() => ProjectList();
}

String _$projectListHash() => r'58223f1c28849042db756dfa419202aac09e8438';

abstract class _$ProjectList
    extends $AsyncNotifier<PagedResultDto<ProjectDto>> {
  FutureOr<PagedResultDto<ProjectDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref
            as $Ref<
              AsyncValue<PagedResultDto<ProjectDto>>,
              PagedResultDto<ProjectDto>
            >;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<PagedResultDto<ProjectDto>>,
                PagedResultDto<ProjectDto>
              >,
              AsyncValue<PagedResultDto<ProjectDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}
