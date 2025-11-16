// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_type_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskTypes)
const taskTypesProvider = TaskTypesProvider._();

final class TaskTypesProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskTypeDto>>,
          List<TaskTypeDto>,
          FutureOr<List<TaskTypeDto>>
        >
    with
        $FutureModifier<List<TaskTypeDto>>,
        $FutureProvider<List<TaskTypeDto>> {
  const TaskTypesProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'taskTypesProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$taskTypesHash();

  @$internal
  @override
  $FutureProviderElement<List<TaskTypeDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskTypeDto>> create(Ref ref) {
    return taskTypes(ref);
  }
}

String _$taskTypesHash() => r'ef78e7f554a8053b29e9f0fd0ae4d741fc9e7afa';

@ProviderFor(taskType)
const taskTypeProvider = TaskTypeFamily._();

final class TaskTypeProvider
    extends
        $FunctionalProvider<
          AsyncValue<TaskTypeDto>,
          TaskTypeDto,
          FutureOr<TaskTypeDto>
        >
    with $FutureModifier<TaskTypeDto>, $FutureProvider<TaskTypeDto> {
  const TaskTypeProvider._({
    required TaskTypeFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskTypeProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskTypeHash();

  @override
  String toString() {
    return r'taskTypeProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<TaskTypeDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<TaskTypeDto> create(Ref ref) {
    final argument = this.argument as String;
    return taskType(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskTypeProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskTypeHash() => r'3c6c39a3536f3d5845ca638d7a46ab3c07e35e02';

final class TaskTypeFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<TaskTypeDto>, String> {
  const TaskTypeFamily._()
    : super(
        retry: null,
        name: r'taskTypeProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskTypeProvider call(String id) =>
      TaskTypeProvider._(argument: id, from: this);

  @override
  String toString() => r'taskTypeProvider';
}
