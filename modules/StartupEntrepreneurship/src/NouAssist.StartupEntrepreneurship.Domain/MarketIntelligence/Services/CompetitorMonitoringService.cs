using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Specifications;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Services
{
    /// <summary>
    /// Domain service for monitoring and managing competitors
    /// </summary>
    public class CompetitorMonitoringService : DomainService, ICompetitorMonitoringService
    {
        private readonly ICompetitorRepository _competitorRepository;
        private readonly ILogger<CompetitorMonitoringService> _logger;

        public CompetitorMonitoringService(
            ICompetitorRepository competitorRepository,
            ILogger<CompetitorMonitoringService> logger)
        {
            _competitorRepository = competitorRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Competitor> CreateCompetitorAsync(
            string name,
            string description,
            CompetitorInfo competitorInfo,
            CompetitorThreatLevel threatLevel = CompetitorThreatLevel.Low)
        {
            try
            {
                // Check for duplicate name
                if (await _competitorRepository.ExistsByNameAsync(name))
                {
                    throw new DuplicateCompetitorException(name);
                }

                // Check for duplicate website
                if (!string.IsNullOrEmpty(competitorInfo.Website) && 
                    await _competitorRepository.ExistsByWebsiteAsync(competitorInfo.Website))
                {
                    throw new DuplicateCompetitorException(name, competitorInfo.Website);
                }

                // Create competitor
                var competitor = Competitor.Create(
                    GuidGenerator.Create(),
                    name,
                    description,
                    competitorInfo,
                    threatLevel,
                    CurrentTenant.Id,
                    Clock.Now
                );

                await _competitorRepository.InsertAsync(competitor);
                return competitor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create competitor with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Competitor> UpdateCompetitorInfoAsync(
            Guid competitorId,
            string name,
            string description,
            CompetitorInfo competitorInfo)
        {
            try
            {
                // Get competitor
                var competitor = await _competitorRepository.GetAsync(competitorId);

                // Check for duplicate name
                if (name != competitor.Name && await _competitorRepository.ExistsByNameAsync(name, competitorId))
                {
                    throw new DuplicateCompetitorException(name);
                }

                // Check for duplicate website
                if (!string.IsNullOrEmpty(competitorInfo.Website) && 
                    competitorInfo.Website != competitor.CompetitorInfo.Website &&
                    await _competitorRepository.ExistsByWebsiteAsync(competitorInfo.Website, competitorId))
                {
                    throw new DuplicateCompetitorException(name, competitorInfo.Website);
                }

                // Update competitor
                competitor.UpdateInfo(name, description, competitorInfo);

                await _competitorRepository.UpdateAsync(competitor);
                return competitor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update competitor {CompetitorId}", competitorId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddCompetitorUpdateAsync(
            Guid competitorId,
            UpdateType updateType,
            string title,
            string description,
            ImpactLevel impactLevel = ImpactLevel.Low,
            string source = null,
            string tags = null)
        {
            try
            {
                var competitor = await _competitorRepository.GetAsync(competitorId);
                
                competitor.AddUpdateLog(
                    updateType,
                    title,
                    description,
                    Clock.Now,
                    impactLevel,
                    source,
                    tags
                );

                await _competitorRepository.UpdateAsync(competitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add update log for competitor {CompetitorId}", competitorId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ChangeCompetitorThreatLevelAsync(
            Guid competitorId,
            CompetitorThreatLevel newThreatLevel,
            string reason = null)
        {
            try
            {
                var competitor = await _competitorRepository.GetAsync(competitorId);
                competitor.ChangeThreatLevel(newThreatLevel, reason);
                await _competitorRepository.UpdateAsync(competitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change threat level for competitor {CompetitorId}", competitorId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task EvaluateCompetitorsAsync()
        {
            try
            {
                var activeCompetitors = await _competitorRepository.GetListAsync(
                    new ActiveCompetitorSpecification().ToExpression());

                foreach (var competitor in activeCompetitors)
                {
                    // Simple scoring based on threat level and recent activity
                    var score = CalculateCompetitiveScore(competitor);
                    competitor.UpdateCompetitiveScore(score, Clock.Now);
                }

                await _competitorRepository.UpdateManyAsync(activeCompetitors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to evaluate competitors");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Competitor>> GetCompetitorsByThreatLevelAsync()
        {
            try
            {
                var competitors = await _competitorRepository.GetListAsync(
                    new ActiveCompetitorSpecification().ToExpression());

                return competitors
                    .OrderByDescending(c => c.ThreatLevel)
                    .ThenByDescending(c => c.CompetitiveScore)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get competitors by threat level");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Competitor>> GetCompetitorsWithRecentUpdatesAsync(int days = 30, int maxResultCount = 10)
        {
            try
            {
                return await _competitorRepository.GetWithRecentUpdatesAsync(days, maxResultCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get competitors with recent updates");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ArchiveCompetitorAsync(Guid competitorId)
        {
            try
            {
                var competitor = await _competitorRepository.GetAsync(competitorId);
                competitor.ChangeStatus(CompetitorStatus.Archived);
                await _competitorRepository.UpdateAsync(competitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to archive competitor {CompetitorId}", competitorId);
                throw;
            }
        }

        /// <summary>
        /// Calculates competitive score based on threat level and recent activities
        /// </summary>
        private decimal CalculateCompetitiveScore(Competitor competitor)
        {
            // Base score based on threat level
            decimal baseScore = competitor.ThreatLevel switch
            {
                CompetitorThreatLevel.Critical => 0.9m,
                CompetitorThreatLevel.High => 0.7m,
                CompetitorThreatLevel.Medium => 0.5m,
                _ => 0.3m
            };

            // Recent update activity bonus
            var recentUpdates = competitor.UpdateLogs.Count(u => u.UpdateDate >= Clock.Now.AddDays(-30));
            var activityBonus = Math.Min(0.3m, recentUpdates * 0.03m);

            // High-impact updates bonus
            var highImpactUpdates = competitor.UpdateLogs.Count(u => 
                u.ImpactLevel == ImpactLevel.High || u.ImpactLevel == ImpactLevel.Critical);
            var impactBonus = Math.Min(0.2m, highImpactUpdates * 0.05m);

            return Math.Min(1.0m, baseScore + activityBonus + impactBonus);
        }
    }
}
