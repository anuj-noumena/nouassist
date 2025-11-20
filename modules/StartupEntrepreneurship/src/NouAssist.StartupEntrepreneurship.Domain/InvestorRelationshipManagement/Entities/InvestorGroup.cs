namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing a group of investors
    /// </summary>
    public class InvestorGroup : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        
        private readonly List<Guid> _investorIds;
        public IReadOnlyCollection<Guid> InvestorIds => _investorIds.AsReadOnly();

        protected InvestorGroup()
        {
            _investorIds = new List<Guid>();
        }

        public InvestorGroup(
            Guid id,
            string name,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            Description = description;
            IsActive = true;
            TenantId = tenantId;
            _investorIds = new List<Guid>();
        }

        public void UpdateName(string name)
        {
            SetName(name);
        }

        public void UpdateDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                IRMConsts.InvestorGroup.MaxDescriptionLength
            );
        }

        public void AddInvestor(Guid investorId)
        {
            if (_investorIds.Contains(investorId))
                return;

            _investorIds.Add(investorId);
        }

        public void RemoveInvestor(Guid investorId)
        {
            _investorIds.Remove(investorId);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: IRMConsts.InvestorGroup.MaxNameLength
            );
        }
    }
}
