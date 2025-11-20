using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using NouAssist.RequirementsManagement.ValidationVerification.Services;
using NouAssist.RequirementsManagement.ValidationVerification.Permissions;
using NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCoverage
{
    /// <summary>
    /// Application service for test coverage analysis
    /// </summary>
    [Authorize(ValidationVerificationPermissions.TestCoverage.Default)]
    public class TestCoverageAppService : ApplicationService, ITestCoverageAppService
    {
        private readonly ITestCoverageAnalyzer _testCoverageAnalyzer;
        private readonly IClock _clock;

        public TestCoverageAppService(
            ITestCoverageAnalyzer testCoverageAnalyzer,
            IClock clock)
        {
            _testCoverageAnalyzer = testCoverageAnalyzer;
            _clock = clock;

            LocalizationResource = typeof(ValidationVerificationResource);
        }

        [Authorize(ValidationVerificationPermissions.TestCoverage.Analyze)]
        public virtual async Task<RequirementTestCoverageDto> AnalyzeRequirementCoverageAsync(Guid requirementId)
        {
            try
            {
                var coverage = await _testCoverageAnalyzer.AnalyzeRequirementCoverageAsync(requirementId);
                return ObjectMapper.Map<RequirementTestCoverage, RequirementTestCoverageDto>(coverage);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCoverage.Analyze)]
        public virtual async Task<List<RequirementTestCoverageDto>> AnalyzeMultipleRequirementsCoverageAsync(
            List<Guid> requirementIds)
        {
            try
            {
                var coverages = await _testCoverageAnalyzer.AnalyzeMultipleRequirementsCoverageAsync(requirementIds);
                return ObjectMapper.Map<List<RequirementTestCoverage>, List<RequirementTestCoverageDto>>(coverages);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCoverage.GenerateReports)]
        public virtual async Task<TestCoverageReportDto> GenerateCoverageReportAsync(GenerateCoverageReportDto input)
        {
            try
            {
                var currentTime = _clock.Now;
                var report = await _testCoverageAnalyzer.GenerateCoverageReportAsync(
                    input.RequirementIds,
                    currentTime,
                    input.ReportScope
                );

                return ObjectMapper.Map<TestCoverageReport, TestCoverageReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ValidationVerificationPermissions.TestCoverage.Analyze)]
        public virtual async Task<List<RequirementTestGapDto>> IdentifyTestGapsAsync(IdentifyTestGapsDto input)
        {
            try
            {
                var gaps = await _testCoverageAnalyzer.IdentifyTestGapsAsync(
                    input.RequirementIds,
                    input.MinimumCoverageThreshold
                );

                return ObjectMapper.Map<List<RequirementTestGap>, List<RequirementTestGapDto>>(gaps);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in TestCoverageAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
