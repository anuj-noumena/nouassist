// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_tag_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(TaskTagList)
const taskTagListProvider = TaskTagListProvider._();

final class TaskTagListProvider
    extends $AsyncNotifierProvider<TaskTagList, List<TaskTagDto>> {
  const TaskTagListProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'taskTagListProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$taskTagListHash();

  @$internal
  @override
  TaskTagList create() => TaskTagList();
}

String _$taskTagListHash() => r'96282e1a1b7b02ed7eda8f7a657e4b1524d39bb0';

abstract class _$TaskTagList extends $AsyncNotifier<List<TaskTagDto>> {
  FutureOr<List<TaskTagDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref as $Ref<AsyncValue<List<TaskTagDto>>, List<TaskTagDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<List<TaskTagDto>>, List<TaskTagDto>>,
              AsyncValue<List<TaskTagDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(taskTag)
const taskTagProvider = TaskTagFamily._();

final class TaskTagProvider
    extends
        $FunctionalProvider<
          AsyncValue<TaskTagDto>,
          TaskTagDto,
          FutureOr<TaskTagDto>
        >
    with $FutureModifier<TaskTagDto>, $FutureProvider<TaskTagDto> {
  const TaskTagProvider._({
    required TaskTagFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskTagProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskTagHash();

  @override
  String toString() {
    return r'taskTagProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<TaskTagDto> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<TaskTagDto> create(Ref ref) {
    final argument = this.argument as String;
    return taskTag(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskTagProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskTagHash() => r'96892347d26a2606d836ec34442ea84f7a7f3bcf';

final class TaskTagFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<TaskTagDto>, String> {
  const TaskTagFamily._()
    : super(
        retry: null,
        name: r'taskTagProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskTagProvider call(String id) =>
      TaskTagProvider._(argument: id, from: this);

  @override
  String toString() => r'taskTagProvider';
}

@ProviderFor(createTaskTag)
const createTaskTagProvider = CreateTaskTagFamily._();

final class CreateTaskTagProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateTaskTagProvider._({
    required CreateTaskTagFamily super.from,
    required CreateUpdateTaskTagDto super.argument,
  }) : super(
         retry: null,
         name: r'createTaskTagProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createTaskTagHash();

  @override
  String toString() {
    return r'createTaskTagProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateTaskTagDto;
    return createTaskTag(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateTaskTagProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createTaskTagHash() => r'0972e7664e773a6851e2c1ed3d13fc39bcd2344a';

final class CreateTaskTagFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateTaskTagDto> {
  const CreateTaskTagFamily._()
    : super(
        retry: null,
        name: r'createTaskTagProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateTaskTagProvider call(CreateUpdateTaskTagDto input) =>
      CreateTaskTagProvider._(argument: input, from: this);

  @override
  String toString() => r'createTaskTagProvider';
}

@ProviderFor(deleteTaskTag)
const deleteTaskTagProvider = DeleteTaskTagFamily._();

final class DeleteTaskTagProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteTaskTagProvider._({
    required DeleteTaskTagFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteTaskTagProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteTaskTagHash();

  @override
  String toString() {
    return r'deleteTaskTagProvider'
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
    return deleteTaskTag(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteTaskTagProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteTaskTagHash() => r'00c9dffb2533267c05c18778a232a65c22a4af4d';

final class DeleteTaskTagFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteTaskTagFamily._()
    : super(
        retry: null,
        name: r'deleteTaskTagProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteTaskTagProvider call(String id) =>
      DeleteTaskTagProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteTaskTagProvider';
}
