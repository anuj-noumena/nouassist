namespace NouAssist.RequirementsManagement.VersioningBaselining.Specifications
{
    /// <summary>
    /// Specifications for RequirementBranch aggregate
    /// </summary>
    public static class BranchSpecifications
    {
        /// <summary>
        /// Specification for branches that can be merged
        /// </summary>
        public class MergeableBranchSpecification : Specification<RequirementBranch>
        {
            public override Expression<Func<RequirementBranch, bool>> ToExpression()
            {
                return branch => branch.Status == BranchStatus.Active;
            }
        }
        
        /// <summary>
        /// Specification for branches that can be abandoned
        /// </summary>
        public class AbandonableBranchSpecification : Specification<RequirementBranch>
        {
            public override Expression<Func<RequirementBranch, bool>> ToExpression()
            {
                return branch => branch.Status == BranchStatus.Active;
            }
        }
    }
}
