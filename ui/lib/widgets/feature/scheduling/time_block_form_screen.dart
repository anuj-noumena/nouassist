import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_spacing.dart';
import '../../../data/models/time_block_dto.dart';
import '../../../providers/time_block_provider.dart';
import '../../atomic/app_text.dart';
import '../../atomic/app_button.dart';
import '../../atomic/app_form_field.dart';
import '../../atomic/app_dropdown.dart';

class TimeBlockFormScreen extends ConsumerStatefulWidget {
  final String? timeBlockId;
  final DateTime? initialDate;

  const TimeBlockFormScreen({
    super.key,
    this.timeBlockId,
    this.initialDate,
  });

  @override
  ConsumerState<TimeBlockFormScreen> createState() =>
      _TimeBlockFormScreenState();
}

class _TimeBlockFormScreenState extends ConsumerState<TimeBlockFormScreen> {
  final _formKey = GlobalKey<FormState>();
  final _titleController = TextEditingController();
  final _descriptionController = TextEditingController();
  final _locationController = TextEditingController();

  TimeBlockType _selectedType = TimeBlockType.work;
  DateTime? _startDateTime;
  DateTime? _endDateTime;
  String _selectedColor = '#2196F3';
  bool _isFlexible = false;
  int? _reminderMinutes;
  bool _isRecurring = false;
  bool _isLoading = false;

  final Map<TimeBlockType, String> _typeColors = {
    TimeBlockType.work: '#2196F3',
    TimeBlockType.personal: '#4CAF50',
    TimeBlockType.breakTime: '#FF9800',
    TimeBlockType.meeting: '#9C27B0',
    TimeBlockType.focus: '#F44336',
    TimeBlockType.admin: '#795548',
    TimeBlockType.learning: '#00BCD4',
  };

  @override
  void initState() {
    super.initState();
    if (widget.initialDate != null) {
      _startDateTime = widget.initialDate!.add(const Duration(hours: 9));
      _endDateTime = widget.initialDate!.add(const Duration(hours: 10));
    } else {
      final now = DateTime.now();
      _startDateTime = DateTime(now.year, now.month, now.day, 9, 0);
      _endDateTime = DateTime(now.year, now.month, now.day, 10, 0);
    }

    if (widget.timeBlockId != null) {
      _loadTimeBlock();
    }
  }

  @override
  void dispose() {
    _titleController.dispose();
    _descriptionController.dispose();
    _locationController.dispose();
    super.dispose();
  }

  Future<void> _loadTimeBlock() async {
    try {
      final timeBlock = await ref.read(timeBlockProvider(widget.timeBlockId!).future);
      setState(() {
        _titleController.text = timeBlock.title;
        _descriptionController.text = timeBlock.description ?? '';
        _locationController.text = timeBlock.location ?? '';
        _selectedType = timeBlock.blockType;
        _startDateTime = timeBlock.startDateTime;
        _endDateTime = timeBlock.endDateTime;
        _selectedColor = timeBlock.color;
        _isFlexible = timeBlock.isFlexible;
        _reminderMinutes = timeBlock.reminderMinutes;
        _isRecurring = timeBlock.isRecurring;
      });
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to load time block: $e'),
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
        title: Text(widget.timeBlockId != null ? 'Edit Time Block' : 'New Time Block'),
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
                hint: 'Enter time block title',
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
              AppDropdown<TimeBlockType>(
                label: 'Type',
                value: _selectedType,
                items: TimeBlockType.values.map((type) {
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
              const SizedBox(height: AppSpacing.md),
              AppFormField(
                controller: _locationController,
                label: 'Location (optional)',
                hint: 'Enter location',
              ),
              const SizedBox(height: AppSpacing.md),
              SwitchListTile(
                title: const Text('Flexible timing'),
                subtitle: const Text('This time block can be moved if needed'),
                value: _isFlexible,
                onChanged: (value) {
                  setState(() {
                    _isFlexible = value;
                  });
                },
              ),
              const SizedBox(height: AppSpacing.md),
              AppDropdown<int?>(
                label: 'Reminder',
                value: _reminderMinutes,
                items: [
                  const DropdownMenuItem(
                    value: null,
                    child: Text('No reminder'),
                  ),
                  const DropdownMenuItem(
                    value: 5,
                    child: Text('5 minutes before'),
                  ),
                  const DropdownMenuItem(
                    value: 15,
                    child: Text('15 minutes before'),
                  ),
                  const DropdownMenuItem(
                    value: 30,
                    child: Text('30 minutes before'),
                  ),
                  const DropdownMenuItem(
                    value: 60,
                    child: Text('1 hour before'),
                  ),
                ],
                onChanged: (value) {
                  setState(() {
                    _reminderMinutes = value;
                  });
                },
              ),
              const SizedBox(height: AppSpacing.md),
              SwitchListTile(
                title: const Text('Recurring'),
                subtitle: const Text('Repeat this time block'),
                value: _isRecurring,
                onChanged: (value) {
                  setState(() {
                    _isRecurring = value;
                  });
                },
              ),
              if (_isRecurring) ...[
                const SizedBox(height: AppSpacing.md),
                Container(
                  padding: const EdgeInsets.all(AppSpacing.md),
                  decoration: BoxDecoration(
                    color: AppColors.info.withOpacity(0.1),
                    borderRadius: BorderRadius.circular(AppSpacing.radiusMd),
                  ),
                  child: const Row(
                    children: [
                      Icon(Icons.info_outline, color: AppColors.info, size: 20),
                      SizedBox(width: AppSpacing.sm),
                      Expanded(
                        child: Text(
                          'Recurrence settings coming soon',
                          style: TextStyle(fontSize: 12, color: AppColors.info),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
              const SizedBox(height: AppSpacing.xl),
              AppButton(
                label: widget.timeBlockId != null ? 'Update Time Block' : 'Create Time Block',
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

  String _formatDate(DateTime dateTime) {
    return '${dateTime.day}/${dateTime.month}/${dateTime.year}';
  }

  String _formatTime(DateTime dateTime) {
    return '${dateTime.hour.toString().padLeft(2, '0')}:${dateTime.minute.toString().padLeft(2, '0')}';
  }

  Future<void> _submitForm() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    setState(() {
      _isLoading = true;
    });

    try {
      final input = CreateUpdateTimeBlockDto(
        userId: 'user-1',
        title: _titleController.text,
        description: _descriptionController.text.isEmpty
            ? null
            : _descriptionController.text,
        startDateTime: _startDateTime!,
        endDateTime: _endDateTime!,
        blockType: _selectedType,
        color: _selectedColor,
        isRecurring: _isRecurring,
        location: _locationController.text.isEmpty
            ? null
            : _locationController.text,
        isFlexible: _isFlexible,
        reminderMinutes: _reminderMinutes,
      );

      if (widget.timeBlockId != null) {
        await ref.read(
          updateTimeBlockProvider(widget.timeBlockId!, input).future,
        );
      } else {
        await ref.read(createTimeBlockProvider(input).future);
      }

      if (mounted) {
        Navigator.of(context).pop();
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              widget.timeBlockId != null
                  ? 'Time block updated successfully'
                  : 'Time block created successfully',
            ),
            backgroundColor: AppColors.success,
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Failed to save time block: $e'),
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
