using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.DecisionModel
{
    /// <summary>
    /// Input DTO for querying decision models
    /// </summary>
    public class GetDecisionModelsInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by name
        /// </summary>
        public string Filter { get; set; }
    }
}
