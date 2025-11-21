using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Managers
{
    /// <summary>
    /// Manager for InvestorProfile aggregate - handles complex business logic
    /// </summary>
    public class InvestorProfileManager : DomainService
    {
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ILogger<InvestorProfileManager> _logger;

        public InvestorProfileManager(
            IInvestorProfileRepository investorRepository,
            ILogger<InvestorProfileManager> logger)
        {
            _investorRepository = investorRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new investor profile with validation
        /// </summary>
        public async Task<InvestorProfile> CreateAsync(
            string name,
            string firm,
            string email,
            string investorType,
            string description = null)
        {
            // Check for duplicate email
            if (!string.IsNullOrWhiteSpace(email))
            {
                var existingInvestors = await _investorRepository.GetByEmailAsync(
                    email,
                    CurrentTenant.Id);

                if (existingInvestors.Any())
                {
                    throw new DuplicateInvestorEmailException(
                        $"An investor with email '{email}' already exists.");
                }
            }

            var investor = InvestorProfile.Create(
                GuidGenerator.Create(),
                name,
                firm,
                email,
                investorType,
                description,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new investor profile {InvestorId} for {Name} at {Firm}",
                investor.Id, name, firm);

            return investor;
        }

        /// <summary>
        /// Updates investor profile with validation
        /// </summary>
        public async Task<InvestorProfile> UpdateAsync(
            Guid investorId,
            string? newName = null,
            string? newFirm = null,
            string? newEmail = null,
            string? newDescription = null)
        {
            var investor = await _investorRepository.GetAsync(investorId);

            // Check for duplicate email if email is being changed
            if (!string.IsNullOrWhiteSpace(newEmail) && investor.Email != newEmail)
            {
                var existingInvestors = await _investorRepository.GetByEmailAsync(
                    newEmail,
                    CurrentTenant.Id);

                if (existingInvestors.Any(i => i.Id != investorId))
                {
                    throw new DuplicateInvestorEmailException(
                        $"An investor with email '{newEmail}' already exists.");
                }
            }

            investor.UpdateBasicInfo(
                newName ?? investor.Name,
                newFirm ?? investor.Firm,
                newEmail ?? investor.Email,
                newDescription ?? investor.Description);

            await _investorRepository.UpdateAsync(investor);

            _logger.LogInformation(
                "Updated investor profile {InvestorId}",
                investorId);

            return investor;
        }

        /// <summary>
        /// Archives an investor profile
        /// </summary>
        public async Task<InvestorProfile> ArchiveAsync(
            Guid investorId,
            string reason)
        {
            var investor = await _investorRepository.GetAsync(investorId);

            investor.Archive(reason);
            await _investorRepository.UpdateAsync(investor);

            _logger.LogInformation(
                "Archived investor profile {InvestorId}. Reason: {Reason}",
                investorId, reason);

            return investor;
        }

        /// <summary>
        /// Deletes an investor profile (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid investorId, bool hardDelete = false)
        {
            var investor = await _investorRepository.GetAsync(investorId);

            // Business rule: Cannot delete investors with active updates
            if (investor.InvestorUpdates.Any(u => !u.IsSent) && !hardDelete)
            {
                throw new IRMDomainException(
                    "IRM:CannotDeleteWithPendingUpdates",
                    "Cannot delete investor with pending updates. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _investorRepository.DeleteAsync(investor);
                _logger.LogWarning("Hard deleted investor profile {InvestorId}", investorId);
            }
            else
            {
                await _investorRepository.DeleteAsync(investor, autoSave: true);
                _logger.LogInformation("Soft deleted investor profile {InvestorId}", investorId);
            }
        }
    }
}
