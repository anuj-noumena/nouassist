using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Dtos;
using NouAssist.WorkManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.GTD.GTD
{
    [Authorize(WorkManagementPermissions.GTD.Horizons.Default)]
    public class GtdHorizonAlignmentAppService : ApplicationService, IGtdHorizonAlignmentAppService
    {
        private readonly IGtdHorizonAlignmentRepository _alignmentRepository;
        private readonly IGtdHorizonRepository _horizonRepository;

        public GtdHorizonAlignmentAppService(
            IGtdHorizonAlignmentRepository alignmentRepository,
            IGtdHorizonRepository horizonRepository)
        {
            _alignmentRepository = alignmentRepository;
            _horizonRepository = horizonRepository;
        }

        public async Task<GtdHorizonAlignmentDto> CreateAlignmentAsync(CreateAlignmentDto input)
        {
            var userId = CurrentUser.GetId();
            
            // Verify the horizon exists and belongs to the user
            var horizon = await _horizonRepository.GetAsync(input.HigherHorizonId);
            if (horizon.OwnerId != userId)
            {
                throw new UserFriendlyException("You can only create alignments for your own horizons.");
            }

            // Check if alignment already exists
            var existing = await _alignmentRepository.FindByHorizonAndEntityAsync(
                input.HigherHorizonId,
                input.LowerEntityType,
                input.LowerEntityId
            );

            if (existing != null)
            {
                throw new UserFriendlyException("This alignment already exists.");
            }

            var alignment = new GtdHorizonAlignment(
                GuidGenerator.Create(),
                userId,
                input.HigherHorizonId,
                horizon.HorizonLevel,
                input.LowerEntityType,
                input.LowerEntityId,
                CurrentTenant.Id
            );

            if (input.AlignmentStrength.HasValue)
                alignment.UpdateStrength(input.AlignmentStrength.Value);

            await _alignmentRepository.InsertAsync(alignment);
            return ObjectMapper.Map<GtdHorizonAlignment, GtdHorizonAlignmentDto>(alignment);
        }

        public async Task<GtdHorizonAlignmentDto> UpdateStrengthAsync(Guid id, int strength)
        {
            var alignment = await _alignmentRepository.GetAsync(id);
            alignment.UpdateStrength(strength);
            await _alignmentRepository.UpdateAsync(alignment);
            return ObjectMapper.Map<GtdHorizonAlignment, GtdHorizonAlignmentDto>(alignment);
        }

        public async Task<GtdHorizonAlignmentDto> ValidateAlignmentAsync(Guid id, ValidateAlignmentDto input)
        {
            var alignment = await _alignmentRepository.GetAsync(id);
            
            alignment.Validate(input.IsAligned);
            
            if (!string.IsNullOrWhiteSpace(input.ValidationNotes))
                alignment.ValidationNotes = input.ValidationNotes;

            await _alignmentRepository.UpdateAsync(alignment);
            return ObjectMapper.Map<GtdHorizonAlignment, GtdHorizonAlignmentDto>(alignment);
        }

        public async Task<ListResultDto<GtdHorizonAlignmentDto>> GetWeakAlignmentsAsync()
        {
            var userId = CurrentUser.GetId();
            var alignments = await _alignmentRepository.GetWeakAlignmentsAsync(userId);
            var dtos = ObjectMapper.Map<List<GtdHorizonAlignment>, List<GtdHorizonAlignmentDto>>(alignments);
            return new ListResultDto<GtdHorizonAlignmentDto>(dtos);
        }

        public async Task DeleteAlignmentAsync(Guid id)
        {
            await _alignmentRepository.DeleteAsync(id);
        }
    }
}
