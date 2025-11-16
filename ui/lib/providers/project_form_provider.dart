import 'package:riverpod_annotation/riverpod_annotation.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import '../data/models/project_dto.dart';
import '../data/mock/mock_project_service.dart';
import '../data/mock/mock_task_service.dart';
import 'project_provider.dart';
import 'project_list_provider.dart';

part 'project_form_provider.g.dart';
part 'project_form_provider.freezed.dart';

@riverpod
class ProjectForm extends _$ProjectForm {
  @override
  ProjectFormState build({String? projectId}) {
    if (projectId != null) {
      ref.listen(projectProvider(projectId), (previous, next) {
        next.whenData((project) {
          state = state.copyWith(
            name: project.name,
            description: project.description,
            parentProjectId: project.parentProjectId,
            startDate: project.startDate,
            endDate: project.endDate,
            status: project.status,
            priority: project.priority,
            viewType: project.viewType,
            teamMembers: project.teamMembers,
            ownerId: project.ownerId,
            workflowId: project.workflowId,
            color: project.color,
            progress: project.progress,
            budget: project.budget,
            actualCost: project.actualCost,
          );
        });
      });
    }

    return ProjectFormState(
      ownerId: 'user-1',
      workflowId: 'workflow-1',
      startDate: DateTime.now(),
      endDate: DateTime.now().add(const Duration(days: 90)),
      status: ProjectStatus.planning,
      priority: ProjectPriority.medium,
      viewType: ProjectViewType.kanban,
    );
  }

  void updateName(String value) {
    state = state.copyWith(
      name: value,
      isDirty: true,
      errors: {...state.errors}..remove('name'),
    );
  }

  void updateDescription(String? value) {
    state = state.copyWith(description: value, isDirty: true);
  }

  void updateParentProjectId(String? value) {
    state = state.copyWith(parentProjectId: value, isDirty: true);
  }

  void updateStartDate(DateTime value) {
    state = state.copyWith(
      startDate: value,
      isDirty: true,
      errors: {...state.errors}..remove('endDate'),
    );
  }

  void updateEndDate(DateTime value) {
    state = state.copyWith(
      endDate: value,
      isDirty: true,
      errors: {...state.errors}..remove('endDate'),
    );
  }

  void updateStatus(ProjectStatus value) {
    state = state.copyWith(status: value, isDirty: true);
  }

  void updatePriority(ProjectPriority value) {
    state = state.copyWith(priority: value, isDirty: true);
  }

  void updateViewType(ProjectViewType value) {
    state = state.copyWith(viewType: value, isDirty: true);
  }

  void updateOwnerId(String value) {
    state = state.copyWith(ownerId: value, isDirty: true);
  }

  void updateWorkflowId(String value) {
    state = state.copyWith(workflowId: value, isDirty: true);
  }

  void updateColor(String value) {
    state = state.copyWith(color: value, isDirty: true);
  }

  void updateProgress(double value) {
    state = state.copyWith(progress: value, isDirty: true);
  }

  void updateBudget(double? value) {
    state = state.copyWith(budget: value, isDirty: true);
  }

  void updateActualCost(double? value) {
    state = state.copyWith(actualCost: value, isDirty: true);
  }

  bool validate() {
    final errors = <String, String>{};

    if (state.name.isEmpty) {
      errors['name'] = 'Name is required';
    } else if (state.name.length < 3) {
      errors['name'] = 'Name must be at least 3 characters';
    }

    if (state.startDate.isAfter(state.endDate)) {
      errors['endDate'] = 'End date must be after start date';
    }

    if (state.budget != null && state.budget! < 0) {
      errors['budget'] = 'Budget must be positive';
    }

    if (state.actualCost != null && state.actualCost! < 0) {
      errors['actualCost'] = 'Actual cost must be positive';
    }

    state = state.copyWith(errors: errors);
    return errors.isEmpty;
  }

  Future<ProjectDto?> submit() async {
    if (!validate()) return null;

    state = state.copyWith(isSubmitting: true, submitError: null);

    try {
      final dto = CreateUpdateProjectDto(
        name: state.name,
        description: state.description,
        parentProjectId: state.parentProjectId,
        startDate: state.startDate,
        endDate: state.endDate,
        status: state.status,
        priority: state.priority,
        viewType: state.viewType,
        teamMembers: state.teamMembers,
        ownerId: state.ownerId,
        workflowId: state.workflowId,
        color: state.color,
        progress: state.progress,
        budget: state.budget,
        actualCost: state.actualCost,
      );

      final ProjectDto result;

      if (projectId != null) {
        result = await MockProjectService.update(projectId!, dto);
        ref.invalidate(projectProvider(projectId!));
      } else {
        result = await MockProjectService.create(dto);
      }

      ref.invalidate(projectListProvider);
      state = ProjectFormState(
        ownerId: 'user-1',
        workflowId: 'workflow-1',
        startDate: DateTime.now(),
        endDate: DateTime.now().add(const Duration(days: 90)),
        status: ProjectStatus.planning,
        priority: ProjectPriority.medium,
        viewType: ProjectViewType.kanban,
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
    state = ProjectFormState(
      ownerId: 'user-1',
      workflowId: 'workflow-1',
      startDate: DateTime.now(),
      endDate: DateTime.now().add(const Duration(days: 90)),
      status: ProjectStatus.planning,
      priority: ProjectPriority.medium,
      viewType: ProjectViewType.kanban,
    );
  }
}

@freezed
abstract class ProjectFormState with _$ProjectFormState {
  const factory ProjectFormState({
    @Default('') String name,
    String? description,
    String? parentProjectId,
    required DateTime startDate,
    required DateTime endDate,
    required ProjectStatus status,
    required ProjectPriority priority,
    required ProjectViewType viewType,
    @Default([]) List<String> teamMembers,
    required String ownerId,
    required String workflowId,
    @Default('#2196F3') String color,
    @Default(0.0) double progress,
    double? budget,
    double? actualCost,
    @Default(false) bool isDirty,
    @Default(false) bool isSubmitting,
    @Default({}) Map<String, String> errors,
    String? submitError,
  }) = _ProjectFormState;
}
