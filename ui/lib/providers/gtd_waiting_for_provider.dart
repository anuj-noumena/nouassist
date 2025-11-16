import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_waiting_for_dto.dart';
import '../data/mock/mock_gtd_waiting_for_service.dart';

part 'gtd_waiting_for_provider.g.dart';

@riverpod
class WaitingForList extends _$WaitingForList {
  WaitingForStatus? _status;

  @override
  Future<List<GtdWaitingForDto>> build(String userId) async {
    return await _loadItems();
  }

  Future<List<GtdWaitingForDto>> _loadItems() async {
    return await MockGtdWaitingForService.getByUser(
      userId,
      status: _status,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadItems());
  }

  Future<void> filterByStatus(WaitingForStatus? status) async {
    _status = status;
    await refresh();
  }
}

@riverpod
Future<GtdWaitingForDto> waitingForItem(Ref ref,
  String id,
) async {
  return await MockGtdWaitingForService.get(id);
}

@riverpod
Future<GtdWaitingForDto> createWaitingFor(Ref ref,
  CreateUpdateWaitingForDto input,
) async {
  final item = await MockGtdWaitingForService.create(input);
  ref.invalidate(waitingForListProvider);
  return item;
}

@riverpod
Future<GtdWaitingForDto> markWaitingForReceived(Ref ref,
  String id,
  String userId,
) async {
  final item = await MockGtdWaitingForService.markAsReceived(id);
  ref.invalidate(waitingForListProvider(userId));
  return item;
}

@riverpod
Future<void> deleteWaitingFor(Ref ref,
  String id,
  String userId,
) async {
  await MockGtdWaitingForService.delete(id);
  ref.invalidate(waitingForListProvider(userId));
}
