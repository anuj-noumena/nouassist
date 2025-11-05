
# NouAssist Domain Model Breakdown

This document outlines the domain models for the NouAssist application, generated from the provided C# source code. The models are broken down by bounded context.

## 1. Agile Domain

This context manages agile development practices like story mapping, retrospectives, and daily standups.

### Aggregates

-   **`DailyStandup`**: Represents a daily standup meeting. It includes participants, their updates, and any blockers raised.
    
-   **`SprintRetrospective`**: Represents a retrospective meeting at the end of a sprint. It contains participants and feedback.
    
-   **`StoryMapping`**: Represents a user story mapping session. It consists of activities (steps in the user journey) and user stories.
    

### Entities

-   **`RetrospectiveFeedback`**: Feedback provided by a participant during a retrospective.
    
-   **`RetrospectiveParticipant`**: A person participating in a retrospective.
    
-   **`StandupBlocker`**: An obstacle or issue raised during a standup.
    
-   **`StandupParticipant`**: A person participating in a daily standup.
    
-   **`StandupUpdate`**: A participant's status update during a standup.
    
-   **`StoryMappingActivity`**: A high-level user activity or step in a story map.
    
-   **`StoryMappingStory`**: A user story that falls under a specific activity in a story map.
    

### Value Objects

-   **`ActionItem`**: A specific, actionable task that comes out of a retrospective.
    
-   **`RetrospectiveInsights`**: A summary of the key takeaways from a retrospective, including what went well, what to improve, and action items.
    

### Domain Services

-   **`SprintMetricsService`**: Calculates sprint-related metrics like velocity and completion rates.
    

### Enums

-   **`RetrospectiveStatus`**: The status of a retrospective (e.g., `Planning`, `InProgress`, `Completed`).
    
-   **`RetrospectiveType`**: The format or type of the retrospective (e.g., `StartStopContinue`, `Mad_Sad_Glad`).
    

### Repositories

-   `IDailyStandupRepository`
    
-   `ISprintRetrospectiveRepository`
    
-   `IStoryMappingRepository`
    

## 2. Task Management Domain

This context is responsible for all aspects of creating, managing, and tracking tasks.

### Aggregates

-   **`TaskAggregate`**: The central aggregate representing a single task. It includes details like status, priority, due date, assignments, and history.
    
-   **`TaskDependencyAggregate`**: Represents a dependency between two tasks.
    
-   **`TaskTemplateAggregate`**: A template for creating new tasks, which can include predefined fields and recurrence patterns.
    

### Entities

-   **`GeneratedInstanceRecord`**: A record of a task being created from a template.
    
-   **`TaskAssignment`**: The assignment of a task to a user.
    
-   **`TaskStatusHistoryEntry`**: A log of status changes for a task.
    
-   **`TemplateVersion`**: A specific version of a task template.
    

### Value Objects

-   **`CustomFieldValue`**: A value for a custom-defined field on a task.
    
-   **`ExecutiveFunctionSupport`**: Provides prompts and suggestions to help with task execution (e.g., breaking down the task).
    
-   **`RecurrencePattern`**: Defines the pattern for a recurring task.
    
-   **`TaskEstimate`**: Contains estimation details for a task, such as complexity and time estimates.
    
-   **`TaskPriority`**: A value object representing the priority of a task based on the Eisenhower Matrix. It contains values for `Urgency` and `Importance` on a scale from 0 to 1.
    

### Domain Services

-   **`TaskCompletionService`**: Handles the logic for completing a task, including validation.
    
-   **`TaskDependencyService`**: Manages and validates dependencies between tasks.
    
-   **`TaskHierarchyService`**: Manages parent-child relationships between tasks.
    
-   **`TaskTemplateInstantiationService`**: Creates new task instances from templates.
    

### Enums

-   **`ComplexityMethod`**: The method used for estimating task complexity (e.g., `FibonacciScale`, `TShirtSizing`).
    
-   **`CustomFieldType`**: The data type of a custom field (e.g., `Text`, `Number`, `Date`).
    
-   **`DependencyType`**: The type of dependency between two tasks (e.g., `FinishToStart`).
    
-   **`TaskStatus`**: The status of a task (e.g., `NotStarted`, `InProgress`, `Completed`).
    

### Repositories

-   `ITaskDependencyRepository`
    
-   `ITaskRepository`
    
-   `ITaskTemplateRepository`
    

### Domain Events (ETOs)

-   `TaskAssignedEto`
    
-   `TaskCompletedEto`
    
-   `TaskCreatedEto`
    
-   `TaskParentChangedEto`
    
