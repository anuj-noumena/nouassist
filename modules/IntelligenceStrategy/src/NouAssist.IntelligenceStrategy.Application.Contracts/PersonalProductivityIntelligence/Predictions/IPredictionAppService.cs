using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions
{
    /// <summary>
    /// Application service for predictions
    /// </summary>
    public interface IPredictionAppService : IApplicationService
    {
        /// <summary>
        /// Gets a prediction by ID
        /// </summary>
        Task<PredictionDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets predictions for a user
        /// </summary>
        Task<PagedResultDto<PredictionDto>> GetListAsync(GetUserPredictionsDto input);
        
        /// <summary>
        /// Predicts task duration
        /// </summary>
        Task<TaskDurationPredictionDto> PredictTaskDurationAsync(PredictTaskDurationDto input);
        
        /// <summary>
        /// Predicts risk
        /// </summary>
        Task<RiskPredictionDto> PredictRiskAsync(PredictRiskDto input);
        
        /// <summary>
        /// Predicts cognitive load
        /// </summary>
        Task<NumericPredictionDto> PredictCognitiveLoadAsync(PredictCognitiveLoadDto input);
        
        /// <summary>
        /// Records actual outcome for a prediction
        /// </summary>
        Task RecordActualOutcomeAsync(RecordActualOutcomeDto input);
    }
}
