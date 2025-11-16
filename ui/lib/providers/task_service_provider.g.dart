// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'task_service_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(taskService)
const taskServiceProvider = TaskServiceProvider._();

final class TaskServiceProvider
    extends
        $FunctionalProvider<MockTaskService, MockTaskService, MockTaskService>
    with $Provider<MockTaskService> {
  const TaskServiceProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'taskServiceProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$taskServiceHash();

  @$internal
  @override
  $ProviderElement<MockTaskService> $createElement($ProviderPointer pointer) =>
      $ProviderElement(pointer);

  @override
  MockTaskService create(Ref ref) {
    return taskService(ref);
  }

  /// {@macro riverpod.override_with_value}
  Override overrideWithValue(MockTaskService value) {
    return $ProviderOverride(
      origin: this,
      providerOverride: $SyncValueProvider<MockTaskService>(value),
    );
  }
}

String _$taskServiceHash() => r'3cc1adb6d2ebb3b852e8f971ac2498e212eb52b5';
