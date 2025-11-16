import 'package:riverpod_annotation/riverpod_annotation.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import '../data/models/task_dto.dart';
import '../data/mock/mock_task_service.dart';
import 'task_provider.dart';
import 'task_list_provider.dart';

part 'task_form_provider.g.dart';
part 'task_form_provider.freezed.dart';

@riverpod
class TaskForm extends _$TaskForm {
  @override
  TaskFormState build({String? taskId}) {
    if (taskId != null) {
      ref.listen(taskProvider(taskId), (previous, next) {
        next.whenData((task) {
          state = state.copyWith(
            title: task.title,
            description: task.description,
            taskTypeId: task.taskTypeId,
            workflowStageId: task.workflowStageId,
            assigneeId: task.assigneeId,
            reporterId: task.reporterId,
            priority: task.priority,
            status: task.status,
            estimatedHours: task.estimatedHours,
            actualHours: task.actualHours,
            storyPoints: task.storyPoints,
            startDate: task.startDate,
            dueDate: task.dueDate,
            tags: task.tags,
            watchers: task.watchers,
            progress: task.progress,
          );
        });
      });
    }

    return TaskFormState(
      reporterId: 'user-1', // Default reporter
      taskTypeId: 'type-4', // Default to "Task" type
      workflowStageId: 'stage-1', // Default to first stage
      priority: TaskPriority.medium,
      status: TaskStatus.todo,
    );
  }

  void updateTitle(String value) {
    state = state.copyWith(
      title: value,
      isDirty: true,
      errors: {...state.errors}..remove('title'),
    );
  }

  void updateDescription(String? value) {
    state = state.copyWith(
      description: value,
      isDirty: true,
    );
  }

  void updateTaskTypeId(String value) {
    state = state.copyWith(
      taskTypeId: value,
      isDirty: true,
    );
  }

  void updateWorkflowStageId(String value) {
    state = state.copyWith(
      workflowStageId: value,
      isDirty: true,
    );
  }

  void updateAssigneeId(String? value) {
    state = state.copyWith(
      assigneeId: value,
      isDirty: true,
    );
  }

  void updateReporterId(String value) {
    state = state.copyWith(
      reporterId: value,
      isDirty: true,
    );
  }

  void updatePriority(TaskPriority value) {
    state = state.copyWith(
      priority: value,
      isDirty: true,
    );
  }

  void updateStatus(TaskStatus value) {
    state = state.copyWith(
      status: value,
      isDirty: true,
    );
  }

  void updateEstimatedHours(double? value) {
    state = state.copyWith(
      estimatedHours: value,
      isDirty: true,
    );
  }

  void updateActualHours(double? value) {
    state = state.copyWith(
      actualHours: value,
      isDirty: true,
    );
  }

  void updateStoryPoints(int? value) {
    state = state.copyWith(
      storyPoints: value,
      isDirty: true,
    );
  }

  void updateStartDate(DateTime? value) {
    state = state.copyWith(
      startDate: value,
      isDirty: true,
    );
  }

  void updateDueDate(DateTime? value) {
    state = state.copyWith(
      dueDate: value,
      isDirty: true,
    );
  }

  void updateTags(List<String> value) {
    state = state.copyWith(
      tags: value,
      isDirty: true,
    );
  }

  void addTag(String tag) {
    if (!state.tags.contains(tag)) {
      state = state.copyWith(
        tags: [...state.tags, tag],
        isDirty: true,
      );
    }
  }

  void removeTag(String tag) {
    state = state.copyWith(
      tags: state.tags.where((t) => t != tag).toList(),
      isDirty: true,
    );
  }

  void updateWatchers(List<String> value) {
    state = state.copyWith(
      watchers: value,
      isDirty: true,
    );
  }

  void updateProgress(double value) {
    state = state.copyWith(
      progress: value,
      isDirty: true,
    );
  }

  bool validate() {
    final errors = <String, String>{};

    if (state.title.isEmpty) {
      errors['title'] = 'Title is required';
    } else if (state.title.length < 3) {
      errors['title'] = 'Title must be at least 3 characters';
    }

    if (state.taskTypeId.isEmpty) {
      errors['taskTypeId'] = 'Task type is required';
    }

    if (state.workflowStageId.isEmpty) {
      errors['workflowStageId'] = 'Workflow stage is required';
    }

    if (state.reporterId.isEmpty) {
      errors['reporterId'] = 'Reporter is required';
    }

    if (state.estimatedHours != null && state.estimatedHours! < 0) {
      errors['estimatedHours'] = 'Estimated hours must be positive';
    }

    if (state.actualHours != null && state.actualHours! < 0) {
      errors['actualHours'] = 'Actual hours must be positive';
    }

    if (state.storyPoints != null && state.storyPoints! < 0) {
      errors['storyPoints'] = 'Story points must be positive';
    }

    if (state.startDate != null &&
        state.dueDate != null &&
        state.startDate!.isAfter(state.dueDate!)) {
      errors['dueDate'] = 'Due date must be after start date';
    }

    state = state.copyWith(errors: errors);
    return errors.isEmpty;
  }

  Future<TaskDto?> submit() async {
    if (!validate()) return null;

    state = state.copyWith(isSubmitting: true, submitError: null);

    try {
      final dto = CreateUpdateTaskDto(
        title: state.title,
        description: state.description,
        taskTypeId: state.taskTypeId,
        workflowStageId: state.workflowStageId,
        assigneeId: state.assigneeId,
        reporterId: state.reporterId,
        priority: state.priority,
        status: state.status,
        estimatedHours: state.estimatedHours,
        actualHours: state.actualHours,
        storyPoints: state.storyPoints,
        startDate: state.startDate,
        dueDate: state.dueDate,
        tags: state.tags,
        watchers: state.watchers,
        progress: state.progress,
      );

      final TaskDto result;

      if (taskId != null) {
        result = await MockTaskService.update(taskId!, dto);
        ref.invalidate(taskProvider(taskId!));
      } else {
        result = await MockTaskService.create(dto);
      }

      ref.invalidate(taskListProvider);
      state = TaskFormState(
        reporterId: 'user-1',
        taskTypeId: 'type-4',
        workflowStageId: 'stage-1',
        priority: TaskPriority.medium,
        status: TaskStatus.todo,
      );

      return result;
    } on ValidationException catch (e) {
      state = state.copyWith(
        isSubmitting: false,
        submitError: e.message,
        errors: e.fieldErrors ?? {},
      );
      return null;
    } catch (e) {
      state = state.copyWith(
        isSubmitting: false,
        submitError: e.toString(),
      );
      return null;
    }
  }

  void reset() {
    state = TaskFormState(
      reporterId: 'user-1',
      taskTypeId: 'type-4',
      workflowStageId: 'stage-1',
      priority: TaskPriority.medium,
      status: TaskStatus.todo,
    );
  }
}

@freezed
abstract class TaskFormState with _$TaskFormState {
  const factory TaskFormState({
    @Default('') String title,
    String? description,
    @Default('') String taskTypeId,
    @Default('') String workflowStageId,
    String? assigneeId,
    @Default('') String reporterId,
    @Default(TaskPriority.medium) TaskPriority priority,
    @Default(TaskStatus.todo) TaskStatus status,
    double? estimatedHours,
    double? actualHours,
    int? storyPoints,
    DateTime? startDate,
    DateTime? dueDate,
    @Default([]) List<String> tags,
    @Default([]) List<String> watchers,
    @Default(0.0) double progress,
    @Default(false) bool isDirty,
    @Default(false) bool isSubmitting,
    @Default({}) Map<String, String> errors,
    String? submitError,
  }) = _TaskFormState;
}
