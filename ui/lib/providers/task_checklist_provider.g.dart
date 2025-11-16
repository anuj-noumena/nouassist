// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_checklist_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskChecklists)
const taskChecklistsProvider = TaskChecklistsFamily._();

final class TaskChecklistsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskChecklistDto>>,
          List<TaskChecklistDto>,
          FutureOr<List<TaskChecklistDto>>
        >
    with
        $FutureModifier<List<TaskChecklistDto>>,
        $FutureProvider<List<TaskChecklistDto>> {
  const TaskChecklistsProvider._({
    required TaskChecklistsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskChecklistsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskChecklistsHash();

  @override
  String toString() {
    return r'taskChecklistsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskChecklistDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskChecklistDto>> create(Ref ref) {
    final argument = this.argument as String;
    return taskChecklists(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskChecklistsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskChecklistsHash() => r'27b0356c9e3295766e615277e27dad8c44e59dcd';

final class TaskChecklistsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<TaskChecklistDto>>, String> {
  const TaskChecklistsFamily._()
    : super(
        retry: null,
        name: r'taskChecklistsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskChecklistsProvider call(String taskId) =>
      TaskChecklistsProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskChecklistsProvider';
}

@ProviderFor(TaskChecklist)
const taskChecklistProvider = TaskChecklistFamily._();

final class TaskChecklistProvider
    extends $AsyncNotifierProvider<TaskChecklist, TaskChecklistDto> {
  const TaskChecklistProvider._({
    required TaskChecklistFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskChecklistProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskChecklistHash();

  @override
  String toString() {
    return r'taskChecklistProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  TaskChecklist create() => TaskChecklist();

  @override
  bool operator ==(Object other) {
    return other is TaskChecklistProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskChecklistHash() => r'ae83b6a9de2f758f5486de93ff50d4b368cef1f2';

final class TaskChecklistFamily extends $Family
    with
        $ClassFamilyOverride<
          TaskChecklist,
          AsyncValue<TaskChecklistDto>,
          TaskChecklistDto,
          FutureOr<TaskChecklistDto>,
          String
        > {
  const TaskChecklistFamily._()
    : super(
        retry: null,
        name: r'taskChecklistProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskChecklistProvider call(String id) =>
      TaskChecklistProvider._(argument: id, from: this);

  @override
  String toString() => r'taskChecklistProvider';
}

abstract class _$TaskChecklist extends $AsyncNotifier<TaskChecklistDto> {
  late final _$args = ref.$arg as String;
  String get id => _$args;

  FutureOr<TaskChecklistDto> build(String id);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref =
        this.ref as $Ref<AsyncValue<TaskChecklistDto>, TaskChecklistDto>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<TaskChecklistDto>, TaskChecklistDto>,
              AsyncValue<TaskChecklistDto>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(createChecklist)
const createChecklistProvider = CreateChecklistFamily._();

final class CreateChecklistProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateChecklistProvider._({
    required CreateChecklistFamily super.from,
    required CreateUpdateChecklistDto super.argument,
  }) : super(
         retry: null,
         name: r'createChecklistProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createChecklistHash();

  @override
  String toString() {
    return r'createChecklistProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateChecklistDto;
    return createChecklist(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateChecklistProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createChecklistHash() => r'225853d756355d6f3153e0f09dcbec95f875d356';

final class CreateChecklistFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateChecklistDto> {
  const CreateChecklistFamily._()
    : super(
        retry: null,
        name: r'createChecklistProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateChecklistProvider call(CreateUpdateChecklistDto input) =>
      CreateChecklistProvider._(argument: input, from: this);

  @override
  String toString() => r'createChecklistProvider';
}

@ProviderFor(deleteChecklist)
const deleteChecklistProvider = DeleteChecklistFamily._();

final class DeleteChecklistProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteChecklistProvider._({
    required DeleteChecklistFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteChecklistProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteChecklistHash();

  @override
  String toString() {
    return r'deleteChecklistProvider'
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
    return deleteChecklist(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteChecklistProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteChecklistHash() => r'd4ab6455449723d5cabcd06168c946a5adc92cc9';

final class DeleteChecklistFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteChecklistFamily._()
    : super(
        retry: null,
        name: r'deleteChecklistProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteChecklistProvider call(String id, String taskId) =>
      DeleteChecklistProvider._(argument: (id, taskId), from: this);

  @override
  String toString() => r'deleteChecklistProvider';
}
