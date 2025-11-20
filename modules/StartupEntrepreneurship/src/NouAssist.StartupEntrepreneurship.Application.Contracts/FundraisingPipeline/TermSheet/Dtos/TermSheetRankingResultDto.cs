using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos
{
    public class TermSheetRankingResultDto
    {
        public TermSheetComparisonResultDto ComparisonResult { get; set; }
        public DateTime RankingDate { get; set; }
        public Dictionary<Guid, int> TermSheetScores { get; set; }
        public Dictionary<Guid, List<string>> RankingExplanations { get; set; }
        public List<Guid> RankedTermSheets { get; set; }
        public Guid TopTermSheetId { get; set; }
    }
}
