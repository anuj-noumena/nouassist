import '../models/project_dto.dart';
import '../models/paged_result_dto.dart';
import '../../data/mock/mock_task_service.dart';

/// Mock project service with in-memory data storage
class MockProjectService {
  /// In-memory storage for projects
  static final List<ProjectDto> _projects = _generateMockProjects();

  /// Simulate network delay
  static Future<void> _simulateDelay() async {
    await Future.delayed(const Duration(milliseconds: 500));
  }

  /// Get paginated list of projects
  static Future<PagedResultDto<ProjectDto>> getList({
    int skipCount = 0,
    int maxResultCount = 10,
    String? sorting,
    String? filter,
    ProjectStatus? statusFilter,
    ProjectPriority? priorityFilter,
    String? ownerFilter,
  }) async {
    await _simulateDelay();

    var filtered = List<ProjectDto>.from(_projects);

    // Apply text filter
    if (filter != null && filter.isNotEmpty) {
      filtered = filtered.where((project) {
        final lowerFilter = filter.toLowerCase();
        return project.name.toLowerCase().contains(lowerFilter) ||
            (project.description?.toLowerCase().contains(lowerFilter) ?? false);
      }).toList();
    }

    // Apply status filter
    if (statusFilter != null) {
      filtered = filtered.where((project) => project.status == statusFilter).toList();
    }

    // Apply priority filter
    if (priorityFilter != null) {
      filtered = filtered.where((project) => project.priority == priorityFilter).toList();
    }

    // Apply owner filter
    if (ownerFilter != null && ownerFilter.isNotEmpty) {
      filtered = filtered.where((project) => project.ownerId == ownerFilter).toList();
    }

    // Apply sorting
    if (sorting != null) {
      final parts = sorting.split(' ');
      final field = parts[0];
      final ascending = parts.length < 2 || parts[1].toUpperCase() == 'ASC';

      filtered.sort((a, b) {
        int comparison = 0;
        switch (field.toLowerCase()) {
          case 'name':
            comparison = a.name.compareTo(b.name);
            break;
          case 'priority':
            comparison = a.priority.index.compareTo(b.priority.index);
            break;
          case 'status':
            comparison = a.status.index.compareTo(b.status.index);
            break;
          case 'startdate':
            comparison = a.startDate.compareTo(b.startDate);
            break;
          case 'progress':
            comparison = a.progress.compareTo(b.progress);
            break;
        }
        return ascending ? comparison : -comparison;
      });
    }

    // Apply pagination
    final paginatedItems = filtered.skip(skipCount).take(maxResultCount).toList();

    return PagedResultDto(
      items: paginatedItems,
      totalCount: filtered.length,
    );
  }

  /// Get single project by ID
  static Future<ProjectDto> get(String id) async {
    await _simulateDelay();

    try {
      return _projects.firstWhere((project) => project.id == id);
    } catch (e) {
      throw Exception('Project not found');
    }
  }

  /// Create new project
  static Future<ProjectDto> create(CreateUpdateProjectDto input) async {
    await _simulateDelay();

    // Validate
    if (input.name.isEmpty) {
      throw ValidationException('Name is required');
    }
    if (input.name.length < 3) {
      throw ValidationException('Name must be at least 3 characters');
    }
    if (input.startDate.isAfter(input.endDate)) {
      throw ValidationException('End date must be after start date');
    }

    final newProject = ProjectDto(
      id: DateTime.now().millisecondsSinceEpoch.toString(),
      name: input.name,
      description: input.description,
      parentProjectId: input.parentProjectId,
      startDate: input.startDate,
      endDate: input.endDate,
      status: input.status,
      priority: input.priority,
      viewType: input.viewType,
      teamMembers: input.teamMembers,
      ownerId: input.ownerId,
      workflowId: input.workflowId,
      color: input.color,
      progress: input.progress,
      budget: input.budget,
      actualCost: input.actualCost,
      creationTime: DateTime.now(),
    );

    _projects.insert(0, newProject);
    return newProject;
  }

