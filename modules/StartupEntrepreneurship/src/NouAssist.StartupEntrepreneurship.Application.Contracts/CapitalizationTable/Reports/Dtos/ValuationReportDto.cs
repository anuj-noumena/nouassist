using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos
{
    /// <summary>
    /// DTO for valuation report
    /// </summary>
    public class ValuationReportDto
    {
        public Guid CapTableId { get; set; }
        public string CompanyName { get; set; }
        public ValuationEntryDto CurrentValuation { get; set; }
        public List<ValuationEntryDto> History { get; set; } = new();
    }
    
    /// <summary>
    /// DTO for valuation entry
    /// </summary>
    public class ValuationEntryDto
    {
        public DateTime Date { get; set; }
        public decimal PreMoneyValuation { get; set; }
        public decimal PostMoneyValuation { get; set; }
        public string Currency { get; set; }
        public string Source { get; set; }
        public string Notes { get; set; }
    }
}
