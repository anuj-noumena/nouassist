# Acceptance Criteria for Domain Aggregates

## Task Management Bounded Context

### Task Aggregate Root

#### Core Task Management
**AC-TASK-001: Create Task**
```gherkin
Given I am an authenticated user
When I create a task with valid title, description, and due date
Then the task should be created with status "Not Started"
And a TaskCreated event should be published
And the task should be assigned a unique TaskId
And the task should have the specified title, description, and due date
```

**AC-TASK-002: Task Title Validation**
```gherkin
Given I am creating a task
When I provide a title that is empty or exceeds 200 characters
Then the system should reject the task creation
And return a validation error message
```

**AC-TASK-003: Complete Task with Mandatory Subtasks**
```gherkin
Given I have a task with mandatory subtasks
And some mandatory subtasks are not completed
When I attempt to complete the task
Then the system should prevent task completion
And return an error "Cannot complete task with incomplete mandatory subtasks"
```

**AC-TASK-004: Complete Task Successfully**
```gherkin
Given I have a task with all mandatory subtasks completed
When I mark the task as completed
Then the task status should change to "Completed"
And the completion timestamp should be recorded
And a TaskCompleted event should be published
And any linked GTD actions should be notified
```

**AC-TASK-005: Task Assignment**
```gherkin
Given I have permission to assign tasks
When I assign a task to a valid user
Then the task should be assigned to that user
And only one active assignment should exist
And a TaskAssigned event should be published
And the assignee should be notified
```

**AC-TASK-006: Due Date Constraints**
```gherkin
Given I am creating or updating a task
When I set a due date in the past
Then the system should reject the due date
Unless the task is being marked as overdue
```

#### Subtask Management
**AC-TASK-007: Add Subtasks**
```gherkin
Given I have a task
When I add subtasks to the task
Then the subtasks should be linked to the parent task
And each subtask should have its own status
And subtasks can be marked as mandatory or optional
```

**AC-TASK-008: Subtask Completion Impact**
```gherkin
Given I have a task with multiple subtasks
When I complete all mandatory subtasks
Then the parent task should be eligible for completion
When I complete all subtasks (mandatory and optional)
Then the task progress should reflect 100% completion
```

#### Task Dependencies
**AC-TASK-009: Create Task Dependency**
```gherkin
Given I have two valid tasks
When I create a dependency between them
Then the dependency should be established with the specified type (FS, SS, FF, SF)
And the system should prevent circular dependencies
And dependent tasks should respect the dependency rules
```

**AC-TASK-010: Circular Dependency Prevention**
```gherkin
Given I have tasks A, B, and C where A depends on B and B depends on C
When I attempt to create a dependency from C to A
Then the system should reject the dependency creation
And return an error "Circular dependency detected"
```

**AC-TASK-011: Dependency Impact on Status**
```gherkin
Given I have a task B that depends on task A (Finish-to-Start)
When task A is not completed
Then task B should not be startable
When task A is completed
Then task B should become eligible to start
```

#### Project Integration
**AC-TASK-012: Link Task to Project**
```gherkin
Given I have a valid project and WBS node
When I create a task linked to that WBS node
Then the task should be associated with the project
And the task should contribute to WBS node progress calculation
And project progress should be updated accordingly
```

### TaskTemplate Aggregate Root

**AC-TEMPLATE-001: Create Task Template**
```gherkin
Given I am an authenticated user
When I create a task template with predefined fields and workflow
Then the template should be saved with a unique identifier
And the template should be available for creating tasks
And all predefined fields should be preserved
```

**AC-TEMPLATE-002: Create Task from Template**
```gherkin
Given I have a valid task template
When I create a task from the template
Then a new task should be created with template defaults
And I should be able to customize specific fields
And the task should not be linked back to the template after creation
```

**AC-TEMPLATE-003: Template Versioning**
```gherkin
Given I have an existing task template
When I modify the template
Then a new version should be created
And existing tasks created from previous versions should remain unchanged
And I should be able to access template version history
```

