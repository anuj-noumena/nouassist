using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services
{
    /// <summary>
    /// Domain service interface for calculating investor relationship scores
    /// </summary>
    public interface IInvestorScoringService
    {
        Task<RelationshipScore> CalculateScoreAsync(InvestorProfile investor, DateTime asOfDate);
        Task<Dictionary<Guid, RelationshipScore>> CalculateScoresAsync(List<InvestorProfile> investors, DateTime asOfDate);
        Task<RelationshipScore> RecalculateScoreAsync(Guid investorId, DateTime asOfDate);
    }
}
