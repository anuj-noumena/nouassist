import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/gtd_inbox_dto.dart';
import '../../../data/models/gtd_next_action_dto.dart';
import '../../../data/models/gtd_waiting_for_dto.dart';
import '../../../providers/gtd_inbox_provider.dart';
import '../../../providers/gtd_next_action_provider.dart';
import '../../../providers/gtd_waiting_for_provider.dart';
import '../../../providers/gtd_review_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import 'inbox_screen.dart';
import 'next_actions_screen.dart';
import 'waiting_for_screen.dart';
import 'someday_maybe_screen.dart';
import 'weekly_review_screen.dart';
import 'contexts_screen.dart';

class GtdDashboard extends ConsumerWidget {
  const GtdDashboard({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final inboxAsync = ref.watch(inboxListProvider('user-1'));
    final actionsAsync = ref.watch(nextActionListProvider('user-1'));
    final waitingForAsync = ref.watch(waitingForListProvider('user-1'));
    final reviewsAsync = ref.watch(reviewListProvider('user-1'));

    return Scaffold(
      appBar: AppBar(
        title: const Text('GTD Dashboard'),
        actions: [
          IconButton(
            icon: const Icon(Icons.settings),
            onPressed: () {
              // Settings
            },
          ),
        ],
      ),
      body: RefreshIndicator(
        onRefresh: () async {
          ref.invalidate(inboxListProvider);
          ref.invalidate(nextActionListProvider);
          ref.invalidate(waitingForListProvider);
          ref.invalidate(reviewListProvider);
        },
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Welcome section
              _buildWelcomeCard(context),
              const SizedBox(height: AppSpacing.xl),

              // Quick stats
              AppText(
                'Overview',
                variant: TextVariant.h4,
                fontWeight: FontWeight.bold,
              ),
              const SizedBox(height: AppSpacing.md),
              _buildStatsGrid(
                inboxAsync,
                actionsAsync,
                waitingForAsync,
                reviewsAsync,
              ),
              const SizedBox(height: AppSpacing.xl),

              // Quick actions
              AppText(
                'Quick Actions',
                variant: TextVariant.h4,
                fontWeight: FontWeight.bold,
              ),
              const SizedBox(height: AppSpacing.md),
              _buildQuickActions(context),
              const SizedBox(height: AppSpacing.xl),

              // GTD workflow sections
              AppText(
                'GTD Workflow',
                variant: TextVariant.h4,
                fontWeight: FontWeight.bold,
              ),
              const SizedBox(height: AppSpacing.md),
              _buildWorkflowCards(context, inboxAsync, actionsAsync),
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          _showQuickCapture(context, ref);
        },
        icon: const Icon(Icons.add),
        label: const Text('Quick Capture'),
      ),
    );
  }

