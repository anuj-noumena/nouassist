using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Dtos;
using NouAssist.WorkManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.GTD.GTD
{
    [Authorize(WorkManagementPermissions.GTD.WaitingFor.Default)]
    public class FollowupAppService : ApplicationService, IFollowupAppService
    {
        private readonly IRepository<Followup, Guid> _followupRepository;

        public FollowupAppService(IRepository<Followup, Guid> followupRepository)
        {
            _followupRepository = followupRepository;
        }

        public async Task<FollowupDto> GetAsync(Guid id)
        {
            var followup = await _followupRepository.GetAsync(id);
            return ObjectMapper.Map<Followup, FollowupDto>(followup);
        }

        public async Task<ListResultDto<FollowupDto>> GetMyDueFollowupsAsync()
        {
            var userId = CurrentUser.GetId();
            var queryable = await _followupRepository.GetQueryableAsync();
            
            var followups = await AsyncExecuter.ToListAsync(
                queryable.Where(f => f.OwnerId == userId && f.IsDue)
                         .OrderBy(f => f.FollowupAt)
            );

            var dtos = ObjectMapper.Map<List<Followup>, List<FollowupDto>>(followups);
            return new ListResultDto<FollowupDto>(dtos);
        }

        public async Task<FollowupDto> CreateAsync(CreateFollowupDto input)
        {
            var userId = CurrentUser.GetId();
            
            var followup = new Followup(
                GuidGenerator.Create(),
                userId,
                input.FollowupAt,
                CurrentTenant.Id
            );

            if (input.TaskInstanceId.HasValue)
                followup.TaskInstanceId = input.TaskInstanceId;

            if (!string.IsNullOrWhiteSpace(input.Note))
                followup.Note = input.Note;

            await _followupRepository.InsertAsync(followup);
            return ObjectMapper.Map<Followup, FollowupDto>(followup);
        }

        public async Task<FollowupDto> MarkAsDoneAsync(Guid id)
        {
            var followup = await _followupRepository.GetAsync(id);
            followup.MarkAsDone();
            await _followupRepository.UpdateAsync(followup);
            return ObjectMapper.Map<Followup, FollowupDto>(followup);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _followupRepository.DeleteAsync(id);
        }
    }
}
