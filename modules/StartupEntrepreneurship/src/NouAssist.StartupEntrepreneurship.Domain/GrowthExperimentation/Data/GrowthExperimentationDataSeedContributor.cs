namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Data
{
    /// <summary>
    /// Data seed contributor for GrowthExperimentation module
    /// </summary>
    public class GrowthExperimentationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly IMarketingChannelPerformanceRepository _channelRepository;
        private readonly IConversionFunnelDefinitionRepository _funnelRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<GrowthExperimentationDataSeedContributor> _logger;

        public GrowthExperimentationDataSeedContributor(
            IGrowthExperimentRepository experimentRepository,
            IMarketingChannelPerformanceRepository channelRepository,
            IConversionFunnelDefinitionRepository funnelRepository,
            IGuidGenerator guidGenerator,
            ILogger<GrowthExperimentationDataSeedContributor> logger)
        {
            _experimentRepository = experimentRepository;
            _channelRepository = channelRepository;
            _funnelRepository = funnelRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {
                _logger.LogInformation("Seeding GrowthExperimentation data...");

                // Check if we already have experiments
                if (await _experimentRepository.GetCountAsync() > 0)
                {
                    _logger.LogInformation("GrowthExperimentation data already seeded.");
                    return;
                }

                // Seed sample experiment
                await SeedSampleExperimentAsync();

                // Seed sample marketing channel
                await SeedSampleMarketingChannelAsync();

                // Seed sample conversion funnel
                await SeedSampleConversionFunnelAsync();

                _logger.LogInformation("GrowthExperimentation data seeding completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding GrowthExperimentation data.");
                throw;
            }
        }

        private async Task SeedSampleExperimentAsync()
        {
            // Create experiment
            var experimentGoal = new ExperimentGoal(
                "Increase sign-up conversion rate",
                15.0, // Expected 15% improvement
                5.0   // Minimum viable improvement 5%
            );

            var experiment = GrowthExperiment.Create(
                _guidGenerator.Create(),
                "Homepage Sign-up Button Test",
                "Testing different button colors and text to improve sign-up conversion rate",
                experimentGoal);

            // Add metrics
            experiment.AddTargetMetric(new TargetMetric(
                "Sign-up Conversion Rate",
                MetricType.ConversionRate,
                0.05, // 5% baseline
                0.0575, // 5.75% target (15% improvement)
                true // Primary metric
            ));

            experiment.AddTargetMetric(new TargetMetric(
                "Button Click-Through Rate",
                MetricType.ClickThroughRate,
                0.12, // 12% baseline
                0.138, // 13.8% target
                false // Secondary metric
            ));

            // Add hypothesis
            experiment.AddHypothesis(
                "Changing the button color from blue to green will increase sign-up conversion rate",
                "Green is associated with positive action and may create a stronger visual contrast on our page"
            );

            // Add variants
            var variantSetupControl = new VariantSetup(0.5, "{\"color\":\"blue\",\"text\":\"Sign Up Now\"}", "/signup?variant=control");
            experiment.AddVariant(
                _guidGenerator.Create(),
                "Control",
                "Current blue button with 'Sign Up Now' text",
                VariantType.Control,
                variantSetupControl
            );

            var variantSetupTreatment = new VariantSetup(0.5, "{\"color\":\"green\",\"text\":\"Sign Up Now\"}", "/signup?variant=treatment");
            experiment.AddVariant(
                _guidGenerator.Create(),
                "Green Button",
                "Green button with 'Sign Up Now' text",
                VariantType.Treatment,
                variantSetupTreatment
            );

            // Mark as ready
            experiment.SetReady();

            await _experimentRepository.InsertAsync(experiment);
        }

        private async Task SeedSampleMarketingChannelAsync()
        {
            // Create marketing channel
            var channelDetails = new ChannelDetails(
                "Facebook",
                "12345",
                "https://facebook.com/ads",
                "{\"adAccountId\":\"12345\",\"campaignType\":\"conversion\"}"
            );

            var channel = MarketingChannelPerformance.Create(
                _guidGenerator.Create(),
                "Facebook Ads",
                "Main Facebook advertising channel",
                MarketingChannelType.SocialMedia,
                channelDetails);

            // Add metrics
            channel.AddMetric(
                _guidGenerator.Create(),
                "Click-Through Rate",
                MetricType.ClickThroughRate,
                0.028, // 2.8% initial
                0.035, // 3.5% target
                DateTime.UtcNow
            );

            channel.AddMetric(
                _guidGenerator.Create(),
                "Cost Per Click",
                MetricType.CostPerAcquisition,
                0.52, // $0.52 initial
                0.45, // $0.45 target
                DateTime.UtcNow
            );

            // Add cost per acquisition data
            var cpa = new CostPerAcquisition(
                1250.00m, // $1,250 cost
                25,       // 25 acquisitions
                DateTime.UtcNow.AddDays(-30),
                DateTime.UtcNow
            );
            channel.UpdateCostPerAcquisition(cpa);

            // Add ROAS data
            var roas = new ReturnOnAdSpend(
                1250.00m, // $1,250 ad spend
                3750.00m, // $3,750 revenue
                DateTime.UtcNow.AddDays(-30),
                DateTime.UtcNow
            );
            channel.UpdateReturnOnAdSpend(roas);

            await _channelRepository.InsertAsync(channel);
        }

        private async Task SeedSampleConversionFunnelAsync()
        {
            // Create conversion funnel
            var funnel = ConversionFunnelDefinition.Create(
                _guidGenerator.Create(),
                "Main Sign-up Funnel",
                "Primary user sign-up flow from landing page to completed registration");

            // Add stages
            var stage1 = funnel.AddStage(
                _guidGenerator.Create(),
                new StageDefinition(
                    "Visit Landing Page",
                    "User lands on the homepage or landing page",
                    1,
                    "event = 'page_view' AND page = 'landing'"
                ),
                false
            );

            var stage2 = funnel.AddStage(
                _guidGenerator.Create(),
                new StageDefinition(
                    "View Sign-up Form",
                    "User clicks sign-up button and views the registration form",
                    2,
                    "event = 'page_view' AND page = 'signup'"
                ),
                false
            );

            var stage3 = funnel.AddStage(
                _guidGenerator.Create(),
                new StageDefinition(
                    "Complete Registration",
                    "User submits the registration form successfully",
                    3,
                    "event = 'signup_complete'"
                ),
                true // Exit stage
            );

            // Add conversion rate data
            funnel.UpdateStageConversionRate(
                stage1.Id,
                new ConversionRatePerStage(
                    stage1.Id,
                    1000, // 1000 entered
                    250,  // 250 completed
                    DateTime.UtcNow
                )
            );

            funnel.UpdateStageConversionRate(
                stage2.Id,
                new ConversionRatePerStage(
                    stage2.Id,
                    250, // 250 entered
                    100, // 100 completed
                    DateTime.UtcNow
                )
            );

            funnel.UpdateStageConversionRate(
                stage3.Id,
                new ConversionRatePerStage(
                    stage3.Id,
                    100, // 100 entered
                    100, // 100 completed (all who reach this stage complete it)
                    DateTime.UtcNow
                )
            );

            // Validate funnel
            funnel.Validate();

            await _funnelRepository.InsertAsync(funnel);
        }
    }
}
