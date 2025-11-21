using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services
{
    /// <summary>
    /// Domain service interface for generating investor updates
    /// </summary>
    public interface IUpdateGenerationService
    {
        Task<InvestorUpdate> GenerateUpdateAsync(string templateId, Dictionary<string, object> data, DateTime scheduledDate);
        Task<InvestorUpdate> GenerateMetricsUpdateAsync(List<MetricValue> metrics, DateTime scheduledDate);
        Task<List<InvestorUpdate>> GenerateScheduledUpdatesAsync(DateTime forDate);
    }
}
