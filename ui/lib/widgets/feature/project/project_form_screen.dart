import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/project_dto.dart';
import '../../../providers/project_form_provider.dart';
import '../../../providers/project_workflow_provider.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_form_field.dart';
import '../../atomic/app_dropdown.dart';
import '../../atomic/app_date_picker.dart';
import '../../atomic/app_text.dart';

class ProjectFormScreen extends ConsumerWidget {
  final String? projectId;

  ProjectFormScreen({super.key, this.projectId});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final formState = ref.watch(projectFormProvider(projectId: projectId));
    final formNotifier =
        ref.read(projectFormProvider(projectId: projectId).notifier);
    final workflowsAsync = ref.watch(projectWorkflowsProvider);

    return Scaffold(
      appBar: AppBar(
        title: Text(projectId == null ? 'Create Project' : 'Edit Project'),
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
              label: 'Project Name',
              hint: 'Enter project name',
              initialValue: formState.name,
              errorText: formState.errors['name'],
              required: true,
              onChanged: formNotifier.updateName,
            ),
            const SizedBox(height: AppSpacing.md),
            AppFormField(
              label: 'Description',
              hint: 'Enter project description (optional)',
              initialValue: formState.description,
              maxLines: 4,
              onChanged: formNotifier.updateDescription,
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppDropdown<ProjectStatus>(
                    label: 'Status',
                    value: formState.status,
                    required: true,
                    items: ProjectStatus.values.map((status) {
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
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppDropdown<ProjectPriority>(
                    label: 'Priority',
                    value: formState.priority,
                    required: true,
                    items: ProjectPriority.values.map((priority) {
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
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppDatePicker(
                    label: 'Start Date',
                    value: formState.startDate,
                    required: true,
                    onChanged: (value) {
                      if (value != null) {
                        formNotifier.updateStartDate(value);
                      }
                    },
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppDatePicker(
                    label: 'End Date',
                    value: formState.endDate,
                    errorText: formState.errors['endDate'],
                    required: true,
                    onChanged: (value) {
                      if (value != null) {
                        formNotifier.updateEndDate(value);
                      }
                    },
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            workflowsAsync.when(
              data: (workflows) {
                return AppDropdown<String>(
                  label: 'Workflow',
                  value: formState.workflowId,
                  required: true,
                  items: workflows.map((workflow) {
                    return DropdownMenuItem(
                      value: workflow.id,
                      child: Text(workflow.name),
                    );
                  }).toList(),
                  onChanged: (value) {
                    if (value != null) {
                      formNotifier.updateWorkflowId(value);
                    }
                  },
                );
              },
              loading: () => const CircularProgressIndicator(),
              error: (_, __) => const Text('Failed to load workflows'),
            ),
            const SizedBox(height: AppSpacing.md),
            AppDropdown<ProjectViewType>(
              label: 'Default View',
              value: formState.viewType,
              required: true,
              items: ProjectViewType.values.map((viewType) {
                return DropdownMenuItem(
                  value: viewType,
                  child: Row(
                    children: [
                      Icon(_getViewIcon(viewType), size: 20),
                      const SizedBox(width: AppSpacing.sm),
                      Text(viewType.displayName),
                    ],
                  ),
                );
              }).toList(),
              onChanged: (value) {
                if (value != null) {
                  formNotifier.updateViewType(value);
                }
              },
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppFormField(
                    label: 'Budget',
                    hint: '0.00',
                    initialValue: formState.budget?.toString(),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                    errorText: formState.errors['budget'],
                    onChanged: (value) {
                      final budget = double.tryParse(value);
                      formNotifier.updateBudget(budget);
                    },
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppFormField(
                    label: 'Actual Cost',
                    hint: '0.00',
                    initialValue: formState.actualCost?.toString(),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                    errorText: formState.errors['actualCost'],
                    onChanged: (value) {
                      final cost = double.tryParse(value);
                      formNotifier.updateActualCost(cost);
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
              'Project Color',
              variant: TextVariant.label,
            ),
            const SizedBox(height: AppSpacing.sm),
            Wrap(
              spacing: AppSpacing.sm,
              children: _colorOptions.map((color) {
                final isSelected = formState.color == color;
                return GestureDetector(
                  onTap: () => formNotifier.updateColor(color),
                  child: Container(
                    width: 48,
                    height: 48,
                    decoration: BoxDecoration(
                      color: Color(int.parse(color.replaceFirst('#', '0xFF'))),
                      shape: BoxShape.circle,
                      border: Border.all(
                        color: isSelected
                            ? AppColors.textPrimary
                            : Colors.transparent,
                        width: 3,
                      ),
                    ),
                    child: isSelected
                        ? const Icon(Icons.check, color: Colors.white)
                        : null,
                  ),
                );
              }).toList(),
            ),
            const SizedBox(height: AppSpacing.xl),
            AppButton(
              label: projectId == null ? 'Create Project' : 'Update Project',
              isLoading: formState.isSubmitting,
              onPressed: () => _submitForm(context, formNotifier),
            ),
          ],
        ),
      ),
    );
  }

  final List<String> _colorOptions = [
    '#2196F3',
    '#4CAF50',
    '#FF9800',
    '#9C27B0',
    '#F44336',
    '#00BCD4',
    '#FFC107',
    '#E91E63',
    '#3F51B5',
    '#8BC34A',
  ];

  Color _getPriorityColor(ProjectPriority priority) {
    switch (priority) {
      case ProjectPriority.low:
        return AppColors.priorityLow;
      case ProjectPriority.medium:
        return AppColors.priorityMedium;
      case ProjectPriority.high:
        return AppColors.priorityHigh;
      case ProjectPriority.critical:
        return AppColors.priorityHighest;
    }
  }

  IconData _getViewIcon(ProjectViewType viewType) {
    switch (viewType) {
      case ProjectViewType.list:
        return Icons.view_list;
      case ProjectViewType.kanban:
        return Icons.view_column;
      case ProjectViewType.gantt:
        return Icons.view_timeline;
    }
  }

  Future<void> _submitForm(
    BuildContext context,
    ProjectForm formNotifier,
  ) async {
    final result = await formNotifier.submit();

    if (result != null && context.mounted) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            projectId == null
                ? 'Project created successfully'
                : 'Project updated successfully',
          ),
          backgroundColor: AppColors.success,
        ),
      );
      Navigator.of(context).pop();
    }
  }
}
