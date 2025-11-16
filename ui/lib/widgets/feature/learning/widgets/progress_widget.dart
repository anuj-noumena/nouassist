import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../atomic/app_text.dart';

class ProgressWidget extends StatelessWidget {
  final double percentage;
  final int completedLessons;
  final int totalLessons;
  final int timeSpent;

  const ProgressWidget({
    super.key,
    required this.percentage,
    required this.completedLessons,
    required this.totalLessons,
    required this.timeSpent,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surfaceVariant,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              AppText(
                'Course Progress',
                variant: TextVariant.bodyMedium,
                fontWeight: FontWeight.w600,
              ),
              AppText(
                '${percentage.toInt()}%',
                variant: TextVariant.h4,
                fontWeight: FontWeight.bold,
                color: _getProgressColor(),
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.md),
          ClipRRect(
            borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            child: LinearProgressIndicator(
              value: percentage / 100,
              minHeight: 10,
              backgroundColor: AppColors.border,
              valueColor: AlwaysStoppedAnimation<Color>(_getProgressColor()),
            ),
          ),
          const SizedBox(height: AppSpacing.md),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              _buildStat(
                Icons.check_circle_outline,
                '$completedLessons/$totalLessons',
                'Lessons',
              ),
              _buildStat(
                Icons.access_time,
                _formatTime(timeSpent),
                'Time Spent',
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStat(IconData icon, String value, String label) {
    return Row(
      children: [
        Icon(icon, size: 20, color: AppColors.textSecondary),
        const SizedBox(width: AppSpacing.xs),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              value,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w600,
            ),
            AppText(
              label,
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ],
        ),
      ],
    );
  }

  Color _getProgressColor() {
    if (percentage < 30) {
      return AppColors.error;
    } else if (percentage < 70) {
      return AppColors.warning;
    } else {
      return AppColors.success;
    }
  }

  String _formatTime(int minutes) {
    final hours = minutes ~/ 60;
    final mins = minutes % 60;

    if (hours > 0) {
      return '${hours}h ${mins}m';
    }
    return '${mins}m';
  }
}
