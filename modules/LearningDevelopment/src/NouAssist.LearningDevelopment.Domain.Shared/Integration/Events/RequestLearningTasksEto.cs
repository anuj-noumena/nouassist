using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using NouAssist.SkillDevelopment.Enums;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event requesting automatic creation of tasks for learning path
    /// </summary>
    [Serializable]
    public class RequestLearningTasksEto
    {
        public Guid LearningPathId { get; set; }
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TheoryPhaseTask> TheoryTasks { get; set; } = new();
        public List<PracticeStepTask> PracticeTasks { get; set; } = new();
        public DateTime StartDate { get; set; }
    }

    [Serializable]
    public class TheoryPhaseTask
    {
        public LearningPhase Phase { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public DateTime DueDate { get; set; }
        public List<string> ChecklistItems { get; set; } = new();
    }

    [Serializable]
    public class PracticeStepTask
    {
        public DevelopmentStep Step { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public DateTime DueDate { get; set; }
        public List<string> ChecklistItems { get; set; } = new();
        public List<Guid> PrerequisiteTaskIds { get; set; } = new();
    }
}
