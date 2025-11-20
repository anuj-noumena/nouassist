using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos
{
    /// <summary>
    /// DTO for adding a share class to a security issuance
    /// </summary>
    public class AddShareClassDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxShareClassNameLength)]
        public string Name { get; set; }
        
        [StringLength(CapTableConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Range(0.0001, 1000)]
        public decimal ParValue { get; set; } = 0.001m;
        
        [Range(0, int.MaxValue)]
        public int VotingRights { get; set; } = 1;
        
        public bool HasDividendRights { get; set; } = true;
        
        public bool HasLiquidationPreference { get; set; } = false;
        
        [Range(0, 100)]
        public decimal LiquidationMultiple { get; set; } = 1.0m;
        
        public bool HasAntiDilutionRights { get; set; } = false;
        
        [StringLength(50)]
        public string AntiDilutionType { get; set; }
    }
}