-   `TaskStatusUpdatedEto`
    

## 3. Project Management Domain

This context handles the overall management of projects, including their structure, members, and methodologies.

### Aggregates

-   **`Methodology`**: Defines a project management methodology (e.g., Agile, Waterfall).
    
-   **`Project`**: The main aggregate representing a project. It includes details like budget, timeline, members, phases, and milestones.
    
-   **`ProjectType`**: Defines a type of project, which can have default settings and fields.
    

### Entities

-   **`Milestone`**: A significant point or event in a project's timeline.
    
-   **`PhaseSprint`**: A distinct phase or sprint within a project.
    
-   **`ProjectMember`**: A user who is a member of a project team.
    
-   **`ProjectStakeholder`**: An individual or group with an interest in the project.
    

### Value Objects

-   **`BudgetAllocation`**: The allocation of the project budget to different categories.
    
-   **`ContactDetails`**: Contact information for a stakeholder.
    
-   **`MethodologyPrinciples`**: The core principles and workflow of a methodology.
    
-   **`ProjectMetrics`**: Key performance indicators for a project (e.g., budget utilization, schedule variance).
    
-   **`ProjectScope`**: Defines the boundaries of the project, including what is in and out of scope.
    
-   **`ProjectTypeSettings`**: Default settings associated with a project type.
    
-   **`StakeholderProfile`**: A profile of a stakeholder, including their influence, interest, and attitude.
    

### Domain Services

-   **`ProjectValidationService`**: Validates a project's data, for example, before it starts.
    
-   **`StakeholderManagementService`**: Provides services for analyzing and managing project stakeholders.
    

### Enums

-   **`MemberRole`**: The role of a project member (e.g., `ProjectManager`, `Developer`).
    
-   **`MethodologyFramework`**: The specific framework of a methodology (e.g., `Scrum`, `Kanban`).
    
-   **`PhaseType`**: The type of a project phase (e.g., `Phase`, `Sprint`).
    
-   **`ProjectPriority`**: The priority of the project.
    
-   **`ProjectStatus`**: The status of the project (e.g., `Planning`, `Active`, `Completed`).
    
-   **`StakeholderType`**: The type of stakeholder (e.g., `Internal`, `External`).
    

### Repositories

-   `IProjectRepository`
    
-   `IProjectStakeholderRepository`
    

### Domain Events (ETOs)

-   `ProjectCreatedEto`
    
-   `ProjectStatusChangedEto`
    
-   `StakeholderAddedEto`
    

## 4. Requirements Domain

This context deals with the management of project requirements.

### Aggregates

-   **`Requirement`**: Represents a single requirement. It includes details like type, priority, status, and acceptance criteria.
    

### Entities

-   **`RequirementDependency`**: A dependency between two requirements.
    
-   **`RequirementTrace`**: A trace linking a requirement to another entity (e.g., a work item or a test case).
    

### Value Objects

-   **`AcceptanceCriteria`**: The criteria that must be met for the requirement to be considered complete.
    

### Enums

-   **`RequirementPriority`**: The priority of the requirement.
    
-   **`RequirementStatus`**: The status of the requirement (e.g., `Draft`, `Approved`, `Implemented`).
    
-   **`RequirementType`**: The type of requirement (e.g., `Functional`, `NonFunctional`).
    

### Repositories

-   `IRequirementRepository`
    

## 5. Work Items Domain

This context manages the work items that are derived from requirements, such as user stories, tasks, and bugs.

### Aggregates

-   **`Epic`**: A large body of work that can be broken down into smaller work items.
    
-   **`WorkItem`**: Represents a single unit of work, such as a user story, bug, or task.
    

### Entities

-   **`TimeLog`**: A log of time spent on a work item.
    
-   **`WorkItemDependency`**: A dependency between two work items.
    
-   **`WorkItemTask`**: A sub-task of a work item.
    

### Value Objects

-   **`EstimationDetails`**: Details about the estimation of a work item, including story points and hours.
    

### Domain Services

-   **`WorkItemProgressService`**: Calculates the progress of a work item.
    

### Enums

-   **`EpicStatus`**: The status of an epic.
    
-   **`TaskStatus`**: The status of a work item sub-task.
    
-   **`WorkItemPriority`**: The priority of a work item.
    
-   **`WorkItemStatus`**: The status of a work item.
    
-   **`WorkItemType`**: The type of work item (e.g., `UserStory`, `Bug`).
    

### Repositories

-   `IWorkItemRepository`
    

### Domain Events (ETOs)

-   `EpicCompletedEto`
    