### TaskDependency Aggregate Root

**AC-DEPENDENCY-001: Dependency Types**
```gherkin
Given I have two tasks
When I create dependencies of different types:
- Finish-to-Start (FS): Task A must finish before Task B starts
- Start-to-Start (SS): Task B cannot start until Task A starts
- Finish-to-Finish (FF): Task B cannot finish until Task A finishes
- Start-to-Finish (SF): Task B cannot finish until Task A starts
Then each dependency type should enforce its specific timing rules
```

**AC-DEPENDENCY-002: Lag and Lead Time**
```gherkin
Given I have a task dependency
When I specify lag time (delay) or lead time (advance)
Then the dependency timing should account for the lag/lead
And task scheduling should respect these timing adjustments
```

**AC-DEPENDENCY-003: Dependency Status Management**
```gherkin
Given I have task dependencies
When predecessor tasks change status
Then dependent task constraints should be re-evaluated
And dependency status should update accordingly (Active, Conflicted, Resolved)
```

### RecurringTaskDefinition Aggregate Root

**AC-RECURRING-001: Create Recurring Definition**
```gherkin
Given I am an authenticated user
When I create a recurring task definition with a valid pattern
Then the definition should be saved with the recurrence rules
And the system should be able to generate future instances
And the pattern should support daily, weekly, monthly, and yearly recurrence
```

**AC-RECURRING-002: Generate Task Instances**
```gherkin
Given I have a recurring task definition
When the scheduled generation time arrives
Then a new task instance should be created automatically
And the task should inherit properties from the definition
And a RecurringTaskInstanceGenerated event should be published
```

**AC-RECURRING-003: Handle Generation Failures**
```gherkin
Given a recurring task generation fails
When the system attempts to generate an instance
Then the failure should be logged
And retry attempts should be made according to the retry policy
And manual intervention should be possible for persistent failures
```

**AC-RECURRING-004: Modify Recurring Definition**
```gherkin
Given I have a recurring task definition
When I modify the definition
Then future instances should use the new definition
And existing instances should remain unchanged
And I should be able to choose whether to apply changes to future instances only
```

## Project Management Bounded Context

### Project Aggregate Root

**AC-PROJECT-001: Create Project**
```gherkin
Given I am an authenticated user with project creation permissions
When I create a project with name, description, goals, and timeline
Then the project should be created with status "Planning"
And I should be automatically assigned as the project owner
And a ProjectCreated event should be published
And the project should have a unique identifier
```

**AC-PROJECT-002: Project Name Uniqueness**
```gherkin
Given I am in a specific area (if area management is used)
When I create a project with a name that already exists in the same area
Then the system should reject the project creation
And return a validation error about duplicate project name
```

**AC-PROJECT-003: Project Timeline Validation**
```gherkin
Given I am creating or updating a project
When I set an end date that is before the start date
Then the system should reject the timeline
And return a validation error message
```

**AC-PROJECT-004: Team Member Management**
```gherkin
Given I have a project
When I add team members with specific roles
Then the members should be associated with the project
And they should have the permissions according to their roles
And at least one member must have the "Owner" role
```

**AC-PROJECT-005: Project Status Transitions**
```gherkin
Given I have a project in various states
When I transition the project status:
- Planning → Active: All prerequisites should be met
- Active → On Hold: Reason should be provided
- Active → Completed: All critical WBS nodes should be completed
- Any status → Cancelled: Proper cleanup should occur
Then the transition should be allowed only if business rules are satisfied
And a ProjectStatusUpdated event should be published
```

**AC-PROJECT-006: Project Deletion Constraints**
```gherkin
Given I have a project with active tasks
When I attempt to delete the project
Then the system should prevent deletion
And return an error indicating active tasks must be resolved first
```

### ProjectWBSNode Entity

