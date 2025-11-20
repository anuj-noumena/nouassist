# Cross-Module Integration Design
## LearningDevelopment ↔ TimeManagement ↔ TaskManagement

**Version:** 1.0
**Date:** 2025-11-20
**Status:** Design Proposal

---

## Executive Summary

This document outlines integration strategies between three core modules:
- **LearningDevelopment**: Theory learning + practical skill development
- **TimeManagement**: Time blocking, scheduling, time tracking
- **TaskManagement**: Task planning, execution, GTD methodology

### Integration Goals
1. **Seamless Workflow**: Learning activities automatically become tasks and scheduled time blocks
2. **Progress Tracking**: Unified view of learning progress, time invested, and task completion
3. **Smart Scheduling**: Recommend optimal times for learning based on productivity patterns
4. **Accountability**: Track commitments and ensure learning goals are met
5. **Resource Optimization**: Allocate time efficiently across learning activities

---

## Module Overview

### LearningDevelopment Structure
```
Learning (Theory)
├── Topic (hierarchical)
├── LearningSession (6 phases)
│   ├── PreLearning
│   ├── ActiveLearning
│   ├── FeynmanSummary
│   ├── BloomAssessment
│   ├── SpacedRepetition
│   └── Mastery
└── LearningNote, Mindmap, BookReadingHistory

SkillDevelopment (Practice)
├── Skill (with prerequisites)
├── SkillDevelopmentSession (6 steps)
│   ├── Assessment
│   ├── Planning
│   ├── Practice
│   ├── Review
│   ├── Application
│   └── Mastery
└── SkillLog, ResourceHistory

Integration
└── LearningPath (orchestrates both)
```

### TimeManagement Structure
```
Calendar
├── CalendarEvent
├── Timeblock (scheduled focus work)
│   ├── Type: FocusWork, DeepWork, Learning, etc.
│   ├── Status: Planned → InProgress → Completed
│   └── LinkedEntity (can link to any entity)
└── TimeEntry (time tracking)
    └── Can link to TaskInstance

Meeting
└── Meeting with attendees, agenda
```

### TaskManagement Structure
```
Task (Template)
├── TaskType (category/type)
├── TaskInstance (actual execution)
│   ├── Status (via Workflow)
│   ├── Priority, Complexity, CognitiveLoad
│   ├── Schedule (start/due dates)
│   ├── AreaId (GTD Area of Responsibility)
│   ├── ContextId (GTD Context)
│   ├── ChecklistItems
│   ├── AcceptanceCriteria
│   └── Dependencies
└── TaskDependency
```

---

## Integration Architecture

### Pattern 1: Entity Linking (Foundation)

Use `LinkedEntity` value object already present in TimeManagement:
```csharp
public class LinkedEntity
{
    Guid EntityId
    LinkedEntityType EntityType // Add: LearningSession, SkillSession, LearningPath
}
```

**Extend LinkedEntityType enum:**
```csharp
public enum LinkedEntityType
{
    // Existing
    TaskInstance = 1,
    Project = 2,
    Meeting = 3,

    // New additions
    LearningSession = 10,
    SkillDevelopmentSession = 11,
    LearningPath = 12,
    Topic = 13,
    Skill = 14
}
```

**Benefits:**
- Timeblocks can link to learning sessions
- TimeEntries can track time for specific learning activities
- TaskInstances can represent learning milestones

---

## Integration Strategy 1: LearningDevelopment ↔ TimeManagement

### 1.1 Automatic Time Blocking for Learning Sessions

**Scenario**: User starts a LearningPath or LearningSession
**Automation**: Auto-create Timeblocks for each learning phase

**Implementation**:

