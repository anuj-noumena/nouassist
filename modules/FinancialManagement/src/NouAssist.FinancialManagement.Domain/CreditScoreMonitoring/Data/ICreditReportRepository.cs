using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CreditScoreMonitoring.Aggregates;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CreditScoreMonitoring.Data
{
    public interface ICreditReportRepository : IRepository<CreditReport, Guid>
    {
        Task<CreditReport?> GetWithDetailsAsync(Guid id);
        Task<CreditReport?> GetLatestReportAsync(Guid userId, CreditBureau bureau);
        Task<List<CreditReport>> GetUserReportsAsync(Guid userId);
        Task<List<CreditReport>> GetReportHistoryAsync(Guid userId, CreditBureau bureau, int months);
    }
}
