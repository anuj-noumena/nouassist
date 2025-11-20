namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Specifications
{
    /// <summary>
    /// Base specification class for FinancialModel aggregate
    /// </summary>
    public abstract class FinancialModelSpecification : Specification<FinancialModel>
    {
    }

    /// <summary>
    /// Specification for active financial models
    /// </summary>
    public class ActiveFinancialModelsSpecification : FinancialModelSpecification
    {
        public override Expression<Func<FinancialModel, bool>> ToExpression()
        {
            return model => model.IsActive && 
                          (model.Status == ModelStatus.Active || model.Status == ModelStatus.Approved);
        }
    }

    /// <summary>
    /// Specification for models with specific status
    /// </summary>
    public class ModelsByStatusSpecification : FinancialModelSpecification
    {
        private readonly ModelStatus _status;

        public ModelsByStatusSpecification(ModelStatus status)
        {
            _status = status;
        }

        public override Expression<Func<FinancialModel, bool>> ToExpression()
        {
            return model => model.Status == _status;
        }
    }

    /// <summary>
    /// Specification for models ready for calculation
    /// </summary>
    public class CalculableModelsSpecification : FinancialModelSpecification
    {
        public override Expression<Func<FinancialModel, bool>> ToExpression()
        {
            return model => model.IsActive && 
                          model.Status != ModelStatus.Archived && 
                          model.Status != ModelStatus.Deprecated &&
                          model.Scenarios.Any() && 
                          model.Assumptions.Any();
        }
    }

    /// <summary>
    /// Specification for models with recent projections
    /// </summary>
    public class ModelsWithRecentProjectionsSpecification : FinancialModelSpecification
    {
        private readonly DateTime _cutoffDate;

        public ModelsWithRecentProjectionsSpecification(int daysBack = 30)
        {
            _cutoffDate = DateTime.UtcNow.AddDays(-daysBack);
        }

        public override Expression<Func<FinancialModel, bool>> ToExpression()
        {
            return model => model.IsActive && 
                          model.Projections.Any(p => p.CalculatedAt >= _cutoffDate);
        }
    }

    /// <summary>
    /// Specification for models by projection years
    /// </summary>
    public class ModelsByProjectionYearsSpecification : FinancialModelSpecification
    {
        private readonly int _minYears;
        private readonly int _maxYears;

        public ModelsByProjectionYearsSpecification(int minYears, int maxYears)
        {
            _minYears = minYears;
            _maxYears = maxYears;
        }

        public override Expression<Func<FinancialModel, bool>> ToExpression()
        {
            return model => model.IsActive && 
                          model.ProjectionYears >= _minYears && 
                          model.ProjectionYears <= _maxYears;
        }
    }
}
