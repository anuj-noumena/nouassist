using System;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Events;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.Elicitation.Aggregates
{
    /// <summary>
    /// Aggregate for managing stakeholder profiles
    /// </summary>
    public class StakeholderProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public StakeholderRole Role { get; private set; }
        public string Organization { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public InfluenceData InfluenceData { get; private set; }
        public bool IsActive { get; private set; }

        protected StakeholderProfile()
        {
            IsActive = true;
        }

        internal StakeholderProfile(
            Guid id,
            string name,
            string description,
            StakeholderRole role,
            string organization,
            ContactInfo contactInfo,
            InfluenceData influenceData,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Role = role;
            SetOrganization(organization);
            ContactInfo = Check.NotNull(contactInfo, nameof(contactInfo));
            InfluenceData = Check.NotNull(influenceData, nameof(influenceData));
            TenantId = tenantId;
            IsActive = true;

            AddDistributedEvent(new StakeholderProfileCreatedEto
            {
                Id = Id,
                Name = Name,
                Role = Role,
                Organization = Organization,
                Email = ContactInfo.Email,
                TenantId = TenantId
            });
        }

        public static StakeholderProfile Create(
            Guid id,
            string name,
            string description,
            StakeholderRole role,
            string organization,
            ContactInfo contactInfo,
            InfluenceData influenceData,
            Guid? tenantId)
        {
            return new StakeholderProfile(
                id, name, description, role, organization, 
                contactInfo, influenceData, tenantId);
        }

        public void UpdateContactInfo(ContactInfo contactInfo)
        {
            ContactInfo = Check.NotNull(contactInfo, nameof(contactInfo));
        }

        public void UpdateInfluenceData(InfluenceData influenceData)
        {
            InfluenceData = Check.NotNull(influenceData, nameof(influenceData));
        }

        public void UpdateRole(StakeholderRole role)
        {
            Role = role;
        }

        public void UpdateOrganization(string organization)
        {
            SetOrganization(organization);
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ElicitationConsts.MaxStakeholderNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = description?.Length <= ElicitationConsts.MaxStakeholderDescriptionLength 
                ? description 
                : throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Description")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxStakeholderDescriptionLength);
        }

        private void SetOrganization(string organization)
        {
            Organization = organization?.Length <= ElicitationConsts.MaxOrganizationLength
                ? organization
                : throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Organization")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxOrganizationLength);
        }
    }
}
