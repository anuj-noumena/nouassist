namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing a customer segment for targeted research
    /// </summary>
    public class CustomerSegment : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public SegmentCriteria Criteria { get; private set; }
        public int TargetSampleSize { get; private set; }
        public int ActualSampleSize { get; private set; }

        protected CustomerSegment() { }

        public CustomerSegment(
            Guid id,
            string name,
            string description,
            SegmentCriteria criteria,
            int targetSampleSize,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            SetTargetSampleSize(targetSampleSize);
            TenantId = tenantId;
            ActualSampleSize = 0;
        }

        public void UpdateDetails(string name, string description)
        {
            SetName(name);
            SetDescription(description);
        }

        public void UpdateCriteria(SegmentCriteria criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        public void UpdateTargetSampleSize(int targetSampleSize)
        {
            SetTargetSampleSize(targetSampleSize);
        }

        public void IncrementActualSampleSize()
        {
            ActualSampleSize++;
        }

        public void DecrementActualSampleSize()
        {
            if (ActualSampleSize > 0)
            {
                ActualSampleSize--;
            }
        }

        public bool IsTargetMet()
        {
            return ActualSampleSize >= TargetSampleSize;
        }

        public decimal CompletionPercentage()
        {
            if (TargetSampleSize == 0) return 0;
            return (decimal)ActualSampleSize / TargetSampleSize * 100;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProductDiscoveryConsts.MaxSegmentNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxSegmentDescriptionLength
            );
        }

        private void SetTargetSampleSize(int targetSampleSize)
        {
            if (targetSampleSize <= 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Target sample size must be greater than 0");
            }

            TargetSampleSize = targetSampleSize;
        }
    }
}
