// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_delegation_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskDelegations)
const taskDelegationsProvider = TaskDelegationsFamily._();

final class TaskDelegationsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskDelegationDto>>,
          List<TaskDelegationDto>,
          FutureOr<List<TaskDelegationDto>>
        >
    with
        $FutureModifier<List<TaskDelegationDto>>,
        $FutureProvider<List<TaskDelegationDto>> {
  const TaskDelegationsProvider._({
    required TaskDelegationsFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'taskDelegationsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$taskDelegationsHash();

  @override
  String toString() {
    return r'taskDelegationsProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskDelegationDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskDelegationDto>> create(Ref ref) {
    final argument = this.argument as String;
    return taskDelegations(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TaskDelegationsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$taskDelegationsHash() => r'ed7370d8c6a68730d5bef6e819c871c7fe788ea5';

final class TaskDelegationsFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<TaskDelegationDto>>, String> {
  const TaskDelegationsFamily._()
    : super(
        retry: null,
        name: r'taskDelegationsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TaskDelegationsProvider call(String taskId) =>
      TaskDelegationsProvider._(argument: taskId, from: this);

  @override
  String toString() => r'taskDelegationsProvider';
}

@ProviderFor(userDelegations)
const userDelegationsProvider = UserDelegationsFamily._();

final class UserDelegationsProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TaskDelegationDto>>,
          List<TaskDelegationDto>,
          FutureOr<List<TaskDelegationDto>>
        >
    with
        $FutureModifier<List<TaskDelegationDto>>,
        $FutureProvider<List<TaskDelegationDto>> {
  const UserDelegationsProvider._({
    required UserDelegationsFamily super.from,
    required (String, {bool? delegatedTo}) super.argument,
  }) : super(
         retry: null,
         name: r'userDelegationsProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$userDelegationsHash();

  @override
  String toString() {
    return r'userDelegationsProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<List<TaskDelegationDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TaskDelegationDto>> create(Ref ref) {
    final argument = this.argument as (String, {bool? delegatedTo});
    return userDelegations(ref, argument.$1, delegatedTo: argument.delegatedTo);
  }

  @override
  bool operator ==(Object other) {
    return other is UserDelegationsProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$userDelegationsHash() => r'37d37e94d79eda0c23ffeae9f9df5ebb51312f93';

final class UserDelegationsFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<List<TaskDelegationDto>>,
          (String, {bool? delegatedTo})
        > {
  const UserDelegationsFamily._()
    : super(
        retry: null,
        name: r'userDelegationsProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UserDelegationsProvider call(String userId, {bool? delegatedTo}) =>
      UserDelegationsProvider._(
        argument: (userId, delegatedTo: delegatedTo),
        from: this,
      );

  @override
  String toString() => r'userDelegationsProvider';
}

@ProviderFor(createDelegation)
const createDelegationProvider = CreateDelegationFamily._();

final class CreateDelegationProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateDelegationProvider._({
    required CreateDelegationFamily super.from,
    required CreateDelegationDto super.argument,
  }) : super(
         retry: null,
         name: r'createDelegationProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createDelegationHash();

  @override
  String toString() {
    return r'createDelegationProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateDelegationDto;
    return createDelegation(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateDelegationProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createDelegationHash() => r'2c61bde28e5009446e544328a81ae1e5e6a63482';

final class CreateDelegationFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateDelegationDto> {
  const CreateDelegationFamily._()
    : super(
        retry: null,
        name: r'createDelegationProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateDelegationProvider call(CreateDelegationDto input) =>
      CreateDelegationProvider._(argument: input, from: this);

  @override
  String toString() => r'createDelegationProvider';
}

@ProviderFor(acceptDelegation)
const acceptDelegationProvider = AcceptDelegationFamily._();

final class AcceptDelegationProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const AcceptDelegationProvider._({
    required AcceptDelegationFamily super.from,
    required (String, String, String) super.argument,
  }) : super(
         retry: null,
         name: r'acceptDelegationProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$acceptDelegationHash();

  @override
  String toString() {
    return r'acceptDelegationProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, String, String);
    return acceptDelegation(ref, argument.$1, argument.$2, argument.$3);
  }

  @override
  bool operator ==(Object other) {
    return other is AcceptDelegationProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$acceptDelegationHash() => r'8bbd8022de20b56b8648b46d6ca75012eaf0a349';

final class AcceptDelegationFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String, String)> {
  const AcceptDelegationFamily._()
    : super(
        retry: null,
        name: r'acceptDelegationProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  AcceptDelegationProvider call(String id, String taskId, String comment) =>
      AcceptDelegationProvider._(argument: (id, taskId, comment), from: this);

  @override
  String toString() => r'acceptDelegationProvider';
}

@ProviderFor(rejectDelegation)
const rejectDelegationProvider = RejectDelegationFamily._();

final class RejectDelegationProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const RejectDelegationProvider._({
    required RejectDelegationFamily super.from,
    required (String, String, String) super.argument,
  }) : super(
         retry: null,
         name: r'rejectDelegationProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$rejectDelegationHash();

  @override
  String toString() {
    return r'rejectDelegationProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, String, String);
    return rejectDelegation(ref, argument.$1, argument.$2, argument.$3);
  }

  @override
  bool operator ==(Object other) {
    return other is RejectDelegationProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$rejectDelegationHash() => r'408ac8858d1a654e741631bc3a5dc92024edad24';

final class RejectDelegationFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String, String)> {
  const RejectDelegationFamily._()
    : super(
        retry: null,
        name: r'rejectDelegationProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  RejectDelegationProvider call(String id, String taskId, String comment) =>
      RejectDelegationProvider._(argument: (id, taskId, comment), from: this);

  @override
  String toString() => r'rejectDelegationProvider';
}
