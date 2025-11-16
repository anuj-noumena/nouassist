import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/task_checklist_dto.dart';
import '../../../../providers/task_checklist_provider.dart';
import '../../../atomic/app_text.dart';
import '../../../atomic/app_button.dart';
import '../../../atomic/app_form_field.dart';

class TaskChecklistWidget extends ConsumerStatefulWidget {
  final String taskId;

  const TaskChecklistWidget({super.key, required this.taskId});

  @override
  ConsumerState<TaskChecklistWidget> createState() => _TaskChecklistWidgetState();
}

class _TaskChecklistWidgetState extends ConsumerState<TaskChecklistWidget> {
  final _titleController = TextEditingController();
  final _itemController = TextEditingController();
  final List<ChecklistItemDto> _items = [];
  bool _isAddingChecklist = false;

  @override
  void dispose() {
    _titleController.dispose();
    _itemController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final checklistsAsync = ref.watch(taskChecklistsProvider(widget.taskId));

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            AppText(
              'Checklists',
              variant: TextVariant.h5,
              fontWeight: FontWeight.w600,
            ),
            if (!_isAddingChecklist)
              IconButton(
                icon: const Icon(Icons.add),
                onPressed: () {
                  setState(() {
                    _isAddingChecklist = true;
                  });
                },
              ),
          ],
        ),
        const SizedBox(height: AppSpacing.md),
        if (_isAddingChecklist) _buildChecklistForm(),
        const SizedBox(height: AppSpacing.md),
        checklistsAsync.when(
          data: (checklists) {
            if (checklists.isEmpty && !_isAddingChecklist) {
              return Container(
                padding: const EdgeInsets.all(AppSpacing.lg),
                decoration: BoxDecoration(
                  color: AppColors.surfaceVariant,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                ),
                child: Center(
                  child: AppText(
                    'No checklists yet',
                    variant: TextVariant.bodyMedium,
                    color: AppColors.textSecondary,
                  ),
                ),
              );
            }

            return Column(
              children: checklists.map((checklist) {
                return _buildChecklistCard(checklist);
              }).toList(),
            );
          },
          loading: () => const Center(child: CircularProgressIndicator()),
          error: (error, _) => Text('Error: $error'),
        ),
      ],
    );
  }

  Widget _buildChecklistForm() {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppFormField(
              controller: _titleController,
              label: 'Checklist Title',
              hint: 'Enter checklist title',
            ),
            const SizedBox(height: AppSpacing.md),
            AppText(
              'Items',
              variant: TextVariant.label,
            ),
            const SizedBox(height: AppSpacing.sm),
            if (_items.isNotEmpty) ...[
              ..._items.map((item) {
                return Padding(
                  padding: const EdgeInsets.only(bottom: AppSpacing.xs),
                  child: Row(
                    children: [
                      const Icon(Icons.drag_indicator, size: 16),
                      const SizedBox(width: AppSpacing.xs),
                      Expanded(
                        child: Text(item.text),
                      ),
                      IconButton(
                        icon: const Icon(Icons.close, size: 16),
                        onPressed: () {
                          setState(() {
                            _items.remove(item);
                          });
                        },
                      ),
                    ],
                  ),
                );
              }).toList(),
              const SizedBox(height: AppSpacing.sm),
            ],
            Row(
              children: [
                Expanded(
                  child: AppFormField(
                    controller: _itemController,
                    hint: 'Add item',
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                AppButton(
                  label: 'Add',
                  isFullWidth: false,
                  variant: ButtonVariant.outlined,
                  size: ButtonSize.medium,
                  onPressed: () {
                    if (_itemController.text.isNotEmpty) {
                      setState(() {
                        _items.add(
                          ChecklistItemDto(
                            id: DateTime.now().millisecondsSinceEpoch.toString(),
                            text: _itemController.text,
                            order: _items.length + 1,
                          ),
                        );
                        _itemController.clear();
                      });
                    }
                  },
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppButton(
                    label: 'Cancel',
                    variant: ButtonVariant.outlined,
                    onPressed: () {
                      setState(() {
                        _isAddingChecklist = false;
                        _titleController.clear();
                        _itemController.clear();
                        _items.clear();
                      });
                    },
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppButton(
                    label: 'Create',
                    onPressed: _createChecklist,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildChecklistCard(TaskChecklistDto checklist) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Expanded(
                  child: AppText(
                    checklist.title,
                    variant: TextVariant.bodyLarge,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                AppText(
                  '${checklist.progress.toInt()}%',
                  variant: TextVariant.bodySmall,
                  fontWeight: FontWeight.w600,
                  color: AppColors.primary,
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.sm),
            ClipRRect(
              borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
              child: LinearProgressIndicator(
                value: checklist.progress / 100,
                minHeight: 6,
                backgroundColor: AppColors.border,
                valueColor: const AlwaysStoppedAnimation<Color>(AppColors.primary),
              ),
            ),
            const SizedBox(height: AppSpacing.md),
            ...checklist.items.map((item) {
              return _buildChecklistItem(checklist.id, item);
            }).toList(),
          ],
        ),
      ),
    );
  }

  Widget _buildChecklistItem(String checklistId, ChecklistItemDto item) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.xs),
      child: InkWell(
        onTap: () {
          ref.read(taskChecklistProvider(checklistId).notifier).toggleItem(item.id);
        },
        child: Row(
          children: [
            Checkbox(
              value: item.isCompleted,
              onChanged: (_) {
                ref
                    .read(taskChecklistProvider(checklistId).notifier)
                    .toggleItem(item.id);
              },
            ),
            Expanded(
              child: Text(
                item.text,
                style: TextStyle(
                  decoration:
                      item.isCompleted ? TextDecoration.lineThrough : null,
                  color: item.isCompleted
                      ? AppColors.textSecondary
                      : AppColors.textPrimary,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Future<void> _createChecklist() async {
    if (_titleController.text.isEmpty || _items.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Please add a title and at least one item'),
          backgroundColor: AppColors.error,
        ),
      );
      return;
    }

    try {
      await ref.read(
        createChecklistProvider(
          CreateUpdateChecklistDto(
            taskId: widget.taskId,
            title: _titleController.text,
            items: _items,
          ),
        ).future,
      );

      setState(() {
        _isAddingChecklist = false;
        _titleController.clear();
        _itemController.clear();
        _items.clear();
      });

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Checklist created successfully'),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to create checklist: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }
}
