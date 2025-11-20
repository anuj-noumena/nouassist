using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos;
using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCoverage
{
    /// <summary>
    /// Application service interface for test coverage analysis
    /// </summary>
    public interface ITestCoverageAppService : IApplicationService
    {
        /// <summary>
        /// Analyzes test coverage for a specific requirement
        /// </summary>
        Task<RequirementTestCoverageDto> AnalyzeRequirementCoverageAsync(Guid requirementId);

        /// <summary>
        /// Analyzes test coverage for multiple requirements
        /// </summary>
        Task<List<RequirementTestCoverageDto>> AnalyzeMultipleRequirementsCoverageAsync(
            List<Guid> requirementIds);

        /// <summary>
        /// Generates a comprehensive test coverage report
        /// </summary>
        Task<TestCoverageReportDto> GenerateCoverageReportAsync(GenerateCoverageReportDto input);

        /// <summary>
        /// Identifies requirements with insufficient test coverage
        /// </summary>
        Task<List<RequirementTestGapDto>> IdentifyTestGapsAsync(IdentifyTestGapsDto input);
    }
}
