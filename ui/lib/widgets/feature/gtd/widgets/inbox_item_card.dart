import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/gtd_inbox_dto.dart';
import '../../../atomic/app_text.dart';

class InboxItemCard extends StatelessWidget {
  final GtdInboxDto item;
  final VoidCallback? onTap;
  final VoidCallback? onDelete;

  const InboxItemCard({
    super.key,
    required this.item,
    this.onTap,
    this.onDelete,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Expanded(
                    child: AppText(
                      item.content,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  _buildTypeIcon(item.itemType),
                ],
              ),
              if (item.notes != null) ...[
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  item.notes!,
                  variant: TextVariant.bodySmall,
                  color: AppColors.textSecondary,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
              const SizedBox(height: AppSpacing.sm),
              Row(
                children: [
                  _buildStatusChip(item.status),
                  const Spacer(),
                  if (item.source != null) ...[
                    Icon(
                      Icons.source,
                      size: 14,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      item.source!,
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.sm),
                  ],
                  Icon(
                    Icons.access_time,
                    size: 14,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(width: AppSpacing.xs),
                  AppText(
                    _formatTime(item.creationTime),
                    variant: TextVariant.bodySmall,
                    color: AppColors.textSecondary,
                  ),
                ],
              ),
              if (item.tags.isNotEmpty) ...[
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.xs,
                  runSpacing: AppSpacing.xs,
                  children: item.tags.map((tag) {
                    return Container(
                      padding: const EdgeInsets.symmetric(
                        horizontal: AppSpacing.sm,
                        vertical: 2,
                      ),
                      decoration: BoxDecoration(
                        color: AppColors.primary.withOpacity(0.1),
                        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                      ),
                      child: Text(
                        tag,
                        style: const TextStyle(
                          fontSize: 11,
                          color: AppColors.primary,
                        ),
                      ),
                    );
                  }).toList(),
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTypeIcon(InboxItemType type) {
    IconData icon;
    Color color;

    switch (type) {
      case InboxItemType.thought:
        icon = Icons.lightbulb_outline;
        color = AppColors.warning;
        break;
      case InboxItemType.email:
        icon = Icons.email_outlined;
        color = AppColors.info;
        break;
      case InboxItemType.note:
        icon = Icons.note_outlined;
        color = AppColors.success;
        break;
      case InboxItemType.task:
        icon = Icons.task_outlined;
        color = AppColors.primary;
        break;
      case InboxItemType.idea:
        icon = Icons.emoji_objects_outlined;
        color = AppColors.warning;
        break;
      case InboxItemType.meeting:
        icon = Icons.event_outlined;
        color = AppColors.error;
        break;
      case InboxItemType.other:
        icon = Icons.more_horiz;
        color = AppColors.textSecondary;
        break;
    }

    return Container(
      padding: const EdgeInsets.all(AppSpacing.sm),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Icon(icon, size: 18, color: color),
    );
  }

  Widget _buildStatusChip(InboxStatus status) {
    Color color;
    String label;

    switch (status) {
      case InboxStatus.unprocessed:
        color = AppColors.warning;
        label = 'Unprocessed';
        break;
      case InboxStatus.processing:
        color = AppColors.info;
        label = 'Processing';
        break;
      case InboxStatus.processed:
        color = AppColors.success;
        label = 'Processed';
        break;
    }

    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        label,
        style: TextStyle(
          fontSize: 11,
          fontWeight: FontWeight.w600,
          color: color,
        ),
      ),
    );
  }

  String _formatTime(DateTime time) {
    final now = DateTime.now();
    final difference = now.difference(time);

    if (difference.inMinutes < 60) {
      return '${difference.inMinutes}m ago';
    } else if (difference.inHours < 24) {
      return '${difference.inHours}h ago';
    } else if (difference.inDays < 7) {
      return '${difference.inDays}d ago';
    } else {
      return '${time.day}/${time.month}/${time.year}';
    }
  }
}
