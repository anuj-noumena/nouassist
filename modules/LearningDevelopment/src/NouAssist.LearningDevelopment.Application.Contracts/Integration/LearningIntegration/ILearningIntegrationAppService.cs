using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Integration.LearningIntegration.Dtos;
using NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.Integration.LearningIntegration
{
    public interface ILearningIntegrationAppService : IApplicationService
    {
        Task<LearningPathDto> CreateIntegratedLearningPathAsync(CreateIntegratedLearningPathDto input);

        Task<LearningDashboardDto> GetDashboardAsync();

        Task<LearningActivitySnapshotDto> GetLearningActivityAsync(Guid learningPathId);

        Task StartLearningPhaseAsync(Guid sessionId, string phase);

        Task StopLearningPhaseAsync(Guid sessionId, string phase, int actualMinutesSpent);
    }
}
