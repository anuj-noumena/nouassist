using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Aggregates
{
    public class FinancialGoal : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public GoalType Type { get; private set; }
        public MonetaryValue TargetAmount { get; private set; }
        public DateTime TargetDate { get; private set; }
        public MonetaryValue CurrentAmount { get; private set; }
        public GoalStatus Status { get; private set; }
        public string Description { get; private set; }

        protected FinancialGoal() { }

        public FinancialGoal(
            Guid id,
            Guid? tenantId,
            Guid userId,
            string name,
            GoalType type,
            MonetaryValue targetAmount,
            DateTime targetDate,
            string description = null) : base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Type = type;
            TargetAmount = Check.NotNull(targetAmount, nameof(targetAmount));
            
            if (targetDate <= DateTime.UtcNow)
                throw new BusinessException("FPA:InvalidTargetDate",
                    "Target date must be in the future");
            
            TargetDate = targetDate;
            CurrentAmount = MonetaryValue.Zero(targetAmount.Currency);
            Status = GoalStatus.Active;
            Description = description;
        }

        public void UpdateProgress(MonetaryValue amount)
        {
            if (Status == GoalStatus.Achieved || Status == GoalStatus.Abandoned)
                throw new BusinessException("FPA:GoalNotActive",
                    "Cannot update progress on completed goal");

            CurrentAmount = CurrentAmount.Add(amount);

            if (CurrentAmount.Amount >= TargetAmount.Amount)
            {
                Status = GoalStatus.Achieved;
            }
            else
            {
                UpdateStatus();
            }
        }

        public void Abandon(string reason)
        {
            Status = GoalStatus.Abandoned;
        }

        public decimal GetProgressPercentage()
        {
            if (TargetAmount.Amount == 0)
                return 0;

            return (CurrentAmount.Amount / TargetAmount.Amount) * 100;
        }

        private void UpdateStatus()
        {
            var daysRemaining = (TargetDate - DateTime.UtcNow).Days;
            var progressPercentage = GetProgressPercentage();

            if (daysRemaining <= 0)
            {
                Status = progressPercentage >= 100 ? GoalStatus.Achieved : GoalStatus.Behind;
            }
            else if (progressPercentage >= 90)
            {
                Status = GoalStatus.OnTrack;
            }
            else
            {
                var expectedProgress = 100m - ((daysRemaining / (TargetDate - CreationTime).Days) * 100m);
                Status = progressPercentage >= expectedProgress ? GoalStatus.OnTrack : GoalStatus.Behind;
            }
        }
    }
}
