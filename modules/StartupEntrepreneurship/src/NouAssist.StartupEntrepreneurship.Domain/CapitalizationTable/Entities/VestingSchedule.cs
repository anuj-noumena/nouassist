using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities
{
    /// <summary>
    /// Represents a vesting schedule for securities
    /// </summary>
    public class VestingSchedule : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid SecurityHolderId { get; private set; }
        public Guid SecurityIssuanceId { get; private set; }
        public DateTime GrantDate { get; private set; }
        public VestingTerms VestingTerms { get; private set; }
        public ShareQuantity TotalShares { get; private set; }
        public ShareQuantity VestedShares { get; private set; }
        public VestingStatus Status { get; private set; }
        public DateTime? LastVestingDate { get; private set; }
        public DateTime? ExercisedDate { get; private set; }

        protected VestingSchedule() { }

        internal VestingSchedule(
            Guid id,
            Guid securityHolderId,
            Guid securityIssuanceId,
            DateTime grantDate,
            VestingTerms vestingTerms,
            ShareQuantity totalShares,
            Guid? tenantId = null) : base(id)
        {
            SecurityHolderId = securityHolderId;
            SecurityIssuanceId = securityIssuanceId;
            GrantDate = grantDate;
            VestingTerms = vestingTerms ?? throw new ArgumentNullException(nameof(vestingTerms));
            TotalShares = totalShares ?? throw new ArgumentNullException(nameof(totalShares));
            VestedShares = new ShareQuantity(0);
            Status = VestingStatus.Unvested;
            TenantId = tenantId;
        }

        public ShareQuantity CalculateVestedShares(DateTime asOfDate)
        {
            var vestedAmount = VestingTerms.CalculateVestedAmount(TotalShares.Value, GrantDate, asOfDate);
            return new ShareQuantity(vestedAmount);
        }

        internal void UpdateVesting(DateTime asOfDate)
        {
            var newVestedShares = CalculateVestedShares(asOfDate);
            
            if (newVestedShares.Value > VestedShares.Value)
            {
                VestedShares = newVestedShares;
                LastVestingDate = asOfDate;

                if (VestedShares.Value >= TotalShares.Value)
                {
                    Status = VestingStatus.Vested;
                }
                else if (VestedShares.Value > 0)
                {
                    Status = VestingStatus.Vesting;
                }
            }
        }

        internal void MarkAsExercised(DateTime exercisedDate)
        {
            if (Status == VestingStatus.Expired)
            {
                throw new BusinessException("CapTable:ErrorMessage:OptionExpired");
            }

            Status = VestingStatus.Exercised;
            ExercisedDate = exercisedDate;
        }

        internal void MarkAsExpired()
        {
            Status = VestingStatus.Expired;
        }

        public ShareQuantity GetExercisableShares(DateTime asOfDate)
        {
            UpdateVesting(asOfDate);
            return VestedShares;
        }
    }
}
