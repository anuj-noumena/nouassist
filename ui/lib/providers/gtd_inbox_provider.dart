import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_inbox_dto.dart';
import '../data/mock/mock_gtd_inbox_service.dart';

part 'gtd_inbox_provider.g.dart';

@riverpod
class InboxList extends _$InboxList {
  @override
  Future<List<GtdInboxDto>> build(String userId) async {
    return await MockGtdInboxService.getByUser(userId);
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => MockGtdInboxService.getByUser(userId));
  }

  Future<void> filterByStatus(InboxStatus? status) async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(
      () => MockGtdInboxService.getByUser(userId, status: status),
    );
  }
}

@riverpod
Future<GtdInboxDto> inboxItem(Ref ref, String id) async {
  return await MockGtdInboxService.get(id);
}

@riverpod
Future<GtdInboxDto> createInboxItem(Ref ref,
  CreateUpdateInboxDto input,
) async {
  final item = await MockGtdInboxService.create(input);
  ref.invalidate(inboxListProvider);
  return item;
}

@riverpod
Future<GtdInboxDto> markInboxItemProcessed(Ref ref,
  String id,
  String processedAs,
) async {
  final item = await MockGtdInboxService.markAsProcessed(id, processedAs);
  ref.invalidate(inboxListProvider);
  return item;
}

@riverpod
Future<void> deleteInboxItem(Ref ref, String id) async {
  await MockGtdInboxService.delete(id);
  ref.invalidate(inboxListProvider);
}