-   `WorkItemCompletedEto`
    

## 6. Notifications & Reminders Domain

This context is responsible for handling all user-facing notifications and scheduled reminders across the application.

### Aggregates

-   **`Notification`**: Represents a single notification sent to a user or a group of users. It is generic and can be triggered by various domain events.
    
-   **`Reminder`**: Represents a scheduled reminder that can be linked to any entity in the system (e.g., a Task, a Milestone, a Project).
    

### Entities

-   **`NotificationChannel`**: Represents a delivery channel for notifications, such as Email, In-App, or Push Notification.
    
-   **`NotificationLog`**: Tracks the delivery status of each notification sent through a specific channel.
    
-   **`UserNotificationPreferences`**: Stores individual user preferences for receiving different types of notifications on various channels.
    

### Value Objects

-   **`NotificationContent`**: Contains the actual content of a notification, including the title, body, and any relevant metadata or deep links.
    
-   **`ReminderSchedule`**: Defines the timing for a reminder, which could be a one-time event or a recurring schedule (e.g., using a CRON expression).
    

### Domain Services

-   **`NotificationDispatcher`**: A service responsible for taking a `Notification` aggregate and sending it through the appropriate channels based on user preferences.
    
-   **`ReminderSchedulingService`**: Manages the scheduling and triggering of `Reminder` aggregates.
    

### Enums

-   **`ChannelType`**: The type of notification channel (`Email`, `InApp`, `Push`, `SMS`).
    
-   **`NotificationStatus`**: The status of a notification in the log (`Queued`, `Sent`, `Failed`, `Read`).
    
-   **`NotificationType`**: Categorizes notifications based on the triggering event (`TaskDue`, `ProjectUpdate`, `UserMention`, `SystemAlert`).
    

### Repositories

-   `INotificationRepository`
    
-   `IReminderRepository`
    
-   `IUserNotificationPreferencesRepository`
    

### Domain Events (ETOs)

-   `NotificationDispatchedEto`
    
-   `ReminderTriggeredEto`
    

## 7. Work Management (GTD) Domain

This context implements the "Getting Things Done" (GTD) methodology, focusing on a systematic workflow for managing personal and professional tasks. It helps users capture, clarify, organize, reflect on, and engage with their work.

### Aggregates

-   **`InboxItem` (AR)**: Raw input (ideas, to-dos, emails) captured by the user before processing.
    
-   **`Context` (AR)**: A tag representing the tool, location, or person needed for an action (e.g., `@computer`, `@office`).
    
-   **`ActionableItem` (AR)**: A concrete task that needs to be done. It can be linked to other entities like a `Task` or `Project` for execution details.
    
-   **`WaitingForItem` (AR)**: Tracks delegated tasks or items that are pending input from others.
    
-   **`SomedayMaybeItem` (AR)**: Ideas or tasks that are not commitments for now but might be in the future.
    
-   **`Review` (AR)**: A scheduled session for the user to review their system (e.g., Weekly Review).
    
-   **`Horizon` (AR)**: Represents a level in the GTD Horizons of Focus, from current actions to life purpose.
    
-   **`TicklerItem` (AR)**: An item deferred to a specific future date for follow-up.
    

### Entities

-   **`ReviewChecklistItemStatus`**: Tracks the completion status of individual items within a `Review`.
    

### Value Objects

-   **`ActionableItem`**: `ActionId`, `UserId`, `Description`, `ContextId`, `Status` (NextAction, Delegated, Deferred, Done), **`AssociatedEntityId`** (nullable), **`AssociatedEntityType`** (nullable), `EnergyRequired`, `TimeRequired`.
    
-   **`WaitingForItem`**: `WaitingItemId`, `UserId`, `ItemDescription`, `DelegatedTo`, `FollowUpDate`, `Status`, **`AssociatedEntityId`** (nullable), **`AssociatedEntityType`** (nullable).
    
-   **Other VOs**: `InboxItemId`, `Content`, `ContextId`, `SomedayMaybeItemId`, `ReviewId`, `HorizonId`, `TicklerItemId`.
    

### Key Commands & Policies

-   **`ProcessInboxItem`**: A crucial command that determines if an inbox item is trash, reference material, a someday/maybe item, or requires action. If action is needed, it can trigger the creation of a `Task` in the Task Management domain and then create an `ActionableItem` linked to it.
    
-   **`LinkActionToEntity`**: Command to associate an `ActionableItem` with another entity (e.g., `Task`, `Project`, `Milestone`).
    
