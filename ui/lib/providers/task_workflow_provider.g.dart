// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_workflow_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskWorkflows)
const taskWorkflowsProvider = TaskWorkflowsProvider._();

final class TaskWorkflowsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskWorkflowDto>>,
          List<TaskWorkflowDto>,
          FutureOr<List<TaskWorkflowDto>>
        >
    with
        $FutureModifier<List<TaskWorkflowDto>>,
        $FutureProvider<List<TaskWorkflowDto>> {
  const TaskWorkflowsProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'taskWorkflowsProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$taskWorkflowsHash();

  @$internal
  @override
  $FutureProviderElement<List<TaskWorkflowDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskWorkflowDto>> create(Ref ref) {
    return taskWorkflows(ref);
  }
}

String _$taskWorkflowsHash() => r'8a2514aace5daff72c9200e5d6810b4ff04463a7';

@ProviderFor(taskWorkflow)
const taskWorkflowProvider = TaskWorkflowFamily._();

final class TaskWorkflowProvider
    extends
        $FunctionalProvider<
          AsyncValue<TaskWorkflowDto>,
          TaskWorkflowDto,
          FutureOr<TaskWorkflowDto>
        >
    with $FutureModifier<TaskWorkflowDto>, $FutureProvider<TaskWorkflowDto> {
  const TaskWorkflowProvider._({
    required TaskWorkflowFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskWorkflowProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskWorkflowHash();

  @override
  String toString() {
    return r'taskWorkflowProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<TaskWorkflowDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<TaskWorkflowDto> create(Ref ref) {
    final argument = this.argument as String;
    return taskWorkflow(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskWorkflowProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskWorkflowHash() => r'0dea82ba5fb6c94082d6f6153358bb3c24126b7d';

final class TaskWorkflowFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<TaskWorkflowDto>, String> {
  const TaskWorkflowFamily._()
    : super(
        retry: null,
        name: r'taskWorkflowProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskWorkflowProvider call(String id) =>
      TaskWorkflowProvider._(argument: id, from: this);

  @override
  String toString() => r'taskWorkflowProvider';
}
