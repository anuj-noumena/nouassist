using System;
using System.Threading.Tasks;
using NouAssist.CreditScoreMonitoring.CreditReports.Dtos;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CreditScoreMonitoring.CreditReports
{
    public interface ICreditReportAppService : IApplicationService
    {
        Task<CreditReportDto> GetAsync(Guid id);
        Task<PagedResultDto<CreditReportDto>> GetListAsync(GetCreditReportListDto input);
        Task<CreditReportDto> GetLatestReportAsync(CreditBureau bureau);
        Task<CreditReportDto> ImportReportAsync(ImportCreditReportDto input);
        Task<CreditScoreHistoryDto> GetScoreHistoryAsync(CreditBureau bureau, int months);
        Task<CreditHealthReportDto> GetCreditHealthAnalysisAsync(Guid reportId);
        Task<ScoreProjectionDto> ProjectScoreImprovementAsync(Guid reportId, CreditImprovementPlanDto plan);
    }
}
