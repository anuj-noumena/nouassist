# Workflow Transition Permission Refactoring - Implementation Summary



## Overview



This document describes the complete refactoring of workflow transition permissions following **Option 2: Full Separation - New Permission Aggregate**. This implementation follows SOLID principles and DDD best practices by separating permission concerns from workflow transition logic.



---



## Architecture Decision



### **Problem**

- WorkflowTransition entity had AllowedRoles and AllowedUserIds properties, violating **Single Responsibility Principle**

- Permissions were tightly coupled to workflow transitions

- No support for project-specific permission overrides

- Difficult to manage permissions independently of workflows



### **Solution**

- Created **TransitionPermissionPolicy** as a separate aggregate root

- Removed all permission-related code from WorkflowTransition entity

- Implemented project-context aware permission checking

- Permission policies can be Global or Project-specific



---



## Changes Made



### 1. **New Domain Components**



#### PermissionPolicyScope Enum

**File:** `/modules/Workflow/src/NouAssist.Workflow.Domain.Shared/Enums/PermissionPolicyScope.cs`



```csharp

public enum PermissionPolicyScope

{

    Global = 0,   // Applies to all projects using this workflow

    Project = 1,  // Applies to specific project only

    Custom = 2    // Future extension point

}

```



#### TransitionPermissionPolicy Aggregate Root

**File:** `/modules/Workflow/src/NouAssist.Workflow.Domain/Aggregates/TransitionPermissionPolicy.cs`



**Key Properties:**

- `Scope` - Global or Project-specific

- `ProjectId` - Required for project-scoped policies

- `WorkflowId` - The workflow this policy applies to

- `TransitionId` - The specific transition being governed

- `AllowedRoles` - List of role names that can perform the transition

- `AllowedUserIds` - List of specific user IDs that can perform the transition

- `IsActive` - Whether the policy is currently active

- `Priority` - For policy evaluation order (higher = first)



**Key Methods:**

- `CanUserPerform(userId, userRoles)` - Permission checking logic

- `AddAllowedRole(role)` / `RemoveAllowedRole(role)`

- `AddAllowedUser(userId)` / `RemoveAllowedUser(userId)`

- `Activate()` / `Deactivate()`

- `SetPriority(priority)`



---



### 2. **Repository Layer**



#### Interface

**File:** `/modules/Workflow/src/NouAssist.Workflow.Domain/Data/ITransitionPermissionPolicyRepository.cs`



**Key Methods:**

- `GetApplicablePolicyAsync(transitionId, projectId)` - Gets the policy that applies (Project > Global)

- `GetPoliciesForTransitionAsync(transitionId, projectId)` - Gets all policies for a transition

- `GetPoliciesByWorkflowAsync(workflowId)` - Gets all policies for a workflow

- `GetPoliciesByProjectAsync(projectId)` - Gets all project-specific policies

- `PolicyExistsAsync(transitionId, projectId, scope)` - Checks if a policy exists



#### Implementation

**File:** `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/Repositories/EfCoreTransitionPermissionPolicyRepository.cs`



**Permission Resolution Strategy:**

1. First, check for project-specific policy (Scope = Project, ProjectId = X)

2. If not found, fallback to global policy (Scope = Global, ProjectId = null)

3. If no policy exists, allow all users (default permissive behavior)



---



### 3. **Database Layer**



#### EF Core Configuration

**File:** `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/Configurations/TransitionPermissionPolicyConfiguration.cs`



**Table:** `WF_TransitionPermissionPolicies`



**Indexes Created:**

- `IX_TransitionPermissionPolicies_Transition_Project_Active` - Primary query index

- `IX_TransitionPermissionPolicies_Workflow_Active` - Query by workflow

- `IX_TransitionPermissionPolicies_Project_Active` - Query by project

- `IX_TransitionPermissionPolicies_Scope_Transition_Active` - Scope filtering

- `IX_TransitionPermissionPolicies_Transition_Priority` - Priority sorting



#### DbContext Updates

**Files Updated:**

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/IWorkflowDbContext.cs`

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/WorkflowDbContext.cs`



Added: `DbSet<TransitionPermissionPolicy> TransitionPermissionPolicies`



---



### 4. **Breaking Changes to WorkflowTransition Entity**



**File:** `/modules/Workflow/src/NouAssist.Workflow.Domain/Entities/WorkflowTransition.cs`



#### Removed Properties:

- ❌ `AllowedRoles` property

- ❌ `AllowedUserIds` property

- ❌ `HasRestrictions` property



#### Removed Methods:

- ❌ `AddAllowedRole(string role)`

- ❌ `AddAllowedUser(Guid userId)`

- ❌ `CanUserPerform(Guid userId, List<string> userRoles)`



#### What Remains:

- ✅ All transition logic (FromStatus, ToStatus, Actions)

