using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications
{
    /// <summary>
    /// Specification for active revenue models
    /// </summary>
    public class ActiveRevenueModelSpecification : BusinessModelManagementSpecification<RevenueModel>
    {
        public override Expression<Func<RevenueModel, bool>> ToExpression()
        {
            return model => model.IsActive;
        }
    }

    /// <summary>
    /// Specification for revenue models of a specific type
    /// </summary>
    public class RevenueModelTypeSpecification : BusinessModelManagementSpecification<RevenueModel>
    {
        private readonly RevenueModelType _type;

        public RevenueModelTypeSpecification(RevenueModelType type)
        {
            _type = type;
        }

        public override Expression<Func<RevenueModel, bool>> ToExpression()
        {
            return model => model.Type == _type;
        }
    }

    /// <summary>
    /// Specification for revenue models with active pricing models
    /// </summary>
    public class RevenueModelWithActivePricingSpecification : BusinessModelManagementSpecification<RevenueModel>
    {
        public override Expression<Func<RevenueModel, bool>> ToExpression()
        {
            return model => model.PricingModels.Any(pm => pm.IsActive);
        }
    }

    /// <summary>
    /// Specification for revenue models with recent scenarios
    /// </summary>
    public class RevenueModelWithRecentScenariosSpecification : BusinessModelManagementSpecification<RevenueModel>
    {
        private readonly DateTime _cutoffDate;

        public RevenueModelWithRecentScenariosSpecification(DateTime cutoffDate)
        {
            _cutoffDate = cutoffDate;
        }

        public override Expression<Func<RevenueModel, bool>> ToExpression()
        {
            return model => model.Scenarios.Any(s => s.CreatedAt >= _cutoffDate);
        }
    }
}
