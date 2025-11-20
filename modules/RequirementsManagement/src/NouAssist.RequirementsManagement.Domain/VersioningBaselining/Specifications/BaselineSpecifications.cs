namespace NouAssist.RequirementsManagement.VersioningBaselining.Specifications
{
    /// <summary>
    /// Specifications for BaselineDefinition aggregate
    /// </summary>
    public static class BaselineSpecifications
    {
        /// <summary>
        /// Specification for baselines that can be activated
        /// </summary>
        public class ActivatableBaselineSpecification : Specification<BaselineDefinition>
        {
            public override Expression<Func<BaselineDefinition, bool>> ToExpression()
            {
                return baseline => 
                    baseline.Status == BaselineStatus.Draft && 
                    baseline.Items.Any();
            }
        }
        
        /// <summary>
        /// Specification for baselines that can be archived
        /// </summary>
        public class ArchivableBaselineSpecification : Specification<BaselineDefinition>
        {
            public override Expression<Func<BaselineDefinition, bool>> ToExpression()
            {
                return baseline => baseline.Status == BaselineStatus.Active;
            }
        }
        
        /// <summary>
        /// Specification for baselines that can be modified
        /// </summary>
        public class ModifiableBaselineSpecification : Specification<BaselineDefinition>
        {
            public override Expression<Func<BaselineDefinition, bool>> ToExpression()
            {
                return baseline => baseline.Status == BaselineStatus.Draft;
            }
        }
    }
}
