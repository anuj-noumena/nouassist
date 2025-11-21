using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Managers
{
    /// <summary>
    /// Manager for CapTable aggregate - handles complex business logic
    /// </summary>
    public class CapTableManager : DomainService
    {
        private readonly ICapTableRepository _capTableRepository;
        private readonly ILogger<CapTableManager> _logger;

        public CapTableManager(
            ICapTableRepository capTableRepository,
            ILogger<CapTableManager> logger)
        {
            _capTableRepository = capTableRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new cap table with validation
        /// </summary>
        public async Task<CapTable> CreateAsync(
            string companyName,
            ShareQuantity totalAuthorizedShares,
            DateTime asOfDate,
            string companyIdentifier = null)
        {
            // Validate authorized shares
            if (totalAuthorizedShares == null || totalAuthorizedShares.Value <= 0)
            {
                throw new CapTableDomainException(
                    "CapTable:InvalidAuthorizedShares",
                    "Total authorized shares must be greater than zero.");
            }

            // Check for duplicate company identifier
            if (!string.IsNullOrWhiteSpace(companyIdentifier))
            {
                var existing = await _capTableRepository.FindByCompanyIdentifierAsync(
                    companyIdentifier,
                    CurrentTenant.Id);

                if (existing != null)
                {
                    throw new CapTableDomainException(
                        "CapTable:DuplicateCompanyIdentifier",
                        $"A cap table with company identifier '{companyIdentifier}' already exists.");
                }
            }

            var capTable = CapTable.Create(
                GuidGenerator.Create(),
                companyName,
                totalAuthorizedShares,
                asOfDate,
                companyIdentifier,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new cap table {CapTableId} for {CompanyName} with {AuthorizedShares} authorized shares",
                capTable.Id, capTable.CompanyName, capTable.TotalAuthorizedShares.Value);

            return capTable;
        }

        /// <summary>
        /// Issues shares with validation
        /// </summary>
        public async Task<CapTable> IssueSharesAsync(
            Guid capTableId,
            Guid toHolderId,
            Guid securityIssuanceId,
            Guid shareClassId,
            ShareQuantity quantity,
            PricePerShare pricePerShare,
            DateTime transactionDate,
            string memo = null)
        {
            var capTable = await _capTableRepository.GetAsync(capTableId);

            // Business rule: Cannot issue shares if cap table is inactive
            if (!capTable.IsActive)
            {
                throw new CapTableDomainException(
                    "CapTable:InactiveCapTable",
                    "Cannot issue shares on an inactive cap table.");
            }

            // Business rule: Cannot issue more shares than authorized
            var totalAfterIssuance = capTable.TotalIssuedShares.Value + quantity.Value;
            if (totalAfterIssuance > capTable.TotalAuthorizedShares.Value)
            {
                throw new InsufficientSharesException(
                    $"Cannot issue {quantity.Value} shares. Would exceed authorized shares limit.");
            }

            capTable.IssueShares(
                toHolderId,
                securityIssuanceId,
                shareClassId,
                quantity,
                pricePerShare,
                transactionDate,
                memo);

            await _capTableRepository.UpdateAsync(capTable);

            _logger.LogInformation(
                "Issued {Quantity} shares to holder {HolderId} on cap table {CapTableId}",
                quantity.Value, toHolderId, capTableId);

            return capTable;
        }

        /// <summary>
        /// Records a valuation event
        /// </summary>
        public async Task<CapTable> RecordValuationAsync(
            Guid capTableId,
            Valuation valuation,
            DateTime valuationDate,
            string notes = null)
        {
            var capTable = await _capTableRepository.GetAsync(capTableId);

            // Business rule: Valuation must be positive
            if (valuation == null || valuation.PostMoneyValuation <= 0)
            {
                throw new CapTableDomainException(
                    "CapTable:InvalidValuation",
                    "Valuation must be greater than zero.");
            }

            capTable.RecordValuation(valuation, valuationDate, notes);
            await _capTableRepository.UpdateAsync(capTable);

            _logger.LogInformation(
                "Recorded valuation of {PostMoneyValuation:C} for cap table {CapTableId}",
                valuation.PostMoneyValuation, capTableId);

            return capTable;
        }

        /// <summary>
        /// Increases authorized shares
        /// </summary>
        public async Task<CapTable> IncreaseAuthorizedSharesAsync(
            Guid capTableId,
            ShareQuantity additionalShares,
            string reason)
        {
            var capTable = await _capTableRepository.GetAsync(capTableId);

            // Business rule: Must provide a reason for increasing authorized shares
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new CapTableDomainException(
                    "CapTable:ReasonRequired",
                    "A reason must be provided for increasing authorized shares.");
            }

            capTable.IncreaseAuthorizedShares(additionalShares, reason);
            await _capTableRepository.UpdateAsync(capTable);

            _logger.LogInformation(
                "Increased authorized shares by {Additional} for cap table {CapTableId}. Reason: {Reason}",
                additionalShares.Value, capTableId, reason);

            return capTable;
        }

        /// <summary>
        /// Deactivates a cap table
        /// </summary>
        public async Task<CapTable> DeactivateAsync(
            Guid capTableId,
            string reason)
        {
            var capTable = await _capTableRepository.GetAsync(capTableId);

            if (!capTable.IsActive)
            {
                throw new CapTableDomainException(
                    "CapTable:AlreadyInactive",
                    "Cap table is already inactive.");
            }

            capTable.Deactivate(reason);
            await _capTableRepository.UpdateAsync(capTable);

            _logger.LogInformation(
                "Deactivated cap table {CapTableId}. Reason: {Reason}",
                capTableId, reason);

            return capTable;
        }

        /// <summary>
        /// Deletes a cap table (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid capTableId, bool hardDelete = false)
        {
            var capTable = await _capTableRepository.GetAsync(capTableId);

            // Business rule: Cannot delete active cap tables with issued shares
            if (capTable.IsActive && capTable.TotalIssuedShares.Value > 0 && !hardDelete)
            {
                throw new CapTableDomainException(
                    "CapTable:CannotDeleteWithIssuedShares",
                    "Cannot delete an active cap table with issued shares. Deactivate it first or use hard delete.");
            }

            if (hardDelete)
            {
                await _capTableRepository.DeleteAsync(capTable);
                _logger.LogWarning("Hard deleted cap table {CapTableId}", capTableId);
            }
            else
            {
                await _capTableRepository.DeleteAsync(capTable, autoSave: true);
                _logger.LogInformation("Soft deleted cap table {CapTableId}", capTableId);
            }
        }
    }
}
