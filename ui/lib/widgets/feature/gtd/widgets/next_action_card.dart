import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/gtd_next_action_dto.dart';
import '../../../../data/models/gtd_context_dto.dart';
import '../../../atomic/app_text.dart';
import 'context_chip.dart';

class NextActionCard extends StatelessWidget {
  final GtdNextActionDto action;
  final List<GtdContextDto> contexts;
  final VoidCallback? onTap;
  final VoidCallback? onToggleComplete;
  final VoidCallback? onToggleStar;
  final VoidCallback? onDelete;

  const NextActionCard({
    super.key,
    required this.action,
    required this.contexts,
    this.onTap,
    this.onToggleComplete,
    this.onToggleStar,
    this.onDelete,
  });

  @override
  Widget build(BuildContext context) {
    final actionContexts = contexts
        .where((c) => action.contextIds.contains(c.id))
        .toList();

    final isOverdue = action.dueDate != null &&
        action.dueDate!.isBefore(DateTime.now()) &&
        !action.isCompleted;

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
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Checkbox
                  InkWell(
                    onTap: onToggleComplete,
                    borderRadius: BorderRadius.circular(4),
                    child: Container(
                      width: 24,
                      height: 24,
                      decoration: BoxDecoration(
                        border: Border.all(
                          color: action.isCompleted
                              ? AppColors.success
                              : AppColors.border,
                          width: 2,
                        ),
                        borderRadius: BorderRadius.circular(4),
                        color: action.isCompleted
                            ? AppColors.success
                            : Colors.transparent,
                      ),
                      child: action.isCompleted
                          ? const Icon(
                              Icons.check,
                              size: 16,
                              color: Colors.white,
                            )
                          : null,
                    ),
                  ),
                  const SizedBox(width: AppSpacing.md),
                  // Content
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        AppText(
                          action.action,
                          variant: TextVariant.bodyLarge,
                          fontWeight: FontWeight.w600,
                          maxLines: 2,
                          overflow: TextOverflow.ellipsis,
                          decoration: action.isCompleted
                              ? TextDecoration.lineThrough
                              : null,
                        ),
                        if (action.notes != null) ...[
                          const SizedBox(height: AppSpacing.xs),
                          AppText(
                            action.notes!,
                            variant: TextVariant.bodySmall,
                            color: AppColors.textSecondary,
                            maxLines: 2,
                            overflow: TextOverflow.ellipsis,
                          ),
                        ],
                      ],
                    ),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  // Star button
                  InkWell(
                    onTap: onToggleStar,
                    child: Icon(
                      action.isStarred ? Icons.star : Icons.star_border,
                      size: 20,
                      color: action.isStarred
                          ? AppColors.warning
                          : AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: AppSpacing.sm),
              // Metadata
              Row(
                children: [
                  _buildMetaChip(
                    action.energyLevel.displayName,
                    _getEnergyColor(action.energyLevel),
                  ),
                  const SizedBox(width: AppSpacing.xs),
                  _buildMetaChip(
                    action.timeRequired.displayName,
                    AppColors.info,
                  ),
                  if (action.dueDate != null) ...[
                    const SizedBox(width: AppSpacing.xs),
                    _buildDueDateChip(action.dueDate!, isOverdue),
                  ],
                ],
              ),
              if (actionContexts.isNotEmpty) ...[
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.xs,
                  runSpacing: AppSpacing.xs,
                  children: actionContexts
                      .map((ctx) => ContextChip(context: ctx))
                      .toList(),
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildMetaChip(String label, Color color) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: 2,
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

  Widget _buildDueDateChip(DateTime dueDate, bool isOverdue) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: 2,
      ),
      decoration: BoxDecoration(
        color: isOverdue
            ? AppColors.error.withOpacity(0.1)
            : AppColors.warning.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(
            Icons.calendar_today,
            size: 10,
            color: isOverdue ? AppColors.error : AppColors.warning,
          ),
          const SizedBox(width: 2),
          Text(
            '${dueDate.day}/${dueDate.month}',
            style: TextStyle(
              fontSize: 11,
              fontWeight: FontWeight.w600,
              color: isOverdue ? AppColors.error : AppColors.warning,
            ),
          ),
        ],
      ),
    );
  }

  Color _getEnergyColor(EnergyLevel level) {
    switch (level) {
      case EnergyLevel.high:
        return AppColors.error;
      case EnergyLevel.medium:
        return AppColors.warning;
      case EnergyLevel.low:
        return AppColors.success;
    }
  }
}
