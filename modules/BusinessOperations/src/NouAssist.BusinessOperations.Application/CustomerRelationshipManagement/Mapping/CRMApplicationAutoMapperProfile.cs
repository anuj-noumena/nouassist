using AutoMapper;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Mapping
{
    public class CRMApplicationAutoMapperProfile : Profile
    {
        public CRMApplicationAutoMapperProfile()
        {
            // Lead mappings
            CreateMap<Lead, LeadDto>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));
                
            CreateMap<ContactInfo, ContactInfoDto>();
            CreateMap<LeadDetails, LeadDetailsDto>();
            CreateMap<LeadSourceRecord, LeadSourceRecordDto>();
            
            // Contact mappings
            CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo));
                
            // Organization mappings
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));
                
            CreateMap<OrganizationInfo, OrganizationInfoDto>();
            
            // InteractionLog mappings
            CreateMap<InteractionLog, InteractionLogDto>();
            CreateMap<CommunicationRecord, CommunicationRecordDto>();
            CreateMap<FollowUpTask, FollowUpTaskDto>();
            CreateMap<DocumentId, DocumentIdDto>();
            
            // AutomationWorkflow mappings
            CreateMap<AutomationWorkflow, AutomationWorkflowDto>()
                .ForMember(dest => dest.TriggerCondition, opt => opt.MapFrom(src => src.TriggerCondition));
                
            CreateMap<AutomationTriggerCondition, AutomationTriggerConditionDto>();
            CreateMap<WorkflowStep, WorkflowStepDto>();
            CreateMap<WorkflowExecutionRecord, WorkflowExecutionRecordDto>();
            
            // CommunicationTemplate mappings
            CreateMap<CommunicationTemplate, CommunicationTemplateDto>();
            
            // CreateDTOs to Value Objects - used in CreateAsync methods
            CreateMap<CreateContactInfoDto, ContactInfo>()
                .ConstructUsing(dto => new ContactInfo(
                    dto.Email,
                    dto.Phone,
                    dto.AlternativePhone,
                    dto.Website,
                    dto.Address,
                    dto.City,
                    dto.State,
                    dto.PostalCode,
                    dto.Country
                ));
                
            CreateMap<CreateLeadDetailsDto, LeadDetails>()
                .ConstructUsing(dto => new LeadDetails(
                    dto.JobTitle,
                    dto.CompanyName,
                    dto.Industry,
                    dto.Source,
                    dto.ReferralSource,
                    dto.CampaignId,
                    dto.Notes,
                    dto.EstimatedBudget
                ));
                
            CreateMap<CreateOrganizationInfoDto, OrganizationInfo>()
                .ConstructUsing(dto => new OrganizationInfo(
                    dto.Type,
                    dto.Industry,
                    dto.EmployeeCount,
                    dto.AnnualRevenue,
                    dto.TaxId,
                    dto.Notes
                ));
                
            CreateMap<CreateAutomationTriggerConditionDto, AutomationTriggerCondition>()
                .ConstructUsing(dto => new AutomationTriggerCondition(
                    dto.TriggerType,
                    dto.EntityType,
                    dto.FieldName,
                    dto.ExpectedValue,
                    dto.ScheduleExpression
                ));
        }
    }
}