- ✅ Conditions, Validators

- ✅ TransitionPurpose

- ✅ All other properties (RequiresComment, ButtonLabel, etc.)



---



### 5. **Application Layer**



#### DTOs Created



**TransitionPermissionPolicyDto**

**File:** `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/TransitionPermissionPolicyDto.cs`



**CreateTransitionPermissionPolicyDto**

**File:** `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/CreateTransitionPermissionPolicyDto.cs`



**UpdateTransitionPermissionPolicyDto**

**File:** `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/UpdateTransitionPermissionPolicyDto.cs`



#### WorkflowTransitionDto Updated

**File:** `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Workflows/Dtos/WorkflowTransitionDto.cs`



**Removed:**

- `AllowedRoles` property

- `AllowedUserIds` property

- `HasRestrictions` property



#### Application Service



**Interface:** `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/ITransitionPermissionPolicyAppService.cs`



**Implementation:** `/modules/Workflow/src/NouAssist.Workflow.Application/Permissions/TransitionPermissionPolicyAppService.cs`



**Key Methods:**

- CRUD operations (Get, Create, Update, Delete, GetList)

- `GetApplicablePolicyAsync(transitionId, projectId)` - Get the policy that applies

- `GetPoliciesForTransitionAsync(transitionId, projectId)` - Get all policies for transition

- `GetPoliciesByWorkflowAsync(workflowId)` - Get all policies for workflow

- `GetPoliciesByProjectAsync(projectId)` - Get all project-specific policies

- `CanUserPerformTransitionAsync(transitionId, userId, userRoles, projectId)` - Permission check

- `ActivateAsync(id)` / `DeactivateAsync(id)`

- `AddAllowedRoleAsync(id, role)` / `RemoveAllowedRoleAsync(id, role)`

- `AddAllowedUserAsync(id, userId)` / `RemoveAllowedUserAsync(id, userId)`



---



### 6. **Service Layer Updates**



#### WorkflowTransitionService

**File:** `/modules/Workflow/src/NouAssist.Workflow.Domain/Services/WorkflowTransitionService.cs`



**Changes:**

- ✅ Added `ITransitionPermissionPolicyRepository` dependency

- ✅ Added `projectId` parameter to `ExecuteTransitionAsync()`

- ✅ Added `projectId` parameter to `ValidateTransitionAsync()`

- ✅ Replaced `transition.CanUserPerform()` with `CheckUserPermissionAsync()`

- ✅ New method: `CheckUserPermissionAsync()` - Queries permission policies



**Method Signature Changes:**

```csharp

// OLD

public async Task<TransitionExecutionResult> ExecuteTransitionAsync(

    Guid taskInstanceId,

    Aggregates.Workflow workflow,

    WorkflowTransition transition,

    Guid currentUserId,

    List<string> userRoles,

    Dictionary<string, object> taskCustomFields = null,

    string comment = null)

 

// NEW

public async Task<TransitionExecutionResult> ExecuteTransitionAsync(

    Guid taskInstanceId,

    Aggregates.Workflow workflow,

    WorkflowTransition transition,

    Guid currentUserId,

    List<string> userRoles,

    Guid? projectId = null,  // ← NEW PARAMETER

    Dictionary<string, object> taskCustomFields = null,

    string comment = null)

```



#### TaskTransitionOrchestrator

**File:** `/modules/Tasks/src/NouAssist.Tasks.Domain/Services/TaskTransitionOrchestrator.cs`



**Changes:**

- ✅ Added `ITransitionPermissionPolicyRepository` dependency

- ✅ Pass `taskInstance.ProjectId` to `ExecuteTransitionAsync()`

- ✅ Replaced `transition.HasRestrictions` and `transition.CanUserPerform()` with `CheckUserPermissionAsync()`

- ✅ New method: `CheckUserPermissionAsync()` - Queries permission policies

- ✅ Updated `GetAvailableTransitionsAsync()` to use new permission logic



---



### 7. **AutoMapper Profile Updates**



**File:** `/modules/Workflow/src/NouAssist.Workflow.Application/WorkflowApplicationAutoMapperProfile.cs`



**Changes:**

- ✅ Added `using NouAssist.Workflow.Permissions.Dtos;`

- ✅ Removed `AllowedRoles`, `AllowedUserIds`, `HasRestrictions` mappings from WorkflowTransitionDto

- ✅ Added mappings for TransitionPermissionPolicy aggregate:

    - `TransitionPermissionPolicy → TransitionPermissionPolicyDto`

    - `CreateTransitionPermissionPolicyDto → TransitionPermissionPolicy`

    - `UpdateTransitionPermissionPolicyDto → TransitionPermissionPolicy`



---



## Permission Checking Flow



### Before (Tightly Coupled)

```

User Request → WorkflowTransition.CanUserPerform(userId, roles)

              → Check AllowedRoles/AllowedUserIds

              → Return true/false

```



