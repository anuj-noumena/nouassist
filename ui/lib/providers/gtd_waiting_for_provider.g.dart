// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_waiting_for_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(WaitingForList)
const waitingForListProvider = WaitingForListFamily._();

final class WaitingForListProvider
    extends $AsyncNotifierProvider<WaitingForList, List<GtdWaitingForDto>> {
  const WaitingForListProvider._({
    required WaitingForListFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'waitingForListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$waitingForListHash();

  @override
  String toString() {
    return r'waitingForListProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  WaitingForList create() => WaitingForList();

  @override
  bool operator ==(Object other) {
    return other is WaitingForListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$waitingForListHash() => r'7ea5933cdd5ba48c8c2578263e1216b43de70d01';

final class WaitingForListFamily extends $Family
    with
        $ClassFamilyOverride<
          WaitingForList,
          AsyncValue<List<GtdWaitingForDto>>,
          List<GtdWaitingForDto>,
          FutureOr<List<GtdWaitingForDto>>,
          String
        > {
  const WaitingForListFamily._()
    : super(
        retry: null,
        name: r'waitingForListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  WaitingForListProvider call(String userId) =>
      WaitingForListProvider._(argument: userId, from: this);

  @override
  String toString() => r'waitingForListProvider';
}

abstract class _$WaitingForList extends $AsyncNotifier<List<GtdWaitingForDto>> {
  late final _$args = ref.$arg as String;
  String get userId => _$args;

  FutureOr<List<GtdWaitingForDto>> build(String userId);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref =
        this.ref
            as $Ref<AsyncValue<List<GtdWaitingForDto>>, List<GtdWaitingForDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<List<GtdWaitingForDto>>,
                List<GtdWaitingForDto>
              >,
              AsyncValue<List<GtdWaitingForDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(waitingForItem)
const waitingForItemProvider = WaitingForItemFamily._();

final class WaitingForItemProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdWaitingForDto>,
          GtdWaitingForDto,
          FutureOr<GtdWaitingForDto>
        >
    with $FutureModifier<GtdWaitingForDto>, $FutureProvider<GtdWaitingForDto> {
  const WaitingForItemProvider._({
    required WaitingForItemFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'waitingForItemProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$waitingForItemHash();

  @override
  String toString() {
    return r'waitingForItemProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdWaitingForDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdWaitingForDto> create(Ref ref) {
    final argument = this.argument as String;
    return waitingForItem(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is WaitingForItemProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$waitingForItemHash() => r'fdabe03a19df37c901dcb80f51b7571ddda618e1';

final class WaitingForItemFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdWaitingForDto>, String> {
  const WaitingForItemFamily._()
    : super(
        retry: null,
        name: r'waitingForItemProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  WaitingForItemProvider call(String id) =>
      WaitingForItemProvider._(argument: id, from: this);

  @override
  String toString() => r'waitingForItemProvider';
}

@ProviderFor(createWaitingFor)
const createWaitingForProvider = CreateWaitingForFamily._();

final class CreateWaitingForProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdWaitingForDto>,
          GtdWaitingForDto,
          FutureOr<GtdWaitingForDto>
        >
    with $FutureModifier<GtdWaitingForDto>, $FutureProvider<GtdWaitingForDto> {
  const CreateWaitingForProvider._({
    required CreateWaitingForFamily super.from,
    required CreateUpdateWaitingForDto super.argument,
  }) : super(
         retry: null,
         name: r'createWaitingForProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createWaitingForHash();

  @override
  String toString() {
    return r'createWaitingForProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdWaitingForDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdWaitingForDto> create(Ref ref) {
    final argument = this.argument as CreateUpdateWaitingForDto;
    return createWaitingFor(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateWaitingForProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createWaitingForHash() => r'07a31cdccfbf0be0441183ae1f916e5226e71dd2';

final class CreateWaitingForFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdWaitingForDto>,
          CreateUpdateWaitingForDto
        > {
  const CreateWaitingForFamily._()
    : super(
        retry: null,
        name: r'createWaitingForProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateWaitingForProvider call(CreateUpdateWaitingForDto input) =>
      CreateWaitingForProvider._(argument: input, from: this);

  @override
  String toString() => r'createWaitingForProvider';
}

@ProviderFor(markWaitingForReceived)
const markWaitingForReceivedProvider = MarkWaitingForReceivedFamily._();

final class MarkWaitingForReceivedProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdWaitingForDto>,
          GtdWaitingForDto,
          FutureOr<GtdWaitingForDto>
        >
    with $FutureModifier<GtdWaitingForDto>, $FutureProvider<GtdWaitingForDto> {
  const MarkWaitingForReceivedProvider._({
    required MarkWaitingForReceivedFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'markWaitingForReceivedProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$markWaitingForReceivedHash();

  @override
  String toString() {
    return r'markWaitingForReceivedProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdWaitingForDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdWaitingForDto> create(Ref ref) {
    final argument = this.argument as (String, String);
    return markWaitingForReceived(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is MarkWaitingForReceivedProvider &&
        other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$markWaitingForReceivedHash() =>
    r'54ce58eff136ae45ec14cf083fe1fb8c81ea6767';

final class MarkWaitingForReceivedFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdWaitingForDto>,
          (String, String)
        > {
  const MarkWaitingForReceivedFamily._()
    : super(
        retry: null,
        name: r'markWaitingForReceivedProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  MarkWaitingForReceivedProvider call(String id, String userId) =>
      MarkWaitingForReceivedProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'markWaitingForReceivedProvider';
}

@ProviderFor(deleteWaitingFor)
const deleteWaitingForProvider = DeleteWaitingForFamily._();

final class DeleteWaitingForProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteWaitingForProvider._({
    required DeleteWaitingForFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteWaitingForProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteWaitingForHash();

  @override
  String toString() {
    return r'deleteWaitingForProvider'
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
    return deleteWaitingFor(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteWaitingForProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteWaitingForHash() => r'ac69243ebd4bb91d9daf02f3cd68f370002c5f0c';

final class DeleteWaitingForFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteWaitingForFamily._()
    : super(
        retry: null,
        name: r'deleteWaitingForProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteWaitingForProvider call(String id, String userId) =>
      DeleteWaitingForProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'deleteWaitingForProvider';
}
