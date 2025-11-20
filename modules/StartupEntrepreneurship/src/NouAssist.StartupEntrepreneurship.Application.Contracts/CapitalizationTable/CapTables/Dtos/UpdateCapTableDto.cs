using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos
{
    /// <summary>
    /// DTO for updating an existing CapTable
    /// </summary>
    public class UpdateCapTableDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxCompanyNameLength)]
        public string CompanyName { get; set; }
        
        [StringLength(CapTableConsts.MaxNameLength)]
        public string CompanyIdentifier { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal TotalAuthorizedShares { get; set; }
        
        public DateTime? AsOfDate { get; set; }
    }
}
