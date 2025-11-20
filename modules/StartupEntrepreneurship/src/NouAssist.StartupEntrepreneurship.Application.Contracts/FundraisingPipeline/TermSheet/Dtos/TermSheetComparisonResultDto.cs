using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class TermSheetComparisonResultDto
    {
        public DateTime ComparisonDate { get; set; }
        public Dictionary<Guid, TermSheetSummaryDto> TermSheetDetails { get; set; }
        public Dictionary<Guid, decimal> ValuationComparison { get; set; }
        public Dictionary<Guid, decimal> EquityPercentageComparison { get; set; }
        public Dictionary<Guid, decimal> LiquidationMultipleComparison { get; set; }
        public Dictionary<string, Dictionary<Guid, string>> KeyClauseComparison { get; set; }
        public Dictionary<string, Dictionary<Guid, bool>> InvestorRightsComparison { get; set; }
    }

    public class TermSheetSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProspectId { get; set; }
        public string ProspectName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public TermSheetStatus Status { get; set; }
        public string StatusName => Status.ToString();
    }
}
