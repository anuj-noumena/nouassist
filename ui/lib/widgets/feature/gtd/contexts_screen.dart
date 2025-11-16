import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_context_dto.dart';
import '../../../providers/gtd_context_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';

class ContextsScreen extends ConsumerWidget {
  const ContextsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final contextsAsync = ref.watch(contextListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('Contexts'),
      ),
      body: contextsAsync.when(
        data: (contexts) {
          if (contexts.isEmpty) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.label_outline,
                    size: 80,
                    color: AppColors.textDisabled,
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  AppText(
                    'No contexts yet!',
                    variant: TextVariant.h5,
                    color: AppColors.textSecondary,
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  AppText(
                    'Add contexts to organize your actions',
                    variant: TextVariant.bodyMedium,
                    color: AppColors.textSecondary,
                  ),
                ],
              ),
            );
          }

          return ListView.builder(
            padding: const EdgeInsets.all(AppSpacing.md),
            itemCount: contexts.length,
            itemBuilder: (context, index) {
              final ctx = contexts[index];
              return _buildContextCard(context, ref, ctx);
            },
          );
        },
        loading: () => const AppLoadingIndicator(),
        error: (error, _) => Center(child: Text('Error: $error')),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => _showAddDialog(context, ref),
        child: const Icon(Icons.add),
      ),
    );
  }

  Widget _buildContextCard(
    BuildContext context,
    WidgetRef ref,
    GtdContextDto ctx,
  ) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: ListTile(
        leading: Container(
          width: 48,
          height: 48,
          decoration: BoxDecoration(
            color: _parseColor(ctx.color).withOpacity(0.1),
            borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          ),
          child: Icon(
            _parseIcon(ctx.icon),
            color: _parseColor(ctx.color),
          ),
        ),
        title: AppText(
          ctx.name,
          variant: TextVariant.bodyLarge,
          fontWeight: FontWeight.w600,
        ),
        subtitle: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (ctx.description != null) ...[
              const SizedBox(height: AppSpacing.xs),
              AppText(
                ctx.description!,
                variant: TextVariant.bodySmall,
                color: AppColors.textSecondary,
              ),
            ],
            const SizedBox(height: AppSpacing.xs),
            AppText(
              '${ctx.actionCount} actions',
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ],
        ),
        trailing: PopupMenuButton(
          itemBuilder: (context) => [
            const PopupMenuItem(
              value: 'delete',
              child: Row(
                children: [
                  Icon(Icons.delete_outline, color: AppColors.error),
                  SizedBox(width: AppSpacing.sm),
                  Text('Delete'),
                ],
              ),
            ),
          ],
          onSelected: (value) {
            if (value == 'delete') {
              _deleteContext(context, ref, ctx.id);
            }
          },
        ),
      ),
    );
  }

  void _showAddDialog(BuildContext context, WidgetRef ref) {
    final nameController = TextEditingController();
    final descriptionController = TextEditingController();
    ContextType selectedType = ContextType.location;
    String selectedIcon = 'place';
    String selectedColor = '#2196F3';

    final iconOptions = {
      'place': Icons.place,
      'computer': Icons.computer,
      'phone': Icons.phone,
      'home': Icons.home,
      'business': Icons.business,
      'shopping_cart': Icons.shopping_cart,
      'person': Icons.person,
      'bolt': Icons.bolt,
      'schedule': Icons.schedule,
    };

    final colorOptions = [
      '#2196F3',
      '#4CAF50',
      '#FF9800',
      '#F44336',
      '#9C27B0',
      '#00BCD4',
      '#FFEB3B',
      '#795548',
    ];

    showDialog(
      context: context,
      builder: (context) => StatefulBuilder(
        builder: (context, setState) => AlertDialog(
          title: const Text('New Context'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: nameController,
                  decoration: const InputDecoration(
                    labelText: 'Name *',
                    hintText: '@Office, @Computer, etc.',
                    border: OutlineInputBorder(),
                  ),
                  autofocus: true,
                ),
                const SizedBox(height: AppSpacing.md),
                TextField(
                  controller: descriptionController,
                  maxLines: 2,
                  decoration: const InputDecoration(
                    labelText: 'Description',
                    border: OutlineInputBorder(),
                  ),
                ),
                const SizedBox(height: AppSpacing.md),
                DropdownButtonFormField<ContextType>(
                  value: selectedType,
                  decoration: const InputDecoration(
                    labelText: 'Type',
                    border: OutlineInputBorder(),
                  ),
                  items: ContextType.values.map((type) {
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
                const SizedBox(height: AppSpacing.md),
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    'Icon',
                    style: Theme.of(context).textTheme.titleSmall,
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.sm,
                  runSpacing: AppSpacing.sm,
                  children: iconOptions.entries.map((entry) {
                    final isSelected = selectedIcon == entry.key;
                    return InkWell(
                      onTap: () {
                        setState(() {
                          selectedIcon = entry.key;
                        });
                      },
                      child: Container(
                        width: 48,
                        height: 48,
                        decoration: BoxDecoration(
                          color: isSelected
                              ? AppColors.primary.withOpacity(0.1)
                              : AppColors.surfaceVariant,
                          borderRadius:
                              BorderRadius.circular(AppSpacing.radiusSm),
                          border: Border.all(
                            color: isSelected
                                ? AppColors.primary
                                : AppColors.border,
                            width: isSelected ? 2 : 1,
                          ),
                        ),
                        child: Icon(
                          entry.value,
                          color: isSelected
                              ? AppColors.primary
                              : AppColors.textSecondary,
                        ),
                      ),
                    );
                  }).toList(),
                ),
                const SizedBox(height: AppSpacing.md),
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    'Color',
                    style: Theme.of(context).textTheme.titleSmall,
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Wrap(
                  spacing: AppSpacing.sm,
                  runSpacing: AppSpacing.sm,
                  children: colorOptions.map((color) {
                    final isSelected = selectedColor == color;
                    return InkWell(
                      onTap: () {
                        setState(() {
                          selectedColor = color;
                        });
                      },
                      child: Container(
                        width: 48,
                        height: 48,
                        decoration: BoxDecoration(
                          color: _parseColor(color),
                          borderRadius:
                              BorderRadius.circular(AppSpacing.radiusSm),
                          border: Border.all(
                            color: isSelected ? Colors.black : Colors.transparent,
                            width: 2,
                          ),
                        ),
                        child: isSelected
                            ? const Icon(Icons.check, color: Colors.white)
                            : null,
                      ),
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
                if (nameController.text.isNotEmpty) {
                  await ref.read(
                    createContextProvider(
                      CreateUpdateContextDto(
                        userId: 'user-1',
                        name: nameController.text,
                        description: descriptionController.text.isEmpty
                            ? null
                            : descriptionController.text,
                        icon: selectedIcon,
                        color: selectedColor,
                        contextType: selectedType,
                      ),
                    ).future,
                  );
                  if (context.mounted) {
                    Navigator.pop(context);
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Context added'),
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

  Future<void> _deleteContext(
    BuildContext context,
    WidgetRef ref,
    String id,
  ) async {
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delete Context'),
        content: const Text('Are you sure you want to delete this context?'),
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
      await ref.read(deleteContextProvider(id).future);
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Context deleted')),
        );
      }
    }
  }

  Color _parseColor(String colorString) {
    return Color(
      int.parse(colorString.substring(1), radix: 16) + 0xFF000000,
    );
  }

  IconData _parseIcon(String iconName) {
    switch (iconName) {
      case 'place':
        return Icons.place;
      case 'computer':
        return Icons.computer;
      case 'phone':
        return Icons.phone;
      case 'home':
        return Icons.home;
      case 'business':
        return Icons.business;
      case 'shopping_cart':
        return Icons.shopping_cart;
      case 'person':
        return Icons.person;
      case 'bolt':
        return Icons.bolt;
      case 'schedule':
        return Icons.schedule;
      default:
        return Icons.label;
    }
  }
}
