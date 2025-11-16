import '../models/task_dto.dart';
import '../models/paged_result_dto.dart';

/// Custom exception for validation errors
class ValidationException implements Exception {
  final String message;
  final Map<String, String>? fieldErrors;

  ValidationException(this.message, {this.fieldErrors});

  @override
  String toString() => message;
}

/// Mock task service with in-memory data storage
class MockTaskService {
  /// In-memory storage for tasks
  static final List<TaskDto> _tasks = _generateMockTasks();

  /// In-memory storage for task types
  static final List<TaskTypeDto> _taskTypes = _generateMockTaskTypes();

  /// Simulate network delay
  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 500));
  }

  /// Get paginated list of tasks
  static Future<PagedResultDto<TaskDto>> getList({
    int skipCount = 0,
    int maxResultCount = 10,
    String? sorting,
    String? filter,
    TaskStatus? statusFilter,
    TaskPriority? priorityFilter,
    String? assigneeFilter,
  }) async {
    await _simulateDelay();

    var filtered = List<TaskDto>.from(_tasks);

    // Apply text filter
    if (filter != null && filter.isNotEmpty) {
      filtered = filtered.where((task) {
        final lowerFilter = filter.toLowerCase();
        return task.title.toLowerCase().contains(lowerFilter) ||
            (task.description?.toLowerCase().contains(lowerFilter) ?? false) ||
            task.tags.any((tag) => tag.toLowerCase().contains(lowerFilter));
      }).toList();
    }

    // Apply status filter
    if (statusFilter != null) {
      filtered = filtered.where((task) => task.status == statusFilter).toList();
    }

    // Apply priority filter
    if (priorityFilter != null) {
      filtered =
          filtered.where((task) => task.priority == priorityFilter).toList();
    }

    // Apply assignee filter
    if (assigneeFilter != null && assigneeFilter.isNotEmpty) {
      filtered = filtered
          .where((task) => task.assigneeId == assigneeFilter)
          .toList();
    }

    // Apply sorting
    if (sorting != null) {
      final parts = sorting.split(' ');
      final field = parts[0];
      final ascending = parts.length < 2 || parts[1].toUpperCase() == 'ASC';

      filtered.sort((a, b) {
        int comparison = 0;
        switch (field.toLowerCase()) {
          case 'title':
            comparison = a.title.compareTo(b.title);
            break;
          case 'priority':
            comparison = a.priority.index.compareTo(b.priority.index);
            break;
          case 'status':
            comparison = a.status.index.compareTo(b.status.index);
            break;
          case 'duedate':
            if (a.dueDate == null && b.dueDate == null) {
              comparison = 0;
            } else if (a.dueDate == null) {
              comparison = 1;
            } else if (b.dueDate == null) {
              comparison = -1;
            } else {
              comparison = a.dueDate!.compareTo(b.dueDate!);
            }
            break;
          case 'creationtime':
            comparison = a.creationTime.compareTo(b.creationTime);
            break;
        }
        return ascending ? comparison : -comparison;
      });
    }

    // Apply pagination
    final paginatedItems =
        filtered.skip(skipCount).take(maxResultCount).toList();

    return PagedResultDto(
      items: paginatedItems,
      totalCount: filtered.length,
    );
  }

  /// Get single task by ID
  static Future<TaskDto> get(String id) async {
    await _simulateDelay();

    try {
      return _tasks.firstWhere((task) => task.id == id);
    } catch (e) {
      throw Exception('Task not found');
    }
  }

  /// Create new task
  static Future<TaskDto> create(CreateUpdateTaskDto input) async {
    await _simulateDelay();

    // Validate
    if (input.title.isEmpty) {
      throw ValidationException('Title is required');
    }
    if (input.title.length < 3) {
      throw ValidationException('Title must be at least 3 characters');
    }

    final newTask = TaskDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      title: input.title,
      description: input.description,
      parentTaskId: input.parentTaskId,
      projectId: input.projectId,
      sprintId: input.sprintId,
      taskTypeId: input.taskTypeId,
      workflowStageId: input.workflowStageId,
      assigneeId: input.assigneeId,
      reporterId: input.reporterId,
      priority: input.priority,
      status: input.status,
      estimatedHours: input.estimatedHours,
      actualHours: input.actualHours,
      storyPoints: input.storyPoints,
      startDate: input.startDate,
      dueDate: input.dueDate,
      tags: input.tags,
      watchers: input.watchers,
      isRecurring: input.isRecurring,
      progress: input.progress,
      creationTime: DateTime.now(),
    );

    _tasks.insert(0, newTask);
    return newTask;
  }

  /// Update existing task
  static Future<TaskDto> update(String id, CreateUpdateTaskDto input) async {
    await _simulateDelay();

    final index = _tasks.indexWhere((task) => task.id == id);
    if (index == -1) {
      throw Exception('Task not found');
    }

    // Validate
    if (input.title.isEmpty) {
      throw ValidationException('Title is required');
    }
    if (input.title.length < 3) {
      throw ValidationException('Title must be at least 3 characters');
    }

    final updatedTask = _tasks[index].copyWith(
      title: input.title,
      description: input.description,
      parentTaskId: input.parentTaskId,
      projectId: input.projectId,
      sprintId: input.sprintId,
      taskTypeId: input.taskTypeId,
      workflowStageId: input.workflowStageId,
      assigneeId: input.assigneeId,
      reporterId: input.reporterId,
      priority: input.priority,
      status: input.status,
      estimatedHours: input.estimatedHours,
      actualHours: input.actualHours,
      storyPoints: input.storyPoints,
      startDate: input.startDate,
      dueDate: input.dueDate,
      tags: input.tags,
      watchers: input.watchers,
      isRecurring: input.isRecurring,
      progress: input.progress,
      lastModificationTime: DateTime.now(),
    );

    _tasks[index] = updatedTask;
    return updatedTask;
  }

  /// Delete task
  static Future<void> delete(String id) async {
    await _simulateDelay();
    
    final initialLength = _tasks.length;
    _tasks.removeWhere((task) => task.id == id);
    if (_tasks.length == initialLength) {
      throw Exception('Task not found');
    }
  }

  /// Get all task types
  static Future<List<TaskTypeDto>> getTaskTypes() async {
    await _simulateDelay();
    return List.from(_taskTypes);
  }

  /// Generate mock task types
  static List<TaskTypeDto> _generateMockTaskTypes() {
    return [
      const TaskTypeDto(
        id: 'type-1',
        name: 'Bug',
        icon: '🐛',
        color: '#D32F2F',
        description: 'Something that needs to be fixed',
      ),
      const TaskTypeDto(
        id: 'type-2',
        name: 'Feature',
        icon: '✨',
        color: '#1976D2',
        description: 'New feature or enhancement',
      ),
      const TaskTypeDto(
        id: 'type-3',
        name: 'Story',
        icon: '📖',
        color: '#388E3C',
        description: 'User story',
      ),
      const TaskTypeDto(
        id: 'type-4',
        name: 'Task',
        icon: '✓',
        color: '#757575',
        description: 'General task',
      ),
      const TaskTypeDto(
        id: 'type-5',
        name: 'Epic',
        icon: '🎯',
        color: '#7B1FA2',
        description: 'Large body of work',
      ),
      const TaskTypeDto(
        id: 'type-6',
        name: 'Improvement',
        icon: '🚀',
        color: '#F57C00',
        description: 'Enhancement or optimization',
      ),
      const TaskTypeDto(
        id: 'type-7',
        name: 'Documentation',
        icon: '📚',
        color: '#0097A7',
        description: 'Documentation task',
      ),
      const TaskTypeDto(
        id: 'type-8',
        name: 'Research',
        icon: '🔬',
        color: '#5D4037',
        description: 'Research or investigation',
      ),
    ];
  }

  /// Generate mock tasks
  static List<TaskDto> _generateMockTasks() {
    final now = DateTime.now();
    final taskTypes = _generateMockTaskTypes();

    return [
      TaskDto(
        id: '1',
        title: 'Implement user authentication system',
        description:
            'Create a comprehensive authentication system with login, registration, and password reset functionality. Include JWT token management and refresh token logic.',
        taskTypeId: taskTypes[1].id,
        workflowStageId: 'stage-2',
        assigneeId: 'user-1',
        reporterId: 'user-3',
        priority: TaskPriority.highest,
        status: TaskStatus.inProgress,
        estimatedHours: 40.0,
        actualHours: 25.0,
        storyPoints: 13,
        startDate: now.subtract(const Duration(days: 5)),
        dueDate: now.add(const Duration(days: 3)),
        tags: ['backend', 'security', 'authentication'],
        watchers: ['user-3', 'user-5'],
        progress: 62.5,
        creationTime: now.subtract(const Duration(days: 10)),
      ),
      TaskDto(
        id: '2',
        title: 'Fix navigation drawer not closing on mobile',
        description:
            'The navigation drawer stays open after selecting a menu item on mobile devices. Need to add auto-close logic.',
        taskTypeId: taskTypes[0].id,
        workflowStageId: 'stage-3',
        assigneeId: 'user-2',
        reporterId: 'user-1',
        priority: TaskPriority.high,
        status: TaskStatus.inReview,
        estimatedHours: 4.0,
        actualHours: 3.5,
        storyPoints: 2,
        startDate: now.subtract(const Duration(days: 2)),
        dueDate: now,
        tags: ['frontend', 'mobile', 'bug'],
        watchers: ['user-1'],
        progress: 90.0,
        creationTime: now.subtract(const Duration(days: 3)),
      ),
      TaskDto(
        id: '3',
        title: 'Design new landing page mockups',
        description:
            'Create high-fidelity mockups for the new landing page including hero section, features, testimonials, and CTA sections.',
        taskTypeId: taskTypes[2].id,
        workflowStageId: 'stage-1',
        assigneeId: 'user-4',
        reporterId: 'user-5',
        priority: TaskPriority.medium,
        status: TaskStatus.todo,
        estimatedHours: 16.0,
        storyPoints: 5,
        startDate: now.add(const Duration(days: 1)),
        dueDate: now.add(const Duration(days: 7)),
        tags: ['design', 'ui', 'marketing'],
        watchers: ['user-5', 'user-3'],
        progress: 0.0,
        creationTime: now.subtract(const Duration(days: 1)),
      ),
      TaskDto(
        id: '4',
        title: 'Optimize database queries for dashboard',
        description:
            'Dashboard is loading slowly. Profile and optimize SQL queries, add proper indexes, and implement caching where appropriate.',
        taskTypeId: taskTypes[5].id,
        workflowStageId: 'stage-2',
        assigneeId: 'user-1',
        reporterId: 'user-3',
        priority: TaskPriority.high,
        status: TaskStatus.inProgress,
        estimatedHours: 12.0,
        actualHours: 8.0,
        storyPoints: 8,
        startDate: now.subtract(const Duration(days: 3)),
        dueDate: now.add(const Duration(days: 2)),
        tags: ['backend', 'performance', 'database'],
        watchers: ['user-3'],
        progress: 66.7,
        creationTime: now.subtract(const Duration(days: 5)),
      ),
      TaskDto(
        id: '5',
        title: 'Write API documentation',
        description:
            'Document all REST API endpoints with request/response examples, authentication requirements, and error codes.',
        taskTypeId: taskTypes[6].id,
        workflowStageId: 'stage-1',
        reporterId: 'user-3',
        priority: TaskPriority.low,
        status: TaskStatus.todo,
        estimatedHours: 8.0,
        storyPoints: 3,
        dueDate: now.add(const Duration(days: 14)),
        tags: ['documentation', 'api'],
        watchers: [],
        progress: 0.0,
        creationTime: now.subtract(const Duration(days: 2)),
      ),
      TaskDto(
        id: '6',
        title: 'Implement real-time notifications',
        description:
            'Add WebSocket support for real-time notifications. Users should receive instant updates for messages, mentions, and system alerts.',
        taskTypeId: taskTypes[1].id,
        workflowStageId: 'stage-4',
        assigneeId: 'user-2',
        reporterId: 'user-5',
        priority: TaskPriority.medium,
        status: TaskStatus.done,
        estimatedHours: 24.0,
        actualHours: 28.0,
        storyPoints: 13,
        startDate: now.subtract(const Duration(days: 20)),
        dueDate: now.subtract(const Duration(days: 5)),
        completedDate: now.subtract(const Duration(days: 3)),
        tags: ['backend', 'websockets', 'notifications'],
        watchers: ['user-5', 'user-1'],
        progress: 100.0,
        creationTime: now.subtract(const Duration(days: 30)),
      ),
      TaskDto(
        id: '7',
        title: 'Research payment gateway options',
        description:
            'Evaluate Stripe, PayPal, and Square payment gateways. Compare features, pricing, and integration complexity.',
        taskTypeId: taskTypes[7].id,
        workflowStageId: 'stage-2',
        assigneeId: 'user-3',
        reporterId: 'user-5',
        priority: TaskPriority.medium,
        status: TaskStatus.inProgress,
        estimatedHours: 16.0,
        actualHours: 10.0,
        storyPoints: 5,
        startDate: now.subtract(const Duration(days: 4)),
        dueDate: now.add(const Duration(days: 5)),
        tags: ['research', 'payment', 'integration'],
        watchers: ['user-5'],
        progress: 62.5,
        creationTime: now.subtract(const Duration(days: 6)),
      ),
      TaskDto(
        id: '8',
        title: 'Memory leak in image carousel',
        description:
            'The image carousel component is causing memory leaks. Images are not being properly disposed when navigating away.',
        taskTypeId: taskTypes[0].id,
        workflowStageId: 'stage-1',
        reporterId: 'user-2',
        priority: TaskPriority.highest,
        status: TaskStatus.blocked,
        estimatedHours: 6.0,
        storyPoints: 5,
        dueDate: now.add(const Duration(days: 1)),
        tags: ['frontend', 'bug', 'memory', 'critical'],
        watchers: ['user-1', 'user-3'],
        progress: 0.0,
        creationTime: now.subtract(const Duration(hours: 8)),
      ),
      TaskDto(
        id: '9',
        title: 'Add dark mode support',
        description:
            'Implement dark mode theming throughout the application. Add toggle in settings and persist user preference.',
        taskTypeId: taskTypes[1].id,
        workflowStageId: 'stage-1',
        assigneeId: 'user-4',
        reporterId: 'user-1',
        priority: TaskPriority.low,
        status: TaskStatus.todo,
        estimatedHours: 20.0,
        storyPoints: 8,
        startDate: now.add(const Duration(days: 10)),
        dueDate: now.add(const Duration(days: 20)),
        tags: ['frontend', 'ui', 'theming'],
        watchers: ['user-1', 'user-5'],
        progress: 0.0,
        creationTime: now.subtract(const Duration(days: 15)),
      ),
      TaskDto(
        id: '10',
        title: 'Set up CI/CD pipeline',
        description:
            'Configure GitHub Actions for automated testing, building, and deployment to staging and production environments.',
        taskTypeId: taskTypes[3].id,
        workflowStageId: 'stage-3',
        assigneeId: 'user-1',
        reporterId: 'user-3',
        priority: TaskPriority.high,
        status: TaskStatus.inReview,
        estimatedHours: 12.0,
        actualHours: 14.0,
        storyPoints: 8,
        startDate: now.subtract(const Duration(days: 6)),
        dueDate: now.add(const Duration(days: 1)),
        tags: ['devops', 'ci-cd', 'automation'],
        watchers: ['user-3'],
        progress: 85.7,
        creationTime: now.subtract(const Duration(days: 8)),
      ),
      // Additional tasks to reach 100 items
      ...List.generate(90, (index) {
        final taskIndex = index + 11;
        final typeIndex = index % taskTypes.length;
        final priorityIndex = index % TaskPriority.values.length;
        final statusIndex = index % TaskStatus.values.length;

        return TaskDto(
          id: taskIndex.toString(),
          title: _getTaskTitle(taskIndex, taskTypes[typeIndex].name),
          description: _getTaskDescription(taskIndex),
          taskTypeId: taskTypes[typeIndex].id,
          workflowStageId: 'stage-${(index % 4) + 1}',
          assigneeId: index % 3 == 0 ? null : 'user-${(index % 5) + 1}',
          reporterId: 'user-${(index % 5) + 1}',
          priority: TaskPriority.values[priorityIndex],
          status: TaskStatus.values[statusIndex],
          estimatedHours: (index % 10 + 1) * 4.0,
          actualHours: index % 2 == 0 ? (index % 10 + 1) * 2.0 : null,
          storyPoints: (index % 13) + 1,
          startDate: index % 2 == 0
              ? now.subtract(Duration(days: index % 30))
              : null,
          dueDate: now.add(Duration(days: (index % 30) + 1)),
          completedDate: statusIndex == 3
              ? now.subtract(Duration(days: index % 10))
              : null,
          tags: _getTaskTags(taskIndex),
          watchers: index % 3 == 0
              ? ['user-${(index % 5) + 1}']
              : [],
          progress: statusIndex == 3
              ? 100.0
              : (statusIndex == 1 || statusIndex == 2
                  ? (index % 10) * 10.0
                  : 0.0),
          creationTime: now.subtract(Duration(days: index % 60 + 1)),
        );
      }),
    ];
  }

  static String _getTaskTitle(int index, String type) {
    final titles = [
      'Update $type dependencies to latest versions',
      'Refactor $type module for better performance',
      'Add unit tests for $type functionality',
      'Investigate $type issues reported by users',
      'Implement $type validation logic',
      'Create $type integration tests',
      'Update $type documentation',
      'Optimize $type rendering performance',
      'Add error handling to $type',
      'Review $type code and provide feedback',
      'Migrate $type to new architecture',
      'Add logging to $type operations',
      'Implement caching for $type',
      'Add accessibility features to $type',
      'Create $type analytics tracking',
      'Fix $type responsive issues',
      'Add internationalization support for $type',
      'Implement $type pagination',
      'Add search functionality to $type',
      'Create $type export feature',
    ];
    return titles[index % titles.length];
  }

  static String _getTaskDescription(int index) {
    final descriptions = [
      'This task requires careful planning and execution. Make sure to follow best practices and coding standards.',
      'Priority task that needs immediate attention. Coordinate with the team before starting.',
      'Research required before implementation. Document findings and propose solutions.',
      'This is a straightforward task. Follow the existing patterns in the codebase.',
      'Complex task that may require multiple iterations. Break it down into smaller subtasks if needed.',
      'User-facing feature. Pay attention to UX and accessibility considerations.',
      'Backend optimization task. Monitor performance metrics after deployment.',
      'Security-related task. Follow security guidelines and best practices.',
      'This task has dependencies on other tasks. Check the task board before starting.',
      'Quick fix needed. Test thoroughly before deploying to production.',
    ];
    return descriptions[index % descriptions.length];
  }

  static List<String> _getTaskTags(int index) {
    final tagSets = [
      ['frontend', 'react', 'ui'],
      ['backend', 'api', 'database'],
      ['mobile', 'ios', 'android'],
      ['testing', 'qa', 'automation'],
      ['devops', 'deployment', 'infrastructure'],
      ['documentation', 'wiki', 'guide'],
      ['performance', 'optimization', 'caching'],
      ['security', 'authentication', 'authorization'],
      ['bug', 'hotfix', 'production'],
      ['feature', 'enhancement', 'improvement'],
    ];
    return tagSets[index % tagSets.length];
  }
}