### After (Separated Concerns)

```

User Request → WorkflowTransitionService.CheckUserPermissionAsync(transitionId, userId, roles, projectId)

              → ITransitionPermissionPolicyRepository.GetApplicablePolicyAsync(transitionId, projectId)

              → Priority: Project-specific policy > Global policy > Allow all

              → TransitionPermissionPolicy.CanUserPerform(userId, roles)

              → Return true/false

```



---



## Migration Strategy



### Database Migration Required



**Migration Type:** Schema change + data migration



**Steps:**

1. **Create Migration:**

   ```bash

   cd /modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore

   dotnet ef migrations add SeparateTransitionPermissions

   ```



2. **Schema Changes:**

    - Create new table: `WF_TransitionPermissionPolicies`

    - Remove JSON columns from `WF_Transitions`: `AllowedRoles`, `AllowedUserIds`



3. **Data Migration:**

   Before running the migration, implement a data migration step to:

    - Read all existing WorkflowTransitions with AllowedRoles or AllowedUserIds

    - Create TransitionPermissionPolicy records with Scope = Global

    - Set appropriate permissions based on existing data



**Data Migration Script (Manual - Run before dropping columns):**

```sql

-- Example data migration logic

INSERT INTO WF_TransitionPermissionPolicies (

    Id, Scope, ProjectId, WorkflowId, TransitionId, Name, AllowedRoles, AllowedUserIds,

    IsActive, Priority, CreationTime

)

SELECT

    NEWID(),

    0, -- Global scope

    NULL,

    WorkflowId,

    Id AS TransitionId,

    CONCAT(Name, ' - Migrated Permission Policy'),

    AllowedRoles, -- JSON column

    AllowedUserIds, -- JSON column

    1, -- IsActive

    0, -- Priority

    GETUTCDATE()

FROM WF_Transitions

WHERE AllowedRoles IS NOT NULL OR AllowedUserIds IS NOT NULL;

```



---



## Testing Recommendations



### Unit Tests Needed

1. **TransitionPermissionPolicy Aggregate**

    - ✅ CanUserPerform returns true when no restrictions

    - ✅ CanUserPerform returns true when user ID in AllowedUserIds

    - ✅ CanUserPerform returns true when user has allowed role

    - ✅ CanUserPerform returns false when user not allowed

    - ✅ Policy scope validation (project scope requires ProjectId)

    - ✅ Add/Remove role and user methods



2. **Repository**

    - ✅ GetApplicablePolicyAsync returns project policy over global

    - ✅ GetApplicablePolicyAsync returns global when no project policy

    - ✅ GetApplicablePolicyAsync returns null when no policies

    - ✅ Priority ordering works correctly



3. **WorkflowTransitionService**

    - ✅ Permission check uses repository instead of entity method

    - ✅ ProjectId is passed correctly

    - ✅ Fallback to allow all when no policy exists



4. **TaskTransitionOrchestrator**

    - ✅ ProjectId from task instance is passed to workflow service

    - ✅ GetAvailableTransitionsAsync filters by permissions correctly



### Integration Tests Needed

1. **End-to-end permission flow**

    - Create workflow → Create transition → Create permission policy → Execute transition

    - Test project-specific override of global policy

    - Test inactive policies are ignored



---



## SOLID Principles Compliance



### ✅ Single Responsibility Principle (SRP)

- **WorkflowTransition**: Now only responsible for transition logic, conditions, and actions

- **TransitionPermissionPolicy**: Solely responsible for permission management

- Clear separation of concerns



### ✅ Open/Closed Principle (OCP)

- Can add new permission policy types (via Custom scope) without modifying existing code

- Permission checking logic is extensible



### ✅ Liskov Substitution Principle (LSP)

- Repository abstractions can be replaced with different implementations

- Aggregate roots follow DDD patterns consistently



### ✅ Interface Segregation Principle (ISP)

- Repository interfaces expose only necessary methods

- Application service interface is focused on permission management



### ✅ Dependency Inversion Principle (DIP)

- Services depend on repository abstractions, not concrete implementations

- No direct dependencies on infrastructure concerns



---



## Domain-Driven Design (DDD) Compliance



### ✅ Aggregate Boundaries

- **TransitionPermissionPolicy** is its own aggregate root

- **WorkflowTransition** remains an entity within Workflow aggregate

- Clear consistency boundaries



### ✅ Ubiquitous Language

- "Permission Policy" - Clear term for permission rules

- "Scope" - Indicates applicability (Global vs Project)

- "Applicable Policy" - The policy that applies in a given context



### ✅ Repository Pattern

- One repository per aggregate root

- Specialized query methods for domain operations



---



## API Usage Examples



### Creating a Global Permission Policy

