// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'project_delete_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(deleteProject)
const deleteProjectProvider = DeleteProjectFamily._();

final class DeleteProjectProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteProjectProvider._({
    required DeleteProjectFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteProjectProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteProjectHash();

  @override
  String toString() {
    return r'deleteProjectProvider'
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
    return deleteProject(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteProjectProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteProjectHash() => r'b8bfddcc0a438f5ac03df86bd32b7221ce15532b';

final class DeleteProjectFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteProjectFamily._()
    : super(
        retry: null,
        name: r'deleteProjectProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteProjectProvider call(String projectId) =>
      DeleteProjectProvider._(argument: projectId, from: this);

  @override
  String toString() => r'deleteProjectProvider';
}
