# Task and TaskInstance Aggregate Split

## Overview

This document describes the refactoring of the Task Management domain aggregates from 2 large aggregates into 6 focused aggregates, following Domain-Driven Design (DDD) best practices.

## Motivation

The original `Task` and `TaskInstance` aggregates were becoming large and contained multiple concerns with different:
- **Consistency boundaries**: Not all parts needed to change together in the same transaction
- **Lifecycle management**: Different parts had different lifecycles and access patterns
- **Scalability**: Some features (collaboration, dependencies) needed independent scaling
- **Complexity**: Large aggregates made the codebase harder to maintain and test

## Original Structure (Before Split)

### 1. Task Aggregate
- Core blueprint properties (Title, Description, Owner, etc.)
- GTD organization (Area, Context)
- Default values for instances
- Recurrence rules
- **ChecklistItem** collection (child entities)
- **AcceptanceCriterion** collection (child entities)

### 2. TaskInstance Aggregate
- Core execution properties
- Work tracking, Schedule, Priority
- Hierarchy (parent-child relationships)
- **TaskCollaboration** value object (Watchers, comments, attachments)
- **TaskChecklistItem** collection (child entities)
- **TaskDependency** collection (child entities)

## New Structure (After Split)

### 1. Task (Core Blueprint) - `Task.cs`
**Responsibility**: Core task blueprint/template properties

**Contains**:
- Identity (Id, OwnerId, TenantId, ProjectId, TaskTypeId)
- Content (Title, Description, ExpectedResult)
- GTD organization (AreaId, ContextId)
- Default values for instances (importance, urgency, due date, workflow, assignee, etc.)
- Recurrence rules
- State management (IsActive, Version)

**Removed**:
- ChecklistItems → Moved to `TaskTemplate`
- AcceptanceCriteria → Moved to `TaskTemplate`

### 2. TaskTemplate (NEW) - `TaskTemplate.cs`
**Responsibility**: Template specifications (checklist definitions and acceptance criteria)

**Contains**:
- TaskId reference (links to Task aggregate)
- ChecklistItem collection (template definitions)
- AcceptanceCriterion collection (BDD-style specifications)

**Benefits**:
- Separate lifecycle from core task properties
- Can be loaded independently when only template info is needed
- Reduces Task aggregate size

### 3. TaskInstance (Core Execution) - `TaskInstance.cs`
**Responsibility**: Core task execution properties and state

**Contains**:
- Identity (Id, TaskId, WorkflowId, ProjectId, TenantId)
- Ownership (OwnerId, AssigneeId, ReporterId, TeamId)
- GTD organization (AreaId, ContextId)
- Workflow state (WorkflowStatusId)
- Value objects:
  - WorkTracking (progress, start/completion dates)
  - Schedule (planned/actual dates)
  - Priority (ImportanceUrgencyMatrix)
  - Hierarchy (parent-child relationships)
  - CustomFields
- Blocking/flagging state
- Resolution info
- Approval reference (ApprovalRequestId)

**Removed**:
- Collaboration → Moved to `TaskCollaborationAggregate`
- ChecklistItems → Moved to `TaskInstanceChecklist`
- Dependencies → Moved to `TaskDependencyAggregate`

### 4. TaskInstanceChecklist (NEW) - `TaskInstanceChecklist.cs`
**Responsibility**: Manages checklist item completion for a specific task instance

**Contains**:
- TaskInstanceId reference
- TaskChecklistItem collection (instance-specific, tracks checked state)

**Key Operations**:
- InitializeFromTemplate (creates checklist from task template)
- CheckItem / UncheckItem
- CheckAllItems / UncheckAllItems
- CompletionPercentage calculation

**Benefits**:
- Checklist can be loaded/updated independently from main task instance
- Reduces contention on TaskInstance aggregate
- Cleaner separation of concerns

### 5. TaskDependencyAggregate (NEW) - `TaskDependencyAggregate.cs`
**Responsibility**: Manages dependencies for a task instance

**Contains**:
- TaskInstanceId reference
- Dependencies collection (owned collection of DependencyItem value objects)
- Each dependency includes:
  - DependsOnTaskInstanceId
  - DependencyType (FinishToStart, StartToStart, etc.)
  - LagMinutes
  - IsBlocking flag

**Key Operations**:
- AddDependency / RemoveDependency
- UpdateDependency
- ClearAllDependencies
- Circular dependency prevention

