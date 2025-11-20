using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for filtering and paging decision model lists
    /// </summary>
    public class GetDecisionModelListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for searching decision models
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by finalized status
        /// </summary>
        public bool? IsFinalized { get; set; }
        
        /// <summary>
        /// Filter by model type
        /// </summary>
        public string? ModelType { get; set; }
    }
}
