import 'package:flutter/material.dart';
import '../../core/theme/app_spacing.dart';
import '../atomic/app_chip.dart';

class AppFilterChipGroup<T> extends StatelessWidget {
  final List<FilterChipData<T>> chips;
  final T? selectedValue;
  final ValueChanged<T?>? onSelected;

  const AppFilterChipGroup({
    super.key,
    required this.chips,
    this.selectedValue,
    this.onSelected,
  });

  @override
  Widget build(BuildContext context) {
    return Wrap(
      spacing: AppSpacing.sm,
      runSpacing: AppSpacing.sm,
      children: chips.map((chip) {
        final isSelected = selectedValue == chip.value;
        return AppChip(
          label: chip.label,
          icon: chip.icon,
          backgroundColor: isSelected ? chip.selectedColor : chip.backgroundColor,
          textColor: isSelected ? chip.selectedTextColor : chip.textColor,
          onTap: () {
            if (isSelected) {
              onSelected?.call(null);
            } else {
              onSelected?.call(chip.value);
            }
          },
        );
      }).toList(),
    );
  }
}

class FilterChipData<T> {
  final String label;
  final T value;
  final IconData? icon;
  final Color? backgroundColor;
  final Color? textColor;
  final Color? selectedColor;
  final Color? selectedTextColor;

  const FilterChipData({
    required this.label,
    required this.value,
    this.icon,
    this.backgroundColor,
    this.textColor,
    this.selectedColor,
    this.selectedTextColor,
  });
}
