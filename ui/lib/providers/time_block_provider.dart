import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/time_block_dto.dart';
import '../data/mock/mock_time_block_service.dart';

part 'time_block_provider.g.dart';

@riverpod
class TimeBlocksByDateRange extends _$TimeBlocksByDateRange {
  String _userId = 'user-1';
  DateTime _startDate = DateTime.now().subtract(const Duration(days: 7));
  DateTime _endDate = DateTime.now().add(const Duration(days: 7));
  TimeBlockType? _typeFilter;

  @override
  Future<List<TimeBlockDto>> build() async {
    return _loadTimeBlocks();
  }

  Future<List<TimeBlockDto>> _loadTimeBlocks() async {
    return await MockTimeBlockService.getByUserAndDateRange(
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
    state = await AsyncValue.guard(() => _loadTimeBlocks());
  }

  Future<void> setTypeFilter(TimeBlockType? type) async {
    _typeFilter = type;
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadTimeBlocks());
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadTimeBlocks());
  }
}

@riverpod
Future<List<TimeBlockDto>> timeBlocksByDate(Ref ref,
  String userId,
  DateTime date,
) async {
  return await MockTimeBlockService.getByDate(
    userId: userId,
    date: date,
  );
}

@riverpod
Future<TimeBlockDto> timeBlock(Ref ref, String id) async {
  return await MockTimeBlockService.get(id);
}

@riverpod
Future<void> createTimeBlock(Ref ref,
  CreateUpdateTimeBlockDto input,
) async {
  await MockTimeBlockService.create(input);
  ref.invalidate(timeBlocksByDateRangeProvider);
  ref.invalidate(timeBlocksByDateProvider);
}

@riverpod
Future<void> updateTimeBlock(Ref ref,
  String id,
  CreateUpdateTimeBlockDto input,
) async {
  await MockTimeBlockService.update(id, input);
  ref.invalidate(timeBlocksByDateRangeProvider);
  ref.invalidate(timeBlocksByDateProvider);
  ref.invalidate(timeBlockProvider(id));
}

@riverpod
Future<void> deleteTimeBlock(Ref ref, String id) async {
  await MockTimeBlockService.delete(id);
  ref.invalidate(timeBlocksByDateRangeProvider);
  ref.invalidate(timeBlocksByDateProvider);
}

@riverpod
Future<void> updateTimeBlockStatus(Ref ref,
  String id,
  TimeBlockStatus status,
) async {
  await MockTimeBlockService.updateStatus(id, status);
  ref.invalidate(timeBlocksByDateRangeProvider);
  ref.invalidate(timeBlocksByDateProvider);
  ref.invalidate(timeBlockProvider(id));
}