**AC-WBS-001: Create WBS Hierarchy**
```gherkin
Given I have a project
When I create WBS nodes in a hierarchical structure
Then each node should have a unique identifier within the project
And child nodes should be properly linked to parent nodes
And the hierarchy should not contain cycles
```

**AC-WBS-002: WBS Progress Calculation**
```gherkin
Given I have a WBS node with linked tasks
When tasks are completed or status changes
Then the WBS node progress should be automatically recalculated
And parent node progress should roll up from child nodes
And the calculation should be based on task weights or completion counts
```

**AC-WBS-003: Leaf Node Progress from Tasks**
```gherkin
Given I have a leaf WBS node (no children) with linked tasks
When task statuses change
Then the node progress should be calculated from:
- Percentage of completed tasks, OR
- Weighted sum based on task complexity/estimates
And the progress should be between 0% and 100%
```

**AC-WBS-004: Parent Node Progress from Children**
```gherkin
Given I have a parent WBS node with child nodes
When child node progress changes
Then the parent progress should be calculated as:
- Average of child node progress percentages, OR
- Weighted average based on child node importance
And the calculation should cascade up the hierarchy
```

**AC-WBS-005: Link Tasks to WBS Nodes**
```gherkin
Given I have a WBS node and valid tasks
When I link tasks to the WBS node
Then the tasks should be associated with the node
And task completion should contribute to node progress
And tasks can be linked to only one WBS node at a time
```

### Risk Aggregate Root

**AC-RISK-001: Identify Project Risk**
```gherkin
Given I have a project
When I identify a new risk with description and category
Then the risk should be created with status "Identified"
And the risk should be assigned a probability and impact assessment
And a RiskIdentified event should be published
```

**AC-RISK-002: Risk Assessment**
```gherkin
Given I have an identified risk
When I assess the risk with probability (Low/Medium/High) and impact (Low/Medium/High)
Then the risk assessment should be recorded
And the risk status should change to "Assessed"
And the risk priority should be calculated (Probability × Impact)
```

**AC-RISK-003: Risk Mitigation Planning**
```gherkin
Given I have an assessed risk
When I create mitigation plan steps
Then each step should have a responsible owner
And steps should have target completion dates
And the risk status should change to "Mitigated" when all steps are completed
```

**AC-RISK-004: Risk Realization**
```gherkin
Given I have a risk that becomes an actual issue
When I mark the risk as realized
Then the risk status should change to "Realized"
And an associated issue should be automatically created
And the risk owner should be notified
```

### Issue Aggregate Root

**AC-ISSUE-001: Create Project Issue**
```gherkin
Given I have a project
When I create an issue with description, type, and severity
Then the issue should be created with status "Open"
And the issue should be assigned to a responsible person
And an issue tracking number should be generated
```

**AC-ISSUE-002: Issue Severity and Priority**
```gherkin
Given I create an issue
When I assign severity (Critical/High/Medium/Low) and priority (Urgent/High/Medium/Low)
Then the issue should be categorized accordingly
And critical/urgent issues should trigger immediate notifications
And issue assignment should consider severity and priority
```

**AC-ISSUE-003: Issue Resolution Process**
```gherkin
Given I have an open issue
When I work on resolving the issue:
- Status changes: Open → In Progress → Resolved → Closed
- Resolution steps are documented
- Resolution time is tracked
Then each status change should be recorded with timestamp
And stakeholders should be notified of status changes
```

**AC-ISSUE-004: Issue Impact on Project**
```gherkin
Given I have issues in a project
When issues are critical and unresolved
Then the project completion should be blocked
And project status should reflect the blocking issues
And project timeline may need adjustment
```

### AgileBoard Aggregate Root

**AC-BOARD-001: Configure Agile Board**
```gherkin
Given I have a project
When I configure an agile board with type (Scrum/Kanban) and columns
Then the board should be created with the specified workflow
And work items should be trackable across columns
And WIP limits should be enforceable if specified
```

**AC-BOARD-002: Move Work Items**
```gherkin
Given I have an agile board with work items
When I move a work item from one column to another
Then the item status should update accordingly
And WIP limits should be enforced
And the move should be logged for velocity tracking
```

