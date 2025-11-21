using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Automation.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.CommunicationTemplate.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Interaction.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Organization.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Mapping
{
    /* Lead Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LeadToLeadDtoMapper : MapperBase<Lead, LeadDto>
    {
        public override partial LeadDto Map(Lead source);

        public override partial void Map(Lead source, LeadDto destination);

        public override void AfterMap(Lead source, LeadDto destination)
        {
            destination.ContactInfo = new ContactInfoDto
            {
                Email = source.ContactInfo.Email,
                Phone = source.ContactInfo.Phone,
                AlternativePhone = source.ContactInfo.AlternativePhone,
                Website = source.ContactInfo.Website,
                Address = source.ContactInfo.Address,
                City = source.ContactInfo.City,
                State = source.ContactInfo.State,
                PostalCode = source.ContactInfo.PostalCode,
                Country = source.ContactInfo.Country
            };
            destination.Details = new LeadDetailsDto
            {
                JobTitle = source.Details.JobTitle,
                CompanyName = source.Details.CompanyName,
                Industry = source.Details.Industry,
                Source = source.Details.Source,
                ReferralSource = source.Details.ReferralSource,
                CampaignId = source.Details.CampaignId,
                Notes = source.Details.Notes,
                EstimatedBudget = source.Details.EstimatedBudget
            };
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContactInfoToContactInfoDtoMapper : MapperBase<ContactInfo, ContactInfoDto>
    {
        public override partial ContactInfoDto Map(ContactInfo source);

        public override partial void Map(ContactInfo source, ContactInfoDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LeadDetailsToLeadDetailsDtoMapper : MapperBase<LeadDetails, LeadDetailsDto>
    {
        public override partial LeadDetailsDto Map(LeadDetails source);

        public override partial void Map(LeadDetails source, LeadDetailsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LeadSourceRecordToLeadSourceRecordDtoMapper : MapperBase<LeadSourceRecord, LeadSourceRecordDto>
    {
        public override partial LeadSourceRecordDto Map(LeadSourceRecord source);

        public override partial void Map(LeadSourceRecord source, LeadSourceRecordDto destination);
    }

    /* Contact Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContactToContactDtoMapper : MapperBase<Contact, ContactDto>
    {
        public override partial ContactDto Map(Contact source);

        public override partial void Map(Contact source, ContactDto destination);

        public override void AfterMap(Contact source, ContactDto destination)
        {
            destination.ContactInfo = new ContactInfoDto
            {
                Email = source.ContactInfo.Email,
                Phone = source.ContactInfo.Phone,
                AlternativePhone = source.ContactInfo.AlternativePhone,
                Website = source.ContactInfo.Website,
                Address = source.ContactInfo.Address,
                City = source.ContactInfo.City,
                State = source.ContactInfo.State,
                PostalCode = source.ContactInfo.PostalCode,
                Country = source.ContactInfo.Country
            };
        }
    }

    /* Organization Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class OrganizationToOrganizationDtoMapper : MapperBase<Organization, OrganizationDto>
    {
        public override partial OrganizationDto Map(Organization source);

        public override partial void Map(Organization source, OrganizationDto destination);

        public override void AfterMap(Organization source, OrganizationDto destination)
        {
            destination.ContactInfo = new ContactInfoDto
            {
                Email = source.ContactInfo.Email,
                Phone = source.ContactInfo.Phone,
                AlternativePhone = source.ContactInfo.AlternativePhone,
                Website = source.ContactInfo.Website,
                Address = source.ContactInfo.Address,
                City = source.ContactInfo.City,
                State = source.ContactInfo.State,
                PostalCode = source.ContactInfo.PostalCode,
                Country = source.ContactInfo.Country
            };
            destination.Details = new OrganizationInfoDto
            {
                Type = source.Details.Type,
                Industry = source.Details.Industry,
                EmployeeCount = source.Details.EmployeeCount,
                AnnualRevenue = source.Details.AnnualRevenue,
                TaxId = source.Details.TaxId,
                Notes = source.Details.Notes
            };
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class OrganizationInfoToOrganizationInfoDtoMapper : MapperBase<OrganizationInfo, OrganizationInfoDto>
    {
        public override partial OrganizationInfoDto Map(OrganizationInfo source);

        public override partial void Map(OrganizationInfo source, OrganizationInfoDto destination);
    }

    /* InteractionLog Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InteractionLogToInteractionLogDtoMapper : MapperBase<InteractionLog, InteractionLogDto>
    {
        public override partial InteractionLogDto Map(InteractionLog source);

        public override partial void Map(InteractionLog source, InteractionLogDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CommunicationRecordToCommunicationRecordDtoMapper : MapperBase<CommunicationRecord, CommunicationRecordDto>
    {
        public override partial CommunicationRecordDto Map(CommunicationRecord source);

        public override partial void Map(CommunicationRecord source, CommunicationRecordDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FollowUpTaskToFollowUpTaskDtoMapper : MapperBase<FollowUpTask, FollowUpTaskDto>
    {
        public override partial FollowUpTaskDto Map(FollowUpTask source);

        public override partial void Map(FollowUpTask source, FollowUpTaskDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentIdToDocumentIdDtoMapper : MapperBase<DocumentId, DocumentIdDto>
    {
        public override partial DocumentIdDto Map(DocumentId source);

        public override partial void Map(DocumentId source, DocumentIdDto destination);
    }

    /* AutomationWorkflow Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AutomationWorkflowToAutomationWorkflowDtoMapper : MapperBase<AutomationWorkflow, AutomationWorkflowDto>
    {
        public override partial AutomationWorkflowDto Map(AutomationWorkflow source);

        public override partial void Map(AutomationWorkflow source, AutomationWorkflowDto destination);

        public override void AfterMap(AutomationWorkflow source, AutomationWorkflowDto destination)
        {
            destination.TriggerCondition = new AutomationTriggerConditionDto
            {
                TriggerType = source.TriggerCondition.TriggerType,
                EntityType = source.TriggerCondition.EntityType,
                FieldName = source.TriggerCondition.FieldName,
                ExpectedValue = source.TriggerCondition.ExpectedValue,
                ScheduleExpression = source.TriggerCondition.ScheduleExpression
            };
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AutomationTriggerConditionToAutomationTriggerConditionDtoMapper : MapperBase<AutomationTriggerCondition, AutomationTriggerConditionDto>
    {
        public override partial AutomationTriggerConditionDto Map(AutomationTriggerCondition source);

        public override partial void Map(AutomationTriggerCondition source, AutomationTriggerConditionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowStepToWorkflowStepDtoMapper : MapperBase<WorkflowStep, WorkflowStepDto>
    {
        public override partial WorkflowStepDto Map(WorkflowStep source);

        public override partial void Map(WorkflowStep source, WorkflowStepDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowExecutionRecordToWorkflowExecutionRecordDtoMapper : MapperBase<WorkflowExecutionRecord, WorkflowExecutionRecordDto>
    {
        public override partial WorkflowExecutionRecordDto Map(WorkflowExecutionRecord source);

        public override partial void Map(WorkflowExecutionRecord source, WorkflowExecutionRecordDto destination);
    }

    /* CommunicationTemplate Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CommunicationTemplateToCommunicationTemplateDtoMapper : MapperBase<CommunicationTemplate, CommunicationTemplateDto>
    {
        public override partial CommunicationTemplateDto Map(CommunicationTemplate source);

        public override partial void Map(CommunicationTemplate source, CommunicationTemplateDto destination);
    }
}
