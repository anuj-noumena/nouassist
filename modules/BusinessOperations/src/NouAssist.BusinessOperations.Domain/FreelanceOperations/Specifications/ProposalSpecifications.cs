namespace NouAssist.BusinessOperations.FreelanceOperations.Specifications
{
    /// <summary>
    /// Base specification for Proposal aggregate
    /// </summary>
    public abstract class ProposalSpecification : Specification<Proposal>
    {
    }

    /// <summary>
    /// Specification for proposals for a specific client
    /// </summary>
    public class ProposalsForClientSpecification : ProposalSpecification
    {
        private readonly Guid _clientId;

        public ProposalsForClientSpecification(Guid clientId)
        {
            _clientId = clientId;
        }

        public override Expression<Func<Proposal, bool>> ToExpression()
        {
            return proposal => proposal.ClientId == _clientId;
        }
    }

    /// <summary>
    /// Specification for proposals with specific status
    /// </summary>
    public class ProposalsByStatusSpecification : ProposalSpecification
    {
        private readonly ProposalStatus _status;

        public ProposalsByStatusSpecification(ProposalStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Proposal, bool>> ToExpression()
        {
            return proposal => proposal.Status == _status;
        }
    }

    /// <summary>
    /// Specification for proposals nearing expiration
    /// </summary>
    public class ProposalsNearingExpirationSpecification : ProposalSpecification
    {
        private readonly int _daysUntilExpiration;

        public ProposalsNearingExpirationSpecification(int daysUntilExpiration = 3)
        {
            _daysUntilExpiration = daysUntilExpiration;
        }

        public override Expression<Func<Proposal, bool>> ToExpression()
        {
            var today = DateTime.UtcNow.Date;
            var thresholdDate = today.AddDays(_daysUntilExpiration);
            
            return proposal => (proposal.Status == ProposalStatus.Sent || 
                               proposal.Status == ProposalStatus.Viewed) &&
                               proposal.ExpirationDate.HasValue &&
                               proposal.ExpirationDate.Value.Date <= thresholdDate &&
                               proposal.ExpirationDate.Value.Date >= today;
        }
    }
}