**AC-BOARD-003: WIP Limit Enforcement**
```gherkin
Given I have a column with WIP limit set
When I attempt to move more items into the column than the limit allows
Then the system should prevent the move
And display a warning about the WIP limit violation
```

**AC-BOARD-004: Board Integration with WBS**
```gherkin
Given I have work items on the agile board
When work items represent WBS components
Then completing work items should update WBS node progress
And board progress should reflect in project progress
```

### Sprint Aggregate Root

**AC-SPRINT-001: Plan Sprint**
```gherkin
Given I have a project with agile board
When I plan a sprint with goal, start date, end date, and capacity
Then the sprint should be created with status "Planning"
And I should be able to add work items to the sprint backlog
And the sprint duration should be validated (typically 1-4 weeks)
```

**AC-SPRINT-002: Sprint Execution**
```gherkin
Given I have a planned sprint
When I start the sprint
Then the sprint status should change to "Active"
And work items should be moved to the sprint board
And daily progress should be trackable
```

**AC-SPRINT-003: Sprint Completion and Velocity**
```gherkin
Given I have an active sprint
When the sprint end date is reached or I manually close the sprint
Then the sprint status should change to "Completed"
And sprint velocity should be calculated based on completed work
And incomplete items should be moved back to product backlog
And a sprint retrospective should be schedulable
```

**AC-SPRINT-004: Sprint Capacity Management**
```gherkin
Given I have a sprint with defined capacity
When I add work items to the sprint
Then the total estimated effort should not exceed sprint capacity
And the system should warn when approaching capacity limits
And capacity utilization should be trackable
```

## Work Management (GTD) Bounded Context

### InboxItem Aggregate Root

**AC-INBOX-001: Capture Item**
```gherkin
Given I am an authenticated user
When I capture an item in my inbox with content and source
Then the item should be stored with capture timestamp
And the item should have status "Unprocessed"
And an InboxItemCaptured event should be published
```

**AC-INBOX-002: Process Inbox Item - Action**
```gherkin
Given I have an unprocessed inbox item
When I process it as an actionable item
Then an ActionableItem should be created
And the inbox item status should change to "Processed"
And the inbox item should reference the created action
```

**AC-INBOX-003: Process Inbox Item - Reference**
```gherkin
Given I have an unprocessed inbox item
When I process it as reference material
Then the item should be moved to reference storage
And the inbox item should be marked as "Processed"
And no actionable item should be created
```

**AC-INBOX-004: Process Inbox Item - Someday/Maybe**
```gherkin
Given I have an unprocessed inbox item
When I process it as a someday/maybe item
Then a SomedayMaybeItem should be created
And the inbox item should be marked as "Processed"
And the item should be scheduled for periodic review
```

**AC-INBOX-005: Two-Minute Rule**
```gherkin
Given I have an inbox item that can be done in 2 minutes or less
When I process the item
Then the system should suggest doing it immediately
And if I complete it immediately, no action item should be created
And the item should be marked as "Processed"
```

### ActionableItem Aggregate Root

**AC-ACTION-001: Create Action Item**
```gherkin
Given I am processing work or creating actions
When I create an actionable item with description and context
Then the action should be created with status "Deferred"
And the action must have an associated context
And the description should be written as an actionable statement
```

**AC-ACTION-002: Mark as Next Action**
```gherkin
Given I have actionable items in various contexts
When I mark an item as "Next Action"
Then the item status should change to "NextAction"
And the item should appear in my next actions list
And I should be able to have multiple next actions per context
```

**AC-ACTION-003: Link Action to Task**
```gherkin
Given I have an action item and want detailed project management
When I link the action to a task in Task Management BC
Then both systems should stay synchronized
And status changes in either system should update the other
And the linking strategy should determine synchronization behavior
```

