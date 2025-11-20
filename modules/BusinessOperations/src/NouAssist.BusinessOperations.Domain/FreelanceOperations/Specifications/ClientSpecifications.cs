namespace NouAssist.BusinessOperations.FreelanceOperations.Specifications
{
    /// <summary>
    /// Base specification for FreelanceClient aggregate
    /// </summary>
    public abstract class ClientSpecification : Specification<FreelanceClient>
    {
    }

    /// <summary>
    /// Specification for active clients
    /// </summary>
    public class ActiveClientsSpecification : ClientSpecification
    {
        public override Expression<Func<FreelanceClient, bool>> ToExpression()
        {
            return client => client.Status == ClientStatus.Active;
        }
    }

    /// <summary>
    /// Specification for clients with pending follow-ups
    /// </summary>
    public class ClientsWithPendingFollowUpsSpecification : ClientSpecification
    {
        private readonly DateTime? _dueBy;

        public ClientsWithPendingFollowUpsSpecification(DateTime? dueBy = null)
        {
            _dueBy = dueBy;
        }

        public override Expression<Func<FreelanceClient, bool>> ToExpression()
        {
            if (_dueBy.HasValue)
            {
                return client => client.CommunicationRecords.Any(r => 
                    r.RequiresFollowUp && 
                    r.FollowUpDate.HasValue && 
                    r.FollowUpDate.Value <= _dueBy.Value);
            }
            
            return client => client.CommunicationRecords.Any(r => r.RequiresFollowUp);
        }
    }

    /// <summary>
    /// Specification for clients by email
    /// </summary>
    public class ClientsByEmailSpecification : ClientSpecification
    {
        private readonly string _email;

        public ClientsByEmailSpecification(string email)
        {
            _email = email;
        }

        public override Expression<Func<FreelanceClient, bool>> ToExpression()
        {
            return client => client.ContactInfo.Email == _email;
        }
    }
}
