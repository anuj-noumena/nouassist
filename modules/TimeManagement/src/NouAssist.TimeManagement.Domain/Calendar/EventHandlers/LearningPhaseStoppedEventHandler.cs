using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.TimeManagement.Calendar.EventHandlers
{
    /// <summary>
    /// Handles LearningPhaseStoppedEto to automatically stop time tracking
    /// </summary>
    public class LearningPhaseStoppedEventHandler :
        IDistributedEventHandler<LearningPhaseStoppedEto>,
        ITransientDependency
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ILogger<LearningPhaseStoppedEventHandler> _logger;

        public LearningPhaseStoppedEventHandler(
            ITimeEntryRepository timeEntryRepository,
            ILogger<LearningPhaseStoppedEventHandler> logger)
        {
            _timeEntryRepository = timeEntryRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(LearningPhaseStoppedEto eventData)
        {
            if (!eventData.TimeEntryId.HasValue)
            {
                _logger.LogDebug(
                    "No time entry ID provided for learning phase {Phase} in session {SessionId}",
                    eventData.Phase, eventData.SessionId);
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Stopping time tracking for learning phase {Phase} in session {SessionId}",
                    eventData.Phase, eventData.SessionId);

                var timeEntry = await _timeEntryRepository.GetAsync(eventData.TimeEntryId.Value);

                if (timeEntry.IsRunning)
                {
                    timeEntry.Stop();

                    // Update duration from actual event data if provided
                    if (eventData.ActualMinutesSpent > 0)
                    {
                        timeEntry.UpdateDuration(eventData.ActualMinutesSpent);
                    }

                    await _timeEntryRepository.UpdateAsync(timeEntry, autoSave: true);

                    _logger.LogInformation(
                        "Time entry {TimeEntryId} stopped. Duration: {Duration} minutes",
                        timeEntry.Id, timeEntry.DurationMinutes);
                }
                else
                {
                    _logger.LogWarning(
                        "Time entry {TimeEntryId} was already stopped",
                        eventData.TimeEntryId.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error stopping time entry {TimeEntryId} for learning phase {Phase}",
                    eventData.TimeEntryId, eventData.Phase);
                throw;
            }
        }
    }
}
