import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/event_dto.dart';
import '../../../providers/event_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_form_field.dart';
import '../../atomic/app_dropdown.dart';

class EventFormScreen extends ConsumerStatefulWidget {
  final String? eventId;
  final DateTime? initialDate;

  const EventFormScreen({
    super.key,
    this.eventId,
    this.initialDate,
  });

  @override
  ConsumerState<EventFormScreen> createState() => _EventFormScreenState();
}

class _EventFormScreenState extends ConsumerState<EventFormScreen> {
  final _formKey = GlobalKey<FormState>();
  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  final _locationController = TextEditingController();
  final _meetingLinkController = TextEditingController();

  EventType _selectedType = EventType.meeting;
  DateTime? _startDateTime;
  DateTime? _endDateTime;
  bool _isAllDay = false;
  String _selectedColor = '#9C27B0';
  EventVisibility _visibility = EventVisibility.public;
  EventStatus _status = EventStatus.confirmed;
  bool _isRecurring = false;
  bool _isLoading = false;
  final List<int> _reminderMinutes = [15];

  final Map<EventType, String> _typeColors = {
    EventType.meeting: '#9C27B0',
    EventType.appointment: '#2196F3',
    EventType.reminder: '#FF9800',
    EventType.deadline: '#F44336',
    EventType.personal: '#4CAF50',
    EventType.holiday: '#00BCD4',
  };

  @override
  void initState() {
    super.initState();
    if (widget.initialDate != null) {
      _startDateTime = widget.initialDate!.add(const Duration(hours: 10));
      _endDateTime = widget.initialDate!.add(const Duration(hours: 11));
    } else {
      final now = DateTime.now();
      _startDateTime = DateTime(now.year, now.month, now.day, 10, 0);
      _endDateTime = DateTime(now.year, now.month, now.day, 11, 0);
    }

    if (widget.eventId != null) {
      _loadEvent();
    }
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    _locationController.dispose();
    _meetingLinkController.dispose();
    super.dispose();
  }

