# Workflow-Based Task Delegation

## Overview

The NouAssist system now supports workflow-based task delegation, allowing tasks to be delegated through configurable workflow transitions. This provides a more structured and auditable delegation process compared to direct delegation.

## Architecture

### Key Components

1. **WorkflowActionExecutorService** - Executes workflow actions including task delegation
2. **WorkflowTransitionService** - Manages workflow transitions and validates permissions
3. **TaskInstanceAppService** - Application layer providing delegation endpoints
4. **WorkflowScheme** - Configures delegation policies per project

### Flow

```
User initiates delegation
    ↓
TaskInstanceAppService.DelegateViaWorkflowAsync()
    ↓
Load WorkflowScheme for project
    ↓
Validate delegation is allowed (AllowQuickDelegation)
    ↓
Find appropriate workflow transition (TransitionPurpose.Delegation)
    ↓
WorkflowTransitionService.ExecuteTransitionAsync()
    ↓
WorkflowActionExecutorService executes DelegateTask action
    ↓
TaskInstance.Delegate() updates domain model
    ↓
Events published (TaskDelegatedEto)
    ↓
Workflow status updated
```

## Configuration

### 1. WorkflowScheme Configuration

Configure delegation settings for each project:

```csharp
workflowScheme.SetDelegationMode(
    allowQuick: true,                      // Allow quick delegation
    requireApprovalForIntegrated: false    // Require approval (future feature)
);
```

### 2. Workflow Definition

Create a workflow with delegation transitions:

```csharp
// Create a delegation transition
var delegationTransition = new WorkflowTransition(
    id: Guid.NewGuid(),
    name: "Delegate Task",
    fromStatusId: inProgressStatusId,
    toStatusId: delegatedStatusId,
    purpose: TransitionPurpose.Delegation  // Important: Mark as delegation
);

// Add delegation action
var delegateAction = new WorkflowAction(
    id: Guid.NewGuid(),
    name: "Delegate to User",
    actionType: WorkflowActionType.DelegateTask,
    orderIndex: 1
);

// Configuration is set at runtime by the application
// delegateAction.Configuration["targetUserId"] = targetUserId;
// delegateAction.Configuration["note"] = note;

delegationTransition.AddAction(delegateAction);
workflow.AddTransition(delegationTransition);
```

## Usage

### API Endpoint

**Delegate Task via Workflow**

```http
POST /api/app/task-instance/{id}/delegate-via-workflow
```

**Request Body:**

```json
{
  "delegatedTo": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "note": "Delegating this task to you for your expertise",
  "comment": "Workflow transition comment",
  "transitionId": null  // Optional: specify specific transition
}
```

**Response:**

```json
{
  "id": "task-instance-id",
  "taskId": "task-blueprint-id",
  "assigneeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "delegationInfo": {
    "delegatedBy": "original-user-id",
    "delegatedTo": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "Delegated",
    "note": "Delegating this task to you for your expertise"
  }
}
```

### C# Client Usage

```csharp
var delegateDto = new DelegateTaskViaWorkflowDto
{
    DelegatedTo = targetUserId,
    Note = "Please handle this task",
    Comment = "Delegating via workflow",
    TransitionId = null  // Auto-detect delegation transition
};

var result = await _taskInstanceAppService.DelegateViaWorkflowAsync(
    taskInstanceId,
    delegateDto
);
```

## Delegation Responses

After a task is delegated, the delegate can respond:

### Accept Delegation

```csharp
await _taskInstanceAppService.AcceptDelegationAsync(
    taskInstanceId,
    new AcceptDelegationDto
    {
        ResponseNote = "I accept this task"
    }
);
```

### Decline Delegation

```csharp
await _taskInstanceAppService.DeclineDelegationAsync(
    taskInstanceId,
    new DeclineDelegationDto
    {
        ResponseNote = "I cannot take this task at this time"
    }
);
```

### Revoke Delegation

```csharp
await _taskInstanceAppService.RevokeDelegationAsync(taskInstanceId);
```

## Comparison: Direct vs Workflow Delegation

| Feature | Direct Delegation | Workflow Delegation |
|---------|-------------------|-------------------|
| Speed | Fast | Slightly slower |
| Auditability | Basic | Full workflow audit trail |
| Permissions | Simple permission check | Workflow role-based validation |
| Actions | Delegation only | Can trigger multiple actions |
| Approval | Not supported | Can integrate with approval workflows |
| Status Updates | Manual | Automatic via workflow |
| Flexibility | Limited | Highly configurable |

## When to Use Each Approach

### Use Direct Delegation When:
- Quick ad-hoc delegation is needed
- Simple team workflows
- No need for complex approval processes
- Minimal audit requirements

### Use Workflow Delegation When:
- Formal delegation processes are required
- Need to trigger additional actions (notifications, updates, etc.)
- Approval workflows are needed
- Comprehensive audit trail is important
- Complex permission structures
- Integration with other systems

## Events

The following events are published during workflow-based delegation:

1. **TaskDelegatedEto** - Published when task is delegated
   ```csharp
   {
       TaskInstanceId: Guid,
       DelegatedBy: Guid,
       DelegatedTo: Guid,
       DelegatedAt: DateTime,
       Note: string,
       ViaWorkflow: true  // Distinguishes workflow delegation
   }
   ```

2. **WorkflowTransitionExecutedEto** - Published when workflow transition completes
   ```csharp
   {
       TaskId: Guid,
       WorkflowId: Guid,
       TransitionId: Guid,
       FromStatusId: Guid,
       ToStatusId: Guid,
       ExecutedBy: Guid,
       ExecutedAt: DateTime
   }
   ```

## Error Handling

### Common Errors

| Error Code | Description | Resolution |
|------------|-------------|------------|
| `Tasks:NoWorkflowSchemeForProject` | No active workflow scheme | Create and activate a workflow scheme for the project |
| `Tasks:QuickDelegationNotAllowed` | Quick delegation disabled | Use standard delegation or update scheme settings |
| `Tasks:NoDelegationTransitionFound` | No delegation transition | Add a transition with `TransitionPurpose.Delegation` |
| `Tasks:NoDelegationActionConfigured` | Missing delegation action | Add `WorkflowActionType.DelegateTask` to transition |
| `Workflow:NotAssignedToYou` | User is not assignee | Only the assignee can delegate |
| `Tasks:WorkflowDelegationFailed` | Workflow execution failed | Check workflow configuration and logs |

## Best Practices

1. **Always configure WorkflowScheme** - Ensure each project has an active workflow scheme
2. **Use TransitionPurpose** - Mark delegation transitions with `TransitionPurpose.Delegation`
3. **Validate permissions** - Use workflow role restrictions for delegation transitions
4. **Provide clear notes** - Include context when delegating tasks
5. **Monitor events** - Subscribe to delegation events for tracking and notifications
6. **Handle failures gracefully** - Implement retry logic for transient failures
7. **Test workflow transitions** - Validate all delegation paths before production use

## Future Enhancements

- **Approval Integration**: Require approval before delegation takes effect
- **Delegation Chains**: Track multi-level delegation
- **Auto-escalation**: Automatic delegation on SLA breaches
- **Smart Delegation**: AI-powered delegation suggestions
- **Bulk Delegation**: Delegate multiple tasks via workflow

## Related Documentation

- [Workflow Engine Overview](./WORKFLOW_ENGINE.md)
- [Task Management](./TASK_MANAGEMENT.md)
- [Event System](./EVENT_SYSTEM.md)
- [Permission System](./PERMISSIONS.md)
