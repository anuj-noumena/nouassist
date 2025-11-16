// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_form_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(TaskForm)
const taskFormProvider = TaskFormFamily._();

final class TaskFormProvider
    extends $NotifierProvider<TaskForm, TaskFormState> {
  const TaskFormProvider._({
    required TaskFormFamily super.from,
    required String? super.argument,
  }) : super(
         retry: null,
         name: r'taskFormProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskFormHash();

  @override
  String toString() {
    return r'taskFormProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  TaskForm create() => TaskForm();

  /// {@macro riverpod.override_with_value}
  Override overrideWithValue(TaskFormState value) {
    return $ProviderOverride(
      origin: this,
      providerOverride: $SyncValueProvider<TaskFormState>(value),
    );
  }

  @override
  bool operator ==(Object other) {
    return other is TaskFormProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskFormHash() => r'cf73372b406b9bd92549aa8d5a239fed2d726ee1';

final class TaskFormFamily extends $Family
    with
        $ClassFamilyOverride<
          TaskForm,
          TaskFormState,
          TaskFormState,
          TaskFormState,
          String?
        > {
  const TaskFormFamily._()
    : super(
        retry: null,
        name: r'taskFormProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskFormProvider call({String? taskId}) =>
      TaskFormProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskFormProvider';
}

abstract class _$TaskForm extends $Notifier<TaskFormState> {
  late final _$args = ref.$arg as String?;
  String? get taskId => _$args;

  TaskFormState build({String? taskId});
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(taskId: _$args);
    final ref = this.ref as $Ref<TaskFormState, TaskFormState>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<TaskFormState, TaskFormState>,
              TaskFormState,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}
