using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Specification.Reporting
{
    /// <summary>
    /// Application service interface for requirement reporting
    /// </summary>
    public interface IRequirementReportingAppService : IApplicationService
    {
        Task<RequirementStatisticsDto> GetStatisticsAsync();
        Task<List<RequirementCountByStatusDto>> GetCountByStatusAsync();
        Task<List<RequirementCountByTypeDto>> GetCountByTypeAsync();
        Task<List<RequirementCountByPriorityDto>> GetCountByPriorityAsync();
        Task<List<RequirementTrendDto>> GetTrendDataAsync(DateTime fromDate, DateTime toDate);
        Task<List<RequirementComplianceDto>> GetComplianceReportAsync();
        Task<byte[]> GenerateReportAsync(string reportType, string format = "pdf");
    }

    public class RequirementStatisticsDto
    {
        public int Total { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int WithAcceptanceCriteria { get; set; }
        public int WithBusinessObjectives { get; set; }
        public double CompletionRate { get; set; }
    }

    public class RequirementCountByStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class RequirementCountByTypeDto
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class RequirementCountByPriorityDto
    {
        public string Priority { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class RequirementTrendDto
    {
        public DateTime Date { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Completed { get; set; }
    }

    public class RequirementComplianceDto
    {
        public string RequirementId { get; set; }
        public string Title { get; set; }
        public bool HasAcceptanceCriteria { get; set; }
        public bool HasBusinessObjective { get; set; }
        public bool HasConstraints { get; set; }
        public double ComplianceScore { get; set; }
    }
}
