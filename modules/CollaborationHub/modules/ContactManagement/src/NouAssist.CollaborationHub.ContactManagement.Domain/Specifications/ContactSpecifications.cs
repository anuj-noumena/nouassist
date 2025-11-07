using System;
using System.Linq.Expressions;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using NouAssist.CollaborationHub.ContactManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CollaborationHub.ContactManagement.Specifications
{
    public class ActiveContactsSpecification : Specification<Contact>
    {
        public override Expression<Func<Contact, bool>> ToExpression() =>
            c => c.IsActive && !c.IsDeleted;
    }

    public class ContactsNeedingFollowupSpecification : Specification<Contact>
    {
        private readonly DateTime _referenceDate;

        public ContactsNeedingFollowupSpecification(DateTime? referenceDate = null)
        {
            _referenceDate = referenceDate ?? DateTime.UtcNow;
        }

        public override Expression<Func<Contact, bool>> ToExpression() =>
            c => c.IsActive && 
                 c.NextFollowupDate.HasValue && 
                 c.NextFollowupDate.Value <= _referenceDate;
    }

    public class ContactsByRelationshipSpecification : Specification<Contact>
    {
        private readonly ContactRelationship _relationship;

        public ContactsByRelationshipSpecification(ContactRelationship relationship)
        {
            _relationship = relationship;
        }

        public override Expression<Func<Contact, bool>> ToExpression() =>
            c => c.Relationship == _relationship && c.IsActive;
    }
}
