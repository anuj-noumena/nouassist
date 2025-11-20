using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.LearningDevelopment.Integration.LearningIntegration.Dtos;
using NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos;
using NouAssist.LearningDevelopment.Integration.Services;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace NouAssist.LearningDevelopment.Integration.LearningIntegration
{
    [Authorize]
    public class LearningIntegrationAppService : ApplicationService, ILearningIntegrationAppService
    {
        private readonly LearningTaskTimeIntegrationService _integrationService;
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILearningActivityRepository _activityRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ICurrentUser _currentUser;

        public LearningIntegrationAppService(
            LearningTaskTimeIntegrationService integrationService,
            ILearningPathRepository learningPathRepository,
            ILearningActivityRepository activityRepository,
            ITopicRepository topicRepository,
            ISkillRepository skillRepository,
            IDistributedEventBus eventBus,
            ICurrentUser currentUser)
        {
            _integrationService = integrationService;
            _learningPathRepository = learningPathRepository;
            _activityRepository = activityRepository;
            _topicRepository = topicRepository;
            _skillRepository = skillRepository;
            _eventBus = eventBus;
            _currentUser = currentUser;
        }

        public async Task<LearningPathDto> CreateIntegratedLearningPathAsync(CreateIntegratedLearningPathDto input)
        {
            var userId = _currentUser.GetId();

            var preferences = new IntegrationPreferences
            {
                CreateTasks = input.CreateTasks,
                ScheduleTimeblocks = input.ScheduleTimeblocks,
                StartDate = input.StartDate,
                PreferredDays = input.PreferredDays,
                PreferredStartTime = input.PreferredStartTime,
                SessionDurationMinutes = input.SessionDurationMinutes,
                EstimatedHours = input.EstimatedHours
            };

            var result = await _integrationService.CreateIntegratedLearningPathAsync(
                userId,
                input.Title,
                input.TopicId,
                input.SkillId,
                input.CreateTasks,
                input.ScheduleTimeblocks,
                preferences,
                _currentUser.TenantId);

            // Create LearningActivity aggregate for tracking
            var activity = new Aggregates.LearningActivity(
                Guid.NewGuid(),
                userId,
                result.LearningPath.Id,
                _currentUser.TenantId);

            await _activityRepository.InsertAsync(activity, autoSave: true);

            var topic = await _topicRepository.GetAsync(input.TopicId);
            var skill = await _skillRepository.GetAsync(input.SkillId);

            return new LearningPathDto
            {
                Id = result.LearningPath.Id,
                UserId = userId,
                Title = result.LearningPath.Title,
                Description = result.LearningPath.Description,
                Status = result.LearningPath.Status,
                CurrentPhase = result.LearningPath.CurrentPhase,
                TopicId = input.TopicId,
                TopicTitle = topic.Title,
                SkillId = input.SkillId,
                SkillTitle = skill.Title,
                OverallProgressPercentage = result.LearningPath.OverallProgressPercentage,
                CreationTime = result.LearningPath.CreationTime
            };
        }

        public async Task<LearningDashboardDto> GetDashboardAsync()
        {
            var userId = _currentUser.GetId();

            var learningPaths = await _learningPathRepository.GetByUserIdAsync(userId);
            var activities = await _activityRepository.GetByUserIdAsync(userId);

            // Calculate metrics
            var activePaths = learningPaths.Count(lp => lp.Status == Enums.LearningPathStatus.Active);
            var completedPaths = learningPaths.Count(lp => lp.Status == Enums.LearningPathStatus.Completed);
            var avgMasteryScore = learningPaths.Any()
                ? learningPaths.Average(lp => (double)lp.IntegratedMasteryScore)
                : 0;

            var totalTasksCompleted = activities.Sum(a => a.TotalTasksCompleted);
            var totalTasksCreated = activities.Sum(a => a.TotalTasksCreated);
            var taskCompletionRate = totalTasksCreated > 0
                ? (double)totalTasksCompleted / totalTasksCreated * 100
                : 0;

            var totalHours = activities.Sum(a => a.ActualMinutesSpent) / 60.0;

            return new LearningDashboardDto
            {
                ActiveLearningPaths = activePaths,
                CompletedLearningPaths = completedPaths,
                AverageMasteryScore = avgMasteryScore,
                TasksCompleted = totalTasksCompleted,
                TasksInProgress = activities.Count(a => a.OverallProgress > 0 && a.OverallProgress < 100),
                TasksOverdue = 0, // Would need to check task due dates
                TaskCompletionRate = taskCompletionRate,
                TotalHoursInvested = totalHours,
                ScheduledHoursThisWeek = activities.Sum(a => a.ScheduledMinutes) / 60,
                ActualVsPlannedTimePercentage = activities.Any()
                    ? activities.Average(a => a.TimeEfficiency)
                    : 0,
                MostProductiveTimeOfDay = "09:00-11:00", // Would calculate from time entries
                AverageSessionLengthMinutes = activities.Any()
                    ? (int)(activities.Average(a => a.ActualMinutesSpent) / Math.Max(1, activities.Average(a => (double)a.TimeEntryIds.Count)))
                    : 0,
                CurrentStreak = CalculateStreak(activities),
                RecommendedNextAction = GenerateRecommendation(learningPaths, activities)
            };
        }

        public async Task<LearningActivitySnapshotDto> GetLearningActivityAsync(Guid learningPathId)
        {
            var activity = await _activityRepository.FindByLearningPathIdAsync(learningPathId);
            if (activity == null)
            {
                return null;
            }

            var snapshot = activity.GetSnapshot();

            return new LearningActivitySnapshotDto
            {
                LearningActivityId = snapshot.LearningActivityId,
                LearningPathId = snapshot.LearningPathId,
                UserId = snapshot.UserId,
                OverallProgress = snapshot.OverallProgress,
                TaskCompletionRate = snapshot.TaskCompletionRate,
                TotalTasksCreated = snapshot.TotalTasksCreated,
                TotalTasksCompleted = snapshot.TotalTasksCompleted,
                TheoryTasksCompleted = snapshot.TheoryTasksCompleted,
                PracticeTasksCompleted = snapshot.PracticeTasksCompleted,
                ScheduledMinutes = snapshot.ScheduledMinutes,
                ActualMinutesSpent = snapshot.ActualMinutesSpent,
                RemainingMinutes = snapshot.RemainingMinutes,
                TimeEfficiency = snapshot.TimeEfficiency,
                IsOnTrack = snapshot.IsOnTrack,
                TimeblockCount = snapshot.TimeblockCount,
                TimeEntryCount = snapshot.TimeEntryCount,
                LastTaskCompletedAt = snapshot.LastTaskCompletedAt,
                LastTimeTrackedAt = snapshot.LastTimeTrackedAt,
                LastSyncAt = snapshot.LastSyncAt
            };
        }

        public async Task StartLearningPhaseAsync(Guid sessionId, string phase)
        {
            var learningPhase = Enum.Parse<LearningPhase>(phase);

            await _eventBus.PublishAsync(new LearningPhaseStartedEto
            {
                SessionId = sessionId,
                TopicId = Guid.Empty, // Would get from session
                UserId = _currentUser.GetId(),
                TenantId = _currentUser.TenantId,
                Phase = learningPhase,
                TopicTitle = "Learning", // Would get from session
                RequestTimeTracking = true,
                StartedAt = DateTime.UtcNow
            });
        }

        public async Task StopLearningPhaseAsync(Guid sessionId, string phase, int actualMinutesSpent)
        {
            var learningPhase = Enum.Parse<LearningPhase>(phase);

            await _eventBus.PublishAsync(new LearningPhaseStoppedEto
            {
                SessionId = sessionId,
                TopicId = Guid.Empty, // Would get from session
                UserId = _currentUser.GetId(),
                TenantId = _currentUser.TenantId,
                Phase = learningPhase,
                TimeEntryId = null, // Would get from session
                ActualMinutesSpent = actualMinutesSpent,
                StoppedAt = DateTime.UtcNow
            });
        }

        private int CalculateStreak(System.Collections.Generic.List<Aggregates.LearningActivity> activities)
        {
            // Simple implementation - would calculate consecutive days with activity
            var daysWithActivity = activities
                .Where(a => a.LastTimeTrackedAt.HasValue)
                .Select(a => a.LastTimeTrackedAt.Value.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            int streak = 0;
            var today = DateTime.Today;

            foreach (var date in daysWithActivity)
            {
                if (date == today.AddDays(-streak))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        private RecommendedAction GenerateRecommendation(
            System.Collections.Generic.List<Aggregates.LearningPath> learningPaths,
            System.Collections.Generic.List<Aggregates.LearningActivity> activities)
        {
            var activePath = learningPaths.FirstOrDefault(lp => lp.Status == Enums.LearningPathStatus.Active);
            if (activePath == null)
            {
                return new RecommendedAction
                {
                    Type = "CreateLearningPath",
                    Message = "Start a new learning journey!"
                };
            }

            return new RecommendedAction
            {
                Type = "ContinueLearning",
                LearningPathId = activePath.Id,
                Phase = activePath.CurrentLearningPhase?.ToString() ?? activePath.CurrentDevelopmentStep?.ToString(),
                Message = $"Continue with {activePath.Title}",
                ScheduledAt = DateTime.Today.AddHours(9) // Would get from next scheduled timeblock
            };
        }
    }
}
