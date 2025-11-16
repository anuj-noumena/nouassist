import 'package:riverpod_annotation/riverpod_annotation.dart';
import '../data/models/gtd_review_dto.dart';
import '../data/mock/mock_gtd_review_service.dart';

part 'gtd_review_provider.g.dart';

@riverpod
Future<List<GtdReviewDto>> reviewList(Ref ref,
  String userId, {
  ReviewType? reviewType,
  bool? isCompleted,
}) async {
  return await MockGtdReviewService.getByUser(
    userId,
    reviewType: reviewType,
    isCompleted: isCompleted,
  );
}

@riverpod
Future<GtdReviewDto> review(Ref ref, String id) async {
  return await MockGtdReviewService.get(id);
}

@riverpod
Future<GtdReviewDto> createReview(Ref ref, {
  required String userId,
  required ReviewType reviewType,
}) async {
  final review = await MockGtdReviewService.create(
    userId: userId,
    reviewType: reviewType,
  );
  ref.invalidate(reviewListProvider);
  return review;
}

@riverpod
Future<GtdReviewDto> updateReviewChecklist(Ref ref,
  String id,
  List<ReviewChecklistItemDto> checklist,
) async {
  final review = await MockGtdReviewService.updateChecklist(id, checklist);
  ref.invalidate(reviewProvider(id));
  return review;
}

@riverpod
Future<GtdReviewDto> completeReview(Ref ref,
  String id,
  String? notes,
) async {
  final review = await MockGtdReviewService.complete(id, notes);
  ref.invalidate(reviewListProvider);
  ref.invalidate(reviewProvider(id));
  return review;
}
