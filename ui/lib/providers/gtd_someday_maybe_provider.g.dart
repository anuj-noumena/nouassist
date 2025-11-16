// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_someday_maybe_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(SomedayMaybeList)
const somedayMaybeListProvider = SomedayMaybeListFamily._();

final class SomedayMaybeListProvider
    extends $AsyncNotifierProvider<SomedayMaybeList, List<GtdSomedayMaybeDto>> {
  const SomedayMaybeListProvider._({
    required SomedayMaybeListFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'somedayMaybeListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$somedayMaybeListHash();

  @override
  String toString() {
    return r'somedayMaybeListProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  SomedayMaybeList create() => SomedayMaybeList();

  @override
  bool operator ==(Object other) {
    return other is SomedayMaybeListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$somedayMaybeListHash() => r'53ce9b7eaa9f7e39bb3dbc1dd40cd843ebfd8d20';

final class SomedayMaybeListFamily extends $Family
    with
        $ClassFamilyOverride<
          SomedayMaybeList,
          AsyncValue<List<GtdSomedayMaybeDto>>,
          List<GtdSomedayMaybeDto>,
          FutureOr<List<GtdSomedayMaybeDto>>,
          String
        > {
  const SomedayMaybeListFamily._()
    : super(
        retry: null,
        name: r'somedayMaybeListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  SomedayMaybeListProvider call(String userId) =>
      SomedayMaybeListProvider._(argument: userId, from: this);

  @override
  String toString() => r'somedayMaybeListProvider';
}

abstract class _$SomedayMaybeList
    extends $AsyncNotifier<List<GtdSomedayMaybeDto>> {
  late final _$args = ref.$arg as String;
  String get userId => _$args;

  FutureOr<List<GtdSomedayMaybeDto>> build(String userId);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref =
        this.ref
            as $Ref<
              AsyncValue<List<GtdSomedayMaybeDto>>,
              List<GtdSomedayMaybeDto>
            >;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<List<GtdSomedayMaybeDto>>,
                List<GtdSomedayMaybeDto>
              >,
              AsyncValue<List<GtdSomedayMaybeDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(somedayMaybeItem)
const somedayMaybeItemProvider = SomedayMaybeItemFamily._();

final class SomedayMaybeItemProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdSomedayMaybeDto>,
          GtdSomedayMaybeDto,
          FutureOr<GtdSomedayMaybeDto>
        >
    with
        $FutureModifier<GtdSomedayMaybeDto>,
        $FutureProvider<GtdSomedayMaybeDto> {
  const SomedayMaybeItemProvider._({
    required SomedayMaybeItemFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'somedayMaybeItemProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$somedayMaybeItemHash();

  @override
  String toString() {
    return r'somedayMaybeItemProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdSomedayMaybeDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdSomedayMaybeDto> create(Ref ref) {
    final argument = this.argument as String;
    return somedayMaybeItem(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is SomedayMaybeItemProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$somedayMaybeItemHash() => r'1c77fbd0ce12c6fbd1f2b91f3b883e212ff138e2';

final class SomedayMaybeItemFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdSomedayMaybeDto>, String> {
  const SomedayMaybeItemFamily._()
    : super(
        retry: null,
        name: r'somedayMaybeItemProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  SomedayMaybeItemProvider call(String id) =>
      SomedayMaybeItemProvider._(argument: id, from: this);

  @override
  String toString() => r'somedayMaybeItemProvider';
}

@ProviderFor(createSomedayMaybe)
const createSomedayMaybeProvider = CreateSomedayMaybeFamily._();

final class CreateSomedayMaybeProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdSomedayMaybeDto>,
          GtdSomedayMaybeDto,
          FutureOr<GtdSomedayMaybeDto>
        >
    with
        $FutureModifier<GtdSomedayMaybeDto>,
        $FutureProvider<GtdSomedayMaybeDto> {
  const CreateSomedayMaybeProvider._({
    required CreateSomedayMaybeFamily super.from,
    required CreateUpdateSomedayMaybeDto super.argument,
  }) : super(
         retry: null,
         name: r'createSomedayMaybeProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createSomedayMaybeHash();

  @override
  String toString() {
    return r'createSomedayMaybeProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdSomedayMaybeDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdSomedayMaybeDto> create(Ref ref) {
    final argument = this.argument as CreateUpdateSomedayMaybeDto;
    return createSomedayMaybe(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateSomedayMaybeProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createSomedayMaybeHash() =>
    r'b82a1f0d61155e8364478af27e3685d43efca414';

final class CreateSomedayMaybeFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdSomedayMaybeDto>,
          CreateUpdateSomedayMaybeDto
        > {
  const CreateSomedayMaybeFamily._()
    : super(
        retry: null,
        name: r'createSomedayMaybeProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateSomedayMaybeProvider call(CreateUpdateSomedayMaybeDto input) =>
      CreateSomedayMaybeProvider._(argument: input, from: this);

  @override
  String toString() => r'createSomedayMaybeProvider';
}

@ProviderFor(activateSomedayMaybe)
const activateSomedayMaybeProvider = ActivateSomedayMaybeFamily._();

final class ActivateSomedayMaybeProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdSomedayMaybeDto>,
          GtdSomedayMaybeDto,
          FutureOr<GtdSomedayMaybeDto>
        >
    with
        $FutureModifier<GtdSomedayMaybeDto>,
        $FutureProvider<GtdSomedayMaybeDto> {
  const ActivateSomedayMaybeProvider._({
    required ActivateSomedayMaybeFamily super.from,
    required (String, String, String) super.argument,
  }) : super(
         retry: null,
         name: r'activateSomedayMaybeProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$activateSomedayMaybeHash();

  @override
  String toString() {
    return r'activateSomedayMaybeProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdSomedayMaybeDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdSomedayMaybeDto> create(Ref ref) {
    final argument = this.argument as (String, String, String);
    return activateSomedayMaybe(ref, argument.$1, argument.$2, argument.$3);
  }

  @override
  bool operator ==(Object other) {
    return other is ActivateSomedayMaybeProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$activateSomedayMaybeHash() =>
    r'c61c744bdd02562c71b6a3199c30bc6e0584fd7b';

final class ActivateSomedayMaybeFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdSomedayMaybeDto>,
          (String, String, String)
        > {
  const ActivateSomedayMaybeFamily._()
    : super(
        retry: null,
        name: r'activateSomedayMaybeProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ActivateSomedayMaybeProvider call(
    String id,
    String projectId,
    String userId,
  ) => ActivateSomedayMaybeProvider._(
    argument: (id, projectId, userId),
    from: this,
  );

  @override
  String toString() => r'activateSomedayMaybeProvider';
}

@ProviderFor(deleteSomedayMaybe)
const deleteSomedayMaybeProvider = DeleteSomedayMaybeFamily._();

final class DeleteSomedayMaybeProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteSomedayMaybeProvider._({
    required DeleteSomedayMaybeFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteSomedayMaybeProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteSomedayMaybeHash();

  @override
  String toString() {
    return r'deleteSomedayMaybeProvider'
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
    return deleteSomedayMaybe(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteSomedayMaybeProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteSomedayMaybeHash() =>
    r'65d34a6abe71caa85093c8975e18d4b329b34bae';

final class DeleteSomedayMaybeFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteSomedayMaybeFamily._()
    : super(
        retry: null,
        name: r'deleteSomedayMaybeProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteSomedayMaybeProvider call(String id, String userId) =>
      DeleteSomedayMaybeProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'deleteSomedayMaybeProvider';
}
