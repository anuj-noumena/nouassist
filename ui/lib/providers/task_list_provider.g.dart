// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_list_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(TaskList)
const taskListProvider = TaskListProvider._();

final class TaskListProvider
    extends $AsyncNotifierProvider<TaskList, PagedResultDto<TaskDto>> {
  const TaskListProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'taskListProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$taskListHash();

  @$internal
  @override
  TaskList create() => TaskList();
}

String _$taskListHash() => r'15c34aa216002e0beb851176c4d8dcae84e9a208';

abstract class _$TaskList extends $AsyncNotifier<PagedResultDto<TaskDto>> {
  FutureOr<PagedResultDto<TaskDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref
            as $Ref<
              AsyncValue<PagedResultDto<TaskDto>>,
              PagedResultDto<TaskDto>
            >;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<PagedResultDto<TaskDto>>,
                PagedResultDto<TaskDto>
              >,
              AsyncValue<PagedResultDto<TaskDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}
