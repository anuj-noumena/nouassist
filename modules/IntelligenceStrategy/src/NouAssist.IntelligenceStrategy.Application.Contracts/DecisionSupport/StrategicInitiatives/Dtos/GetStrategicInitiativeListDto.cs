using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for filtering and paging strategic initiative lists
    /// </summary>
    public class GetStrategicInitiativeListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for searching initiatives
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by initiative status
        /// </summary>
        public ProposalStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by strategic alignment level
        /// </summary>
        public AlignmentStatus? Alignment { get; set; }
        
        /// <summary>
        /// Filter by related decision model ID
        /// </summary>
        public Guid? DecisionModelId { get; set; }
    }
}
