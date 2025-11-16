import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_review_dto.dart';
import '../../../providers/gtd_review_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import 'widgets/review_checklist.dart';

class WeeklyReviewScreen extends ConsumerStatefulWidget {
  const WeeklyReviewScreen({super.key});

  @override
  ConsumerState<WeeklyReviewScreen> createState() => _WeeklyReviewScreenState();
}

class _WeeklyReviewScreenState extends ConsumerState<WeeklyReviewScreen> {
  @override
  Widget build(BuildContext context) {
    final reviewsAsync = ref.watch(
      reviewListProvider(
        'user-1',
        reviewType: ReviewType.weekly,
        isCompleted: false,
      ),
    );

    return Scaffold(
      appBar: AppBar(
        title: const Text('Weekly Review'),
      ),
      body: reviewsAsync.when(
        data: (reviews) {
          if (reviews.isEmpty) {
            return _buildEmptyState();
          }

          final currentReview = reviews.first;
          return _buildReviewContent(currentReview);
        },
        loading: () => const AppLoadingIndicator(),
        error: (error, _) => Center(child: Text('Error: $error')),
      ),
    );
  }

  Widget _buildEmptyState() {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(
            Icons.assignment_outlined,
            size: 80,
            color: AppColors.textDisabled,
          ),
          const SizedBox(height: AppSpacing.lg),
          AppText(
            'No active review',
            variant: TextVariant.h5,
            color: AppColors.textSecondary,
          ),
          const SizedBox(height: AppSpacing.sm),
          AppText(
            'Start a new weekly review',
            variant: TextVariant.bodyMedium,
            color: AppColors.textSecondary,
          ),
          const SizedBox(height: AppSpacing.xl),
          AppButton(
            label: 'Start Weekly Review',
            icon: Icons.play_arrow,
            onPressed: _startNewReview,
          ),
        ],
      ),
    );
  }

  Widget _buildReviewContent(GtdReviewDto review) {
    final completedCount =
        review.checklist.where((item) => item.isCompleted).length;
    final progress = completedCount / review.checklist.length;

    return Column(
      children: [
        // Progress header
        Container(
          padding: const EdgeInsets.all(AppSpacing.lg),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                AppColors.primary,
                AppColors.primary.withOpacity(0.8),
              ],
            ),
          ),
          child: SafeArea(
            bottom: false,
            child: Column(
              children: [
                Row(
                  children: [
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text(
                            'Weekly Review',
                            style: TextStyle(
                              fontSize: 24,
                              fontWeight: FontWeight.bold,
                              color: Colors.white,
                            ),
                          ),
                          const SizedBox(height: AppSpacing.xs),
                          Text(
                            '${review.reviewDate.day}/${review.reviewDate.month}/${review.reviewDate.year}',
                            style: const TextStyle(
                              fontSize: 14,
                              color: Colors.white70,
                            ),
                          ),
                        ],
                      ),
                    ),
                    CircleAvatar(
                      radius: 35,
                      backgroundColor: Colors.white,
                      child: Text(
                        '${(progress * 100).toInt()}%',
                        style: const TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: AppColors.primary,
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.md),
                ClipRRect(
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  child: LinearProgressIndicator(
                    value: progress,
                    minHeight: 8,
                    backgroundColor: Colors.white.withOpacity(0.3),
                    valueColor: const AlwaysStoppedAnimation<Color>(Colors.white),
                  ),
                ),
                const SizedBox(height: AppSpacing.xs),
                Text(
                  '$completedCount of ${review.checklist.length} completed',
                  style: const TextStyle(
                    fontSize: 12,
                    color: Colors.white70,
                  ),
                ),
              ],
            ),
          ),
        ),

        // Checklist
        Expanded(
          child: ReviewChecklist(
            review: review,
            onChecklistUpdated: (updatedChecklist) {
              ref.read(
                updateReviewChecklistProvider(
                  review.id,
                  updatedChecklist,
                ).future,
              );
            },
          ),
        ),

        // Complete button
        if (!review.isCompleted)
          Container(
            padding: const EdgeInsets.all(AppSpacing.md),
            decoration: BoxDecoration(
              color: AppColors.surface,
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(0.1),
                  blurRadius: 10,
                  offset: const Offset(0, -2),
                ),
              ],
            ),
            child: SafeArea(
              child: AppButton(
                label: 'Complete Review',
                icon: Icons.check_circle,
                onPressed: review.checklist.every((item) => item.isCompleted)
                    ? () => _completeReview(review)
                    : null,
              ),
            ),
          ),
      ],
    );
  }

  Future<void> _startNewReview() async {
    await ref.read(
      createReviewProvider(
        userId: 'user-1',
        reviewType: ReviewType.weekly,
      ).future,
    );

    if (mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Weekly review started'),
          backgroundColor: AppColors.success,
        ),
      );
    }
  }

  Future<void> _completeReview(GtdReviewDto review) async {
    final notesController = TextEditingController();

    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Complete Review'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Text('Add any notes or reflections from this review:'),
            const SizedBox(height: AppSpacing.md),
            TextField(
              controller: notesController,
              maxLines: 4,
              decoration: const InputDecoration(
                hintText: 'Notes (optional)',
                border: OutlineInputBorder(),
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Complete'),
          ),
        ],
      ),
    );

    if (confirmed == true) {
      await ref.read(
        completeReviewProvider(
          review.id,
          notesController.text.isEmpty ? null : notesController.text,
        ).future,
      );

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Review completed! 🎉'),
            backgroundColor: AppColors.success,
          ),
        );
      }
    }
  }
}
