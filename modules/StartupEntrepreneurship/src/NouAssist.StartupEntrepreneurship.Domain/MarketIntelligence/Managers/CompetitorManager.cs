using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Managers
{
    /// <summary>
    /// Manager for Competitor aggregate - handles complex business logic
    /// </summary>
    public class CompetitorManager : DomainService
    {
        private readonly ICompetitorRepository _competitorRepository;
        private readonly ILogger<CompetitorManager> _logger;

        public CompetitorManager(
            ICompetitorRepository competitorRepository,
            ILogger<CompetitorManager> logger)
        {
            _competitorRepository = competitorRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new competitor with validation
        /// </summary>
        public async Task<Competitor> CreateAsync(
            string name,
            string website,
            string description,
            string industry = null,
            string location = null)
        {
            // Check for duplicate name
            var existing = await _competitorRepository.FindByNameAsync(
                name,
                CurrentTenant.Id);

            if (existing != null)
            {
                throw new DuplicateCompetitorException(
                    $"A competitor with name '{name}' already exists.");
            }

            // Check for duplicate website if provided
            if (!string.IsNullOrWhiteSpace(website))
            {
                var existingByWebsite = await _competitorRepository.FindByWebsiteAsync(
                    website,
                    CurrentTenant.Id);

                if (existingByWebsite != null)
                {
                    throw new DuplicateCompetitorException(
                        $"A competitor with website '{website}' already exists.");
                }
            }

            var competitor = Competitor.Create(
                GuidGenerator.Create(),
                name,
                website,
                description,
                industry,
                location,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new competitor {CompetitorId} with name '{Name}'",
                competitor.Id, competitor.Name);

            return competitor;
        }

        /// <summary>
        /// Updates competitor with validation
        /// </summary>
        public async Task<Competitor> UpdateAsync(
            Guid competitorId,
            string newName = null,
            string newWebsite = null,
            string newDescription = null,
            string newIndustry = null,
            string newLocation = null)
        {
            var competitor = await _competitorRepository.GetAsync(competitorId);

            // Check for duplicate name if being changed
            if (!string.IsNullOrWhiteSpace(newName) && competitor.Name != newName)
            {
                var existing = await _competitorRepository.FindByNameAsync(
                    newName,
                    CurrentTenant.Id);

                if (existing != null && existing.Id != competitorId)
                {
                    throw new DuplicateCompetitorException(
                        $"A competitor with name '{newName}' already exists.");
                }
            }

            // Check for duplicate website if being changed
            if (!string.IsNullOrWhiteSpace(newWebsite) && competitor.Website != newWebsite)
            {
                var existing = await _competitorRepository.FindByWebsiteAsync(
                    newWebsite,
                    CurrentTenant.Id);

                if (existing != null && existing.Id != competitorId)
                {
                    throw new DuplicateCompetitorException(
                        $"A competitor with website '{newWebsite}' already exists.");
                }
            }

            competitor.UpdateBasicInfo(
                newName ?? competitor.Name,
                newWebsite ?? competitor.Website,
                newDescription ?? competitor.Description,
                newIndustry ?? competitor.Industry,
                newLocation ?? competitor.Location);

            await _competitorRepository.UpdateAsync(competitor);

            _logger.LogInformation(
                "Updated competitor {CompetitorId}",
                competitorId);

            return competitor;
        }

        /// <summary>
        /// Archives a competitor
        /// </summary>
        public async Task<Competitor> ArchiveAsync(
            Guid competitorId,
            string reason)
        {
            var competitor = await _competitorRepository.GetAsync(competitorId);

            if (competitor.IsArchived)
            {
                throw new MarketIntelligenceDomainException(
                    "MarketIntelligence:AlreadyArchived",
                    "Competitor is already archived.");
            }

            competitor.Archive(reason);
            await _competitorRepository.UpdateAsync(competitor);

            _logger.LogInformation(
                "Archived competitor {CompetitorId}. Reason: {Reason}",
                competitorId, reason);

            return competitor;
        }

        /// <summary>
        /// Deletes a competitor (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid competitorId, bool hardDelete = false)
        {
            var competitor = await _competitorRepository.GetAsync(competitorId);

            if (hardDelete)
            {
                await _competitorRepository.DeleteAsync(competitor);
                _logger.LogWarning("Hard deleted competitor {CompetitorId}", competitorId);
            }
            else
            {
                await _competitorRepository.DeleteAsync(competitor, autoSave: true);
                _logger.LogInformation("Soft deleted competitor {CompetitorId}", competitorId);
            }
        }
    }
}
