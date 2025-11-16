import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_context_dto.dart';
import '../data/mock/mock_gtd_context_service.dart';

part 'gtd_context_provider.g.dart';

@riverpod
Future<List<GtdContextDto>> contextList(Ref ref,
  String userId,
) async {
  return await MockGtdContextService.getByUser(userId);
}

@riverpod
Future<GtdContextDto> context(Ref ref, String id) async {
  return await MockGtdContextService.get(id);
}

@riverpod
Future<GtdContextDto> createContext(Ref ref,
  CreateUpdateContextDto input,
) async {
  final context = await MockGtdContextService.create(input);
  ref.invalidate(contextListProvider);
  return context;
}

@riverpod
Future<void> deleteContext(Ref ref, String id) async {
  await MockGtdContextService.delete(id);
  ref.invalidate(contextListProvider);
}
