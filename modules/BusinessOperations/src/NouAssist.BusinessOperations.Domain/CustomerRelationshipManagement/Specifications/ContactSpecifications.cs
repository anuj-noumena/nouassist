namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for Contact aggregate
    /// </summary>
    public abstract class ContactSpecification : Specification<Contact>
    {
    }

    /// <summary>
    /// Specification for active contacts
    /// </summary>
    public class ActiveContactSpecification : ContactSpecification
    {
        public override Expression<Func<Contact, bool>> ToExpression()
        {
            return contact => contact.IsActive;
        }
    }

    /// <summary>
    /// Specification for contacts belonging to a specific organization
    /// </summary>
    public class ContactInOrganizationSpecification : ContactSpecification
    {
        private readonly Guid _organizationId;
        private readonly bool _primaryOnly;

        public ContactInOrganizationSpecification(Guid organizationId, bool primaryOnly = false)
        {
            _organizationId = organizationId;
            _primaryOnly = primaryOnly;
        }

        public override Expression<Func<Contact, bool>> ToExpression()
        {
            if (_primaryOnly)
            {
                return contact => 
                    contact.OrganizationId == _organizationId && 
                    contact.IsPrimaryContact;
            }
            
            return contact => contact.OrganizationId == _organizationId;
        }
    }

    /// <summary>
    /// Specification for contacts in a specific group
    /// </summary>
    public class ContactInGroupSpecification : ContactSpecification
    {
        private readonly Guid _groupId;

        public ContactInGroupSpecification(Guid groupId)
        {
            _groupId = groupId;
        }

        public override Expression<Func<Contact, bool>> ToExpression()
        {
            return contact => contact.GroupIds.Contains(_groupId);
        }
    }

    /// <summary>
    /// Specification for contacts with a specific tag
    /// </summary>
    public class ContactWithTagSpecification : ContactSpecification
    {
        private readonly string _tag;

        public ContactWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<Contact, bool>> ToExpression()
        {
            return contact => contact.Tags.Contains(_tag);
        }
    }

    /// <summary>
    /// Specification for contacts converted from leads
    /// </summary>
    public class ContactFromLeadSpecification : ContactSpecification
    {
        public override Expression<Func<Contact, bool>> ToExpression()
        {
            return contact => contact.ConvertedFromLeadId != null;
        }
    }
}
