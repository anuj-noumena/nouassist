namespace NouAssist.RequirementsManagement.Elicitation.Specifications
{
    /// <summary>
    /// Base specification class for StakeholderProfile aggregate
    /// </summary>
    public abstract class StakeholderSpecification : Specification<StakeholderProfile>
    {
    }

    /// <summary>
    /// Specification that identifies active stakeholders with high influence
    /// </summary>
    public class HighInfluenceStakeholderSpecification : StakeholderSpecification
    {
        private readonly int _minimumInfluenceLevel;

        public HighInfluenceStakeholderSpecification(int minimumInfluenceLevel = 7)
        {
            _minimumInfluenceLevel = minimumInfluenceLevel;
        }

        public override Expression<Func<StakeholderProfile, bool>> ToExpression()
        {
            return stakeholder => stakeholder.IsActive &&
                                 stakeholder.InfluenceData.InfluenceLevel >= _minimumInfluenceLevel;
        }
    }

    /// <summary>
    /// Specification that identifies stakeholders by role
    /// </summary>
    public class StakeholdersByRoleSpecification : StakeholderSpecification
    {
        private readonly StakeholderRole[] _roles;

        public StakeholdersByRoleSpecification(params StakeholderRole[] roles)
        {
            _roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public override Expression<Func<StakeholderProfile, bool>> ToExpression()
        {
            return stakeholder => stakeholder.IsActive &&
                                 _roles.Contains(stakeholder.Role);
        }
    }

    /// <summary>
    /// Specification that identifies key stakeholders (high influence and high interest)
    /// </summary>
    public class KeyStakeholderSpecification : StakeholderSpecification
    {
        private readonly int _minimumInfluence;
        private readonly int _minimumInterest;

        public KeyStakeholderSpecification(int minimumInfluence = 6, int minimumInterest = 6)
        {
            _minimumInfluence = minimumInfluence;
            _minimumInterest = minimumInterest;
        }

        public override Expression<Func<StakeholderProfile, bool>> ToExpression()
        {
            return stakeholder => stakeholder.IsActive &&
                                 stakeholder.InfluenceData.InfluenceLevel >= _minimumInfluence &&
                                 stakeholder.InfluenceData.InterestLevel >= _minimumInterest;
        }
    }
}
