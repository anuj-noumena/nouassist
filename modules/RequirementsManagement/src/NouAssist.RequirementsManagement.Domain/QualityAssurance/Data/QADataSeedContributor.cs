namespace NouAssist.RequirementsManagement.QualityAssurance.Data
{
    /// <summary>
    /// Seeds initial data for the QA module
    /// </summary>
    public class QADataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ITestPlanRepository _testPlanRepository;
        private readonly ITestPlanManager _testPlanManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public QADataSeedContributor(
            ITestPlanRepository testPlanRepository,
            ITestPlanManager testPlanManager,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _testPlanRepository = testPlanRepository;
            _testPlanManager = testPlanManager;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Check if we already have data
            if (await _testPlanRepository.GetCountAsync() > 0)
            {
                return;
            }

            // Create sample test plan
            await _testPlanManager.CreateAsync(
                "Initial Release Test Plan",
                "Test plan for the initial release of the application",
                "1.0.0",
                new TestingStrategy(
                    "Comprehensive",
                    "Full functional testing of all core features",
                    "Performance testing will be handled separately"),
                new EnvironmentConfiguration(
                    "UAT",
                    "User Acceptance Testing Environment",
                    "https://uat.example.com"),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(14));
        }
    }
}
