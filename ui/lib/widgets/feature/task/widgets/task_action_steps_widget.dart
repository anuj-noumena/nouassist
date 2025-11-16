import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_button.dart';

class TaskActionStepsWidget extends StatefulWidget {
  final String taskId;

  const TaskActionStepsWidget({super.key, required this.taskId});

  @override
  State<TaskActionStepsWidget> createState() => _TaskActionStepsWidgetState();
}

class _TaskActionStepsWidgetState extends State<TaskActionStepsWidget> {
  final List<ActionStep> _steps = [
    ActionStep(
      id: '1',
      description: 'Review requirements',
      isCompleted: true,
    ),
    ActionStep(
      id: '2',
      description: 'Create implementation plan',
      isCompleted: true,
    ),
    ActionStep(
      id: '3',
      description: 'Write unit tests',
      isCompleted: false,
    ),
    ActionStep(
      id: '4',
      description: 'Code review',
      isCompleted: false,
    ),
  ];

  bool _isAddingStep = false;
  final _stepController = TextEditingController();

  @override
  void dispose() {
    _stepController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Action Steps',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            if (!_isAddingStep)
              IconButton(
                icon: const Icon(Icons.add),
                onPressed: () {
                  setState(() {
                    _isAddingStep = true;
                  });
                },
              ),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        if (_isAddingStep) ...[
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: _stepController,
                  decoration: const InputDecoration(
                    hintText: 'Add action step',
                    isDense: true,
                  ),
                  onSubmitted: (_) => _addStep(),
                ),
              ),
              const SizedBox(width: AppSpacing.sm),
              AppButton(
                label: 'Add',
                isFullWidth: false,
                variant: ButtonVariant.outlined,
                size: ButtonSize.small,
                onPressed: _addStep,
              ),
              IconButton(
                icon: const Icon(Icons.close, size: 20),
                onPressed: () {
                  setState(() {
                    _isAddingStep = false;
                    _stepController.clear();
                  });
                },
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.md),
        ],
        if (_steps.isEmpty)
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.surfaceVariant,
              borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
            ),
            child: Center(
              child: AppText(
                'No action steps yet',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ),
          )
        else
          ..._steps.map((step) => _buildActionStep(step)).toList(),
      ],
    );
  }

  Widget _buildActionStep(ActionStep step) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: ListTile(
        leading: Checkbox(
          value: step.isCompleted,
          onChanged: (value) {
            setState(() {
              step.isCompleted = value ?? false;
            });
          },
        ),
        title: Text(
          step.description,
          style: TextStyle(
            decoration: step.isCompleted ? TextDecoration.lineThrough : null,
            color: step.isCompleted
                ? AppColors.textSecondary
                : AppColors.textPrimary,
          ),
        ),
        trailing: IconButton(
          icon: const Icon(Icons.delete_outline, size: 20),
          onPressed: () {
            setState(() {
              _steps.remove(step);
            });
          },
        ),
      ),
    );
  }

  void _addStep() {
    if (_stepController.text.isNotEmpty) {
      setState(() {
        _steps.add(
          ActionStep(
            id: DateTime.now().millisecondsSinceEpoch.toString(),
            description: _stepController.text,
            isCompleted: false,
          ),
        );
        _stepController.clear();
        _isAddingStep = false;
      });
    }
  }
}

class ActionStep {
  final String id;
  final String description;
  bool isCompleted;

  ActionStep({
    required this.id,
    required this.description,
    required this.isCompleted,
  });
}
