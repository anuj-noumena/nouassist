namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Data
{
    /// <summary>
    /// Provides seed data for Strategic Planning domain
    /// </summary>
    public class StrategicPlanningDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IStrategicGoalRepository _strategicGoalRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<StrategicPlanningDataSeedContributor> _logger;

        public StrategicPlanningDataSeedContributor(
            IStrategicGoalRepository strategicGoalRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            ILogger<StrategicPlanningDataSeedContributor> logger)
        {
            _strategicGoalRepository = strategicGoalRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {
                using (_currentTenant.Change(context?.TenantId))
                {
                    await SeedStrategicGoalsAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding Strategic Planning data");
                throw;
            }
        }

        private async Task SeedStrategicGoalsAsync()
        {
            if (await _strategicGoalRepository.CountAsync() > 0)
            {
                return;
            }

            var sampleGoals = new List<(string Title, string Description, GoalPriority Priority, PlanningHorizon Horizon)>
            {
                (
                    "Digital Transformation Initiative",
                    "Complete digital transformation of core business processes to improve efficiency and customer experience",
                    GoalPriority.High,
                    PlanningHorizon.H2
                ),
                (
                    "Market Expansion - APAC Region",
                    "Establish presence in Asia-Pacific markets with 20% market share in key segments",
                    GoalPriority.Medium,
                    PlanningHorizon.H3
                ),
                (
                    "Sustainability Compliance",
                    "Achieve carbon neutrality and ESG compliance standards",
                    GoalPriority.Critical,
                    PlanningHorizon.H2
                )
            };

            foreach (var (title, description, priority, horizon) in sampleGoals)
            {
                var goal = StrategicGoalDefinition.Create(
                    _guidGenerator.Create(),
                    title,
                    description,
                    priority,
                    horizon,
                    DateTime.UtcNow.AddYears((int)horizon),
                    tenantId: _currentTenant.Id
                );

                // Add sample targets
                switch (title)
                {
                    case "Digital Transformation Initiative":
                        goal.AddTarget("Process Automation", 85, "Percentage", DateTime.UtcNow.AddMonths(18));
                        goal.AddTarget("Customer Satisfaction", 4.5m, "Rating (1-5)", DateTime.UtcNow.AddMonths(24));
                        break;
                    case "Market Expansion - APAC Region":
                        goal.AddTarget("Market Share", 20, "Percentage", DateTime.UtcNow.AddYears(3));
                        goal.AddTarget("Revenue Growth", 15000000, "USD", DateTime.UtcNow.AddYears(3));
                        break;
                    case "Sustainability Compliance":
                        goal.AddTarget("Carbon Reduction", 50, "Percentage", DateTime.UtcNow.AddYears(2));
                        goal.AddTarget("ESG Score", 80, "Points", DateTime.UtcNow.AddMonths(18));
                        break;
                }

                await _strategicGoalRepository.InsertAsync(goal);
            }

            _logger.LogInformation("Successfully seeded {Count} strategic goals", sampleGoals.Count);
        }
    }
}
