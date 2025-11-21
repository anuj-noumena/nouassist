using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Managers
{
    /// <summary>
    /// Manager for SecurityHolder aggregate - handles complex business logic
    /// </summary>
    public class SecurityHolderManager : DomainService
    {
        private readonly ISecurityHolderRepository _securityHolderRepository;
        private readonly ILogger<SecurityHolderManager> _logger;

        public SecurityHolderManager(
            ISecurityHolderRepository securityHolderRepository,
            ILogger<SecurityHolderManager> logger)
        {
            _securityHolderRepository = securityHolderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new security holder with validation
        /// </summary>
        public async Task<SecurityHolder> CreateAsync(
            string name,
            HolderType holderType,
            string email = null,
            string taxId = null,
            string address = null,
            DateTime? dateJoined = null,
            string externalReference = null)
        {
            // Check for duplicate email if provided
            if (!string.IsNullOrWhiteSpace(email))
            {
                var existingHolders = await _securityHolderRepository.GetByEmailAsync(
                    email,
                    CurrentTenant.Id);

                if (existingHolders.Any())
                {
                    throw new DuplicateSecurityHolderException(
                        $"A security holder with email '{email}' already exists.");
                }
            }

            // Check for duplicate tax ID if provided
            if (!string.IsNullOrWhiteSpace(taxId))
            {
                var existingHolders = await _securityHolderRepository.GetByTaxIdAsync(
                    taxId,
                    CurrentTenant.Id);

                if (existingHolders.Any())
                {
                    throw new DuplicateSecurityHolderException(
                        $"A security holder with tax ID '{taxId}' already exists.");
                }
            }

            var holder = SecurityHolder.Create(
                GuidGenerator.Create(),
                name,
                holderType,
                email,
                taxId,
                address,
                dateJoined ?? Clock.Now,
                externalReference,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new security holder {HolderId} with name '{Name}' and type {HolderType}",
                holder.Id, holder.Name, holder.HolderType);

            return holder;
        }

        /// <summary>
        /// Updates security holder information with validation
        /// </summary>
        public async Task<SecurityHolder> UpdateAsync(
            Guid holderId,
            string name = null,
            string email = null,
            string taxId = null,
            string address = null)
        {
            var holder = await _securityHolderRepository.GetAsync(holderId);

            // Check for duplicate email if being changed
            if (!string.IsNullOrWhiteSpace(email) && holder.Email != email)
            {
                var existingHolders = await _securityHolderRepository.GetByEmailAsync(
                    email,
                    CurrentTenant.Id);

                if (existingHolders.Any(h => h.Id != holderId))
                {
                    throw new DuplicateSecurityHolderException(
                        $"A security holder with email '{email}' already exists.");
                }
            }

            // Check for duplicate tax ID if being changed
            if (!string.IsNullOrWhiteSpace(taxId) && holder.TaxId != taxId)
            {
                var existingHolders = await _securityHolderRepository.GetByTaxIdAsync(
                    taxId,
                    CurrentTenant.Id);

                if (existingHolders.Any(h => h.Id != holderId))
                {
                    throw new DuplicateSecurityHolderException(
                        $"A security holder with tax ID '{taxId}' already exists.");
                }
            }

            holder.UpdateInfo(
                name ?? holder.Name,
                email ?? holder.Email,
                taxId ?? holder.TaxId,
                address ?? holder.Address);

            await _securityHolderRepository.UpdateAsync(holder);

            _logger.LogInformation(
                "Updated security holder {HolderId}",
                holderId);

            return holder;
        }

        /// <summary>
        /// Adds a vesting schedule to a security holder
        /// </summary>
        public async Task<SecurityHolder> AddVestingScheduleAsync(
            Guid holderId,
            Guid securityIssuanceId,
            DateTime grantDate,
            VestingTerms vestingTerms,
            ShareQuantity totalShares)
        {
            var holder = await _securityHolderRepository.GetAsync(holderId);

            // Business rule: Cannot add vesting schedule if holder is inactive
            if (!holder.IsActive)
            {
                throw new CapTableDomainException(
                    "SecurityHolder:InactiveHolder",
                    "Cannot add vesting schedule to an inactive security holder.");
            }

            // Business rule: Grant date cannot be in the future
            if (grantDate > Clock.Now)
            {
                throw new CapTableDomainException(
                    "SecurityHolder:InvalidGrantDate",
                    "Grant date cannot be in the future.");
            }

            // Check for duplicate vesting schedule for the same security issuance
            if (holder.VestingSchedules.Any(vs => vs.SecurityIssuanceId == securityIssuanceId))
            {
                throw new InvalidVestingScheduleException(
                    $"A vesting schedule already exists for security issuance {securityIssuanceId}.");
            }

            holder.AddVestingSchedule(securityIssuanceId, grantDate, vestingTerms, totalShares);
            await _securityHolderRepository.UpdateAsync(holder);

            _logger.LogInformation(
                "Added vesting schedule for security issuance {IssuanceId} to holder {HolderId}",
                securityIssuanceId, holderId);

            return holder;
        }

        /// <summary>
        /// Deactivates a security holder
        /// </summary>
        public async Task<SecurityHolder> DeactivateAsync(
            Guid holderId,
            string reason)
        {
            var holder = await _securityHolderRepository.GetAsync(holderId);

            if (!holder.IsActive)
            {
                throw new CapTableDomainException(
                    "SecurityHolder:AlreadyInactive",
                    "Security holder is already inactive.");
            }

            holder.Deactivate(reason);
            await _securityHolderRepository.UpdateAsync(holder);

            _logger.LogInformation(
                "Deactivated security holder {HolderId}. Reason: {Reason}",
                holderId, reason);

            return holder;
        }

        /// <summary>
        /// Deletes a security holder (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid holderId, bool hardDelete = false)
        {
            var holder = await _securityHolderRepository.GetAsync(holderId);

            // Business rule: Cannot delete holders with active vesting schedules
            if (holder.VestingSchedules.Any(vs => !vs.IsFullyVested) && !hardDelete)
            {
                throw new CapTableDomainException(
                    "SecurityHolder:CannotDeleteWithActiveVesting",
                    "Cannot delete a holder with active vesting schedules. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _securityHolderRepository.DeleteAsync(holder);
                _logger.LogWarning("Hard deleted security holder {HolderId}", holderId);
            }
            else
            {
                await _securityHolderRepository.DeleteAsync(holder, autoSave: true);
                _logger.LogInformation("Soft deleted security holder {HolderId}", holderId);
            }
        }
    }
}
