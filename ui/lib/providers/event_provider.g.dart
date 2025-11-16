// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'event_provider.dart';

// **************************************************************************
// RiverpodGenerator
// **************************************************************************

// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint, type=warning

@ProviderFor(EventsByDateRange)
const eventsByDateRangeProvider = EventsByDateRangeProvider._();

final class EventsByDateRangeProvider
    extends $AsyncNotifierProvider<EventsByDateRange, List<EventDto>> {
  const EventsByDateRangeProvider._()
    : super(
        from: null,
        argument: null,
        retry: null,
        name: r'eventsByDateRangeProvider',
        isAutoDispose: true,
        dependencies: null,
        $allTransitiveDependencies: null,
      );

  @override
  String debugGetCreateSourceHash() => _$eventsByDateRangeHash();

  @$internal
  @override
  EventsByDateRange create() => EventsByDateRange();
}

String _$eventsByDateRangeHash() => r'cca4480ba7e91b36fc968cd4eb97000a0658c1c2';

abstract class _$EventsByDateRange extends $AsyncNotifier<List<EventDto>> {
  FutureOr<List<EventDto>> build();
  @$mustCallSuper
  @override
  void runBuild() {
    final created = build();
    final ref = this.ref as $Ref<AsyncValue<List<EventDto>>, List<EventDto>>;
    final element =
        ref.element
            as $ClassProviderElement<
              AnyNotifier<AsyncValue<List<EventDto>>, List<EventDto>>,
              AsyncValue<List<EventDto>>,
              Object?,
              Object?
            >;
    element.handleValue(ref, created);
  }
}

@ProviderFor(eventsByDate)
const eventsByDateProvider = EventsByDateFamily._();

final class EventsByDateProvider
    extends
        $FunctionalProvider<
          AsyncValue<List<EventDto>>,
          List<EventDto>,
          FutureOr<List<EventDto>>
        >
    with $FutureModifier<List<EventDto>>, $FutureProvider<List<EventDto>> {
  const EventsByDateProvider._({
    required EventsByDateFamily super.from,
    required (String, DateTime) super.argument,
  }) : super(
         retry: null,
         name: r'eventsByDateProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$eventsByDateHash();

  @override
  String toString() {
    return r'eventsByDateProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<List<EventDto>> $createElement(
    $ProviderPointer pointer,
  ) => $FutureProviderElement(pointer);

  @override
  FutureOr<List<EventDto>> create(Ref ref) {
    final argument = this.argument as (String, DateTime);
    return eventsByDate(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is EventsByDateProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$eventsByDateHash() => r'167a9ff3a7d695e1910a63c7e7615504acb3ad46';

final class EventsByDateFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<List<EventDto>>,
          (String, DateTime)
        > {
  const EventsByDateFamily._()
    : super(
        retry: null,
        name: r'eventsByDateProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  EventsByDateProvider call(String userId, DateTime date) =>
      EventsByDateProvider._(argument: (userId, date), from: this);

  @override
  String toString() => r'eventsByDateProvider';
}

@ProviderFor(event)
const eventProvider = EventFamily._();

final class EventProvider
    extends
        $FunctionalProvider<AsyncValue<EventDto>, EventDto, FutureOr<EventDto>>
    with $FutureModifier<EventDto>, $FutureProvider<EventDto> {
  const EventProvider._({
    required EventFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'eventProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$eventHash();

  @override
  String toString() {
    return r'eventProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<EventDto> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<EventDto> create(Ref ref) {
    final argument = this.argument as String;
    return event(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is EventProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$eventHash() => r'9bb0bece9b28228ac15c4000e46d416b9ebbba35';

final class EventFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<EventDto>, String> {
  const EventFamily._()
    : super(
        retry: null,
        name: r'eventProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  EventProvider call(String id) => EventProvider._(argument: id, from: this);

  @override
  String toString() => r'eventProvider';
}

@ProviderFor(createEvent)
const createEventProvider = CreateEventFamily._();

final class CreateEventProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const CreateEventProvider._({
    required CreateEventFamily super.from,
    required CreateUpdateEventDto super.argument,
  }) : super(
         retry: null,
         name: r'createEventProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$createEventHash();

  @override
  String toString() {
    return r'createEventProvider'
        ''
        '($argument)';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as CreateUpdateEventDto;
    return createEvent(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is CreateEventProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$createEventHash() => r'0c2aa2230d276717127005f60be5f85ce264e864';

final class CreateEventFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, CreateUpdateEventDto> {
  const CreateEventFamily._()
    : super(
        retry: null,
        name: r'createEventProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  CreateEventProvider call(CreateUpdateEventDto input) =>
      CreateEventProvider._(argument: input, from: this);

  @override
  String toString() => r'createEventProvider';
}

@ProviderFor(updateEvent)
const updateEventProvider = UpdateEventFamily._();

final class UpdateEventProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const UpdateEventProvider._({
    required UpdateEventFamily super.from,
    required (String, CreateUpdateEventDto) super.argument,
  }) : super(
         retry: null,
         name: r'updateEventProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$updateEventHash();

  @override
  String toString() {
    return r'updateEventProvider'
        ''
        '$argument';
  }

  @$internal
  @override
  $FutureProviderElement<void> $createElement($ProviderPointer pointer) =>
      $FutureProviderElement(pointer);

  @override
  FutureOr<void> create(Ref ref) {
    final argument = this.argument as (String, CreateUpdateEventDto);
    return updateEvent(ref, argument.$1, argument.$2);
  }

  @override
  bool operator ==(Object other) {
    return other is UpdateEventProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$updateEventHash() => r'5e75a34325fafd2895f06abf70b03adce93e3060';

final class UpdateEventFamily extends $Family
    with
        $FunctionalFamilyOverride<
          FutureOr<void>,
          (String, CreateUpdateEventDto)
        > {
  const UpdateEventFamily._()
    : super(
        retry: null,
        name: r'updateEventProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  UpdateEventProvider call(String id, CreateUpdateEventDto input) =>
      UpdateEventProvider._(argument: (id, input), from: this);

  @override
  String toString() => r'updateEventProvider';
}

@ProviderFor(deleteEvent)
const deleteEventProvider = DeleteEventFamily._();

final class DeleteEventProvider
    extends $FunctionalProvider<AsyncValue<void>, void, FutureOr<void>>
    with $FutureModifier<void>, $FutureProvider<void> {
  const DeleteEventProvider._({
    required DeleteEventFamily super.from,
    required String super.argument,
  }) : super(
         retry: null,
         name: r'deleteEventProvider',
         isAutoDispose: true,
         dependencies: null,
         $allTransitiveDependencies: null,
       );

  @override
  String debugGetCreateSourceHash() => _$deleteEventHash();

  @override
  String toString() {
    return r'deleteEventProvider'
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
    return deleteEvent(ref, argument);
  }

  @override
  bool operator ==(Object other) {
    return other is DeleteEventProvider && other.argument == argument;
  }

  @override
  int get hashCode {
    return argument.hashCode;
  }
}

String _$deleteEventHash() => r'5ddfc9ca6a806dcf3aed2031f6a647b3428385f7';

final class DeleteEventFamily extends $Family
    with $FunctionalFamilyOverride<FutureOr<void>, String> {
  const DeleteEventFamily._()
    : super(
        retry: null,
        name: r'deleteEventProvider',
        dependencies: null,
        $allTransitiveDependencies: null,
        isAutoDispose: true,
      );

  DeleteEventProvider call(String id) =>
      DeleteEventProvider._(argument: id, from: this);

  @override
  String toString() => r'deleteEventProvider';
}
