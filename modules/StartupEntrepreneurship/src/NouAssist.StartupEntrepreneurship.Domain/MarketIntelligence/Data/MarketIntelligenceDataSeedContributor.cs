using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Data
{
    /// <summary>
    /// Seeds initial data for the Market Intelligence module
    /// </summary>
    public class MarketIntelligenceDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ICompetitorRepository _competitorRepository;
        private readonly IMarketTrendRepository _marketTrendRepository;
        private readonly ICustomerSegmentProfileRepository _segmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly ILogger<MarketIntelligenceDataSeedContributor> _logger;

        public MarketIntelligenceDataSeedContributor(
            ICompetitorRepository competitorRepository,
            IMarketTrendRepository marketTrendRepository,
            ICustomerSegmentProfileRepository segmentRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IClock clock,
            ILogger<MarketIntelligenceDataSeedContributor> logger)
        {
            _competitorRepository = competitorRepository;
            _marketTrendRepository = marketTrendRepository;
            _segmentRepository = segmentRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _clock = clock;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            try
            {
                await SeedCompetitorsAsync();
                await SeedMarketTrendsAsync();
                await SeedCustomerSegmentsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while seeding market intelligence data");
            }
        }

        private async Task SeedCompetitorsAsync()
        {
            if (await _competitorRepository.GetCountAsync() > 0)
            {
                return;
            }

            _logger.LogInformation("Seeding competitors");

            var seedCompetitors = new List<Competitor>
            {
                Competitor.Create(
                    _guidGenerator.Create(),
                    "Acme Corporation",
                    "A global leader in innovative solutions",
                    new CompetitorInfo(
                        "https://acme.example.com", 
                        "Technology", 
                        "Global", 
                        1500, 
                        50000000, 
                        "Series C"),
                    CompetitorThreatLevel.High,
                    _currentTenant.Id,
                    _clock.Now
                ),
                
                Competitor.Create(
                    _guidGenerator.Create(),
                    "Innovatech Solutions",
                    "Specialized in cutting-edge technology solutions",
                    new CompetitorInfo(
                        "https://innovatech.example.com", 
                        "Software", 
                        "North America", 
                        500, 
                        20000000, 
                        "Series B"),
                    CompetitorThreatLevel.Medium,
                    _currentTenant.Id,
                    _clock.Now
                ),
                
                Competitor.Create(
                    _guidGenerator.Create(),
                    "TechStart Inc.",
                    "Emerging player in the tech ecosystem",
                    new CompetitorInfo(
                        "https://techstart.example.com", 
                        "Technology", 
                        "Europe", 
                        100, 
                        5000000, 
                        "Series A"),
                    CompetitorThreatLevel.Low,
                    _currentTenant.Id,
                    _clock.Now
                )
            };

            foreach (var competitor in seedCompetitors)
            {
                await _competitorRepository.InsertAsync(competitor);
            }

            _logger.LogInformation("Seeded {Count} competitors", seedCompetitors.Count);
        }

        private async Task SeedMarketTrendsAsync()
        {
            if (await _marketTrendRepository.GetCountAsync() > 0)
            {
                return;
            }

            _logger.LogInformation("Seeding market trends");

            var seedTrends = new List<MarketTrend>
            {
                MarketTrend.Create(
                    _guidGenerator.Create(),
                    new TrendDescription(
                        "Rise of AI-Powered Analytics",
                        "Increasing adoption of AI for business analytics across industries",
                        "AI, analytics, business intelligence, machine learning",
                        "https://example.com/trends/ai-analytics"),
                    TrendCategory.Technology,
                    new ImpactAssessment(
                        ImpactLevel.High,
                        "Major impact on how businesses process and analyze data",
                        0.85m,
                        "Develop AI capabilities or partner with AI solution providers"),
                    _clock.Now,
                    _clock.Now.AddYears(1),
                    _currentTenant.Id
                ),
                
                MarketTrend.Create(
                    _guidGenerator.Create(),
                    new TrendDescription(
                        "Sustainability in Business Operations",
                        "Growing focus on environmental sustainability in business practices",
                        "sustainability, green business, ESG, corporate responsibility",
                        "https://example.com/trends/sustainability"),
                    TrendCategory.Social,
                    new ImpactAssessment(
                        ImpactLevel.Medium,
                        "Increasing consumer preference for sustainable businesses",
                        0.75m,
                        "Evaluate sustainability initiatives and messaging"),
                    _clock.Now,
                    _clock.Now.AddYears(2),
                    _currentTenant.Id
                ),
                
                MarketTrend.Create(
                    _guidGenerator.Create(),
                    new TrendDescription(
                        "Remote Work Transformation",
                        "Shift to remote and hybrid work models across industries",
                        "remote work, hybrid work, work from home, distributed teams",
                        "https://example.com/trends/remote-work"),
                    TrendCategory.Industry,
                    new ImpactAssessment(
                        ImpactLevel.Critical,
                        "Fundamental change in work organization and technology needs",
                        0.9m,
                        "Adapt products/services for remote work scenarios"),
                    _clock.Now,
                    _clock.Now.AddMonths(18),
                    _currentTenant.Id
                )
            };

            foreach (var trend in seedTrends)
            {
                await _marketTrendRepository.InsertAsync(trend);
            }

            _logger.LogInformation("Seeded {Count} market trends", seedTrends.Count);
        }

        private async Task SeedCustomerSegmentsAsync()
        {
            if (await _segmentRepository.GetCountAsync() > 0)
            {
                return;
            }

            _logger.LogInformation("Seeding customer segments");

            var seedSegments = new List<CustomerSegmentProfile>
            {
                CustomerSegmentProfile.Create(
                    _guidGenerator.Create(),
                    "Enterprise Tech Leaders",
                    "CIOs and IT decision-makers in large enterprises",
                    SegmentType.Primary,
                    new Demographics(
                        "35-55",
                        "Predominantly male",
                        "$150K+",
                        "Graduate degree",
                        "Global",
                        "Technology executive"),
                    new Psychographics(
                        "Technology, innovation, efficiency",
                        "Security, reliability, ROI",
                        "High-pressure, results-driven",
                        "Analytical, cautious",
                        "Risk-averse, progressive on technology"),
                    "Seeking efficient solutions to complex technical challenges while maintaining security and reliability",
                    _currentTenant.Id
                ),
                
                CustomerSegmentProfile.Create(
                    _guidGenerator.Create(),
                    "Startup Founders",
                    "Founders and leaders of early-stage startups",
                    SegmentType.Secondary,
                    new Demographics(
                        "25-45",
                        "Mixed gender",
                        "Variable",
                        "Bachelor's or higher",
                        "Tech hubs",
                        "Entrepreneur"),
                    new Psychographics(
                        "Innovation, growth, networking",
                        "Agility, efficiency, cost-effectiveness",
                        "Fast-paced, adaptive",
                        "Risk-taking, optimistic",
                        "Open to new tools and approaches"),
                    "Looking for scalable, cost-effective solutions that can grow with their business",
                    _currentTenant.Id
                ),
                
                CustomerSegmentProfile.Create(
                    _guidGenerator.Create(),
                    "SMB Operations Managers",
                    "Operations leaders in small and medium businesses",
                    SegmentType.Prospective,
                    new Demographics(
                        "30-50",
                        "Mixed gender",
                        "$80K-120K",
                        "Bachelor's degree",
                        "Nationwide",
                        "Operations Manager"),
                    new Psychographics(
                        "Business efficiency, team management",
                        "Reliability, simplicity, support",
                        "Practical, balanced",
                        "Steady, dependable",
                        "Cautious about new technology"),
                    "Need straightforward, reliable solutions with excellent support and minimal learning curve",
                    _currentTenant.Id
                )
            };

            foreach (var segment in seedSegments)
            {
                await _segmentRepository.InsertAsync(segment);
            }

            _logger.LogInformation("Seeded {Count} customer segments", seedSegments.Count);
        }
    }
}