**AC-ACTION-004: Delegate Action**
```gherkin
Given I have an action item that I need to delegate
When I delegate the action to someone else
Then the action status should change to "Delegated"
And a WaitingForItem should be created
And a follow-up date should be established
```

**AC-ACTION-005: Complete Action**
```gherkin
Given I have an action item
When I mark it as completed
Then the action status should change to "Done"
And completion timestamp should be recorded
And if linked to a task, the task should be notified
```

**AC-ACTION-006: Energy and Time Estimates**
```gherkin
Given I am creating an action item
When I specify energy level required and time estimate
Then these should be stored with the action
And I should be able to filter next actions by available energy/time
And the system should learn from actual completion data
```

### Context Aggregate Root

**AC-CONTEXT-001: Define Context**
```gherkin
Given I am setting up my GTD system
When I define contexts like @Computer, @Phone, @Errands
Then each context should be unique per user
And contexts should support hierarchical organization
And contexts can be marked as active or inactive
```

**AC-CONTEXT-002: Context-Based Action Filtering**
```gherkin
Given I have actions in various contexts
When I want to see actions for a specific context
Then I should see only actions associated with that context
And actions should be sorted by priority or other criteria
And inactive contexts should not show in active lists
```

**AC-CONTEXT-003: Context Availability**
```gherkin
Given I have contexts defined
When I mark a context as unavailable (e.g., @Office when working from home)
Then actions in that context should not appear in current action lists
And I should be able to quickly toggle context availability
```

### WaitingForItem Aggregate Root

**AC-WAITING-001: Create Waiting Item**
```gherkin
Given I have delegated an action or am waiting for something
When I create a waiting for item with description and follow-up date
Then the item should be tracked with status "Active"
And I should be able to specify who I'm waiting for
And a follow-up reminder should be scheduled
```

**AC-WAITING-002: Follow-up Reminders**
```gherkin
Given I have waiting for items with follow-up dates
When the follow-up date arrives
Then I should receive a reminder to check status
And I should be able to extend the follow-up date or mark as received
```

**AC-WAITING-003: Mark as Received**
```gherkin
Given I have a waiting for item
When I receive what I was waiting for
Then the item status should change to "Received"
And if it was linked to an action, that action should be reactivated
```

### SomedayMaybeItem Aggregate Root

**AC-SOMEDAY-001: Create Someday/Maybe Item**
```gherkin
Given I have ideas or potential future actions
When I create a someday/maybe item
Then it should be stored with category and review frequency
And it should not appear in current action lists
And it should be included in periodic reviews
```

**AC-SOMEDAY-002: Periodic Review**
```gherkin
Given I have someday/maybe items with different review frequencies
When review time arrives (weekly, monthly, quarterly)
Then items should be presented for review
And I should be able to activate them as actions or keep in someday/maybe
```

**AC-SOMEDAY-003: Activate Someday/Maybe**
```gherkin
Given I have a someday/maybe item that I want to act on
When I activate the item
Then it should be converted to an actionable item
And moved to the appropriate context
And removed from the someday/maybe list
```

### Review Aggregate Root

**AC-REVIEW-001: Schedule Reviews**
```gherkin
Given I am using the GTD system
When I schedule different types of reviews (Daily, Weekly, Monthly)
Then each review should have its appropriate checklist
And reviews should be automatically scheduled based on frequency
And I should receive reminders for due reviews
```

**AC-REVIEW-002: Conduct Weekly Review**
```gherkin
Given I have a weekly review scheduled
When I conduct the weekly review
Then I should review:
- All project lists and next actions
- Waiting for list
- Someday/maybe list
- Calendar and upcoming commitments
And each checklist item should be marked as completed
```

**AC-REVIEW-003: Review Outcomes**
```gherkin
Given I complete a review
When I finish all review checklist items
Then the review should be marked as completed
And any decisions or actions from the review should be captured
And the next review should be automatically scheduled
```

### Horizon Aggregate Root

