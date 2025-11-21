using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Managers
{
    /// <summary>
    /// Manager for Prospect aggregate - handles complex business logic
    /// </summary>
    public class ProspectManager : DomainService
    {
        private readonly IProspectRepository _prospectRepository;
        private readonly ILogger<ProspectManager> _logger;

        public ProspectManager(
            IProspectRepository prospectRepository,
            ILogger<ProspectManager> _logger)
        {
            _prospectRepository = prospectRepository;
            this._logger = _logger;
        }

        /// <summary>
        /// Creates a new prospect with validation
        /// </summary>
        public async Task<Prospect> CreateAsync(
            string name,
            string company,
            string title,
            ContactInfo contactInfo,
            string description = null)
        {
            // Check for duplicate by email if email is provided
            if (!string.IsNullOrWhiteSpace(contactInfo.Email))
            {
                var existingProspects = await _prospectRepository.GetByEmailAsync(
                    contactInfo.Email,
                    CurrentTenant.Id);

                if (existingProspects.Any())
                {
                    throw new FundraisingPipelineDomainException(
                        "FundraisingPipeline:DuplicateProspectEmail",
                        $"A prospect with email '{contactInfo.Email}' already exists.");
                }
            }

            var prospect = Prospect.Create(
                GuidGenerator.Create(),
                name,
                company,
                title,
                contactInfo,
                description,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new prospect {ProspectId} for {Name} at {Company}",
                prospect.Id, prospect.Name, prospect.Company);

            return prospect;
        }

        /// <summary>
        /// Updates prospect details
        /// </summary>
        public async Task<Prospect> UpdateAsync(
            Guid prospectId,
            string? name = null,
            string? company = null,
            string? title = null,
            string? description = null,
            ContactInfo? contactInfo = null)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);

            // If email is being changed, check for duplicates
            if (contactInfo != null &&
                !string.IsNullOrWhiteSpace(contactInfo.Email) &&
                prospect.ContactInfo.Email != contactInfo.Email)
            {
                var existingProspects = await _prospectRepository.GetByEmailAsync(
                    contactInfo.Email,
                    CurrentTenant.Id);

                if (existingProspects.Any(p => p.Id != prospectId))
                {
                    throw new FundraisingPipelineDomainException(
                        "FundraisingPipeline:DuplicateProspectEmail",
                        $"A prospect with email '{contactInfo.Email}' already exists.");
                }
            }

            prospect.UpdateBasicInfo(
                name ?? prospect.Name,
                company ?? prospect.Company,
                title ?? prospect.Title,
                description ?? prospect.Description,
                contactInfo ?? prospect.ContactInfo);

            await _prospectRepository.UpdateAsync(prospect);

            _logger.LogInformation(
                "Updated prospect {ProspectId}",
                prospectId);

            return prospect;
        }

        /// <summary>
        /// Advances prospect status with validation
        /// </summary>
        public async Task<Prospect> AdvanceStatusAsync(
            Guid prospectId,
            ProspectStatus newStatus,
            string notes = null)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);

            // Validate status transition
            if (!CanTransitionToStatus(prospect.PipelineStatus.Status, newStatus))
            {
                throw new InvalidProspectStatusTransitionException(
                    $"Cannot transition from {prospect.PipelineStatus.Status} to {newStatus}");
            }

            prospect.UpdateStatus(newStatus, notes, Clock.Now);
            await _prospectRepository.UpdateAsync(prospect);

            _logger.LogInformation(
                "Advanced prospect {ProspectId} to status {NewStatus}",
                prospectId, newStatus);

            return prospect;
        }

        /// <summary>
        /// Assigns prospect to a fundraising round
        /// </summary>
        public async Task<Prospect> AssignToRoundAsync(
            Guid prospectId,
            Guid fundraisingRoundId)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);

            prospect.AssignToFundraisingRound(fundraisingRoundId);
            await _prospectRepository.UpdateAsync(prospect);

            _logger.LogInformation(
                "Assigned prospect {ProspectId} to fundraising round {RoundId}",
                prospectId, fundraisingRoundId);

            return prospect;
        }

        /// <summary>
        /// Updates prospect score
        /// </summary>
        public async Task<Prospect> UpdateScoreAsync(
            Guid prospectId,
            int fit,
            int interest,
            int capacity)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);

            var newScore = new ProspectScore(fit, interest, capacity);
            prospect.UpdateScore(newScore);

            await _prospectRepository.UpdateAsync(prospect);

            _logger.LogInformation(
                "Updated prospect {ProspectId} score to {OverallScore}",
                prospectId, newScore.OverallScore);

            return prospect;
        }

        /// <summary>
        /// Validates if a status transition is allowed
        /// </summary>
        private bool CanTransitionToStatus(ProspectStatus currentStatus, ProspectStatus newStatus)
        {
            // Define valid status transitions
            return (currentStatus, newStatus) switch
            {
                // From New
                (ProspectStatus.New, ProspectStatus.Researching) => true,
                (ProspectStatus.New, ProspectStatus.Qualified) => true,
                (ProspectStatus.New, ProspectStatus.Disqualified) => true,

                // From Researching
                (ProspectStatus.Researching, ProspectStatus.Qualified) => true,
                (ProspectStatus.Researching, ProspectStatus.Disqualified) => true,

                // From Qualified
                (ProspectStatus.Qualified, ProspectStatus.Contacted) => true,
                (ProspectStatus.Qualified, ProspectStatus.Disqualified) => true,

                // From Contacted
                (ProspectStatus.Contacted, ProspectStatus.InDiscussion) => true,
                (ProspectStatus.Contacted, ProspectStatus.Disqualified) => true,

                // From InDiscussion
                (ProspectStatus.InDiscussion, ProspectStatus.DueDiligence) => true,
                (ProspectStatus.InDiscussion, ProspectStatus.Declined) => true,

                // From DueDiligence
                (ProspectStatus.DueDiligence, ProspectStatus.Negotiating) => true,
                (ProspectStatus.DueDiligence, ProspectStatus.Declined) => true,

                // From Negotiating
                (ProspectStatus.Negotiating, ProspectStatus.Committed) => true,
                (ProspectStatus.Negotiating, ProspectStatus.Declined) => true,

                // Same status is allowed (for updates)
                _ when currentStatus == newStatus => true,

                _ => false
            };
        }

        /// <summary>
        /// Deletes a prospect (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid prospectId, bool hardDelete = false)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);

            // Business rule: Cannot delete committed prospects
            if (prospect.PipelineStatus.Status == ProspectStatus.Committed && !hardDelete)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:CannotDeleteCommittedProspect",
                    "Cannot delete a committed prospect. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _prospectRepository.DeleteAsync(prospect);
                _logger.LogWarning("Hard deleted prospect {ProspectId}", prospectId);
            }
            else
            {
                await _prospectRepository.DeleteAsync(prospect, autoSave: true);
                _logger.LogInformation("Soft deleted prospect {ProspectId}", prospectId);
            }
        }
    }
}
