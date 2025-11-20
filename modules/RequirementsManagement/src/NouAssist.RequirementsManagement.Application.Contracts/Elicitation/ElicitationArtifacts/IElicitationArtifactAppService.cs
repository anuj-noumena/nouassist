using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts
{
    /// <summary>
    /// Application service interface for managing elicitation artifacts
    /// </summary>
    public interface IElicitationArtifactAppService : IApplicationService
    {
        Task<ElicitationArtifactDto> GetAsync(Guid id);
        Task<PagedResultDto<ElicitationArtifactDto>> GetListAsync(GetElicitationArtifactListDto input);
        Task<ElicitationArtifactDto> CreateAsync(CreateElicitationArtifactDto input);
        Task<ElicitationArtifactDto> CreateUserStoryAsync(CreateUserStoryDto input);
        Task<ElicitationArtifactDto> CreateUseCaseAsync(CreateUseCaseDto input);
        Task<ElicitationArtifactDto> CreateEventDefinitionAsync(CreateEventDefinitionDto input);
        Task<ElicitationArtifactDto> UpdateAsync(Guid id, UpdateElicitationArtifactDto input);
        Task DeleteAsync(Guid id);
        Task<ElicitationArtifactDto> SubmitForReviewAsync(Guid id);
        Task<ElicitationArtifactDto> ApproveAsync(Guid id);
        Task<ElicitationArtifactDto> RejectAsync(Guid id);
        Task<ElicitationArtifactDto> MarkAsImplementedAsync(Guid id);
        Task<ElicitationArtifactDto> MarkAsObsoleteAsync(Guid id);
        Task<ElicitationArtifactDto> ReturnToDraftAsync(Guid id);
        Task<ElicitationArtifactDto> AddTagAsync(Guid id, IdeaTagDto tag);
        Task<ElicitationArtifactDto> RemoveTagAsync(Guid id, string tagName);
        Task<ListResultDto<ElicitationArtifactDto>> GetApprovedArtifactsAsync();
        Task<ListResultDto<ElicitationArtifactDto>> GetArtifactsBySessionAsync(Guid sessionId);
    }
}
