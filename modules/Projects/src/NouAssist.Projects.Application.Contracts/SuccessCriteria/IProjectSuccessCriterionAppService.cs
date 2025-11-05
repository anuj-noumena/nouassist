using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Projects.SuccessCriteria.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.SuccessCriteria
{
    public interface IProjectSuccessCriterionAppService : IApplicationService
    {
        Task<SuccessCriterionDto> GetAsync(Guid id);
        
        Task<List<SuccessCriterionDto>> GetByProjectAsync(Guid projectId);
        
        Task<List<SuccessCriterionDto>> GetUnmetCriteriaAsync(Guid projectId);
        
        Task<SuccessCriterionDto> CreateAsync(CreateSuccessCriterionDto input);
        
        Task<SuccessCriterionDto> MarkAsMetAsync(Guid id, MarkCriterionAsMetDto input);
        
        Task DeleteAsync(Guid id);
    }
}