  Widget _buildWelcomeCard(BuildContext context) {
    final hour = DateTime.now().hour;
    String greeting;
    IconData icon;

    if (hour < 12) {
      greeting = 'Good Morning';
      icon = Icons.wb_sunny;
    } else if (hour < 18) {
      greeting = 'Good Afternoon';
      icon = Icons.wb_cloudy;
    } else {
      greeting = 'Good Evening';
      icon = Icons.nightlight_round;
    }

    return Container(
      padding: const EdgeInsets.all(AppSpacing.lg),
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFF2196F3), Color(0xFF1976D2)],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(icon, color: Colors.white, size: 32),
              const SizedBox(width: AppSpacing.sm),
              Text(
                greeting,
                style: const TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.sm),
          const Text(
            'Stay organized and get things done!',
            style: TextStyle(
              fontSize: 14,
              color: Colors.white70,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatsGrid(
    AsyncValue inboxAsync,
    AsyncValue actionsAsync,
    AsyncValue waitingForAsync,
    AsyncValue reviewsAsync,
  ) {
    return GridView.count(
      crossAxisCount: 2,
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      mainAxisSpacing: AppSpacing.md,
      crossAxisSpacing: AppSpacing.md,
      childAspectRatio: 1.5,
      children: [
        _buildStatCard(
          'Inbox',
          inboxAsync.when(
            data: (data) =>
                (data as List<GtdInboxDto>).where((i) => i.status == InboxStatus.unprocessed).length.toString(),
            loading: () => '...',
            error: (_, __) => '0',
          ),
          Icons.inbox,
          AppColors.primary,
          'items to process',
        ),
        _buildStatCard(
          'Next Actions',
          actionsAsync.when(
            data: (data) =>
                (data as List<GtdNextActionDto>).where((a) => !a.isCompleted).length.toString(),
            loading: () => '...',
            error: (_, __) => '0',
          ),
          Icons.check_circle_outline,
          AppColors.success,
          'active actions',
        ),
        _buildStatCard(
          'Waiting For',
          waitingForAsync.when(
            data: (data) =>
                (data as List<GtdWaitingForDto>).where((i) => i.status == WaitingForStatus.waiting).length.toString(),
            loading: () => '...',
            error: (_, __) => '0',
          ),
          Icons.hourglass_empty,
          AppColors.warning,
          'pending items',
        ),
        _buildStatCard(
          'Reviews',
          reviewsAsync.when(
            data: (data) => (data as List).where((r) => !r.isCompleted).length.toString(),
            loading: () => '...',
            error: (_, __) => '0',
          ),
          Icons.assignment,
          AppColors.info,
          'due reviews',
        ),
      ],
    );
  }

  Widget _buildStatCard(
    String title,
    String count,
    IconData icon,
    Color color,
    String subtitle,
  ) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        border: Border.all(color: AppColors.border),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(AppSpacing.sm),
                decoration: BoxDecoration(
                  color: color.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                ),
                child: Icon(icon, color: color, size: 20),
              ),
              const Spacer(),
              Text(
                count,
                style: TextStyle(
                  fontSize: 28,
                  fontWeight: FontWeight.bold,
                  color: color,
                ),
              ),
            ],
          ),
          const Spacer(),
          AppText(
            title,
            variant: TextVariant.bodyMedium,
            fontWeight: FontWeight.w600,
          ),
          AppText(
            subtitle,
            variant: TextVariant.bodySmall,
            color: AppColors.textSecondary,
          ),
        ],
      ),
    );
  }

  Widget _buildQuickActions(BuildContext context) {
    return Wrap(
      spacing: AppSpacing.sm,
      runSpacing: AppSpacing.sm,
      children: [
        _buildActionChip(
          context,
          'Process Inbox',
          Icons.inbox,
          AppColors.primary,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const InboxScreen()),
          ),
        ),
        _buildActionChip(
          context,
          'Next Actions',
          Icons.check_circle_outline,
          AppColors.success,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const NextActionsScreen()),
          ),
        ),
        _buildActionChip(
          context,
          'Weekly Review',
          Icons.assignment,
          AppColors.warning,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const WeeklyReviewScreen()),
          ),
        ),
        _buildActionChip(
          context,
          'Contexts',
          Icons.label,
          AppColors.info,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const ContextsScreen()),
          ),
        ),
      ],
    );
  }

  Widget _buildActionChip(
    BuildContext context,
    String label,
    IconData icon,
    Color color,
    VoidCallback onTap,
  ) {
    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
      child: Container(
        padding: const EdgeInsets.symmetric(
          horizontal: AppSpacing.md,
          vertical: AppSpacing.sm,
        ),
        decoration: BoxDecoration(
          color: color.withOpacity(0.1),
          borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
          border: Border.all(color: color),
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(icon, size: 18, color: color),
            const SizedBox(width: AppSpacing.sm),
            Text(
              label,
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w600,
                color: color,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildWorkflowCards(
    BuildContext context,
    AsyncValue inboxAsync,
    AsyncValue actionsAsync,
  ) {
    return Column(
      children: [
        _buildWorkflowCard(
          context,
          'Capture',
          'Collect everything that needs your attention',
          Icons.inbox,
          AppColors.primary,
          inboxAsync.when(
            data: (items) =>
                (items as List).where((i) => i.status == InboxStatus.unprocessed).length,
            loading: () => 0,
            error: (_, __) => 0,
          ),
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const InboxScreen()),
          ),
        ),
        const SizedBox(height: AppSpacing.md),
        _buildWorkflowCard(
          context,
          'Clarify',
          'Process inbox items into actionable steps',
          Icons.filter_list,
          AppColors.info,
          null,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const InboxScreen()),
          ),
        ),
        const SizedBox(height: AppSpacing.md),
        _buildWorkflowCard(
          context,
          'Organize',
          'Put items where they belong',
          Icons.folder_outlined,
          AppColors.warning,
          null,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const NextActionsScreen()),
          ),
        ),
        const SizedBox(height: AppSpacing.md),
        _buildWorkflowCard(
          context,
          'Reflect',
          'Review and update your system',
          Icons.refresh,
          AppColors.success,
          null,
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const WeeklyReviewScreen()),
          ),
        ),
        const SizedBox(height: AppSpacing.md),
        _buildWorkflowCard(
          context,
          'Engage',
          'Do the work with confidence',
          Icons.play_circle_outline,
          AppColors.error,
          actionsAsync.when(
            data: (items) => items.where((a) => !a.isCompleted).length,
            loading: () => 0,
            error: (_, __) => 0,
          ),
          () => Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const NextActionsScreen()),
          ),
        ),
      ],
    );
  }

  Widget _buildWorkflowCard(
    BuildContext context,
    String title,
    String description,
    IconData icon,
    Color color,
    int? count,
    VoidCallback onTap,
  ) {
    return Card(
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.md),
          child: Row(
            children: [
              Container(
                padding: const EdgeInsets.all(AppSpacing.md),
                decoration: BoxDecoration(
                  color: color.withOpacity(0.1),
                  borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                ),
                child: Icon(icon, color: color, size: 32),
              ),
              const SizedBox(width: AppSpacing.md),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        AppText(
                          title,
                          variant: TextVariant.bodyLarge,
                          fontWeight: FontWeight.bold,
                        ),
                        if (count != null && count > 0) ...[
                          const SizedBox(width: AppSpacing.sm),
                          Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: AppSpacing.sm,
                              vertical: 2,
                            ),
                            decoration: BoxDecoration(
                              color: color,
                              borderRadius:
                                  BorderRadius.circular(AppSpacing.radiusSm),
                            ),
                            child: Text(
                              count.toString(),
                              style: const TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.bold,
                                color: Colors.white,
                              ),
                            ),
                          ),
                        ],
                      ],
                    ),
                    const SizedBox(height: AppSpacing.xs),
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

  void _showQuickCapture(BuildContext context, WidgetRef ref) {
    final controller = TextEditingController();

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(AppSpacing.radiusMd)),
      ),
      builder: (context) => Padding(
        padding: EdgeInsets.only(
          bottom: MediaQuery.of(context).viewInsets.bottom,
          left: AppSpacing.md,
          right: AppSpacing.md,
          top: AppSpacing.md,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              'Quick Capture',
              variant: TextVariant.h5,
              fontWeight: FontWeight.bold,
            ),
            const SizedBox(height: AppSpacing.md),
            TextField(
              controller: controller,
              autofocus: true,
              maxLines: 3,
              decoration: const InputDecoration(
                hintText: 'What\'s on your mind?',
                border: OutlineInputBorder(),
              ),
            ),
            const SizedBox(height: AppSpacing.md),
            Row(
              children: [
                Expanded(
                  child: TextButton(
                    onPressed: () => Navigator.pop(context),
                    child: const Text('Cancel'),
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: AppButton(
                    label: 'Add to Inbox',
                    onPressed: () async {
                      if (controller.text.isNotEmpty) {
                        await ref.read(
                          createInboxItemProvider(
                            CreateUpdateInboxDto(
                              userId: 'user-1',
                              content: controller.text,
                              itemType: InboxItemType.thought,
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
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
          ],
        ),
      ),
    );
  }
}
