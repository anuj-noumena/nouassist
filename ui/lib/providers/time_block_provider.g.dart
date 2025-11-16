// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'time_block_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(TimeBlocksByDateRange)
const timeBlocksByDateRangeProvider = TimeBlocksByDateRangeProvider._();

final class TimeBlocksByDateRangeProvider
    extends $AsyncNotifierProvider<TimeBlocksByDateRange, List<TimeBlockDto>> {
  const TimeBlocksByDateRangeProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'timeBlocksByDateRangeProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$timeBlocksByDateRangeHash();

  @$internal
  @override
  TimeBlocksByDateRange create() => TimeBlocksByDateRange();
}

String _$timeBlocksByDateRangeHash() =>
    r'4534a4d0399d6dec8614c739844f14dee60f99e2';

abstract class _$TimeBlocksByDateRange
    extends $AsyncNotifier<List<TimeBlockDto>> {
  FutureOr<List<TimeBlockDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref =
        this.ref as $Ref<AsyncValue<List<TimeBlockDto>>, List<TimeBlockDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<List<TimeBlockDto>>, List<TimeBlockDto>>,
              AsyncValue<List<TimeBlockDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(timeBlocksByDate)
const timeBlocksByDateProvider = TimeBlocksByDateFamily._();

final class TimeBlocksByDateProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<TimeBlockDto>>,
          List<TimeBlockDto>,
          FutureOr<List<TimeBlockDto>>
        >
    with
        $FutureModifier<List<TimeBlockDto>>,
        $FutureProvider<List<TimeBlockDto>> {
  const TimeBlocksByDateProvider._({
    required TimeBlocksByDateFamily super.from,
    required (String, DateTime) super.argument,
  }) : super(
         retry: null,
         name: r'timeBlocksByDateProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$timeBlocksByDateHash();

  @override
  String toString() {
    return r'timeBlocksByDateProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<List<TimeBlockDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<TimeBlockDto>> create(Ref ref) {
    final argument = this.argument as (String, DateTime);
    return timeBlocksByDate(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is TimeBlocksByDateProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$timeBlocksByDateHash() => r'db791dff9440f56830fcfe47ef5e118b93368f94';

final class TimeBlocksByDateFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<List<TimeBlockDto>>,
          (String, DateTime)
        > {
  const TimeBlocksByDateFamily._()
    : super(
        retry: null,
        name: r'timeBlocksByDateProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TimeBlocksByDateProvider call(String userId, DateTime date) =>
      TimeBlocksByDateProvider._(argument: (userId, date), from: this);

  @override
  String toString() => r'timeBlocksByDateProvider';
}

@ProviderFor(timeBlock)
const timeBlockProvider = TimeBlockFamily._();

final class TimeBlockProvider
    extends
        $FunctionalProvider<
          AsyncValue<TimeBlockDto>,
          TimeBlockDto,
          FutureOr<TimeBlockDto>
        >
    with $FutureModifier<TimeBlockDto>, $FutureProvider<TimeBlockDto> {
  const TimeBlockProvider._({
    required TimeBlockFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'timeBlockProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$timeBlockHash();

  @override
  String toString() {
    return r'timeBlockProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<TimeBlockDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<TimeBlockDto> create(Ref ref) {
    final argument = this.argument as String;
    return timeBlock(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is TimeBlockProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$timeBlockHash() => r'5134bfc83400ea44e2dd34d5af901c24978273f5';

final class TimeBlockFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<TimeBlockDto>, String> {
  const TimeBlockFamily._()
    : super(
        retry: null,
        name: r'timeBlockProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  TimeBlockProvider call(String id) =>
      TimeBlockProvider._(argument: id, from: this);

  @override
  String toString() => r'timeBlockProvider';
}

@ProviderFor(createTimeBlock)
const createTimeBlockProvider = CreateTimeBlockFamily._();

final class CreateTimeBlockProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateTimeBlockProvider._({
    required CreateTimeBlockFamily super.from,
    required CreateUpdateTimeBlockDto super.argument,
  }) : super(
         retry: null,
         name: r'createTimeBlockProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createTimeBlockHash();

  @override
  String toString() {
    return r'createTimeBlockProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateTimeBlockDto;
    return createTimeBlock(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateTimeBlockProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createTimeBlockHash() => r'e5b054a697fd43ebc0abd4f3e2b10cc75dab9692';

final class CreateTimeBlockFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateTimeBlockDto> {
  const CreateTimeBlockFamily._()
    : super(
        retry: null,
        name: r'createTimeBlockProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateTimeBlockProvider call(CreateUpdateTimeBlockDto input) =>
      CreateTimeBlockProvider._(argument: input, from: this);

  @override
  String toString() => r'createTimeBlockProvider';
}

@ProviderFor(updateTimeBlock)
const updateTimeBlockProvider = UpdateTimeBlockFamily._();

final class UpdateTimeBlockProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const UpdateTimeBlockProvider._({
    required UpdateTimeBlockFamily super.from,
    required (String, CreateUpdateTimeBlockDto) super.argument,
  }) : super(
         retry: null,
         name: r'updateTimeBlockProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$updateTimeBlockHash();

  @override
  String toString() {
    return r'updateTimeBlockProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, CreateUpdateTimeBlockDto);
    return updateTimeBlock(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is UpdateTimeBlockProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$updateTimeBlockHash() => r'96be5aba1a8d984e29c32d87c2a4e6dd8954c13c';

final class UpdateTimeBlockFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<void>,
          (String, CreateUpdateTimeBlockDto)
        > {
  const UpdateTimeBlockFamily._()
    : super(
        retry: null,
        name: r'updateTimeBlockProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UpdateTimeBlockProvider call(String id, CreateUpdateTimeBlockDto input) =>
      UpdateTimeBlockProvider._(argument: (id, input), from: this);

  @override
  String toString() => r'updateTimeBlockProvider';
}

@ProviderFor(deleteTimeBlock)
const deleteTimeBlockProvider = DeleteTimeBlockFamily._();

final class DeleteTimeBlockProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteTimeBlockProvider._({
    required DeleteTimeBlockFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteTimeBlockProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteTimeBlockHash();

  @override
  String toString() {
    return r'deleteTimeBlockProvider'
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
    return deleteTimeBlock(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteTimeBlockProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteTimeBlockHash() => r'd90c79c0cf8ec740d6f2e2d6f6e346896efdd992';

final class DeleteTimeBlockFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteTimeBlockFamily._()
    : super(
        retry: null,
        name: r'deleteTimeBlockProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteTimeBlockProvider call(String id) =>
      DeleteTimeBlockProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteTimeBlockProvider';
}

@ProviderFor(updateTimeBlockStatus)
const updateTimeBlockStatusProvider = UpdateTimeBlockStatusFamily._();

final class UpdateTimeBlockStatusProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const UpdateTimeBlockStatusProvider._({
    required UpdateTimeBlockStatusFamily super.from,
    required (String, TimeBlockStatus) super.argument,
  }) : super(
         retry: null,
         name: r'updateTimeBlockStatusProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$updateTimeBlockStatusHash();

  @override
  String toString() {
    return r'updateTimeBlockStatusProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, TimeBlockStatus);
    return updateTimeBlockStatus(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is UpdateTimeBlockStatusProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$updateTimeBlockStatusHash() =>
    r'a6d787071dab82a245d91bc352e4a31ab348a05d';

final class UpdateTimeBlockStatusFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, TimeBlockStatus)> {
  const UpdateTimeBlockStatusFamily._()
    : super(
        retry: null,
        name: r'updateTimeBlockStatusProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UpdateTimeBlockStatusProvider call(String id, TimeBlockStatus status) =>
      UpdateTimeBlockStatusProvider._(argument: (id, status), from: this);

  @override
  String toString() => r'updateTimeBlockStatusProvider';
}