**AC-HORIZON-001: Define Horizons of Focus**
```gherkin
Given I am setting up strategic levels
When I define horizons from Purpose (50,000ft) down to Actions (Runway)
Then each horizon should be clearly defined:
- Purpose (50,000ft): Life purpose and principles
- Vision (40,000ft): 3-5 year vision
- Goals (30,000ft): 1-2 year goals
- Areas of Responsibility (20,000ft): Ongoing areas to maintain
- Projects (10,000ft): Outcomes to achieve in 1 year
- Actions (Runway): Next physical actions
```

**AC-HORIZON-002: Link Projects and Actions to Horizons**
```gherkin
Given I have horizons defined
When I link projects and actions to appropriate horizon levels
Then I should be able to see alignment between levels
And projects should support goals and areas of responsibility
And actions should support project outcomes
```

**AC-HORIZON-003: Horizon Reviews**
```gherkin
Given I have horizons defined
When I conduct horizon reviews at appropriate frequencies:
- Daily: Actions review
- Weekly: Projects review
- Monthly: Areas of responsibility review
- Quarterly: Goals review
- Annual: Vision and purpose review
Then each level should be evaluated for alignment and progress
```

### TicklerItem Aggregate Root

**AC-TICKLER-001: Create Tickler Items**
```gherkin
Given I want to be reminded of something on a specific date
When I create a tickler item with target date and content
Then the item should be scheduled to surface on the specified date
And the item should reference what needs attention
```

**AC-TICKLER-002: Trigger Tickler Items**
```gherkin
Given I have tickler items scheduled for today
When the system processes daily ticklers
Then items should be moved to my inbox or appropriate list
And I should be notified of the triggered items
```

**AC-TICKLER-003: Process Triggered Items**
```gherkin
Given a tickler item has been triggered
When I process the triggered item
Then I should be able to:
- Convert it to an action if needed
- Reschedule it for later
- Mark it as completed if no action needed
- Convert it to a project if it's complex
```

## Time Management Bounded Context

### Calendar Aggregate Root

**AC-CALENDAR-001: Create Calendar**
```gherkin
Given I am an authenticated user
When I create a calendar with name, type, and timezone
Then the calendar should be created with specified properties
And I should be able to create events in this calendar
And the calendar should support different visibility levels
```

**AC-CALENDAR-002: Create Calendar Event**
```gherkin
Given I have a calendar
When I create an event with title, start time, end time, and attendees
Then the event should be created with a unique identifier
And the event should not overlap with other events (if conflict checking enabled)
And attendees should be notified if specified
```

**AC-CALENDAR-003: Recurring Events**
```gherkin
Given I want to create a recurring event
When I specify a recurrence pattern (daily, weekly, monthly, yearly)
Then future instances should be automatically generated
And I should be able to modify individual instances
And changes should not affect other instances unless specified
```

**AC-CALENDAR-004: Event Rescheduling**
```gherkin
Given I have a scheduled event
When I reschedule the event to a different time
Then the event should be moved to the new time slot
And attendees should be notified of the change
And conflicts should be checked for the new time
```

**AC-CALENDAR-005: External Calendar Integration**
```gherkin
Given I have external calendars (Google, Outlook)
When I connect them to the system
Then external events should be synchronized
And I should see all events in a unified view
And changes in either system should synchronize
```

### UserSchedule Aggregate Root

**AC-SCHEDULE-001: Define Working Hours**
```gherkin
Given I am setting up my schedule
When I define my working hours for each day of the week
Then these hours should be used for availability calculations
And meetings should preferably be scheduled within working hours
And overtime scheduling should require confirmation
```

**AC-SCHEDULE-002: Set Availability Rules**
```gherkin
Given I want to control my availability
When I create availability rules (available, busy, tentative)
Then these rules should override default working hours
And the rules should support date ranges
And more specific rules should take precedence
```

**AC-SCHEDULE-003: Calculate Availability**
```gherkin
Given I have working hours and availability rules defined
When someone queries my availability for a time period
Then the system should return accurate availability information
And consider existing calendar events
And respect blocked time periods
```

