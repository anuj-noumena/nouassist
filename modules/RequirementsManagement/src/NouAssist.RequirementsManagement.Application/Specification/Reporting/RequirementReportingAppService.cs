using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Permissions;
using NouAssist.RequirementsManagement.Specification.Repositories;
using NouAssist.RequirementsManagement.Specification.Specifications;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.Specification.Reporting
{
    [Authorize(RequirementSpecificationManagementPermissions.Reporting.Default)]
    public class RequirementReportingAppService : ApplicationService, IRequirementReportingAppService
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;

        public RequirementReportingAppService(
            IRequirementRepository requirementRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant)
        {
            _requirementRepository = requirementRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;

            LocalizationResource = typeof(RequirementSpecificationManagementResource);
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<RequirementStatisticsDto> GetStatisticsAsync()
        {
            try
            {
                var query = await _requirementRepository.GetQueryableAsync();
                var total = await AsyncExecuter.CountAsync(query);

                if (total == 0)
                {
                    return new RequirementStatisticsDto();
                }

                var inProgressStatuses = new[] 
                { 
                    RequirementStatus.Draft, 
                    RequirementStatus.UnderReview, 
                    RequirementStatus.Approved, 
                    RequirementStatus.Implemented 
                };

                var completedStatuses = new[] 
                { 
                    RequirementStatus.Tested 
                };

                var inProgress = await AsyncExecuter.CountAsync(
                    query.Where(r => inProgressStatuses.Contains(r.Status))
                );

                var completed = await AsyncExecuter.CountAsync(
                    query.Where(r => completedStatuses.Contains(r.Status))
                );

                var withAcceptanceCriteria = await AsyncExecuter.CountAsync(
                    query.Where(r => r.AcceptanceCriteria.Any())
                );

                var withBusinessObjectives = await AsyncExecuter.CountAsync(
                    query.Where(r => r.BusinessObjectiveLinks.Any())
                );

                return new RequirementStatisticsDto
                {
                    Total = total,
                    InProgress = inProgress,
                    Completed = completed,
                    WithAcceptanceCriteria = withAcceptanceCriteria,
                    WithBusinessObjectives = withBusinessObjectives,
                    CompletionRate = total > 0 ? (double)completed / total * 100 : 0
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<List<RequirementCountByStatusDto>> GetCountByStatusAsync()
        {
            try
            {
                var query = await _requirementRepository.GetQueryableAsync();
                var total = await AsyncExecuter.CountAsync(query);

                if (total == 0)
                {
                    return new List<RequirementCountByStatusDto>();
                }

                var statusCounts = await AsyncExecuter.ToListAsync(
                    query.GroupBy(r => r.Status)
                         .Select(g => new { Status = g.Key, Count = g.Count() })
                );

                return statusCounts.Select(sc => new RequirementCountByStatusDto
                {
                    Status = sc.Status.ToString(),
                    Count = sc.Count,
                    Percentage = (double)sc.Count / total * 100
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<List<RequirementCountByTypeDto>> GetCountByTypeAsync()
        {
            try
            {
                var query = await _requirementRepository.GetQueryableAsync();
                var total = await AsyncExecuter.CountAsync(query);

                if (total == 0)
                {
                    return new List<RequirementCountByTypeDto>();
                }

                var typeCounts = await AsyncExecuter.ToListAsync(
                    query.GroupBy(r => r.Type)
                         .Select(g => new { Type = g.Key, Count = g.Count() })
                );

                return typeCounts.Select(tc => new RequirementCountByTypeDto
                {
                    Type = tc.Type.ToString(),
                    Count = tc.Count,
                    Percentage = (double)tc.Count / total * 100
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<List<RequirementCountByPriorityDto>> GetCountByPriorityAsync()
        {
            try
            {
                var query = await _requirementRepository.GetQueryableAsync();
                var total = await AsyncExecuter.CountAsync(query);

                if (total == 0)
                {
                    return new List<RequirementCountByPriorityDto>();
                }

                var priorityCounts = await AsyncExecuter.ToListAsync(
                    query.GroupBy(r => r.Priority)
                         .Select(g => new { Priority = g.Key, Count = g.Count() })
                );

                return priorityCounts.Select(pc => new RequirementCountByPriorityDto
                {
                    Priority = pc.Priority.ToString(),
                    Count = pc.Count,
                    Percentage = (double)pc.Count / total * 100
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<List<RequirementTrendDto>> GetTrendDataAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var query = await _requirementRepository.GetQueryableAsync();
                
                // Group by date and count created/updated/completed
                var trendData = new List<RequirementTrendDto>();
                var currentDate = fromDate.Date;

                while (currentDate <= toDate.Date)
                {
                    var nextDate = currentDate.AddDays(1);

                    var created = await AsyncExecuter.CountAsync(
                        query.Where(r => r.CreationTime >= currentDate && r.CreationTime < nextDate)
                    );

                    var updated = await AsyncExecuter.CountAsync(
                        query.Where(r => r.LastModificationTime >= currentDate && 
                                        r.LastModificationTime < nextDate &&
                                        r.CreationTime < currentDate)
                    );

                    var completed = await AsyncExecuter.CountAsync(
                        query.Where(r => r.Status == RequirementStatus.Tested &&
                                        r.LastModificationTime >= currentDate && 
                                        r.LastModificationTime < nextDate)
                    );

                    trendData.Add(new RequirementTrendDto
                    {
                        Date = currentDate,
                        Created = created,
                        Updated = updated,
                        Completed = completed
                    });

                    currentDate = nextDate;
                }

                return trendData;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ViewReports)]
        public virtual async Task<List<RequirementComplianceDto>> GetComplianceReportAsync()
        {
            try
            {
                var requirements = await _requirementRepository.GetListAsync(
                    new RequirementByStatusListSpecification(
                        RequirementStatus.Approved,
                        RequirementStatus.Implemented,
                        RequirementStatus.Tested
                    )
                );

                return requirements.Select(r => new RequirementComplianceDto
                {
                    RequirementId = r.Identifier.Value,
                    Title = r.Text.Title,
                    HasAcceptanceCriteria = r.AcceptanceCriteria.Any(),
                    HasBusinessObjective = r.BusinessObjectiveLinks.Any(),
                    HasConstraints = r.AssociatedConstraints.Any(),
                    ComplianceScore = CalculateComplianceScore(r)
                }).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ExportData)]
        public virtual async Task<byte[]> GenerateReportAsync(string reportType, string format = "pdf")
        {
            try
            {
                return reportType.ToLowerInvariant() switch
                {
                    "statistics" => await GenerateStatisticsReportAsync(format),
                    "compliance" => await GenerateComplianceReportAsync(format),
                    "trend" => await GenerateTrendReportAsync(format),
                    _ => throw new ArgumentException($"Unsupported report type: {reportType}")
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper methods
        private double CalculateComplianceScore(Aggregates.Requirement requirement)
        {
            var score = 0.0;
            var maxScore = 4.0;

            // Base score for having the requirement
            score += 1.0;

            // Score for having acceptance criteria
            if (requirement.AcceptanceCriteria.Any())
                score += 1.0;

            // Score for having business objective links
            if (requirement.BusinessObjectiveLinks.Any())
                score += 1.0;

            // Score for having constraints or rules
            if (requirement.AssociatedConstraints.Any() || requirement.AssociatedRules.Any())
                score += 1.0;

            return (score / maxScore) * 100;
        }

        private async Task<byte[]> GenerateStatisticsReportAsync(string format)
        {
            var statistics = await GetStatisticsAsync();
            
            // This would typically use a reporting library
            // For now, return empty byte array
            Logger.LogInformation("Generating statistics report in {Format} format", format);
            return Array.Empty<byte>();
        }

        private async Task<byte[]> GenerateComplianceReportAsync(string format)
        {
            var compliance = await GetComplianceReportAsync();
            
            // This would typically use a reporting library
            // For now, return empty byte array
            Logger.LogInformation("Generating compliance report in {Format} format", format);
            return Array.Empty<byte>();
        }

        private async Task<byte[]> GenerateTrendReportAsync(string format)
        {
            var trends = await GetTrendDataAsync(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
            
            // This would typically use a reporting library
            // For now, return empty byte array
            Logger.LogInformation("Generating trend report in {Format} format", format);
            return Array.Empty<byte>();
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception,
                "Error in RequirementReportingAppService: {ErrorMessage}. User: {UserId}, Tenant: {TenantId}",
                exception.Message,
                _currentUser.Id,
                _currentTenant.Id);

            await Task.CompletedTask;
        }
    }
}
