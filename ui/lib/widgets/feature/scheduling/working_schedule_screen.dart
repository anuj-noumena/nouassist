import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/user_working_schedule_dto.dart';
import '../../../providers/working_schedule_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_loading_indicator.dart';
import '../../atomic/app_error_widget.dart';

class WorkingScheduleScreen extends ConsumerWidget {
  const WorkingScheduleScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final scheduleAsync = ref.watch(userWorkingScheduleProvider);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Working Schedule'),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: () {
              ref.read(userWorkingScheduleProvider.notifier).applyDefault();
            },
            tooltip: 'Reset to default (9-5)',
          ),
        ],
      ),
      body: scheduleAsync.when(
        data: (schedules) => _buildScheduleList(context, ref, schedules),
        loading: () => const AppLoadingIndicator(message: 'Loading schedule...'),
        error: (error, stack) => AppErrorWidget(
          error: error.toString(),
          onRetry: () => ref.invalidate(userWorkingScheduleProvider),
        ),
      ),
    );
  }

  Widget _buildScheduleList(
    BuildContext context,
    WidgetRef ref,
    List<UserWorkingScheduleDto> schedules,
  ) {
    return ListView.builder(
      padding: const EdgeInsets.all(AppSpacing.md),
      itemCount: schedules.length,
      itemBuilder: (context, index) {
        final schedule = schedules[index];
        return _buildScheduleCard(context, ref, schedule);
      },
    );
  }

  Widget _buildScheduleCard(
    BuildContext context,
    WidgetRef ref,
    UserWorkingScheduleDto schedule,
  ) {
    return Card(
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  width: 8,
                  height: 8,
                  decoration: BoxDecoration(
                    color: schedule.isWorkingDay
                        ? AppColors.success
                        : AppColors.textDisabled,
                    shape: BoxShape.circle,
                  ),
                ),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: AppText(
                    schedule.dayOfWeek.displayName,
                    variant: TextVariant.bodyLarge,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                Switch(
                  value: schedule.isWorkingDay,
                  onChanged: (value) {
                    _toggleWorkingDay(context, ref, schedule, value);
                  },
                ),
              ],
            ),
            if (schedule.isWorkingDay) ...[
              const SizedBox(height: AppSpacing.md),
              Row(
                children: [
                  Expanded(
                    child: _buildTimeField(
                      context,
                      'Start',
                      schedule.startTime ?? '09:00',
                      Icons.wb_sunny,
                    ),
                  ),
                  const SizedBox(width: AppSpacing.md),
                  Expanded(
                    child: _buildTimeField(
                      context,
                      'End',
                      schedule.endTime ?? '17:00',
                      Icons.nightlight,
                    ),
                  ),
                ],
              ),
              if (schedule.breakStartTime != null &&
                  schedule.breakEndTime != null) ...[
                const SizedBox(height: AppSpacing.sm),
                Container(
                  padding: const EdgeInsets.all(AppSpacing.sm),
                  decoration: BoxDecoration(
                    color: AppColors.surfaceVariant,
                    borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
                  ),
                  child: Row(
                    children: [
                      const Icon(Icons.coffee, size: 16),
                      const SizedBox(width: AppSpacing.sm),
                      AppText(
                        'Break: ${schedule.breakStartTime} - ${schedule.breakEndTime}',
                        variant: TextVariant.bodySmall,
                      ),
                    ],
                  ),
                ),
              ],
              const SizedBox(height: AppSpacing.sm),
              AppText(
                _calculateWorkingHours(schedule),
                variant: TextVariant.bodySmall,
                color: AppColors.textSecondary,
              ),
            ] else ...[
              const SizedBox(height: AppSpacing.sm),
              AppText(
                'Non-working day',
                variant: TextVariant.bodyMedium,
                color: AppColors.textSecondary,
              ),
            ],
          ],
        ),
      ),
    );
  }

  Widget _buildTimeField(
    BuildContext context,
    String label,
    String time,
    IconData icon,
  ) {
    return Container(
      padding: const EdgeInsets.all(AppSpacing.sm),
      decoration: BoxDecoration(
        border: Border.all(color: AppColors.border),
        borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
      ),
      child: Row(
        children: [
          Icon(icon, size: 16, color: AppColors.textSecondary),
          const SizedBox(width: AppSpacing.xs),
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              AppText(
                label,
                variant: TextVariant.bodySmall,
                color: AppColors.textSecondary,
              ),
              AppText(
                time,
                variant: TextVariant.bodyMedium,
                fontWeight: FontWeight.w600,
              ),
            ],
          ),
        ],
      ),
    );
  }

  String _calculateWorkingHours(UserWorkingScheduleDto schedule) {
    if (!schedule.isWorkingDay ||
        schedule.startTime == null ||
        schedule.endTime == null) {
      return '';
    }

    final start = _parseTime(schedule.startTime!);
    final end = _parseTime(schedule.endTime!);
    final duration = end.difference(start);

    var hours = duration.inHours;
    final minutes = duration.inMinutes.remainder(60);

    // Subtract break time if available
    if (schedule.breakStartTime != null && schedule.breakEndTime != null) {
      final breakStart = _parseTime(schedule.breakStartTime!);
      final breakEnd = _parseTime(schedule.breakEndTime!);
      final breakDuration = breakEnd.difference(breakStart);
      hours -= breakDuration.inHours;
    }

    return 'Working hours: ${hours}h ${minutes}m';
  }

  DateTime _parseTime(String time) {
    final parts = time.split(':');
    final now = DateTime.now();
    return DateTime(
      now.year,
      now.month,
      now.day,
      int.parse(parts[0]),
      int.parse(parts[1]),
    );
  }

  Future<void> _toggleWorkingDay(
    BuildContext context,
    WidgetRef ref,
    UserWorkingScheduleDto schedule,
    bool value,
  ) async {
    try {
      await ref.read(
        createOrUpdateWorkingScheduleProvider(
          CreateUpdateWorkingScheduleDto(
            userId: schedule.userId,
            dayOfWeek: schedule.dayOfWeek,
            isWorkingDay: value,
            startTime: value ? schedule.startTime ?? '09:00' : null,
            endTime: value ? schedule.endTime ?? '17:00' : null,
            breakStartTime: value ? schedule.breakStartTime : null,
            breakEndTime: value ? schedule.breakEndTime : null,
            timezone: schedule.timezone,
            effectiveFrom: schedule.effectiveFrom,
            effectiveTo: schedule.effectiveTo,
            isDefault: schedule.isDefault,
          ),
        ).future,
      );

      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              value
                  ? 'Set as working day'
                  : 'Set as non-working day',
            ),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (context.mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to update schedule: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }
}
