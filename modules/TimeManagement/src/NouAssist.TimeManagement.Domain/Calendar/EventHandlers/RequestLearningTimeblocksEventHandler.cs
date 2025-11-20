using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.Calendar.Enums;
using NouAssist.TimeManagement.Calendar.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.TimeManagement.Calendar.EventHandlers
{
    /// <summary>
    /// Handles RequestLearningTimeblocksEto to automatically create scheduled timeblocks
    /// </summary>
    public class RequestLearningTimeblocksEventHandler :
        IDistributedEventHandler<RequestLearningTimeblocksEto>,
        ITransientDependency
    {
        private readonly ITimeblockRepository _timeblockRepository;
        private readonly ICalendarEventRepository _calendarEventRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<RequestLearningTimeblocksEventHandler> _logger;

        public RequestLearningTimeblocksEventHandler(
            ITimeblockRepository timeblockRepository,
            ICalendarEventRepository calendarEventRepository,
            IDistributedEventBus eventBus,
            ILogger<RequestLearningTimeblocksEventHandler> logger)
        {
            _timeblockRepository = timeblockRepository;
            _calendarEventRepository = calendarEventRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(RequestLearningTimeblocksEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating timeblocks for learning path {LearningPathId} with {PhaseCount} phases",
                    eventData.LearningPathId, eventData.Phases.Count);

                var createdTimeblocks = new List<Guid>();
                var currentDate = DateTime.Today.AddDays(1); // Start tomorrow
                var preferredDays = eventData.Schedule?.PreferredDays ?? GetDefaultPreferredDays();
                var startTime = eventData.Schedule?.PreferredStartTime ?? new TimeSpan(9, 0, 0);
                var duration = eventData.Schedule?.SessionDurationMinutes ?? 60;

                foreach (var phase in eventData.Phases)
                {
                    // Find next available preferred day
                    while (!preferredDays.Contains(currentDate.DayOfWeek))
                    {
                        currentDate = currentDate.AddDays(1);
                    }

                    var startAt = currentDate.Date.Add(startTime);
                    var endAt = startAt.AddMinutes(duration);

                    // Create calendar event first
                    var calendarEvent = new CalendarEvent(
                        id: Guid.NewGuid(),
                        ownerId: eventData.UserId,
                        title: $"{eventData.Title} - {phase.PhaseName}",
                        startAt: startAt,
                        endAt: endAt,
                        tenantId: eventData.TenantId);

                    calendarEvent.UpdateDescription($"Learning session for {phase.PhaseName}");
                    calendarEvent.SetStatus(EventStatus.Confirmed);

                    await _calendarEventRepository.InsertAsync(calendarEvent, autoSave: false);

                    // Create timeblock linked to learning session
                    var timeblock = new Timeblock(
                        id: Guid.NewGuid(),
                        ownerId: eventData.UserId,
                        calendarEventId: calendarEvent.Id,
                        startAt: startAt,
                        endAt: endAt,
                        tenantId: eventData.TenantId,
                        blockType: TimeblockType.Learning,
                        title: $"{eventData.Title} - {phase.PhaseName}",
                        description: $"Focused learning time for {phase.PhaseName}");

                    // Link to learning session
                    var linkedEntity = new LinkedEntity(
                        phase.SessionId,
                        LinkedEntityType.LearningSession);

                    // Note: Need to add method to set LinkedEntity on Timeblock
                    // For now, we'll track it via description or add to aggregate

                    await _timeblockRepository.InsertAsync(timeblock, autoSave: false);

                    createdTimeblocks.Add(timeblock.Id);

                    _logger.LogDebug(
                        "Created timeblock {TimeblockId} for phase {Phase} on {Date}",
                        timeblock.Id, phase.PhaseName, currentDate);

                    // Move to next preferred day
                    currentDate = currentDate.AddDays(1);
                    while (!preferredDays.Contains(currentDate.DayOfWeek))
                    {
                        currentDate = currentDate.AddDays(1);
                    }
                }

                await _timeblockRepository.GetDbContext().SaveChangesAsync();

                // Publish completion event
                await _eventBus.PublishAsync(new LearningTimeblocksCreatedEto
                {
                    LearningPathId = eventData.LearningPathId,
                    UserId = eventData.UserId,
                    TenantId = eventData.TenantId,
                    TimeblockIds = createdTimeblocks,
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation(
                    "Created {Count} timeblocks for learning path {LearningPathId}",
                    createdTimeblocks.Count, eventData.LearningPathId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating timeblocks for learning path {LearningPathId}",
                    eventData.LearningPathId);
                throw;
            }
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

    /// <summary>
    /// Response event sent back to LearningDevelopment
    /// </summary>
    [Serializable]
    public class LearningTimeblocksCreatedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public List<Guid> TimeblockIds { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