-   **Synchronization Policy**: A critical policy to ensure that when an `ActionableItem` is linked to a `Task`, their statuses remain synchronized. For instance, when a `Task` is completed, a domain event should trigger the completion of the linked `ActionableItem`.
    
-   **Two-Minute Rule Policy**: Logic within `ProcessInboxItem` to handle tasks that can be done immediately.
    

### Events & Subscribers

-   **`ActionableItemCreated`**: Published when a new action is defined.
    
-   **`TicklerItemTriggered`**: Published when a deferred item's trigger date is reached.
    
-   **`ActionLinkedToEntity`**: Published when an action is associated with another entity.
    
-   **Critical Subscriber**: This context must subscribe to events from other domains, such as `TaskCompleted` (from Task Management). When a `Task` is completed, this subscriber finds the associated `ActionableItem` and updates its status, ensuring the GTD system reflects reality.

---

## 8. Work Breakdown Structure (WBS) Bounded Context

* **Subdomains:** WBS Core Structure, WBS Dictionary & Scope, WBS Roll-up & Analytics
* **Aggregates (AR = Aggregate Root):**
    * **`WBSNodeAggregate` (AR):** Represents a single hierarchical node in the WBS, which can be a control account, work package, or planning package. It is the primary unit for tracking scope, schedule, and cost roll-ups.
        * *Entities:* None. Child nodes are separate `WBSNodeAggregate` instances linked by ID.
        * *Value Objects:* `WBSNodeId`, `ProjectId` (immutable link to Project BC), `ParentNodeId` (nullable, for hierarchy), `WBSCode` (e.g., "1.2.1"), `NodeTitle`, `NodeType` (ControlAccount, WorkPackage), `NodeStatus` (Planned, InProgress, Completed, OnHold), **`Budget`** (PlannedValue, ActualCost, EarnedValue, EstimateAtCompletion), **`Schedule`** (PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate), **`Progress`** (PercentComplete, CalculationMethod - e.g., Manual, TaskBased), `ResponsibleParty` (UserId or TeamId).
    * **`WBSDictionaryAggregate` (AR):** Provides a detailed, formal description of the work for a single WBS Node. It is managed separately to keep the primary node aggregate focused on status and structure.
        * *Entities:* None.
        * *Value Objects:* `WBSNodeId` (immutable link to a `WBSNodeAggregate`), `StatementOfWork`, `AcceptanceCriteria`, `Assumptions`, `Constraints`, `TechnicalReferences` (List<URL/DocumentLink>).
* **Commands:**
    * `CreateWBSNode`, `UpdateNodeDetails` (Title, Description), `SetNodeParent` (to move a node), `UpdateNodeBudget`, `UpdateNodeSchedule`, `UpdateNodeStatus`, `DeleteWBSNode`, `AssignNodeResponsibility`, `DefineDictionaryForNode`, `UpdateDictionaryContent`.
* **Queries:**
    * `GetWBSNodeById`, `GetWBSForProject` (returns the full tree structure), `GetChildrenForNode`, `GetNodeDictionary`, `GetNodesByStatus`, `GetNodesByResponsibleParty`, `GetProjectBudgetSummary` (queries root node), `SearchNodesByTitleOrCode`.
* **Events:**
    * `WBSNodeCreated` (includes `ProjectId`, `ParentNodeId`), `WBSNodeParentChanged`, `WBSNodeDetailsUpdated`, `WBSNodeStatusUpdated`, `WBSNodeBudgetUpdated`, `WBSNodeScheduleUpdated`, `WBSNodeProgressRecorded`, `WBSNodeDeleted`, `WBSDictionaryDefined`, `WBSDictionaryUpdated`.
* **Event Subscribers/Policies:**
    * *Policy:* A `WBSNode` cannot be deleted if it has child nodes.
    * *Policy:* Prevent circular parenting within the `SetNodeParent` command logic.
    * *Policy:* `WBSNodeStatus` cannot be 'Completed' if any child nodes are not 'Completed'.
    * *Subscriber (Internal):* On `WBSNodeBudgetUpdated` or `WBSNodeProgressRecorded` for a child node, a policy triggers a re-calculation of the parent node's roll-up `Budget` and `Progress` values.
    * *Subscriber (External):* Listens for `TaskStatusUpdated` and `TaskCompleted` events from the Task Management BC. When a task linked to a `WBSNodeId` is updated, this subscriber updates the `Progress` and `ActualCost` on the corresponding `WBSNodeAggregate`.
    * *Subscriber (External):* Publishes events like `WBSNodeStatusUpdated` and `WBSNodeBudgetUpdated` for other BCs (Project Management, Notifications, Analytics).