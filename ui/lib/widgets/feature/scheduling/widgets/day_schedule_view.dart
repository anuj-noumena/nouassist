import 'package:flutter/material.dart';
import '../../../../core/theme/app_colors.dart';
import '../../../../core/theme/app_spacing.dart';
import '../../../../data/models/time_block_dto.dart';
import '../../../../data/models/event_dto.dart';
import '../../../atomic/app_text.dart';

class DayScheduleView extends StatelessWidget {
  final DateTime date;
  final List<TimeBlockDto> timeBlocks;
  final List<EventDto> events;
  final Function(TimeBlockDto)? onTimeBlockTap;
  final Function(EventDto)? onEventTap;

  const DayScheduleView({
    super.key,
    required this.date,
    required this.timeBlocks,
    required this.events,
    this.onTimeBlockTap,
    this.onEventTap,
  });

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: const EdgeInsets.all(AppSpacing.md),
      itemCount: 24,
      itemBuilder: (context, hour) {
        return _buildHourSlot(hour);
      },
    );
  }

  Widget _buildHourSlot(int hour) {
    final hourBlocks = timeBlocks.where((block) {
      return block.startDateTime.hour == hour;
    }).toList();

    final hourEvents = events.where((event) {
      return event.startDateTime.hour == hour;
    }).toList();

    return Container(
      height: 80,
      decoration: BoxDecoration(
        border: Border(
          bottom: BorderSide(color: AppColors.border),
        ),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 60,
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.sm),
              child: AppText(
                _formatHour(hour),
                variant: TextVariant.bodySmall,
                color: AppColors.textSecondary,
              ),
            ),
          ),
          Expanded(
            child: Column(
              children: [
                ...hourBlocks.map((block) => _buildTimeBlockItem(block)),
                ...hourEvents.map((event) => _buildEventItem(event)),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTimeBlockItem(TimeBlockDto block) {
    return InkWell(
      onTap: () => onTimeBlockTap?.call(block),
      child: Container(
        margin: const EdgeInsets.all(AppSpacing.xs),
        padding: const EdgeInsets.all(AppSpacing.sm),
        decoration: BoxDecoration(
          color: Color(int.parse(block.color.replaceFirst('#', '0xFF')))
              .withOpacity(0.2),
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          border: Border(
            left: BorderSide(
              color: Color(int.parse(block.color.replaceFirst('#', '0xFF'))),
              width: 3,
            ),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            AppText(
              block.title,
              variant: TextVariant.bodySmall,
              fontWeight: FontWeight.w600,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
            AppText(
              '${_formatTime(block.startDateTime)} - ${_formatTime(block.endDateTime)}',
              variant: TextVariant.bodySmall,
              color: AppColors.textSecondary,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildEventItem(EventDto event) {
    return InkWell(
      onTap: () => onEventTap?.call(event),
      child: Container(
        margin: const EdgeInsets.all(AppSpacing.xs),
        padding: const EdgeInsets.all(AppSpacing.sm),
        decoration: BoxDecoration(
          color: Color(int.parse(event.color.replaceFirst('#', '0xFF')))
              .withOpacity(0.2),
          borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
          border: Border(
            left: BorderSide(
              color: Color(int.parse(event.color.replaceFirst('#', '0xFF'))),
              width: 3,
            ),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(
                  _getEventIcon(event.eventType),
                  size: 12,
                  color: Color(int.parse(event.color.replaceFirst('#', '0xFF'))),
                ),
                const SizedBox(width: AppSpacing.xs),
                Expanded(
                  child: AppText(
                    event.title,
                    variant: TextVariant.bodySmall,
                    fontWeight: FontWeight.w600,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ],
            ),
            if (!event.isAllDay)
              AppText(
                '${_formatTime(event.startDateTime)} - ${_formatTime(event.endDateTime)}',
                variant: TextVariant.bodySmall,
                color: AppColors.textSecondary,
              ),
          ],
        ),
      ),
    );
  }

  String _formatHour(int hour) {
    if (hour == 0) return '12 AM';
    if (hour < 12) return '$hour AM';
    if (hour == 12) return '12 PM';
    return '${hour - 12} PM';
  }

  String _formatTime(DateTime dateTime) {
    return '${dateTime.hour.toString().padLeft(2, '0')}:${dateTime.minute.toString().padLeft(2, '0')}';
  }

  IconData _getEventIcon(EventType type) {
    switch (type) {
      case EventType.meeting:
        return Icons.people;
      case EventType.appointment:
        return Icons.event;
      case EventType.reminder:
        return Icons.notifications;
      case EventType.deadline:
        return Icons.flag;
      case EventType.personal:
        return Icons.person;
      case EventType.holiday:
        return Icons.celebration;
    }
  }
}
