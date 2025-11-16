// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_inbox_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(InboxList)
const inboxListProvider = InboxListFamily._();

final class InboxListProvider
    extends $AsyncNotifierProvider<InboxList, List<GtdInboxDto>> {
  const InboxListProvider._({
    required InboxListFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'inboxListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$inboxListHash();

  @override
  String toString() {
    return r'inboxListProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  InboxList create() => InboxList();

  @override
  bool operator ==(Object other) {
    return other is InboxListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$inboxListHash() => r'1cb2fd39dbad05e1eaa55410b02a02c2294477c0';

final class InboxListFamily extends $Family
    with
        $ClassFamilyOverride<
          InboxList,
          AsyncValue<List<GtdInboxDto>>,
          List<GtdInboxDto>,
          FutureOr<List<GtdInboxDto>>,
          String
        > {
  const InboxListFamily._()
    : super(
        retry: null,
        name: r'inboxListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  InboxListProvider call(String userId) =>
      InboxListProvider._(argument: userId, from: this);

  @override
  String toString() => r'inboxListProvider';
}

abstract class _$InboxList extends $AsyncNotifier<List<GtdInboxDto>> {
  late final _$args = ref.$arg as String;
  String get userId => _$args;

  FutureOr<List<GtdInboxDto>> build(String userId);
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build(_$args);
    final ref =
        this.ref as $Ref<AsyncValue<List<GtdInboxDto>>, List<GtdInboxDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<List<GtdInboxDto>>, List<GtdInboxDto>>,
              AsyncValue<List<GtdInboxDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(inboxItem)
const inboxItemProvider = InboxItemFamily._();

final class InboxItemProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdInboxDto>,
          GtdInboxDto,
          FutureOr<GtdInboxDto>
        >
    with $FutureModifier<GtdInboxDto>, $FutureProvider<GtdInboxDto> {
  const InboxItemProvider._({
    required InboxItemFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'inboxItemProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$inboxItemHash();

  @override
  String toString() {
    return r'inboxItemProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdInboxDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdInboxDto> create(Ref ref) {
    final argument = this.argument as String;
    return inboxItem(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is InboxItemProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$inboxItemHash() => r'625e58b16c7dfdf839f23bfb5061f315671b20d3';

final class InboxItemFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdInboxDto>, String> {
  const InboxItemFamily._()
    : super(
        retry: null,
        name: r'inboxItemProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  InboxItemProvider call(String id) =>
      InboxItemProvider._(argument: id, from: this);

  @override
  String toString() => r'inboxItemProvider';
}

@ProviderFor(createInboxItem)
const createInboxItemProvider = CreateInboxItemFamily._();

final class CreateInboxItemProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdInboxDto>,
          GtdInboxDto,
          FutureOr<GtdInboxDto>
        >
    with $FutureModifier<GtdInboxDto>, $FutureProvider<GtdInboxDto> {
  const CreateInboxItemProvider._({
    required CreateInboxItemFamily super.from,
    required CreateUpdateInboxDto super.argument,
  }) : super(
         retry: null,
         name: r'createInboxItemProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createInboxItemHash();

  @override
  String toString() {
    return r'createInboxItemProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdInboxDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdInboxDto> create(Ref ref) {
    final argument = this.argument as CreateUpdateInboxDto;
    return createInboxItem(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateInboxItemProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createInboxItemHash() => r'f0ef0b7636f63c97b5793824b900ac63a09c5b28';

final class CreateInboxItemFamily extends $Family
    with
        $FunctionalFamilyOverride<FutureOr<GtdInboxDto>, CreateUpdateInboxDto> {
  const CreateInboxItemFamily._()
    : super(
        retry: null,
        name: r'createInboxItemProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateInboxItemProvider call(CreateUpdateInboxDto input) =>
      CreateInboxItemProvider._(argument: input, from: this);

  @override
  String toString() => r'createInboxItemProvider';
}

@ProviderFor(markInboxItemProcessed)
const markInboxItemProcessedProvider = MarkInboxItemProcessedFamily._();

final class MarkInboxItemProcessedProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdInboxDto>,
          GtdInboxDto,
          FutureOr<GtdInboxDto>
        >
    with $FutureModifier<GtdInboxDto>, $FutureProvider<GtdInboxDto> {
  const MarkInboxItemProcessedProvider._({
    required MarkInboxItemProcessedFamily super.from,
    required (String, String) super.argument,
  }) : super(
         retry: null,
         name: r'markInboxItemProcessedProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$markInboxItemProcessedHash();

  @override
  String toString() {
    return r'markInboxItemProcessedProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<GtdInboxDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdInboxDto> create(Ref ref) {
    final argument = this.argument as (String, String);
    return markInboxItemProcessed(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is MarkInboxItemProcessedProvider &&
        other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$markInboxItemProcessedHash() =>
    r'761e8267525dda8ce0a1c0f181bc6237efdfe1bb';

final class MarkInboxItemProcessedFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdInboxDto>, (String, String)> {
  const MarkInboxItemProcessedFamily._()
    : super(
        retry: null,
        name: r'markInboxItemProcessedProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  MarkInboxItemProcessedProvider call(String id, String processedAs) =>
      MarkInboxItemProcessedProvider._(argument: (id, processedAs), from: this);

  @override
  String toString() => r'markInboxItemProcessedProvider';
}

@ProviderFor(deleteInboxItem)
const deleteInboxItemProvider = DeleteInboxItemFamily._();

final class DeleteInboxItemProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteInboxItemProvider._({
    required DeleteInboxItemFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteInboxItemProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteInboxItemHash();

  @override
  String toString() {
    return r'deleteInboxItemProvider'
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
    return deleteInboxItem(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteInboxItemProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteInboxItemHash() => r'18ecc0f758cd2e12dfb6d219caee09579fc369d7';

final class DeleteInboxItemFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteInboxItemFamily._()
    : super(
        retry: null,
        name: r'deleteInboxItemProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteInboxItemProvider call(String id) =>
      DeleteInboxItemProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteInboxItemProvider';
}
