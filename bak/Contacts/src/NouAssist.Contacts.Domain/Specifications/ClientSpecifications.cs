using System;
using System.Linq.Expressions;
using NouAssist.Contacts.Aggregates;
using NouAssist.Contacts.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Contacts.Specifications
{
    public class ActiveClientsSpecification : Specification<Client>
    {
        public override Expression<Func<Client, bool>> ToExpression() =>
            c => c.Status == ClientStatus.Active && c.IsActive;
    }

    public class PotentialClientsSpecification : Specification<Client>
    {
        public override Expression<Func<Client, bool>> ToExpression() =>
            c => c.Status == ClientStatus.Potential;
    }

    public class ClientsNeedingReviewSpecification : Specification<Client>
    {
        private readonly DateTime _referenceDate;

        public ClientsNeedingReviewSpecification(DateTime? referenceDate = null)
        {
            _referenceDate = referenceDate ?? DateTime.UtcNow;
        }

        public override Expression<Func<Client, bool>> ToExpression() =>
            c => c.IsActive && 
                 c.NextReviewDate.HasValue && 
                 c.NextReviewDate.Value <= _referenceDate;
    }
}

