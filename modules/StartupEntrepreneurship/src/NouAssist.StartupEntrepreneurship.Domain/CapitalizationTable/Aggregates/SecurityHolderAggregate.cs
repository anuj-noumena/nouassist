using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates
{
    /// <summary>
    /// Represents an individual or entity that holds securities in the company
    /// </summary>
    public class SecurityHolder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public HolderType HolderType { get; private set; }
        public string TaxId { get; private set; }
        public string Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? DateJoined { get; private set; }
        public string ExternalReference { get; private set; }

        private readonly List<VestingSchedule> _vestingSchedules;
        public IReadOnlyCollection<VestingSchedule> VestingSchedules => _vestingSchedules.AsReadOnly();

        protected SecurityHolder() 
        {
            _vestingSchedules = new List<VestingSchedule>();
        }

        /// <summary>
        /// Creates a new security holder
        /// </summary>
        public static SecurityHolder Create(
            Guid id,
            string name,
            HolderType holderType,
            string email = null,
            string taxId = null,
            string address = null,
            DateTime? dateJoined = null,
            string externalReference = null,
            Guid? tenantId = null)
        {
            return new SecurityHolder(id, name, holderType, email, taxId, address, dateJoined, externalReference, tenantId);
        }

        private SecurityHolder(
            Guid id,
            string name,
            HolderType holderType,
            string email = null,
            string taxId = null,
            string address = null,
            DateTime? dateJoined = null,
            string externalReference = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            HolderType = holderType;
            SetEmail(email);
            TaxId = taxId;
            Address = address;
            DateJoined = dateJoined;
            ExternalReference = externalReference;
            IsActive = true;
            TenantId = tenantId;

            _vestingSchedules = new List<VestingSchedule>();
        }

        /// <summary>
        /// Updates basic holder information
        /// </summary>
        public void UpdateInfo(
            string name,
            string email = null,
            string taxId = null,
            string address = null)
        {
            SetName(name);
            SetEmail(email);
            TaxId = taxId;
            Address = address;
        }

        /// <summary>
        /// Adds a vesting schedule for securities held by this holder
        /// </summary>
        public void AddVestingSchedule(
            Guid securityIssuanceId,
            DateTime grantDate,
            VestingTerms vestingTerms,
            ShareQuantity totalShares)
        {
            // Check if vesting schedule already exists for this issuance
            if (_vestingSchedules.Any(v => v.SecurityIssuanceId == securityIssuanceId))
            {
                throw new BusinessException("CapTable:ErrorMessage:DuplicateSecurityHolder")
                    .WithData("reason", "Vesting schedule already exists for this security issuance");
            }

            var vestingSchedule = new VestingSchedule(
                Guid.NewGuid(),
                Id,
                securityIssuanceId,
                grantDate,
                vestingTerms,
                totalShares,
                TenantId
            );

            _vestingSchedules.Add(vestingSchedule);
        }

        /// <summary>
        /// Updates vesting schedules based on current date
        /// </summary>
        public void UpdateVesting(DateTime asOfDate)
        {
            foreach (var schedule in _vestingSchedules.Where(v => v.Status != VestingStatus.Exercised && v.Status != VestingStatus.Expired))
            {
                schedule.UpdateVesting(asOfDate);
            }
        }

        /// <summary>
        /// Gets total vested shares across all vesting schedules
        /// </summary>
        public ShareQuantity GetTotalVestedShares(DateTime asOfDate)
        {
            UpdateVesting(asOfDate);
            var totalVested = _vestingSchedules.Sum(v => v.VestedShares.Value);
            return new ShareQuantity(totalVested);
        }

        /// <summary>
        /// Gets exercisable shares (vested but not yet exercised)
        /// </summary>
        public ShareQuantity GetExercisableShares(DateTime asOfDate)
        {
            UpdateVesting(asOfDate);
            var exercisable = _vestingSchedules
                .Where(v => v.Status == VestingStatus.Vested || v.Status == VestingStatus.Vesting)
                .Sum(v => v.VestedShares.Value);
            return new ShareQuantity(exercisable);
        }

        /// <summary>
        /// Exercises options from a specific vesting schedule
        /// </summary>
        public void ExerciseOptions(Guid securityIssuanceId, ShareQuantity quantity, DateTime exerciseDate)
        {
            var vestingSchedule = _vestingSchedules.FirstOrDefault(v => v.SecurityIssuanceId == securityIssuanceId);
            if (vestingSchedule == null)
            {
                throw new BusinessException("CapTable:ErrorMessage:EntityNotFound")
                    .WithData("entityType", "Vesting Schedule")
                    .WithData("securityIssuanceId", securityIssuanceId);
            }

            var exercisableShares = vestingSchedule.GetExercisableShares(exerciseDate);
            if (quantity.IsGreaterThan(exercisableShares))
            {
                throw new InsufficientSharesException(quantity.Value, exercisableShares.Value);
            }

            vestingSchedule.MarkAsExercised(exerciseDate);
        }

        /// <summary>
        /// Deactivates the security holder
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactivates the security holder
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CapTableConsts.MaxHolderNameLength
            );
        }

        private void SetEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Basic email validation - in production, use more robust validation
                if (!email.Contains("@") || !email.Contains("."))
                {
                    throw new BusinessException("CapTable:Validation:InvalidValue")
                        .WithData("propertyName", "Email");
                }
            }
            Email = email;
        }
    }
}