```csharp
// Event Handler in TimeManagement module
public class LearningSessionStartedEventHandler :
    IDistributedEventHandler<LearningSessionStartedEto>
{
    private readonly ITimeblockRepository _timeblockRepo;
    private readonly ICalendarEventRepository _eventRepo;

    public async Task HandleEventAsync(LearningSessionStartedEto eventData)
    {
        // Suggest time blocks for next 6 phases
        var blocks = GenerateLearningTimeblocks(
            userId: eventData.UserId,
            sessionId: eventData.SessionId,
            topicTitle: eventData.TopicTitle,
            estimatedHours: eventData.EstimatedHours);

        foreach (var block in blocks)
        {
            await _timeblockRepo.InsertAsync(block);
        }
    }

    private List<Timeblock> GenerateLearningTimeblocks(...)
    {
        // Create 6 timeblocks (one per phase)
        // Use smart scheduling based on user availability
        return new List<Timeblock>
        {
            new Timeblock(
                id: GuidGenerator.Create(),
                ownerId: userId,
                startAt: DateTime.Today.AddDays(1).AddHours(9),
                endAt: DateTime.Today.AddDays(1).AddHours(10),
                blockType: TimeblockType.Learning,
                title: $"{topicTitle} - PreLearning Phase")
            {
                LinkedEntity = new LinkedEntity(sessionId, LinkedEntityType.LearningSession)
            },
            // ... repeat for 6 phases
        };
    }
}
```

**Events to Handle:**
- `LearningSessionStartedEto` → Create timeblocks
- `PhaseCompletedEto` → Mark timeblock complete, track actual time
- `SkillDevelopmentSession` started → Create practice timeblocks

### 1.2 Time Tracking for Learning Activities

**Scenario**: User works on a learning session
**Automation**: Auto-start/stop TimeEntry when phase begins/ends

**Implementation**:

```csharp
// Add to LearningSession aggregate
public class LearningSession
{
    public Guid? ActiveTimeEntryId { get; private set; }

    public void StartPhaseWithTimeTracking(LearningPhase phase)
    {
        // Publish event to start time tracking
        AddDistributedEvent(new LearningPhaseStartedEto
        {
            SessionId = Id,
            Phase = phase,
            UserId = UserId,
            RequestTimeTracking = true
        });
    }
}

// Event Handler in TimeManagement
public class LearningPhaseStartedEventHandler :
    IDistributedEventHandler<LearningPhaseStartedEto>
{
    public async Task HandleEventAsync(LearningPhaseStartedEto eventData)
    {
        if (!eventData.RequestTimeTracking) return;

        var timeEntry = new TimeEntry(
            id: GuidGenerator.Create(),
            ownerId: eventData.UserId,
            startedAt: DateTime.UtcNow,
            description: $"Learning: {eventData.Phase}")
        {
            TaskInstanceId = null, // Not a task
            Tags = new[] { "Learning", eventData.Phase.ToString() }
        };

        await _timeEntryRepo.InsertAsync(timeEntry);

        // Notify LearningSession of the TimeEntry ID
        await _eventBus.PublishAsync(new TimeEntryStartedForLearningEto
        {
            SessionId = eventData.SessionId,
            TimeEntryId = timeEntry.Id
        });
    }
}
```

### 1.3 Learning Time Analytics

**New Domain Service**: `LearningTimeAnalyticsService` in TimeManagement

```csharp
public class LearningTimeAnalyticsService : DomainService
{
    public async Task<LearningTimeReport> GetLearningTimeReportAsync(
        Guid userId,
        DateTime fromDate,
        DateTime toDate)
    {
        var timeEntries = await _timeEntryRepo.GetByUserAndDateRangeAsync(
            userId, fromDate, toDate);

        var learningEntries = timeEntries
            .Where(te => te.Tags.Contains("Learning"));

        return new LearningTimeReport
        {
            TotalMinutes = learningEntries.Sum(te => te.DurationMinutes),
            ByPhase = learningEntries
                .GroupBy(te => te.Tags.FirstOrDefault(t => Enum.TryParse<LearningPhase>(t, out _)))
                .ToDictionary(g => g.Key, g => g.Sum(te => te.DurationMinutes)),
            AverageSessionLength = learningEntries.Average(te => te.DurationMinutes),
            MostProductiveTimeOfDay = CalculateMostProductiveHour(learningEntries)
        };
    }
}
```

