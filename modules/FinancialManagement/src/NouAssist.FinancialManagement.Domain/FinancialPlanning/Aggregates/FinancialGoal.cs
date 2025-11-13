using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
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
        public decimal ProgressPercentage => CalculateProgressPercentage();
        public GoalStatus Status { get; private set; }
        public string Description { get; private set; }

        private readonly List<GoalContribution> _contributions = new();
        public IReadOnlyCollection<GoalContribution> Contributions => _contributions.AsReadOnly();

        protected FinancialGoal() { } // For ORM

        public FinancialGoal(
            Guid id,
            Guid? tenantId,
            Guid userId,
            string name,
            GoalType type,
            MonetaryValue targetAmount,
            DateTime targetDate,
            string description = null)
            : base(id)
        {
            if (targetDate <= DateTime.Today)
                throw new BusinessException("FinancialPlanning:InvalidTargetDate",
                    "Target date must be in the future");

            if (targetAmount.IsNegative || targetAmount.IsZero)
                throw new BusinessException("FinancialPlanning:InvalidTargetAmount",
                    "Target amount must be positive");

            TenantId = tenantId;
            UserId = userId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
            Type = type;
            TargetAmount = targetAmount;
            TargetDate = targetDate;
            CurrentAmount = new MonetaryValue(0, targetAmount.Currency);
            Status = GoalStatus.Active;
            Description = description;
        }

        public void AddContribution(MonetaryValue amount, DateTime contributionDate, string notes = null)
        {
            if (Status == GoalStatus.Achieved || Status == GoalStatus.Abandoned)
                throw new BusinessException("FinancialPlanning:CannotContributeToCompletedGoal",
                    "Cannot add contributions to achieved or abandoned goals");

            var contribution = new GoalContribution(Id, amount, contributionDate, notes);
            _contributions.Add(contribution);

            CurrentAmount = CurrentAmount.Add(amount);
            UpdateStatus();
        }

        public void UpdateTargetAmount(MonetaryValue newAmount, string justification)
        {
            if (Status == GoalStatus.Achieved)
                throw new BusinessException("FinancialPlanning:CannotModifyAchievedGoal",
                    "Cannot modify achieved goal");

            TargetAmount = newAmount;
            UpdateStatus();
        }

        public void UpdateTargetDate(DateTime newDate, string justification)
        {
            if (newDate <= DateTime.Today)
                throw new BusinessException("FinancialPlanning:InvalidTargetDate",
                    "Target date must be in the future");

            TargetDate = newDate;
            UpdateStatus();
        }

        public void Abandon(string reason)
        {
            if (Status == GoalStatus.Achieved)
                throw new BusinessException("FinancialPlanning:CannotAbandonAchievedGoal",
                    "Cannot abandon achieved goal");

            Status = GoalStatus.Abandoned;
        }

        private void UpdateStatus()
        {
            if (CurrentAmount.Amount >= TargetAmount.Amount)
            {
                Status = GoalStatus.Achieved;
                return;
            }

            var daysRemaining = (TargetDate - DateTime.Today).Days;
            var amountRemaining = TargetAmount.Amount - CurrentAmount.Amount;
            var requiredMonthlyContribution = amountRemaining / (daysRemaining / 30m);

            // Calculate if on track based on current progress vs expected progress
            var elapsedDays = (DateTime.Today - CreationTime.Date).Days;
            var totalDays = (TargetDate - CreationTime.Date).Days;
            var expectedProgress = (decimal)elapsedDays / totalDays;
            var actualProgress = CurrentAmount.Amount / TargetAmount.Amount;

            if (actualProgress >= expectedProgress)
            {
                Status = GoalStatus.OnTrack;
            }
            else
            {
                Status = GoalStatus.Behind;
            }
        }

        private decimal CalculateProgressPercentage()
        {
            if (TargetAmount.IsZero) return 0;
            return Math.Min((CurrentAmount.Amount / TargetAmount.Amount) * 100, 100);
        }
    }
}
