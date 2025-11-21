using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates
{
    /// <summary>
    /// Represents an employee stock option pool
    /// </summary>
    public class OptionPool : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid CapTableId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ShareQuantity TotalReserved { get; private set; }
        public ShareQuantity TotalGranted { get; private set; }
        public ShareQuantity TotalExercised { get; private set; }
        public ShareQuantity TotalForfeited { get; private set; }
        public OwnershipPercentage PoolPercentage { get; private set; }
        public VestingTerms DefaultVestingTerms { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedDate { get; private set; }

        protected OptionPool() { }

        /// <summary>
        /// Creates a new option pool
        /// </summary>
        public static OptionPool Create(
            Guid id,
            Guid capTableId,
            string name,
            ShareQuantity totalReserved,
            OwnershipPercentage poolPercentage,
            VestingTerms defaultVestingTerms,
            string description = null,
            Guid? tenantId = null)
        {
            return new OptionPool(id, capTableId, name, totalReserved, poolPercentage, defaultVestingTerms, description, tenantId);
        }

        private OptionPool(
            Guid id,
            Guid capTableId,
            string name,
            ShareQuantity totalReserved,
            OwnershipPercentage poolPercentage,
            VestingTerms defaultVestingTerms,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            CapTableId = capTableId;
            SetName(name);
            SetDescription(description);
            TotalReserved = totalReserved ?? throw new ArgumentNullException(nameof(totalReserved));
            PoolPercentage = poolPercentage ?? throw new ArgumentNullException(nameof(poolPercentage));
            DefaultVestingTerms = defaultVestingTerms ?? throw new ArgumentNullException(nameof(defaultVestingTerms));
            TotalGranted = new ShareQuantity(0);
            TotalExercised = new ShareQuantity(0);
            TotalForfeited = new ShareQuantity(0);
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            TenantId = tenantId;
        }

        /// <summary>
        /// Grants options from this pool
        /// </summary>
        public void GrantOptions(ShareQuantity quantity)
        {
            ValidateGrant(quantity);
            TotalGranted = TotalGranted.Add(quantity);
        }

        /// <summary>
        /// Records option exercise
        /// </summary>
        public void ExerciseOptions(ShareQuantity quantity)
        {
            if (quantity.IsGreaterThan(TotalGranted.Subtract(TotalExercised)))
            {
                throw new InsufficientSharesException(quantity.Value, TotalGranted.Subtract(TotalExercised).Value);
            }

            TotalExercised = TotalExercised.Add(quantity);
        }

        /// <summary>
        /// Records forfeited options (due to termination, expiry, etc.)
        /// </summary>
        public void ForfeitOptions(ShareQuantity quantity)
        {
            var outstandingOptions = TotalGranted.Subtract(TotalExercised).Subtract(TotalForfeited);
            if (quantity.IsGreaterThan(outstandingOptions))
            {
                throw new InsufficientSharesException(quantity.Value, outstandingOptions.Value);
            }

            TotalForfeited = TotalForfeited.Add(quantity);
        }

        /// <summary>
        /// Gets available options for granting
        /// </summary>
        public ShareQuantity GetAvailableOptions()
        {
            return TotalReserved.Subtract(TotalGranted);
        }

        /// <summary>
        /// Gets outstanding options (granted but not exercised or forfeited)
        /// </summary>
        public ShareQuantity GetOutstandingOptions()
        {
            return TotalGranted.Subtract(TotalExercised).Subtract(TotalForfeited);
        }

        /// <summary>
        /// Expands the option pool
        /// </summary>
        public void ExpandPool(ShareQuantity additionalShares, OwnershipPercentage newPoolPercentage)
        {
            if (!IsActive)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("reason", "Cannot expand inactive option pool");
            }

            TotalReserved = TotalReserved.Add(additionalShares);
            PoolPercentage = newPoolPercentage;
        }

        /// <summary>
        /// Updates the pool details
        /// </summary>
        public void UpdateDetails(
            string name,
            string description = null,
            VestingTerms defaultVestingTerms = null)
        {
            SetName(name);
            SetDescription(description);
            
            if (defaultVestingTerms != null)
            {
                DefaultVestingTerms = defaultVestingTerms;
            }
        }

        /// <summary>
        /// Deactivates the option pool
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        private void ValidateGrant(ShareQuantity quantity)
        {
            if (!IsActive)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOperation")
                    .WithData("reason", "Cannot grant from inactive option pool");
            }

            var availableOptions = GetAvailableOptions();
            if (quantity.IsGreaterThan(availableOptions))
            {
                throw new InsufficientSharesException(quantity.Value, availableOptions.Value);
            }
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CapTableConsts.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: CapTableConsts.MaxDescriptionLength
            );
        }
    }
}
