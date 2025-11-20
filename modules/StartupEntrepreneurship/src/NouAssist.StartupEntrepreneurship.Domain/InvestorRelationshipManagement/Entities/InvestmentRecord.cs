namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing an investment record for an investor
    /// </summary>
    public class InvestmentRecord : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid InvestorId { get; private set; }
        public CommitmentDetails Commitment { get; private set; }
        public string DocumentReference { get; private set; }
        public bool IsActive { get; private set; }

        protected InvestmentRecord() { }

        internal InvestmentRecord(
            Guid id,
            Guid investorId,
            CommitmentDetails commitment,
            string documentReference = null,
            Guid? tenantId = null) : base(id)
        {
            InvestorId = investorId;
            Commitment = Check.NotNull(commitment, nameof(commitment));
            DocumentReference = documentReference;
            IsActive = true;
            TenantId = tenantId;
        }

        public void UpdateCommitment(CommitmentDetails newCommitment)
        {
            Commitment = Check.NotNull(newCommitment, nameof(newCommitment));
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
