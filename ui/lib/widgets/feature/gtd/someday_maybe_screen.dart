import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_someday_maybe_dto.dart';
import '../../../providers/gtd_someday_maybe_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';

class SomedayMaybeScreen extends ConsumerStatefulWidget {
  const SomedayMaybeScreen({super.key});

  @override
  ConsumerState<SomedayMaybeScreen> createState() => _SomedayMaybeScreenState();
}

class _SomedayMaybeScreenState extends ConsumerState<SomedayMaybeScreen> {
  SomedayMaybeCategory? _categoryFilter;

  @override
  Widget build(BuildContext context) {
    final itemsAsync = ref.watch(somedayMaybeListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Someday/Maybe'),
        actions: [
          IconButton(
            icon: const Icon(Icons.filter_list),
            onPressed: _showFilterDialog,
          ),
        ],
      ),
      body: Column(
        children: [
          if (_categoryFilter != null)
            Container(
              padding: const EdgeInsets.all(AppSpacing.md),
              child: Row(
                children: [
                  Chip(
                    label: Text(_categoryFilter!.displayName),
                    onDeleted: () {
                      setState(() {
                        _categoryFilter = null;
                      });
                      ref.read(somedayMaybeListProvider('user-1').notifier).refresh();
                    },
                  ),
                ],
              ),
            ),
          Expanded(
            child: itemsAsync.when(
              data: (items) {
                if (items.isEmpty) {
                  return Center(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          Icons.lightbulb_outline,
                          size: 80,
                          color: AppColors.textDisabled,
                        ),
                        const SizedBox(height: AppSpacing.lg),
                        AppText(
                          'No ideas yet!',
                          variant: TextVariant.h5,
                          color: AppColors.textSecondary,
                        ),
                        const SizedBox(height: AppSpacing.sm),
                        AppText(
                          'Capture ideas for someday',
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
                    return _buildSomedayMaybeCard(item);
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

  Widget _buildSomedayMaybeCard(GtdSomedayMaybeDto item) {
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
                      item.title,
                      variant: TextVariant.bodyLarge,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  _buildCategoryChip(item.category),
                ],
              ),
              if (item.description != null) ...[
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  item.description!,
                  variant: TextVariant.bodyMedium,
                  color: AppColors.textSecondary,
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],
              if (item.isActivated) ...[
                const SizedBox(height: AppSpacing.sm),
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: AppSpacing.sm,
                    vertical: AppSpacing.xs,
                  ),
                  decoration: BoxDecoration(
                    color: AppColors.success.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      const Icon(
                        Icons.check_circle,
                        size: 14,
                        color: AppColors.success,
                      ),
                      const SizedBox(width: AppSpacing.xs),
                      Text(
                        'Activated on ${item.activatedDate!.day}/${item.activatedDate!.month}/${item.activatedDate!.year}',
                        style: const TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.w600,
                          color: AppColors.success,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildCategoryChip(SomedayMaybeCategory category) {
    return Container(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.sm,
        vertical: AppSpacing.xs,
      ),
      decoration: BoxDecoration(
        color: AppColors.info.withOpacity(0.1),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Text(
        category.displayName,
        style: const TextStyle(
          fontSize: 12,
          fontWeight: FontWeight.w600,
          color: AppColors.info,
        ),
      ),
    );
  }

  void _showFilterDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Filter by Category'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            ListTile(
              title: const Text('All'),
              trailing: _categoryFilter == null
                  ? const Icon(Icons.check, color: AppColors.primary)
                  : null,
              onTap: () {
                setState(() {
                  _categoryFilter = null;
                });
                ref.read(somedayMaybeListProvider('user-1').notifier).refresh();
                Navigator.pop(context);
              },
            ),
            ...SomedayMaybeCategory.values.map((category) {
              return ListTile(
                title: Text(category.displayName),
                trailing: _categoryFilter == category
                    ? const Icon(Icons.check, color: AppColors.primary)
                    : null,
                onTap: () {
                  setState(() {
                    _categoryFilter = category;
                  });
                  ref
                      .read(somedayMaybeListProvider('user-1').notifier)
                      .filterByCategory(category);
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
    final titleController = TextEditingController();
    final descriptionController = TextEditingController();
    SomedayMaybeCategory selectedCategory = SomedayMaybeCategory.other;

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('New Someday/Maybe'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: titleController,
                  decoration: const InputDecoration(
                    labelText: 'Title *',
                    hintText: 'What would you like to do someday?',
                    border: OutlineInputBorder(),
                  ),
                  autofocus: true,
                ),
                const SizedBox(height: AppSpacing.md),
                TextField(
                  controller: descriptionController,
                  maxLines: 3,
                  decoration: const InputDecoration(
                    labelText: 'Description',
                    border: OutlineInputBorder(),
                  ),
                ),
                const SizedBox(height: AppSpacing.md),
                DropdownButtonFormField<SomedayMaybeCategory>(
                  value: selectedCategory,
                  decoration: const InputDecoration(
                    labelText: 'Category',
                    border: OutlineInputBorder(),
                  ),
                  items: SomedayMaybeCategory.values.map((category) {
                    return DropdownMenuItem(
                      value: category,
                      child: Text(category.displayName),
                    );
                  }).toList(),
                  onChanged: (value) {
                    if (value != null) {
                      setState(() {
                        selectedCategory = value;
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
                if (titleController.text.isNotEmpty) {
                  await ref.read(
                    createSomedayMaybeProvider(
                      CreateUpdateSomedayMaybeDto(
                        userId: 'user-1',
                        title: titleController.text,
                        description: descriptionController.text.isEmpty
                            ? null
                            : descriptionController.text,
                        category: selectedCategory,
                      ),
                    ).future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Item added'),
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

  void _showItemDetails(GtdSomedayMaybeDto item) {
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
                      item.title,
                      variant: TextVariant.h5,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  _buildCategoryChip(item.category),
                ],
              ),
              const SizedBox(height: AppSpacing.lg),
              if (item.description != null) ...[
                AppText(
                  'Description',
                  variant: TextVariant.bodyMedium,
                  fontWeight: FontWeight.w600,
                ),
                const SizedBox(height: AppSpacing.sm),
                AppText(
                  item.description!,
                  variant: TextVariant.bodyMedium,
                  color: AppColors.textSecondary,
                ),
                const SizedBox(height: AppSpacing.lg),
              ],
              if (item.isActivated) ...[
                Container(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  decoration: BoxDecoration(
                    color: AppColors.success.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                    border: Border.all(color: AppColors.success),
                  ),
                  child: Row(
                    children: [
                      const Icon(Icons.check_circle, color: AppColors.success),
                      const SizedBox(width: AppSpacing.sm),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const Text(
                              'Activated',
                              style: TextStyle(
                                fontWeight: FontWeight.w600,
                                color: AppColors.success,
                              ),
                            ),
                            Text(
                              'Converted to project on ${item.activatedDate!.day}/${item.activatedDate!.month}/${item.activatedDate!.year}',
                              style: const TextStyle(
                                fontSize: 12,
                                color: AppColors.success,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: AppSpacing.lg),
              ],
              if (!item.isActivated)
                Row(
                  children: [
                    Expanded(
                      child: ElevatedButton.icon(
                        onPressed: () async {
                          await ref.read(
                            activateSomedayMaybeProvider(
                              item.id,
                              'new-project-id',
                              'user-1',
                            ).future,
                          );
                          if (context.mounted) {
                            Navigator.pop(context);
                            ScaffoldMessenger.of(context).showSnackBar(
                              const SnackBar(
                                content: Text('Activated as project'),
                                backgroundColor: AppColors.success,
                              ),
                            );
                          }
                        },
                        icon: const Icon(Icons.play_arrow),
                        label: const Text('Activate Now'),
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
      await ref.read(deleteSomedayMaybeProvider(id, 'user-1').future);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Item deleted')),
        );
      }
    }
  }
}
