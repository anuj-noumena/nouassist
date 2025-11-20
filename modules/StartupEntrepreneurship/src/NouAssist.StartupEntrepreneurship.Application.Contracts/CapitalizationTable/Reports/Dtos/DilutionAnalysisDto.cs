using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos
{
    /// <summary>
    /// DTO for dilution analysis report
    /// </summary>
    public class DilutionAnalysisDto
    {
        public Guid CapTableId { get; set; }
        public string CompanyName { get; set; }
        public decimal NewSharesIssued { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<DilutionImpactDto> DilutionImpacts { get; set; } = new();
    }
    
    /// <summary>
    /// DTO for individual dilution impact
    /// </summary>
    public class DilutionImpactDto
    {
        public Guid HolderId { get; set; }
        public string HolderName { get; set; }
        public string HolderType { get; set; }
        public decimal OwnershipBefore { get; set; }
        public decimal OwnershipAfter { get; set; }
        public decimal DilutionPercentage { get; set; }
        public decimal SharesBefore { get; set; }
        public decimal SharesAfter { get; set; }
    }
}
