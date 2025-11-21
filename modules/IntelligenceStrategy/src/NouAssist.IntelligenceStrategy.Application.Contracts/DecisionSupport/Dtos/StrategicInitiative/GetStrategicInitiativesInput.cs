using NouAssist.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.StrategicInitiative
{
    /// <summary>
    /// Input DTO for querying strategic initiatives
    /// </summary>
    public class GetStrategicInitiativesInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by title
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public ProposalStatus? Status { get; set; }
    }
}
