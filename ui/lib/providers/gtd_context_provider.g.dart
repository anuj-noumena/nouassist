// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'gtd_context_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(contextList)
const contextListProvider = ContextListFamily._();

final class ContextListProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<GtdContextDto>>,
          List<GtdContextDto>,
          FutureOr<List<GtdContextDto>>
        >
    with
        $FutureModifier<List<GtdContextDto>>,
        $FutureProvider<List<GtdContextDto>> {
  const ContextListProvider._({
    required ContextListFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'contextListProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$contextListHash();

  @override
  String toString() {
    return r'contextListProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<List<GtdContextDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<GtdContextDto>> create(Ref ref) {
    final argument = this.argument as String;
    return contextList(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is ContextListProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$contextListHash() => r'afc88c1b56f6f7cfd53c00d35ec1fecf22ef810d';

final class ContextListFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<List<GtdContextDto>>, String> {
  const ContextListFamily._()
    : super(
        retry: null,
        name: r'contextListProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ContextListProvider call(String userId) =>
      ContextListProvider._(argument: userId, from: this);

  @override
  String toString() => r'contextListProvider';
}

@ProviderFor(context)
const contextProvider = ContextFamily._();

final class ContextProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdContextDto>,
          GtdContextDto,
          FutureOr<GtdContextDto>
        >
    with $FutureModifier<GtdContextDto>, $FutureProvider<GtdContextDto> {
  const ContextProvider._({
    required ContextFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'contextProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$contextHash();

  @override
  String toString() {
    return r'contextProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdContextDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdContextDto> create(Ref ref) {
    final argument = this.argument as String;
    return context(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is ContextProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$contextHash() => r'41f6d3434b1611cfbffb5fbac5a35b4f55f7192e';

final class ContextFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<GtdContextDto>, String> {
  const ContextFamily._()
    : super(
        retry: null,
        name: r'contextProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  ContextProvider call(String id) =>
      ContextProvider._(argument: id, from: this);

  @override
  String toString() => r'contextProvider';
}

@ProviderFor(createContext)
const createContextProvider = CreateContextFamily._();

final class CreateContextProvider
    extends
        $FunctionalProvider<
          AsyncValue<GtdContextDto>,
          GtdContextDto,
          FutureOr<GtdContextDto>
        >
    with $FutureModifier<GtdContextDto>, $FutureProvider<GtdContextDto> {
  const CreateContextProvider._({
    required CreateContextFamily super.from,
    required CreateUpdateContextDto super.argument,
  }) : super(
         retry: null,
         name: r'createContextProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createContextHash();

  @override
  String toString() {
    return r'createContextProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<GtdContextDto> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<GtdContextDto> create(Ref ref) {
    final argument = this.argument as CreateUpdateContextDto;
    return createContext(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateContextProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createContextHash() => r'33b5646d24a45594f91ff8d0798a400afa15b36f';

final class CreateContextFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<GtdContextDto>,
          CreateUpdateContextDto
        > {
  const CreateContextFamily._()
    : super(
        retry: null,
        name: r'createContextProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateContextProvider call(CreateUpdateContextDto input) =>
      CreateContextProvider._(argument: input, from: this);

  @override
  String toString() => r'createContextProvider';
}

@ProviderFor(deleteContext)
const deleteContextProvider = DeleteContextFamily._();

final class DeleteContextProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteContextProvider._({
    required DeleteContextFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteContextProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteContextHash();

  @override
  String toString() {
    return r'deleteContextProvider'
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
    return deleteContext(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteContextProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteContextHash() => r'a61c9c1696cd9c1d83761dc0dda11d54a60ebfd6';

final class DeleteContextFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteContextFamily._()
    : super(
        retry: null,
        name: r'deleteContextProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteContextProvider call(String id) =>
      DeleteContextProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteContextProvider';
}
