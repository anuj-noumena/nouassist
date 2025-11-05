## 1. Bounded Context Overview & Subdomains

1.  **Task Management:** (Core Subdomain - Strategic) Handles the lifecycle, properties, structure (subtasks), assignment history, dependencies, and templating of individual tasks. Links tasks to WBS and potentially GTD Actions.
2.  **Project Management:** (Core Subdomain - Strategic) Manages projects, milestones, complex Work Breakdown Structure (WBS) nodes, progress tracking (including roll-up from tasks), risk, issues, and agile aspects (boards, sprints).
3.  **Work Management (GTD):** (Core Subdomain - Strategic, depends on app focus) Implements the Getting Things Done methodology - Inbox, Contexts, Actions (potentially linked to Tasks), Waiting For, Someday/Maybe, Reviews, Horizons, Tickler. Requires careful synchronization logic if linking heavily to Tasks.
4.  **Time Management:** (Core Subdomain - Strategic) Deals with scheduling, availability, time blocking, routines, time tracking, focus sessions, breaks, and estimation accuracy tracking (comparing Task estimates to actual Time Entries).
5.  **Area Management:** (Supporting Subdomain) Organizes work, projects, and resources into logical areas and manages resources within them, including basic allocation checks against capacity.
6.  **Collaboration:** (Supporting Subdomain) Facilitates communication around tasks and projects (comments, mentions).
7.  **Notifications:** (Generic Subdomain) Manages the generation, configuration, and delivery of notifications based on events across the system, respecting user preferences.
8.  **Analytics Platform:** (Supporting Subdomain) Aggregates event data from other BCs into read models/data stores for cross-cutting reporting, KPIs, and dashboards. Manages configuration of reports/dashboards.
9.  **Learning System:** (Supporting/Strategic Subdomain - depends on AI focus) Handles ML/AI aspects for predictions, suggestions, pattern recognition, and adaptation based on user data. Requires clear interfaces for consuming data and providing insights.
10. **Identity & Access Management (IAM):** (Generic Subdomain) Manages users, roles, permissions, and authentication/authorization, leveraging ABP framework capabilities. User profile aspects (theme, language) are managed by ABP.
11. **Reference Management:** (Generic Subdomain) Manages storage, organization, and linking of documents, notes, and other reference materials.

## 2. DDD Elements per Bounded Context

---

### A. Task Management Bounded Context

