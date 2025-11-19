using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Dtos;
using NouAssist.WorkManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.GTD.GTD
{
    [Authorize(WorkManagementPermissions.GTD.WaitingFor.Default)]
    public class WaitingForAppService : ApplicationService, IWaitingForAppService
    {
        private readonly IWaitingForRepository _waitingForRepository;

        public WaitingForAppService(IWaitingForRepository waitingForRepository)
        {
            _waitingForRepository = waitingForRepository;
        }

        public async Task<WaitingForDto> GetAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        public async Task<ListResultDto<WaitingForDto>> GetMyActiveItemsAsync()
        {
            var userId = CurrentUser.GetId();
            var items = await _waitingForRepository.GetActiveByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<WaitingFor>, List<WaitingForDto>>(items);
            return new ListResultDto<WaitingForDto>(dtos);
        }

        public async Task<ListResultDto<WaitingForDto>> GetFollowUpDueAsync()
        {
            var userId = CurrentUser.GetId();
            var items = await _waitingForRepository.GetFollowUpDueAsync(userId, Clock.Now);
            var dtos = ObjectMapper.Map<List<WaitingFor>, List<WaitingForDto>>(items);
            return new ListResultDto<WaitingForDto>(dtos);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Create)]
        public async Task<WaitingForDto> CreateAsync(CreateWaitingForDto input)
        {
            var userId = CurrentUser.GetId();

            var item = new WaitingFor(
                GuidGenerator.Create(),
                userId,
                input.WfType,
                CurrentTenant.Id
            );

            if (input.TaskInstanceId.HasValue)
            {
                item.TaskInstanceId = input.TaskInstanceId;
            }

            if (input.ForUserId.HasValue)
            {
                item.ForUserId = input.ForUserId;
            }

            if (!string.IsNullOrWhiteSpace(input.Reason))
            {
                item.Reason = input.Reason;
            }

            if (input.FollowUpDate.HasValue)
            {
                item.SetFollowUp(input.FollowUpDate.Value);
            }

            await _waitingForRepository.InsertAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Update)]
        public async Task<WaitingForDto> UpdateAsync(Guid id, UpdateWaitingForDto input)
        {
            var item = await _waitingForRepository.GetAsync(id);

            if (input.Reason != null)
            {
                item.Reason = input.Reason;
            }

            if (input.Note != null)
            {
                item.AddNote(input.Note);
            }

            if (input.FollowUpDate.HasValue)
            {
                item.SetFollowUp(input.FollowUpDate.Value);
            }

            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _waitingForRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Resolve)]
        public async Task<WaitingForDto> ResolveAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            item.Resolve();
            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Update)]
        public async Task<WaitingForDto> PutOnHoldAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            item.PutOnHold();
            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Update)]
        public async Task<WaitingForDto> ReactivateAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            item.Reactivate();
            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Update)]
        public async Task<WaitingForDto> CancelAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            item.Cancel();
            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }

        [Authorize(WorkManagementPermissions.GTD.WaitingFor.Update)]
        public async Task<WaitingForDto> EscalateAsync(Guid id)
        {
            var item = await _waitingForRepository.GetAsync(id);
            item.Escalate();
            await _waitingForRepository.UpdateAsync(item);
            return ObjectMapper.Map<WaitingFor, WaitingForDto>(item);
        }
    }
}