**Benefits**:
- Dependencies have independent lifecycle
- Can be queried separately for dependency analysis
- Easier to implement dependency graph algorithms

### 6. TaskCollaborationAggregate (NEW) - `TaskCollaborationAggregate.cs`
**Responsibility**: Manages collaboration features for a task instance

**Contains**:
- TaskInstanceId reference
- Watchers collection (list of user IDs)
- CommentCount (metric)
- AttachmentCount (metric)

**Key Operations**:
- AddWatcher / RemoveWatcher
- AddWatchers / ClearWatchers
- IncrementComments / DecrementComments
- IncrementAttachments / DecrementAttachments
- IsWatching check

**Benefits**:
- Collaboration data can be updated independently
- Reduces lock contention on main TaskInstance
- Can be extended to include actual Comment and Attachment entities in the future
- Scalable for high-frequency updates (likes, watches, etc.)

## Aggregate Relationship Diagram

```
┌──────────────────┐          ┌──────────────────────┐
│      Task        │ 1     1  │   TaskTemplate       │
│  (Blueprint)     │◄─────────┤  (Specifications)    │
│                  │          │                      │
│ - Title          │          │ - ChecklistItems     │
│ - Description    │          │ - AcceptanceCriteria │
│ - GTD Org        │          └──────────────────────┘
│ - Defaults       │
│ - Recurrence     │
└────────┬─────────┘
         │ 1
         │
         │ N
         ▼
┌──────────────────┐          ┌─────────────────────────┐
│  TaskInstance    │ 1     1  │ TaskInstanceChecklist   │
│  (Execution)     │◄─────────┤                         │
│                  │          │ - TaskChecklistItems    │
│ - WorkflowState  │          │ - Completion %          │
│ - WorkTracking   │          └─────────────────────────┘
│ - Schedule       │
│ - Priority       │          ┌─────────────────────────┐
│ - Hierarchy      │ 1     1  │ TaskDependencyAggregate │
│ - CustomFields   │◄─────────┤                         │
└────────┬─────────┘          │ - Dependencies          │
         │                    │ - Blocking checks       │
         │ 1                  └─────────────────────────┘
         │
         │ 1                  ┌──────────────────────────┐
         └───────────────────►│ TaskCollaborationAgg     │
                              │                          │
                              │ - Watchers               │
                              │ - CommentCount           │
                              │ - AttachmentCount        │
                              └──────────────────────────┘
```

## Domain Events

### New Events Added

**TaskTemplate Events**:
- `TaskTemplateCreatedEto`
- `TaskChecklistItemAddedEto` (still used but now from TaskTemplate)
- `TaskChecklistItemRemovedEto`

**TaskInstanceChecklist Events**:
- `TaskInstanceChecklistCreatedEto`
- `TaskChecklistItemCheckedEto`
- `TaskChecklistItemUncheckedEto`

**TaskDependencyAggregate Events**:
- `TaskDependencyAggregateCreatedEto`
- `TaskDependencyAddedEto`
- `TaskDependencyRemovedEto`
- `TaskDependenciesClearedEto`

**TaskCollaborationAggregate Events**:
- `TaskCollaborationAggregateCreatedEto`
- `TaskWatcherAddedEto`
- `TaskWatcherRemovedEto`
- `TaskWatchersClearedEto`
- `TaskCommentAddedEto`
- `TaskAttachmentAddedEto`

## Repository Interfaces

### New Repositories

1. **ITaskTemplateRepository**
   - `GetByTaskIdAsync()` - Get template by task ID
   - `GetWithDetailsAsync()` - Load with all checklist items and criteria
   - `ExistsByTaskIdAsync()` - Check if template exists

2. **ITaskInstanceChecklistRepository**
   - `GetByTaskInstanceIdAsync()` - Get checklist by task instance ID
   - `GetWithDetailsAsync()` - Load with all checklist items
   - `ExistsByTaskInstanceIdAsync()` - Check if checklist exists

3. **ITaskDependencyAggregateRepository**
   - `GetByTaskInstanceIdAsync()` - Get dependencies for a task instance
   - `GetDependentTaskInstancesAsync()` - Find all tasks depending on a given task
   - `WouldCreateCircularDependencyAsync()` - Validate before adding dependency

