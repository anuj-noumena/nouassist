import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_waiting_for_dto.dart';
import '../../../providers/gtd_waiting_for_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';

class WaitingForScreen extends ConsumerStatefulWidget {
  const WaitingForScreen({super.key});

  @override
  ConsumerState<WaitingForScreen> createState() => _WaitingForScreenState();
}

class _WaitingForScreenState extends ConsumerState<WaitingForScreen> {
  WaitingForStatus? _statusFilter;

  @override
  Widget build(BuildContext context) {
    final waitingForAsync = ref.watch(waitingForListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Waiting For'),
        actions: [
          IconButton(
            icon: const Icon(Icons.filter_list),
            onPressed: _showFilterDialog,
          ),
        ],
      ),
      body: Column(
        children: [
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
                      ref.read(waitingForListProvider('user-1').notifier).refresh();
                    },
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  TextButton(
                    onPressed: () {
                      setState(() {
                        _statusFilter = null;
                      });
                      ref.read(waitingForListProvider('user-1').notifier).refresh();
                    },
                    child: const Text('Clear filters'),
                  ),
                ],
              ),
            ),
          Expanded(
            child: waitingForAsync.when(
              data: (items) {
                if (items.isEmpty) {
                  return Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.hourglass_empty,
                          size: 80,
                          color: AppColors.textDisabled,
                        ),
                        const SizedBox(height: AppSpacing.lg),
                        AppText(
                          'Nothing waiting!',
                          variant: TextVariant.h5,
                          color: AppColors.textSecondary,
                        ),
                        const SizedBox(height: AppSpacing.sm),
                        AppText(
                          'Track things you\'re waiting on',
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
                    return _buildWaitingForCard(item);
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

  Widget _buildWaitingForCard(GtdWaitingForDto item) {
    final isOverdue = item.expectedDate != null &&
        item.expectedDate!.isBefore(DateTime.now()) &&
        item.status == WaitingForStatus.waiting;

    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: InkWell(
        onTap: () => _showItemDetails(item),
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Expanded(
                    child: AppText(
                      item.description,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  _buildStatusChip(item.status),
                ],
              ),
              const SizedBox(height: AppSpacing.sm),
              Row(
                children: [
                  Icon(
                    Icons.person_outline,
                    size: 16,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(width: AppSpacing.xs),
                  Expanded(
                    child: AppText(
                      'Waiting for: ${item.waitingFor}',
                      variant: TextVariant.bodyMedium,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
              if (item.expectedDate != null) ...[
                const SizedBox(height: AppSpacing.sm),
                Row(
                  children: [
                    Icon(
                      Icons.calendar_today,
                      size: 16,
                      color: isOverdue ? AppColors.error : AppColors.textSecondary,
                    ),
                    const SizedBox(width: AppSpacing.xs),
                    AppText(
                      'Expected: ${item.expectedDate!.day}/${item.expectedDate!.month}/${item.expectedDate!.year}',
                      variant: TextVariant.bodySmall,
                      color: isOverdue ? AppColors.error : AppColors.textSecondary,
                      fontWeight: isOverdue ? FontWeight.w600 : FontWeight.normal,
                    ),
                    if (isOverdue) ...[
                      const SizedBox(width: AppSpacing.xs),
                      Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: AppSpacing.xs,
                          vertical: 2,
                        ),
                        decoration: BoxDecoration(
                          color: AppColors.error.withOpacity(0.1),
                          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                        ),
                        child: const Text(
                          'OVERDUE',
                          style: TextStyle(
                            fontSize: 10,
                            fontWeight: FontWeight.bold,
                            color: AppColors.error,
                          ),
                        ),
                      ),
                    ],
                  ],
                ),
              ],
              if (item.notes != null) ...[
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  item.notes!,
                  variant: TextVariant.bodySmall,
                  color: AppColors.textSecondary,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildStatusChip(WaitingForStatus status) {
    Color color;
    switch (status) {
      case WaitingForStatus.waiting:
        color = AppColors.warning;
        break;
      case WaitingForStatus.reminded:
        color = AppColors.info;
        break;
      case WaitingForStatus.received:
        color = AppColors.success;
        break;
      case WaitingForStatus.cancelled:
        color = AppColors.textSecondary;
        break;
    }

    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: color.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        status.displayName,
        style: TextStyle(
          fontSize: 12,
          fontWeight: FontWeight.w600,
          color: color,
        ),
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
                ref.read(waitingForListProvider('user-1').notifier).refresh();
                Navigator.pop(context);
              },
            ),
            ...WaitingForStatus.values.map((status) {
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
                      .read(waitingForListProvider('user-1').notifier)
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
    final descriptionController = TextEditingController();
    final waitingForController = TextEditingController();
    final notesController = TextEditingController();
    DateTime? expectedDate;

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('New Waiting For'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: descriptionController,
                  decoration: const InputDecoration(
                    labelText: 'Description *',
                    hintText: 'What are you waiting for?',
                    border: OutlineInputBorder(),
                  ),
                  autofocus: true,
                ),
                const SizedBox(height: AppSpacing.md),
                TextField(
                  controller: waitingForController,
                  decoration: const InputDecoration(
                    labelText: 'Waiting For *',
                    hintText: 'Person or department',
                    border: OutlineInputBorder(),
                  ),
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
                ListTile(
                  title: const Text('Expected Date'),
                  subtitle: Text(
                    expectedDate == null
                        ? 'Not set'
                        : '${expectedDate!.day}/${expectedDate!.month}/${expectedDate!.year}',
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
                        expectedDate = date;
                      });
                    }
                  },
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
                if (descriptionController.text.isNotEmpty &&
                    waitingForController.text.isNotEmpty) {
                  await ref.read(
                    createWaitingForProvider(
                      CreateUpdateWaitingForDto(
                        userId: 'user-1',
                        description: descriptionController.text,
                        waitingFor: waitingForController.text,
                        notes: notesController.text.isEmpty
                            ? null
                            : notesController.text,
                        expectedDate: expectedDate,
                      ),
                    ).future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Waiting for item added'),
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

  void _showItemDetails(GtdWaitingForDto item) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(AppSpacing.radiusMd)),
      ),
      builder: (context) => DraggableScrollableSheet(
        initialChildSize: 0.6,
        minChildSize: 0.4,
        maxChildSize: 0.9,
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
                      item.description,
                      variant: TextVariant.h5,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  _buildStatusChip(item.status),
                ],
              ),
              const SizedBox(height: AppSpacing.lg),
              _buildDetailRow('Waiting For', item.waitingFor),
              if (item.requestedDate != null)
                _buildDetailRow(
                  'Requested',
                  '${item.requestedDate!.day}/${item.requestedDate!.month}/${item.requestedDate!.year}',
                ),
              if (item.expectedDate != null)
                _buildDetailRow(
                  'Expected',
                  '${item.expectedDate!.day}/${item.expectedDate!.month}/${item.expectedDate!.year}',
                ),
              if (item.receivedDate != null)
                _buildDetailRow(
                  'Received',
                  '${item.receivedDate!.day}/${item.receivedDate!.month}/${item.receivedDate!.year}',
                ),
              if (item.notes != null) ...[
                const SizedBox(height: AppSpacing.md),
                AppText(
                  'Notes',
                  variant: TextVariant.bodyMedium,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  item.notes!,
                  variant: TextVariant.bodyMedium,
                  color: AppColors.textSecondary,
                ),
              ],
              const SizedBox(height: AppSpacing.xl),
              if (item.status == WaitingForStatus.waiting)
                Row(
                  children: [
                    Expanded(
                      child: ElevatedButton.icon(
                        onPressed: () async {
                          await ref.read(
                            markWaitingForReceivedProvider(item.id, 'user-1').future,
                          );
                          if (context.mounted) {
                            Navigator.pop(context);
                            ScaffoldMessenger.of(context).showSnackBar(
                              const SnackBar(
                                content: Text('Marked as received'),
                                backgroundColor: AppColors.success,
                              ),
                            );
                          }
                        },
                        icon: const Icon(Icons.check_circle),
                        label: const Text('Mark Received'),
                      ),
                    ),
                    const SizedBox(width: AppSpacing.sm),
                    IconButton(
                      icon: const Icon(Icons.delete_outline),
                      color: AppColors.error,
                      onPressed: () {
                        Navigator.pop(context);
                        _deleteItem(item.id);
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
      await ref.read(deleteWaitingForProvider(id, 'user-1').future);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Item deleted')),
        );
      }
    }
  }
}
