using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Integration.Enums;
using NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.Integration.LearningPaths
{
    public interface ILearningPathAppService : IApplicationService
    {
        Task<LearningPathDto> CreateAsync(CreateLearningPathDto input);

        Task<LearningPathDto> GetAsync(Guid id);

        Task<List<LearningPathDto>> GetListAsync();

        Task<List<LearningPathDto>> GetByStatusAsync(LearningPathStatus status);

        Task<LearningPathDto> StartAsync(Guid id);

        Task<LearningPathDto> PutOnHoldAsync(Guid id);

        Task<LearningPathDto> ResumeAsync(Guid id);

        Task<LearningPathDto> AbandonAsync(Guid id, string reason = null);

        Task<LearningPathDto> EnterMaintenanceAsync(Guid id);

        Task LinkTopicToSkillAsync(Guid topicId, Guid skillId);

        Task UnlinkTopicFromSkillAsync(Guid topicId, Guid skillId);

        Task<IntegrationStatusDto> GetIntegrationStatusAsync(Guid topicId, Guid skillId);

        Task<LearningPathDto> GetByTopicIdAsync(Guid topicId);

        Task<LearningPathDto> GetBySkillIdAsync(Guid skillId);
    }
}