  Future<void> _loadEvent() async {
    try {
      final event = await ref.read(eventProvider(widget.eventId!).future);
      setState(() {
        _titleController.text = event.title;
        _descriptionController.text = event.description ?? '';
        _locationController.text = event.location ?? '';
        _meetingLinkController.text = event.meetingLink ?? '';
        _selectedType = event.eventType;
        _startDateTime = event.startDateTime;
        _endDateTime = event.endDateTime;
        _isAllDay = event.isAllDay;
        _selectedColor = event.color;
        _visibility = event.visibility;
        _status = event.status;
        _isRecurring = event.isRecurring;
        _reminderMinutes.clear();
        _reminderMinutes.addAll(event.reminderMinutes);
      });
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to load event: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.eventId != null ? 'Edit Event' : 'New Event'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Form(
          key: _formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              AppFormField(
                controller: _titleController,
                label: 'Title',
                hint: 'Enter event title',
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter a title';
                  }
                  return null;
                },
              ),
              const SizedBox(height: AppSpacing.md),
              AppFormField(
                controller: _descriptionController,
                label: 'Description (optional)',
                hint: 'Enter description',
                maxLines: 3,
              ),
              const SizedBox(height: AppSpacing.md),
              AppDropdown<EventType>(
                label: 'Type',
                value: _selectedType,
                items: EventType.values.map((type) {
                  return DropdownMenuItem(
                    value: type,
                    child: Row(
                      children: [
                        Container(
                          width: 16,
                          height: 16,
                          decoration: BoxDecoration(
                            color: Color(int.parse(
                                _typeColors[type]!.replaceFirst('#', '0xFF'))),
                            shape: BoxShape.circle,
                          ),
                        ),
                        const SizedBox(width: AppSpacing.sm),
                        Text(type.displayName),
                      ],
                    ),
                  );
                }).toList(),
                onChanged: (value) {
                  if (value != null) {
                    setState(() {
                      _selectedType = value;
                      _selectedColor = _typeColors[value]!;
                    });
                  }
                },
              ),
              const SizedBox(height: AppSpacing.md),
              SwitchListTile(
                title: const Text('All-day event'),
                value: _isAllDay,
                onChanged: (value) {
                  setState(() {
                    _isAllDay = value;
                  });
                },
              ),
              const SizedBox(height: AppSpacing.md),
              if (!_isAllDay) ...[
                Row(
                  children: [
                    Expanded(
                      child: _buildDateTimeField(
                        label: 'Start',
                        dateTime: _startDateTime!,
                        onChanged: (dateTime) {
                          setState(() {
                            _startDateTime = dateTime;
                            if (_endDateTime!.isBefore(dateTime)) {
                              _endDateTime = dateTime.add(const Duration(hours: 1));
                            }
                          });
                        },
                      ),
                    ),
                    const SizedBox(width: AppSpacing.md),
                    Expanded(
                      child: _buildDateTimeField(
                        label: 'End',
                        dateTime: _endDateTime!,
                        onChanged: (dateTime) {
                          if (dateTime.isAfter(_startDateTime!)) {
                            setState(() {
                              _endDateTime = dateTime;
                            });
                          } else {
                            ScaffoldMessenger.of(context).showSnackBar(
                              const SnackBar(
                                content: Text('End time must be after start time'),
                                backgroundColor: AppColors.error,
                              ),
                            );
                          }
                        },
                      ),
                    ),
                  ],
                ),
              ] else ...[
                _buildDateField(
                  label: 'Date',
                  dateTime: _startDateTime!,
                  onChanged: (dateTime) {
                    setState(() {
                      _startDateTime = dateTime;
                      _endDateTime = dateTime.add(const Duration(hours: 23, minutes: 59));
                    });
                  },
                ),
              ],
              const SizedBox(height: AppSpacing.md),
              AppFormField(
                controller: _locationController,
                label: 'Location (optional)',
                hint: 'Enter location',
              ),
              const SizedBox(height: AppSpacing.md),
              AppFormField(
                controller: _meetingLinkController,
                label: 'Meeting Link (optional)',
                hint: 'Enter video meeting link',
                keyboardType: TextInputType.url,
              ),
              const SizedBox(height: AppSpacing.md),
              AppDropdown<EventVisibility>(
                label: 'Visibility',
                value: _visibility,
                items: EventVisibility.values.map((visibility) {
                  return DropdownMenuItem(
                    value: visibility,
                    child: Text(visibility.displayName),
                  );
                }).toList(),
                onChanged: (value) {
                  if (value != null) {
                    setState(() {
                      _visibility = value;
                    });
                  }
                },
              ),
              const SizedBox(height: AppSpacing.md),
              AppDropdown<EventStatus>(
                label: 'Status',
                value: _status,
                items: EventStatus.values.map((status) {
                  return DropdownMenuItem(
                    value: status,
                    child: Text(status.displayName),
                  );
                }).toList(),
                onChanged: (value) {
                  if (value != null) {
                    setState(() {
                      _status = value;
                    });
                  }
                },
              ),
              const SizedBox(height: AppSpacing.md),
              AppText(
                'Reminders',
                variant: TextVariant.label,
              ),
              const SizedBox(height: AppSpacing.sm),
              Wrap(
                spacing: AppSpacing.sm,
                runSpacing: AppSpacing.sm,
                children: [
                  ..._reminderMinutes.map((minutes) {
                    return Chip(
                      label: Text(_formatReminderMinutes(minutes)),
                      onDeleted: () {
                        setState(() {
                          _reminderMinutes.remove(minutes);
                        });
                      },
                    );
                  }).toList(),
                  ActionChip(
                    label: const Text('Add Reminder'),
                    onPressed: _showAddReminderDialog,
                  ),
                ],
              ),
              const SizedBox(height: AppSpacing.md),
              SwitchListTile(
                title: const Text('Recurring'),
                subtitle: const Text('Repeat this event'),
                value: _isRecurring,
                onChanged: (value) {
                  setState(() {
                    _isRecurring = value;
                  });
                },
              ),
              const SizedBox(height: AppSpacing.xl),
              AppButton(
                label: widget.eventId != null ? 'Update Event' : 'Create Event',
                isLoading: _isLoading,
                onPressed: _submitForm,
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildDateTimeField({
    required String label,
    required DateTime dateTime,
    required Function(DateTime) onChanged,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          label,
          variant: TextVariant.label,
        ),
        const SizedBox(height: AppSpacing.xs),
        InkWell(
          onTap: () => _selectDateTime(dateTime, onChanged),
          child: Container(
            padding: const EdgeInsets.all(AppSpacing.md),
            decoration: BoxDecoration(
              border: Border.all(color: AppColors.border),
              borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            ),
            child: Row(
              children: [
                const Icon(Icons.calendar_today, size: 16),
                const SizedBox(width: AppSpacing.sm),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        _formatDate(dateTime),
                        style: const TextStyle(fontSize: 12),
                      ),
                      Text(
                        _formatTime(dateTime),
                        style: const TextStyle(
                          fontSize: 14,
                          fontWeight: FontWeight.w600,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildDateField({
    required String label,
    required DateTime dateTime,
    required Function(DateTime) onChanged,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AppText(
          label,
          variant: TextVariant.label,
        ),
        const SizedBox(height: AppSpacing.xs),
        InkWell(
          onTap: () => _selectDate(dateTime, onChanged),
          child: Container(
            padding: const EdgeInsets.all(AppSpacing.md),
            decoration: BoxDecoration(
              border: Border.all(color: AppColors.border),
              borderRadius: BorderRadius.circular(AppSpacing.radiusSm),
            ),
            child: Row(
              children: [
                const Icon(Icons.calendar_today, size: 16),
                const SizedBox(width: AppSpacing.sm),
                Text(
                  _formatDate(dateTime),
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Future<void> _selectDateTime(
    DateTime current,
    Function(DateTime) onChanged,
  ) async {
    final date = await showDatePicker(
      context: context,
      initialDate: current,
      firstDate: DateTime(2020),
      lastDate: DateTime(2030),
    );

    if (date != null && mounted) {
      final time = await showTimePicker(
        context: context,
        initialTime: TimeOfDay.fromDateTime(current),
      );

      if (time != null) {
        onChanged(DateTime(
          date.year,
          date.month,
          date.day,
          time.hour,
          time.minute,
        ));
      }
    }
  }

  Future<void> _selectDate(
    DateTime current,
    Function(DateTime) onChanged,
  ) async {
    final date = await showDatePicker(
      context: context,
      initialDate: current,
      firstDate: DateTime(2020),
      lastDate: DateTime(2030),
    );

    if (date != null) {
      onChanged(DateTime(date.year, date.month, date.day));
    }
  }

  String _formatDate(DateTime dateTime) {
    return '${dateTime.day}/${dateTime.month}/${dateTime.year}';
  }

  String _formatTime(DateTime dateTime) {
    return '${dateTime.hour.toString().padLeft(2, '0')}:${dateTime.minute.toString().padLeft(2, '0')}';
  }

  String _formatReminderMinutes(int minutes) {
    if (minutes < 60) {
      return '$minutes min before';
    } else if (minutes < 1440) {
      return '${minutes ~/ 60} hour${minutes ~/ 60 > 1 ? 's' : ''} before';
    } else {
      return '${minutes ~/ 1440} day${minutes ~/ 1440 > 1 ? 's' : ''} before';
    }
  }

  Future<void> _showAddReminderDialog() async {
    int? selectedMinutes;

    final result = await showDialog<int>(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Add Reminder'),
        content: StatefulBuilder(
          builder: (context, setState) => Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              RadioListTile<int>(
                title: const Text('5 minutes before'),
                value: 5,
                groupValue: selectedMinutes,
                onChanged: (value) {
                  setState(() {
                    selectedMinutes = value;
                  });
                },
              ),
              RadioListTile<int>(
                title: const Text('15 minutes before'),
                value: 15,
                groupValue: selectedMinutes,
                onChanged: (value) {
                  setState(() {
                    selectedMinutes = value;
                  });
                },
              ),
              RadioListTile<int>(
                title: const Text('30 minutes before'),
                value: 30,
                groupValue: selectedMinutes,
                onChanged: (value) {
                  setState(() {
                    selectedMinutes = value;
                  });
                },
              ),
              RadioListTile<int>(
                title: const Text('1 hour before'),
                value: 60,
                groupValue: selectedMinutes,
                onChanged: (value) {
                  setState(() {
                    selectedMinutes = value;
                  });
                },
              ),
              RadioListTile<int>(
                title: const Text('1 day before'),
                value: 1440,
                groupValue: selectedMinutes,
                onChanged: (value) {
                  setState(() {
                    selectedMinutes = value;
                  });
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
          ElevatedButton(
            onPressed: () => Navigator.pop(context, selectedMinutes),
            child: const Text('Add'),
          ),
        ],
      ),
    );

    if (result != null && !_reminderMinutes.contains(result)) {
      setState(() {
        _reminderMinutes.add(result);
        _reminderMinutes.sort();
      });
    }
  }

  Future<void> _submitForm() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      final input = CreateUpdateEventDto(
        userId: 'user-1',
        title: _titleController.text,
        description: _descriptionController.text.isEmpty
            ? null
            : _descriptionController.text,
        eventType: _selectedType,
        startDateTime: _startDateTime!,
        endDateTime: _endDateTime!,
        isAllDay: _isAllDay,
        location: _locationController.text.isEmpty
            ? null
            : _locationController.text,
        meetingLink: _meetingLinkController.text.isEmpty
            ? null
            : _meetingLinkController.text,
        isRecurring: _isRecurring,
        reminderMinutes: _reminderMinutes,
        color: _selectedColor,
        visibility: _visibility,
        status: _status,
      );

      if (widget.eventId != null) {
        await ref.read(updateEventProvider(widget.eventId!, input).future);
      } else {
        await ref.read(createEventProvider(input).future);
      }

      if (mounted) {
        Navigator.of(context).pop();
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              widget.eventId != null
                  ? 'Event updated successfully'
                  : 'Event created successfully',
            ),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to save event: $e'),
            backgroundColor: AppColors.error,
          ),
        );
      }
    } finally {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }
}
