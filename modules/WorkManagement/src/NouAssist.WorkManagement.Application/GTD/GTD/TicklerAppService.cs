using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using NouAssist.WorkManagement.GTD.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.GTD.GTD
{
    [Authorize(GtdPermissions.Ticklers.Default)]
    public class TicklerAppService : ApplicationService, ITicklerAppService
    {
        private readonly ITicklerRepository _ticklerRepository;
        private readonly TicklerProcessingService _ticklerProcessingService;

        public TicklerAppService(
            ITicklerRepository ticklerRepository,
            TicklerProcessingService ticklerProcessingService)
        {
            _ticklerRepository = ticklerRepository;
            _ticklerProcessingService = ticklerProcessingService;
        }

        public async Task<TicklerItemDto> GetAsync(Guid id)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        public async Task<PagedResultDto<TicklerItemDto>> GetListAsync(GetTicklerListDto input)
        {
            var userId = CurrentUser.GetId();
            var queryable = await _ticklerRepository.GetQueryableAsync();
            
            var query = queryable.Where(t => t.OwnerId == userId);

            if (input.Status.HasValue)
                query = query.Where(t => t.Status == input.Status.Value);

            if (input.FromDate.HasValue)
                query = query.Where(t => t.TickleDate >= input.FromDate.Value);

            if (input.ToDate.HasValue)
                query = query.Where(t => t.TickleDate <= input.ToDate.Value);

            if (input.OnlyDue == true)
                query = query.Where(t => t.TickleDate <= Clock.Now && t.Status == TicklerStatus.Scheduled);

            var totalCount = await AsyncExecuter.CountAsync(query);
            
            query = query
                .OrderBy(input.Sorting ?? "TickleDate")
                .PageBy(input.SkipCount, input.MaxResultCount);

            var ticklers = await AsyncExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<TicklerItem>, List<TicklerItemDto>>(ticklers);

            return new PagedResultDto<TicklerItemDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<TicklerItemDto>> GetDueTicklersAsync()
        {
            var userId = CurrentUser.GetId();
            var ticklers = await _ticklerRepository.GetDueTicklersAsync(userId, Clock.Now);
            var dtos = ObjectMapper.Map<List<TicklerItem>, List<TicklerItemDto>>(ticklers);
            return new ListResultDto<TicklerItemDto>(dtos);
        }

        public async Task<ListResultDto<TicklerItemDto>> GetSurfacedTicklersAsync()
        {
            var userId = CurrentUser.GetId();
            var ticklers = await _ticklerRepository.GetSurfacedTicklersAsync(userId);
            var dtos = ObjectMapper.Map<List<TicklerItem>, List<TicklerItemDto>>(ticklers);
            return new ListResultDto<TicklerItemDto>(dtos);
        }

        [Authorize(GtdPermissions.Ticklers.Create)]
        public async Task<TicklerItemDto> CreateAsync(CreateTicklerItemDto input)
        {
            var userId = CurrentUser.GetId();

            // Validate tickler creation
            var validation = _ticklerProcessingService.ValidateTicklerCreation(input.TickleDate, false);
            if (!validation.IsValid)
            {
                throw new UserFriendlyException(string.Join(", ", validation.Errors));
            }

            var tickler = new TicklerItem(
                GuidGenerator.Create(),
                userId,
                input.Title,
                input.TickleDate,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                tickler.Description = input.Description;

            if (!string.IsNullOrWhiteSpace(input.TickleTime))
            {
                if (TimeSpan.TryParse(input.TickleTime, out var time))
                    tickler.TickleTime = time;
            }

            if (input.ImportanceScore.HasValue)
                tickler.Importance = new ImportanceScore(input.ImportanceScore.Value);

            if (input.UrgencyScore.HasValue)
                tickler.Urgency = new UrgencyScore(input.UrgencyScore.Value);

            if (input.ContextId.HasValue)
                tickler.ContextId = input.ContextId;

            await _ticklerRepository.InsertAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Update)]
        public async Task<TicklerItemDto> UpdateAsync(Guid id, UpdateTicklerItemDto input)
        {
            var tickler = await _ticklerRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                tickler.Title = input.Title;

            if (input.Description != null)
                tickler.Description = input.Description;

            if (input.TickleDate.HasValue)
                tickler.Reschedule(input.TickleDate.Value);

            if (input.Notes != null)
                tickler.Notes = input.Notes;

            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _ticklerRepository.DeleteAsync(id);
        }

        [Authorize(GtdPermissions.Ticklers.Surface)]
        public async Task<TicklerItemDto> SurfaceAsync(Guid id)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            tickler.Surface();
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Process)]
        public async Task<TicklerItemDto> TakeActionAsync(Guid id, TicklerActionType actionType)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            tickler.TakeAction(actionType);
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Process)]
        public async Task<TicklerItemDto> DismissAsync(Guid id)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            tickler.Dismiss();
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Update)]
        public async Task<TicklerItemDto> SnoozeAsync(Guid id, SnoozeTicklerDto input)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            tickler.Snooze(input.SnoozeUntil);
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Update)]
        public async Task<TicklerItemDto> RescheduleAsync(Guid id, DateTime newDate)
        {
            var tickler = await _ticklerRepository.GetAsync(id);
            tickler.Reschedule(newDate);
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Update)]
        public async Task<TicklerItemDto> SetRecurrenceAsync(Guid id, SetRecurrenceDto input)
        {
            var tickler = await _ticklerRepository.GetAsync(id);

            var frequency = Enum.Parse<RecurrenceFrequency>(input.Frequency);
            var recurrence = new RecurrenceRule(
                frequency,
                input.Interval,
                input.Until,
                input.Count
            );

            tickler.SetRecurrence(recurrence);
            await _ticklerRepository.UpdateAsync(tickler);
            return ObjectMapper.Map<TicklerItem, TicklerItemDto>(tickler);
        }

        [Authorize(GtdPermissions.Ticklers.Process)]
        public async Task ProcessDueTicklersAsync()
        {
            var userId = CurrentUser.GetId();
            await _ticklerProcessingService.SurfaceDueTicklersAsync(userId, Clock.Now);
        }
    }
}
