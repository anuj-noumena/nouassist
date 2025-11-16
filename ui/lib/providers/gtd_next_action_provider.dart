import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_next_action_dto.dart';
import '../data/mock/mock_gtd_next_action_service.dart';

part 'gtd_next_action_provider.g.dart';

@riverpod
class NextActionList extends _$NextActionList {
  List<String>? _contextIds;
  bool? _isCompleted;
  bool? _isStarred;

  @override
  Future<List<GtdNextActionDto>> build(String userId) async {
    return await _loadActions();
  }

  Future<List<GtdNextActionDto>> _loadActions() async {
    return await MockGtdNextActionService.getByUser(
      userId,
      contextIds: _contextIds,
      isCompleted: _isCompleted,
      isStarred: _isStarred,
    );
  }

  Future<void> refresh() async {
    state = const AsyncValue.loading();
    state = await AsyncValue.guard(() => _loadActions());
  }

  Future<void> filterByContexts(List<String>? contextIds) async {
    _contextIds = contextIds;
    await refresh();
  }

  Future<void> filterByCompletion(bool? isCompleted) async {
    _isCompleted = isCompleted;
    await refresh();
  }

  Future<void> filterByStarred(bool? isStarred) async {
    _isStarred = isStarred;
    await refresh();
  }
}

@riverpod
Future<GtdNextActionDto> nextAction(Ref ref, String id) async {
  return await MockGtdNextActionService.get(id);
}

@riverpod
Future<GtdNextActionDto> createNextAction(Ref ref,
  CreateUpdateNextActionDto input,
) async {
  final action = await MockGtdNextActionService.create(input);
  ref.invalidate(nextActionListProvider);
  return action;
}

@riverpod
Future<GtdNextActionDto> toggleActionCompletion(Ref ref,
  String id,
  String userId,
) async {
  final action = await MockGtdNextActionService.toggleCompletion(id);
  ref.invalidate(nextActionListProvider(userId));
  ref.invalidate(nextActionProvider(id));
  return action;
}

@riverpod
Future<GtdNextActionDto> toggleActionStar(Ref ref,
  String id,
  String userId,
) async {
  final action = await MockGtdNextActionService.toggleStar(id);
  ref.invalidate(nextActionListProvider(userId));
  ref.invalidate(nextActionProvider(id));
  return action;
}

@riverpod
Future<void> deleteNextAction(Ref ref,
  String id,
  String userId,
) async {
  await MockGtdNextActionService.delete(id);
  ref.invalidate(nextActionListProvider(userId));
}
