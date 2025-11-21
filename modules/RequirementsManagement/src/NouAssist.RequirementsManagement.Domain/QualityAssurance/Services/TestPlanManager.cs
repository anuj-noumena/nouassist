using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Implementation of test plan management domain service
    /// </summary>
    public class TestPlanManager : DomainService, ITestPlanManager
    {
        private readonly ITestPlanRepository _testPlanRepository;
        private readonly ILogger<TestPlanManager> _logger;

        public TestPlanManager(
            ITestPlanRepository testPlanRepository,
            ILogger<TestPlanManager> logger)
        {
            _testPlanRepository = testPlanRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TestPlan> CreateAsync(
            string name,
            string description,
            string releaseVersion,
            TestingStrategy strategy,
            EnvironmentConfiguration environment,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var testPlan = TestPlan.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    releaseVersion,
                    strategy,
                    environment,
                    startDate,
                    endDate,
                    CurrentTenant.Id);

                await _testPlanRepository.InsertAsync(testPlan);
                
                _logger.LogInformation("Created test plan {TestPlanId} for release {Release}", 
                    testPlan.Id, releaseVersion);
                
                return testPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create test plan {Name} for release {Release}", 
                    name, releaseVersion);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestPlan> ActivateAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                testPlan.Activate();
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Activated test plan {TestPlanId}", testPlanId);
                
                return testPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to activate test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestPlan> CompleteAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetWithDetailsAsync(testPlanId);
                
                testPlan.Complete();
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Completed test plan {TestPlanId}", testPlanId);
                
                return testPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestPlan> CancelAsync(Guid testPlanId, string reason)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                testPlan.Cancel(reason);
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Cancelled test plan {TestPlanId}. Reason: {Reason}", 
                    testPlanId, reason);
                
                return testPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cancel test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TestPlan> ArchiveAsync(Guid testPlanId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                testPlan.Archive();
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Archived test plan {TestPlanId}", testPlanId);
                
                return testPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to archive test plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LinkRequirementAsync(Guid testPlanId, Guid requirementId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                testPlan.AddRequirementToScope(requirementId);
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Linked requirement {RequirementId} to test plan {TestPlanId}", 
                    requirementId, testPlanId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to link requirement {RequirementId} to test plan {TestPlanId}", 
                    requirementId, testPlanId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UnlinkRequirementAsync(Guid testPlanId, Guid requirementId)
        {
            try
            {
                var testPlan = await _testPlanRepository.GetAsync(testPlanId);
                
                testPlan.RemoveRequirementFromScope(requirementId);
                
                await _testPlanRepository.UpdateAsync(testPlan);
                
                _logger.LogInformation("Unlinked requirement {RequirementId} from test plan {TestPlanId}", 
                    requirementId, testPlanId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unlink requirement {RequirementId} from test plan {TestPlanId}", 
                    requirementId, testPlanId);
                throw;
            }
        }
    }
}