```csharp

var policy = await _permissionPolicyAppService.CreateAsync(new CreateTransitionPermissionPolicyDto

{

    Scope = PermissionPolicyScope.Global,

    ProjectId = null,

    WorkflowId = workflowId,

    TransitionId = transitionId,

    Name = "Manager Approval Required",

    AllowedRoles = new List<string> { "Manager", "Admin" },

    Priority = 10

});

```



### Creating a Project-Specific Permission Policy

```csharp

var policy = await _permissionPolicyAppService.CreateAsync(new CreateTransitionPermissionPolicyDto

{

    Scope = PermissionPolicyScope.Project,

    ProjectId = projectId,

    WorkflowId = workflowId,

    TransitionId = transitionId,

    Name = "Project X - Lead Only",

    AllowedUserIds = new List<Guid> { projectLeadUserId },

    Priority = 20 // Higher priority than global

});

```



### Checking Permissions

```csharp

var canPerform = await _permissionPolicyAppService.CanUserPerformTransitionAsync(

    transitionId,

    currentUserId,

    userRoles,

    projectId);

```



---



## Performance Considerations



### Caching Strategy (Recommended)

- Cache permission policies by transition ID + project ID

- Cache duration: 5-15 minutes (policies don't change frequently)

- Invalidate cache when policies are created/updated/deleted



### Query Optimization

- All necessary indexes have been created

- Most common query (transition + project + active) has composite index

- JSON querying for AllowedRoles/AllowedUserIds is supported by EF Core



---



## Benefits of This Approach



1. **✅ Separation of Concerns**: Permissions are completely separate from workflow logic

2. **✅ Project-Specific Permissions**: Can override global permissions per project

3. **✅ Flexibility**: Can easily add new permission types in the future

4. **✅ Maintainability**: Clear boundaries make code easier to understand and modify

5. **✅ Testability**: Can test permissions independently of workflows

6. **✅ Scalability**: Repository pattern allows for optimizations and caching

7. **✅ SOLID Compliance**: Follows all SOLID principles

8. **✅ DDD Compliance**: Proper aggregate boundaries and repository pattern



---



## Files Created



### Domain Layer

- `/modules/Workflow/src/NouAssist.Workflow.Domain.Shared/Enums/PermissionPolicyScope.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Domain/Aggregates/TransitionPermissionPolicy.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Domain/Data/ITransitionPermissionPolicyRepository.cs`



### Infrastructure Layer

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/Repositories/EfCoreTransitionPermissionPolicyRepository.cs`

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/Configurations/TransitionPermissionPolicyConfiguration.cs`



### Application Layer

- `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/TransitionPermissionPolicyDto.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/CreateTransitionPermissionPolicyDto.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/Dtos/UpdateTransitionPermissionPolicyDto.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Permissions/ITransitionPermissionPolicyAppService.cs`

- `/modules/Workflow/src/NouAssist.Workflow.Application/Permissions/TransitionPermissionPolicyAppService.cs`



### Files Modified



#### Domain Layer

- `/modules/Workflow/src/NouAssist.Workflow.Domain/Entities/WorkflowTransition.cs` - Removed permissions

- `/modules/Workflow/src/NouAssist.Workflow.Domain/Services/WorkflowTransitionService.cs` - Updated permission checking

- `/modules/Tasks/src/NouAssist.Tasks.Domain/Services/TaskTransitionOrchestrator.cs` - Updated permission checking



#### Infrastructure Layer

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/IWorkflowDbContext.cs` - Added DbSet

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/WorkflowDbContext.cs` - Added DbSet

- `/modules/Workflow/src/NouAssist.Workflow.EntityFrameworkCore/EntityFrameworkCore/Configurations/WorkflowTransitionConfiguration.cs` - Removed permission mappings



#### Application Layer

- `/modules/Workflow/src/NouAssist.Workflow.Application.Contracts/Workflows/Dtos/WorkflowTransitionDto.cs` - Removed permission properties

- `/modules/Workflow/src/NouAssist.Workflow.Application/WorkflowApplicationAutoMapperProfile.cs` - Updated mappings



---



## Next Steps (After Deployment)



1. **Create and run database migration**

2. **Monitor performance** - Add caching if needed

3. **Create UI** - Admin interface for managing permission policies

4. **Documentation** - User guide for configuring transition permissions

5. **Consider deprecation** - Old permission data in backups can be removed after migration is stable



---



## Summary



This refactoring successfully implements a **clean separation between workflow transitions and permissions**, following **SOLID principles** and **DDD best practices**. The new design is more flexible, maintainable, and testable, while supporting project-specific permission overrides that weren't possible before.



The implementation is complete and ready for:

1. Database migration creation

2. Data migration from old structure

3. Testing

4. Deployment



---



**Date:** 2025-11-06

**Implementation:** Complete

**Status:** Ready for Migration & Testing