  /// Update existing project
  static Future<ProjectDto> update(String id, CreateUpdateProjectDto input) async {
    await _simulateDelay();

    final index = _projects.indexWhere((project) => project.id == id);
    if (index == -1) {
      throw Exception('Project not found');
    }

    // Validate
    if (input.name.isEmpty) {
      throw ValidationException('Name is required');
    }
    if (input.name.length < 3) {
      throw ValidationException('Name must be at least 3 characters');
    }
    if (input.startDate.isAfter(input.endDate)) {
      throw ValidationException('End date must be after start date');
    }

    final updatedProject = _projects[index].copyWith(
      name: input.name,
      description: input.description,
      parentProjectId: input.parentProjectId,
      startDate: input.startDate,
      endDate: input.endDate,
      status: input.status,
      priority: input.priority,
      viewType: input.viewType,
      teamMembers: input.teamMembers,
      ownerId: input.ownerId,
      workflowId: input.workflowId,
      color: input.color,
      progress: input.progress,
      budget: input.budget,
      actualCost: input.actualCost,
      lastModificationTime: DateTime.now(),
    );

    _projects[index] = updatedProject;
    return updatedProject;
  }

  /// Delete project
  static Future<void> delete(String id) async {
    await _simulateDelay();

    final initialLength = _projects.length;
    _projects.removeWhere((project) => project.id == id);
    if (_projects.length == initialLength) {
      throw Exception('Project not found');
    }

    // Also delete child projects
    _projects.removeWhere((project) => project.parentProjectId == id);
  }

  /// Get child projects
  static Future<List<ProjectDto>> getChildProjects(String parentId) async {
    await _simulateDelay();
    return _projects.where((p) => p.parentProjectId == parentId).toList();
  }

