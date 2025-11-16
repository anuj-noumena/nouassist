import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/event_dto.dart';
import '../data/mock/mock_event_service.dart';

part 'event_provider.g.dart';

@riverpod
class EventsByDateRange extends _$EventsByDateRange {
  String _userId = 'user-1';
  DateTime _startDate = DateTime.now().subtract(const Duration(days: 7));
  DateTime _endDate = DateTime.now().add(const Duration(days: 7));
  EventType? _typeFilter;

  @override
  Future<List<EventDto>> build() async {
    return _loadEvents();
  }

  Future<List<EventDto>> _loadEvents() async {
    return await MockEventService.getByUserAndDateRange(
      userId: _userId,
      startDate: _startDate,
      endDate: _endDate,
      typeFilter: _typeFilter,
    );
  }

  Future<void> setDateRange(DateTime start, DateTime end) async {
    _startDate = start;
    _endDate = end;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadEvents());
  }

  Future<void> setTypeFilter(EventType? type) async {
    _typeFilter = type;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadEvents());
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadEvents());
  }
}

@riverpod
Future<List<EventDto>> eventsByDate(Ref ref,
  String userId,
  DateTime date,
) async {
  return await MockEventService.getByDate(
    userId: userId,
    date: date,
  );
}

@riverpod
Future<EventDto> event(Ref ref, String id) async {
  return await MockEventService.get(id);
}

@riverpod
Future<void> createEvent(Ref ref,
  CreateUpdateEventDto input,
) async {
  await MockEventService.create(input);
  ref.invalidate(eventsByDateRangeProvider);
  ref.invalidate(eventsByDateProvider);
}

@riverpod
Future<void> updateEvent(Ref ref,
  String id,
  CreateUpdateEventDto input,
) async {
  await MockEventService.update(id, input);
  ref.invalidate(eventsByDateRangeProvider);
  ref.invalidate(eventsByDateProvider);
  ref.invalidate(eventProvider(id));
}

@riverpod
Future<void> deleteEvent(Ref ref, String id) async {
  await MockEventService.delete(id);
  ref.invalidate(eventsByDateRangeProvider);
  ref.invalidate(eventsByDateProvider);
}
