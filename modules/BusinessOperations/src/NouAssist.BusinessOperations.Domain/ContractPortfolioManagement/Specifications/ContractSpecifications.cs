using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications
{
    /// <summary>
    /// Base specification class for Contract aggregate
    /// </summary>
    public abstract class ContractSpecification : Specification<ContractAggregate>
    {
    }

    /// <summary>
    /// Specification that identifies active contracts
    /// </summary>
    public class ActiveContractSpecification : ContractSpecification
    {
        private readonly DateTime _referenceDate;

        public ActiveContractSpecification(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        public override Expression<Func<ContractAggregate, bool>> ToExpression()
        {
            return contract => contract.Status == ContractStatus.Active &&
                              contract.EffectiveDate <= _referenceDate &&
                              (!contract.ExpirationDate.HasValue || contract.ExpirationDate.Value >= _referenceDate);
        }
    }

    /// <summary>
    /// Specification that identifies contracts that will expire soon
    /// </summary>
    public class ExpiringContractSpecification : ContractSpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _daysThreshold;

        public ExpiringContractSpecification(DateTime referenceDate, int daysThreshold)
        {
            _referenceDate = referenceDate;
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<ContractAggregate, bool>> ToExpression()
        {
            return contract => contract.Status == ContractStatus.Active &&
                              contract.ExpirationDate.HasValue &&
                              contract.ExpirationDate.Value >= _referenceDate &&
                              DbFunctions.DiffDays(_referenceDate, contract.ExpirationDate.Value) <= _daysThreshold;
        }
    }

    /// <summary>
    /// Specification that identifies contracts needing attention (pending approval, expiring soon, etc.)
    /// </summary>
    public class ContractsNeedingAttentionSpecification : ContractSpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _expirationDaysThreshold;

        public ContractsNeedingAttentionSpecification(DateTime referenceDate, int expirationDaysThreshold)
        {
            _referenceDate = referenceDate;
            _expirationDaysThreshold = expirationDaysThreshold;
        }

        public override Expression<Func<ContractAggregate, bool>> ToExpression()
        {
            var expiringSpec = new ExpiringContractSpecification(_referenceDate, _expirationDaysThreshold);
            
            return contract => 
                // Contracts in approval flow
                (contract.Status == ContractStatus.UnderReview || 
                 contract.Status == ContractStatus.Negotiation ||
                 contract.Status == ContractStatus.PendingSignature) ||
                // Or contracts expiring soon
                (expiringSpec.ToExpression().Compile()(contract));
        }
    }
}