### 1.4 Smart Learning Schedule Recommendations

**New Service**: `LearningScheduleOptimizerService`

```csharp
public class LearningScheduleOptimizerService : DomainService
{
    public async Task<List<TimeSlot>> RecommendLearningTimesAsync(
        Guid userId,
        int durationMinutes,
        DayOfWeek[] preferredDays = null)
    {
        // Analyze historical time tracking data
        var historicalData = await _timeEntryRepo.GetLearningHistoryAsync(userId);

        // Find patterns: when does user learn most effectively?
        var productivityPatterns = AnalyzeProductivityPatterns(historicalData);

        // Check calendar availability
        var availability = await _availabilityService.GetAvailableSlotsAsync(
            userId, DateTime.Today, DateTime.Today.AddDays(30));

        // Match availability with high-productivity times
        return OptimizeSchedule(availability, productivityPatterns, durationMinutes);
    }
}
```

---

## Integration Strategy 2: LearningDevelopment ↔ TaskManagement

### 2.1 Learning Phases as Tasks

**Scenario**: LearningPath phases become actionable tasks in task list
**Benefits**: Users see learning activities in their daily task list

**Implementation**:

```csharp
// Event Handler in TaskManagement
public class LearningPathCreatedEventHandler :
    IDistributedEventHandler<LearningPathCreatedEto>
{
    private readonly ITaskRepository _taskRepo;
    private readonly ITaskInstanceRepository _taskInstanceRepo;

    public async Task HandleEventAsync(LearningPathCreatedEto eventData)
    {
        // Create a Task template for this learning path
        var learningTask = new Task(
            id: GuidGenerator.Create(),
            userId: eventData.UserId,
            title: $"Complete: {eventData.Title}",
            tenantId: eventData.TenantId);

        learningTask.UpdateDescription($"Master both theory and practice for {eventData.Title}");
        learningTask.SetCategory("Learning");

        await _taskRepo.InsertAsync(learningTask, autoSave: false);

        // Create 6 TaskInstances for theory phases
        var theoryInstances = await CreatePhaseTaskInstances(
            eventData, learningTask.Id, "Theory");

        // Create 6 TaskInstances for practice steps
        var practiceInstances = await CreatePracticeTaskInstances(
            eventData, learningTask.Id, "Practice");

        // Set up dependencies: Theory tasks must complete before practice tasks
        SetupTaskDependencies(theoryInstances, practiceInstances);
    }

    private async Task<List<TaskInstance>> CreatePhaseTaskInstances(
        LearningPathCreatedEto eventData,
        Guid taskId,
        string category)
    {
        var phases = new[] {
            "PreLearning", "ActiveLearning", "FeynmanSummary",
            "BloomAssessment", "SpacedRepetition", "Mastery"
        };

        var instances = new List<TaskInstance>();

        for (int i = 0; i < phases.Length; i++)
        {
            var instance = new TaskInstance(
                id: GuidGenerator.Create(),
                taskId: taskId,
                workflowId: defaultWorkflowId,
                workflowStatusId: todoStatusId,
                ownerId: eventData.UserId,
                projectId: null,
                areaId: null, // Could map to "Personal Development" area
                tenantId: eventData.TenantId);

            instance.SetTitle($"{category}: {phases[i]}");
            instance.SetSchedule(new TaskSchedule
            {
                StartDate = DateTime.Today.AddDays(i),
                DueDate = DateTime.Today.AddDays(i + 1)
            });
            instance.SetPriority(ImportanceUrgencyMatrix.ImportantNotUrgent);
            instance.SetCognitiveLoad(CognitiveLoad.Medium);

            // Store reference to LearningPath
            instance.AddCustomField("LearningPathId", eventData.LearningPathId.ToString());
            instance.AddCustomField("Phase", phases[i]);

            instances.Add(instance);
            await _taskInstanceRepo.InsertAsync(instance, autoSave: false);
        }

        return instances;
    }
}
```

