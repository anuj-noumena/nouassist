using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for exercising stock options
    /// </summary>
    public class ExerciseOptionsDto
    {
        [Required]
        public Guid SecurityIssuanceId { get; set; }
        
        [Required]
        [Range(1, (double)CapTableConsts.MaxShareQuantity)]
        public decimal Quantity { get; set; }
        
        public DateTime? ExerciseDate { get; set; }
    }
}
