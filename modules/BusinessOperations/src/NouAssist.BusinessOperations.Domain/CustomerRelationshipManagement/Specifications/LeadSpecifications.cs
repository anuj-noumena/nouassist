namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for Lead aggregate
    /// </summary>
    public abstract class LeadSpecification : Specification<Lead>
    {
    }

    /// <summary>
    /// Specification for active leads that can be contacted
    /// </summary>
    public class ContactableLeadSpecification : LeadSpecification
    {
        public override Expression<Func<Lead, bool>> ToExpression()
        {
            return lead => 
                lead.Status != LeadStatus.Converted && 
                lead.Status != LeadStatus.Archived &&
                lead.Status != LeadStatus.Unqualified &&
                !string.IsNullOrEmpty(lead.ContactInfo.Email);
        }
    }

    /// <summary>
    /// Specification for leads ready for qualification
    /// </summary>
    public class QualificationReadyLeadSpecification : LeadSpecification
    {
        private readonly int _minimumScore;

        public QualificationReadyLeadSpecification(int minimumScore = 50)
        {
            _minimumScore = minimumScore;
        }

        public override Expression<Func<Lead, bool>> ToExpression()
        {
            return lead => 
                lead.Status == LeadStatus.Contacted && 
                lead.Score >= _minimumScore;
        }
    }

    /// <summary>
    /// Specification for leads that can be converted to contacts
    /// </summary>
    public class ConvertibleLeadSpecification : LeadSpecification
    {
        public override Expression<Func<Lead, bool>> ToExpression()
        {
            return lead => 
                lead.Status == LeadStatus.Qualified && 
                lead.ConvertedToContactId == null;
        }
    }

    /// <summary>
    /// Specification for leads with a specific tag
    /// </summary>
    public class LeadWithTagSpecification : LeadSpecification
    {
        private readonly string _tag;

        public LeadWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<Lead, bool>> ToExpression()
        {
            return lead => lead.Tags.Contains(_tag);
        }
    }

    /// <summary>
    /// Specification for leads from a specific source
    /// </summary>
    public class LeadFromSourceSpecification : LeadSpecification
    {
        private readonly LeadSource _source;

        public LeadFromSourceSpecification(LeadSource source)
        {
            _source = source;
        }

        public override Expression<Func<Lead, bool>> ToExpression()
        {
            return lead => 
                lead.Details.Source == _source;
        }
    }
}
