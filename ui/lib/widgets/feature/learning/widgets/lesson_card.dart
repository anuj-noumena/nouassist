import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/lesson_dto.dart';
import '../../../atomic/app_text.dart';

class LessonCard extends StatelessWidget {
  final LessonDto lesson;
  final VoidCallback? onTap;
  final bool isCompleted;
  final bool isLocked;

  const LessonCard({
    super.key,
    required this.lesson,
    this.onTap,
    this.isCompleted = false,
    this.isLocked = false,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: InkWell(
        onTap: isLocked ? null : onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Row(
            children: [
              // Order number
              Container(
                width: 40,
                height: 40,
                decoration: BoxDecoration(
                  color: isCompleted
                      ? AppColors.success.withOpacity(0.1)
                      : AppColors.primary.withOpacity(0.1),
                  shape: BoxShape.circle,
                ),
                child: Center(
                  child: isCompleted
                      ? const Icon(
                          Icons.check,
                          color: AppColors.success,
                          size: 20,
                        )
                      : Text(
                          lesson.order.toString(),
                          style: TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.bold,
                            color: isLocked
                                ? AppColors.textDisabled
                                : AppColors.primary,
                          ),
                        ),
                ),
              ),
              const SizedBox(width: AppSpacing.md),
              // Content
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        Expanded(
                          child: AppText(
                            lesson.title,
                            variant: TextVariant.bodyMedium,
                            fontWeight: FontWeight.w600,
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis,
                            color: isLocked
                                ? AppColors.textDisabled
                                : AppColors.textPrimary,
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: AppSpacing.xs),
                    Row(
                      children: [
                        _buildTypeIcon(),
                        const SizedBox(width: AppSpacing.sm),
                        Icon(
                          Icons.access_time,
                          size: 14,
                          color: AppColors.textSecondary,
                        ),
                        const SizedBox(width: AppSpacing.xs),
                        AppText(
                          '${lesson.duration} min',
                          variant: TextVariant.bodySmall,
                          color: AppColors.textSecondary,
                        ),
                        if (lesson.isFree) ...[
                          const SizedBox(width: AppSpacing.sm),
                          Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: AppSpacing.xs,
                              vertical: 2,
                            ),
                            decoration: BoxDecoration(
                              color: AppColors.success.withOpacity(0.1),
                              borderRadius:
                                  BorderRadius.circular(AppSpacing.radiusSm),
                            ),
                            child: const Text(
                              'FREE',
                              style: TextStyle(
                                fontSize: 9,
                                fontWeight: FontWeight.bold,
                                color: AppColors.success,
                              ),
                            ),
                          ),
                        ],
                      ],
                    ),
                  ],
                ),
              ),
              // Lock icon
              if (isLocked)
                Icon(
                  Icons.lock,
                  color: AppColors.textDisabled,
                  size: 20,
                ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTypeIcon() {
    IconData icon;
    Color color;

    switch (lesson.lessonType) {
      case LessonType.video:
        icon = Icons.play_circle_outline;
        color = AppColors.primary;
        break;
      case LessonType.article:
        icon = Icons.article_outlined;
        color = AppColors.info;
        break;
      case LessonType.quiz:
        icon = Icons.quiz_outlined;
        color = AppColors.warning;
        break;
      case LessonType.assignment:
        icon = Icons.assignment_outlined;
        color = AppColors.error;
        break;
      case LessonType.exercise:
        icon = Icons.fitness_center;
        color = AppColors.success;
        break;
      case LessonType.project:
        icon = Icons.work_outline;
        color = const Color(0xFF9C27B0);
        break;
    }

    return Icon(icon, size: 16, color: color);
  }
}
