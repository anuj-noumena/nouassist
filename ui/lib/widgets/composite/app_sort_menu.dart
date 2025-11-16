import 'package:flutter/material.dart';
import '../../core/theme/app_colors.dart';
import '../../core/theme/app_spacing.dart';
import '../atomic/app_text.dart';

class AppSortMenu extends StatelessWidget {
  final List<SortOption> options;
  final String? currentSortField;
  final bool currentSortAscending;
  final ValueChanged<SortOption>? onSortChanged;

  const AppSortMenu({
    super.key,
    required this.options,
    this.currentSortField,
    this.currentSortAscending = true,
    this.onSortChanged,
  });

  @override
  Widget build(BuildContext context) {
    return PopupMenuButton<SortOption>(
      icon: const Icon(Icons.sort),
      tooltip: 'Sort',
      onSelected: onSortChanged,
      itemBuilder: (context) {
        return options.map((option) {
          final isSelected = currentSortField == option.field;
          return PopupMenuItem<SortOption>(
            value: option,
            child: Row(
              children: [
                Expanded(
                  child: AppText(
                    option.label,
                    variant: TextVariant.bodyMedium,
                    color: isSelected
                        ? AppColors.primary
                        : AppColors.textPrimary,
                  ),
                ),
                if (isSelected) ...[
                  const SizedBox(width: AppSpacing.sm),
                  Icon(
                    currentSortAscending
                        ? Icons.arrow_upward
                        : Icons.arrow_downward,
                    size: 16,
                    color: AppColors.primary,
                  ),
                ],
              ],
            ),
          );
        }).toList();
      },
    );
  }
}

class SortOption {
  final String label;
  final String field;
  final bool ascending;

  const SortOption({
    required this.label,
    required this.field,
    this.ascending = true,
  });
}
