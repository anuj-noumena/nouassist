# Features Specification

## Overview
This specification outlines the core features of a comprehensive productivity platform designed to manage projects, tasks, schedules, learning, GTD workflows, reviews, and a centralized Today's Dashboard. Enhancements focus on optimizing UI/UX for maximum productivity: intuitive navigation, minimal cognitive load, quick actions, visual hierarchy, and seamless integrations across features. The platform prioritizes mobile-first responsiveness, dark/light mode support, accessibility (WCAG 2.1 AA), and performance (sub-2s load times via lazy loading and caching).

Key UI/UX Principles:
- **Productivity-First:** Dashboard as the daily "command center" with at-a-glance insights and one-tap actions.
- **Customization:** Drag-and-drop sections, AI-suggested layouts based on user behavior.
- **Feedback Loops:** Real-time progress updates, micro-animations for completions, gamification (streaks, badges).
- **Integrations:** Auto-pull data from other features; AI assistants for prioritization and summaries.
- **Mock Data:** All entities include generation instructions for realistic, diverse samples.

---

## Feature: Projects

### Entity: Project
Properties:
- id: String (primary key)
- name: String (required, min 3, max 100)
- description: String (optional, max 500)
- parentProjectId: String? (nullable, for hierarchical structure)
- startDate: DateTime (required)
- endDate: DateTime (required)
- status: Enum (Planning, Active, On Hold, Completed, Cancelled)
- priority: Enum (Low, Medium, High, Critical)
- viewType: Enum (List, Kanban, Gantt) – default synced to user preference
- teamMembers: List<String> (user IDs)
- ownerId: String (required, user ID)
- workflowId: String (required)
- color: String (hex color code) – auto-suggest based on status
- progress: Decimal (0-100, calculated from tasks/sprints)
- budget: Decimal? (optional)
- actualCost: Decimal? (optional)
- creationTime: DateTime
- lastModificationTime: DateTime?
- aiSummary: String? (optional, auto-generated overview for dashboard previews)

Operations:
- List (with pagination, filter by status/priority/owner, search by name, sort by name/startDate/priority; include AI-suggested "Recommended" filter)
- Get by ID (includes child projects, team, tasks summary, AI insights)
- Create (requires name, startDate, endDate, owner, workflow; auto-set viewType from user config)
- Update (all fields, recalculate progress, trigger notifications)
- Delete (cascade delete child projects, with confirmation modal showing impact)
- Move (change parent project, with drag-drop UI)
- Clone (duplicate project structure, excluding history)
- Archive (soft delete, auto-move to "Archived" view)
- Change View (switch between List/Kanban/Gantt; persist per project)

