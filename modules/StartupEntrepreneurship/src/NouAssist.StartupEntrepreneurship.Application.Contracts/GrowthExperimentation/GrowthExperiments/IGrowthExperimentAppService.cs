using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments
{
    public interface IGrowthExperimentAppService : IApplicationService
    {
        // Basic CRUD operations
        Task<GrowthExperimentDto> GetAsync(Guid id);
        Task<PagedResultDto<GrowthExperimentDto>> GetListAsync(GetGrowthExperimentListDto input);
        Task<GrowthExperimentDto> CreateAsync(CreateGrowthExperimentDto input);
        Task<GrowthExperimentDto> UpdateAsync(Guid id, UpdateGrowthExperimentDto input);
        Task DeleteAsync(Guid id);
        
        // Experiment design operations
        Task<GrowthExperimentDto> AddHypothesisAsync(Guid id, CreateHypothesisDto input);
        Task<GrowthExperimentDto> AddVariantAsync(Guid id, CreateExperimentVariantDto input);
        Task<GrowthExperimentDto> SetExperimentReadyAsync(Guid id);
        
        // Experiment execution operations
        Task<GrowthExperimentDto> StartExperimentAsync(Guid id);
        Task<GrowthExperimentDto> PauseExperimentAsync(Guid id);
        Task<GrowthExperimentDto> ResumeExperimentAsync(Guid id);
        Task<GrowthExperimentDto> CompleteExperimentAsync(Guid id);
        Task<GrowthExperimentDto> TerminateExperimentAsync(Guid id);
        
        // Results operations
        Task<ExperimentResultDataDto> RecordResultAsync(Guid experimentId, RecordExperimentResultDto input);
        Task<List<ExperimentResultDataDto>> GetResultsForVariantAsync(Guid variantId);
        Task<StatisticalSignificanceDto> AnalyzeSignificanceAsync(Guid id, string primaryMetricName);
        Task<GrowthExperimentDto> FinalizeLearningsAsync(Guid id, CreateExperimentLearningsDto input);
    }
}
