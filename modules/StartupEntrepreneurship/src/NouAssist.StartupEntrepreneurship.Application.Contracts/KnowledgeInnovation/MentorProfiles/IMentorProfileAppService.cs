using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles
{
    public interface IMentorProfileAppService : IApplicationService
    {
        Task<MentorProfileDto> GetAsync(Guid id);
        
        Task<PagedResultDto<MentorProfileListDto>> GetListAsync(GetMentorProfileListDto input);
        
        Task<MentorProfileDto> CreateAsync(CreateMentorProfileDto input);
        
        Task<MentorProfileDto> UpdateAsync(Guid id, UpdateMentorProfileDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<NetworkRelationshipDto> AddRelationshipAsync(Guid id, AddRelationshipDto input);
        
        Task RemoveRelationshipAsync(Guid id, Guid relatedEntityId);
        
        Task<AdviceRecordDto> AddAdviceAsync(Guid id, AddAdviceDto input);
        
        Task<List<MentorProfileListDto>> GetByExpertiseAsync(string expertiseKeyword);
        
        Task<List<MentorProfileListDto>> GetExperiencedMentorsAsync(int minimumYears);
    }
}
