using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.ValidationVerification.Services
{
    /// <summary>
    /// Interface for test coverage analysis domain service
    /// </summary>
    public interface ITestCoverageAnalyzer : IDomainService
    {
        /// <summary>
        /// Analyzes test coverage for a specific requirement
        /// </summary>
        Task<RequirementTestCoverage> AnalyzeRequirementCoverageAsync(Guid requirementId);

        /// <summary>
        /// Analyzes test coverage for multiple requirements
        /// </summary>
        Task<List<RequirementTestCoverage>> AnalyzeMultipleRequirementsCoverageAsync(List<Guid> requirementIds);

        /// <summary>
        /// Generates a comprehensive test coverage report
        /// </summary>
        Task<TestCoverageReport> GenerateCoverageReportAsync(
            List<Guid> requirementIds,
            DateTime reportDate,
            string reportScope = null);

        /// <summary>
        /// Identifies requirements with insufficient test coverage
        /// </summary>
        Task<List<RequirementTestGap>> IdentifyTestGapsAsync(
            List<Guid> requirementIds,
            double minimumCoverageThreshold = 0.8);
    }
}
