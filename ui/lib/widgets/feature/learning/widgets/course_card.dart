import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/course_dto.dart';
import '../../../atomic/app_text.dart';

class CourseCard extends StatelessWidget {
  final CourseDto course;
  final VoidCallback? onTap;
  final double? progress;

  const CourseCard({
    super.key,
    required this.course,
    this.onTap,
    this.progress,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      clipBehavior: Clip.antiAlias,
      child: InkWell(
        onTap: onTap,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Thumbnail
            Container(
              height: 160,
              width: double.infinity,
              color: AppColors.surfaceVariant,
              child: course.thumbnail != null
                  ? Image.network(
                      course.thumbnail!,
                      fit: BoxFit.cover,
                      errorBuilder: (_, __, ___) => _buildPlaceholder(),
                    )
                  : _buildPlaceholder(),
            ),
            Padding(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Category badge
                  Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: AppSpacing.sm,
                      vertical: AppSpacing.xs,
                    ),
                    decoration: BoxDecoration(
                      color: _getCategoryColor().withOpacity(0.1),
                      borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                    ),
                    child: Text(
                      course.category.displayName,
                      style: TextStyle(
                        fontSize: 10,
                        fontWeight: FontWeight.w600,
                        color: _getCategoryColor(),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  // Title
                  AppText(
                    course.title,
                    variant: TextVariant.bodyLarge,
                    fontWeight: FontWeight.w600,
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                  const SizedBox(height: AppSpacing.xs),
                  // Instructor
                  AppText(
                    'By ${course.instructorName}',
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.md),
                  // Stats
                  Row(
                    children: [
                      Icon(
                        Icons.star,
                        size: 16,
                        color: AppColors.warning,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        course.rating.toStringAsFixed(1),
                        variant: TextVariant.bodySmall,
                        fontWeight: FontWeight.w600,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        '(${course.reviewCount})',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                      const Spacer(),
                      Icon(
                        Icons.people_outline,
                        size: 16,
                        color: AppColors.textSecondary,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      AppText(
                        _formatEnrollment(course.enrollmentCount),
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  // Difficulty and duration
                  Row(
                    children: [
                      _buildInfoChip(
                        Icons.signal_cellular_alt,
                        course.difficulty.displayName,
                        _getDifficultyColor(),
                      ),
                      const SizedBox(width: AppSpacing.sm),
                      _buildInfoChip(
                        Icons.access_time,
                        '${_formatDuration(course.estimatedDuration)}',
                        AppColors.textSecondary,
                      ),
                    ],
                  ),
                  // Progress bar
                  if (progress != null) ...[
                    const SizedBox(height: AppSpacing.md),
                    ClipRRect(
                      borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                      child: LinearProgressIndicator(
                        value: progress! / 100,
                        minHeight: 6,
                        backgroundColor: AppColors.border,
                        valueColor: const AlwaysStoppedAnimation<Color>(
                          AppColors.success,
                        ),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.xs),
                    AppText(
                      '${progress!.toInt()}% Complete',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                  // Price
                  const SizedBox(height: AppSpacing.md),
                  Row(
                    children: [
                      if (course.isFree)
                        Container(
                          padding: const EdgeInsets.symmetric(
                            horizontal: AppSpacing.sm,
                            vertical: AppSpacing.xs,
                          ),
                          decoration: BoxDecoration(
                            color: AppColors.success.withOpacity(0.1),
                            borderRadius:
                                BorderRadius.circular(AppSpacing.radiusSm),
                          ),
                          child: const Text(
                            'FREE',
                            style: TextStyle(
                              fontSize: 12,
                              fontWeight: FontWeight.bold,
                              color: AppColors.success,
                            ),
                          ),
                        )
                      else if (course.price != null)
                        AppText(
                          '\$${course.price!.toStringAsFixed(2)}',
                          variant: TextVariant.h5,
                          fontWeight: FontWeight.bold,
                          color: AppColors.primary,
                        ),
                      const Spacer(),
                      if (course.isCertified)
                        Container(
                          padding: const EdgeInsets.symmetric(
                            horizontal: AppSpacing.sm,
                            vertical: AppSpacing.xs,
                          ),
                          decoration: BoxDecoration(
                            color: AppColors.warning.withOpacity(0.1),
                            borderRadius:
                                BorderRadius.circular(AppSpacing.radiusSm),
                          ),
                          child: const Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Icon(
                                Icons.verified,
                                size: 12,
                                color: AppColors.warning,
                              ),
                              SizedBox(width: AppSpacing.xs),
                              Text(
                                'Certified',
                                style: TextStyle(
                                  fontSize: 10,
                                  fontWeight: FontWeight.w600,
                                  color: AppColors.warning,
                                ),
                              ),
                            ],
                          ),
                        ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPlaceholder() {
    return Center(
      child: Icon(
        Icons.school,
        size: 64,
        color: AppColors.textDisabled,
      ),
    );
  }

  Widget _buildInfoChip(IconData icon, String text, Color color) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(icon, size: 14, color: color),
        const SizedBox(width: AppSpacing.xs),
        Text(
          text,
          style: TextStyle(
            fontSize: 12,
            color: color,
          ),
        ),
      ],
    );
  }

  Color _getCategoryColor() {
    switch (course.category) {
      case CourseCategory.programming:
        return AppColors.primary;
      case CourseCategory.design:
        return const Color(0xFFE91E63);
      case CourseCategory.business:
        return const Color(0xFF795548);
      case CourseCategory.marketing:
        return const Color(0xFFFF9800);
      case CourseCategory.dataScience:
        return const Color(0xFF9C27B0);
      case CourseCategory.devOps:
        return const Color(0xFF607D8B);
      case CourseCategory.mobile:
        return const Color(0xFF00BCD4);
      case CourseCategory.web:
        return const Color(0xFF4CAF50);
      case CourseCategory.other:
        return AppColors.textSecondary;
    }
  }

  Color _getDifficultyColor() {
    switch (course.difficulty) {
      case CourseDifficulty.beginner:
        return AppColors.success;
      case CourseDifficulty.intermediate:
        return AppColors.warning;
      case CourseDifficulty.advanced:
        return AppColors.error;
      case CourseDifficulty.expert:
        return const Color(0xFF9C27B0);
    }
  }

  String _formatEnrollment(int count) {
    if (count >= 1000) {
      return '${(count / 1000).toStringAsFixed(1)}K';
    }
    return count.toString();
  }

  String _formatDuration(int minutes) {
    final hours = minutes ~/ 60;
    if (hours > 0) {
      return '${hours}h';
    }
    return '${minutes}m';
  }
}
