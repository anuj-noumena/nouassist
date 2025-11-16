import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_next_action_dto.dart';
import '../../../data/models/gtd_context_dto.dart';
import '../../../providers/gtd_next_action_provider.dart';
import '../../../providers/gtd_context_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import 'widgets/next_action_card.dart';
import 'widgets/context_chip.dart';

class NextActionsScreen extends ConsumerStatefulWidget {
  const NextActionsScreen({super.key});

  @override
  ConsumerState<NextActionsScreen> createState() => _NextActionsScreenState();
}

class _NextActionsScreenState extends ConsumerState<NextActionsScreen> {
  bool? _completionFilter;
  bool? _starredFilter;
  List<String>? _contextFilter;

  @override
  Widget build(BuildContext context) {
    final actionsAsync = ref.watch(nextActionListProvider('user-1'));
    final contextsAsync = ref.watch(contextListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Next Actions'),
        actions: [
          IconButton(
            icon: Icon(
              _starredFilter == true ? Icons.star : Icons.star_border,
              color: _starredFilter == true ? AppColors.warning : null,
            ),
            onPressed: () {
              setState(() {
                _starredFilter = _starredFilter == true ? null : true;
              });
              ref
                  .read(nextActionListProvider('user-1').notifier)
                  .filterByStarred(_starredFilter);
            },
          ),
          IconButton(
            icon: const Icon(Icons.filter_list),
            onPressed: () => _showFilterDialog(context, contextsAsync.value ?? []),
          ),
        ],
      ),
      body: Column(
        children: [
          // Filters
          if (_completionFilter != null || _contextFilter != null)
            Container(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                child: Row(
                  children: [
                    if (_completionFilter != null)
                      Chip(
                        label: Text(
                          _completionFilter! ? 'Completed' : 'Active',
                        ),
                        onDeleted: () {
                          setState(() {
                            _completionFilter = null;
                          });
                          ref
                              .read(nextActionListProvider('user-1').notifier)
                              .filterByCompletion(null);
                        },
                      ),
                    if (_contextFilter != null) ...[
                      const SizedBox(width: AppSpacing.sm),
                      ...(_contextFilter!.map((contextId) {
                        final context = (contextsAsync.value ?? [])
                            .where((c) => c.id == contextId)
                            .firstOrNull;
                        return Padding(
                          padding: const EdgeInsets.only(right: AppSpacing.sm),
                          child: Chip(
                            label: Text(context?.name ?? 'Context'),
                            onDeleted: () {
                              setState(() {
                                _contextFilter!.remove(contextId);
                                if (_contextFilter!.isEmpty) {
                                  _contextFilter = null;
                                }
                              });
                              ref
                                  .read(nextActionListProvider('user-1').notifier)
                                  .filterByContexts(_contextFilter);
                            },
                          ),
                        );
                      }).toList()),
                    ],
                    TextButton(
                      onPressed: () {
                        setState(() {
                          _completionFilter = null;
                          _contextFilter = null;
                        });
                        ref
                            .read(nextActionListProvider('user-1').notifier)
                            .refresh();
                      },
                      child: const Text('Clear all'),
                    ),
                  ],
                ),
              ),
            ),

          // Actions list
          Expanded(
            child: actionsAsync.when(
              data: (actions) {
                if (actions.isEmpty) {
                  return Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.check_circle_outline,
                          size: 80,
                          color: AppColors.textDisabled,
                        ),
                        const SizedBox(height: AppSpacing.lg),
                        AppText(
                          'No actions yet!',
                          variant: TextVariant.h5,
                          color: AppColors.textSecondary,
                        ),
                        const SizedBox(height: AppSpacing.sm),
                        AppText(
                          'Add your first next action',
                          variant: TextVariant.bodyMedium,
                          color: AppColors.textSecondary,
                        ),
                      ],
                    ),
                  );
                }

                return ListView.builder(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  itemCount: actions.length,
                  itemBuilder: (context, index) {
                    final action = actions[index];
                    return NextActionCard(
                      action: action,
                      contexts: contextsAsync.value ?? [],
                      onTap: () => _showActionDetails(context, action),
                      onToggleComplete: () => _toggleCompletion(action.id),
                      onToggleStar: () => _toggleStar(action.id),
                      onDelete: () => _deleteAction(context, action.id),
                    );
                  },
                );
              },
              loading: () => const AppLoadingIndicator(),
              error: (error, _) => Center(child: Text('Error: $error')),
            ),
          ),
        ],
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => _showAddDialog(context, contextsAsync.value ?? []),
        child: const Icon(Icons.add),
      ),
    );
  }

  void _showFilterDialog(BuildContext context, List<GtdContextDto> contexts) {
    showModalBottomSheet(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Padding(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: AppText(
                'Filter Actions',
                variant: TextVariant.h5,
                fontWeight: FontWeight.bold,
              ),
            ),
            const Divider(),
            ListTile(
              title: const Text('Status'),
              subtitle: Text(
                _completionFilter == null
                    ? 'All'
                    : _completionFilter!
                        ? 'Completed'
                        : 'Active',
              ),
              trailing: const Icon(Icons.arrow_forward_ios, size: 16),
              onTap: () {
                showDialog(
                  context: context,
                  builder: (context) => AlertDialog(
                    title: const Text('Filter by Status'),
                    content: Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        ListTile(
                          title: const Text('All'),
                          trailing: _completionFilter == null
                              ? const Icon(Icons.check, color: AppColors.primary)
                              : null,
                          onTap: () {
                            this.setState(() {
                              _completionFilter = null;
                            });
                            ref
                                .read(nextActionListProvider('user-1').notifier)
                                .filterByCompletion(null);
                            Navigator.pop(context);
                            Navigator.pop(this.context);
                          },
                        ),
                        ListTile(
                          title: const Text('Active'),
                          trailing: _completionFilter == false
                              ? const Icon(Icons.check, color: AppColors.primary)
                              : null,
                          onTap: () {
                            this.setState(() {
                              _completionFilter = false;
                            });
                            ref
                                .read(nextActionListProvider('user-1').notifier)
                                .filterByCompletion(false);
                            Navigator.pop(context);
                            Navigator.pop(this.context);
                          },
                        ),
                        ListTile(
                          title: const Text('Completed'),
                          trailing: _completionFilter == true
                              ? const Icon(Icons.check, color: AppColors.primary)
                              : null,
                          onTap: () {
                            this.setState(() {
                              _completionFilter = true;
                            });
                            ref
                                .read(nextActionListProvider('user-1').notifier)
                                .filterByCompletion(true);
                            Navigator.pop(context);
                            Navigator.pop(this.context);
                          },
                        ),
                      ],
                    ),
                  ),
                );
              },
            ),
            ListTile(
              title: const Text('Contexts'),
              subtitle: Text(
                _contextFilter == null
                    ? 'All'
                    : '${_contextFilter!.length} selected',
              ),
              trailing: const Icon(Icons.arrow_forward_ios, size: 16),
              onTap: () {
                showDialog(
                  context: context,
                  builder: (context) => AlertDialog(
                    title: const Text('Filter by Context'),
                    content: SingleChildScrollView(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: contexts.map((ctx) {
                          final isSelected =
                              _contextFilter?.contains(ctx.id) ?? false;
                          return CheckboxListTile(
                            title: Text(ctx.name),
                            value: isSelected,
                            onChanged: (value) {
                              this.setState(() {
                                if (value == true) {
                                  _contextFilter ??= [];
                                  _contextFilter!.add(ctx.id);
                                } else {
                                  _contextFilter?.remove(ctx.id);
                                  if (_contextFilter?.isEmpty ?? false) {
                                    _contextFilter = null;
                                  }
                                }
                              });
                              ref
                                  .read(nextActionListProvider('user-1').notifier)
                                  .filterByContexts(_contextFilter);
                            },
                          );
                        }).toList(),
                      ),
                    ),
                    actions: [
                      TextButton(
                        onPressed: () => Navigator.pop(context),
                        child: const Text('Done'),
                      ),
                    ],
                  ),
                );
              },
            ),
            const SizedBox(height: AppSpacing.md),
          ],
        ),
      ),
    );
  }

  void _showAddDialog(BuildContext context, List<GtdContextDto> contexts) {
    final actionController = TextEditingController();
    final notesController = TextEditingController();
    List<String> selectedContexts = [];
    EnergyLevel selectedEnergy = EnergyLevel.medium;
    TimeRequired selectedTime = TimeRequired.medium;
    DateTime? dueDate;

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('New Next Action'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: actionController,
                  decoration: const InputDecoration(
                    labelText: 'Action *',
                    hintText: 'What is the next physical action?',
                    border: OutlineInputBorder(),
                  ),
                  autofocus: true,
                ),
                const SizedBox(height: AppSpacing.md),
                TextField(
                  controller: notesController,
                  maxLines: 2,
                  decoration: const InputDecoration(
                    labelText: 'Notes',
                    border: OutlineInputBorder(),
                  ),
                ),
                const SizedBox(height: AppSpacing.md),
                DropdownButtonFormField<EnergyLevel>(
                  value: selectedEnergy,
                  decoration: const InputDecoration(
                    labelText: 'Energy Required',
                    border: OutlineInputBorder(),
                  ),
                  items: EnergyLevel.values.map((energy) {
                    return DropdownMenuItem(
                      value: energy,
                      child: Text(energy.displayName),
                    );
                  }).toList(),
                  onChanged: (value) {
                    if (value != null) {
                      setState(() {
                        selectedEnergy = value;
                      });
                    }
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                DropdownButtonFormField<TimeRequired>(
                  value: selectedTime,
                  decoration: const InputDecoration(
                    labelText: 'Time Required',
                    border: OutlineInputBorder(),
                  ),
                  items: TimeRequired.values.map((time) {
                    return DropdownMenuItem(
                      value: time,
                      child: Text(time.displayName),
                    );
                  }).toList(),
                  onChanged: (value) {
                    if (value != null) {
                      setState(() {
                        selectedTime = value;
                      });
                    }
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                ListTile(
                  title: const Text('Due Date'),
                  subtitle: Text(
                    dueDate == null
                        ? 'Not set'
                        : '${dueDate!.day}/${dueDate!.month}/${dueDate!.year}',
                  ),
                  trailing: const Icon(Icons.calendar_today),
                  onTap: () async {
                    final date = await showDatePicker(
                      context: context,
                      initialDate: DateTime.now(),
                      firstDate: DateTime.now(),
                      lastDate: DateTime.now().add(const Duration(days: 365)),
                    );
                    if (date != null) {
                      setState(() {
                        dueDate = date;
                      });
                    }
                  },
                ),
                const SizedBox(height: AppSpacing.md),
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    'Contexts',
                    style: Theme.of(context).textTheme.titleSmall,
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.sm,
                  children: contexts.map((ctx) {
                    final isSelected = selectedContexts.contains(ctx.id);
                    return FilterChip(
                      label: Text(ctx.name),
                      selected: isSelected,
                      onSelected: (selected) {
                        setState(() {
                          if (selected) {
                            selectedContexts.add(ctx.id);
                          } else {
                            selectedContexts.remove(ctx.id);
                          }
                        });
                      },
                    );
                  }).toList(),
                ),
              ],
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Cancel'),
            ),
            TextButton(
              onPressed: () async {
                if (actionController.text.isNotEmpty) {
                  await ref.read(
                    createNextActionProvider(
                      CreateUpdateNextActionDto(
                        userId: 'user-1',
                        action: actionController.text,
                        notes: notesController.text.isEmpty
                            ? null
                            : notesController.text,
                        contextIds: selectedContexts,
                        energyLevel: selectedEnergy,
                        timeRequired: selectedTime,
                        dueDate: dueDate,
                      ),
                    ).future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Action added'),
                        backgroundColor: AppColors.success,
                      ),
                    );
                  }
                }
              },
              child: const Text('Add'),
            ),
          ],
        ),
      ),
    );
  }

  void _showActionDetails(BuildContext context, GtdNextActionDto action) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(AppSpacing.radiusMd)),
      ),
      builder: (context) => DraggableScrollableSheet(
        initialChildSize: 0.7,
        minChildSize: 0.5,
        maxChildSize: 0.95,
        expand: false,
        builder: (context, scrollController) => SingleChildScrollView(
          controller: scrollController,
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Expanded(
                    child: AppText(
                      action.action,
                      variant: TextVariant.h5,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  IconButton(
                    icon: Icon(
                      action.isStarred ? Icons.star : Icons.star_border,
                      color: action.isStarred ? AppColors.warning : null,
                    ),
                    onPressed: () {
                      _toggleStar(action.id);
                      Navigator.pop(context);
                    },
                  ),
                ],
              ),
              const SizedBox(height: AppSpacing.md),
              if (action.notes != null) ...[
                AppText(
                  'Notes',
                  variant: TextVariant.bodyMedium,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  action.notes!,
                  variant: TextVariant.bodyMedium,
                  color: AppColors.textSecondary,
                ),
                const SizedBox(height: AppSpacing.md),
              ],
              _buildDetailRow('Energy', action.energyLevel.displayName),
              _buildDetailRow('Time', action.timeRequired.displayName),
              if (action.dueDate != null)
                _buildDetailRow(
                  'Due Date',
                  '${action.dueDate!.day}/${action.dueDate!.month}/${action.dueDate!.year}',
                ),
              if (action.scheduledDate != null)
                _buildDetailRow(
                  'Scheduled',
                  '${action.scheduledDate!.day}/${action.scheduledDate!.month}/${action.scheduledDate!.year}',
                ),
              const SizedBox(height: AppSpacing.md),
              if (action.contextIds.isNotEmpty) ...[
                AppText(
                  'Contexts',
                  variant: TextVariant.bodyMedium,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.sm),
                Consumer(
                  builder: (context, ref, _) {
                    final contextsAsync =
                        ref.watch(contextListProvider('user-1'));
                    return contextsAsync.when(
                      data: (contexts) {
                        final actionContexts = contexts
                            .where((c) => action.contextIds.contains(c.id))
                            .toList();
                        return Wrap(
                          spacing: AppSpacing.sm,
                          runSpacing: AppSpacing.sm,
                          children: actionContexts
                              .map((ctx) => ContextChip(context: ctx))
                              .toList(),
                        );
                      },
                      loading: () => const SizedBox.shrink(),
                      error: (_, __) => const SizedBox.shrink(),
                    );
                  },
                ),
              ],
              const SizedBox(height: AppSpacing.xl),
              Row(
                children: [
                  Expanded(
                    child: AppButton(
                      label: action.isCompleted
                          ? 'Mark Incomplete'
                          : 'Complete',
                      variant: action.isCompleted
                          ? ButtonVariant.outlined
                          : ButtonVariant.primary,
                      icon: action.isCompleted
                          ? Icons.remove_circle_outline
                          : Icons.check_circle,
                      onPressed: () {
                        _toggleCompletion(action.id);
                        Navigator.pop(context);
                      },
                    ),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  IconButton(
                    icon: const Icon(Icons.delete_outline),
                    color: AppColors.error,
                    onPressed: () {
                      Navigator.pop(context);
                      _deleteAction(context, action.id);
                    },
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildDetailRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: Row(
        children: [
          SizedBox(
            width: 100,
            child: AppText(
              label,
              variant: TextVariant.bodyMedium,
              color: AppColors.textSecondary,
            ),
          ),
          Expanded(
            child: AppText(
              value,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w600,
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _toggleCompletion(String id) async {
    await ref.read(toggleActionCompletionProvider(id, 'user-1').future);
  }

  Future<void> _toggleStar(String id) async {
    await ref.read(toggleActionStarProvider(id, 'user-1').future);
  }

  Future<void> _deleteAction(BuildContext context, String id) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Action'),
        content: const Text('Are you sure you want to delete this action?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            style: TextButton.styleFrom(foregroundColor: AppColors.error),
            child: const Text('Delete'),
          ),
        ],
      ),
    );

    if (confirmed == true) {
      await ref.read(deleteNextActionProvider(id, 'user-1').future);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Action deleted')),
        );
      }
    }
  }
}
