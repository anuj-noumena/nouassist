// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_dependency_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskDependencies)
const taskDependenciesProvider = TaskDependenciesFamily._();

final class TaskDependenciesProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskDependencyDto>>,
          List<TaskDependencyDto>,
          FutureOr<List<TaskDependencyDto>>
        >
    with
        $FutureModifier<List<TaskDependencyDto>>,
        $FutureProvider<List<TaskDependencyDto>> {
  const TaskDependenciesProvider._({
    required TaskDependenciesFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskDependenciesProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskDependenciesHash();

  @override
  String toString() {
    return r'taskDependenciesProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskDependencyDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskDependencyDto>> create(Ref ref) {
    final argument = this.argument as String;
    return taskDependencies(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskDependenciesProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskDependenciesHash() => r'b8ca46064dd2c7abf7e6a1a4fb2ef5ab6c1e791d';

final class TaskDependenciesFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<TaskDependencyDto>>, String> {
  const TaskDependenciesFamily._()
    : super(
        retry: null,
        name: r'taskDependenciesProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskDependenciesProvider call(String taskId) =>
      TaskDependenciesProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskDependenciesProvider';
}

@ProviderFor(taskPredecessors)
const taskPredecessorsProvider = TaskPredecessorsFamily._();

final class TaskPredecessorsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskDependencyDto>>,
          List<TaskDependencyDto>,
          FutureOr<List<TaskDependencyDto>>
        >
    with
        $FutureModifier<List<TaskDependencyDto>>,
        $FutureProvider<List<TaskDependencyDto>> {
  const TaskPredecessorsProvider._({
    required TaskPredecessorsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskPredecessorsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskPredecessorsHash();

  @override
  String toString() {
    return r'taskPredecessorsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskDependencyDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskDependencyDto>> create(Ref ref) {
    final argument = this.argument as String;
    return taskPredecessors(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskPredecessorsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskPredecessorsHash() => r'694433c4a382868bb0fadffbeb7dcda45a691081';

final class TaskPredecessorsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<TaskDependencyDto>>, String> {
  const TaskPredecessorsFamily._()
    : super(
        retry: null,
        name: r'taskPredecessorsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskPredecessorsProvider call(String taskId) =>
      TaskPredecessorsProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskPredecessorsProvider';
}

@ProviderFor(taskSuccessors)
const taskSuccessorsProvider = TaskSuccessorsFamily._();

final class TaskSuccessorsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskDependencyDto>>,
          List<TaskDependencyDto>,
          FutureOr<List<TaskDependencyDto>>
        >
    with
        $FutureModifier<List<TaskDependencyDto>>,
        $FutureProvider<List<TaskDependencyDto>> {
  const TaskSuccessorsProvider._({
    required TaskSuccessorsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskSuccessorsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskSuccessorsHash();

  @override
  String toString() {
    return r'taskSuccessorsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskDependencyDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskDependencyDto>> create(Ref ref) {
    final argument = this.argument as String;
    return taskSuccessors(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskSuccessorsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskSuccessorsHash() => r'9ea87ae3991c23c4bac7451dcd261ca0fcf68efd';

final class TaskSuccessorsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<TaskDependencyDto>>, String> {
  const TaskSuccessorsFamily._()
    : super(
        retry: null,
        name: r'taskSuccessorsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskSuccessorsProvider call(String taskId) =>
      TaskSuccessorsProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskSuccessorsProvider';
}

@ProviderFor(createDependency)
const createDependencyProvider = CreateDependencyFamily._();

final class CreateDependencyProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateDependencyProvider._({
    required CreateDependencyFamily super.from,
    required CreateDependencyDto super.argument,
  }) : super(
         retry: null,
         name: r'createDependencyProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createDependencyHash();

  @override
  String toString() {
    return r'createDependencyProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateDependencyDto;
    return createDependency(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateDependencyProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createDependencyHash() => r'c5ee3fb8cbf1ba2c024c6abf36d20ba8dc550ce2';

final class CreateDependencyFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateDependencyDto> {
  const CreateDependencyFamily._()
    : super(
        retry: null,
        name: r'createDependencyProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateDependencyProvider call(CreateDependencyDto input) =>
      CreateDependencyProvider._(argument: input, from: this);

  @override
  String toString() => r'createDependencyProvider';
}

@ProviderFor(deleteDependency)
const deleteDependencyProvider = DeleteDependencyFamily._();

final class DeleteDependencyProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteDependencyProvider._({
    required DeleteDependencyFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteDependencyProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteDependencyHash();

  @override
  String toString() {
    return r'deleteDependencyProvider'
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
    return deleteDependency(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteDependencyProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteDependencyHash() => r'91797ffacdfa3e2a63058a4971617b4e8d5276fa';

final class DeleteDependencyFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteDependencyFamily._()
    : super(
        retry: null,
        name: r'deleteDependencyProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteDependencyProvider call(String id, String taskId) =>
      DeleteDependencyProvider._(argument: (id, taskId), from: this);

  @override
  String toString() => r'deleteDependencyProvider';
}