**AC-SCHEDULE-004: Find Available Time Slots**
```gherkin
Given I need to schedule a meeting of specific duration
When I search for available time slots
Then the system should return suitable time slots
And consider all attendees' availability if specified
And prefer optimal meeting times based on preferences
```

### TimeBlock Aggregate Root

**AC-TIMEBLOCK-001: Create Time Block**
```gherkin
Given I want to allocate time for specific work
When I create a time block with type, duration, and associated task
Then the block should be created in my schedule
And the block should be linked to the task if specified
And conflicts should be checked with existing blocks
```

**AC-TIMEBLOCK-002: Time Block Types**
```gherkin
Given I am creating time blocks
When I specify different block types:
- Focus: Deep work requiring concentration
- Meeting: Scheduled meetings with others
- Break: Rest periods
- Travel: Time for travel between locations
- Buffer: Padding time between activities
Then each type should have appropriate properties and constraints
```

**AC-TIMEBLOCK-003: Move Time Block**
```gherkin
Given I have a time block that needs rescheduling
When I move the block to a different time
Then the block should be moved if no conflicts exist
And the flexibility level should determine if move is allowed
And linked tasks should be notified of the change
```

**AC-TIMEBLOCK-004: Split Time Block**
```gherkin
Given I have a time block that needs to be divided
When I split the block at a specific time
Then two separate blocks should be created
And properties should be preserved appropriately
And any linked tasks should handle the split correctly
```

**AC-TIMEBLOCK-005: Time Block Priority**
```gherkin
Given I have overlapping time blocks with different priorities
When conflicts occur
Then higher priority blocks should take precedence
And lower priority blocks should be rescheduled if flexible
And conflicts should be presented for resolution if needed
```

### FocusSession Aggregate Root

**AC-FOCUS-001: Start Focus Session**
```gherkin
Given I want to begin focused work
When I start a focus session with intended focus level and goal
Then the session should begin tracking time
And interruptions should be monitorable
And I should be able to specify Pomodoro settings if desired
```

**AC-FOCUS-002: Log Interruptions**
```gherkin
Given I am in an active focus session
When I experience an interruption
Then I should be able to log the interruption type and duration
And the interruption should impact the productivity score
And I should be able to resume the session after interruption
```

**AC-FOCUS-003: Complete Focus Session**
```gherkin
Given I have an active focus session
When I complete the session
Then the actual focus level and productivity should be recorded
And the system should calculate a productivity score
And the data should be used for future session optimization
```

**AC-FOCUS-004: Pomodoro Integration**
```gherkin
Given I want to use the Pomodoro Technique
When I configure a focus session with Pomodoro settings
Then the session should be divided into work and break periods
And I should receive notifications for transitions
And break times should be enforced or suggested
```

**AC-FOCUS-005: Focus Session Analytics**
```gherkin
Given I have completed multiple focus sessions
When I review my focus analytics
Then I should see:
- Average productivity scores over time
- Best focus times by hour/day
- Common interruption patterns
- Focus level achievements
And the system should suggest optimal focus times
```

### TimeEntry Aggregate Root

**AC-TIMEENTRY-001: Start Time Tracking**
```gherkin
Given I am beginning work on a task
When I start time tracking
Then a new time entry should begin
And the start time should be recorded
And only one active time entry should exist per user
```

**AC-TIMEENTRY-002: Stop Time Tracking**
```gherkin
Given I have an active time entry
When I stop time tracking
Then the end time should be recorded
And the duration should be calculated
And the entry should be saved with activity details
```

**AC-TIMEENTRY-003: Manual Time Entry**
```gherkin
Given I need to log time worked previously
When I create a manual time entry with start/end times
Then the entry should be validated for overlaps
And the duration should be calculated automatically
And the entry should be marked as manual vs automatic
```

