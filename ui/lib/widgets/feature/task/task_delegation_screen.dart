import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../core/utils/extensions.dart';
import '../../../data/models/task_delegation_dto.dart';
import '../../../providers/task_delegation_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_form_field.dart';

class TaskDelegationScreen extends ConsumerStatefulWidget {
  final String taskId;
  final String taskTitle;

  const TaskDelegationScreen({
    super.key,
    required this.taskId,
    required this.taskTitle,
  });

  @override
  ConsumerState<TaskDelegationScreen> createState() =>
      _TaskDelegationScreenState();
}

class _TaskDelegationScreenState extends ConsumerState<TaskDelegationScreen>
    with SingleTickerProviderStateMixin {
  late TabController _tabController;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Task Delegation'),
        bottom: TabBar(
          controller: _tabController,
          tabs: const [
            Tab(text: 'History'),
            Tab(text: 'My Delegations'),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          _buildDelegationHistory(),
          _buildMyDelegations(),
        ],
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => _showDelegateDialog(context),
        icon: const Icon(Icons.person_add),
        label: const Text('Delegate'),
      ),
    );
  }

  Widget _buildDelegationHistory() {
    final delegationsAsync = ref.watch(taskDelegationsProvider(widget.taskId));

    return delegationsAsync.when(
      data: (delegations) {
        if (delegations.isEmpty) {
          return Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.people_outline,
                  size: 64,
                  color: AppColors.textDisabled,
                ),
                const SizedBox(height: AppSpacing.md),
                AppText(
                  'No delegation history',
                  variant: TextVariant.bodyLarge,
                  color: AppColors.textSecondary,
                ),
              ],
            ),
          );
        }

        return ListView.builder(
          padding: const EdgeInsets.all(AppSpacing.md),
          itemCount: delegations.length,
          itemBuilder: (context, index) {
            final delegation = delegations[index];
            return _buildDelegationCard(delegation);
          },
        );
      },
      loading: () => const AppLoadingIndicator(message: 'Loading...'),
      error: (error, _) => AppErrorWidget(error: error.toString()),
    );
  }

  Widget _buildMyDelegations() {
    final delegationsAsync = ref.watch(
      userDelegationsProvider('user-1', delegatedTo: true),
    );

    return delegationsAsync.when(
      data: (delegations) {
        final pending = delegations
            .where((d) => d.acceptanceStatus == DelegationStatus.pending)
            .toList();

        if (pending.isEmpty) {
          return Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(
                  Icons.inbox_outlined,
                  size: 64,
                  color: AppColors.textDisabled,
                ),
                const SizedBox(height: AppSpacing.md),
                AppText(
                  'No pending delegations',
                  variant: TextVariant.bodyLarge,
                  color: AppColors.textSecondary,
                ),
              ],
            ),
          );
        }

        return ListView.builder(
          padding: const EdgeInsets.all(AppSpacing.md),
          itemCount: pending.length,
          itemBuilder: (context, index) {
            final delegation = pending[index];
            return _buildPendingDelegationCard(delegation);
          },
        );
      },
      loading: () => const AppLoadingIndicator(message: 'Loading...'),
      error: (error, _) => AppErrorWidget(error: error.toString()),
    );
  }

  Widget _buildDelegationCard(TaskDelegationDto delegation) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                CircleAvatar(
                  backgroundColor: AppColors.primary,
                  child: Text(
                    delegation.delegatedFrom.substring(delegation.delegatedFrom.length - 1),
                    style: const TextStyle(color: Colors.white),
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      AppText(
                        delegation.delegatedFromName,
                        variant: TextVariant.bodyMedium,
                        fontWeight: FontWeight.w600,
                      ),
                      AppText(
                        'delegated to ${delegation.delegatedToName}',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ),
                ),
                _buildStatusBadge(delegation.acceptanceStatus),
              ],
            ),
            if (delegation.reason != null) ...[
              const SizedBox(height: AppSpacing.sm),
              Container(
                padding: const EdgeInsets.all(AppSpacing.sm),
                decoration: BoxDecoration(
                  color: AppColors.surfaceVariant,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                ),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Icon(Icons.info_outline, size: 16),
                    const SizedBox(width: AppSpacing.xs),
                    Expanded(
                      child: AppText(
                        delegation.reason!,
                        variant: TextVariant.bodySmall,
                      ),
                    ),
                  ],
                ),
              ),
            ],
            const SizedBox(height: AppSpacing.sm),
            Row(
              children: [
                Icon(
                  Icons.calendar_today,
                  size: 14,
                  color: AppColors.textSecondary,
                ),
                const SizedBox(width: AppSpacing.xs),
                AppText(
                  delegation.delegationDate.toRelativeTimeString(),
                  variant: TextVariant.bodySmall,
                  color: AppColors.textSecondary,
                ),
              ],
            ),
            if (delegation.responseComment != null) ...[
              const SizedBox(height: AppSpacing.sm),
              Container(
                padding: const EdgeInsets.all(AppSpacing.sm),
                decoration: BoxDecoration(
                  color: delegation.acceptanceStatus == DelegationStatus.accepted
                      ? AppColors.success.withOpacity(0.1)
                      : AppColors.error.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    AppText(
                      'Response:',
                      variant: TextVariant.bodySmall,
                      fontWeight: FontWeight.w600,
                    ),
                    const SizedBox(height: AppSpacing.xs),
                    AppText(
                      delegation.responseComment!,
                      variant: TextVariant.bodySmall,
                    ),
                  ],
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }

  Widget _buildPendingDelegationCard(TaskDelegationDto delegation) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                CircleAvatar(
                  backgroundColor: AppColors.primary,
                  child: Text(
                    delegation.delegatedFrom.substring(delegation.delegatedFrom.length - 1),
                    style: const TextStyle(color: Colors.white),
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      AppText(
                        delegation.delegatedFromName,
                        variant: TextVariant.bodyMedium,
                        fontWeight: FontWeight.w600,
                      ),
                      AppText(
                        'wants to delegate a task',
                        variant: TextVariant.bodySmall,
                        color: AppColors.textSecondary,
                      ),
                    ],
                  ),
                ),
              ],
            ),
            if (delegation.reason != null) ...[
              const SizedBox(height: AppSpacing.sm),
              Container(
                padding: const EdgeInsets.all(AppSpacing.sm),
                decoration: BoxDecoration(
                  color: AppColors.surfaceVariant,
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                ),
                child: AppText(
                  delegation.reason!,
                  variant: TextVariant.bodySmall,
                ),
              ),
            ],
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: AppButton(
                    label: 'Reject',
                    variant: ButtonVariant.outlined,
                    onPressed: () => _showRejectDialog(context, delegation),
                  ),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: AppButton(
                    label: 'Accept',
                    variant: ButtonVariant.success,
                    onPressed: () => _acceptDelegation(delegation),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusBadge(DelegationStatus status) {
    Color color;
    IconData icon;

    switch (status) {
      case DelegationStatus.pending:
        color = AppColors.warning;
        icon = Icons.pending;
        break;
      case DelegationStatus.accepted:
        color = AppColors.success;
        icon = Icons.check_circle;
        break;
      case DelegationStatus.rejected:
        color = AppColors.error;
        icon = Icons.cancel;
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
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 14, color: color),
          const SizedBox(width: AppSpacing.xs),
          Text(
            status.displayName,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: color,
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _showDelegateDialog(BuildContext context) async {
    final reasonController = TextEditingController();
    String? selectedUser;

    final result = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Delegate Task'),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              widget.taskTitle,
              variant: TextVariant.bodyMedium,
              fontWeight: FontWeight.w600,
            ),
            const SizedBox(height: AppSpacing.md),
            DropdownButtonFormField<String>(
              decoration: const InputDecoration(
                labelText: 'Delegate to',
                border: OutlineInputBorder(),
              ),
              items: ['user-2', 'user-3', 'user-4', 'user-5']
                  .map((userId) => DropdownMenuItem(
                        value: userId,
                        child: Text('User $userId'),
                      ))
                  .toList(),
              onChanged: (value) {
                selectedUser = value;
              },
            ),
            const SizedBox(height: AppSpacing.md),
            AppFormField(
              controller: reasonController,
              label: 'Reason (optional)',
              hint: 'Why are you delegating this task?',
              maxLines: 3,
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () {
              if (selectedUser != null) {
                Navigator.pop(context, true);
              }
            },
            child: const Text('Delegate'),
          ),
        ],
      ),
    );

    if (result == true && selectedUser != null && context.mounted) {
      try {
        await ref.read(
          createDelegationProvider(
            CreateDelegationDto(
              taskId: widget.taskId,
              delegatedTo: selectedUser!,
              reason: reasonController.text.isEmpty
                  ? null
                  : reasonController.text,
            ),
          ).future,
        );

        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Task delegated successfully'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to delegate: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }

    reasonController.dispose();
  }

  Future<void> _acceptDelegation(TaskDelegationDto delegation) async {
    try {
      await ref.read(
        acceptDelegationProvider(
          delegation.id,
          delegation.taskId,
          'Happy to take this on',
        ).future,
      );

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Delegation accepted'),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to accept: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }

  Future<void> _showRejectDialog(
    BuildContext context,
    TaskDelegationDto delegation,
  ) async {
    final commentController = TextEditingController();

    final result = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Reject Delegation'),
        content: AppFormField(
          controller: commentController,
          label: 'Reason for rejection',
          hint: 'Please provide a reason...',
          maxLines: 3,
          required: true,
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(context, true),
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.error,
            ),
            child: const Text('Reject'),
          ),
        ],
      ),
    );

    if (result == true && context.mounted) {
      try {
        await ref.read(
          rejectDelegationProvider(
            delegation.id,
            delegation.taskId,
            commentController.text,
          ).future,
        );

        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Delegation rejected'),
              backgroundColor: AppColors.success,
            ),
          );
        }
      } catch (e) {
        if (context.mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('Failed to reject: $e'),
              backgroundColor: AppColors.error,
            ),
          );
        }
      }
    }

    commentController.dispose();
  }
}
