// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_next_action_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(NextActionList)
const nextActionListProvider = NextActionListFamily._();

final class NextActionListProvider
    extends $AsyncNotifierProvider<NextActionList, List<GtdNextActionDto>> {
  const NextActionListProvider._({
    required NextActionListFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'nextActionListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$nextActionListHash();

  @override
  String toString() {
    return r'nextActionListProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  NextActionList create() => NextActionList();

  @override
  bool operator ==(Object other) {
    return other is NextActionListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$nextActionListHash() => r'7cfa3739c3de40eacf2adc1d78b9d290e5aeb9e1';

final class NextActionListFamily extends $Family
    with
        $ClassFamilyOverride<
          NextActionList,
          AsyncValue<List<GtdNextActionDto>>,
          List<GtdNextActionDto>,
          FutureOr<List<GtdNextActionDto>>,
          String
        > {
  const NextActionListFamily._()
    : super(
        retry: null,
        name: r'nextActionListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  NextActionListProvider call(String userId) =>
      NextActionListProvider._(argument: userId, from: this);

  @override
  String toString() => r'nextActionListProvider';
}

abstract class _$NextActionList extends $AsyncNotifier<List<GtdNextActionDto>> {
  late final _$args = ref.$arg as String;
  String get userId => _$args;

  FutureOr<List<GtdNextActionDto>> build(String userId);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref =
        this.ref
            as $Ref<AsyncValue<List<GtdNextActionDto>>, List<GtdNextActionDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<
                AsyncValue<List<GtdNextActionDto>>,
                List<GtdNextActionDto>
              >,
              AsyncValue<List<GtdNextActionDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(nextAction)
const nextActionProvider = NextActionFamily._();

final class NextActionProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdNextActionDto>,
          GtdNextActionDto,
          FutureOr<GtdNextActionDto>
        >
    with $FutureModifier<GtdNextActionDto>, $FutureProvider<GtdNextActionDto> {
  const NextActionProvider._({
    required NextActionFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'nextActionProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$nextActionHash();

  @override
  String toString() {
    return r'nextActionProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdNextActionDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdNextActionDto> create(Ref ref) {
    final argument = this.argument as String;
    return nextAction(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is NextActionProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$nextActionHash() => r'b696fdd11ea17b7b09dfd8d9ba267fc0e46b352f';

final class NextActionFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdNextActionDto>, String> {
  const NextActionFamily._()
    : super(
        retry: null,
        name: r'nextActionProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  NextActionProvider call(String id) =>
      NextActionProvider._(argument: id, from: this);

  @override
  String toString() => r'nextActionProvider';
}

@ProviderFor(createNextAction)
const createNextActionProvider = CreateNextActionFamily._();

final class CreateNextActionProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdNextActionDto>,
          GtdNextActionDto,
          FutureOr<GtdNextActionDto>
        >
    with $FutureModifier<GtdNextActionDto>, $FutureProvider<GtdNextActionDto> {
  const CreateNextActionProvider._({
    required CreateNextActionFamily super.from,
    required CreateUpdateNextActionDto super.argument,
  }) : super(
         retry: null,
         name: r'createNextActionProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createNextActionHash();

  @override
  String toString() {
    return r'createNextActionProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdNextActionDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdNextActionDto> create(Ref ref) {
    final argument = this.argument as CreateUpdateNextActionDto;
    return createNextAction(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateNextActionProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createNextActionHash() => r'7ac8501e4cf1c1db76fb5fa5d4f4540d6739de0e';

final class CreateNextActionFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdNextActionDto>,
          CreateUpdateNextActionDto
        > {
  const CreateNextActionFamily._()
    : super(
        retry: null,
        name: r'createNextActionProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateNextActionProvider call(CreateUpdateNextActionDto input) =>
      CreateNextActionProvider._(argument: input, from: this);

  @override
  String toString() => r'createNextActionProvider';
}

@ProviderFor(toggleActionCompletion)
const toggleActionCompletionProvider = ToggleActionCompletionFamily._();

final class ToggleActionCompletionProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdNextActionDto>,
          GtdNextActionDto,
          FutureOr<GtdNextActionDto>
        >
    with $FutureModifier<GtdNextActionDto>, $FutureProvider<GtdNextActionDto> {
  const ToggleActionCompletionProvider._({
    required ToggleActionCompletionFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'toggleActionCompletionProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$toggleActionCompletionHash();

  @override
  String toString() {
    return r'toggleActionCompletionProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdNextActionDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdNextActionDto> create(Ref ref) {
    final argument = this.argument as (String, String);
    return toggleActionCompletion(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is ToggleActionCompletionProvider &&
        other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$toggleActionCompletionHash() =>
    r'e379c7c3c3bc0a21e0ad5d50d2ad363d919de409';

final class ToggleActionCompletionFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdNextActionDto>,
          (String, String)
        > {
  const ToggleActionCompletionFamily._()
    : super(
        retry: null,
        name: r'toggleActionCompletionProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ToggleActionCompletionProvider call(String id, String userId) =>
      ToggleActionCompletionProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'toggleActionCompletionProvider';
}

@ProviderFor(toggleActionStar)
const toggleActionStarProvider = ToggleActionStarFamily._();

final class ToggleActionStarProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdNextActionDto>,
          GtdNextActionDto,
          FutureOr<GtdNextActionDto>
        >
    with $FutureModifier<GtdNextActionDto>, $FutureProvider<GtdNextActionDto> {
  const ToggleActionStarProvider._({
    required ToggleActionStarFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'toggleActionStarProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$toggleActionStarHash();

  @override
  String toString() {
    return r'toggleActionStarProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdNextActionDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdNextActionDto> create(Ref ref) {
    final argument = this.argument as (String, String);
    return toggleActionStar(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is ToggleActionStarProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$toggleActionStarHash() => r'2cb49a00e988cdf552bb46a7dc9dbb010643f27c';

final class ToggleActionStarFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdNextActionDto>,
          (String, String)
        > {
  const ToggleActionStarFamily._()
    : super(
        retry: null,
        name: r'toggleActionStarProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ToggleActionStarProvider call(String id, String userId) =>
      ToggleActionStarProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'toggleActionStarProvider';
}

@ProviderFor(deleteNextAction)
const deleteNextActionProvider = DeleteNextActionFamily._();

final class DeleteNextActionProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteNextActionProvider._({
    required DeleteNextActionFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'deleteNextActionProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteNextActionHash();

  @override
  String toString() {
    return r'deleteNextActionProvider'
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
    return deleteNextAction(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteNextActionProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteNextActionHash() => r'412d0bccf7bef1a675f3398a37a4be5adec2c2aa';

final class DeleteNextActionFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, (String, String)> {
  const DeleteNextActionFamily._()
    : super(
        retry: null,
        name: r'deleteNextActionProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteNextActionProvider call(String id, String userId) =>
      DeleteNextActionProvider._(argument: (id, userId), from: this);

  @override
  String toString() => r'deleteNextActionProvider';
}