Screens:
1. Project List Screen (hierarchical tree view with expand/collapse, search bar with autocomplete, filters as chips, floating create button; infinite scroll for pagination)
2. Project Kanban Board (drag-drop columns with swim lanes by team/priority; quick-add cards, real-time collaboration indicators)
3. Project Gantt Chart (interactive timeline with zoom, dependencies as arrows, milestones as diamonds; export to PDF)
4. Project Form Screen (stepper wizard for create/edit with validation, team selector as searchable dropdown, color picker)
5. Project Details Screen (tabbed layout: overview with AI summary, team avatars, tasks burndown chart, timeline slider, budget variance gauge)
6. Project Dashboard (compact metrics cards, progress donut charts, recent activities feed; embeddable in Today's Dashboard)

UI/UX Enhancements: Use color-coded badges for status/priority; hover tooltips for quick stats; keyboard shortcuts (e.g., 'N' for new project).

Mock Data: Generate 20 sample projects with 3 levels of hierarchy, various statuses; include diverse names (e.g., "Q4 Marketing Campaign"), realistic dates, 40% with budgets.

---

### Entity: ProjectTeam
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- userId: String (required, foreign key)
- roleId: String (required, foreign key)
- assignedDate: DateTime
- billableRate: Decimal? (optional)
- availabilityScore: Decimal? (0-100, calculated from schedules)

Operations:
- List by Project (include availability previews)
- Add Team Member (bulk import from CSV/user search)
- Remove Team Member (with reassignment prompts)
- Update Role (role dropdown with permission previews)
- Bulk Add Members (search + assign)

Screens:
1. Team Management Screen (avatar grid with roles, drag-drop to reorder; capacity utilization bars)
2. Team Member Details (workload heatmap, assigned tasks pie chart, availability calendar snippet)

UI/UX Enhancements: Real-time presence indicators; quick-chat links to users.

Mock Data: Generate team assignments for all projects with 3-8 members each; mix roles, 20% with billable rates.

---

### Entity: ProjectRole
Properties:
- id: String (primary key)
- name: String (required, unique, max 50)
- description: String (optional, max 200)
- permissions: List<String> (view, edit, delete, manage_team, manage_sprints)
- isSystemRole: bool (default false)
- creationTime: DateTime

Operations:
- List (sortable table with permission toggles)
- Get by ID
- Create (requires name, permissions; template presets)
- Update
- Delete (prevent if assigned to users; suggest alternatives)

Screens:
1. Roles List Screen (permissions matrix as interactive table; export CSV)
2. Role Form Screen (create/edit with permission checkboxes, search/filter)

UI/UX Enhancements: Visual permission tree for complex roles.

Mock Data: Generate 8 sample roles (Project Manager, Developer, Designer, QA, Stakeholder, Viewer, Admin, Guest); predefined permissions.

---

### Entity: ProjectWorkflow
Properties:
- id: String (primary key)
- name: String (required, max 50)
- description: String (optional, max 200)
- stages: List<WorkflowStage> (required, min 2)
- isDefault: bool (default false)
- projectType: String (optional)
- creationTime: DateTime

SubEntity: WorkflowStage
- id: String
- name: String (required)
- order: Integer (required)
- color: String (hex code)
- isInitial: bool
- isFinal: bool
- allowedTransitions: List<String> (stage IDs)
- autoAdvanceRules: String? (JSON for conditions, e.g., "progress > 80")

Operations:
- List (with preview thumbnails)
- Get by ID
- Create (requires name, stages; AI-suggest stages based on type)
- Update
- Delete (prevent if used by projects)
- Clone (with stage customization)

Screens:
1. Workflow List Screen (card grid with stage previews)
2. Workflow Builder (visual drag-drop editor with transition lines, rule toggles; undo/redo)
3. Workflow Preview (simulate task flow)

UI/UX Enhancements: Auto-validate transitions for loops; color harmony checker.

Mock Data: Generate 5 sample workflows (Agile with 5 stages, Waterfall with 7, Kanban continuous, Custom Dev with branches, Design Process iterative).

---

### Entity: ProjectEvent
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- title: String (required, max 100)
- description: String (optional, max 500)
- eventType: Enum (Meeting, Milestone, Deadline, Review, Demo)
- startDateTime: DateTime (required)
- endDateTime: DateTime (required)
- location: String (optional, max 200)
- isAllDay: bool (default false)
- attendees: List<String> (user IDs)
- reminderMinutes: Integer? (optional, 15/30/60/1440)
- meetingLink: String? (optional, URL)
- creationTime: DateTime
- rsvpStatus: Map<String, Enum> (userId -> Accepted/Declined/Tentative)

Operations:
- List by Project (filter by eventType, date range; iCal export)
- Get by ID
- Create (auto-invite attendees)
- Update
- Delete
- Send Reminders (push/email)
- RSVP Update

Screens:
1. Project Calendar View (Google Calendar-like: month/week/day/agenda; color by type)
2. Event List Screen (upcoming cards with RSVP counts)
3. Event Form Screen (create/edit with attendee multi-select, recurrence toggle)
4. Event Details Screen (agenda view, join button prominent; chat integration)

UI/UX Enhancements: Conflict detection; one-click join for video links.

Mock Data: Generate 30 events across projects with various types; 50% recurring, mixed RSVPs.

---

### Entity: ProjectSprint
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- name: String (required, max 50)
- goal: String (optional, max 300)
- startDate: DateTime (required)
- endDate: DateTime (required)
- status: Enum (Planning, Active, Completed, Cancelled)
- velocity: Integer? (story points)
- capacity: Integer? (total hours)
- actualHours: Decimal (calculated)
- creationTime: DateTime
- burndownData: String? (JSON for chart)

Operations:
- List by Project (filter by status; timeline slider)
- Get by ID (includes tasks, burndown data)
- Create (auto-populate from backlog)
- Update
- Delete (only if no tasks assigned)
- Start Sprint (lock tasks)
- Complete Sprint (calculate velocity)
- Generate Burndown Chart (real-time updates)

Screens:
1. Sprint List Screen (horizontal timeline with status badges)
2. Sprint Board (Kanban for sprint tasks; WIP limits)
3. Sprint Form Screen (goal textarea, capacity calculator)
4. Sprint Details (goals expandable, tasks table, burndown line chart, velocity bar)
5. Sprint Planning Screen (backlog drag-drop, capacity pie chart)

UI/UX Enhancements: Auto-suggest sprint length based on historical velocity.

Mock Data: Generate 15 sprints across projects (3-5 per active project); include burndown JSON.

---

### Entity: ProjectRequirement
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- title: String (required, max 150)
- description: String (required, max 2000)
- requirementType: Enum (Functional, Non-Functional, Business, Technical)
- priority: Enum (Must Have, Should Have, Could Have, Won't Have)
- status: Enum (Draft, Approved, In Progress, Completed, Rejected)
- acceptanceCriteria: String (optional, max 1000)
- estimatedEffort: Decimal? (hours)
- parentRequirementId: String? (nullable)
- tags: List<String>
- createdBy: String (user ID)
- approvedBy: String? (user ID)
- creationTime: DateTime
- approvalTime: DateTime?
- traceabilityLinks: List<String> (task IDs)

Operations:
- List by Project (hierarchical view, filter by type/priority/status; search with fuzzy matching)
- Get by ID
- Create
- Update
- Delete
- Approve/Reject (with comment requirement)
- Link to Tasks (bulk link)
- Export (PDF/Excel with traceability)

Screens:
1. Requirements List Screen (hierarchical tree with expanders, filters as sidebar, traceability badges)
2. Requirements Form Screen (rich text editor with criteria checklist builder)
3. Requirements Details Screen (linked tasks accordion, approval history timeline)
4. Requirements Traceability Matrix (interactive table with forward/backward links)

UI/UX Enhancements: AI-generate acceptance criteria from description; color-coded priority bubbles.

Mock Data: Generate 40 requirements across projects with hierarchy; 30% linked to tasks.

---

### Entity: ProjectAttachment
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- fileName: String (required)
- fileSize: Long (bytes)
- fileType: String (MIME type)
- fileUrl: String (storage path/URL)
- uploadedBy: String (user ID)
- description: String? (optional, max 200)
- tags: List<String>
- uploadTime: DateTime
- version: Integer (default 1, for updates)

Operations:
- List by Project (filter by fileType, sort by uploadTime; thumbnail previews)
- Upload (with progress bar, drag-drop zone, auto-tagging)
- Download
- Delete (with confirmation, version history)
- Preview (for images/PDFs; full-screen modal)
- Search by filename/tags (global search integration)
- Version Control (upload new version)

Screens:
1. Attachments List Screen (grid/list toggle, upload button as + icon; search bar)
2. Attachment Preview Modal (zoom/pan for images, scroll for PDFs)
3. Upload Dialog (multi-file, auto-scan for metadata)

UI/UX Enhancements: OCR search for PDFs; file health check (virus scan indicator).

Mock Data: Generate 50 sample attachments (documents, images, spreadsheets); varied sizes/types.

---

### Entity: ProjectDiscussion
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- parentDiscussionId: String? (nullable, for threading)
- authorId: String (required, user ID)
- content: String (required, max 2000)
- mentions: List<String> (user IDs)
- attachmentIds: List<String>
- isEdited: bool (default false)
- isPinned: bool (default false)
- reactions: Map<String, List<String>> (emoji -> user IDs)
- creationTime: DateTime
- lastEditTime: DateTime?
- readBy: List<String> (for unread counts)

Operations:
- List by Project (threaded view, pagination, sort by date; unread filters)
- Get Thread (full context load)
- Create (with mentions @autocomplete, attachments drag-drop)
- Update (own messages only; edit history toggle)
- Delete (own messages only, soft delete with undo)
- Pin/Unpin (admin only)
- Add Reaction (emoji picker)
- Search Discussions (full-text, highlight matches)

Screens:
1. Discussion Board Screen (threaded collapsible, filters sidebar, search with live results)
2. Discussion Composer (rich text toolbar, mention dropdown, file uploader)
3. Discussion Thread View (nested replies, quote parent, real-time typing indicators)

UI/UX Enhancements: AI-summarize long threads; voice-to-text input.

Mock Data: Generate 100 discussion messages across projects with threads; include reactions/mentions.

---

### Entity: ProjectSuccessCriteria
Properties:
- id: String (primary key)
- projectId: String (required, foreign key)
- criterion: String (required, max 200)
- description: String (optional, max 500)
- measurementType: Enum (Quantitative, Qualitative, Binary)
- targetValue: String? (optional)
- actualValue: String? (optional)
- unit: String? (optional)
- status: Enum (Not Started, In Progress, Achieved, Not Achieved)
- weight: Decimal (0-1, for overall calculation)
- verificationMethod: String (optional, max 300)
- creationTime: DateTime
- achievementTime: DateTime?
- evidenceAttachmentId: String? (optional)

Operations:
- List by Project (sortable by weight/status)
- Get by ID
- Create
- Update
- Delete
- Mark as Achieved (with evidence upload prompt)
- Calculate Overall Success Score (weighted average, visualized as gauge)

Screens:
1. Success Criteria List Screen (cards with status progress bars, weight sliders)
2. Success Criteria Form Screen (type-specific inputs, e.g., number input for quantitative)
3. Success Dashboard (radar chart for criteria, overall score badge; integrate with project dashboard)

UI/UX Enhancements: Auto-calculate score on updates; threshold alerts for at-risk criteria.

Mock Data: Generate 25 success criteria across projects; mix types, 60% achieved.

---

## Feature: Tasks

### Entity: Task
Properties:
- id: String (primary key)
- title: String (required, min 3, max 150)
- description: String (optional, max 2000)
- parentTaskId: String? (nullable, for hierarchical structure)
- projectId: String? (optional, foreign key)
- sprintId: String? (optional, foreign key)
- taskTypeId: String (required, foreign key)
- workflowStageId: String (required, foreign key)
- assigneeId: String? (optional, user ID)
- reporterId: String (required, user ID)
- priority: Enum (Lowest, Low, Medium, High, Highest)
- status: Enum (Todo, In Progress, In Review, Done, Blocked)
- estimatedHours: Decimal? (optional)
- actualHours: Decimal? (optional)
- storyPoints: Integer? (optional)
- startDate: DateTime?
- dueDate: DateTime?
- completedDate: DateTime?
- tags: List<String>
- watchers: List<String> (user IDs)
- isRecurring: bool (default false)
- recurrencePattern: RecurrencePattern? (optional)
- customFields: Map<String, Any> (dynamic fields)
- progress: Decimal (0-100)
- creationTime: DateTime
- lastModificationTime: DateTime?
- aiPriorityScore: Decimal? (0-100, auto-calculated from due date/effort)

SubEntity: RecurrencePattern
- frequency: Enum (Daily, Weekly, Monthly, Yearly)
- interval: Integer (every N days/weeks/months)
- daysOfWeek: List<DayOfWeek>? (for weekly)
- dayOfMonth: Integer? (for monthly)
- endDate: DateTime? (optional)
- occurrences: Integer? (optional)

Operations:
- List (with pagination, hierarchical view, filter by status/priority/assignee/project/sprint, search by title/AI semantic, sort by priority/dueDate/creationTime; "My Overdue" smart filter)
- Get by ID (includes subtasks, dependencies, history timeline)
- Create (requires title, taskType, workflow stage, reporter; AI-suggest assignee/du e)
- Update (all fields; bulk for selections)
- Delete (with confirmation, cascade to subtasks)
- Move (change parent task, drag-drop)
- Clone
- Assign/Reassign (with availability check)
- Change Status (dropdown with transition rules)
- Log Time (quick timer button)
- Add Watchers (multi-select)
- Generate Recurring Tasks (auto-create series)
- Bulk Update (status, assignee, tags; CSV import)

Screens:
1. Task List Screen (hierarchical tree with inline editing, Kanban toggle, filters as pills, search with suggestions, bulk checkbox actions)
2. Task Board (Kanban by status, swimlanes by assignee/priority; drag-drop with snap feedback)
3. Task Form Screen (create/edit modal with all fields, custom fields dynamic form, AI description generator)
4. Task Details Screen (collapsible tabs: details with progress slider, subtasks tree, checklist toggle, attachments gallery, discussion feed, action steps numbered list, time logs table, history changelog)
5. My Tasks Screen (personal filtered view, today/week/month tabs with calendar integration)
6. Task Calendar View (due dates as events, drag-drop rescheduling, color by priority)

UI/UX Enhancements: Pomodoro timer integration; AI chat for task breakdown; voice commands for status updates.

Mock Data: Generate 100 tasks with hierarchy (3 levels), various statuses, assignments, 20% recurring; include custom fields.

---

### Entity: TaskType
Properties:
- id: String (primary key)
- name: String (required, unique, max 50)
- icon: String (icon name/emoji)
- color: String (hex code)
- description: String (optional, max 200)
- defaultWorkflowId: String (foreign key)
- isActive: bool (default true)
- creationTime: DateTime

Operations:
- List (filter by isActive; icon/color previews)
- Get by ID
- Create
- Update
- Delete (prevent if tasks exist; migrate option)
- Deactivate (soft hide)

Screens:
1. Task Types List Screen (colorful cards with icon badges)
2. Task Type Form Screen (icon selector library)

UI/UX Enhancements: Drag-drop to reorder types in boards.

Mock Data: Generate 10 task types (Bug, Feature, Story, Epic, Task, Subtask, Improvement, Spike, Research, Documentation); assign icons/colors.

---

### Entity: TaskWorkflow
Properties:
- id: String (primary key)
- name: String (required, max 50)
- description: String (optional, max 200)
- stages: List<WorkflowStage> (required, min 2)
- taskTypeIds: List<String> (applicable task types)
- isDefault: bool (default false)
- creationTime: DateTime

SubEntity: WorkflowStage
- id: String
- name: String (required)
- order: Integer (required)
- color: String (hex code)
- statusCategory: Enum (Todo, InProgress, Done)
- allowedTransitions: List<String> (stage IDs)
- requiresComment: bool (default false)
- automationRules: String? (JSON, e.g., "auto to Done if subtasks complete")

Operations:
- List
- Get by ID
- Create (AI-suggest stages)
- Update
- Delete (prevent if used)
- Clone

Screens:
1. Task Workflow List Screen (thumbnails)
2. Workflow Builder (drag-drop canvas with validation)

UI/UX Enhancements: Simulate workflow with sample task.

Mock Data: Generate 6 workflows (Simple 3-stage, Development 5-stage, Bug Tracking 4-stage, Review Process branched, Agile Scrum, Custom QA).

---

### Entity: TaskCustomField
Properties:
- id: String (primary key)
- name: String (required, max 50)
- fieldType: Enum (Text, Number, Date, DateTime, Dropdown, MultiSelect, Checkbox, URL, Email, Formula)
- description: String (optional, max 200)
- options: List<String>? (for Dropdown/MultiSelect)
- isRequired: bool (default false)
- defaultValue: String? (optional)
- taskTypeIds: List<String> (applicable to specific task types)
- projectIds: List<String>? (optional, project-specific fields)
- validationRules: String? (JSON, e.g., regex/min-max)
- displayOrder: Integer
- isActive: bool (default true)
- creationTime: DateTime

Operations:
- List (filter by taskType/project; preview in context)
- Get by ID
- Create
- Update
- Delete (soft delete; data migration)
- Reorder (drag-drop)

Screens:
1. Custom Fields List Screen (drag-drop reorder, type previews)
2. Custom Field Builder (wizard for type/options, formula editor)
3. Custom Fields Configuration (per task type/project toggle)

UI/UX Enhancements: AI-generate formula fields (e.g., "due date + 2 days").

Mock Data: Generate 15 custom fields for various task types (e.g., "Severity" dropdown for Bugs).

---

### Entity: TaskTag
Properties:
- id: String (primary key)
- name: String (required, unique, max 30)
- color: String (hex code)
- description: String? (optional, max 100)
- category: String? (optional, for grouping)
- usageCount: Integer (calculated)
- creationTime: DateTime

Operations:
- List (search by name, sort by name/usageCount; category filters)
- Get by ID
- Create (auto-color suggest)
- Update
- Delete (with confirmation, merge prompt)
- Merge Tags (bulk)

Screens:
1. Tags List Screen (color chips with usage badges, category accordions)
2. Tag Form Screen
3. Tag Manager (bulk select, merge wizard)

UI/UX Enhancements: Auto-complete tag suggestions in task forms.

Mock Data: Generate 40 tags with categories (technology, priority, department); varied usage.

---

### Entity: TaskAttachment
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- fileName: String (required)
- fileSize: Long (bytes)
- fileType: String (MIME type)
- fileUrl: String (storage path/URL)
- uploadedBy: String (user ID)
- description: String? (optional, max 200)
- uploadTime: DateTime
- thumbnailUrl: String? (generated for previews)

Operations:
- List by Task (sorted, previews)
- Upload (inline drag-drop)
- Download
- Delete
- Preview (modal with tools)

Screens:
1. Attachments Section (carousel for multiples, + upload button)
2. Upload Dialog (progress circle)

UI/UX Enhancements: Annotate images in preview.

Mock Data: Generate 80 attachments across tasks; mix types/sizes.

---

### Entity: TaskDiscussion
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- parentDiscussionId: String? (nullable)
- authorId: String (required, user ID)
- content: String (required, max 2000)
- mentions: List<String> (user IDs)
- attachmentIds: List<String>
- isEdited: bool (default false)
- reactions: Map<String, List<String>>
- creationTime: DateTime
- lastEditTime: DateTime?
- resolved: bool (default false)

Operations:
- List by Task (threaded, @mentions notify)
- Create (inline composer)
- Update
- Delete
- Add Reaction
- Resolve Thread

Screens:
1. Discussion Section (nested list, resolve button)
2. Comment Composer (emoji/reaction preview)

UI/UX Enhancements: Thread collapse on resolve; AI-suggest replies.

Mock Data: Generate 150 comments across tasks with threads; 20% resolved.

---

### Entity: TaskAcceptanceCriteria
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- criterion: String (required, max 200)
- isCompleted: bool (default false)
- order: Integer
- creationTime: DateTime
- completionTime: DateTime?
- verifiedBy: String? (user ID)

Operations:
- List by Task
- Create (bulk add)
- Update
- Delete
- Mark Complete/Incomplete (with verification toggle)
- Reorder (drag-drop)

Screens:
1. Acceptance Criteria Section (interactive checklist with progress bar)
2. Criteria Editor (inline add/edit)

UI/UX Enhancements: Auto-check based on status changes.

Mock Data: Generate 100 acceptance criteria across tasks; 70% completed.

---

### Entity: TaskActionStep
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- description: String (required, max 200)
- isCompleted: bool (default false)
- assigneeId: String? (optional, user ID)
- dueDate: DateTime? (optional)
- order: Integer
- creationTime: DateTime
- completionTime: DateTime?

Operations:
- List by Task
- Create
- Update
- Delete
- Mark Complete/Incomplete
- Reorder

Screens:
1. Action Steps Section (numbered list with assignee avatars, due badges)
2. Action Step Editor (inline, quick-assign dropdown)

UI/UX Enhancements: Dependency links between steps.

Mock Data: Generate 120 action steps across tasks; mix assignees/dues.

---

### Entity: TaskChecklist
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- title: String (required, max 100)
- items: List<ChecklistItem> (required)
- progress: Decimal (0-100, calculated)
- creationTime: DateTime

SubEntity: ChecklistItem
- id: String
- text: String (required, max 200)
- isCompleted: bool (default false)
- order: Integer
- completedBy: String? (user ID)
- completionTime: DateTime?

Operations:
- List by Task
- Get by ID
- Create
- Update
- Delete
- Add Item (template presets, e.g., "Code Review")
- Remove Item
- Toggle Item
- Reorder Items

Screens:
1. Checklist Section (collapsible with overall progress ring)
2. Checklist Editor (drag-drop items, bulk toggle)

UI/UX Enhancements: Shareable checklists; import from templates.

Mock Data: Generate 60 checklists across tasks with 5-10 items each; varied progress.

---

### Entity: TaskDelegation
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- delegatedFrom: String (user ID)
- delegatedTo: String (user ID)
- reason: String? (optional, max 300)
- delegationDate: DateTime
- acceptanceStatus: Enum (Pending, Accepted, Rejected)
- responseDate: DateTime?
- responseComment: String? (optional, max 300)

Operations:
- List by User (delegated to/from me)
- Create Delegation (with reason template)
- Accept Delegation
- Reject Delegation
- Cancel Delegation
- Delegation History (timeline)

Screens:
1. Delegation Requests Screen (inbox-style cards, accept/reject buttons)
2. Delegation Dialog (user search, reason editor)
3. Delegation History (tab in task details)

UI/UX Enhancements: Auto-notify on acceptance; delegation analytics.

Mock Data: Generate 30 delegations across tasks with various statuses.

---

### Entity: TaskDependency
Properties:
- id: String (primary key)
- predecessorTaskId: String (required, foreign key)
- successorTaskId: String (required, foreign key)
- dependencyType: Enum (FinishToStart, StartToStart, FinishToFinish, StartToFinish)
- lagDays: Integer (default 0, can be negative for lead time)
- creationTime: DateTime

Operations:
- List by Task (predecessors/successors)
- Create Dependency (task picker)
- Delete Dependency
- Validate (prevent circular; visual warning)
- Get Dependency Chain (critical path highlight)
- Update Critical Path (auto-recalculate dates)

Screens:
1. Dependencies Section (force-directed graph with nodes/lines)
2. Add Dependency Dialog (visual type selector)
3. Dependency Graph View (zoomable network, export PNG)

UI/UX Enhancements: Auto-suggest dependencies from similar tasks.

Mock Data: Generate 50 dependencies across tasks; mix types.

---

### Entity: TaskSchedule
Properties:
- id: String (primary key)
- taskId: String (required, foreign key)
- scheduledDate: DateTime (required)
- scheduledDuration: Integer (minutes)
- timeBlockId: String? (optional, foreign key)
- status: Enum (Scheduled, Completed, Rescheduled, Cancelled)
- completionNotes: String? (optional, max 500)
- creationTime: DateTime

Operations:
- List by Task/User/Date
- Create Schedule
- Update Schedule
- Reschedule (drag to new slot)
- Mark Complete
- Cancel Schedule
- Get Schedule Conflicts (highlight overlaps)

Screens:
1. Task Scheduling Section (mini-calendar embed)
2. Schedule Calendar View (drag-drop with conflict popups)
3. Schedule Conflict Resolution Dialog (suggest alternatives)

UI/UX Enhancements: Integrate with Google/Outlook sync.

Mock Data: Generate 70 scheduled tasks across users and dates.

---

### Entity: TaskPrioritization
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- taskId: String (required, foreign key)
- rank: Integer (required, unique per user)
- prioritizationMethod: Enum (Manual, Eisenhower, MoSCoW, RICE, Value vs Effort)
- urgency: Integer? (1-5, for Eisenhower)
- importance: Integer? (1-5, for Eisenhower)
- reach: Integer? (for RICE)
- impact: Integer? (for RICE)
- confidence: Integer? (for RICE)
- effort: Integer? (for RICE/Value vs Effort)
- value: Integer? (for Value vs Effort)
- lastUpdated: DateTime

Operations:
- List by User (ordered by rank)
- Rerank Tasks (drag-drop list)
- Auto-Prioritize (AI based on method)
- Reset Prioritization
- Get Priority Score (calculated formula)

Screens:
1. Task Prioritization Screen (sortable cards, method switcher)
2. Eisenhower Matrix View (4-quadrant drag-drop)
3. Priority Score Calculator (sliders for inputs)
4. Bulk Prioritization Tool (select + apply method)

UI/UX Enhancements: Daily re-prioritize nudge; integrate scores into Today's Dashboard.

Mock Data: Generate prioritization data for 50 tasks per user; mix methods.

---

## Feature: Scheduling

### Entity: TimeBlock
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- title: String (required, max 100)
- description: String? (optional, max 300)
- startDateTime: DateTime (required)
- endDateTime: DateTime (required)
- blockType: Enum (Work, Personal, Break, Meeting, Focus, Admin, Learning)
- color: String (hex code) – auto from type
- isRecurring: bool (default false)
- recurrencePattern: RecurrencePattern? (optional)
- taskId: String? (optional, foreign key)
- projectId: String? (optional, foreign key)
- location: String? (optional, max 100)
- isFlexible: bool (default false)
- reminderMinutes: Integer? (optional)
- actualStartTime: DateTime? (optional)
- actualEndTime: DateTime? (optional)
- status: Enum (Scheduled, InProgress, Completed, Cancelled)
- creationTime: DateTime
- productivityScore: Decimal? (post-block self-rate, 1-5)

Operations:
- List by User (filter by date range, blockType; gap finder)
- Get by ID
- Create (quick-add from task)
- Update
- Delete
- Reschedule (drag-drop with ripple effect)
- Mark InProgress/Completed (timer auto-start)
- Cancel
- Generate Recurring Blocks
- Find Available Slots (AI-optimized)
- Detect Conflicts (color-coded warnings)

Screens:
1. Time Block Calendar (full-screen month/week/day/agenda; color-coded, drag to resize/create)
2. Time Block Form Screen (type presets, task linker)
3. Time Block List View (daily accordion agenda)
4. Time Block Analytics (pie chart by type, productivity trend line; export report)

UI/UX Enhancements: Focus mode (dim non-current blocks); voice-schedule.

Mock Data: Generate 200 time blocks across users for next 30 days; 30% recurring, varied types.

---

### Entity: UserWorkingSchedule
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- dayOfWeek: DayOfWeek (required)
- isWorkingDay: bool (default true)
- startTime: Time (required if isWorkingDay)
- endTime: Time (required if isWorkingDay)
- breakStartTime: Time? (optional)
- breakEndTime: Time? (optional)
- timezone: String (required)
- effectiveFrom: DateTime (required)
- effectiveTo: DateTime? (optional)
- isDefault: bool (default false)

Operations:
- Get User Schedule (for date range, expand to events)
- Create Schedule
- Update Schedule
- Delete Schedule
- Get Available Hours (slot suggestions)
- Get Working Days (holiday overrides)
- Apply Default Schedule (one-click templates)

Screens:
1. Working Schedule Settings Screen (weekly grid with time pickers, holiday calendar)
2. Schedule Template Selector (presets: 9-5, Remote Flex; customize)
3. Working Hours Calendar View (overlay on main calendar)
4. Time Zone Manager (auto-detect, converter tool)

UI/UX Enhancements: Drag to adjust daily blocks; conflict preview.

Mock Data: Generate working schedules for all users (standard 9-5, some flexible, 10% holidays).

---

### Entity: Event
Properties:
- id: String (primary key)
- userId: String (required, foreign key - event creator)
- title: String (required, max 150)
- description: String? (optional, max 1000)
- eventType: Enum (Meeting, Appointment, Reminder, Deadline, Personal, Holiday)
- startDateTime: DateTime (required)
- endDateTime: DateTime (required)
- isAllDay: bool (default false)
- location: String? (optional, max 200)
- meetingLink: String? (optional, URL)
- attendees: List<String> (user IDs)
- isRecurring: bool (default false)
- recurrencePattern: RecurrencePattern? (optional)
- reminderMinutes: List<Integer> (e.g., [15, 60, 1440])
- color: String (hex code)
- visibility: Enum (Public, Private, Confidential)
- status: Enum (Tentative, Confirmed, Cancelled)
- projectId: String? (optional, foreign key)
- taskId: String? (optional, foreign key)
- timeBlockId: String? (optional, foreign key)
- creationTime: DateTime
- lastModificationTime: DateTime?
- rsvpResponses: Map<String, Enum> (userId -> response)

Operations:
- List by User (filter by date range, eventType; shared calendars)
- Get by ID
- Create (with attendee search, auto-propose times)
- Update (notify changes)
- Delete
- Cancel Event (bulk for series)
- Respond to Invitation (Accept/Decline/Tentative; one-tap)
- Send Reminders
- Generate Recurring Events
- Check Availability (free/busy grid)
- Find Meeting Times (polls for groups)

Screens:
1. Event Calendar (multi-view with search, legend for types; sync with external)
2. Event Form Screen (attendee chips, recurrence wizard, link to task/project)
3. Event Details Screen (RSVP bar, response list, edit history)
4. Availability Grid (heatmapped weeks for group scheduling)
5. Event List View (timeline cards, filter chips)

UI/UX Enhancements: AI-suggest optimal times; emoji reactions on invites.

Mock Data: Generate 150 events across users with various types and recurrences; 40% group events.

---

## Feature: Learning

### Entity: LearningTopic
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- title: String (required, min 3, max 150)
- description: String? (optional, max 1000)
- parentTopicId: String? (nullable, for hierarchical structure)
- category: String? (optional, max 50)
- tags: List<String>
- workflowStageId: Enum (Phase1_PreLearning, Phase2_ActiveLearning, Phase3_ImmediateConsolidation, Phase4_FirstReview, Phase5_SpacedRepetition, Phase6_MasteryValidation) (required; pre-configured fixed phases)
- priority: Enum (Low, Medium, High)
- estimatedHours: Decimal? (optional)
- actualHours: Decimal? (optional)
- startDate: DateTime? (optional)
- targetCompletionDate: DateTime? (optional)
- completionDate: DateTime? (optional)
- progress: Decimal (0-100)
- difficulty: Enum (Beginner, Intermediate, Advanced, Expert)
- resources: List<String> (URLs, book names, etc.)
- skillsToAcquire: List<String>
- creationTime: DateTime
- lastReviewTime: DateTime?
- nextReviewTime: DateTime? (for spaced repetition)
- aiRecommendedResources: List<String>? (auto-curated)

Operations:
- List (hierarchical view, filter by category/priority/workflow stage, search by title/semantic)
- Get by ID (includes subtopics, notes, mind map, resource embeds)
- Create (AI-suggest structure from description; default to Phase1_PreLearning)
- Update (includes stage transitions with prompts for phase-specific actions, e.g., "Add notes for Phase3")
- Delete (cascade to subtopics)
- Move (change parent topic)
- Clone
- Mark Complete (auto-advance to next phase or archive if Phase6)
- Schedule Review (phase-specific: e.g., auto-set nextReviewTime for Phase4+)
- Calculate Progress (from sub-items and phase completion)
- Generate Learning Path (AI-sequence subtopics, respecting phase order)

Screens:
1. Learning Topics List Screen (tree with progress rings, category tabs; phase badges with colors)
2. Learning Topic Form Screen (skill tagger, resource linker; phase dropdown with descriptions/tooltips)
3. Learning Topic Details Screen (tabbed: overview timeline, notes feed, mind map embed, resources carousel, progress funnel chart segmented by phase)
4. Learning Dashboard (in-progress cards, review queue, completion radar; link to Today's Dashboard; phase progress overview)
5. Learning Path Visualizer (stepper graph with ETAs, phase milestones as gates)

UI/UX Enhancements: Embed YouTube/Khan Academy; daily learning nudge; phase-specific tooltips (e.g., "Phase2: Consume resources"); auto-transition suggestions based on progress.

Mock Data: Generate 40 learning topics with hierarchy (programming, design, business); include resources; distribute across phases (e.g., 30% Phase1-2, 40% Phase3-4, 30% Phase5-6).

---

### Entity: CornellNote
Properties:
- id: String (primary key)
- learningTopicId: String (required, foreign key)
- userId: String (required, foreign key)
- title: String (required, max 150)
- cues: String (required, max 2000, left column)
- notes: String (required, max 5000, right column)
- summary: String (required, max 1000, bottom section)
- source: String? (optional, max 200)
- tags: List<String>
- creationTime: DateTime
- lastModificationTime: DateTime?
- lastReviewTime: DateTime?
- flashcardsGenerated: Integer (calculated from cues)

Operations:
- List by Topic (sortable by date/review)
- Get by ID
- Create (with Cornell template pre-load)
- Update
- Delete
- Search Notes (full-text across sections)
- Export (PDF/Markdown with layout)
- Convert to Flashcards (auto-generate from cues/summary)

Screens:
1. Cornell Notes List Screen (thumbnails of layout, topic groupers)
2. Cornell Note Editor (fixed 3-section grid, rich text per area)
3. Cornell Note Viewer (print-friendly, highlight search terms)
4. Notes Search Screen (faceted by topic/tag, snippet previews)

UI/UX Enhancements: Handwriting recognition import; AI-summarize notes.

Mock Data: Generate 60 Cornell notes across learning topics; structured content.

---

### Entity: MindMap
Properties:
- id: String (primary key)
- learningTopicId: String (required, foreign key)
- userId: String (required, foreign key)
- title: String (required, max 150)
- centralIdea: String (required, max 100)
- nodes: List<MindMapNode> (required)
- layout: Enum (Radial, Tree, Organizational)
- theme: String (color scheme)
- creationTime: DateTime
- lastModificationTime: DateTime?

SubEntity: MindMapNode
- id: String
- parentNodeId: String? (nullable)
- text: String (required, max 200)
- color: String (hex code)
- icon: String? (optional)
- notes: String? (optional, max 500)
- position: Position (x, y coordinates)
- isExpanded: bool (default true)
- links: List<String> (to other nodes/resources)

Operations:
- List by Topic
- Get by ID
- Create
- Update
- Delete
- Add Node (context menu)
- Remove Node
- Move Node (drag)
- Expand/Collapse Branches
- Export (Image/PDF/SVG)

Screens:
1. Mind Map List Screen (thumbnail previews)
2. Mind Map Canvas (infinite zoom/pan, node shapes, auto-layout button)
3. Mind Map Outline View (collapsible tree toggle)

UI/UX Enhancements: AI-expand node with sub-ideas; collaboration cursors.

Mock Data: Generate 20 mind maps with 5-15 nodes each; hierarchical.

---

### Entity: ActiveRecallSession
Properties:
- id: String (primary key)
- learningTopicId: String (required, foreign key)
- userId: String (required, foreign key)
- sessionDate: DateTime (required)
- questions: List<RecallQuestion> (required)
- score: Decimal (0-100, calculated)
- duration: Integer (minutes)
- completionStatus: Enum (InProgress, Completed, Abandoned)
- difficultyAvg: Decimal? (average question difficulty)

SubEntity: RecallQuestion
- id: String
- question: String (required, max 300)
- userAnswer: String (optional, max 1000)
- correctAnswer: String (required, max 1000)
- isCorrect: bool? (optional)
- difficulty: Enum (Easy, Medium, Hard)
- responseTime: Integer (seconds)

Operations:
- List by Topic/User
- Get by ID
- Create Session (randomize order)
- Submit Answer
- Complete Session
- Review Session (highlight errors)
- Generate Questions (from notes/mindmaps)
- Get Statistics (ELO-like rating per topic)

Screens:
1. Active Recall Sessions List Screen (score badges, topic filters)
2. Recall Session Screen (full-screen flashcards, timer)
3. Session Results Screen (score breakdown pie, retry incorrect)
4. Recall Statistics Dashboard (progress curves, weak areas list)

UI/UX Enhancements: Voice answers; spaced session scheduling.

Mock Data: Generate 30 active recall sessions with 5-10 questions each; varied scores.

---

### Entity: FeynmanExplanation
Properties:
- id: String (primary key)
- learningTopicId: String (required, foreign key)
- userId: String (required, foreign key)
- concept: String (required, max 150)
- simpleExplanation: String (required, max 2000, explain like teaching a child)
- analogies: List<String> (simple analogies)
- identifiedGaps: List<String> (concepts that need more learning)
- refinedExplanation: String? (optional, max 2000, after reviewing gaps)
- reviewCount: Integer (default 0)
- masteryLevel: Enum (Attempting, Developing, Proficient, Expert)
- creationTime: DateTime
- lastReviewTime: DateTime?
- audioRecordingUrl: String? (optional, spoken explanation)

Operations:
- List by Topic
- Get by ID
- Create (template prompts)
- Update
- Delete
- Review and Refine (guided questions)
- Identify Gaps (AI-scan)
- Record Teaching Session (audio upload)
- Assess Mastery (self-quiz link)

Screens:
1. Feynman Explanations List Screen (mastery badges)
2. Feynman Editor (sectioned form, analogy suggester)
3. Explanation Review Screen (diff view before/after)
4. Gap Analysis Screen (gap cards with action links)

UI/UX Enhancements: Record/play audio inline; AI-refine explanations.

Mock Data: Generate 25 Feynman explanations across topics; include gaps.

---

### Entity: SpacedRepetition
Properties:
- id: String (primary key)
- learningTopicId: String? (optional, foreign key)
- userId: String (required, foreign key)
- contentType: Enum (Flashcard, Note, Concept, Question)
- contentId: String (ID of the content item)
- front: String (required, max 300, question/cue)
- back: String (required, max 1000, answer/explanation)
- easeFactor: Decimal (default 2.5, for SM-2 algorithm)
- interval: Integer (days until next review, default 1)
- repetitions: Integer (default 0)
- nextReviewDate: DateTime (required)
- lastReviewDate: DateTime?
- reviewHistory: List<ReviewRecord> (required)
- tags: List<String>
- isRetired: bool (default false)
- creationTime: DateTime
- imageFront: String? (URL)
- imageBack: String? (URL)

SubEntity: ReviewRecord
- reviewDate: DateTime
- quality: Integer (0-5, user's self-rating)
- responseTime: Integer (seconds)
- previousInterval: Integer (days)
- newInterval: Integer (days)

Operations:
- List Due Cards (filter by due date, topic; daily digest)
- Get by ID
- Create Card (bulk from summaries/notes)
- Update Card
- Delete Card
- Review Card (record quality, update interval; audio hints)
- Calculate Next Review Date
- Reset Card
- Retire Card
- Get Review Statistics (retention curves)
- Bulk Import Cards (Anki CSV)

Screens:
1. Spaced Repetition Dashboard (due count badge, session start button, stats overview)
2. Review Session Screen (flip animation, quality buttons 1-5, progress bar)
3. Card Editor (front/back rich text, image upload)
4. Statistics Screen (line charts for ease/retention, tag heatmaps)
5. Card Browser (grid search, bulk edit)

UI/UX Enhancements: Adaptive difficulty; cross-device sync.

Mock Data: Generate 100 spaced repetition cards with various intervals and review histories; image URLs for 20%.

---

### Entity: Summary
Properties:
- id: String (primary key)
- learningTopicId: String (required, foreign key)
- userId: String (required, foreign key)
- title: String (required, max 150)
- sourceType: Enum (Book, Article, Video, Lecture, Course, Document)
- sourceTitle: String (required, max 200)
- sourceAuthor: String? (optional, max 100)
- sourceUrl: String? (optional, URL)
- originalLength: String? (optional, e.g., "300 pages", "2 hours")
- keyPoints: List<String> (required, min 3)
- detailedSummary: String (required, max 5000)
- quotes: List<Quote> (optional)
- insights: String? (optional, max 1000, personal insights)
- actionItems: List<String> (optional, auto-link to tasks)
- relatedTopics: List<String> (topic IDs)
- tags: List<String>
- rating: Integer? (1-5, optional)
- creationTime: DateTime
- lastModificationTime: DateTime?
- aiGenerated: bool (default false)

SubEntity: Quote
- text: String (required, max 500)
- page: String? (optional, page number or timestamp)
- context: String? (optional, max 200)

Operations:
- List by Topic (sortable by rating/date)
- Get by ID
- Create (AI-assist from URL/text)
- Update
- Delete
- Search Summaries (semantic across fields)
- Export (PDF/Markdown with TOC)
- Link Related Topics
- Generate Flashcards from Key Points (one-click)

Screens:
1. Summaries List Screen (cards by source type, rating stars)
2. Summary Editor (accordion sections, key points bullet adder)
3. Summary Viewer (collapsible details, quote highlights)
4. Summary Search Screen (faceted, AI-refine query)

UI/UX Enhancements: URL import with auto-summary; highlight key phrases.

Mock Data: Generate 35 summaries across learning topics; include quotes/actions.

---

## Feature: GTD (Getting Things Done)

### Entity: QuickCapture
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- content: String (required, max 500)
- captureDateTime: DateTime (required)
- processedStatus: Enum (Unprocessed, Processed, Deleted)
- processedDateTime: DateTime? (optional)
- processedIntoType: Enum? (Task, Project, Event, Note, Reference, Someday)
- processedIntoId: String? (optional, ID of created entity)
- tags: List<String>
- attachmentIds: List<String>
- source: Enum? (Voice, Email, Text, Browser)

Operations:
- List (filter by processedStatus, date range; unprocessed first)
- Get by ID
- Create (quick add with minimal fields; voice transcription)
- Bulk Create (from email, voice note, etc.)
- Update
- Delete
- Process (AI-suggest type, one-click convert)
- Mark as Processed
- Search Captures

Screens:
1. Quick Capture Widget (persistent floating action button, modal input with source selector)
2. Capture Inbox Screen (card list, process buttons per item)
3. Capture Details Screen (edit/process/delete trio)
4. Capture History Screen (filtered timeline)

UI/UX Enhancements: Global hotkey (e.g., Cmd+Shift+C); AI-categorize on capture.

Mock Data: Generate 50 quick captures with various statuses/sources.

---

### Entity: Inbox
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- itemType: Enum (Email, Task, Note, File, Link, Message)
- title: String (required, max 200)
- description: String? (optional, max 1000)
- source: String? (optional, e.g., "Gmail", "Slack")
- sourceId: String? (optional)
- receivedDateTime: DateTime (required)
- processedStatus: Enum (Unprocessed, InProgress, Processed, Delegated, Deferred)
- processedDateTime: DateTime? (optional)
- priority: Enum (Low, Medium, High, Urgent) – AI-suggested
- actionRequired: bool (default true)
- estimatedMinutes: Integer? (optional)
- attachmentIds: List<String>
- tags: List<String>

Operations:
- List (filter by processedStatus, itemType, priority; sort by received)
- Get by ID
- Create
- Update
- Delete
- Process Item (decide action with AI guidance)
- Archive (move to reference)
- Bulk Process (select + action)
- Sync from External Sources (Gmail/Slack hooks)

Screens:
1. Inbox Screen (kanban by status, priority colors, bulk toolbar)
2. Item Processing Screen (stepper: clarify, decide, act)
3. Inbox Dashboard (funnel chart for status, processing time avg)

UI/UX Enhancements: Snooze items; voice process.

Mock Data: Generate 60 inbox items from various sources; 40% unprocessed.

---

### Entity: InboxProcessing
Properties:
- id: String (primary key)
- inboxItemId: String (required, foreign key)
- userId: String (required, foreign key)
- processingDateTime: DateTime (required)
- decision: Enum (DoIt, Delegate, Defer, Delete, Reference, Someday)
- reasoningNotes: String? (optional, max 500)
- timeSpentSeconds: Integer
- actionTaken: String? (optional, max 300)
- createdEntityType: Enum? (Task, Event, Project, Note, Reference)
- createdEntityId: String? (optional)

Operations:
- List by User/InboxItem
- Get by ID
- Create Processing Record
- Get Processing Statistics (time per decision)
- Get Decision Patterns (pie chart)

Screens:
1. Processing Decision Screen (radio buttons with examples, timer)
2. Processing History Screen (table with trends)
3. Processing Analytics (bar graphs, improvement tips)

UI/UX Enhancements: Template reasoning notes.

Mock Data: Generate processing records for inbox items; varied decisions/times.

---

### Entity: SomedayMaybe
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- title: String (required, max 150)
- description: String? (optional, max 1000)
- category: String? (optional, max 50)
- reason: String? (optional, max 300, why deferred)
- addedDate: DateTime (required)
- reviewDate: DateTime? (optional, next review date)
- lastReviewDate: DateTime? (optional)
- reviewFrequency: Enum? (Weekly, Monthly, Quarterly, Yearly)
- priority: Enum (Low, Medium, High)
- tags: List<String>
- relatedLinks: List<String> (URLs)
- convertedToType: Enum? (Task, Project, LearningTopic)
- convertedToId: String? (optional)
- status: Enum (Active, Converted, Deleted)

Operations:
- List (filter by category, reviewDate, status; search)
- Get by ID
- Create
- Update
- Delete
- Schedule Review (auto-set based on frequency)
- Convert to Action (create task/project with context)
- Bulk Review (multi-select)

Screens:
1. Someday/Maybe List Screen (tabs by category, review due highlighter)
2. Someday/Maybe Form Screen (reason dropdown templates)
3. Someday/Maybe Review Screen (due cards with convert button)
4. Conversion Dialog (map fields to new entity)

UI/UX Enhancements: Quarterly auto-review reminder; priority bump on news scan.

Mock Data: Generate 40 someday/maybe items with various categories; 20% converted.

---

### Entity: WaitingFor
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- title: String (required, max 150)
- description: String? (optional, max 1000)
- waitingFor: String (required, max 100, person/organization)
- waitingForUserId: String? (optional, if internal user)
- contextProject: String? (optional, project name)
- contextProjectId: String? (optional, foreign key)
- requestDate: DateTime (required)
- expectedDate: DateTime? (optional)
- followUpDate: DateTime? (optional)
- lastFollowUpDate: DateTime? (optional)
- status: Enum (Waiting, FollowedUp, Received, Cancelled)
- priority: Enum (Low, Medium, High)
- notes: String? (optional, max 500)
- tags: List<String>
- lastContactMethod: Enum? (Email, Call, Message)

Operations:
- List (filter by status, waitingFor, followUpDate; overdue sort)
- Get by ID
- Create
- Update
- Delete
- Mark as Received (with notes)
- Schedule Follow-up (calendar link)
- Record Follow-up (log method)
- Send Reminder (template email)

Screens:
1. Waiting For List Screen (grouped accordion by person, overdue red flags)
2. Waiting For Form Screen (waitingFor autocomplete)
3. Follow-up Dialog (method picker, script generator)
4. Waiting For Dashboard (timeline of due, received rate gauge)

UI/UX Enhancements: Auto-schedule follow-ups; integrate with email client.

Mock Data: Generate 30 waiting-for items; 50% followed up.

---

### Entity: Followup
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- relatedEntityType: Enum (Task, Project, WaitingFor, Email, Meeting)
- relatedEntityId: String (required)
- title: String (required, max 150)
- description: String? (optional, max 500)
- followUpDate: DateTime (required)
- completedDate: DateTime? (optional)
- status: Enum (Pending, Completed, Cancelled, Snoozed)
- priority: Enum (Low, Medium, High)
- reminderMinutes: List<Integer> (e.g., [60, 1440])
- notes: String? (optional, max 500)
- creationTime: DateTime
- snoozeUntil: DateTime? (optional)

Operations:
- List (filter by status, followUpDate, relatedEntityType; today/upcoming/overdue tabs)
- Get by ID
- Create (from entity context menu)
- Update
- Delete
- Mark Complete (quick note)
- Snooze (presets: 1 day, 1 week)
- Send Reminder
- Get Overdue Followups (notification batch)

Screens:
1. Follow-ups List Screen (priority icons, due calendar view)
2. Follow-up Form Screen (entity linker)
3. Follow-up Dashboard (completion funnel, overdue alert bar)

UI/UX Enhancements: Chain follow-ups; AI-predict snooze needs.

Mock Data: Generate 35 follow-ups across various entities; 60% completed.

---

### Entity: GTDHorizon
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- level: Enum (Level0_Actions, Level1_Projects, Level2_AreasOfFocus, Level3_Goals, Level4_Vision, Level5_Purpose)
- title: String (required, max 150)
- description: String (required, max 2000)
- timeframe: String? (optional, e.g., "1-2 years", "3-5 years")
- parentHorizonId: String? (nullable, link to higher level)
- childrenHorizonIds: List<String> (link to lower levels)
- relatedEntityType: Enum? (Project, Task, LearningTopic)
- relatedEntityIds: List<String>
- status: Enum (Active, Achieved, Abandoned, OnHold)
- reviewFrequency: Enum (Weekly, Monthly, Quarterly, Yearly)
- lastReviewDate: DateTime? (optional)
- nextReviewDate: DateTime? (optional)
- progress: Decimal (0-100, optional)
- tags: List<String>
- creationTime: DateTime
- lastModificationTime: DateTime?
- alignmentScore: Decimal? (0-100, auto from child progress)

Operations:
- List (filter by level, status; hierarchical expand)
- Get by ID
- Create (level-specific templates)
- Update
- Delete
- Link to Lower Level (cascade alignment recalc)
- Link to Higher Level
- Get Horizon Tree (full graph)
- Check Alignment (score gaps)
- Schedule Review

Screens:
1. GTD Horizons Screen (pyramid view by level, progress bars)
2. Horizon Level Screen (cards with child links)
3. Horizon Form Screen (timeframe picker)
4. Alignment Visualizer (tree map with color gradients)
5. Horizon Details Screen (linked entities list, score breakdown)

UI/UX Enhancements: Drag to link levels; quarterly alignment audit.

Mock Data: Generate 30 horizon items across all 6 levels; linked hierarchies.

---

## Feature: Reviews

### Entity: GTDHorizonReview
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- horizonLevel: Enum (Level0_Actions, Level1_Projects, Level2_AreasOfFocus, Level3_Goals, Level4_Vision, Level5_Purpose)
- reviewDate: DateTime (required)
- scheduledDate: DateTime (required)
- completedDate: DateTime? (optional)
- duration: Integer (minutes, optional)
- status: Enum (Scheduled, InProgress, Completed, Skipped)
- reviewedHorizonIds: List<String> (specific horizon items reviewed)
- notes: String? (optional, max 2000)
- insights: String? (optional, max 1000)
- actionItemsCreated: List<String> (task/project IDs)
- itemsArchived: List<String> (horizon item IDs)
- itemsModified: List<String> (horizon item IDs)
- alignmentScore: Integer? (1-10, self-assessed)
- nextReviewDate: DateTime? (optional)
- aiInsights: String? (auto-generated trends)

Operations:
- List (filter by horizonLevel, status, date range)
- Get by ID
- Create (schedule review)
- Start Review (timer start)
- Complete Review
- Skip Review (reason dropdown)
- Get Review History
- Get Review Statistics (completion rate, insights trends)
- Suggest Next Review Date (based on frequency)

Screens:
1. Review Schedule Screen (calendar with level colors, reschedule drag)
2. Review Session Screen (guided stepper per level, checklist)
3. Review Dashboard (history timeline, stats cards)
4. Review Details Screen (expandable notes, linked actions)
5. Review Insights Screen (word cloud for insights, pattern graphs)

UI/UX Enhancements: Voice record notes; AI-summarize past reviews.

Mock Data: Generate 25 reviews across all horizon levels with various statuses; include actions.

---

### Entity: WeeklyReview
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- weekStartDate: DateTime (required, Monday)
- weekEndDate: DateTime (required, Sunday)
- reviewDate: DateTime (required)
- completedDate: DateTime? (optional)
- duration: Integer (minutes, optional)
- status: Enum (Scheduled, InProgress, Completed, Skipped)
- checklistProgress: Map<String, bool> (checklist items completed)
- tasksCompleted: Integer (calculated)
- tasksCarriedOver: Integer (calculated)
- projectsReviewed: List<String> (project IDs)
- inboxItemsProcessed: Integer
- calendarReviewed: bool
- somedayMaybeReviewed: bool
- waitingForReviewed: bool
- wins: List<String> (achievements)
- challenges: List<String> (obstacles)
- lessonsLearned: String? (optional, max 1000)
- nextWeekPriorities: List<String> (top 3-5)
- notes: String? (optional, max 2000)
- mood: Enum? (VeryLow, Low, Neutral, High, VeryHigh)
- energyLevel: Enum? (VeryLow, Low, Neutral, High, VeryHigh)
- productivityRating: Integer? (1-10)

Operations:
- List (filter by status, date range)
- Get by ID
- Get Current Week Review
- Create (schedule review)
- Start Review
- Update Progress (save partial)
- Complete Review
- Skip Review
- Get Review Trends (completion rates, wins/challenges patterns)
- Generate Review Template (AI-populate from week data)

Screens:
1. Weekly Review Dashboard (progress wheel, quick-start button)
2. Weekly Review Session Screen (sectioned accordion: checklist, metrics, reflections)
3. Weekly Review Summary (one-page PDF export, key highlights)
4. Review Trends Screen (multi-week line charts for mood/productivity)

UI/UX Enhancements: Auto-fill from app data (e.g., tasks completed count); Sunday nudge.

Mock Data: Generate 12 weekly reviews (3 months of data); realistic wins/challenges.

---

## Feature: Today's Dashboard

### Overview
The Today's Dashboard serves as the app's productivity hub, loading on app open (or via quick-nav hotkey). It aggregates key entities from all features into a scrollable, sectioned feed optimized for quick scans and actions. Default layout: vertical stack with collapsible sections, adaptive to screen size (full-width cards on mobile, 2-3 column on desktop). Load time <1s via pre-fetching user data.

UI/UX Optimizations for Productivity:
- **At-a-Glance:** High-contrast visuals, icons/badges for status (e.g., green check for complete), 5s auto-scroll preview.
- **Quick Actions:** Inline checkboxes, + buttons for adds, swipe gestures (mobile) for defer/snooze.
- **Personalization:** AI-suggest section order based on usage (e.g., prioritize Tasks if high-volume user); dark mode auto-toggle by time.
- **Integrations:** Auto-pull from Tasks/Scheduling/GTD/Learning; weather/events as glanceable widgets.
- **Focus Mode:** One-tap to hide non-today items, Pomodoro overlay.
- **End-of-Day:** Auto-prompt reflection; export daily summary PDF.
- **Analytics Embed:** Subtle footer with daily productivity score (tasks done + habits + mood).

New/Enhanced Screens:
1. **Today's Dashboard** (core view: dynamic sections, search bar for all content, bottom nav to features).
2. **Dashboard Quick Actions Bar** (floating: +New Task/Event, Search, Settings).
3. **Daily Productivity Scorecard** (modal: score breakdown, tips).

Mock Data: Generate daily snapshots for current date (November 14, 2025); tie to existing entities (e.g., today's tasks from Task mock).

---

### Entity: DailyQuote
Properties:
- id: String (primary key)
- quote: String (required, max 500)
- author: String? (optional, max 100)
- category: String? (optional, e.g., "Motivation", "Wisdom", "Success")
- date: DateTime (required, date when quote is shown)
- userId: String? (optional, for user-specific quotes)
- isFavorite: bool (default false)
- source: String? (optional, max 150)
- moodMatch: Enum? (auto-suggested based on user history)

Operations:
- Get Quote of the Day (randomized by category, mood-personalized)
- List Quotes (filter by category/favorite)
- Get by ID
- Create
- Update
- Delete
- Mark as Favorite (collection)
- Get Random Quote
- Get Quote History (last 30 days)

Screens:
1. **Daily Dashboard Section** (full-width hero card with fade-in animation, share button, "Why this quote?" AI explain)
2. Quote Library (swipeable carousel, search)
3. Quote Settings (category preferences, daily email toggle)

UI/UX Enhancements: Voice playback; related quotes carousel.

Mock Data: Generate 100 inspirational quotes with various categories; 20% favorited.

---

### Entity: Top3Priority
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- date: DateTime (required, date for these priorities)
- priority1: String (required, max 200)
- priority1TaskId: String? (optional, linked task)
- priority1Completed: bool (default false)
- priority1Notes: String? (optional, max 100, inline sub-notes)
- priority2: String (required, max 200)
- priority2TaskId: String? (optional, linked task)
- priority2Completed: bool (default false)
- priority2Notes: String? (optional, max 100)
- priority3: String (required, max 200)
- priority3TaskId: String? (optional, linked task)
- priority3Completed: bool (default false)
- priority3Notes: String? (optional, max 100)
- notes: String? (optional, max 500)
- reflectionNotes: String? (optional, max 500, end-of-day reflection)
- creationTime: DateTime
- completionTime: DateTime? (optional, when all 3 completed)
- aiSuggested: bool (default false, from task prio scores)

Operations:
- Get Today's Priorities (with linked task previews)
- Get Priorities by Date (calendar picker)
- Create (AI-suggest from overdue tasks/waiting for)
- Update (inline edit)
- Delete
- Mark Priority Complete (cascade to task status)
- Mark All Complete
- Get Completion History (streak counter)
- Get Completion Rate (monthly avg)

Screens:
1. **Today's Dashboard Section** (stacked cards with large checkboxes, progress stepper 0/3, expand for notes; drag to reorder)
2. Top 3 Editor (modal with task linker, AI fill)
3. Priority History Screen (calendar heatmap, trend line)
4. Priority Analytics (completion radar, pattern insights e.g., "Tuesdays 80% done")

UI/UX Enhancements: Voice set priorities; auto-migrate unfinished to tomorrow.

Mock Data: Generate top 3 priorities for last 30 days with varying completion; link 70% to tasks.

---

### Entity: DailyRoutine
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- name: String (required, max 100)
- description: String? (optional, max 300)
- routineType: Enum (Morning, Evening, Midday, Custom)
- timeOfDay: Time? (optional, scheduled time)
- duration: Integer (minutes)
- items: List<RoutineItem> (required, min 1)
- isActive: bool (default true)
- daysOfWeek: List<DayOfWeek> (default all days)
- reminderEnabled: bool (default false)
- reminderMinutes: Integer? (optional, minutes before)
- color: String (hex code)
- icon: String? (optional)
- estimatedTotalTime: Integer (calculated sum)

SubEntity: RoutineItem
- id: String
- description: String (required, max 150)
- duration: Integer (minutes, optional)
- order: Integer (required)
- isCompleted: bool (default false)
- completionTime: DateTime? (optional)
- linkedEntityId: String? (optional, task/event link)

Operations:
- List by User (filter by routineType, isActive; today only)
- Get by ID
- Create (template library: "Morning Yoga Flow")
- Update
- Delete
- Toggle Active
- Get Today's Routines (filter + expand items)
- Start Routine (timer sequence)
- Complete Item (next item auto-focus)
- Complete Routine
- Get Completion Statistics (streak, avg time)

Screens:
1. **Today's Dashboard Section** (horizontal stepper with icons, time estimates, tap to start; collapse if done)
2. Routines List Screen (type tabs, edit button)
3. Routine Form Screen (item drag-drop, duration calculator)
4. Routine Session Screen (full-screen guided flow, voice cues)
5. Routine Statistics Screen (streak flame icon, monthly bar chart)

UI/UX Enhancements: Auto-pause on app switch; integrate with wearables for tracking.

Mock Data: Generate 10 routines per user (morning, evening, workout); 5-8 items each.

---

### Entity: DailyRoutineCompletion
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- dailyRoutineId: String (required, foreign key)
- date: DateTime (required)
- startTime: DateTime? (optional)
- endTime: DateTime? (optional)
- itemsCompleted: Integer (calculated)
- totalItems: Integer (calculated)
- completionPercentage: Decimal (0-100)
- status: Enum (NotStarted, InProgress, Completed, Skipped)
- notes: String? (optional, max 500)
- mood: Enum? (VeryLow, Low, Neutral, High, VeryHigh)
- timeVariance: Integer (actual vs estimated, minutes)

Operations:
- List by User/Date
- Get by ID
- Get Today's Completions
- Create (auto on item completes)
- Update
- Record Completion
- Get Completion Streak (cross-routine)
- Get Statistics (type-specific rates)

Screens:
1. **Today's Dashboard Section** (integrated into routine cards, percentage rings)
2. Routine History (calendar with completion dots)

UI/UX Enhancements: Predictive ETAs based on past variance.

Mock Data: Generate completion records for last 60 days; 75% completed.

---

### Entity: TodayTask
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- taskId: String (required, foreign key to Task entity)
- date: DateTime (required, the "today" date)
- addedReason: Enum (Scheduled, Prioritized, FromTop3, Delegation, Manual)
- plannedStartTime: Time? (optional)
- plannedDuration: Integer? (minutes, optional)
- actualStartTime: DateTime? (optional)
- actualEndTime: DateTime? (optional)
- status: Enum (Planned, InProgress, Completed, Deferred, Cancelled)
- completionNotes: String? (optional, max 300)
- pomodoroCount: Integer? (default 0, tracked sessions)

Operations:
- Get Today's Tasks (sorted by priority/start time, group by status)
- Get Tasks by Date
- Add Task to Today (bulk from My Tasks)
- Remove Task from Today
- Update Status (with time log prompt)
- Defer to Tomorrow (auto-reschedule)
- Record Time (incremental)
- Reorder Tasks (drag-drop timeline)
- Get Today Summary (completed/total, est vs actual time)

Screens:
1. **Today's Dashboard Section** (collapsible list with time blocks, priority colors, timer button per task; "Add More" from backlog)
2. Task Scheduler (gantt-mini view, drag to slots)

UI/UX Enhancements: Auto-add high-priority tasks; conflict warnings with schedule.

Mock Data: Generate today's tasks for users (5-10 tasks per user); link to Task mock, 50% in progress.

---

### Entity: TodayTimeBlock
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- timeBlockId: String (required, foreign key to TimeBlock entity)
- date: DateTime (required, today's date)
- displayOrder: Integer (order on dashboard)
- bufferMinutes: Integer? (default 5, padding)

Operations:
- Get Today's Time Blocks (timeline sorted)
- Get Time Blocks by Date
- Reorder (drag in dashboard)
- Insert Buffer (auto-add breaks)

Screens:
1. **Today's Dashboard Section** (horizontal timeline scroller, color bars, tap to edit/jump; gaps as "Free Time" suggestions)

UI/UX Enhancements: Live clock overlay; auto-adjust for overruns.

Mock Data: Links to existing time blocks for today; 6-8 per user.

---

### Entity: Habit
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- name: String (required, max 100)
- description: String? (optional, max 300)
- category: String? (optional, max 50)
- frequency: Enum (Daily, Weekly, Custom)
- frequencyDetails: String? (optional, e.g., "3 times per week")
- targetCount: Integer? (optional, for countable habits)
- targetUnit: String? (optional, e.g., "glasses", "minutes")
- reminderTime: Time? (optional)
- reminderEnabled: bool (default false)
- streakCount: Integer (default 0, current streak)
- longestStreak: Integer (default 0)
- totalCompletions: Integer (default 0)
- startDate: DateTime (required)
- isActive: bool (default true)
- color: String (hex code)
- icon: String? (optional)
- motivationNote: String? (optional, max 200)
- achievementBadges: List<Enum> (e.g., 7-day streak)

Operations:
- List by User (filter by category, isActive; today due)
- Get by ID
- Create (category templates)
- Update
- Delete
- Deactivate
- Get Today's Habits (frequency check)
- Record Completion (counter increment)
- Update Streak
- Get Statistics (yearly trends)
- Get Completion Heatmap

Screens:
1. **Today's Dashboard Section** (horizontal chip row with checkboxes/counters, streak flames)
2. Habits List Screen (category grid, + new)
3. Habit Form Screen (frequency wizard)
4. Habit Details Screen (streak graph, badge showcase)
5. Habit Tracker (monthly calendar heatmap, zoomable)

UI/UX Enhancements: Gamified badges; group similar habits into challenges.

Mock Data: Generate 15 habits per user (exercise, reading, meditation, water); varied frequencies.

---

### Entity: HabitCompletion
Properties:
- id: String (primary key)
- habitId: String (required, foreign key)
- userId: String (required, foreign key)
- date: DateTime (required)
- completed: bool (default false)
- count: Integer? (optional, for countable habits)
- completionTime: DateTime? (optional)
- notes: String? (optional, max 300)
- mood: Enum? (VeryLow, Low, Neutral, High, VeryHigh)

Operations:
- List by Habit/User/Date
- Get by ID
- Get Today's Completions
- Record Completion (multi-habit batch)
- Update Completion
- Get Completion History
- Calculate Streak
- Get Statistics (mood correlation)

Screens:
1. **Today's Dashboard Section** (integrated checkboxes)
2. Habit Details (daily log table)

UI/UX Enhancements: Photo proof for countable (e.g., water glass).

Mock Data: Generate completion records for last 90 days for all habits; 80% daily.

---

### Entity: TodayDashboardConfig
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- sections: List<DashboardSection> (required)
- theme: String (default "default", options: Light, Dark, Auto, HighContrast)
- layout: Enum (Compact, Comfortable, Spacious, Focus) – Compact hides details
- showQuote: bool (default true)
- showTop3: bool (default true)
- showRoutines: bool (default true)
- showTasks: bool (default true)
- showTimeBlocks: bool (default true)
- showHabits: bool (default true)
- showWeather: bool (default true) – new: geo-based
- showUpcomingEvents: bool (default true)
- showLearningNudge: bool (default true) – new: daily review card
- showGTDAlerts: bool (default true) – new: inbox/waiting counts
- lastModificationTime: DateTime
- aiLayoutSuggestion: String? (JSON, usage-based)

SubEntity: DashboardSection
- sectionType: Enum (Quote, Top3, Routines, Tasks, TimeBlocks, Habits, Weather, Events, LearningNudge, GTDAlerts, ProductivityScore)
- order: Integer (required)
- isVisible: bool (default true)
- isExpanded: bool (default true)
- heightPreference: Enum? (Auto, Fixed, Full) – new for layouts

Operations:
- Get User Config (with defaults fallback)
- Update Config
- Reset to Default
- Reorder Sections (drag-drop preview)
- Toggle Section Visibility (live update)
- Generate AI Layout (based on past engagement, e.g., "Task-heavy user")

Screens:
1. **Today's Dashboard** (render based on config; sections as accordions/cards, drag to reorder)
2. Dashboard Settings Screen (visual editor: section toggles, layout preview, theme switcher)

UI/UX Enhancements: Onboarding wizard for first config; A/B test layouts.

Mock Data: Generate default configs for all users; varied sections for diversity.

---

### Entity: DailyReflection
Properties:
- id: String (primary key)
- userId: String (required, foreign key)
- date: DateTime (required)
- gratitude: List<String> (3 things, optional)
- wins: List<String> (achievements, optional)
- challenges: String? (optional, max 500)
- lessonsLearned: String? (optional, max 500)
- tomorrowFocus: String? (optional, max 300) – auto-suggest to Top3
- mood: Enum (VeryLow, Low, Neutral, High, VeryHigh)
- energyLevel: Enum (VeryLow, Low, Neutral, High, VeryHigh)
- productivityRating: Integer (1-10)
- notes: String? (optional, max 1000)
- creationTime: DateTime
- aiSummary: String? (auto-generated highlights)

Operations:
- List by User (date range)
- Get by ID
- Get Today's Reflection (pre-fill from day data)
- Create
- Update
- Delete
- Get Reflection Trends (mood/productivity lines)
- Generate Insights (e.g., "Mood dips on Mondays")

Screens:
1. **Daily Reflection Modal** (evening popup: guided prompts, emoji pickers for mood, auto-save draft)
2. Reflection History Screen (calendar view with mood colors)
3. Reflection Insights Screen (trend charts, AI tips e.g., "Try evening routine for better energy")

UI/UX Enhancements: Voice journal; share anonymized trends.

Mock Data: Generate reflections for last 45 days; varied moods/ratings.

---

### New Entity: WeatherWidget (Dashboard-Only)
Properties:
- id: String (primary key) – global or user-specific
- location: String (required, e.g., "New York, NY")
- date: DateTime (required)
- temperature: Integer (C/F)
- condition: String (e.g., "Sunny")
- highTemp: Integer
- lowTemp: Integer
- icon: String (weather emoji/code)
- alert: String? (optional, e.g., "Rain expected")

Operations:
- Get Today's Weather (API fetch, cache 1h)
- Update Location

Screens:
1. **Today's Dashboard Section** (compact card: temp badge, hourly forecast scroller, outfit suggestion AI)

Mock Data: Static for November 14, 2025 (e.g., 12°C, partly cloudy, no alerts).

---

### New Entity: ProductivityScore
Properties:
- id: String (primary key)
- userId: String (required)
- date: DateTime (required)
- score: Decimal (0-100, weighted: tasks 30%, habits 20%, routines 20%, mood 15%, time utilization 15%)
- components: Map<String, Decimal> (breakdown)
- tips: List<String> (AI-generated, e.g., "Focus on high-energy blocks")
- creationTime: DateTime

Operations:
- Calculate Daily Score (on dashboard load)
- Get History
- Get Trends

Screens:
1. **Today's Dashboard Footer** (large gauge with score, component bars, tip carousel)

Mock Data: Generate scores for last 30 days; avg 75.

---