namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for go-to-market strategy planning
    /// </summary>
    public class GtmPlanner : DomainService, IGtmPlanner
    {
        private readonly ILogger<GtmPlanner> _logger;

        public GtmPlanner(ILogger<GtmPlanner> logger)
        {
            _logger = logger;
        }

        public async Task<GoToMarketStrategy> CreateGtmStrategyAsync(
            RevenueModel revenueModel,
            string name,
            string description,
            string targetMarket,
            DateTime startDate,
            DateTime? endDate = null)
        {
            try
            {
                var strategy = revenueModel.AddGtmStrategy(name, description, targetMarket, startDate, endDate);

                _logger.LogInformation("GTM strategy created: {StrategyId} - {Name} for revenue model {RevenueModelId}",
                    strategy.Id, strategy.Name, revenueModel.Id);

                return strategy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create GTM strategy {Name} for revenue model {RevenueModelId}",
                    name, revenueModel.Id);
                throw;
            }
        }

        public async Task<List<GtmPlanElement>> GenerateRecommendedChannelsAsync(
            string targetMarket,
            RevenueModelType revenueModelType,
            decimal budget)
        {
            var recommendations = new List<GtmPlanElement>();

            // Generate recommendations based on revenue model type and budget
            switch (revenueModelType)
            {
                case RevenueModelType.Subscription:
                    recommendations.AddRange(await GetSubscriptionChannels(targetMarket, budget));
                    break;

                case RevenueModelType.OneTime:
                    recommendations.AddRange(await GetOneTimeChannels(targetMarket, budget));
                    break;

                case RevenueModelType.Freemium:
                    recommendations.AddRange(await GetFreemiumChannels(targetMarket, budget));
                    break;

                case RevenueModelType.Marketplace:
                    recommendations.AddRange(await GetMarketplaceChannels(targetMarket, budget));
                    break;

                default:
                    recommendations.AddRange(await GetGenericChannels(targetMarket, budget));
                    break;
            }

            return recommendations.OrderBy(r => r.Priority).ToList();
        }

        public async Task<ValidationResult> ValidateGtmStrategyAsync(GoToMarketStrategy strategy)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(strategy.TargetMarket))
                result.AddError("Target market must be defined");

            if (!strategy.PlanElements.Any())
                result.AddWarning("No plan elements defined - strategy may be incomplete");

            if (strategy.StartDate > strategy.EndDate)
                result.AddError("End date must be after start date");

            var totalBudget = strategy.PlanElements.Sum(pe => pe.EstimatedCost);
            if (totalBudget == 0)
                result.AddWarning("No budget allocated for strategy execution");

            // Check for duplicate channels
            var channelTypes = strategy.PlanElements.Select(pe => pe.ChannelType).ToList();
            var duplicates = channelTypes.GroupBy(ct => ct).Where(g => g.Count() > 1).ToList();
            if (duplicates.Any())
                result.AddWarning($"Duplicate channels detected: {string.Join(", ", duplicates.Select(d => d.Key))}");

            return result;
        }

        public async Task<decimal> EstimateChannelCostAsync(
            GtmChannelType channelType,
            string targetAudience,
            int duration)
        {
            // Cost estimation based on channel type
            var baseCosts = new Dictionary<GtmChannelType, decimal>
            {
                [GtmChannelType.Direct] = 5000m,
                [GtmChannelType.Partner] = 3000m,
                [GtmChannelType.Retail] = 8000m,
                [GtmChannelType.Online] = 2000m,
                [GtmChannelType.SocialMedia] = 1500m,
                [GtmChannelType.ContentMarketing] = 2500m,
                [GtmChannelType.EventMarketing] = 10000m,
                [GtmChannelType.ReferralProgram] = 1000m,
                [GtmChannelType.Influencer] = 3500m,
                [GtmChannelType.Affiliate] = 2000m
            };

            var baseCost = baseCosts.GetValueOrDefault(channelType, 2000m);

            // Apply duration multiplier
            var monthlyMultiplier = 1m + (duration * 0.1m); // 10% increase per month

            // Apply audience complexity multiplier
            var audienceMultiplier = GetAudienceComplexityMultiplier(targetAudience);

            return baseCost * monthlyMultiplier * audienceMultiplier;
        }

        public async Task<List<string>> GetEffectivenessMetricsAsync(GtmChannelType channelType)
        {
            return channelType switch
            {
                GtmChannelType.Direct => new List<string>
                {
                    "Customer Acquisition Cost (CAC)",
                    "Conversion Rate",
                    "Sales Cycle Length",
                    "Customer Lifetime Value (LTV)"
                },
                GtmChannelType.Online => new List<string>
                {
                    "Click-Through Rate (CTR)",
                    "Conversion Rate",
                    "Cost Per Acquisition (CPA)",
                    "Return on Ad Spend (ROAS)"
                },
                GtmChannelType.SocialMedia => new List<string>
                {
                    "Engagement Rate",
                    "Reach and Impressions",
                    "Cost Per Click (CPC)",
                    "Social Media ROI"
                },
                GtmChannelType.ContentMarketing => new List<string>
                {
                    "Organic Traffic Growth",
                    "Lead Generation Rate",
                    "Content Engagement",
                    "SEO Rankings"
                },
                GtmChannelType.EventMarketing => new List<string>
                {
                    "Event Attendance",
                    "Lead Quality Score",
                    "Follow-up Conversion Rate",
                    "Cost Per Lead"
                },
                _ => new List<string>
                {
                    "Customer Acquisition Cost",
                    "Conversion Rate",
                    "Return on Investment"
                }
            };
        }

        private async Task<List<GtmPlanElement>> GetSubscriptionChannels(string targetMarket, decimal budget)
        {
            return new List<GtmPlanElement>
            {
                new GtmPlanElement(
                    GtmChannelType.ContentMarketing,
                    "SEO & Content Marketing",
                    "Long-term organic growth through valuable content",
                    targetMarket,
                    budget * 0.3m,
                    "USD",
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(12),
                    new List<string> { "Organic Traffic", "Lead Generation", "Brand Awareness" }
                ),
                new GtmPlanElement(
                    GtmChannelType.Online,
                    "Paid Search & Social",
                    "Targeted online advertising for immediate results",
                    targetMarket,
                    budget * 0.4m,
                    "USD",
                    2,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6),
                    new List<string> { "CAC", "Conversion Rate", "ROAS" }
                ),
                new GtmPlanElement(
                    GtmChannelType.ReferralProgram,
                    "Customer Referral Program",
                    "Leverage existing customers for new acquisitions",
                    targetMarket,
                    budget * 0.2m,
                    "USD",
                    3,
                    DateTime.UtcNow.AddMonths(3),
                    DateTime.UtcNow.AddMonths(9),
                    new List<string> { "Referral Rate", "Customer LTV", "Program ROI" }
                )
            };
        }

        private async Task<List<GtmPlanElement>> GetOneTimeChannels(string targetMarket, decimal budget)
        {
            return new List<GtmPlanElement>
            {
                new GtmPlanElement(
                    GtmChannelType.Direct,
                    "Direct Sales",
                    "Direct outreach and sales process",
                    targetMarket,
                    budget * 0.5m,
                    "USD",
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6),
                    new List<string> { "Sales Conversion", "Sales Cycle Length", "Deal Size" }
                ),
                new GtmPlanElement(
                    GtmChannelType.Partner,
                    "Channel Partners",
                    "Leverage partner networks for sales",
                    targetMarket,
                    budget * 0.3m,
                    "USD",
                    2,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(12),
                    new List<string> { "Partner Performance", "Channel Revenue", "Partner Satisfaction" }
                )
            };
        }

        private async Task<List<GtmPlanElement>> GetFreemiumChannels(string targetMarket, decimal budget)
        {
            return new List<GtmPlanElement>
            {
                new GtmPlanElement(
                    GtmChannelType.Online,
                    "Performance Marketing",
                    "Paid campaigns to drive freemium sign-ups",
                    targetMarket,
                    budget * 0.4m,
                    "USD",
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6),
                    new List<string> { "Sign-up Rate", "Free-to-Paid Conversion", "CAC Payback" }
                ),
                new GtmPlanElement(
                    GtmChannelType.SocialMedia,
                    "Social Media Marketing",
                    "Build community and drive organic growth",
                    targetMarket,
                    budget * 0.3m,
                    "USD",
                    2,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(9),
                    new List<string> { "Community Growth", "Engagement Rate", "Viral Coefficient" }
                )
            };
        }

        private async Task<List<GtmPlanElement>> GetMarketplaceChannels(string targetMarket, decimal budget)
        {
            return new List<GtmPlanElement>
            {
                new GtmPlanElement(
                    GtmChannelType.Partner,
                    "Supplier Acquisition",
                    "Recruit and onboard marketplace suppliers",
                    "Suppliers/Vendors",
                    budget * 0.4m,
                    "USD",
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6),
                    new List<string> { "Supplier Count", "Product Variety", "Quality Score" }
                ),
                new GtmPlanElement(
                    GtmChannelType.Online,
                    "Customer Acquisition",
                    "Drive demand-side growth through digital marketing",
                    targetMarket,
                    budget * 0.4m,
                    "USD",
                    2,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(12),
                    new List<string> { "Customer Growth", "Transaction Volume", "Marketplace GMV" }
                )
            };
        }

        private async Task<List<GtmPlanElement>> GetGenericChannels(string targetMarket, decimal budget)
        {
            return new List<GtmPlanElement>
            {
                new GtmPlanElement(
                    GtmChannelType.Online,
                    "Digital Marketing",
                    "Multi-channel digital marketing approach",
                    targetMarket,
                    budget * 0.6m,
                    "USD",
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6),
                    new List<string> { "Lead Generation", "Conversion Rate", "ROI" }
                ),
                new GtmPlanElement(
                    GtmChannelType.EventMarketing,
                    "Industry Events",
                    "Participate in relevant industry events",
                    targetMarket,
                    budget * 0.4m,
                    "USD",
                    2,
                    DateTime.UtcNow.AddMonths(1),
                    DateTime.UtcNow.AddMonths(10),
                    new List<string> { "Event ROI", "Lead Quality", "Brand Awareness" }
                )
            };
        }

        private decimal GetAudienceComplexityMultiplier(string targetAudience)
        {
            var audience = targetAudience.ToLower();

            if (audience.Contains("enterprise") || audience.Contains("b2b"))
                return 1.5m; // B2B is typically more expensive

            if (audience.Contains("niche") || audience.Contains("specialized"))
                return 1.3m; // Niche markets require specialized approach

            if (audience.Contains("mass market") || audience.Contains("consumer"))
                return 1.0m; // Baseline for mass market

            return 1.1m; // Default slight increase
        }
    }
}