### 2.2 Task Completion → Learning Progress Sync

**Scenario**: User completes a learning task → Sync with LearningSession

**Implementation**:

```csharp
// Event Handler in LearningDevelopment
public class TaskCompletedEventHandler :
    IDistributedEventHandler<TaskCompletedEto>
{
    public async Task HandleEventAsync(TaskCompletedEto eventData)
    {
        // Check if this task is a learning task
        var customFields = await _taskInstanceRepo.GetCustomFieldsAsync(eventData.TaskInstanceId);
        if (!customFields.ContainsKey("LearningPathId")) return;

        var learningPathId = Guid.Parse(customFields["LearningPathId"]);
        var phase = customFields["Phase"];

        var learningPath = await _learningPathRepo.GetAsync(learningPathId);

        // Update learning path progress based on task completion
        if (IsTheoryPhase(phase))
        {
            await CompleteTheoryPhase(learningPath, phase);
        }
        else
        {
            await CompletePracticeStep(learningPath, phase);
        }
    }
}
```

### 2.3 Skill Prerequisites → Task Dependencies

**Scenario**: Skill prerequisites become task dependencies
**Benefits**: Visual dependency graph, prevents starting skills before prerequisites met

**Implementation**:

```csharp
public class SkillToTaskDependencyMapper : DomainService
{
    public async Task MapSkillPrerequisitesToTaskDependenciesAsync(Guid skillId)
    {
        var skill = await _skillRepo.GetAsync(skillId);
        var skillTaskInstance = await _taskInstanceRepo.FindByCustomFieldAsync(
            "SkillId", skillId.ToString());

        if (skillTaskInstance == null) return;

        foreach (var prerequisiteSkillId in skill.Prerequisites)
        {
            var prerequisiteTask = await _taskInstanceRepo.FindByCustomFieldAsync(
                "SkillId", prerequisiteSkillId.ToString());

            if (prerequisiteTask != null)
            {
                skillTaskInstance.AddDependency(new TaskDependency(
                    dependentTaskId: skillTaskInstance.Id,
                    dependsOnTaskId: prerequisiteTask.Id,
                    dependencyType: DependencyType.FinishToStart));
            }
        }

        await _taskInstanceRepo.UpdateAsync(skillTaskInstance);
    }
}
```

### 2.4 Learning Checklist Items

**Scenario**: LearningSession milestones become task checklist items

**Implementation**:

```csharp
// When creating learning task, add checklist items
public void AddLearningChecklistItems(TaskInstance taskInstance, LearningPhase phase)
{
    switch (phase)
    {
        case LearningPhase.PreLearning:
            taskInstance.AddChecklistItem("Survey the material");
            taskInstance.AddChecklistItem("Identify key concepts");
            taskInstance.AddChecklistItem("Set learning objectives");
            break;

        case LearningPhase.ActiveLearning:
            taskInstance.AddChecklistItem("Read/watch core content");
            taskInstance.AddChecklistItem("Take notes in Mindmap");
            taskInstance.AddChecklistItem("Practice exercises");
            break;

        case LearningPhase.BloomAssessment:
            taskInstance.AddChecklistItem("Remember: Recall key facts");
            taskInstance.AddChecklistItem("Understand: Explain in own words");
            taskInstance.AddChecklistItem("Apply: Solve sample problems");
            taskInstance.AddChecklistItem("Analyze: Break down complex concepts");
            taskInstance.AddChecklistItem("Evaluate: Critique solutions");
            taskInstance.AddChecklistItem("Create: Build original project");
            break;

        // ... other phases
    }
}
```

---

## Integration Strategy 3: Three-Way Integration

### 3.1 Complete Learning Workflow

**The Vision**: From learning goal → scheduled time → task completion

