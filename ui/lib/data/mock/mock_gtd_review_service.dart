import '../models/gtd_review_dto.dart';

/// Mock GTD Review service
class MockGtdReviewService {
  static final List<GtdReviewDto> _reviews = _generateMockReviews();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get reviews
  static Future<List<GtdReviewDto>> getByUser(
    String userId, {
    ReviewType? reviewType,
    bool? isCompleted,
  }) async {
    await _simulateDelay();

    var filtered = _reviews.where((review) => review.userId == userId);

    if (reviewType != null) {
      filtered = filtered.where((review) => review.reviewType == reviewType);
    }

    if (isCompleted != null) {
      filtered = filtered.where((review) => review.isCompleted == isCompleted);
    }

    return filtered.toList()
      ..sort((a, b) => b.reviewDate.compareTo(a.reviewDate));
  }

  /// Get single review
  static Future<GtdReviewDto> get(String id) async {
    await _simulateDelay();

    try {
      return _reviews.firstWhere((review) => review.id == id);
    } catch (e) {
      throw Exception('Review not found');
    }
  }

  /// Create review
  static Future<GtdReviewDto> create({
    required String userId,
    required ReviewType reviewType,
  }) async {
    await _simulateDelay();

    final checklist = _getChecklistForReviewType(reviewType);

    final newReview = GtdReviewDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: userId,
      reviewType: reviewType,
      reviewDate: DateTime.now(),
      checklist: checklist,
      metrics: {},
      creationTime: DateTime.now(),
    );

    _reviews.insert(0, newReview);
    return newReview;
  }

  /// Update review
  static Future<GtdReviewDto> updateChecklist(
    String id,
    List<ReviewChecklistItemDto> checklist,
  ) async {
    await _simulateDelay();

    final index = _reviews.indexWhere((review) => review.id == id);
    if (index == -1) {
      throw Exception('Review not found');
    }

    final allCompleted = checklist.every((item) => item.isCompleted);

    final updatedReview = _reviews[index].copyWith(
      checklist: checklist,
      isCompleted: allCompleted,
      completedDate: allCompleted ? DateTime.now() : null,
    );

    _reviews[index] = updatedReview;
    return updatedReview;
  }

  /// Complete review
  static Future<GtdReviewDto> complete(String id, String? notes) async {
    await _simulateDelay();

    final index = _reviews.indexWhere((review) => review.id == id);
    if (index == -1) {
      throw Exception('Review not found');
    }

    final updatedReview = _reviews[index].copyWith(
      isCompleted: true,
      completedDate: DateTime.now(),
      notes: notes,
    );

    _reviews[index] = updatedReview;
    return updatedReview;
  }

  /// Get checklist for review type
  static List<ReviewChecklistItemDto> _getChecklistForReviewType(
    ReviewType type,
  ) {
    switch (type) {
      case ReviewType.daily:
        return [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Review calendar for today',
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Check next actions list',
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Process inbox to zero',
            order: 3,
          ),
          const ReviewChecklistItemDto(
            id: '4',
            description: 'Review waiting for items',
            order: 4,
          ),
        ];

      case ReviewType.weekly:
        return [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Clear your head - brain dump',
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Process inbox to zero',
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Review next actions lists',
            order: 3,
          ),
          const ReviewChecklistItemDto(
            id: '4',
            description: 'Review waiting for list',
            order: 4,
          ),
          const ReviewChecklistItemDto(
            id: '5',
            description: 'Review projects and outcomes',
            order: 5,
          ),
          const ReviewChecklistItemDto(
            id: '6',
            description: 'Review someday/maybe list',
            order: 6,
          ),
          const ReviewChecklistItemDto(
            id: '7',
            description: 'Review calendar for past and future',
            order: 7,
          ),
        ];

      case ReviewType.monthly:
        return [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Review goals and objectives',
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Review areas of focus',
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Update someday/maybe list',
            order: 3,
          ),
          const ReviewChecklistItemDto(
            id: '4',
            description: 'Archive completed projects',
            order: 4,
          ),
        ];

      case ReviewType.quarterly:
        return [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Review vision and long-term goals',
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Assess progress on key projects',
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Update areas of responsibility',
            order: 3,
          ),
        ];

      case ReviewType.yearly:
        return [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Review life vision and purpose',
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Set goals for next year',
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Review all areas of focus',
            order: 3,
          ),
        ];
    }
  }

  /// Generate mock reviews
  static List<GtdReviewDto> _generateMockReviews() {
    final now = DateTime.now();
    return [
      GtdReviewDto(
        id: 'review-1',
        userId: 'user-1',
        reviewType: ReviewType.weekly,
        reviewDate: now.subtract(const Duration(days: 7)),
        isCompleted: true,
        completedDate: now.subtract(const Duration(days: 7)),
        checklist: [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Clear your head - brain dump',
            isCompleted: true,
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Process inbox to zero',
            isCompleted: true,
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Review next actions lists',
            isCompleted: true,
            order: 3,
          ),
        ],
        notes: 'Good week overall. Need to focus more on project X.',
        metrics: {
          'inboxProcessed': 15,
          'actionsCompleted': 23,
          'projectsReviewed': 5,
        },
        creationTime: now.subtract(const Duration(days: 7)),
      ),
      GtdReviewDto(
        id: 'review-2',
        userId: 'user-1',
        reviewType: ReviewType.daily,
        reviewDate: now,
        isCompleted: false,
        checklist: [
          const ReviewChecklistItemDto(
            id: '1',
            description: 'Review calendar for today',
            isCompleted: true,
            order: 1,
          ),
          const ReviewChecklistItemDto(
            id: '2',
            description: 'Check next actions list',
            isCompleted: false,
            order: 2,
          ),
          const ReviewChecklistItemDto(
            id: '3',
            description: 'Process inbox to zero',
            isCompleted: false,
            order: 3,
          ),
        ],
        metrics: {},
        creationTime: now,
      ),
    ];
  }
}
