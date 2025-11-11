using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.GTD.Services
{
    public class TicklerProcessingService : DomainService
    {
        private readonly ITicklerRepository _ticklerRepository;

        public TicklerProcessingService(ITicklerRepository ticklerRepository)
        {
            _ticklerRepository = ticklerRepository;
        }

        public async Task<TicklerProcessingResult> SurfaceDueTicklersAsync(
            Guid userId,
            DateTime currentDate)
        {
            var result = new TicklerProcessingResult();
            
            var dueTicklers = await _ticklerRepository
                .GetDueTicklersAsync(userId, currentDate);

            foreach (var tickler in dueTicklers)
            {
                if (tickler.IsScheduled)
                {
                    tickler.Surface();
                    result.SurfacedCount++;
                }

                // Handle recurring ticklers
                if (tickler.IsRecurring && tickler.RecurrencePattern != null)
                {
                    var nextOccurrence = tickler.GetNextOccurrence();
                    if (nextOccurrence.HasValue)
                    {
                        // Create new tickler for next occurrence
                        var newTickler = new TicklerItem(
                            GuidGenerator.Create(),
                            tickler.OwnerId,
                            tickler.Title,
                            nextOccurrence.Value,
                            tickler.TenantId
                        );
                        
                        newTickler.SetRecurrence(tickler.RecurrencePattern);
                        await _ticklerRepository.InsertAsync(newTickler);
                        result.RecurringTicklersCreated++;
                    }
                }
            }

            result.IsSuccessful = true;
            return result;
        }

        public TicklerValidationResult ValidateTicklerCreation(
            DateTime tickleDate,
            bool isRecurring)
        {
            var result = new TicklerValidationResult();

            if (tickleDate < Clock.Now.Date)
            {
                result.AddError("Tickle date cannot be in the past.");
            }

            if (tickleDate > Clock.Now.AddYears(5))
            {
                result.AddError("Tickle date cannot be more than 5 years in the future.");
            }

            result.IsValid = result.Errors.Count == 0;
            return result;
        }
    }

    public class TicklerProcessingResult
    {
        public bool IsSuccessful { get; set; }
        public int SurfacedCount { get; set; }
        public int RecurringTicklersCreated { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class TicklerValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }
    }
}
