namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for Organization aggregate
    /// </summary>
    public abstract class OrganizationSpecification : Specification<Organization>
    {
    }

    /// <summary>
    /// Specification for active organizations
    /// </summary>
    public class ActiveOrganizationSpecification : OrganizationSpecification
    {
        public override Expression<Func<Organization, bool>> ToExpression()
        {
            return org => org.IsActive;
        }
    }

    /// <summary>
    /// Specification for organizations of a specific type
    /// </summary>
    public class OrganizationOfTypeSpecification : OrganizationSpecification
    {
        private readonly OrganizationType _type;

        public OrganizationOfTypeSpecification(OrganizationType type)
        {
            _type = type;
        }

        public override Expression<Func<Organization, bool>> ToExpression()
        {
            return org => org.Details.Type == _type;
        }
    }

    /// <summary>
    /// Specification for organizations with a specific tag
    /// </summary>
    public class OrganizationWithTagSpecification : OrganizationSpecification
    {
        private readonly string _tag;

        public OrganizationWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<Organization, bool>> ToExpression()
        {
            return org => org.Tags.Contains(_tag);
        }
    }

    /// <summary>
    /// Specification for child organizations
    /// </summary>
    public class ChildOrganizationSpecification : OrganizationSpecification
    {
        private readonly Guid _parentId;

        public ChildOrganizationSpecification(Guid parentId)
        {
            _parentId = parentId;
        }

        public override Expression<Func<Organization, bool>> ToExpression()
        {
            return org => org.ParentOrganizationId == _parentId;
        }
    }

    /// <summary>
    /// Specification for organizations in a specific industry
    /// </summary>
    public class OrganizationInIndustrySpecification : OrganizationSpecification
    {
        private readonly string _industry;

        public OrganizationInIndustrySpecification(string industry)
        {
            _industry = industry;
        }

        public override Expression<Func<Organization, bool>> ToExpression()
        {
            return org => org.Details.Industry == _industry;
        }
    }
}