  /// Generate mock projects
  static List<ProjectDto> _generateMockProjects() {
    final now = DateTime.now();
    final colors = ['#2196F3', '#4CAF50', '#FF9800', '#9C27B0', '#F44336', '#00BCD4'];

    return [
      ProjectDto(
        id: '1',
        name: 'Mobile App Development',
        description: 'Cross-platform mobile application for iOS and Android using Flutter framework',
        startDate: now.subtract(const Duration(days: 90)),
        endDate: now.add(const Duration(days: 120)),
        status: ProjectStatus.active,
        priority: ProjectPriority.critical,
        viewType: ProjectViewType.kanban,
        teamMembers: ['user-1', 'user-2', 'user-3', 'user-4'],
        ownerId: 'user-1',
        workflowId: 'workflow-1',
        color: colors[0],
        progress: 65.5,
        budget: 150000.0,
        actualCost: 95000.0,
        creationTime: now.subtract(const Duration(days: 100)),
      ),
      ProjectDto(
        id: '2',
        name: 'Backend API Refactoring',
        description: 'Migrate legacy REST API to modern GraphQL architecture with microservices',
        parentProjectId: '1',
        startDate: now.subtract(const Duration(days: 60)),
        endDate: now.add(const Duration(days: 90)),
        status: ProjectStatus.active,
        priority: ProjectPriority.high,
        viewType: ProjectViewType.list,
        teamMembers: ['user-1', 'user-2', 'user-5'],
        ownerId: 'user-2',
        workflowId: 'workflow-1',
        color: colors[1],
        progress: 45.0,
        budget: 80000.0,
        actualCost: 40000.0,
        creationTime: now.subtract(const Duration(days: 70)),
      ),
      ProjectDto(
        id: '3',
        name: 'UI/UX Redesign',
        description: 'Complete redesign of user interface with modern design system and accessibility improvements',
        startDate: now.subtract(const Duration(days: 30)),
        endDate: now.add(const Duration(days: 60)),
        status: ProjectStatus.active,
        priority: ProjectPriority.high,
        viewType: ProjectViewType.kanban,
        teamMembers: ['user-3', 'user-4'],
        ownerId: 'user-3',
        workflowId: 'workflow-2',
        color: colors[2],
        progress: 30.0,
        budget: 50000.0,
        actualCost: 18000.0,
        creationTime: now.subtract(const Duration(days: 40)),
      ),
      ProjectDto(
        id: '4',
        name: 'DevOps Infrastructure',
        description: 'Set up CI/CD pipelines, container orchestration, and monitoring infrastructure',
        startDate: now.subtract(const Duration(days: 45)),
        endDate: now.add(const Duration(days: 45)),
        status: ProjectStatus.active,
        priority: ProjectPriority.medium,
        viewType: ProjectViewType.list,
        teamMembers: ['user-1', 'user-5'],
        ownerId: 'user-5',
        workflowId: 'workflow-1',
        color: colors[3],
        progress: 55.0,
        budget: 40000.0,
        actualCost: 25000.0,
        creationTime: now.subtract(const Duration(days: 50)),
      ),
      ProjectDto(
        id: '5',
        name: 'E-commerce Platform',
        description: 'Build comprehensive e-commerce platform with payment gateway integration',
        startDate: now.add(const Duration(days: 15)),
        endDate: now.add(const Duration(days: 180)),
        status: ProjectStatus.planning,
        priority: ProjectPriority.high,
        viewType: ProjectViewType.gantt,
        teamMembers: ['user-2', 'user-3', 'user-4'],
        ownerId: 'user-2',
        workflowId: 'workflow-1',
        color: colors[4],
        progress: 0.0,
        budget: 200000.0,
        actualCost: 0.0,
        creationTime: now.subtract(const Duration(days: 10)),
      ),
      ProjectDto(
        id: '6',
        name: 'Payment Gateway Integration',
        description: 'Integrate Stripe, PayPal, and local payment methods',
        parentProjectId: '5',
        startDate: now.add(const Duration(days: 30)),
        endDate: now.add(const Duration(days: 90)),
        status: ProjectStatus.planning,
        priority: ProjectPriority.high,
        viewType: ProjectViewType.list,
        teamMembers: ['user-2', 'user-4'],
        ownerId: 'user-2',
        workflowId: 'workflow-1',
        color: colors[5],
        progress: 0.0,
        budget: 30000.0,
        actualCost: 0.0,
        creationTime: now.subtract(const Duration(days: 8)),
      ),
      ProjectDto(
        id: '7',
        name: 'Security Audit',
        description: 'Comprehensive security audit and penetration testing of all systems',
        startDate: now.subtract(const Duration(days: 15)),
        endDate: now.add(const Duration(days: 30)),
        status: ProjectStatus.active,
        priority: ProjectPriority.critical,
        viewType: ProjectViewType.list,
        teamMembers: ['user-1', 'user-5'],
        ownerId: 'user-5',
        workflowId: 'workflow-2',
        color: colors[0],
        progress: 40.0,
        actualCost: 15000.0,
        creationTime: now.subtract(const Duration(days: 20)),
      ),
      ProjectDto(
        id: '8',
        name: 'Analytics Dashboard',
        description: 'Real-time analytics dashboard with customizable widgets and reports',
        startDate: now.subtract(const Duration(days: 120)),
        endDate: now.subtract(const Duration(days: 30)),
        status: ProjectStatus.completed,
        priority: ProjectPriority.medium,
        viewType: ProjectViewType.kanban,
        teamMembers: ['user-1', 'user-3'],
        ownerId: 'user-1',
        workflowId: 'workflow-1',
        color: colors[1],
        progress: 100.0,
        budget: 60000.0,
        actualCost: 58000.0,
        creationTime: now.subtract(const Duration(days: 150)),
        lastModificationTime: now.subtract(const Duration(days: 30)),
      ),
      ProjectDto(
        id: '9',
        name: 'Customer Portal',
        description: 'Self-service customer portal with account management and support ticketing',
        startDate: now.subtract(const Duration(days: 90)),
        endDate: now.subtract(const Duration(days: 15)),
        status: ProjectStatus.onHold,
        priority: ProjectPriority.medium,
        viewType: ProjectViewType.list,
        teamMembers: ['user-2', 'user-4'],
        ownerId: 'user-2',
        workflowId: 'workflow-1',
        color: colors[2],
        progress: 70.0,
        budget: 70000.0,
        actualCost: 55000.0,
        creationTime: now.subtract(const Duration(days: 100)),
      ),
      ProjectDto(
        id: '10',
        name: 'Machine Learning Integration',
        description: 'Integrate ML models for recommendation engine and predictive analytics',
        startDate: now.add(const Duration(days: 60)),
        endDate: now.add(const Duration(days: 240)),
        status: ProjectStatus.planning,
        priority: ProjectPriority.low,
        viewType: ProjectViewType.list,
        teamMembers: ['user-1', 'user-5'],
        ownerId: 'user-1',
        workflowId: 'workflow-2',
        color: colors[3],
        progress: 0.0,
        budget: 120000.0,
        actualCost: 0.0,
        creationTime: now.subtract(const Duration(days: 5)),
      ),
      // Additional projects
      ...List.generate(10, (index) {
        final projectIndex = index + 11;
        final statusIndex = index % ProjectStatus.values.length;
        final priorityIndex = index % ProjectPriority.values.length;

        return ProjectDto(
          id: projectIndex.toString(),
          name: _getProjectName(projectIndex),
          description: _getProjectDescription(projectIndex),
          parentProjectId: index % 3 == 0 ? (index % 5 + 1).toString() : null,
          startDate: now.subtract(Duration(days: (index % 100) + 1)),
          endDate: now.add(Duration(days: (index % 150) + 30)),
          status: ProjectStatus.values[statusIndex],
          priority: ProjectPriority.values[priorityIndex],
          viewType: ProjectViewType.values[index % ProjectViewType.values.length],
          teamMembers: ['user-${(index % 5) + 1}', 'user-${((index + 1) % 5) + 1}'],
          ownerId: 'user-${(index % 5) + 1}',
          workflowId: 'workflow-${(index % 2) + 1}',
          color: colors[index % colors.length],
          progress: statusIndex == 3 ? 100.0 : (index % 10) * 10.0,
          budget: (50000 + (index * 10000)).toDouble(),
          actualCost: statusIndex == 3
              ? (50000 + (index * 10000)).toDouble()
              : (25000 + (index * 5000)).toDouble(),
          creationTime: now.subtract(Duration(days: (index % 120) + 1)),
        );
      }),
    ];
  }

  static String _getProjectName(int index) {
    final names = [
      'Cloud Migration Project',
      'CRM System Implementation',
      'Data Warehouse Modernization',
      'Microservices Architecture',
      'Mobile Banking App',
      'Real-time Chat Platform',
      'Inventory Management System',
      'HR Management Portal',
      'IoT Device Integration',
      'Blockchain Implementation',
    ];
    return names[index % names.length];
  }

  static String _getProjectDescription(int index) {
    final descriptions = [
      'Migrate on-premise infrastructure to cloud-based services',
      'Implement comprehensive customer relationship management system',
      'Modernize data warehouse with latest technologies and best practices',
      'Refactor monolithic application into microservices architecture',
      'Develop feature-rich mobile banking application',
      'Build scalable real-time chat and messaging platform',
      'Create comprehensive inventory management and tracking system',
      'Implement HR portal with employee self-service features',
      'Integrate IoT devices for real-time monitoring and control',
      'Implement blockchain for supply chain transparency',
    ];
    return descriptions[index % descriptions.length];
  }
}
