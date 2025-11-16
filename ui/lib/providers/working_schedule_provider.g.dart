// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'working_schedule_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(UserWorkingSchedule)
const userWorkingScheduleProvider = UserWorkingScheduleProvider._();

final class UserWorkingScheduleProvider
    extends
        $AsyncNotifierProvider<
          UserWorkingSchedule,
          List<UserWorkingScheduleDto>
        > {
  const UserWorkingScheduleProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'userWorkingScheduleProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$userWorkingScheduleHash();

  @$internal
  @override
  UserWorkingSchedule create() => UserWorkingSchedule();
}

String _$userWorkingScheduleHash() =>
    r'60a597d803ff08bf31deb2105022ee7ac1602654';

abstract class _$UserWorkingSchedule
    extends $AsyncNotifier<List<UserWorkingScheduleDto>> {
  FutureOr<List<UserWorkingScheduleDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref
            as $Ref<
              AsyncValue<List<UserWorkingScheduleDto>>,
              List<UserWorkingScheduleDto>
            >;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<List<UserWorkingScheduleDto>>,
                List<UserWorkingScheduleDto>
              >,
              AsyncValue<List<UserWorkingScheduleDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(workingScheduleByDay)
const workingScheduleByDayProvider = WorkingScheduleByDayFamily._();

final class WorkingScheduleByDayProvider
    extends
        $FunctionalProvider<
          AsyncValue<UserWorkingScheduleDto?>,
          UserWorkingScheduleDto?,
          FutureOr<UserWorkingScheduleDto?>
        >
    with
        $FutureModifier<UserWorkingScheduleDto?>,
        $FutureProvider<UserWorkingScheduleDto?> {
  const WorkingScheduleByDayProvider._({
    required WorkingScheduleByDayFamily super.from,
    required (String, DayOfWeek) super.argument,
  }) : super(
         retry: null,
         name: r'workingScheduleByDayProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$workingScheduleByDayHash();

  @override
  String toString() {
    return r'workingScheduleByDayProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<UserWorkingScheduleDto?> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<UserWorkingScheduleDto?> create(Ref ref) {
    final argument = this.argument as (String, DayOfWeek);
    return workingScheduleByDay(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is WorkingScheduleByDayProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$workingScheduleByDayHash() =>
    r'31da3c60b7d4b9de60007629c9b262e6414d7e39';

final class WorkingScheduleByDayFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<UserWorkingScheduleDto?>,
          (String, DayOfWeek)
        > {
  const WorkingScheduleByDayFamily._()
    : super(
        retry: null,
        name: r'workingScheduleByDayProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  WorkingScheduleByDayProvider call(String userId, DayOfWeek dayOfWeek) =>
      WorkingScheduleByDayProvider._(argument: (userId, dayOfWeek), from: this);

  @override
  String toString() => r'workingScheduleByDayProvider';
}

@ProviderFor(createOrUpdateWorkingSchedule)
const createOrUpdateWorkingScheduleProvider =
    CreateOrUpdateWorkingScheduleFamily._();

final class CreateOrUpdateWorkingScheduleProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateOrUpdateWorkingScheduleProvider._({
    required CreateOrUpdateWorkingScheduleFamily super.from,
    required CreateUpdateWorkingScheduleDto super.argument,
  }) : super(
         retry: null,
         name: r'createOrUpdateWorkingScheduleProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createOrUpdateWorkingScheduleHash();

  @override
  String toString() {
    return r'createOrUpdateWorkingScheduleProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateWorkingScheduleDto;
    return createOrUpdateWorkingSchedule(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateOrUpdateWorkingScheduleProvider &&
        other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createOrUpdateWorkingScheduleHash() =>
    r'9113ab1e0bd7d0bfbb40e7a3465a512e866421d3';

final class CreateOrUpdateWorkingScheduleFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<void>,
          CreateUpdateWorkingScheduleDto
        > {
  const CreateOrUpdateWorkingScheduleFamily._()
    : super(
        retry: null,
        name: r'createOrUpdateWorkingScheduleProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateOrUpdateWorkingScheduleProvider call(
    CreateUpdateWorkingScheduleDto input,
  ) => CreateOrUpdateWorkingScheduleProvider._(argument: input, from: this);

  @override
  String toString() => r'createOrUpdateWorkingScheduleProvider';
}

@ProviderFor(deleteWorkingSchedule)
const deleteWorkingScheduleProvider = DeleteWorkingScheduleFamily._();

final class DeleteWorkingScheduleProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteWorkingScheduleProvider._({
    required DeleteWorkingScheduleFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteWorkingScheduleProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteWorkingScheduleHash();

  @override
  String toString() {
    return r'deleteWorkingScheduleProvider'
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
    return deleteWorkingSchedule(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteWorkingScheduleProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteWorkingScheduleHash() =>
    r'78d4d3bbdbd050885e36f5f96f7dffd39466ae78';

final class DeleteWorkingScheduleFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteWorkingScheduleFamily._()
    : super(
        retry: null,
        name: r'deleteWorkingScheduleProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteWorkingScheduleProvider call(String id) =>
      DeleteWorkingScheduleProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteWorkingScheduleProvider';
}