4. **ITaskCollaborationAggregateRepository**
   - `GetByTaskInstanceIdAsync()` - Get collaboration data for a task instance
   - `GetTaskInstancesWatchedByUserAsync()` - Get all tasks watched by a user
   - `IsUserWatchingAsync()` - Check if user is watching a task

## Database Schema Changes

### New Tables

1. **WorkManagement_TaskTemplates**
   - Id (PK)
   - TaskId (FK to Tasks, unique)
   - TenantId
   - Audit fields (CreationTime, CreatorId, etc.)

2. **WorkManagement_TaskInstanceChecklists**
   - Id (PK)
   - TaskInstanceId (FK to TaskInstances, unique)
   - TenantId
   - Audit fields

3. **WorkManagement_TaskDependencyAggregates**
   - Id (PK)
   - TaskInstanceId (FK to TaskInstances, unique)
   - TenantId
   - Audit fields

4. **WorkManagement_TaskDependencyItems** (owned collection)
   - TaskDependencyAggregateId (FK)
   - DependsOnTaskInstanceId
   - DependencyType
   - LagMinutes
   - IsBlocking

5. **WorkManagement_TaskCollaborationAggregates**
   - Id (PK)
   - TaskInstanceId (FK to TaskInstances, unique)
   - TenantId
   - Watchers (JSONB array)
   - CommentCount
   - AttachmentCount
   - Audit fields

### Modified Tables

**WorkManagement_Tasks**:
- Removed: ChecklistItems relationship
- Removed: AcceptanceCriteria relationship

**WorkManagement_TaskInstances**:
- Removed: Collaboration value object properties
- Removed: ChecklistItems relationship
- Removed: Dependencies relationship

**WorkManagement_ChecklistItems**:
- Now references TaskTemplateId instead of TaskId (via TaskTemplate aggregate)

**WorkManagement_AcceptanceCriteria**:
- Now references TaskTemplateId instead of TaskId (via TaskTemplate aggregate)

**WorkManagement_TaskChecklistItems**:
- Now references TaskInstanceChecklistId instead of TaskInstanceId

## Migration Strategy

### Recommended Approach

1. **Create new aggregates and tables**
   - Run migrations to create new tables
   - Keep old structure intact initially

2. **Data migration**
   - Create TaskTemplate for each existing Task
   - Migrate ChecklistItems and AcceptanceCriteria to TaskTemplates
   - Create TaskInstanceChecklist for each TaskInstance
   - Migrate TaskChecklistItems to new checklists
   - Create TaskDependencyAggregate for each TaskInstance with dependencies
   - Create TaskCollaborationAggregate for each TaskInstance

3. **Update application layer**
   - Update Application Services to work with new aggregates
   - Update queries and DTOs
   - Update UI if necessary

4. **Clean up**
   - Remove old columns/relationships once migration is verified
   - Update indexes

### Data Migration Script Outline

```sql
-- 1. Create TaskTemplate for each Task
INSERT INTO WorkManagement_TaskTemplates (Id, TaskId, TenantId, CreationTime, CreatorId)
SELECT gen_random_uuid(), Id, TenantId, CreationTime, CreatorId
FROM WorkManagement_Tasks;

-- 2. Update ChecklistItems foreign keys
UPDATE WorkManagement_ChecklistItems ci
SET TaskTemplateId = (SELECT Id FROM WorkManagement_TaskTemplates WHERE TaskId = ci.TaskId);

-- 3. Similar migrations for other entities...
```

## Benefits Summary

### 1. **Better Consistency Boundaries**
- Each aggregate now has a clear, focused responsibility
- Reduces unnecessary transaction scope
- Minimizes lock contention

### 2. **Improved Scalability**
- Collaboration data can be updated independently
- Dependencies can be queried without loading entire task
- Checklists can be loaded separately

### 3. **Cleaner Code**
- Smaller, more focused aggregates
- Easier to understand and maintain
- Better separation of concerns

### 4. **Better Performance**
- Reduced payload when querying tasks
- Can load only necessary data
- Less serialization overhead

### 5. **Enhanced Testability**
- Smaller units to test
- Clearer test boundaries
- Easier to mock dependencies

### 6. **Future Extensibility**
- TaskCollaborationAggregate can grow to include Comment and Attachment entities
- Dependency analysis can be enhanced without affecting core task logic
- Template system can be extended independently

## Breaking Changes

### Application Services Impact

