import '../models/project_workflow_dto.dart';

/// Mock project workflow service
class MockProjectWorkflowService {
  /// In-memory storage for workflows
  static final List<ProjectWorkflowDto> _workflows = _generateMockWorkflows();

  /// Simulate network delay
  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 300));
  }

  /// Get all workflows
  static Future<List<ProjectWorkflowDto>> getList() async {
    await _simulateDelay();
    return List.from(_workflows);
  }

  /// Get single workflow by ID
  static Future<ProjectWorkflowDto> get(String id) async {
    await _simulateDelay();

    try {
      return _workflows.firstWhere((workflow) => workflow.id == id);
    } catch (e) {
      throw Exception('Workflow not found');
    }
  }

  /// Generate mock workflows
  static List<ProjectWorkflowDto> _generateMockWorkflows() {
    return [
      ProjectWorkflowDto(
        id: 'workflow-1',
        name: 'Agile Development',
        description: 'Standard agile development workflow',
        stages: [
          const WorkflowStageDto(
            id: 'stage-1',
            name: 'Backlog',
            order: 1,
            color: '#9E9E9E',
            isInitial: true,
            allowedTransitions: ['stage-2'],
          ),
          const WorkflowStageDto(
            id: 'stage-2',
            name: 'In Progress',
            order: 2,
            color: '#2196F3',
            allowedTransitions: ['stage-3', 'stage-1'],
          ),
          const WorkflowStageDto(
            id: 'stage-3',
            name: 'Review',
            order: 3,
            color: '#FF9800',
            allowedTransitions: ['stage-4', 'stage-2'],
          ),
          const WorkflowStageDto(
            id: 'stage-4',
            name: 'Done',
            order: 4,
            color: '#4CAF50',
            isFinal: true,
            allowedTransitions: [],
          ),
        ],
        isDefault: true,
        creationTime: DateTime.now().subtract(const Duration(days: 365)),
      ),
      ProjectWorkflowDto(
        id: 'workflow-2',
        name: 'Waterfall',
        description: 'Traditional waterfall project workflow',
        stages: [
          const WorkflowStageDto(
            id: 'stage-5',
            name: 'Requirements',
            order: 1,
            color: '#9C27B0',
            isInitial: true,
            allowedTransitions: ['stage-6'],
          ),
          const WorkflowStageDto(
            id: 'stage-6',
            name: 'Design',
            order: 2,
            color: '#3F51B5',
            allowedTransitions: ['stage-7'],
          ),
          const WorkflowStageDto(
            id: 'stage-7',
            name: 'Implementation',
            order: 3,
            color: '#2196F3',
            allowedTransitions: ['stage-8'],
          ),
          const WorkflowStageDto(
            id: 'stage-8',
            name: 'Testing',
            order: 4,
            color: '#FF9800',
            allowedTransitions: ['stage-9'],
          ),
          const WorkflowStageDto(
            id: 'stage-9',
            name: 'Deployment',
            order: 5,
            color: '#4CAF50',
            isFinal: true,
            allowedTransitions: [],
          ),
        ],
        creationTime: DateTime.now().subtract(const Duration(days: 300)),
      ),
    ];
  }
}
