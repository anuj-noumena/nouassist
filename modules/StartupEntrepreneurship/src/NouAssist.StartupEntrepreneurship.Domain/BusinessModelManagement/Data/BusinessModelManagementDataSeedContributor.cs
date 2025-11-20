namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Data
{
    /// <summary>
    /// Data seed contributor for Business Model Management module
    /// </summary>
    public class BusinessModelManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IBusinessModelCanvasRepository _canvasRepository;
        private readonly IRevenueModelRepository _revenueModelRepository;
        private readonly IStrategicPivotRepository _pivotRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<BusinessModelManagementDataSeedContributor> _logger;

        public BusinessModelManagementDataSeedContributor(
            IBusinessModelCanvasRepository canvasRepository,
            IRevenueModelRepository revenueModelRepository,
            IStrategicPivotRepository pivotRepository,
            ICurrentTenant currentTenant,
            ILogger<BusinessModelManagementDataSeedContributor> logger)
        {
            _canvasRepository = canvasRepository;
            _revenueModelRepository = revenueModelRepository;
            _pivotRepository = pivotRepository;
            _currentTenant = currentTenant;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {
                await SeedBusinessModelCanvasesAsync();
                await SeedRevenueModelsAsync();
                await SeedStrategicPivotsAsync();

                _logger.LogInformation("Business Model Management sample data seeded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed Business Model Management sample data");
                throw;
            }
        }

        private async Task SeedBusinessModelCanvasesAsync()
        {
            if (await _canvasRepository.CountAsync() > 0)
                return;

            var sampleUserId = Guid.NewGuid();
            var creationTime = DateTime.UtcNow.AddDays(-30);

            // Sample Canvas 1: SaaS Startup
            var saasCanvas = BusinessModelCanvas.Create(
                Guid.NewGuid(),
                "SaaS Project Management Tool",
                "A cloud-based project management solution for small to medium businesses",
                sampleUserId,
                _currentTenant.Id,
                creationTime
            );

            // Add sample content to components
            saasCanvas.UpdateComponent(
                CanvasComponentType.ValuePropositions,
                new ComponentContent(
                    "Streamlined project management with real-time collaboration",
                    new List<string>
                    {
                        "Easy task assignment and tracking",
                        "Real-time team collaboration features",
                        "Automated reporting and analytics",
                        "Integration with popular tools"
                    }
                ),
                sampleUserId,
                creationTime
            );

            saasCanvas.UpdateComponent(
                CanvasComponentType.CustomerSegments,
                new ComponentContent(
                    "Small to medium businesses needing project management solutions",
                    new List<string>
                    {
                        "Tech startups (10-50 employees)",
                        "Marketing agencies",
                        "Freelance project managers",
                        "Remote teams"
                    }
                ),
                sampleUserId,
                creationTime
            );

            await _canvasRepository.InsertAsync(saasCanvas);

            // Sample Canvas 2: E-commerce Platform
            var ecommerceCanvas = BusinessModelCanvas.Create(
                Guid.NewGuid(),
                "Sustainable Fashion E-commerce",
                "An online marketplace for sustainable and eco-friendly fashion brands",
                sampleUserId,
                _currentTenant.Id,
                creationTime.AddDays(5)
            );

            await _canvasRepository.InsertAsync(ecommerceCanvas);
        }

        private async Task SeedRevenueModelsAsync()
        {
            if (await _revenueModelRepository.CountAsync() > 0)
                return;

            var sampleUserId = Guid.NewGuid();

            // Sample Revenue Model 1: Subscription SaaS
            var subscriptionModel = RevenueModel.Create(
                Guid.NewGuid(),
                "SaaS Subscription Model",
                "Multi-tier subscription model for project management SaaS",
                RevenueModelType.Subscription,
                sampleUserId,
                _currentTenant.Id
            );

            // Add pricing models
            var freemiumPricing = subscriptionModel.AddPricingModel(
                "Freemium Tiers",
                PricingStrategy.ValueBased,
                "Free tier with premium upgrades"
            );

            freemiumPricing.AddPricingTier(new PricingTier(
                "Free",
                0m,
                "USD",
                "Monthly",
                new List<string> { "Up to 3 projects", "5 team members", "Basic support" },
                5
            ));

            freemiumPricing.AddPricingTier(new PricingTier(
                "Pro",
                29m,
                "USD",
                "Monthly",
                new List<string> { "Unlimited projects", "25 team members", "Advanced analytics", "Priority support" },
                25
            ));

            freemiumPricing.AddPricingTier(new PricingTier(
                "Enterprise",
                99m,
                "USD",
                "Monthly",
                new List<string> { "Everything in Pro", "Unlimited team members", "Custom integrations", "Dedicated support" },
                null
            ));

            // Add revenue scenario
            var optimisticScenario = subscriptionModel.CreateScenario(
                "Optimistic Growth",
                "Aggressive growth scenario with 15% monthly growth",
                new ScenarioParameters(
                    customerCount: 100,
                    averageRevenuePerUser: 45m,
                    churnRate: 0.05,
                    growthRate: 0.15,
                    timeHorizonMonths: 24,
                    costPerCustomer: 15m
                ),
                sampleUserId,
                DateTime.UtcNow
            );

            await _revenueModelRepository.InsertAsync(subscriptionModel);

            // Sample Revenue Model 2: Marketplace Commission
            var marketplaceModel = RevenueModel.Create(
                Guid.NewGuid(),
                "Fashion Marketplace Commission",
                "Commission-based revenue from sustainable fashion sales",
                RevenueModelType.Commission,
                sampleUserId,
                _currentTenant.Id
            );

            var commissionPricing = marketplaceModel.AddPricingModel(
                "Commission Structure",
                PricingStrategy.ValueBased,
                "Tiered commission based on sales volume"
            );

            await _revenueModelRepository.InsertAsync(marketplaceModel);
        }

        private async Task SeedStrategicPivotsAsync()
        {
            if (await _pivotRepository.CountAsync() > 0)
                return;

            var sampleUserId = Guid.NewGuid();
            var initiationDate = DateTime.UtcNow.AddDays(-60);

            // Sample Pivot 1: Customer Segment Pivot
            var customerSegmentPivot = StrategicPivot.Create(
                Guid.NewGuid(),
                PivotType.CustomerSegment,
                new PivotRationale(
                    "Initial target market too broad, need to focus on specific niche",
                    "Increase customer acquisition rate by 300% and improve product-market fit",
                    new List<string>
                    {
                        "High churn rate in enterprise segment",
                        "SMBs show higher engagement and lower churn",
                        "Feature requests align better with SMB needs"
                    },
                    new List<string>
                    {
                        "Enterprise churn rate: 35%",
                        "SMB churn rate: 12%",
                        "SMB customer satisfaction: 8.5/10 vs Enterprise: 6.2/10"
                    },
                    "Market is too competitive in enterprise space",
                    new Dictionary<string, string>
                    {
                        ["Customer Acquisition Rate"] = "Increase from 50 to 150 customers/month",
                        ["Churn Rate"] = "Reduce from 25% to 12%",
                        ["Customer Satisfaction"] = "Increase from 6.5/10 to 8.5/10"
                    }
                ),
                "Pivot from targeting large enterprises to focusing exclusively on SMBs (10-50 employees)",
                sampleUserId,
                initiationDate,
                initiationDate.AddDays(90),
                _currentTenant.Id
            );

            // Start the pivot implementation
            customerSegmentPivot.StartImplementation();

            // Define impact metrics
            customerSegmentPivot.DefineImpactMetric(
                "Customer Acquisition Rate",
                "Number of new customers acquired per month",
                "customers/month",
                "Monthly tracking of new signups",
                50, // baseline
                150 // target
            );

            customerSegmentPivot.DefineImpactMetric(
                "Monthly Churn Rate",
                "Percentage of customers churning per month",
                "percentage",
                "Monthly cohort analysis",
                25, // baseline
                12 // target
            );

            await _pivotRepository.InsertAsync(customerSegmentPivot);

            // Sample Pivot 2: Revenue Model Pivot
            var revenueModelPivot = StrategicPivot.Create(
                Guid.NewGuid(),
                PivotType.Revenue,
                new PivotRationale(
                    "One-time payment model limiting scalability and predictable revenue",
                    "Transition to subscription model to increase LTV and revenue predictability",
                    new List<string>
                    {
                        "Cash flow unpredictability with one-time payments",
                        "Customer lifetime value too low",
                        "Competitors moving to subscription models"
                    },
                    new List<string>
                    {
                        "Average LTV: $180 (one-time)",
                        "Projected LTV with subscription: $720",
                        "Monthly revenue variance: 45%"
                    },
                    riskAssessment: "Risk of customer resistance to subscription model",
                    successMetrics: new Dictionary<string, string>
                    {
                        ["Monthly Recurring Revenue"] = "Achieve $50K MRR within 6 months",
                        ["Customer Lifetime Value"] = "Increase LTV from $180 to $500+",
                        ["Revenue Predictability"] = "Reduce monthly variance to <15%"
                    }
                ),
                "Transition from one-time payment to subscription-based pricing model",
                sampleUserId,
                initiationDate.AddDays(30),
                initiationDate.AddDays(180),
                _currentTenant.Id
            );

            await _pivotRepository.InsertAsync(revenueModelPivot);
        }
    }
}
