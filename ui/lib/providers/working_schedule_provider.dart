import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/user_working_schedule_dto.dart';
import '../data/mock/mock_working_schedule_service.dart';

part 'working_schedule_provider.g.dart';

@riverpod
class UserWorkingSchedule extends _$UserWorkingSchedule {
  String _userId = 'user-1';

  @override
  Future<List<UserWorkingScheduleDto>> build() async {
    return _loadSchedule();
  }

  Future<List<UserWorkingScheduleDto>> _loadSchedule() async {
    return await MockWorkingScheduleService.getByUser(_userId);
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadSchedule());
  }

  Future<void> applyDefault() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(
      () => MockWorkingScheduleService.applyDefaultSchedule(_userId),
    );
  }
}

@riverpod
Future<UserWorkingScheduleDto?> workingScheduleByDay(Ref ref,
  String userId,
  DayOfWeek dayOfWeek,
) async {
  return await MockWorkingScheduleService.getByUserAndDay(
    userId: userId,
    dayOfWeek: dayOfWeek,
  );
}

@riverpod
Future<void> createOrUpdateWorkingSchedule(Ref ref,
  CreateUpdateWorkingScheduleDto input,
) async {
  await MockWorkingScheduleService.createOrUpdate(input);
  ref.invalidate(userWorkingScheduleProvider);
  ref.invalidate(workingScheduleByDayProvider);
}

@riverpod
Future<void> deleteWorkingSchedule(Ref ref, String id) async {
  await MockWorkingScheduleService.delete(id);
  ref.invalidate(userWorkingScheduleProvider);
}
