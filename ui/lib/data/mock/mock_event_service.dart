import '../models/event_dto.dart';

/// Mock event service
class MockEventService {
  static final List<EventDto> _events = _generateMockEvents();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get events by user and date range
  static Future<List<EventDto>> getByUserAndDateRange({
    required String userId,
    required DateTime startDate,
    required DateTime endDate,
    EventType? typeFilter,
  }) async {
    await _simulateDelay();

    var filtered = _events.where((event) {
      return event.userId == userId &&
          event.startDateTime.isAfter(startDate.subtract(const Duration(days: 1))) &&
          event.startDateTime.isBefore(endDate.add(const Duration(days: 1)));
    }).toList();

    if (typeFilter != null) {
      filtered = filtered.where((event) => event.eventType == typeFilter).toList();
    }

    filtered.sort((a, b) => a.startDateTime.compareTo(b.startDateTime));
    return filtered;
  }

  /// Get events for a specific date
  static Future<List<EventDto>> getByDate({
    required String userId,
    required DateTime date,
  }) async {
    await _simulateDelay();

    final startOfDay = DateTime(date.year, date.month, date.day);
    final endOfDay = startOfDay.add(const Duration(days: 1));

    return _events.where((event) {
      return event.userId == userId &&
          event.startDateTime.isAfter(startOfDay.subtract(const Duration(seconds: 1))) &&
          event.startDateTime.isBefore(endOfDay);
    }).toList()
      ..sort((a, b) => a.startDateTime.compareTo(b.startDateTime));
  }

  /// Get single event
  static Future<EventDto> get(String id) async {
    await _simulateDelay();

    try {
      return _events.firstWhere((event) => event.id == id);
    } catch (e) {
      throw Exception('Event not found');
    }
  }

  /// Create event
  static Future<EventDto> create(CreateUpdateEventDto input) async {
    await _simulateDelay();

    if (input.startDateTime.isAfter(input.endDateTime)) {
      throw Exception('End time must be after start time');
    }

    final newEvent = EventDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      userId: input.userId,
      title: input.title,
      description: input.description,
      eventType: input.eventType,
      startDateTime: input.startDateTime,
      endDateTime: input.endDateTime,
      isAllDay: input.isAllDay,
      location: input.location,
      meetingLink: input.meetingLink,
      attendees: input.attendees,
      isRecurring: input.isRecurring,
      recurrencePattern: input.recurrencePattern,
      reminderMinutes: input.reminderMinutes,
      color: input.color,
      visibility: input.visibility,
      status: input.status,
      projectId: input.projectId,
      taskId: input.taskId,
      timeBlockId: input.timeBlockId,
      creationTime: DateTime.now(),
    );

    _events.insert(0, newEvent);
    return newEvent;
  }

  /// Update event
  static Future<EventDto> update(String id, CreateUpdateEventDto input) async {
    await _simulateDelay();

    final index = _events.indexWhere((event) => event.id == id);
    if (index == -1) {
      throw Exception('Event not found');
    }

    if (input.startDateTime.isAfter(input.endDateTime)) {
      throw Exception('End time must be after start time');
    }

    final updatedEvent = _events[index].copyWith(
      title: input.title,
      description: input.description,
      eventType: input.eventType,
      startDateTime: input.startDateTime,
      endDateTime: input.endDateTime,
      isAllDay: input.isAllDay,
      location: input.location,
      meetingLink: input.meetingLink,
      attendees: input.attendees,
      isRecurring: input.isRecurring,
      recurrencePattern: input.recurrencePattern,
      reminderMinutes: input.reminderMinutes,
      color: input.color,
      visibility: input.visibility,
      status: input.status,
      projectId: input.projectId,
      taskId: input.taskId,
      timeBlockId: input.timeBlockId,
      lastModificationTime: DateTime.now(),
    );

    _events[index] = updatedEvent;
    return updatedEvent;
  }

  /// Delete event
  static Future<void> delete(String id) async {
    await _simulateDelay();
    final initialLength = _events.length;
    _events.removeWhere((event) => event.id == id);
    if (_events.length == initialLength) {
      throw Exception('Event not found');
    }
  }

  /// Generate mock events
  static List<EventDto> _generateMockEvents() {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final events = <EventDto>[];

    final colors = {
      EventType.meeting: '#9C27B0',
      EventType.appointment: '#2196F3',
      EventType.reminder: '#FF9800',
      EventType.deadline: '#F44336',
      EventType.personal: '#4CAF50',
      EventType.holiday: '#00BCD4',
    };

    // Generate events for the next 2 weeks
    for (int day = 0; day <= 14; day++) {
      final date = today.add(Duration(days: day));

      // Skip weekends for meetings
      if (date.weekday == DateTime.saturday || date.weekday == DateTime.sunday) {
        // Add personal events on weekends
        events.add(EventDto(
          id: 'event-${events.length + 1}',
          userId: 'user-1',
          title: 'Weekend Activity',
          description: 'Personal time',
          eventType: EventType.personal,
          startDateTime: date.add(const Duration(hours: 10)),
          endDateTime: date.add(const Duration(hours: 12)),
          color: colors[EventType.personal]!,
          visibility: EventVisibility.private,
          status: EventStatus.confirmed,
          creationTime: date.subtract(const Duration(days: 7)),
        ));
        continue;
      }

      // Client meeting
      if (day % 3 == 0) {
        events.add(EventDto(
          id: 'event-${events.length + 1}',
          userId: 'user-1',
          title: 'Client Meeting',
          description: 'Discuss project requirements',
          eventType: EventType.meeting,
          startDateTime: date.add(const Duration(hours: 10)),
          endDateTime: date.add(const Duration(hours: 11)),
          location: 'Conference Room B',
          meetingLink: 'https://meet.google.com/abc-defg-hij',
          attendees: ['user-2', 'user-3'],
          reminderMinutes: [15, 60],
          color: colors[EventType.meeting]!,
          visibility: EventVisibility.public,
          status: EventStatus.confirmed,
          creationTime: date.subtract(const Duration(days: 7)),
        ));
      }

      // Project deadline
      if (day % 5 == 0) {
        events.add(EventDto(
          id: 'event-${events.length + 1}',
          userId: 'user-1',
          title: 'Sprint Deadline',
          description: 'Complete all sprint tasks',
          eventType: EventType.deadline,
          startDateTime: date.add(const Duration(hours: 17)),
          endDateTime: date.add(const Duration(hours: 17, minutes: 30)),
          isAllDay: false,
          reminderMinutes: [1440, 60],
          color: colors[EventType.deadline]!,
          visibility: EventVisibility.public,
          status: EventStatus.confirmed,
          projectId: '1',
          creationTime: date.subtract(const Duration(days: 14)),
        ));
      }

      // Doctor appointment
      if (day == 7) {
        events.add(EventDto(
          id: 'event-${events.length + 1}',
          userId: 'user-1',
          title: 'Doctor Appointment',
          description: 'Annual checkup',
          eventType: EventType.appointment,
          startDateTime: date.add(const Duration(hours: 15)),
          endDateTime: date.add(const Duration(hours: 16)),
          location: 'Medical Center',
          reminderMinutes: [60],
          color: colors[EventType.appointment]!,
          visibility: EventVisibility.private,
          status: EventStatus.confirmed,
          creationTime: date.subtract(const Duration(days: 30)),
        ));
      }
    }

    return events;
  }
}
