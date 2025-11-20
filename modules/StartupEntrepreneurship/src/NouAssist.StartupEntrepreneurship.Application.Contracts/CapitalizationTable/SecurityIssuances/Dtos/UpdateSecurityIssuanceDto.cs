using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for updating an existing SecurityIssuance
    /// </summary>
    public class UpdateSecurityIssuanceDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal? TotalAuthorized { get; set; }
    }
}
