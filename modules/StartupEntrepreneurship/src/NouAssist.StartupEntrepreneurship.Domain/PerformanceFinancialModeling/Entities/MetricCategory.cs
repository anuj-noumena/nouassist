using System;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Categorizes metrics for better organization
    /// </summary>
    public class MetricCategory : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Color { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        protected MetricCategory() { }

        public MetricCategory(Guid id, string name, string description = null, 
                            string color = null, int displayOrder = 0, Guid? tenantId = null) : base(id)
        {
            SetName(name);
            Description = description?.Trim();
            Color = color?.Trim();
            DisplayOrder = displayOrder;
            TenantId = tenantId;
            IsActive = true;
        }

        public void UpdateInfo(string name, string description = null, string color = null, int displayOrder = 0)
        {
            SetName(name);
            Description = description?.Trim();
            Color = color?.Trim();
            DisplayOrder = displayOrder;
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
                maxLength: PerformanceFinancialModelingConsts.MaxCategoryNameLength
            );
        }
    }
}
