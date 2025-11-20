using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.TimeManagement.Calendar.Events;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles LearningTimeblocksCreatedEto to update LearningPath with timeblock IDs
    /// </summary>
    public class LearningTimeblocksCreatedHandler :
        IDistributedEventHandler<LearningTimeblocksCreatedEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILogger<LearningTimeblocksCreatedHandler> _logger;

        public LearningTimeblocksCreatedHandler(
            ILearningPathRepository learningPathRepository,
            ILogger<LearningTimeblocksCreatedHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(LearningTimeblocksCreatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Received {Count} created timeblocks for learning path {LearningPathId}",
                    eventData.TimeblockIds.Count, eventData.LearningPathId);

                // Store timeblock IDs in LearningPath or LearningActivity aggregate
                // This allows us to track and manage all related timeblocks

                // In real implementation:
                // var learningPath = await _learningPathRepository.GetAsync(eventData.LearningPathId);
                // learningPath.AssociateTimeblocks(eventData.TimeblockIds);
                // await _learningPathRepository.UpdateAsync(learningPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error associating timeblocks with learning path {LearningPathId}",
                    eventData.LearningPathId);
                throw;
            }
        }
    }

    /// <summary>
    /// Handles LearningTasksCreatedEto to update LearningPath with task IDs
    /// </summary>
    public class LearningTasksCreatedHandler :
        IDistributedEventHandler<LearningTasksCreatedEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILogger<LearningTasksCreatedHandler> _logger;

        public LearningTasksCreatedHandler(
            ILearningPathRepository learningPathRepository,
            ILogger<LearningTasksCreatedHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(LearningTasksCreatedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Received {Count} created tasks for learning path {LearningPathId}",
                    eventData.TaskInstanceIds.Count, eventData.LearningPathId);

                // Store task IDs in LearningPath or LearningActivity aggregate
                // This allows us to track and manage all related tasks

                // In real implementation:
                // var learningPath = await _learningPathRepository.GetAsync(eventData.LearningPathId);
                // learningPath.AssociateTasks(eventData.TaskInstanceIds);
                // await _learningPathRepository.UpdateAsync(learningPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error associating tasks with learning path {LearningPathId}",
                    eventData.LearningPathId);
                throw;
            }
        }
    }

    /// <summary>
    /// Handles TimeEntryStartedForLearningEto to track time entry ID in learning session
    /// </summary>
    public class TimeEntryStartedHandler :
        IDistributedEventHandler<TimeEntryStartedForLearningEto>,
        ITransientDependency
    {
        private readonly ILogger<TimeEntryStartedHandler> _logger;

        public TimeEntryStartedHandler(ILogger<TimeEntryStartedHandler> logger)
        {
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(TimeEntryStartedForLearningEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Time entry {TimeEntryId} started for learning session {SessionId}",
                    eventData.TimeEntryId, eventData.SessionId);

                // Store time entry ID in learning session
                // This allows us to stop it later

                // In real implementation:
                // var session = await _learningSessionRepository.GetAsync(eventData.SessionId);
                // session.SetActiveTimeEntry(eventData.TimeEntryId);
                // await _learningSessionRepository.UpdateAsync(session);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error associating time entry {TimeEntryId} with session {SessionId}",
                    eventData.TimeEntryId, eventData.SessionId);
                throw;
            }
        }
    }
}
