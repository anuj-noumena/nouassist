import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/task_dto.dart';
import '../../../providers/task_form_provider.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_form_field.dart';
import '../../atomic/app_dropdown.dart';
import '../../atomic/app_date_picker.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_chip.dart';

class TaskFormScreen extends ConsumerStatefulWidget {
  final String? taskId;

  const TaskFormScreen({super.key, this.taskId});

  @override
  ConsumerState<TaskFormScreen> createState() => _TaskFormScreenState();
}

class _TaskFormScreenState extends ConsumerState<TaskFormScreen> {
  final _tagController = TextEditingController();

  @override
  void dispose() {
    _tagController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final formState = ref.watch(taskFormProvider(taskId: widget.taskId));
    final formNotifier = ref.read(taskFormProvider(taskId: widget.taskId).notifier);

    return Scaffold(
      appBar: AppBar(
        title: Text(widget.taskId == null ? 'Create Task' : 'Edit Task'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (formState.submitError != null) ...[
              Container(
                padding: const EdgeInsets.all(AppSpacing.md),
                decoration: BoxDecoration(
                  color: AppColors.error.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  border: Border.all(color: AppColors.error),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.error_outline, color: AppColors.error),
                    const SizedBox(width: AppSpacing.sm),
                    Expanded(
                      child: AppText(
                        formState.submitError!,
                        variant: TextVariant.bodySmall,
                        color: AppColors.error,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: AppSpacing.md),
            ],
            AppFormField(
              label: 'Title',
              hint: 'Enter task title',
              initialValue: formState.title,
              errorText: formState.errors['title'],
              required: true,
              onChanged: formNotifier.updateTitle,
            ),
            const SizedBox(height: AppSpacing.md),
            AppFormField(
              label: 'Description',
              hint: 'Enter task description (optional)',
              initialValue: formState.description,
              maxLines: 4,
              onChanged: formNotifier.updateDescription,
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppDropdown<TaskPriority>(
                    label: 'Priority',
                    value: formState.priority,
                    required: true,
                    items: TaskPriority.values.map((priority) {
                      return DropdownMenuItem(
                        value: priority,
                        child: Row(
                          children: [
                            Container(
                              width: 12,
                              height: 12,
                              decoration: BoxDecoration(
                                color: _getPriorityColor(priority),
                                shape: BoxShape.circle,
                              ),
                            ),
                            const SizedBox(width: AppSpacing.sm),
                            Text(priority.displayName),
                          ],
                        ),
                      );
                    }).toList(),
                    onChanged: (value) {
                      if (value != null) {
                        formNotifier.updatePriority(value);
                      }
                    },
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppDropdown<TaskStatus>(
                    label: 'Status',
                    value: formState.status,
                    required: true,
                    items: TaskStatus.values.map((status) {
                      return DropdownMenuItem(
                        value: status,
                        child: Text(status.displayName),
                      );
                    }).toList(),
                    onChanged: (value) {
                      if (value != null) {
                        formNotifier.updateStatus(value);
                      }
                    },
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppDatePicker(
                    label: 'Start Date',
                    value: formState.startDate,
                    onChanged: formNotifier.updateStartDate,
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppDatePicker(
                    label: 'Due Date',
                    value: formState.dueDate,
                    errorText: formState.errors['dueDate'],
                    onChanged: formNotifier.updateDueDate,
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppFormField(
                    label: 'Estimated Hours',
                    hint: '0.0',
                    initialValue: formState.estimatedHours?.toString(),
                    keyboardType: const TextInputType.numberWithOptions(decimal: true),
                    errorText: formState.errors['estimatedHours'],
                    onChanged: (value) {
                      final hours = double.tryParse(value);
                      formNotifier.updateEstimatedHours(hours);
                    },
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppFormField(
                    label: 'Story Points',
                    hint: '0',
                    initialValue: formState.storyPoints?.toString(),
                    keyboardType: TextInputType.number,
                    errorText: formState.errors['storyPoints'],
                    onChanged: (value) {
                      final points = int.tryParse(value);
                      formNotifier.updateStoryPoints(points);
                    },
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            AppText(
              'Progress: ${formState.progress.toInt()}%',
              variant: TextVariant.label,
            ),
            const SizedBox(height: AppSpacing.xs),
            Slider(
              value: formState.progress,
              min: 0,
              max: 100,
              divisions: 20,
              label: '${formState.progress.toInt()}%',
              onChanged: formNotifier.updateProgress,
            ),
            const SizedBox(height: AppSpacing.md),
            AppText(
              'Tags',
              variant: TextVariant.label,
            ),
            const SizedBox(height: AppSpacing.xs),
            if (formState.tags.isNotEmpty) ...[
              Wrap(
                spacing: AppSpacing.xs,
                runSpacing: AppSpacing.xs,
                children: formState.tags.map((tag) {
                  return AppChip(
                    label: tag,
                    onDelete: () => formNotifier.removeTag(tag),
                  );
                }).toList(),
              ),
              const SizedBox(height: AppSpacing.sm),
            ],
            Row(
              children: [
                Expanded(
                  child: AppFormField(
                    controller: _tagController,
                    hint: 'Add a tag',
                    onChanged: (_) {},
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                AppButton(
                  label: 'Add',
                  isFullWidth: false,
                  variant: ButtonVariant.outlined,
                  size: ButtonSize.medium,
                  onPressed: () {
                    if (_tagController.text.isNotEmpty) {
                      formNotifier.addTag(_tagController.text.trim());
                      _tagController.clear();
                    }
                  },
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.xl),
            AppButton(
              label: widget.taskId == null ? 'Create Task' : 'Update Task',
              isLoading: formState.isSubmitting,
              onPressed: () => _submitForm(context, formNotifier),
            ),
          ],
        ),
      ),
    );
  }

  Color _getPriorityColor(TaskPriority priority) {
    switch (priority) {
      case TaskPriority.lowest:
        return AppColors.priorityLowest;
      case TaskPriority.low:
        return AppColors.priorityLow;
      case TaskPriority.medium:
        return AppColors.priorityMedium;
      case TaskPriority.high:
        return AppColors.priorityHigh;
      case TaskPriority.highest:
        return AppColors.priorityHighest;
    }
  }

  Future<void> _submitForm(
    BuildContext context,
    TaskForm formNotifier,
  ) async {
    final result = await formNotifier.submit();

    if (result != null && context.mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            widget.taskId == null
                ? 'Task created successfully'
                : 'Task updated successfully',
          ),
          backgroundColor: AppColors.success,
        ),
      );
      Navigator.of(context).pop();
    }
  }
}
