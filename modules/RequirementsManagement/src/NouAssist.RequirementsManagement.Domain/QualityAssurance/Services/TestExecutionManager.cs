using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Implementation of test execution management domain service
    /// </summary>
    public class TestExecutionManager : DomainService, ITestExecutionManager
    {
        private readonly ITestPlanRepository _testPlanRepository;
        private readonly ITestCycleRepository _testCycleRepository;
        private readonly ILogger<TestExecutionManager> _logger;

        public TestExecutionManager(
            ITestPlanRepository testPlanRepository,
            ITestCycleRepository testCycleRepository,
            ILogger<TestExecutionManager> logger)
        {
            _testPlanRepository = testPlanRepository;
            _testCycleRepository = testCycleRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TestCycle> CreateTestCycleAsync(
            Guid testPlanId,
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                var testCycle = testPlan.AddTestCycle(
                    name,
                    description,
                    buildInformation,
                    startDate,
                    endDate,
                    assigneeId);
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Created test cycle {TestCycleId} in test plan {TestPlanId}", 
                    testCycle.Id, testPlanId);
                
                return testCycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create test cycle in test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestCycle> StartTestCycleAsync(Guid testCycleId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(testCycleId);
                
                testCycle.Start(Clock.Now);
                
                await _testCycleRepository.UpdateAsync(testCycle);
                
                _logger.LogInformation("Started test cycle {TestCycleId}", testCycleId);
                
                return testCycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start test cycle {TestCycleId}", testCycleId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestCycle> CompleteTestCycleAsync(Guid testCycleId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                
                testCycle.Complete(Clock.Now);
                
                await _testCycleRepository.UpdateAsync(testCycle);
                
                _logger.LogInformation("Completed test cycle {TestCycleId}", testCycleId);
                
                return testCycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete test cycle {TestCycleId}", testCycleId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestRun> AddTestCaseAsync(
            Guid testCycleId,
            Guid testCaseId,
            string testCaseTitle,
            string expectedOutcome)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetAsync(testCycleId);
                
                var testRun = testCycle.AddTestRun(
                    testCaseId,
                    testCaseTitle,
                    expectedOutcome);
                
                await _testCycleRepository.UpdateAsync(testCycle);
                
                _logger.LogInformation("Added test case {TestCaseId} to test cycle {TestCycleId}", 
                    testCaseId, testCycleId);
                
                return testRun;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add test case {TestCaseId} to test cycle {TestCycleId}", 
                    testCaseId, testCycleId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RecordTestResultAsync(
            Guid testCycleId,
            Guid testCaseId,
            TestRunResult result,
            string actualOutcomeDescription,
            string actualOutcomeLogData = null)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                
                testCycle.RecordTestResult(
                    testCaseId,
                    result,
                    actualOutcomeDescription,
                    actualOutcomeLogData,
                    CurrentUser.Id.Value,
                    Clock.Now);
                
                await _testCycleRepository.UpdateAsync(testCycle);
                
                _logger.LogInformation("Recorded test result {Result} for test case {TestCaseId} in cycle {TestCycleId}", 
                    result, testCaseId, testCycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record test result for test case {TestCaseId} in cycle {TestCycleId}", 
                    testCaseId, testCycleId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<(int Total, int Passed, int Failed, int Blocked, int NotRun)> GetTestPlanProgressAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetWithDetailsAsync(testPlanId);
                
                var progress = testPlan.GetTestExecutionProgress();
                
                return progress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get test execution progress for test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<(int Total, int Passed, int Failed, int Blocked, int NotRun)> GetTestCycleProgressAsync(Guid testCycleId)
        {
            try
            {
                var testCycle = await _testCycleRepository.GetWithDetailsAsync(testCycleId);
                
                var progress = testCycle.GetTestExecutionProgress();
                
                return progress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get test execution progress for test cycle {TestCycleId}", testCycleId);
                throw;
            }
        }
    }
}
