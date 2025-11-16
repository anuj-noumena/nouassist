import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/gtd_context_dto.dart';

class ContextChip extends StatelessWidget {
  final GtdContextDto context;
  final VoidCallback? onTap;
  final VoidCallback? onDelete;

  const ContextChip({
    super.key,
    required this.context,
    this.onTap,
    this.onDelete,
  });

  @override
  Widget build(BuildContext context) {
    final color = _parseColor(this.context.color);

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      child: Container(
        padding: const EdgeInsets.symmetric(
          horizontal: AppSpacing.sm,
          vertical: AppSpacing.xs,
        ),
        decoration: BoxDecoration(
          color: color.withOpacity(0.1),
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          border: Border.all(color: color),
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              _parseIcon(this.context.icon),
              size: 14,
              color: color,
            ),
            const SizedBox(width: AppSpacing.xs),
            Text(
              this.context.name,
              style: TextStyle(
                fontSize: 12,
                fontWeight: FontWeight.w600,
                color: color,
              ),
            ),
            if (onDelete != null) ...[
              const SizedBox(width: AppSpacing.xs),
              InkWell(
                onTap: onDelete,
                child: Icon(
                  Icons.close,
                  size: 14,
                  color: color,
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }

  Color _parseColor(String colorString) {
    return Color(
      int.parse(colorString.substring(1), radix: 16) + 0xFF000000,
    );
  }

  IconData _parseIcon(String iconName) {
    switch (iconName) {
      case 'place':
        return Icons.place;
      case 'computer':
        return Icons.computer;
      case 'phone':
        return Icons.phone;
      case 'home':
        return Icons.home;
      case 'business':
        return Icons.business;
      case 'shopping_cart':
        return Icons.shopping_cart;
      case 'person':
        return Icons.person;
      case 'bolt':
        return Icons.bolt;
      case 'schedule':
        return Icons.schedule;
      default:
        return Icons.label;
    }
  }
}
