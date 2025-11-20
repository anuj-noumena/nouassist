using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.LearningDevelopment.Integration.Services
{
    /// <summary>
    /// Domain service for integrating LearningDevelopment with TimeManagement and TaskManagement
    /// </summary>
    public class LearningTaskTimeIntegrationService : DomainService
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILearningSessionRepository _learningSessionRepository;
        private readonly ISkillDevelopmentSessionRepository _skillSessionRepository;
        private readonly LearningSkillIntegrationService _learningSkillIntegrationService;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<LearningTaskTimeIntegrationService> _logger;

        public LearningTaskTimeIntegrationService(
            ILearningPathRepository learningPathRepository,
            ITopicRepository topicRepository,
            ISkillRepository skillRepository,
            ILearningSessionRepository learningSessionRepository,
            ISkillDevelopmentSessionRepository skillSessionRepository,
            LearningSkillIntegrationService learningSkillIntegrationService,
            IDistributedEventBus eventBus,
            ILogger<LearningTaskTimeIntegrationService> logger)
        {
            _learningPathRepository = learningPathRepository;
            _topicRepository = topicRepository;
            _skillRepository = skillRepository;
            _learningSessionRepository = learningSessionRepository;
            _skillSessionRepository = skillSessionRepository;
            _learningSkillIntegrationService = learningSkillIntegrationService;
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <summary>
        /// Creates an integrated learning path with automatic task and timeblock creation
        /// </summary>
        public async Task<IntegratedLearningPathResult> CreateIntegratedLearningPathAsync(
            Guid userId,
            string title,
            Guid topicId,
            Guid skillId,
            bool createTasks,
            bool scheduleTimeblocks,
            IntegrationPreferences preferences,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Creating integrated learning path for user {UserId}: {Title}",
                userId, title);

            // Create the base learning path
            var learningPath = await _learningSkillIntegrationService.CreateIntegratedLearningPathAsync(
                userId, title, topicId, skillId, tenantId);

            // Start the learning session
            var learningSession = await _learningSkillIntegrationService.StartLearningPathAsync(
                learningPath.Id);

            var result = new IntegratedLearningPathResult
            {
                LearningPath = learningPath,
                LearningSessionId = learningSession.Id
            };

            // Request task creation if enabled
            if (createTasks)
            {
                await RequestTaskCreationAsync(learningPath, topicId, skillId, preferences);
                _logger.LogInformation(
                    "Requested task creation for learning path {LearningPathId}",
                    learningPath.Id);
            }

            // Request timeblock scheduling if enabled
            if (scheduleTimeblocks)
            {
                await RequestTimeblockSchedulingAsync(learningPath, learningSession.Id, preferences);
                _logger.LogInformation(
                    "Requested timeblock scheduling for learning path {LearningPathId}",
                    learningPath.Id);
            }

            _logger.LogInformation(
                "Created integrated learning path {LearningPathId} with tasks={CreateTasks}, timeblocks={ScheduleTimeblocks}",
                learningPath.Id, createTasks, scheduleTimeblocks);

            return result;
        }

        private async Task RequestTaskCreationAsync(
            LearningPath learningPath,
            Guid topicId,
            Guid skillId,
            IntegrationPreferences preferences)
        {
            var topic = await _topicRepository.GetAsync(topicId);
            var skill = await _skillRepository.GetAsync(skillId);

            var theoryTasks = GenerateTheoryTasks(topic, learningPath, preferences);
            var practiceTasks = GeneratePracticeTasks(skill, learningPath, preferences);

            await _eventBus.PublishAsync(new RequestLearningTasksEto
            {
                LearningPathId = learningPath.Id,
                TopicId = topicId,
                SkillId = skillId,
                UserId = learningPath.UserId,
                TenantId = learningPath.TenantId,
                Title = learningPath.Title,
                Description = learningPath.Description,
                TheoryTasks = theoryTasks,
                PracticeTasks = practiceTasks,
                StartDate = preferences.StartDate ?? DateTime.Today.AddDays(1)
            });
        }

        private async Task RequestTimeblockSchedulingAsync(
            LearningPath learningPath,
            Guid learningSessionId,
            IntegrationPreferences preferences)
        {
            var phases = GenerateLearningPhases(learningPath, learningSessionId, preferences);

            await _eventBus.PublishAsync(new RequestLearningTimeblocksEto
            {
                LearningPathId = learningPath.Id,
                UserId = learningPath.UserId,
                TenantId = learningPath.TenantId,
                Title = learningPath.Title,
                EstimatedHours = preferences.EstimatedHours,
                Phases = phases,
                Schedule = new PreferredSchedule
                {
                    PreferredDays = preferences.PreferredDays ?? GetDefaultPreferredDays(),
                    PreferredStartTime = preferences.PreferredStartTime ?? new TimeSpan(9, 0, 0),
                    SessionDurationMinutes = preferences.SessionDurationMinutes ?? 60
                }
            });
        }

        private List<TheoryPhaseTask> GenerateTheoryTasks(
            Learning.Aggregates.Topic topic,
            LearningPath learningPath,
            IntegrationPreferences preferences)
        {
            var tasks = new List<TheoryPhaseTask>();
            var phases = new[]
            {
                LearningPhase.PreLearning,
                LearningPhase.ActiveLearning,
                LearningPhase.FeynmanSummary,
                LearningPhase.BloomAssessment,
                LearningPhase.SpacedRepetition,
                LearningPhase.Mastery
            };

            var startDate = preferences.StartDate ?? DateTime.Today.AddDays(1);

            for (int i = 0; i < phases.Length; i++)
            {
                var phase = phases[i];
                var dueDate = startDate.AddDays(i * 2); // 2 days per phase

                tasks.Add(new TheoryPhaseTask
                {
                    Phase = phase,
                    Title = $"{topic.Title} - {phase}",
                    Description = GetPhaseDescription(phase),
                    EstimatedMinutes = GetPhaseEstimatedMinutes(phase, topic.EstimatedHours),
                    DueDate = dueDate,
                    ChecklistItems = GetPhaseChecklistItems(phase)
                });
            }

            return tasks;
        }

        private List<PracticeStepTask> GeneratePracticeTasks(
            SkillDevelopment.Aggregates.Skill skill,
            LearningPath learningPath,
            IntegrationPreferences preferences)
        {
            var tasks = new List<PracticeStepTask>();
            var steps = new[]
            {
                DevelopmentStep.Assessment,
                DevelopmentStep.Planning,
                DevelopmentStep.Practice,
                DevelopmentStep.Review,
                DevelopmentStep.Application,
                DevelopmentStep.Mastery
            };

            var startDate = (preferences.StartDate ?? DateTime.Today.AddDays(1)).AddDays(12); // After theory

            for (int i = 0; i < steps.Length; i++)
            {
                var step = steps[i];
                var dueDate = startDate.AddDays(i * 2);

                tasks.Add(new PracticeStepTask
                {
                    Step = step,
                    Title = $"{skill.Title} - {step}",
                    Description = GetStepDescription(step),
                    EstimatedMinutes = GetStepEstimatedMinutes(step, skill.EstimatedPracticeHours),
                    DueDate = dueDate,
                    ChecklistItems = GetStepChecklistItems(step),
                    PrerequisiteTaskIds = new List<Guid>() // Would include actual prerequisites
                });
            }

            return tasks;
        }

        private List<LearningPhaseInfo> GenerateLearningPhases(
            LearningPath learningPath,
            Guid learningSessionId,
            IntegrationPreferences preferences)
        {
            var phases = new[]
            {
                LearningPhase.PreLearning,
                LearningPhase.ActiveLearning,
                LearningPhase.FeynmanSummary,
                LearningPhase.BloomAssessment,
                LearningPhase.SpacedRepetition,
                LearningPhase.Mastery
            };

            return phases.Select((phase, index) => new LearningPhaseInfo
            {
                PhaseName = phase.ToString(),
                EstimatedMinutes = preferences.SessionDurationMinutes ?? 60,
                SessionId = learningSessionId
            }).ToList();
        }

        private string GetPhaseDescription(LearningPhase phase)
        {
            return phase switch
            {
                LearningPhase.PreLearning => "Survey the material and identify key concepts",
                LearningPhase.ActiveLearning => "Engage deeply with the content through reading, watching, and note-taking",
                LearningPhase.FeynmanSummary => "Explain concepts in your own words as if teaching someone",
                LearningPhase.BloomAssessment => "Test understanding at all levels of Bloom's taxonomy",
                LearningPhase.SpacedRepetition => "Review material at spaced intervals for retention",
                LearningPhase.Mastery => "Demonstrate complete mastery through synthesis and application",
                _ => ""
            };
        }

        private string GetStepDescription(DevelopmentStep step)
        {
            return step switch
            {
                DevelopmentStep.Assessment => "Assess current skill level and identify gaps",
                DevelopmentStep.Planning => "Create a development plan with milestones",
                DevelopmentStep.Practice => "Deliberate practice with focused exercises",
                DevelopmentStep.Review => "Review progress and adjust approach",
                DevelopmentStep.Application => "Apply skill in real-world scenarios",
                DevelopmentStep.Mastery => "Demonstrate complete proficiency",
                _ => ""
            };
        }

        private int GetPhaseEstimatedMinutes(LearningPhase phase, int topicEstimatedHours)
        {
            var totalMinutes = topicEstimatedHours * 60;
            return phase switch
            {
                LearningPhase.PreLearning => (int)(totalMinutes * 0.10),
                LearningPhase.ActiveLearning => (int)(totalMinutes * 0.40),
                LearningPhase.FeynmanSummary => (int)(totalMinutes * 0.15),
                LearningPhase.BloomAssessment => (int)(totalMinutes * 0.15),
                LearningPhase.SpacedRepetition => (int)(totalMinutes * 0.10),
                LearningPhase.Mastery => (int)(totalMinutes * 0.10),
                _ => 60
            };
        }

        private int GetStepEstimatedMinutes(DevelopmentStep step, int skillEstimatedHours)
        {
            var totalMinutes = skillEstimatedHours * 60;
            return step switch
            {
                DevelopmentStep.Assessment => (int)(totalMinutes * 0.10),
                DevelopmentStep.Planning => (int)(totalMinutes * 0.10),
                DevelopmentStep.Practice => (int)(totalMinutes * 0.40),
                DevelopmentStep.Review => (int)(totalMinutes * 0.15),
                DevelopmentStep.Application => (int)(totalMinutes * 0.15),
                DevelopmentStep.Mastery => (int)(totalMinutes * 0.10),
                _ => 60
            };
        }

        private List<string> GetPhaseChecklistItems(LearningPhase phase)
        {
            return phase switch
            {
                LearningPhase.PreLearning => new List<string>
                {
                    "Survey the material",
                    "Identify key concepts",
                    "Set learning objectives"
                },
                LearningPhase.ActiveLearning => new List<string>
                {
                    "Read/watch core content",
                    "Take detailed notes",
                    "Create mindmap",
                    "Complete practice exercises"
                },
                LearningPhase.BloomAssessment => new List<string>
                {
                    "Remember: Recall key facts",
                    "Understand: Explain in own words",
                    "Apply: Solve sample problems",
                    "Analyze: Break down complex concepts",
                    "Evaluate: Critique solutions",
                    "Create: Build original project"
                },
                _ => new List<string>()
            };
        }

        private List<string> GetStepChecklistItems(DevelopmentStep step)
        {
            return step switch
            {
                DevelopmentStep.Practice => new List<string>
                {
                    "Complete 10 practice exercises",
                    "Focus on weak areas",
                    "Track practice time",
                    "Review mistakes"
                },
                DevelopmentStep.Application => new List<string>
                {
                    "Complete a real-world project",
                    "Document the process",
                    "Get feedback",
                    "Iterate based on feedback"
                },
                _ => new List<string>()
            };
        }

        private List<DayOfWeek> GetDefaultPreferredDays()
        {
            return new List<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday
            };
        }
    }

    public class IntegratedLearningPathResult
    {
        public LearningPath LearningPath { get; set; }
        public Guid LearningSessionId { get; set; }
        public List<Guid> TaskInstanceIds { get; set; } = new();
        public List<Guid> TimeblockIds { get; set; } = new();
    }

    public class IntegrationPreferences
    {
        public bool CreateTasks { get; set; } = true;
        public bool ScheduleTimeblocks { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public List<DayOfWeek> PreferredDays { get; set; }
        public TimeSpan? PreferredStartTime { get; set; }
        public int? SessionDurationMinutes { get; set; }
        public int EstimatedHours { get; set; } = 40;
    }
}