```
1. User creates LearningPath ("Master C# Programming")
   ↓
2. LearningPath creates:
   - 12 TaskInstances (6 theory + 6 practice)
   - 12 Timeblocks (scheduled throughout the month)
   - Links tasks ↔ timeblocks via LinkedEntity
   ↓
3. User receives daily notification:
   "Time for: PreLearning - C# Basics (9:00 AM - 10:00 AM)"
   ↓
4. User clicks notification → opens task → starts timeblock
   ↓
5. TimeEntry auto-starts tracking time
   ↓
6. User completes phase → marks task complete
   ↓
7. System syncs:
   - Task marked complete
   - Timeblock marked complete
   - TimeEntry stopped
   - LearningSession phase advanced
   - Next task/timeblock highlighted
   ↓
8. After all 12 tasks complete:
   → LearningPath reaches IntegratedMastery
   → User receives achievement notification
   → Analytics show total time invested
```

### 3.2 Unified Dashboard Data Model

**New Aggregate**: `LearningActivity` (in Integration layer)

```csharp
public class LearningActivity : FullAuditedAggregateRoot<Guid>
{
    // References to all related entities
    public Guid UserId { get; private set; }
    public Guid LearningPathId { get; private set; }
    public Guid? LearningSessionId { get; private set; }
    public Guid? SkillDevelopmentSessionId { get; private set; }

    // TaskManagement references
    public List<Guid> TaskInstanceIds { get; private set; }

    // TimeManagement references
    public List<Guid> TimeblockIds { get; private set; }
    public List<Guid> TimeEntryIds { get; private set; }

    // Aggregated metrics
    public int TheoryTasksCompleted { get; private set; }
    public int PracticeTasksCompleted { get; private set; }
    public int TotalTasksCreated { get; private set; }

    public int ScheduledMinutes { get; private set; }
    public int ActualMinutesSpent { get; private set; }
    public int OverallProgress { get; private set; }

    // Methods
    public void SyncFromTask(TaskCompletedEto taskEvent) { }
    public void SyncFromTimeblock(TimeblockCompletedEto timeEvent) { }
    public void SyncFromLearningPath(LearningPath learningPath) { }

    public LearningActivitySnapshot GetSnapshot()
    {
        return new LearningActivitySnapshot
        {
            Progress = OverallProgress,
            TimeInvested = ActualMinutesSpent,
            TimeRemaining = ScheduledMinutes - ActualMinutesSpent,
            CompletionRate = (double)TasksCompleted / TotalTasksCreated,
            OnTrack = ActualMinutesSpent <= ScheduledMinutes * 1.2 // 20% buffer
        };
    }
}
```

### 3.3 Smart Notifications & Reminders

**New Service**: `LearningReminderService`

```csharp
public class LearningReminderService : DomainService
{
    public async Task SendLearningRemindersAsync()
    {
        var upcomingTimeblocks = await _timeblockRepo.GetUpcomingLearningBlocksAsync(
            withinMinutes: 15);

        foreach (var timeblock in upcomingTimeblocks)
        {
            var linkedEntity = timeblock.LinkedEntity;
            if (linkedEntity?.EntityType != LinkedEntityType.LearningSession) continue;

            var learningPath = await _learningPathRepo.FindBySessionIdAsync(
                linkedEntity.EntityId);

            var taskInstance = await _taskInstanceRepo.FindByCustomFieldAsync(
                "LearningPathId", learningPath.Id.ToString());

            // Send notification with all context
            await _notificationService.SendAsync(new Notification
            {
                UserId = timeblock.OwnerId,
                Title = $"Learning Time: {timeblock.Title}",
                Message = $"Your learning session starts in 15 minutes. Task: {taskInstance?.Title}",
                Actions = new[]
                {
                    new NotificationAction("Start Now", $"/learning/{learningPath.Id}/start"),
                    new NotificationAction("View Task", $"/tasks/{taskInstance?.Id}"),
                    new NotificationAction("Reschedule", $"/calendar/reschedule/{timeblock.Id}")
                }
            });
        }
    }
}
```

### 3.4 Learning Analytics Dashboard

**Query Service**: `LearningAnalyticsDashboardService`

