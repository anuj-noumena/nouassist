import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/gtd_review_dto.dart';
import '../../../atomic/app_text.dart';

class ReviewChecklist extends StatefulWidget {
  final GtdReviewDto review;
  final Function(List<ReviewChecklistItemDto>)? onChecklistUpdated;

  const ReviewChecklist({
    super.key,
    required this.review,
    this.onChecklistUpdated,
  });

  @override
  State<ReviewChecklist> createState() => _ReviewChecklistState();
}

class _ReviewChecklistState extends State<ReviewChecklist> {
  late List<ReviewChecklistItemDto> _checklist;

  @override
  void initState() {
    super.initState();
    _checklist = List.from(widget.review.checklist);
  }

  @override
  void didUpdateWidget(ReviewChecklist oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.review.id != oldWidget.review.id) {
      _checklist = List.from(widget.review.checklist);
    }
  }

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: const EdgeInsets.all(AppSpacing.md),
      itemCount: _checklist.length,
      itemBuilder: (context, index) {
        final item = _checklist[index];
        return _buildChecklistItem(item, index);
      },
    );
  }

  Widget _buildChecklistItem(ReviewChecklistItemDto item, int index) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: CheckboxListTile(
        value: item.isCompleted,
        onChanged: (value) {
          setState(() {
            _checklist[index] = item.copyWith(isCompleted: value ?? false);
          });
          widget.onChecklistUpdated?.call(_checklist);
        },
        title: AppText(
          item.description,
          variant: TextVariant.bodyMedium,
          fontWeight: FontWeight.w600,
          decoration: item.isCompleted ? TextDecoration.lineThrough : null,
        ),
        secondary: Container(
          width: 32,
          height: 32,
          decoration: BoxDecoration(
            color: item.isCompleted
                ? AppColors.success.withOpacity(0.1)
                : AppColors.primary.withOpacity(0.1),
            shape: BoxShape.circle,
          ),
          child: Center(
            child: Text(
              '${index + 1}',
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: item.isCompleted ? AppColors.success : AppColors.primary,
              ),
            ),
          ),
        ),
        controlAffinity: ListTileControlAffinity.leading,
        activeColor: AppColors.success,
        contentPadding: const EdgeInsets.symmetric(
          horizontal: AppSpacing.md,
          vertical: AppSpacing.sm,
        ),
      ),
    );
  }
}
