import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../core/utils/extensions.dart';
import '../../../../data/models/time_block_dto.dart';
import '../../../atomic/app_text.dart';

class TimeBlockCard extends StatelessWidget {
  final TimeBlockDto timeBlock;
  final VoidCallback? onTap;
  final VoidCallback? onStatusChange;

  const TimeBlockCard({
    super.key,
    required this.timeBlock,
    this.onTap,
    this.onStatusChange,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            border: Border(
              left: BorderSide(
                color: Color(int.parse(timeBlock.color.replaceFirst('#', '0xFF'))),
                width: 4,
              ),
            ),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.md),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Expanded(
                      child: AppText(
                        timeBlock.title,
                        variant: TextVariant.bodyLarge,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                    _buildStatusBadge(timeBlock.status),
                  ],
                ),
                const SizedBox(height: AppSpacing.xs),
                Row(
                  children: [
                    Icon(
                      Icons.access_time,
                      size: 14,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      '${_formatTime(timeBlock.startDateTime)} - ${_formatTime(timeBlock.endDateTime)}',
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.sm),
                    AppText(
                      _getDuration(),
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                ),
                if (timeBlock.description != null) ...[
                  const SizedBox(height: AppSpacing.xs),
                  AppText(
                    timeBlock.description!,
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                ],
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.sm,
                  runSpacing: AppSpacing.xs,
                  children: [
                    _buildTypeBadge(),
                    if (timeBlock.location != null) _buildLocationChip(),
                    if (timeBlock.isRecurring) _buildRecurringChip(),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildStatusBadge(TimeBlockStatus status) {
    Color color;
    IconData icon;

    switch (status) {
      case TimeBlockStatus.scheduled:
        color = AppColors.info;
        icon = Icons.schedule;
        break;
      case TimeBlockStatus.inProgress:
        color = AppColors.warning;
        icon = Icons.play_circle;
        break;
      case TimeBlockStatus.completed:
        color = AppColors.success;
        icon = Icons.check_circle;
        break;
      case TimeBlockStatus.cancelled:
        color = AppColors.error;
        icon = Icons.cancel;
        break;
    }

    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xxs,
      ),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 12, color: color),
          const SizedBox(width: AppSpacing.xs),
          Text(
            status.displayName,
            style: TextStyle(
              fontSize: 10,
              fontWeight: FontWeight.w600,
              color: color,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTypeBadge() {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xxs,
      ),
      decoration: BoxDecoration(
        color: Color(int.parse(timeBlock.color.replaceFirst('#', '0xFF')))
            .withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        timeBlock.blockType.displayName,
        style: TextStyle(
          fontSize: 10,
          fontWeight: FontWeight.w600,
          color: Color(int.parse(timeBlock.color.replaceFirst('#', '0xFF'))),
        ),
      ),
    );
  }

  Widget _buildLocationChip() {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xxs,
      ),
      decoration: BoxDecoration(
        color: AppColors.surfaceVariant,
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          const Icon(Icons.location_on, size: 10),
          const SizedBox(width: AppSpacing.xxs),
          Text(
            timeBlock.location!,
            style: const TextStyle(fontSize: 10),
          ),
        ],
      ),
    );
  }

  Widget _buildRecurringChip() {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xxs,
      ),
      decoration: BoxDecoration(
        color: AppColors.info.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: const Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.repeat, size: 10, color: AppColors.info),
          SizedBox(width: AppSpacing.xxs),
          Text(
            'Recurring',
            style: TextStyle(
              fontSize: 10,
              fontWeight: FontWeight.w600,
              color: AppColors.info,
            ),
          ),
        ],
      ),
    );
  }

  String _formatTime(DateTime dateTime) {
    return '${dateTime.hour.toString().padLeft(2, '0')}:${dateTime.minute.toString().padLeft(2, '0')}';
  }

  String _getDuration() {
    final duration = timeBlock.endDateTime.difference(timeBlock.startDateTime);
    final hours = duration.inHours;
    final minutes = duration.inMinutes.remainder(60);

    if (hours > 0 && minutes > 0) {
      return '${hours}h ${minutes}m';
    } else if (hours > 0) {
      return '${hours}h';
    } else {
      return '${minutes}m';
    }
  }
}