```csharp
public class LearningAnalyticsDashboardService : IApplicationService
{
    public async Task<LearningDashboardDto> GetDashboardAsync(Guid userId)
    {
        // Aggregate data from all three modules
        var learningPaths = await _learningPathRepo.GetByUserIdAsync(userId);
        var learningTasks = await _taskInstanceRepo.GetLearningTasksAsync(userId);
        var learningTimeblocks = await _timeblockRepo.GetLearningBlocksAsync(userId);
        var learningTimeEntries = await _timeEntryRepo.GetLearningEntriesAsync(userId);

        return new LearningDashboardDto
        {
            // Learning Progress
            ActiveLearningPaths = learningPaths.Count(lp => lp.Status == LearningPathStatus.Active),
            CompletedLearningPaths = learningPaths.Count(lp => lp.Status == LearningPathStatus.Completed),
            AverageMasteryScore = learningPaths.Average(lp => lp.IntegratedMasteryScore),

            // Task Metrics
            TasksCompleted = learningTasks.Count(t => t.IsCompleted),
            TasksInProgress = learningTasks.Count(t => t.IsInProgress),
            TasksOverdue = learningTasks.Count(t => t.IsOverdue),

            // Time Metrics
            TotalHoursInvested = learningTimeEntries.Sum(te => te.DurationMinutes) / 60.0,
            ScheduledHoursThisWeek = GetScheduledHoursThisWeek(learningTimeblocks),
            ActualVsPlannedTime = CalculateTimeAccuracy(learningTimeblocks, learningTimeEntries),

            // Productivity Insights
            MostProductiveTimeOfDay = GetMostProductiveTime(learningTimeEntries),
            AverageSessionLength = learningTimeEntries.Average(te => te.DurationMinutes),
            LongestStreak = CalculateLearningStreak(learningTimeEntries),

            // Recommendations
            RecommendedNextAction = GenerateRecommendation(learningPaths, learningTasks, learningTimeblocks)
        };
    }
}
```

---

## Event Catalog

### Events Published by LearningDevelopment

| Event | When | Handled By |
|-------|------|------------|
| `LearningPathCreatedEto` | LearningPath created | TaskManagement, TimeManagement |
| `LearningSessionStartedEto` | Learning begins | TimeManagement (create timeblocks) |
| `PhaseCompletedEto` | Phase done | TaskManagement (mark task complete) |
| `TopicMasteredEto` | Topic mastered | TaskManagement (complete milestone) |
| `SkillMasteredEto` | Skill mastered | TaskManagement (achievement) |
| `IntegratedMasteryAchievedEto` | Both theory+practice done | TaskManagement, Analytics |

### Events Subscribed by LearningDevelopment

| Event | Source | Action |
|-------|--------|--------|
| `TaskCompletedEto` | TaskManagement | Advance learning phase |
| `TimeblockCompletedEto` | TimeManagement | Record actual time spent |
| `TimeEntryCreatedEto` | TimeManagement | Link to session |

---

## Database Schema Changes

### LearningPath Table (Already exists)
```sql
-- Add columns for integration
ALTER TABLE LearningPaths ADD COLUMN TaskInstanceId UUID NULL;
ALTER TABLE LearningPaths ADD COLUMN TimeblockId UUID NULL;
```

### TaskInstance Table (Extend custom fields)
```json
CustomFields: {
  "LearningPathId": "guid",
  "LearningPhase": "PreLearning",
  "SkillId": "guid",
  "DevelopmentStep": "Assessment"
}
```

### Timeblock Table (Already has LinkedEntity)
```sql
-- LinkedEntity will store:
-- EntityType = LinkedEntityType.LearningSession (10)
-- EntityId = LearningSessionId or SkillDevelopmentSessionId
```