Application services that previously worked with Task or TaskInstance aggregates will need updates:

1. **Task creation**: Now need to also create TaskTemplate
2. **TaskInstance creation**: Need to create TaskInstanceChecklist, TaskDependencyAggregate, and TaskCollaborationAggregate
3. **Querying**: May need to load multiple aggregates and compose results
4. **Updates**: Updates to checklists, dependencies, or collaboration now go through separate aggregates

### Example Changes

**Before**:
```csharp
var task = await _taskRepository.GetWithDetailsAsync(id);
task.AddChecklistItem("Item 1");
await _taskRepository.UpdateAsync(task);
```

**After**:
```csharp
var template = await _taskTemplateRepository.GetByTaskIdAsync(taskId);
template.AddChecklistItem("Item 1");
await _taskTemplateRepository.UpdateAsync(template);
```

## Files Modified

### New Files Created

**Aggregates**:
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskTemplate.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskInstanceChecklist.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskDependencyAggregate.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskCollaborationAggregate.cs`

**Repositories**:
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Data/ITaskTemplateRepository.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Data/ITaskInstanceChecklistRepository.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Data/ITaskDependencyAggregateRepository.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Data/ITaskCollaborationAggregateRepository.cs`

**Events** (in Domain.Shared):
- `TaskTemplateCreatedEto.cs`
- `TaskInstanceChecklistCreatedEto.cs`
- `TaskChecklistItemCheckedEto.cs`
- `TaskChecklistItemUncheckedEto.cs`
- `TaskDependencyAggregateCreatedEto.cs`
- `TaskDependencyAddedEto.cs`
- `TaskDependencyRemovedEto.cs`
- `TaskDependenciesClearedEto.cs`
- `TaskCollaborationAggregateCreatedEto.cs`
- `TaskWatcherAddedEto.cs`
- `TaskWatcherRemovedEto.cs`
- `TaskWatchersClearedEto.cs`
- `TaskCommentAddedEto.cs`
- `TaskAttachmentAddedEto.cs`

**EF Core Configurations**:
- `/modules/WorkManagement/src/NouAssist.WorkManagement.EntityFrameworkCore/EntityFrameworkCore/TaskManagement/Configurations/TaskTemplateConfiguration.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.EntityFrameworkCore/EntityFrameworkCore/TaskManagement/Configurations/TaskInstanceChecklistConfiguration.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.EntityFrameworkCore/EntityFrameworkCore/TaskManagement/Configurations/TaskDependencyAggregateConfiguration.cs`
- `/modules/WorkManagement/src/NouAssist.WorkManagement.EntityFrameworkCore/EntityFrameworkCore/TaskManagement/Configurations/TaskCollaborationAggregateConfiguration.cs`

### Files Modified

**Aggregates**:
- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/Task.cs`
  - Removed ChecklistItems collection and related methods
  - Removed AcceptanceCriteria collection and related methods
  - Removed computed properties for checklist/criteria

- `/modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/Aggregates/TaskInstance.cs`
  - Removed Collaboration value object
  - Removed ChecklistItems collection and related methods
  - Removed Dependencies collection and related methods
  - Removed collaboration-related methods (AddWatcher, RemoveWatcher, etc.)

**EF Core**:
- `/modules/WorkManagement/src/NouAssist.WorkManagement.EntityFrameworkCore/EntityFrameworkCore/TaskManagement/TasksDbContext.cs`
  - Added DbSets for new aggregates
  - Registered new configurations

## Next Steps

1. **Implement Repository Implementations**
   - Create EF Core repository implementations for new repositories

2. **Update Application Services**
   - Refactor TaskAppService to work with new aggregates
   - Update DTOs and mappings

3. **Create Data Migration**
   - Write migration scripts to move existing data to new structure
   - Test migration thoroughly

4. **Update Tests**
   - Update unit tests for modified aggregates
   - Create tests for new aggregates
   - Update integration tests

5. **Update Documentation**
   - Update API documentation
   - Update developer guides
   - Create migration guide for consuming applications

## Conclusion

This aggregate split follows DDD best practices by creating smaller, focused aggregates with clear consistency boundaries. The refactoring improves scalability, maintainability, and testability while setting a foundation for future enhancements to the task management domain.

---

**Document Version**: 1.0
**Date**: 2025-11-17
**Author**: Claude AI Assistant
