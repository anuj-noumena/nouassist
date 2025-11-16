import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_inbox_dto.dart';
import '../../../providers/gtd_inbox_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import 'widgets/inbox_item_card.dart';

class InboxScreen extends ConsumerStatefulWidget {
  const InboxScreen({super.key});

  @override
  ConsumerState<InboxScreen> createState() => _InboxScreenState();
}

class _InboxScreenState extends ConsumerState<InboxScreen> {
  InboxStatus? _statusFilter;

  @override
  Widget build(BuildContext context) {
    final inboxAsync = ref.watch(inboxListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Inbox'),
        actions: [
          IconButton(
            icon: const Icon(Icons.filter_list),
            onPressed: _showFilterDialog,
          ),
        ],
      ),
      body: Column(
        children: [
          // Filter chips
          if (_statusFilter != null)
            Container(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: Row(
                children: [
                  Chip(
                    label: Text(_statusFilter!.displayName),
                    onDeleted: () {
                      setState(() {
                        _statusFilter = null;
                      });
                      ref.read(inboxListProvider('user-1').notifier).refresh();
                    },
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  TextButton(
                    onPressed: () {
                      setState(() {
                        _statusFilter = null;
                      });
                      ref.read(inboxListProvider('user-1').notifier).refresh();
                    },
                    child: const Text('Clear filters'),
                  ),
                ],
              ),
            ),

          // Inbox list
          Expanded(
            child: inboxAsync.when(
              data: (items) {
                if (items.isEmpty) {
                  return Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.inbox,
                          size: 80,
                          color: AppColors.textDisabled,
                        ),
                        const SizedBox(height: AppSpacing.lg),
                        AppText(
                          'Inbox is empty!',
                          variant: TextVariant.h5,
                          color: AppColors.textSecondary,
                        ),
                        const SizedBox(height: AppSpacing.sm),
                        AppText(
                          'Your mind is clear 🎉',
                          variant: TextVariant.bodyMedium,
                          color: AppColors.textSecondary,
                        ),
                      ],
                    ),
                  );
                }

                return ListView.builder(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  itemCount: items.length,
                  itemBuilder: (context, index) {
                    final item = items[index];
                    return InboxItemCard(
                      item: item,
                      onTap: () => _processItem(item),
                      onDelete: () => _deleteItem(item.id),
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
        onPressed: _showAddDialog,
        child: const Icon(Icons.add),
      ),
    );
  }

  void _showFilterDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Filter by Status'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            ListTile(
              title: const Text('All'),
              trailing: _statusFilter == null
                  ? const Icon(Icons.check, color: AppColors.primary)
                  : null,
              onTap: () {
                setState(() {
                  _statusFilter = null;
                });
                ref.read(inboxListProvider('user-1').notifier).refresh();
                Navigator.pop(context);
              },
            ),
            ...InboxStatus.values.map((status) {
              return ListTile(
                title: Text(status.displayName),
                trailing: _statusFilter == status
                    ? const Icon(Icons.check, color: AppColors.primary)
                    : null,
                onTap: () {
                  setState(() {
                    _statusFilter = status;
                  });
                  ref
                      .read(inboxListProvider('user-1').notifier)
                      .filterByStatus(status);
                  Navigator.pop(context);
                },
              );
            }).toList(),
          ],
        ),
      ),
    );
  }

  void _showAddDialog() {
    final contentController = TextEditingController();
    InboxItemType selectedType = InboxItemType.thought;

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('Add to Inbox'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: contentController,
                maxLines: 3,
                decoration: const InputDecoration(
                  labelText: 'Content',
                  hintText: 'What needs your attention?',
                  border: OutlineInputBorder(),
                ),
              ),
              const SizedBox(height: AppSpacing.md),
              DropdownButtonFormField<InboxItemType>(
                value: selectedType,
                decoration: const InputDecoration(
                  labelText: 'Type',
                  border: OutlineInputBorder(),
                ),
                items: InboxItemType.values.map((type) {
                  return DropdownMenuItem(
                    value: type,
                    child: Text(type.displayName),
                  );
                }).toList(),
                onChanged: (value) {
                  if (value != null) {
                    setState(() {
                      selectedType = value;
                    });
                  }
                },
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Cancel'),
            ),
            TextButton(
              onPressed: () async {
                if (contentController.text.isNotEmpty) {
                  await ref.read(
                    createInboxItemProvider(
                      CreateUpdateInboxDto(
                        userId: 'user-1',
                        content: contentController.text,
                        itemType: selectedType,
                      ),
                    ).future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Added to inbox'),
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

  void _processItem(GtdInboxDto item) {
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
              AppText(
                'Process Item',
                variant: TextVariant.h4,
                fontWeight: FontWeight.bold,
              ),
              const SizedBox(height: AppSpacing.md),
              Container(
                padding: const EdgeInsets.all(AppSpacing.md),
                decoration: BoxDecoration(
                  color: AppColors.surfaceVariant,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    AppText(
                      item.content,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                    ),
                    if (item.notes != null) ...[
                      const SizedBox(height: AppSpacing.sm),
                      AppText(
                        item.notes!,
                        variant: TextVariant.bodyMedium,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ],
                ),
              ),
              const SizedBox(height: AppSpacing.xl),
              AppText(
                'What is it?',
                variant: TextVariant.h6,
                fontWeight: FontWeight.w600,
              ),
              const SizedBox(height: AppSpacing.md),
              _buildProcessButton(
                context,
                'Next Action',
                'It\'s actionable and takes one step',
                Icons.check_circle_outline,
                () async {
                  await ref.read(
                    markInboxItemProcessedProvider(item.id, 'Next Action').future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Moved to Next Actions')),
                    );
                  }
                },
              ),
              _buildProcessButton(
                context,
                'Project',
                'It requires multiple steps',
                Icons.folder_outlined,
                () async {
                  await ref.read(
                    markInboxItemProcessedProvider(item.id, 'Project').future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Converted to Project')),
                    );
                  }
                },
              ),
              _buildProcessButton(
                context,
                'Waiting For',
                'Delegated or waiting on someone',
                Icons.hourglass_empty,
                () async {
                  await ref.read(
                    markInboxItemProcessedProvider(item.id, 'Waiting For').future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Moved to Waiting For')),
                    );
                  }
                },
              ),
              _buildProcessButton(
                context,
                'Someday/Maybe',
                'Not actionable now, but maybe later',
                Icons.lightbulb_outline,
                () async {
                  await ref.read(
                    markInboxItemProcessedProvider(item.id, 'Someday/Maybe').future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Moved to Someday/Maybe')),
                    );
                  }
                },
              ),
              _buildProcessButton(
                context,
                'Reference',
                'Information to keep for later',
                Icons.bookmark_outline,
                () async {
                  await ref.read(
                    markInboxItemProcessedProvider(item.id, 'Reference').future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Saved as Reference')),
                    );
                  }
                },
              ),
              _buildProcessButton(
                context,
                'Trash',
                'Not needed anymore',
                Icons.delete_outline,
                () async {
                  await _deleteItem(item.id);
                  if (context.mounted) {
                    Navigator.pop(context);
                  }
                },
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildProcessButton(
    BuildContext context,
    String title,
    String description,
    IconData icon,
    VoidCallback onTap,
  ) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.sm),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Row(
            children: [
              Icon(icon, color: AppColors.primary, size: 28),
              const SizedBox(width: AppSpacing.md),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    AppText(
                      title,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                    ),
                    AppText(
                      description,
                      variant: TextVariant.bodySmall,
                      color: AppColors.textSecondary,
                    ),
                  ],
                ),
              ),
              Icon(
                Icons.arrow_forward_ios,
                size: 16,
                color: AppColors.textSecondary,
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _deleteItem(String id) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Item'),
        content: const Text('Are you sure you want to delete this item?'),
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
      await ref.read(deleteInboxItemProvider(id).future);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Item deleted')),
        );
      }
    }
  }
}