### New Table: LearningActivity (Optional)
```sql
CREATE TABLE LearningActivities (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL,
    LearningPathId UUID NOT NULL,
    TaskInstanceIds JSONB, -- Array of task IDs
    TimeblockIds JSONB,    -- Array of timeblock IDs
    TimeEntryIds JSONB,    -- Array of time entry IDs
    TheoryTasksCompleted INT DEFAULT 0,
    PracticeTasksCompleted INT DEFAULT 0,
    TotalTasksCreated INT DEFAULT 0,
    ScheduledMinutes INT DEFAULT 0,
    ActualMinutesSpent INT DEFAULT 0,
    OverallProgress INT DEFAULT 0,
    CreationTime TIMESTAMP NOT NULL,
    LastModificationTime TIMESTAMP
);
```

---

## Implementation Phases

### Phase 1: Foundation (Week 1-2)
1. Extend `LinkedEntityType` enum
2. Create integration events in each module
3. Set up event handlers for basic linking

### Phase 2: TimeManagement Integration (Week 3-4)
1. Auto-create timeblocks from learning sessions
2. Implement time tracking for learning
3. Build time analytics service

### Phase 3: TaskManagement Integration (Week 5-6)
1. Create tasks from learning phases
2. Sync task completion with learning progress
3. Map skill prerequisites to task dependencies

### Phase 4: Three-Way Integration (Week 7-8)
1. Build `LearningActivity` aggregate
2. Implement smart notifications
3. Create unified dashboard

### Phase 5: Advanced Features (Week 9-10)
1. Learning schedule optimizer
2. Productivity pattern analysis
3. Recommendation engine

---

## API Examples

### Create Integrated Learning Path

```http
POST /api/learning-integration/learning-paths
Content-Type: application/json

{
  "title": "Master C# Programming",
  "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "skillId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "createTasks": true,
  "scheduleTimeblocks": true,
  "preferredTimes": [
    { "dayOfWeek": "Monday", "startTime": "09:00", "durationMinutes": 60 },
    { "dayOfWeek": "Wednesday", "startTime": "09:00", "durationMinutes": 60 },
    { "dayOfWeek": "Friday", "startTime": "09:00", "durationMinutes": 60 }
  ]
}
```

**Response:**
```json
{
  "learningPathId": "guid",
  "taskInstanceIds": ["guid", "guid", ...],
  "timeblockIds": ["guid", "guid", ...],
  "estimatedCompletionDate": "2025-12-20",
  "totalEstimatedHours": 40
}
```

### Get Learning Dashboard

```http
GET /api/learning-integration/dashboard
```

**Response:**
```json
{
  "activeLearningPaths": 3,
  "completedLearningPaths": 7,
  "averageMasteryScore": 85,
  "tasksCompleted": 42,
  "tasksInProgress": 6,
  "tasksOverdue": 1,
  "totalHoursInvested": 120.5,
  "scheduledHoursThisWeek": 5,
  "mostProductiveTimeOfDay": "09:00-11:00",
  "averageSessionLength": 45,
  "currentStreak": 12,
  "recommendedNextAction": {
    "type": "StartLearningSession",
    "learningPathId": "guid",
    "phase": "BloomAssessment",
    "taskInstanceId": "guid",
    "timeblockId": "guid",
    "scheduledAt": "2025-11-21T09:00:00Z"
  }
}
```

---

## Key Benefits

### For Users
✅ **Unified Experience**: See learning, tasks, and time in one place
✅ **Automatic Scheduling**: No manual planning needed
✅ **Progress Visibility**: Track learning journey holistically
✅ **Accountability**: Tasks and calendar keep you on track
✅ **Smart Insights**: Learn when you're most productive

### For System
✅ **Event-Driven**: Scalable, maintainable architecture
✅ **Loose Coupling**: Modules remain independent
✅ **Extensible**: Easy to add new integrations
✅ **Data-Rich**: Analytics across all dimensions

---

## Conclusion

This integration transforms three powerful modules into a cohesive learning management system. Users benefit from seamless workflows, automatic planning, and comprehensive tracking, while the system maintains architectural integrity through event-driven patterns and clear boundaries.

**Next Steps**: Review this design, prioritize features, and begin implementation in phases.
