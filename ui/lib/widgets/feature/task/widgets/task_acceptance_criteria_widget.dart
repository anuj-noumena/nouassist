import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_button.dart';

class TaskAcceptanceCriteriaWidget extends StatefulWidget {
  final String taskId;

  const TaskAcceptanceCriteriaWidget({super.key, required this.taskId});

  @override
  State<TaskAcceptanceCriteriaWidget> createState() =>
      _TaskAcceptanceCriteriaWidgetState();
}

class _TaskAcceptanceCriteriaWidgetState
    extends State<TaskAcceptanceCriteriaWidget> {
  final List<Criterion> _criteria = [
    Criterion(
      id: '1',
      text: 'User can log in with email and password',
      isCompleted: true,
    ),
    Criterion(
      id: '2',
      text: 'JWT token is generated on successful login',
      isCompleted: true,
    ),
    Criterion(
      id: '3',
      text: 'Token is automatically refreshed when expired',
      isCompleted: false,
    ),
    Criterion(
      id: '4',
      text: 'All API endpoints are protected',
      isCompleted: false,
    ),
  ];

  bool _isAddingCriteria = false;
  final _criteriaController = TextEditingController();

  @override
  void dispose() {
    _criteriaController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final completedCount = _criteria.where((c) => c.isCompleted).length;
    final progress = _criteria.isEmpty
        ? 0.0
        : (completedCount / _criteria.length) * 100;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Acceptance Criteria',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            if (!_isAddingCriteria)
              IconButton(
                icon: const Icon(Icons.add),
                onPressed: () {
                  setState(() {
                    _isAddingCriteria = true;
                  });
                },
              ),
          ],
        ),
        const SizedBox(height: AppSpacing.sm),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              '$completedCount of ${_criteria.length} completed',
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
            AppText(
              '${progress.toInt()}%',
              variant: TextVariant.bodySmall,
              fontWeight: FontWeight.w600,
              color: AppColors.primary,
            ),
          ],
        ),
        const SizedBox(height: AppSpacing.xs),
        ClipRRect(
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          child: LinearProgressIndicator(
            value: progress / 100,
            minHeight: 6,
            backgroundColor: AppColors.border,
            valueColor: const AlwaysStoppedAnimation<Color>(AppColors.primary),
          ),
        ),
        const SizedBox(height: AppSpacing.md),
        if (_isAddingCriteria) ...[
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: _criteriaController,
                  decoration: const InputDecoration(
                    hintText: 'Add acceptance criterion',
                    isDense: true,
                  ),
                  maxLines: 2,
                  onSubmitted: (_) => _addCriterion(),
                ),
              ),
              const SizedBox(width: AppSpacing.sm),
              AppButton(
                label: 'Add',
                isFullWidth: false,
                variant: ButtonVariant.outlined,
                size: ButtonSize.small,
                onPressed: _addCriterion,
              ),
              IconButton(
                icon: const Icon(Icons.close, size: 20),
                onPressed: () {
                  setState(() {
                    _isAddingCriteria = false;
                    _criteriaController.clear();
                  });
                },
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.md),
        ],
        if (_criteria.isEmpty)
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.surfaceVariant,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Center(
              child: AppText(
                'No acceptance criteria defined',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ),
          )
        else
          ..._criteria.map((criterion) => _buildCriterion(criterion)).toList(),
      ],
    );
  }

  Widget _buildCriterion(Criterion criterion) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: ListTile(
        leading: Checkbox(
          value: criterion.isCompleted,
          onChanged: (value) {
            setState(() {
              criterion.isCompleted = value ?? false;
            });
          },
        ),
        title: Text(
          criterion.text,
          style: TextStyle(
            decoration:
                criterion.isCompleted ? TextDecoration.lineThrough : null,
            color: criterion.isCompleted
                ? AppColors.textSecondary
                : AppColors.textPrimary,
          ),
        ),
        trailing: IconButton(
          icon: const Icon(Icons.delete_outline, size: 20),
          onPressed: () {
            setState(() {
              _criteria.remove(criterion);
            });
          },
        ),
      ),
    );
  }

  void _addCriterion() {
    if (_criteriaController.text.isNotEmpty) {
      setState(() {
        _criteria.add(
          Criterion(
            id: DateTime.now().millisecondsSinceEpoch.toString(),
            text: _criteriaController.text,
            isCompleted: false,
          ),
        );
        _criteriaController.clear();
        _isAddingCriteria = false;
      });
    }
  }
}

class Criterion {
  final String id;
  final String text;
  bool isCompleted;

  Criterion({
    required this.id,
    required this.text,
    required this.isCompleted,
  });
}
