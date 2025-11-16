import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_someday_maybe_dto.dart';
import '../data/mock/mock_gtd_someday_maybe_service.dart';

part 'gtd_someday_maybe_provider.g.dart';

@riverpod
class SomedayMaybeList extends _$SomedayMaybeList {
  SomedayMaybeCategory? _category;
  bool? _isActivated;

  @override
  Future<List<GtdSomedayMaybeDto>> build(String userId) async {
    return await _loadItems();
  }

  Future<List<GtdSomedayMaybeDto>> _loadItems() async {
    return await MockGtdSomedayMaybeService.getByUser(
      userId,
      category: _category,
      isActivated: _isActivated,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadItems());
  }

  Future<void> filterByCategory(SomedayMaybeCategory? category) async {
    _category = category;
    await refresh();
  }

  Future<void> filterByActivation(bool? isActivated) async {
    _isActivated = isActivated;
    await refresh();
  }
}

@riverpod
Future<GtdSomedayMaybeDto> somedayMaybeItem(Ref ref,
  String id,
) async {
  return await MockGtdSomedayMaybeService.get(id);
}

@riverpod
Future<GtdSomedayMaybeDto> createSomedayMaybe(Ref ref,
  CreateUpdateSomedayMaybeDto input,
) async {
  final item = await MockGtdSomedayMaybeService.create(input);
  ref.invalidate(somedayMaybeListProvider);
  return item;
}

@riverpod
Future<GtdSomedayMaybeDto> activateSomedayMaybe(Ref ref,
  String id,
  String projectId,
  String userId,
) async {
  final item = await MockGtdSomedayMaybeService.activate(id, projectId);
  ref.invalidate(somedayMaybeListProvider(userId));
  return item;
}

@riverpod
Future<void> deleteSomedayMaybe(Ref ref,
  String id,
  String userId,
) async {
  await MockGtdSomedayMaybeService.delete(id);
  ref.invalidate(somedayMaybeListProvider(userId));
}
