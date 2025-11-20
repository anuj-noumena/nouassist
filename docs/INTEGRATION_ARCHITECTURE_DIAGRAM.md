# Integration Architecture Diagram

## High-Level Module Relationships

```
┌─────────────────────────────────────────────────────────────────────┐
│                    USER LEARNING JOURNEY                            │
│  "I want to master C# programming with structured time & tasks"    │
└─────────────────────────────────────────────────────────────────────┘
                              │
                              ▼
        ┌─────────────────────────────────────────────────┐
        │     LearningDevelopment Module (Core)           │
        │  ┌─────────────────────────────────────────┐   │
        │  │         LearningPath                    │   │
        │  │  (Orchestrates Theory + Practice)       │   │
        │  │                                         │   │
        │  │  ┌──────────────┐  ┌───────────────┐  │   │
        │  │  │ LearningSession│  │ SkillDevelopment│ │   │
        │  │  │  (6 phases)   │  │ Session (6 steps)││   │
        │  │  │               │  │                 ││   │
        │  │  │ • PreLearning │  │ • Assessment    ││   │
        │  │  │ • Active      │  │ • Planning      ││   │
        │  │  │ • Feynman     │  │ • Practice      ││   │
        │  │  │ • Bloom       │  │ • Review        ││   │
        │  │  │ • Spaced      │  │ • Application   ││   │
        │  │  │ • Mastery     │  │ • Mastery       ││   │
        │  │  └──────────────┘  └───────────────┘  │   │
        │  └─────────────────────────────────────────┘   │
        └─────────────────────────────────────────────────┘
                    │                    │
                    │                    │
        Publishes Events        Publishes Events
                    │                    │
        ┌───────────▼────────────────────▼────────────────┐
        │                Event Bus                        │
        │  (Distributed Event System)                     │
        └──────────┬─────────────────────┬────────────────┘
                   │                     │
     ┌─────────────▼─────────────┐ ┌────▼────────────────────┐
     │  TimeManagement Module    │ │ TaskManagement Module   │
     │                           │ │                         │
     │  ┌─────────────────────┐ │ │  ┌───────────────────┐ │
     │  │    Timeblock        │ │ │  │   TaskInstance    │ │
     │  │  (Scheduled blocks)  │ │ │  │  (Actionable      │ │
     │  │                     │ │ │  │   tasks)          │ │
     │  │  LinkedEntity       │ │ │  │                   │ │
     │  │  → LearningSession  │ │ │  │  CustomFields:    │ │
     │  └─────────────────────┘ │ │  │  • LearningPathId │ │
     │                           │ │  │  • Phase          │ │
     │  ┌─────────────────────┐ │ │  └───────────────────┘ │
     │  │    TimeEntry        │ │ │                         │
     │  │  (Time tracking)     │ │ │  ┌───────────────────┐ │
     │  │                     │ │ │  │  TaskDependency   │ │
     │  │  Tags: ["Learning"] │ │ │  │  (Prerequisites)  │ │
     │  └─────────────────────┘ │ │  └───────────────────┘ │
     └───────────────────────────┘ └─────────────────────────┘
                    │                         │
                    └──────────┬──────────────┘
                               │
                     Syncs via Events
                               │
                               ▼
                ┌──────────────────────────────┐
                │  Unified Learning Activity   │
                │  (Aggregated View)           │
                │                              │
                │  • Progress: 45%             │
                │  • Time: 12h/25h             │
                │  • Tasks: 6/12 complete      │
                │  • Next: Bloom Assessment    │
                └──────────────────────────────┘
```

## Event Flow Example: Starting a Learning Session

```
USER ACTION: "Start C# Programming Learning Path"
│
├─ LearningDevelopment Module
│  └─ Creates LearningPath
│     └─ Publishes: LearningPathCreatedEto
│        ├─ learningPathId
│        ├─ topicId
│        ├─ skillId
│        └─ estimatedHours: 40
│
├─────────────────┬──────────────────┐
│                 │                  │
▼                 ▼                  ▼
TaskManagement    TimeManagement     Analytics
│                 │                  │
├─ Handler        ├─ Handler         └─ Logs metric
│  Creates:       │  Creates:
│  • 12 Tasks     │  • 12 Timeblocks
│    (6 theory +  │    (scheduled
│     6 practice) │     over weeks)
│                 │
│  Publishes:     │  Publishes:
│  TaskCreatedEto │  TimeblockCreatedEto
│                 │
└─────────────────┴──────────────────┐
                                     │
                              User Receives:
                              • Task list populated
                              • Calendar scheduled
                              • Ready to start!
```

## Data Relationships

```
┌────────────────────────────────────────────────────────────────────┐
│                      LearningPath (Core)                           │
│  ID: 123                                                           │
│  TopicId: T-456                                                    │
│  SkillId: S-789                                                    │
│  OverallProgress: 45%                                              │
└─────┬──────────────────────────────────────────┬──────────────────┘
      │                                          │
      │ References                               │ References
      │                                          │
┌─────▼──────────────────────┐      ┌───────────▼──────────────────┐
│  TaskInstance (x12)        │      │  Timeblock (x12)             │
│                            │      │                              │
│  ID: T-001                 │      │  ID: TB-001                  │
│  Title: "PreLearning"      │      │  Title: "PreLearning"        │
│  CustomFields: {           │      │  LinkedEntity: {             │
│    LearningPathId: "123"   │      │    EntityType: Learning      │
│    Phase: "PreLearning"    │      │    EntityId: LS-001          │
│  }                         │      │  }                           │
│  Status: Todo              │      │  Status: Planned             │
│  Schedule:                 │      │  StartAt: 2025-11-21 09:00   │
│    DueDate: 2025-11-21     │      │  EndAt: 2025-11-21 10:00     │
│                            │      │                              │
│  Dependencies: []          │      │  Tasks: [T-001]              │
│                            │      │                              │
└────────────────────────────┘      └──────────────────────────────┘
      │                                          │
      │ Creates when started                     │ Creates when started
      │                                          │
┌─────▼──────────────────────┐      ┌───────────▼──────────────────┐
│  TaskHistory               │      │  TimeEntry                   │
│                            │      │                              │
│  TaskInstanceId: T-001     │      │  TimeblockId: TB-001         │
│  Status: InProgress        │      │  StartedAt: 09:05            │
│  ChangedAt: 09:05          │      │  EndedAt: null (running)     │
│                            │      │  Tags: ["Learning",          │
└────────────────────────────┘      │         "PreLearning"]       │
                                    └──────────────────────────────┘
```