* **Subdomains:** Task Core, Dependencies, Task Templates & Recurrence
* **Aggregates (AR = Aggregate Root):**
    * **`TaskAggregate` (AR):** Represents a single, potentially hierarchical, actionable item.
        * *Entities:* `TaskAssignment` (tracks current/historical assignments with timestamps), `TaskStatusHistoryEntry`, `TaskReminder`.
        * *Value Objects:*
            * `TaskId`, `TaskTitle`, `TaskDescription`, `DueDate`, `Priority`.
            * **`ParentTaskId`** (nullable link to another `TaskAggregate`, forms the hierarchy).
            * `TaskEstimate` (ComplexityValue, ComplexityMethod, CognitiveLoadScale, CognitiveLoadValue, TimeBest, TimeWorst, TimeLikely).
            * `EstimatedStartDate`, `CustomFieldValue` (Type, Name, Value), `TaskStatus` (Standard or Custom).
            * `ExecutiveFunctionSupport` (BreakdownPrompts, TimeboxingSuggestion).
            * **`ProjectId`** (nullable link).
            * **`ProjectSectionId`** (nullable link to Project BC's Section).
            * **`MilestoneId`** (nullable link to Project BC's Milestone).
            * `SourceTemplateId` (nullable link to originating TaskTemplate).
    * **`TaskTemplateAggregate` (AR):** Defines a reusable task structure. (No changes to this aggregate).
        * *Entities:* `TemplateVersion`, `GeneratedInstanceRecord`.
        * *Value Objects:* `TemplateId`, `TemplateName`, `PredefinedFields`, `PredefinedWorkflow`, `TemplateTags`, `RecurrencePattern`, `StartDate`, `CreationTimeOfDay`, `PropagationOptions`, `IsRecurring`.
    * **`TaskDependencyAggregate` (AR):** Explicitly represents a dependency link (e.g., Finish-to-Start) between two tasks. This is separate from the parent-child hierarchy. (No changes to this aggregate).
        * *Value Objects:* `PredecessorTaskId`, `SuccessorTaskId`, `Type` (FS, SS, FF, SF), `LagLeadTime`, `DependencyStatus`.
* **Commands:**
    * `CreateTask` (optionally accepts **`ParentTaskId`**, `ProjectId`, **`ProjectSectionId`**, **`MilestoneId`**, `SourceTemplateId`).
    * `SetTaskTitle`, `SetTaskDescription`, `SetTaskDueDate`, `SetTaskPriority`, `SetTaskCustomField`.
    * `SetTaskReminder`, `SetTaskEstimate`, `SetTaskEstimatedStartDate`, `UpdateTaskStatus`.
    * **`SetTaskParent`** (Moves a task under a new parent or makes it a root task).
    * **`SetTaskProjectSection`**, **`SetTaskMilestone`**.
    * `CancelTask`, `ArchiveTask`, `RestoreTask`.
    * `AssignTaskToUser`, `ChangeTaskAssignment`.
    * (All `TaskTemplate` and `TaskDependency` commands remain unchanged).
* **Queries:**
    * `GetTaskById`, `GetTaskHistory`, `GetTaskAssignmentHistory`.
    * **`GetChildTasks`** (replaces `GetSubtasksForTask`).
    * **`GetTaskHierarchy`** (fetches a root task and all its descendants).
    * `GetTasksByStatus`, `GetTasksByAssignee`, `GetTasksByDueDateRange`, `GetOverdueTasks`.
    * `GetTasksByProject`, **`GetRootTasksForProject`** (gets all tasks in a project with no parent).
    * **`GetTasksByProjectSection`**, **`GetTasksByMilestone`**.
    * `GetTasksBySourceTemplate`, `GetTaskTemplateById`, `ListTaskTemplates`, `GetDependenciesForTask`.
    * `SearchTasks`, `SearchTaskTemplates`.
* **Events:**
    * `TaskCreated` (includes optional **`ParentTaskId`**, `ProjectId`, **`ProjectSectionId`**, **`MilestoneId`**, `SourceTemplateId`).
    * `TaskPropertiesUpdated`, `TaskStatusUpdated`, `TaskCompleted`, `TaskCancelled`, `TaskArchived`, `TaskRestored`.
    * **`TaskParentChanged`**.
    * **`TaskProjectSectionChanged`**, **`TaskMilestoneChanged`**.
    * `TaskReminderScheduled`, `TaskAssigned`, `TaskReassigned`.
    * (All `TaskTemplate` and `TaskDependency` events remain unchanged).
* **Event Subscribers/Policies:**
    * *Policy:* **Cannot complete Task if open child Tasks exist.**
    * *Policy:* Prevent circular parenting (`SetTaskParent` command must validate that a task is not moved under one of its own descendants).
    * *Policy:* Prevent circular dependencies (`DefineTaskDependency` command).
    * *Policy:* Check user permissions via IAM before executing commands.
    * *Subscriber (Internal):* On `TaskDueDateUpdated` or `TaskCompleted` of a predecessor, check and potentially update dependent Tasks via `TaskDependency` AR.
    * *Subscriber (External):* Publishes events like `TaskCreated`, `TaskStatusUpdated` (with `ProjectId`, `ProjectSectionId`, `MilestoneId`), `TaskCompleted`, `TaskAssigned` for other BCs (Notifications, Analytics, Project Management).

---

### B. Project Management Bounded Context

* **Subdomains:** Project Core, Progress Tracking

**Aggregates (AR = Aggregate Root):**
* **`Project` (AR):** Represents a personal project or hobby with organized tasks.
    * *Entities:* `Milestone`, `ProjectSection` (simple grouping of related tasks)
    * *Value Objects:* `ProjectId`, `ProjectName`, `ProjectDescription`, `Timeline` (StartDate, EndDate), `ProjectStatus`, `Progress` (percentage)

* **`ProjectTemplate` (AR):** Simple reusable project structure.
    * *Value Objects:* `TemplateId`, `TemplateName`, `PredefinedSections`, `DefaultSettings`

**Commands:**
* `CreateProject`, `UpdateProjectDetails`, `SetProjectTimeline`, `AddProjectSection`, `UpdateSectionName`, `DefineProjectMilestone`, `CompleteMilestone`, `UpdateProjectStatus`, `ArchiveProject`, `CreateProjectFromTemplate`

**Queries:**
* `GetProjectById`, `GetAllProjects`, `GetProjectsByStatus`, `GetMilestonesForProject`, `GetProjectProgress`, `GetProjectSections`

**Events:**
* `ProjectCreated`, `ProjectUpdated`, `ProjectStatusUpdated`, `MilestoneCompleted`, `ProjectArchived`, `SectionAdded`

**Event Subscribers/Policies:**
* *Policy:* Cannot archive Project if incomplete tasks exist
* *Subscriber:* Listens to `TaskCompleted` events from Task BC to update project progress
* *Subscriber:* On `MilestoneCompleted`, check if project should be marked complete

---

### C. Work Management (GTD) Bounded Context

* **Subdomains:** GTD Workflow
* **Aggregates (AR = Aggregate Root):**
    * **`InboxItem` (AR):** Raw input captured by the user.
        * *Value Objects:* `InboxItemId`, `Content`, `Source`, `CaptureTime`, `ProcessedStatus`.
    * **`Context` (AR):** A tag representing the situation/tool needed for an action.
        * *Value Objects:* `ContextId`, `UserId`, `Name`, `Type`, `ParentContextId`, `IsActive`.
    * **`ActionableItem` (AR):** An item determined to require action. *Crucially, may link to a `Task` in Task BC for execution details.*
        * *Value Objects:* `ActionId`, `UserId`, `Description`, `ContextId`, `Status` (NextAction, Delegated, Deferred, Done), **`AssociatedTaskId`** (nullable link to Task BC `Task`), `AssociatedProjectId` (link to Project BC), `EnergyRequired`, `TimeRequired`.
    * **`WaitingForItem` (AR):** Tracks items delegated or waiting on external input.
        * *Value Objects:* `WaitingItemId`, `UserId`, `ItemDescription`, `DelegatedTo`, `FollowUpDate`, `Status`, `AssociatedActionId`.
    * **`SomedayMaybeItem` (AR):** Ideas or tasks for potential future consideration.
        * *Value Objects:* `SomedayMaybeItemId`, `UserId`, `Description`, `Category`, `ReviewFrequency`, `LastReviewedDate`, `ActivationCriteria`.
    * **`Review` (AR):** Represents a scheduled review session.
        * *Entities:* `ReviewChecklistItemStatus`.
        * *Value Objects:* `ReviewId`, `UserId`, `ReviewType`, `ScheduledDate`, `CompletionDate`, `Status`, `OutcomeNotes`.
    * **`Horizon` (AR):** Represents a level in the GTD Horizons of Focus.
        * *Value Objects:* `HorizonId`, `UserId`, `Level`, `Statement`, `ParentHorizonId`, `LastReviewedDate`.
    * **`TicklerItem` (AR):** An item deferred to a specific future date.
        * *Value Objects:* `TicklerItemId`, `UserId`, `TargetEntityId`, `TargetEntityType`, `TriggerDate`, `Status`.
* **Commands:**
    * `CaptureInboxItem`, **`ProcessInboxItem`** (Complex: logic determines if item is reference, trash, someday, requires action. If action: may issue `CreateTask` command to Task BC, then creates `ActionableItem` with `AssociatedTaskId`, or links to existing Task), `DefineContext`, `UpdateContext`, `DeactivateContext`, `MarkItemAsActionable` (potentially triggers linking/creation of Task), `AssignContextToAction`, `SetActionAsNextAction`, `CompleteAction` (may need to sync with linked Task), `DelegateAction` (creates `WaitingForItem`), `DeferAction` (creates `TicklerItem`), **`LinkActionToTask`**, **`UnlinkActionFromTask`**, `CreateWaitingForItem`, `MarkWaitingItemReceived`, `SetWaitingItemFollowUpDate`, `CreateSomedayMaybeItem`, `ReviewSomedayMaybeItem`, `ScheduleReview`, `StartReview`, `CompleteReviewChecklistItem`, `CompleteReview`, `DefineHorizon`, `LinkProjectToHorizon`, `LinkActionToHorizon`, `CreateTicklerItem`, `ProcessTriggeredTicklerItem`.
* **Queries:**
    * `GetInboxItems`, `GetContextsByUser`, `GetNextActionsByUser`, `GetActionsByProject`, `GetActionsLinkedToTask`, `GetWaitingForItemsByUser`, `GetOverdueWaitingForItems`, `GetSomedayMaybeItemsByUser`, `GetScheduledReviews`, `GetReviewHistory`, `GetHorizonsByUser`, `GetItemsForHorizonLevel`, `GetTicklerItemsByUser`, `GetDueTicklerItems`.
* **Events:**
    * `InboxItemCaptured`, `InboxItemProcessed`, `ContextDefined`, `ActionableItemCreated` (may include `AssociatedTaskId`), `NextActionSet`, `ActionCompleted` (may be triggered by linked Task completion), `ItemDelegated`, `ItemDeferred`, `SomedayMaybeItemCreated`, `SomedayMaybeItemActivated`, `ReviewScheduled`, `ReviewCompleted`, `HorizonDefined`, `TicklerItemTriggered`, **`ActionLinkedToTask`**, **`ActionUnlinkedFromTask`**.
* **Event Subscribers/Policies:**
    * *Policy (Two-Minute Rule):* `ProcessInboxItem` logic.
    * *Policy:* Ensure Next Actions exist for active projects.
    * **Policy (Synchronization):** If `ActionableItem` is linked to a `Task`, its `Status` should ideally reflect the `Task.Status` (e.g., Done when Task is Completed). This requires careful handling of updates from both contexts. Define which system is the master for status or how conflicts are resolved.
    * *Subscriber (Internal):* `ProcessInboxItem` handler orchestrates creation/linking of Tasks and ActionableItems.
    * *Subscriber (Internal):* Background job checks `TicklerItem` trigger dates.
    * **Subscriber (Critical):** Listens to `TaskStatusUpdated`, `TaskCompleted` events (from Task BC). If the event's `TaskId` matches an `AssociatedTaskId` in an `ActionableItem`, update the `ActionableItem` status accordingly (e.g., issue `CompleteAction` command internally). Requires robust handling of event ordering and idempotency.
    * *Subscriber (External):* Listens to `TaskCreated`, `ProjectCreated` to potentially link them. Publishes GTD-specific events for Notifications, Analytics.

---

### D. Time Management Bounded Context

* **Subdomains:** Calendar & Scheduling, Time Orchestration, Time Tracking, Focus Management, Holiday & Availability, Routine Management

* **Aggregates (AR = Aggregate Root):**

    * **`Calendar` (AR):** Core calendar functionality and event management.
        * *Entities:* `CalendarEvent`, `RecurringEvent`, `EventSeries`.
        * *Value Objects:* `CalendarId`, `UserId`, `CalendarName`, `CalendarType`, `Visibility`, `TimeZone`, `Color`.

    * **`UserSchedule` (AR):** Manages a user's overall schedule and availability.
        * *Entities:* `AvailabilityRule`, `ScheduledEvent`, `BlockedTime`.
        * *Value Objects:* `ScheduleId` (`UserId`), `TimeZone`, `WorkingHours`, `BookingRule`, `BufferTime`.

    * **`HolidayManager` (AR):** Manages holidays, time-off, and availability exceptions.
        * *Entities:* `Holiday`, `TimeOffRequest`, `AvailabilityException`.
        * *Value Objects:* `HolidayId`, `UserId`, `HolidayType`, `RecurrencePattern`, `ApprovalStatus`, `TimeOffReason`.

    * **`TimeBlock` (AR):** A dedicated block of time for specific activities.
        * *Value Objects:* `BlockId`, `UserId`, `StartTime`, `EndTime`, `BlockType`, `AssociatedTaskId`, `FocusLevel`, `Status`, `IsRecurringInstance`, `Priority`, `FlexibilityLevel`.

    * **`Routine` (AR):** A predefined sequence of activities and habit management.
        * *Entities:* `RoutineStep`, `HabitTracker`, `RoutineTemplate`.
        * *Value Objects:* `RoutineId`, `UserId`, `Name`, `RecurrencePattern`, `AdherenceStatus`, `StreakCount`, `OptimizationScore`, `BehavioralTrigger`.

    * **`TimeEntry` (AR):** A record of time spent on activities.
        * *Value Objects:* `EntryId`, `UserId`, `TaskId`, `ProjectId`, `StartTime`, `EndTime`, `Duration`, `Notes`, `IsBillable`, `ContextId`, `ActivityType`, `EnergyLevel`.

    * **`FocusSession` (AR):** A period dedicated to focused work and productivity tracking.
        * *Entities:* `FocusBlock`, `InterruptionLog`.
        * *Value Objects:* `SessionId`, `UserId`, `StartTime`, `EndTime`, `IntendedFocusLevel`, `ActualFocusLevel`, `Status`, `PomodoroSettings`, `DistractionsCount`, `ProductivityScore`.

    * **`TimeEstimationAccuracyRecord` (AR):** Tracks estimation vs actual time for learning.
        * *Value Objects:* `AccuracyRecordId`, `UserId`, `TaskId`, `EstimatedTime`, `ActualTime`, `Variance`, `ConfidenceLevel`, `EstimationMethodUsed`, `LearningWeight`.

* **Commands:**
    * **Calendar & Scheduling:** `CreateCalendar`, `UpdateCalendar`, `DeleteCalendar`, `CreateEvent`, `UpdateEvent`, `DeleteEvent`, `CreateRecurringEvent`, `UpdateRecurringSeries`, `InviteToEvent`, `RespondToInvitation`.
    * **Time Orchestration:** `CreateTimeBlock`, `UpdateTimeBlock`, `CompleteTimeBlock`, `InterruptTimeBlock`, `DeleteTimeBlock`, `MoveTimeBlock`, `SplitTimeBlock`, `MergeTimeBlocks`.
    * **Time Tracking:** `StartTimeTracking`, `StopTimeTracking`, `PauseTimeTracking`, `LogTimeEntryManually`, `EditTimeEntry`, `DeleteTimeEntry`, `BulkImportTimeEntries`, `CategorizeTimeEntry`.
    * **Focus Management:** `StartFocusSession`, `PauseFocusSession`, `ResumeFocusSession`, `CompleteFocusSession`, `LogInterruption`, `SetFocusGoal`, `ConfigurePomodoroSettings`, `TrackProductivityMetrics`.
    * **Holiday & Availability:** `SetUserTimeZone`, `DefineAvailabilityRule`, `RemoveAvailabilityRule`, `AddHoliday`, `RemoveHoliday`, `RequestTimeOff`, `ApproveTimeOff`, `CreateAvailabilityException`, `ShareAvailability`.
    * **Routine Management:** `CreateRoutine`, `AddStepToRoutine`, `RemoveStepFromRoutine`, `ActivateRoutine`, `DeactivateRoutine`, `TrackRoutineAdherence`, `OptimizeRoutine`, `CreateHabitTracker`, `LogHabitCompletion`, `ResetRoutineStreak`.
    * **Learning & Optimization:** `RecordEstimationAccuracy`, `UpdateEstimationModel`, `GenerateScheduleSuggestions`.

* **Queries:**
    * **Calendar & Scheduling:** `GetCalendarById`, `ListUserCalendars`, `GetEventsForPeriod`, `GetEventById`, `SearchEvents`, `GetUpcomingEvents`, `GetConflictingEvents`.
    * **Time Orchestration:** `GetTimeBlocksForPeriod`, `GetTimeBlockById`, `GetAvailableTimeSlots`, `GetOptimalTimeBlocks`, `GetTimeBlockUtilization`.
    * **Time Tracking:** `GetTimeEntriesForTask`, `GetTimeEntriesForProject`, `GetTimeEntriesForPeriod`, `GetTimeEntriesByCategory`, `GetBillableHours`, `GetTimeTrackingReport`.
    * **Focus Management:** `GetFocusSessionHistory`, `GetFocusMetrics`, `GetProductivityTrends`, `GetInterruptionPatterns`, `GetOptimalFocusTimes`.
    * **Holiday & Availability:** `GetUserAvailability`, `GetScheduleForPeriod`, `GetHolidaysForPeriod`, `GetTimeOffRequests`, `GetWorkingHoursForUser`, `GetAvailabilityExceptions`.
    * **Routine Management:** `GetRoutineById`, `ListRoutinesByUser`, `GetRoutineAdherence`, `GetHabitStreaks`, `GetRoutineOptimizationSuggestions`, `GetBehavioralPatterns`.
    * **Analytics & Learning:** `GetTimeEstimationAccuracyForTask`, `GetAverageEstimationAccuracy`, `GetProductivityInsights`, `GetTimeAllocationReport`.

* **Events:**
    * **Calendar & Scheduling:** `CalendarCreated`, `EventCreated`, `EventUpdated`, `EventDeleted`, `EventInvitationSent`, `EventRSVPReceived`.
    * **Time Orchestration:** `TimeBlockCreated`, `TimeBlockUpdated`, `TimeBlockCompleted`, `TimeBlockInterrupted`, `TimeBlockOptimized`.
    * **Time Tracking:** `TimeTrackingStarted`, `TimeTrackingStopped`, `TimeEntryLogged`, `TimeEntryEdited`, `BillableTimeRecorded`.
    * **Focus Management:** `FocusSessionStarted`, `FocusSessionCompleted`, `InterruptionLogged`, `ProductivityMilestoneReached`, `FocusGoalAchieved`.
    * **Holiday & Availability:** `AvailabilityRuleDefined`, `HolidayAdded`, `TimeOffRequested`, `TimeOffApproved`, `AvailabilityExceptionCreated`.
    * **Routine Management:** `RoutineCreated`, `RoutineActivated`, `HabitCompleted`, `StreakAchieved`, `RoutineOptimized`, `BehavioralPatternDetected`.
    * **Learning & Optimization:** `EstimationAccuracyRecorded`, `ScheduleOptimizationTriggered`, `ProductivityInsightGenerated`.

* **Event Subscribers/Policies:**
    * **Calendar Integration Policy:** Prevents double-booking and manages calendar conflicts across multiple calendar sources.
    * **Availability Enforcement Policy:** Prevents booking events outside defined availability rules or during holidays/time-off.
    * **Focus Session Policy:** Manages Pomodoro timer logic, break enforcement, and distraction handling within focus sessions.
    * **Routine Optimization Policy:** Analyzes routine adherence patterns and suggests optimizations based on success rates.
    * **Holiday Management Policy:** Automatically blocks availability during holidays and approved time-off periods.
    
    * **Smart Scheduling Subscriber:** Listens to Task/Calendar events and suggests optimal time block creation using AI/ML insights.
    * **Routine Engine Subscriber:** Triggers time block creation for routine steps and habit tracking.
    * **Estimation Learning Subscriber:** Listens to `TaskCompleted` events, calculates actual vs estimated time, and issues `RecordEstimationAccuracy` commands.
    * **Behavioral Pattern Subscriber:** Analyzes routine adherence, focus session data, and time tracking to detect behavioral patterns and optimization opportunities.
    * **Productivity Analytics Subscriber:** Aggregates data from all subdomains to generate productivity insights and recommendations.
    * **External Integration Subscriber:** Syncs calendar events, availability, and time tracking data with external calendars, project management tools, and billing systems.
    * **Notification Subscriber:** Publishes time-related events for reminders, deadline alerts, routine prompts, and productivity notifications.

---

### E. Area Management Bounded Context

* **Subdomains:** Organizational Structure, Resource Allocation
* **Aggregates (AR = Aggregate Root):**
    * **`Area` (AR):** A logical grouping for work and resources.
        * *Entities:* `AreaVersion`.
        * *Value Objects:* `AreaId`, `Name`, `Description`, `ParentAreaId`, `Status`, `CustomProperty`, `AccessControlList`.
    * **`Resource` (AR):** Represents a person, equipment, or material.
        * *Value Objects:* `ResourceId`, `Name`, `ResourceType`, `Capacity`, `CostRate`, `AvailabilityCalendarRef` (logically links to Time BC UserSchedule), `Skills`.
    * **`ResourceAllocation` (AR):** Assigns a resource to an Area, Project, or Task.
        * *Value Objects:* `AllocationId`, `ResourceId`, `TargetId`, `TargetType`, `StartDate`, `EndDate`, `AllocatedAmount`, `Status`.
* **Commands:**
    * `CreateArea`, `UpdateAreaDetails`, `SetAreaParent`, `ArchiveArea`, `DeleteArea`, `DefineResource`, `UpdateResourceDetails`, `SetResourceCapacity`, `AllocateResource`, `UpdateAllocation`, `DeallocateResource`.
* **Queries:**
    * `GetAreaById`, `GetSubAreas`, `ListRootAreas`, `GetResourcesByArea`, `GetResourceById`, `GetResourceAvailability` (Needs to query Time BC), `GetAllocationsForResource`, `GetAllocationsForArea`, `GetAllocationsForProject`, `SearchResourcesBySkill`.
* **Events:**
    * `AreaCreated`, `AreaUpdated`, `AreaArchived`, `ResourceDefined`, `ResourceUpdated`, `ResourceAllocated`, `ResourceDeallocated`.
* **Event Subscribers/Policies:**
    * *Policy:* Enforce unique Area names within the same parent.
    * **Policy:* Prevent allocation exceeding Resource capacity. *Interaction:* This check within the `AllocateResource` command handler likely requires a synchronous query to the Time BC (`GetUserAvailability`) or querying a local read model populated by Time BC availability events. Choose strategy based on consistency needs.
    * *Policy:* Handle deletion/archiving of Areas containing active Projects/Tasks.
    * *Subscriber (Internal):* On `AreaCreated`, potentially set default permissions (interacts with IAM).
    * *Subscriber (External):* Listens for `ProjectCreated`, `TaskCreated` to ensure `AreaId` is valid. Publishes events for Notifications, Analytics.

---

### F. Collaboration Bounded Context

* **Subdomains:** Collaboration (Comments/Mentions)
* **Aggregates (AR = Aggregate Root):**
    * **`DiscussionThread` (AR):** Conversation around a specific entity (Task, Project, WBSNode, etc.).
        * *Entities:* `Comment`.
        * *Value Objects:* `ThreadId` (e.g., `TaskId`, `ProjectId`), `EntityType`, `CommentId`, `AuthorUserId`, `Timestamp`, `Content`, `ParentCommentId`, `EditHistory`, `MentionedUserIds`.
* **Commands:**
    * `AddComment`, `EditComment`, `DeleteComment`, `ReplyToComment`.
* **Queries:**
    * `GetCommentsForThread`, `GetCommentById`.
* **Events:**
    * `CommentAdded`, `CommentEdited`, `CommentDeleted`, `UserMentionedInComment`.
* **Event Subscribers/Policies:**
    * *Policy:* Check user permissions via IAM.
    * *Policy:* Limit comment length/attachments.
    * *Subscriber (External):* Publishes events (`CommentAdded`, `UserMentionedInComment`) primarily to Notifications BC and potentially Analytics.

---

### G. Notifications Bounded Context

* **Subdomains:** Notification Delivery & Preferences
* **Aggregates (AR = Aggregate Root):**
    * **`UserNotificationPreferences` (AR):** User-specific settings.
        * *Value Objects:* `UserId`, `ChannelPreferences`, `FrequencyPreference`, `DigestSchedule`, `DoNotDisturbSettings`, `NotificationFilters`.
    * **`NotificationRule` (AR):** System or user-defined rule.
        * *Value Objects:* `RuleId`, `Name`, `TriggerEvent`, `Conditions`, `TargetUserLogic`, `NotificationTemplate`, `Channels`, `PriorityLevel`.
    * **`Notification` (AR):** A single notification instance.
        * *Value Objects:* `NotificationId`, `RecipientUserId`, `Content`, `Channel`, `TimestampGenerated`, `TimestampSent`, `Status`, `RetryCount`, `TriggeringRuleId`, `TriggeringEventId`, `PriorityLevel`.
* **Commands:**
    * `SetNotificationPreferences`, `DefineNotificationRule`, `UpdateNotificationRule`, `DeleteNotificationRule`, `SendNotification` (Internal), `MarkNotificationRead`, `MarkNotificationFailed`, `RetryFailedNotification`.
* **Queries:**
    * `GetNotificationPreferencesForUser`, `ListNotificationRules`, `GetNotificationRuleById`, `GetUserNotifications`, `GetNotificationStatus`.
* **Events:**
    * `NotificationPreferencesUpdated`, `NotificationRuleDefined`, `NotificationSent`, `NotificationDelivered`, `NotificationFailed`, `NotificationRead`.
* **Event Subscribers/Policies:**
    * *Policy:* Respect user preferences unless priority overrides.
    * *Policy:* Retry failed notifications.
    * **Subscriber (Critical):** Listens to relevant events from *all other BCs* using ABP's event bus. Matches events against `NotificationRule`s. If matched, generates `Notification` instances and issues `SendNotification` command to queue/dispatch them.
    * *Subscriber (Internal):* Background job processes digests.
    * *Subscriber (External):* Publishes delivery status events.

---

### H. Analytics Platform Bounded Context

* **Subdomains:** Business Intelligence/Reporting Configuration & Data Aggregation
* **Aggregates (AR = Aggregate Root):**
    * **`DashboardConfiguration` (AR):** User's dashboard setup.
        * *Entities:* `WidgetConfiguration`.
        * *Value Objects:* `DashboardId`, `UserId`, `Name`, `Layout`.
    * **`ReportDefinition` (AR):** Definition for a specific report.
        * *Value Objects:* `ReportId`, `Name`, `Description`, `TargetDataSources` (Refers to specific events/read models), `Metrics`, `Dimensions`, `Filters`, `VisualizationType`, `Schedule`, `RecipientList`.
    * **(Conceptual) `AnalyticsReadModel`:** Not a traditional AR, but represents the aggregated data store (e.g., tables/views in a data warehouse) populated by events.
* **Commands:**
    * `ConfigureDashboard`, `AddWidgetToDashboard`, `RemoveWidgetFromDashboard`, `DefineReport`, `UpdateReportDefinition`, `ScheduleReportGeneration`, `GenerateReportOnDemand`, `ProcessIncomingDomainEvent` (Internal command/handler to update read models).
* **Queries:**
    * `GetDashboardConfiguration`, `GetReportDefinitionById`, `ListReportDefinitions`, `GetGeneratedReport`, `QueryAnalyticsData` (Directly queries the read models/data warehouse).
* **Events:**
    * `DashboardConfigured`, `ReportDefinitionCreated`, `ReportGenerated`, `AnalyticsDataUpdated`.
* **Event Subscribers/Policies:**
    * *Policy:* Validate report definitions.
    * **Subscriber (Critical):** Listens to events from all relevant BCs via ABP event bus. Updates internal, optimized read models or triggers jobs in a dedicated data store (Data Warehouse/Lakehouse). This is the core data aggregation mechanism.
    * *Subscriber (Internal):** Background job runs scheduled report generation.
    * *Subscriber (External):* Publishes `ReportGenerated` event (e.g., for Notifications).

---

### I. Learning System Bounded Context

* **Subdomains:** AI/ML Augmentation, Prediction, Recommendation
* **Aggregates (AR = Aggregate Root):**
    * **`LearningModel` (AR):** Represents a trained ML model.
        * *Entities:* `ModelVersion`.
        * *Value Objects:* `ModelId`, `LearningArea`, `AlgorithmType`, `Status`, `PerformanceMetrics`, `TrainingDataSourceRef`, `LastTrainedTimestamp`, `ExplainabilityInfo`.
    * **`UserProductivityProfile` (AR):** Stores learned patterns for a user.
        * *Value Objects:* `UserId`, `LearnedWorkPatterns`, `EstimationBias`, `CognitiveLoadTolerance`, `FeedbackHistory`, `ABTestGroup`.
* **Commands:**
    * `TrainModel`, `DeployModelVersion`, `RetireModel`, `RecordUserFeedback`, `UpdateUserProfileBasedOnActivity`, `RunABTestVariant`.
* **Queries:**
    * `GetModelById`, `GetActiveModelForArea`, `GetUserProfile`, **`GetPrediction`** (e.g., `PredictTaskDuration`), **`GetRecommendation`** (e.g., `RecommendTaskBreakdown`). *Interaction:* Interface (e.g., REST API via ABP or gRPC) needs clear definition for requests (data input) and responses (prediction + confidence/explanation). Determine if sync or async.
    * `ExplainPrediction`.
* **Events:**
    * `ModelTrainingStarted`, `ModelTrainingCompleted`, `ModelDeployed`, `ModelPerformanceDegraded`, `UserProfileUpdated`, `RecommendationGenerated`, `PredictionMade`.
* **Event Subscribers/Policies:**
    * *Policy:* Monitor model performance.
    * *Policy:* Ensure user privacy/anonymization.
    * *Policy:* Allow user overrides and learn from them.
    * **Subscriber (Internal):** Listens to events from relevant BCs (Task, Time, User Feedback) via ABP event bus to collect training data and update `UserProductivityProfile`.
    * *Subscriber (External)/Service Interface:* Provides prediction/recommendation results via its defined query interfaces (sync or async) to other BCs (e.g., Time Management for Smart Scheduling, Task Management for estimation/breakdown assistance). Publishes events like `RecommendationGenerated` if needed.

---


### K. Reference Management Bounded Context

* **Subdomains:** Document/Knowledge Management & Storage
* **Aggregates (AR = Aggregate Root):**
    * **`ReferenceItem` (AR):** Stored information (document, link, note).
        * *Entities:* `ReferenceVersion`.
        * *Value Objects:* `ReferenceId`, `OwnerUserId`, `Name`, `Type`, `ContentPointer`, `MimeType`, `Size`, `Tags`, `Metadata`, `AreaId` (optional link), `ProjectId` (optional link), `TaskId` (optional link).
* **Commands:**
    * `UploadReferenceItem`, `CreateWebLinkReference`, `CreateNoteReference`, `UpdateReferenceMetadata`, `TagReferenceItem`, `UntagReferenceItem`, `DeleteReferenceItem`, `ArchiveReferenceItem`, `AddReferenceVersion`.
* **Queries:**
    * `GetReferenceItemById`, `GetReferenceVersions`, `SearchReferences` (requires indexing service), `ListReferencesByArea`, `ListReferencesByProject`, `ListReferencesByTask`.
* **Events:**
    * `ReferenceItemCreated`, `ReferenceItemUpdated`, `ReferenceItemDeleted`, `ReferenceItemArchived`, `ReferenceVersionAdded`.
* **Event Subscribers/Policies:**
    * *Policy:* Enforce storage quotas.
    * *Policy:* Check permissions (via IAM) for access/modification/deletion.
    * *Subscriber (Internal):* Background job for content indexing.
    * *Subscriber (External):* Publishes events if needed (e.g., Analytics). Other BCs (Task, Project) call this BC (likely via service interface) to link/unlink references by storing/removing the `ReferenceId`.

---