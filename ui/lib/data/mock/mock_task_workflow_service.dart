import '../models/task_workflow_dto.dart';

/// Mock task workflow service
class MockTaskWorkflowService {
  static final List<TaskWorkflowDto> _workflows = _generateMockWorkflows();

  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  static Future<List<TaskWorkflowDto>> getList() async {
    await _simulateDelay();
    return List.from(_workflows);
  }

  static Future<TaskWorkflowDto> get(String id) async {
    await _simulateDelay();
    try {
      return _workflows.firstWhere((workflow) => workflow.id == id);
    } catch (e) {
      throw Exception('Workflow not found');
    }
  }

  static List<TaskWorkflowDto> _generateMockWorkflows() {
    final now = DateTime.now();
    return [
      TaskWorkflowDto(
        id: 'workflow-1',
        name: 'Simple Workflow',
        description: 'Basic task workflow',
        stages: [
          const TaskWorkflowStageDto(
            id: 'stage-1',
            name: 'To Do',
            order: 1,
            color: '#9E9E9E',
            statusCategory: StatusCategory.todo,
            allowedTransitions: ['stage-2'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-2',
            name: 'In Progress',
            order: 2,
            color: '#2196F3',
            statusCategory: StatusCategory.inProgress,
            allowedTransitions: ['stage-3', 'stage-1'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-3',
            name: 'In Review',
            order: 3,
            color: '#FF9800',
            statusCategory: StatusCategory.inProgress,
            allowedTransitions: ['stage-4', 'stage-2'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-4',
            name: 'Done',
            order: 4,
            color: '#4CAF50',
            statusCategory: StatusCategory.done,
            allowedTransitions: [],
          ),
        ],
        isDefault: true,
        creationTime: now.subtract(const Duration(days: 365)),
      ),
      TaskWorkflowDto(
        id: 'workflow-2',
        name: 'Development Workflow',
        description: 'Workflow for development tasks',
        stages: [
          const TaskWorkflowStageDto(
            id: 'stage-5',
            name: 'Backlog',
            order: 1,
            color: '#9E9E9E',
            statusCategory: StatusCategory.todo,
            allowedTransitions: ['stage-6'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-6',
            name: 'Selected for Dev',
            order: 2,
            color: '#3F51B5',
            statusCategory: StatusCategory.todo,
            allowedTransitions: ['stage-7', 'stage-5'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-7',
            name: 'In Development',
            order: 3,
            color: '#2196F3',
            statusCategory: StatusCategory.inProgress,
            allowedTransitions: ['stage-8', 'stage-6'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-8',
            name: 'Code Review',
            order: 4,
            color: '#FF9800',
            statusCategory: StatusCategory.inProgress,
            allowedTransitions: ['stage-9', 'stage-7'],
            requiresComment: true,
          ),
          const TaskWorkflowStageDto(
            id: 'stage-9',
            name: 'Testing',
            order: 5,
            color: '#FFC107',
            statusCategory: StatusCategory.inProgress,
            allowedTransitions: ['stage-10', 'stage-7'],
          ),
          const TaskWorkflowStageDto(
            id: 'stage-10',
            name: 'Done',
            order: 6,
            color: '#4CAF50',
            statusCategory: StatusCategory.done,
            allowedTransitions: [],
          ),
        ],
        taskTypeIds: ['type-2', 'type-3', 'type-6'],
        creationTime: now.subtract(const Duration(days: 300)),
      ),
    ];
  }
}
