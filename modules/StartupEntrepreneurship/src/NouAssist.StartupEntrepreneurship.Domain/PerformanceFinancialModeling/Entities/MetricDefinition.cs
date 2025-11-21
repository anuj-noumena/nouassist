using System;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Defines how a metric should be calculated and tracked
    /// </summary>
    public class MetricDefinition : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MetricType Type { get; private set; }
        public MetricDataType DataType { get; private set; }
        public string Unit { get; private set; }
        public CalculationRule CalculationRule { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? CategoryId { get; private set; }

        protected MetricDefinition() { }

        public MetricDefinition(Guid id, string name, string description, MetricType type, 
                              MetricDataType dataType, CalculationRule calculationRule, 
                              string unit = null, Guid? categoryId = null, Guid? tenantId = null) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            DataType = dataType;
            CalculationRule = calculationRule ?? throw new ArgumentNullException(nameof(calculationRule));
            Unit = unit?.Trim();
            CategoryId = categoryId;
            TenantId = tenantId;
            IsActive = true;
        }

        public void UpdateDefinition(string name, string description, CalculationRule calculationRule, string unit = null)
        {
            SetName(name);
            SetDescription(description);
            CalculationRule = calculationRule ?? throw new ArgumentNullException(nameof(calculationRule));
            Unit = unit?.Trim();
        }

        public void SetCategory(Guid? categoryId)
        {
            CategoryId = categoryId;
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
                maxLength: PerformanceFinancialModelingConsts.MaxDefinitionNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength
            );
        }
    }
}
