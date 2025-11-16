// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(Task)
const taskProvider = TaskFamily._();

final class TaskProvider extends $AsyncNotifierProvider<Task, TaskDto> {
  const TaskProvider._({
    required TaskFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskHash();

  @override
  String toString() {
    return r'taskProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  Task create() => Task();

  @override
  bool operator ==(Object other) {
    return other is TaskProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskHash() => r'4042ddb0920d75d63469a9375455288d21dff0c8';

final class TaskFamily extends $Family
    with
        $ClassFamilyOverride<
          Task,
          AsyncValue<TaskDto>,
          TaskDto,
          FutureOr<TaskDto>,
          String
        > {
  const TaskFamily._()
    : super(
        retry: null,
        name: r'taskProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskProvider call(String id) => TaskProvider._(argument: id, from: this);

  @override
  String toString() => r'taskProvider';
}

abstract class _$Task extends $AsyncNotifier<TaskDto> {
  late final _$args = ref.$arg as String;
  String get id => _$args;

  FutureOr<TaskDto> build(String id);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref = this.ref as $Ref<AsyncValue<TaskDto>, TaskDto>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<TaskDto>, TaskDto>,
              AsyncValue<TaskDto>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}
