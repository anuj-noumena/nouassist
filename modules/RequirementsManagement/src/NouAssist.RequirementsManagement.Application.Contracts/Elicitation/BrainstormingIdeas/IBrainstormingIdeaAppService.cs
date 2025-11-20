using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos;
using NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas
{
    /// <summary>
    /// Application service interface for managing brainstorming ideas
    /// </summary>
    public interface IBrainstormingIdeaAppService : IApplicationService
    {
        Task<BrainstormingIdeaDto> GetAsync(Guid id);
        Task<PagedResultDto<BrainstormingIdeaDto>> GetListAsync(GetBrainstormingIdeaListDto input);
        Task<BrainstormingIdeaDto> CreateAsync(CreateBrainstormingIdeaDto input);
        Task<BrainstormingIdeaDto> UpdateAsync(Guid id, UpdateBrainstormingIdeaDto input);
        Task DeleteAsync(Guid id);
        Task<BrainstormingIdeaDto> SelectAsync(Guid id);
        Task<BrainstormingIdeaDto> DeselectAsync(Guid id);
        Task<BrainstormingIdeaDto> AddTagAsync(Guid id, CreateIdeaTagDto tag);
        Task<BrainstormingIdeaDto> RemoveTagAsync(Guid id, string tagName);
        Task<ListResultDto<BrainstormingIdeaDto>> GetBySessionAsync(Guid sessionId);
        Task<ListResultDto<BrainstormingIdeaDto>> GetSelectedBySessionAsync(Guid sessionId);
        Task<Dictionary<string, int>> GetTagUsageStatisticsAsync(Guid sessionId);
        Task<ElicitationArtifactDto> ConvertToArtifactAsync(Guid ideaId, ArtifactType artifactType, string title, string description);
    }
}
