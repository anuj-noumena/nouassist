using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class GetGrowthExperimentListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by name or description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public ExperimentStatus? Status { get; set; }
    }
}

// Value Object DTOs
