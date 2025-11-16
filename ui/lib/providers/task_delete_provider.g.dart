// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_delete_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(deleteTask)
const deleteTaskProvider = DeleteTaskFamily._();

final class DeleteTaskProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteTaskProvider._({
    required DeleteTaskFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteTaskProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteTaskHash();

  @override
  String toString() {
    return r'deleteTaskProvider'
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
    return deleteTask(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteTaskProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteTaskHash() => r'f455c5f3c32c9bfc40bfddca121d255d5c94b296';

final class DeleteTaskFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteTaskFamily._()
    : super(
        retry: null,
        name: r'deleteTaskProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteTaskProvider call(String taskId) =>
      DeleteTaskProvider._(argument: taskId, from: this);

  @override
  String toString() => r'deleteTaskProvider';
}
