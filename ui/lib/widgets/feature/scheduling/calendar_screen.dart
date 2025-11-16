import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:table_calendar/table_calendar.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/time_block_dto.dart';
import '../../../data/models/event_dto.dart';
import '../../../providers/time_block_provider.dart';
import '../../../providers/event_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_loading_indicator.dart';
import 'widgets/time_block_card.dart';
import 'widgets/event_card.dart';
import 'time_block_form_screen.dart';
import 'event_form_screen.dart';

class CalendarScreen extends ConsumerStatefulWidget {
  const CalendarScreen({super.key});

  @override
  ConsumerState<CalendarScreen> createState() => _CalendarScreenState();
}

class _CalendarScreenState extends ConsumerState<CalendarScreen>
    with SingleTickerProviderStateMixin {
  CalendarFormat _calendarFormat = CalendarFormat.month;
  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  late TabController _tabController;

  @override
  void initState() {
    super.initState();
    _selectedDay = _focusedDay;
    _tabController = TabController(length: 2, vsync: this);
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final timeBlocksAsync = _selectedDay != null
        ? ref.watch(timeBlocksByDateProvider('user-1', _selectedDay!))
        : null;
    final eventsAsync = _selectedDay != null
        ? ref.watch(eventsByDateProvider('user-1', _selectedDay!))
        : null;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Calendar'),
        actions: [
          IconButton(
            icon: const Icon(Icons.today),
            onPressed: () {
              setState(() {
                _focusedDay = DateTime.now();
                _selectedDay = DateTime.now();
              });
            },
          ),
        ],
      ),
      body: Column(
        children: [
          TableCalendar(
            firstDay: DateTime.utc(2020, 1, 1),
            lastDay: DateTime.utc(2030, 12, 31),
            focusedDay: _focusedDay,
            calendarFormat: _calendarFormat,
            selectedDayPredicate: (day) {
              return isSameDay(_selectedDay, day);
            },
            onDaySelected: (selectedDay, focusedDay) {
              setState(() {
                _selectedDay = selectedDay;
                _focusedDay = focusedDay;
              });
            },
            onFormatChanged: (format) {
              setState(() {
                _calendarFormat = format;
              });
            },
            onPageChanged: (focusedDay) {
              _focusedDay = focusedDay;
            },
            calendarStyle: CalendarStyle(
              todayDecoration: BoxDecoration(
                color: AppColors.primary.withOpacity(0.3),
                shape: BoxShape.circle,
              ),
              selectedDecoration: const BoxDecoration(
                color: AppColors.primary,
                shape: BoxShape.circle,
              ),
              markerDecoration: const BoxDecoration(
                color: AppColors.error,
                shape: BoxShape.circle,
              ),
            ),
            headerStyle: HeaderStyle(
              titleCentered: true,
              formatButtonVisible: true,
              formatButtonDecoration: BoxDecoration(
                color: AppColors.primary.withOpacity(0.1),
                borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
              ),
            ),
          ),
          const Divider(),
          TabBar(
            controller: _tabController,
            tabs: const [
              Tab(text: 'Time Blocks'),
              Tab(text: 'Events'),
            ],
          ),
          Expanded(
            child: TabBarView(
              controller: _tabController,
              children: [
                _buildTimeBlocksList(timeBlocksAsync),
                _buildEventsList(eventsAsync),
              ],
            ),
          ),
        ],
      ),
      floatingActionButton: Column(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          FloatingActionButton(
            heroTag: 'add_time_block',
            onPressed: () => _navigateToTimeBlockForm(context),
            child: const Icon(Icons.view_timeline),
          ),
          const SizedBox(height: AppSpacing.md),
          FloatingActionButton(
            heroTag: 'add_event',
            onPressed: () => _navigateToEventForm(context),
            child: const Icon(Icons.event),
          ),
        ],
      ),
    );
  }

  Widget _buildTimeBlocksList(AsyncValue<List<TimeBlockDto>>? timeBlocksAsync) {
    if (_selectedDay == null) {
      return _buildEmptyState('Select a date to view time blocks');
    }

    if (timeBlocksAsync == null) {
      return const AppLoadingIndicator();
    }

    return timeBlocksAsync.when(
      data: (timeBlocks) {
        if (timeBlocks.isEmpty) {
          return _buildEmptyState('No time blocks for this date');
        }

        return ListView.builder(
          padding: const EdgeInsets.all(AppSpacing.md),
          itemCount: timeBlocks.length,
          itemBuilder: (context, index) {
            final block = timeBlocks[index];
            return TimeBlockCard(
              timeBlock: block,
              onTap: () {
                // Navigate to time block details
              },
            );
          },
        );
      },
      loading: () => const AppLoadingIndicator(),
      error: (error, _) => Center(child: Text('Error: $error')),
    );
  }

  Widget _buildEventsList(AsyncValue<List<EventDto>>? eventsAsync) {
    if (_selectedDay == null) {
      return _buildEmptyState('Select a date to view events');
    }

    if (eventsAsync == null) {
      return const AppLoadingIndicator();
    }

    return eventsAsync.when(
      data: (events) {
        if (events.isEmpty) {
          return _buildEmptyState('No events for this date');
        }

        return ListView.builder(
          padding: const EdgeInsets.all(AppSpacing.md),
          itemCount: events.length,
          itemBuilder: (context, index) {
            final event = events[index];
            return EventCard(
              event: event,
              onTap: () {
                // Navigate to event details
              },
            );
          },
        );
      },
      loading: () => const AppLoadingIndicator(),
      error: (error, _) => Center(child: Text('Error: $error')),
    );
  }

  Widget _buildEmptyState(String message) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(
            Icons.calendar_today,
            size: 64,
            color: AppColors.textDisabled,
          ),
          const SizedBox(height: AppSpacing.md),
          AppText(
            message,
            variant: TextVariant.bodyLarge,
            color: AppColors.textSecondary,
          ),
        ],
      ),
    );
  }

  void _navigateToTimeBlockForm(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => TimeBlockFormScreen(
          initialDate: _selectedDay ?? DateTime.now(),
        ),
      ),
    );
  }

  void _navigateToEventForm(BuildContext context) {
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => EventFormScreen(
          initialDate: _selectedDay ?? DateTime.now(),
        ),
      ),
    );
  }
}
