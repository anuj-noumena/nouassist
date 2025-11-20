using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.TimeManagement.Calendar.EventHandlers
{
    /// <summary>
    /// Handles LearningPhaseStartedEto to automatically start time tracking
    /// </summary>
    public class LearningPhaseStartedEventHandler :
        IDistributedEventHandler<LearningPhaseStartedEto>,
        ITransientDependency
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<LearningPhaseStartedEventHandler> _logger;

        public LearningPhaseStartedEventHandler(
            ITimeEntryRepository timeEntryRepository,
            IDistributedEventBus eventBus,
            ILogger<LearningPhaseStartedEventHandler> logger)
        {
            _timeEntryRepository = timeEntryRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(LearningPhaseStartedEto eventData)
        {
            if (!eventData.RequestTimeTracking)
            {
                _logger.LogDebug(
                    "Time tracking not requested for learning phase {Phase} in session {SessionId}",
                    eventData.Phase, eventData.SessionId);
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Starting time tracking for learning phase {Phase} in session {SessionId}",
                    eventData.Phase, eventData.SessionId);

                var timeEntry = new TimeEntry(
                    id: Guid.NewGuid(),
                    ownerId: eventData.UserId,
                    startedAt: eventData.StartedAt,
                    tenantId: eventData.TenantId,
                    description: $"{eventData.TopicTitle} - {eventData.Phase}");

                // Add tags for filtering
                timeEntry.AddTag("Learning");
                timeEntry.AddTag(eventData.Phase.ToString());
                timeEntry.AddTag(eventData.TopicTitle);

                await _timeEntryRepository.InsertAsync(timeEntry, autoSave: true);

                // Notify LearningDevelopment that time tracking started
                await _eventBus.PublishAsync(new TimeEntryStartedForLearningEto
                {
                    SessionId = eventData.SessionId,
                    TimeEntryId = timeEntry.Id,
                    UserId = eventData.UserId,
                    TenantId = eventData.TenantId,
                    StartedAt = timeEntry.StartedAt
                });

                _logger.LogInformation(
                    "Time entry {TimeEntryId} created for learning session {SessionId}",
                    timeEntry.Id, eventData.SessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating time entry for learning phase {Phase} in session {SessionId}",
                    eventData.Phase, eventData.SessionId);
                throw;
            }
        }
    }

    /// <summary>
    /// Response event sent back to LearningDevelopment
    /// </summary>
    [Serializable]
    public class TimeEntryStartedForLearningEto
    {
        public Guid SessionId { get; set; }
        public Guid TimeEntryId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