**AC-TIMEENTRY-004: Billable Time Tracking**
```gherkin
Given I am working on billable tasks
When I log time entries
Then I should be able to mark entries as billable
And billable entries should require associated project/client
And billing rates should be applicable if configured
```

**AC-TIMEENTRY-005: Time Entry Analytics**
```gherkin
Given I have time entries logged
When I generate time reports
Then I should see:
- Time by task, project, client
- Billable vs non-billable time breakdown
- Daily/weekly/monthly summaries
- Productivity trends and patterns
```

### Routine Aggregate Root

**AC-ROUTINE-001: Create Routine**
```gherkin
Given I want to establish a routine
When I create a routine with name, steps, and recurrence pattern
Then the routine should be created with all specified steps
And the routine should be scheduled according to the pattern
And each step should be trackable individually
```

**AC-ROUTINE-002: Execute Routine**
```gherkin
Given I have a routine scheduled for today
When I execute the routine
Then I should be able to mark each step as completed
And the overall routine completion should be calculated
And adherence should be tracked for streak counting
```

**AC-ROUTINE-003: Track Adherence**
```gherkin
Given I have established routines
When I complete or miss routine executions
Then adherence should be tracked as percentages
And streaks should be maintained for successful completions
And missed routines should reset streaks appropriately
```

**AC-ROUTINE-004: Optimize Routine**
```gherkin
Given I have routine execution history
When the system analyzes routine performance
Then optimization suggestions should be provided:
- Best times for routine execution
- Steps that are frequently skipped
- Duration optimization opportunities
- Trigger effectiveness analysis
```

**AC-ROUTINE-005: Behavioral Triggers**
```gherkin
Given I am creating a routine
When I specify behavioral triggers (time, location, event)
Then the routine should be activated when triggers occur
And trigger effectiveness should be tracked
And I should be able to modify triggers based on success rates
```

### HolidayManager Aggregate Root

**AC-HOLIDAY-001: Define Holidays**
```gherkin
Given I need to manage holidays and time off
When I define holidays (system-wide or personal)
Then holidays should be created with appropriate types
And recurring holidays should be automatically generated
And holidays should affect availability calculations
```

**AC-HOLIDAY-002: Request Time Off**
```gherkin
Given I want to request time off
When I submit a time-off request with dates and reason
Then the request should be created with pending status
And my manager should be notified for approval
And my availability should be blocked for the requested dates
```

**AC-HOLIDAY-003: Approve Time Off**
```gherkin
Given I have time-off requests to review
When I approve or reject requests
Then the request status should be updated
And the requester should be notified
And approved time off should block availability
```

**AC-HOLIDAY-004: Holiday Impact on Scheduling**
```gherkin
Given I have holidays and time off defined
When scheduling meetings or time blocks
Then holidays should be considered as unavailable time
And the system should suggest alternative times
And holiday conflicts should be clearly indicated
```

### TimeEstimationAccuracyRecord Aggregate Root

**AC-ESTIMATION-001: Record Estimation Accuracy**
```gherkin
Given a task is completed with time tracking
When the actual time differs from the estimated time
Then an accuracy record should be created automatically
And the variance percentage should be calculated
And the record should include estimation method used
```

**AC-ESTIMATION-002: Learn from Estimation History**
```gherkin
Given I have multiple estimation accuracy records
When I estimate time for similar tasks
Then the system should provide improved estimates based on:
- My historical accuracy patterns
- Similar task completion times
- Complexity and category adjustments
- Confidence level considerations
```

**AC-ESTIMATION-003: Estimation Confidence Tracking**
```gherkin
Given I am making time estimates
When I specify my confidence level in the estimate
Then confidence should be tracked against actual accuracy
And low confidence estimates should be weighted differently
And confidence patterns should inform future estimations
```

**AC-ESTIMATION-004: Three-Point Estimation Support**
```gherkin
Given I am using three-point estimation
When I provide optimistic, pessimistic, and most likely estimates
Then the system should calculate expected duration
And track which estimate was closest to actual
And learn from the distribution patterns for future estimates
```

---

