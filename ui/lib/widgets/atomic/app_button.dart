import 'package:flutter/material.dart';
import '../../core/theme/app_colors.dart';
import '../../core/theme/app_text_styles.dart';
import '../../core/theme/app_spacing.dart';

enum ButtonVariant { primary, secondary, outlined, text, danger, success }

enum ButtonSize { small, medium, large }

class AppButton extends StatelessWidget {
  final String label;
  final VoidCallback? onPressed;
  final bool isLoading;
  final bool isFullWidth;
  final ButtonVariant variant;
  final ButtonSize size;
  final IconData? icon;
  final bool iconRight;

  const AppButton({
    super.key,
    required this.label,
    this.onPressed,
    this.isLoading = false,
    this.isFullWidth = true,
    this.variant = ButtonVariant.primary,
    this.size = ButtonSize.medium,
    this.icon,
    this.iconRight = false,
  });

  @override
  Widget build(BuildContext context) {
    final bool isEnabled = onPressed != null && !isLoading;

    return SizedBox(
      width: isFullWidth ? double.infinity : null,
      height: _getHeight(),
      child: ElevatedButton(
        onPressed: isEnabled ? onPressed : null,
        style: _getButtonStyle(),
        child: isLoading
            ? SizedBox(
                height: 20,
                width: 20,
                child: CircularProgressIndicator(
                  strokeWidth: 2,
                  valueColor: AlwaysStoppedAnimation<Color>(
                    _getLoadingColor(),
                  ),
                ),
              )
            : Row(
                mainAxisSize: MainAxisSize.min,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  if (icon != null && !iconRight) ...[
                    Icon(icon, size: _getIconSize()),
                    SizedBox(width: AppSpacing.sm),
                  ],
                  Text(label, style: _getTextStyle()),
                  if (icon != null && iconRight) ...[
                    SizedBox(width: AppSpacing.sm),
                    Icon(icon, size: _getIconSize()),
                  ],
                ],
              ),
      ),
    );
  }

  ButtonStyle _getButtonStyle() {
    return ElevatedButton.styleFrom(
      backgroundColor: _getBackgroundColor(),
      foregroundColor: _getForegroundColor(),
      disabledBackgroundColor: AppColors.border,
      disabledForegroundColor: AppColors.textDisabled,
      elevation: variant == ButtonVariant.outlined || variant == ButtonVariant.text ? 0 : 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
        side: variant == ButtonVariant.outlined
            ? BorderSide(color: _getBorderColor())
            : BorderSide.none,
      ),
      padding: EdgeInsets.symmetric(
        horizontal: _getHorizontalPadding(),
        vertical: AppSpacing.sm,
      ),
    );
  }

  Color _getBackgroundColor() {
    switch (variant) {
      case ButtonVariant.primary:
        return AppColors.primary;
      case ButtonVariant.secondary:
        return AppColors.secondary;
      case ButtonVariant.outlined:
      case ButtonVariant.text:
        return Colors.transparent;
      case ButtonVariant.danger:
        return AppColors.error;
      case ButtonVariant.success:
        return AppColors.success;
    }
  }

  Color _getForegroundColor() {
    switch (variant) {
      case ButtonVariant.primary:
      case ButtonVariant.secondary:
      case ButtonVariant.danger:
      case ButtonVariant.success:
        return AppColors.textOnPrimary;
      case ButtonVariant.outlined:
      case ButtonVariant.text:
        return AppColors.primary;
    }
  }

  Color _getBorderColor() {
    return AppColors.primary;
  }

  Color _getLoadingColor() {
    switch (variant) {
      case ButtonVariant.primary:
      case ButtonVariant.secondary:
      case ButtonVariant.danger:
      case ButtonVariant.success:
        return Colors.white;
      case ButtonVariant.outlined:
      case ButtonVariant.text:
        return AppColors.primary;
    }
  }

  TextStyle _getTextStyle() {
    return (size == ButtonSize.small
            ? AppTextStyles.buttonSmall
            : AppTextStyles.button)
        .copyWith(color: _getForegroundColor());
  }

  double _getHeight() {
    switch (size) {
      case ButtonSize.small:
        return 36;
      case ButtonSize.medium:
        return 44;
      case ButtonSize.large:
        return 52;
    }
  }

  double _getHorizontalPadding() {
    switch (size) {
      case ButtonSize.small:
        return AppSpacing.md;
      case ButtonSize.medium:
        return AppSpacing.lg;
      case ButtonSize.large:
        return AppSpacing.xl;
    }
  }

  double _getIconSize() {
    switch (size) {
      case ButtonSize.small:
        return AppSpacing.iconXs;
      case ButtonSize.medium:
        return AppSpacing.iconSm;
      case ButtonSize.large:
        return AppSpacing.iconMd;
    }
  }
}
