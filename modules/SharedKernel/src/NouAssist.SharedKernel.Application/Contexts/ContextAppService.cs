using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using NouAssist.SharedKernel.Tags.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel.Contexts
{
    [Authorize]
    public class ContextAppService : ApplicationService, IContextAppService
    {
        private readonly IContextRepository _contextRepository;

        public ContextAppService(IContextRepository contextRepository)
        {
            _contextRepository = contextRepository;
        }

        public async Task<ContextDto> GetAsync(Guid id)
        {
            var context = await _contextRepository.GetAsync(id);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }

        public async Task<ListResultDto<ContextDto>> GetMyActiveContextsAsync()
        {
            var userId = CurrentUser.GetId();
            var contexts = await _contextRepository.GetActiveByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<Context>, List<ContextDto>>(contexts);
            return new ListResultDto<ContextDto>(dtos);
        }

        public async Task<ListResultDto<ContextDto>> GetByLocationAsync(string location)
        {
            var userId = CurrentUser.GetId();
            var contexts = await _contextRepository.GetByLocationAsync(userId, location);
            var dtos = ObjectMapper.Map<List<Context>, List<ContextDto>>(contexts);
            return new ListResultDto<ContextDto>(dtos);
        }

        public async Task<ContextDto> CreateAsync(CreateContextDto input)
        {
            var userId = CurrentUser.GetId();
            
            var context = new Context(
                GuidGenerator.Create(),
                userId,
                input.Title,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Notes))
                context.Notes = input.Notes;

            if (!string.IsNullOrWhiteSpace(input.Icon))
                context.Icon = input.Icon;

            if (!string.IsNullOrWhiteSpace(input.Color))
                context.Color = input.Color;

            if (!string.IsNullOrWhiteSpace(input.Location))
                context.SetLocation(input.Location);

            if (input.EnergyLevel.HasValue)
                context.SetEnergyLevel(input.EnergyLevel.Value);

            await _contextRepository.InsertAsync(context);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }

        public async Task<ContextDto> UpdateAsync(Guid id, UpdateContextDto input)
        {
            var context = await _contextRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                context.Title = input.Title;

            if (input.Notes != null)
                context.Notes = input.Notes;

            if (input.Icon != null)
                context.Icon = input.Icon;

            if (input.Color != null)
                context.Color = input.Color;

            if (input.Location != null)
                context.SetLocation(input.Location);

            if (input.EnergyLevel.HasValue)
                context.SetEnergyLevel(input.EnergyLevel.Value);

            await _contextRepository.UpdateAsync(context);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _contextRepository.DeleteAsync(id);
        }

        public async Task<ContextDto> ArchiveAsync(Guid id)
        {
            var context = await _contextRepository.GetAsync(id);
            context.Archive();
            await _contextRepository.UpdateAsync(context);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }

        public async Task<ContextDto> ReactivateAsync(Guid id)
        {
            var context = await _contextRepository.GetAsync(id);
            context.Reactivate();
            await _contextRepository.UpdateAsync(context);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }

        public async Task<ContextDto> RecordUsageAsync(Guid id)
        {
            var context = await _contextRepository.GetAsync(id);
            context.RecordUsage();
            await _contextRepository.UpdateAsync(context);
            return ObjectMapper.Map<Context, ContextDto>(context);
        }
    }
}