## Integration Points Summary

### 1. LearningDevelopment → TimeManagement

| Trigger | Action |
|---------|--------|
| LearningPath created | Auto-create 12 Timeblocks |
| LearningSession started | Start TimeEntry |
| Phase completed | Stop TimeEntry, mark Timeblock complete |
| Request schedule | Recommend optimal learning times |

### 2. LearningDevelopment → TaskManagement

| Trigger | Action |
|---------|--------|
| LearningPath created | Create 12 TaskInstances |
| Phase requirements | Add checklist items to task |
| Skill prerequisites | Create task dependencies |
| Learning milestone | Add acceptance criteria |

### 3. TaskManagement → LearningDevelopment

| Trigger | Action |
|---------|--------|
| Learning task completed | Advance LearningSession phase |
| Checklist item checked | Update learning progress |
| Task started | Notify LearningSession |

### 4. TimeManagement → LearningDevelopment

| Trigger | Action |
|---------|--------|
| Timeblock completed | Record actual time spent |
| TimeEntry stopped | Update session metrics |
| Schedule changed | Notify user of conflicts |

### 5. Three-Way Sync

| Trigger | Syncs To |
|---------|----------|
| Task completed | Learning phase advanced + Timeblock marked done |
| Timeblock completed | Task progress updated + Time tracked in session |
| Learning phase completed | Task auto-completed + Analytics updated |

## Service Dependencies

```
┌─────────────────────────────────────────────────────────────────┐
│            Application Layer (User-facing APIs)                 │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐ │
│  │     LearningIntegrationAppService                        │ │
│  │  (Orchestrates all three modules)                        │ │
│  │                                                          │ │
│  │  Methods:                                                │ │
│  │  • CreateIntegratedLearningPath(...)                     │ │
│  │  • GetLearningDashboard(userId)                          │ │
│  │  • StartLearningSession(pathId)                          │ │
│  │  • CompleteLearningPhase(sessionId, phase)               │ │
│  │  • RescheduleLearning(timeblockId, newTime)              │ │
│  └──────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                             │
           ┌─────────────────┼─────────────────┐
           │                 │                 │
           ▼                 ▼                 ▼
┌──────────────────┐ ┌──────────────┐ ┌───────────────────┐
│ Learning         │ │ Task         │ │ Time              │
│ SkillIntegration │ │ AppService   │ │ Management        │
│ Service          │ │              │ │ Service           │
└──────────────────┘ └──────────────┘ └───────────────────┘
           │                 │                 │
           │                 │                 │
           └─────────────────┴─────────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │  Event Bus      │
                    │  (Distributed)  │
                    └─────────────────┘
```

## Technology Stack

### Event Bus
- **ABP Framework Distributed Event Bus**
- RabbitMQ / Azure Service Bus for production
- In-memory for development

### Domain Events
- All events are `[Serializable]` DTOs
- Event handlers are `IDistributedEventHandler<TEvent>`
- Auto-registered via ABP's DI

### Database
- Entity Framework Core
- PostgreSQL / SQL Server
- Separate database per module (optional)

### Caching
- Redis for frequently accessed data
- In-memory cache for lookups

---

## Example User Journey

### Day 1: Setup
```
09:00 - User creates "Master C# Programming" Learning Path
        • System creates 12 tasks
        • System schedules 12 timeblocks (MWF 9-10am for 4 weeks)
        • User sees: "Ready to start! First session: Mon 9am"
```

### Day 2: First Session (Monday 9am)
```
08:45 - Notification: "Learning session in 15 minutes"
09:00 - User clicks "Start" on task "PreLearning - C#"
        • TimeEntry starts automatically
        • Timeblock status → InProgress
        • Task status → InProgress
09:45 - User completes phase, clicks "Done"
        • TimeEntry stops (45 minutes tracked)
        • Timeblock → Completed
        • Task → Completed
        • LearningSession advances to ActiveLearning phase
09:46 - System shows: "Great job! 45 min invested. Next: Wed 9am"
```

### Day 4: Second Session (Wednesday 9am)
```
Similar flow for "ActiveLearning" phase
```

### Week 4: Completion
```
After 12 sessions completed:
• LearningPath → IntegratedMastery (100%)
• Dashboard shows:
  - Total time: 40 hours
  - Mastery score: 87%
  - All tasks complete ✓
  - All timeblocks complete ✓
• Achievement unlocked: "C# Master" 🏆
```

---

## Monitoring & Analytics

### Metrics Tracked

**Learning Module:**
- Paths created/completed
- Average mastery score
- Phase completion times

**Task Module:**
- Learning tasks created
- Completion rate
- Average task duration

**Time Module:**
- Scheduled vs actual time
- Time tracking accuracy
- Productivity patterns

**Cross-Module:**
- End-to-end completion rate
- User engagement score
- Recommendation effectiveness
