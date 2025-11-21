using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;

namespace NouAssist.RequirementsManagement.ValidationVerification.Data
{
    /// <summary>
    /// Data seed contributor for ValidationVerification module
    /// </summary>
    public class ValidationVerificationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRequirementReviewCycleRepository _reviewCycleRepository;
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ILogger<ValidationVerificationDataSeedContributor> _logger;

        public ValidationVerificationDataSeedContributor(
            IRequirementReviewCycleRepository reviewCycleRepository,
            ITestCaseRepository testCaseRepository,
            ILogger<ValidationVerificationDataSeedContributor> logger)
        {
            _reviewCycleRepository = reviewCycleRepository;
            _testCaseRepository = testCaseRepository;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            _logger.LogInformation("Seeding ValidationVerification module data...");

            await SeedSampleWorkflowDefinitionsAsync(context);
            await SeedSampleTestCasesAsync(context);

            _logger.LogInformation("ValidationVerification module data seeding completed");
        }

        private async Task SeedSampleWorkflowDefinitionsAsync(DataSeedContext context)
        {
            // For demo purposes, create some sample workflow definitions
            // In real implementation, these would come from a workflow engine
            
            var simpleWorkflow = new WorkflowDefinitionRef(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Simple Review",
                "1.0"
            );

            var comprehensiveWorkflow = new WorkflowDefinitionRef(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Comprehensive Review",
                "1.0"
            );

            _logger.LogInformation("Sample workflow definitions prepared for seeding");
        }

        private async Task SeedSampleTestCasesAsync(DataSeedContext context)
        {
            // Check if we already have test cases
            var existingTestCases = await _testCaseRepository.GetListAsync();
            if (existingTestCases.Any())
            {
                _logger.LogInformation("Test cases already exist, skipping seeding");
                return;
            }

            // Create sample test cases for demonstration
            var sampleTestCases = new List<TestCase>
            {
                CreateSampleTestCase(
                    "Login Functionality Test",
                    "Verify that users can successfully log into the system",
                    TestCasePriority.High,
                    new[] { Guid.Parse("33333333-3333-3333-3333-333333333333") }
                ),
                CreateSampleTestCase(
                    "Password Reset Test",
                    "Verify that users can reset their passwords via email",
                    TestCasePriority.Medium,
                    new[] { Guid.Parse("44444444-4444-4444-4444-444444444444") }
                ),
                CreateSampleTestCase(
                    "User Permission Test",
                    "Verify that user permissions are enforced correctly",
                    TestCasePriority.Critical,
                    new[] { Guid.Parse("55555555-5555-5555-5555-555555555555") }
                )
            };

            foreach (var testCase in sampleTestCases)
            {
                await _testCaseRepository.InsertAsync(testCase);
                _logger.LogInformation("Seeded test case: {TestCaseName}", testCase.Name);
            }
        }

        private TestCase CreateSampleTestCase(
            string name,
            string description,
            TestCasePriority priority,
            Guid[] requirementIds)
        {
            var testCase = TestCase.Create(
                Guid.NewGuid(),
                name,
                description,
                priority,
                requirementIds,
                "User must have valid credentials",
                "System returns to login page"
            );

            // Add sample test steps
            testCase.AddTestStep(
                "Navigate to login page",
                "Login page is displayed with username and password fields"
            );

            testCase.AddTestStep(
                "Enter valid credentials and click Login",
                "User is successfully logged in and redirected to dashboard"
            );

            testCase.AddTestStep(
                "Verify user session is active",
                "User can access protected resources without re-authentication"
            );

            testCase.MarkAsReady();

            return testCase;
        }
    }
}
