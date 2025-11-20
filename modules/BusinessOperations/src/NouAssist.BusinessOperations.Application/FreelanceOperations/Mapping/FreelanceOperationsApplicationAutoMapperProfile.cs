using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using System;
using AutoMapper;

namespace NouAssist.BusinessOperations.FreelanceOperations.Mapping
{
    public class FreelanceOperationsApplicationAutoMapperProfile : Profile
    {
        public FreelanceOperationsApplicationAutoMapperProfile()
        {
            // Project Mappings
            CreateMap<FreelanceProject, FreelanceProjectDto>()
                .ForMember(dest => dest.HourlyRate, opt => 
                    opt.MapFrom(src => src.HourlyRate != null ? src.HourlyRate.Amount : (decimal?)null))
                .ForMember(dest => dest.Currency, opt => 
                    opt.MapFrom(src => src.HourlyRate != null ? src.HourlyRate.Currency : src.Currency))
                .ForMember(dest => dest.ScopeDescription, opt => 
                    opt.MapFrom(src => src.Scope.Description))
                .ForMember(dest => dest.ScopeDeliverables, opt => 
                    opt.MapFrom(src => src.Scope.Deliverables))
                .ForMember(dest => dest.ScopeIsFlexible, opt => 
                    opt.MapFrom(src => src.Scope.IsFlexible))
                .ForMember(dest => dest.TotalHours, opt => 
                    opt.MapFrom(src => src.GetTotalHours()))
                .ForMember(dest => dest.TotalBillableHours, opt => 
                    opt.MapFrom(src => src.GetTotalBillableHours()))
                .ForMember(dest => dest.TotalAmount, opt => 
                    opt.MapFrom(src => src.CalculateTotalAmount()))
                .ForMember(dest => dest.ClientName, opt => opt.Ignore()); // Will be set by app service
                
            // Time Entry Mappings
            CreateMap<TimeEntry, TimeEntryDto>()
                .ForMember(dest => dest.FormattedDuration, opt => 
                    opt.MapFrom(src => src.GetFormattedDuration()))
                .ForMember(dest => dest.DurationInHours, opt => 
                    opt.MapFrom(src => src.GetDurationInHours()));
                
            // Project Milestone Mappings
            CreateMap<ProjectMilestone, MilestoneDto>()
                .ForMember(dest => dest.Title, opt => 
                    opt.MapFrom(src => src.Details.Title))
                .ForMember(dest => dest.Description, opt => 
                    opt.MapFrom(src => src.Details.Description))
                .ForMember(dest => dest.Price, opt => 
                    opt.MapFrom(src => src.Details.Price))
                .ForMember(dest => dest.Currency, opt => 
                    opt.MapFrom(src => src.Details.Currency))
                .ForMember(dest => dest.AcceptanceCriteria, opt => 
                    opt.MapFrom(src => src.Details.AcceptanceCriteria))
                .ForMember(dest => dest.IsOverdue, opt => 
                    opt.MapFrom(src => src.IsOverdue()));
                    
            // Value Object Mappings for creation
            CreateMap<CreateFreelanceProjectDto, ProjectScopeDescription>()
                .ConstructUsing(src => new ProjectScopeDescription(
                    src.ScopeDescription,
                    src.ScopeDeliverables,
                    src.ScopeIsFlexible));
                    
            CreateMap<CreateFreelanceProjectDto, HourlyRate>()
                .ConstructUsing(src => src.HourlyRate.HasValue && !string.IsNullOrEmpty(src.HourlyRateCurrency) 
                    ? new HourlyRate(src.HourlyRate.Value, src.HourlyRateCurrency) 
                    : null);
                    
            CreateMap<CreateMilestoneDto, MilestoneDetails>()
                .ConstructUsing(src => new MilestoneDetails(
                    src.Title,
                    src.Description,
                    src.Price,
                    src.Currency,
                    src.AcceptanceCriteria));
                
            // Client Mappings
            CreateMap<FreelanceClient, FreelanceClientDto>()
                .ForMember(dest => dest.Email, opt => 
                    opt.MapFrom(src => src.ContactInfo.Email))
                .ForMember(dest => dest.Phone, opt => 
                    opt.MapFrom(src => src.ContactInfo.Phone))
                .ForMember(dest => dest.Website, opt => 
                    opt.MapFrom(src => src.ContactInfo.Website))
                .ForMember(dest => dest.Address, opt => 
                    opt.MapFrom(src => src.ContactInfo.Address))
                .ForMember(dest => dest.HasOverdueFollowUps, opt => 
                    opt.MapFrom(src => src.HasOverdueFollowUps()))
                .ForMember(dest => dest.ProjectCount, opt => opt.Ignore()); // Will be set by service
                
            // Value Object Mappings for Client
            CreateMap<CreateFreelanceClientDto, ClientContactInfo>()
                .ConstructUsing(src => new ClientContactInfo(
                    src.Email,
                    src.Phone,
                    src.Website,
                    src.Address));
                    
            CreateMap<UpdateFreelanceClientDto, ClientContactInfo>()
                .ConstructUsing(src => new ClientContactInfo(
                    src.Email,
                    src.Phone,
                    src.Website,
                    src.Address));
                    
            // Communication Record Mappings
            CreateMap<ClientCommunicationRecord, ClientCommunicationRecordDto>()
                .ForMember(dest => dest.Timestamp, opt => 
                    opt.MapFrom(src => src.CommunicationDetails.Timestamp))
                .ForMember(dest => dest.Type, opt => 
                    opt.MapFrom(src => src.CommunicationDetails.Type))
                .ForMember(dest => dest.Subject, opt => 
                    opt.MapFrom(src => src.CommunicationDetails.Subject))
                .ForMember(dest => dest.Content, opt => 
                    opt.MapFrom(src => src.CommunicationDetails.Content))
                .ForMember(dest => dest.UserId, opt => 
                    opt.MapFrom(src => src.CommunicationDetails.UserId))
                .ForMember(dest => dest.IsFollowUpOverdue, opt => 
                    opt.MapFrom(src => src.RequiresFollowUp && src.FollowUpDate.HasValue && src.FollowUpDate.Value < DateTime.UtcNow))
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.ProjectName, opt => opt.Ignore()); // Will be set by service
                
            // Proposal Mappings
            CreateMap<Proposal, ProposalDto>()
                .ForMember(dest => dest.TemplateName, opt => 
                    opt.MapFrom(src => src.TemplateReference.TemplateName))
                .ForMember(dest => dest.TemplateVersion, opt => 
                    opt.MapFrom(src => src.TemplateReference.Version))
                .ForMember(dest => dest.IsExpired, opt => 
                    opt.MapFrom(src => src.IsExpired()))
                .ForMember(dest => dest.FormattedStatus, opt => 
                    opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ClientName, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.ProjectName, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.CurrentVersion, opt => opt.Ignore()); // Will be set by service
                
            // Value Object Mappings for Proposal
            CreateMap<CreateProposalDto, ProposalTemplateReference>()
                .ConstructUsing(src => new ProposalTemplateReference(
                    src.TemplateId,
                    src.TemplateName,
                    src.TemplateVersion));
                    
            CreateMap<UpdateProposalDto, ProposalTemplateReference>()
                .ConstructUsing(src => new ProposalTemplateReference(
                    src.TemplateId,
                    src.TemplateName,
                    src.TemplateVersion));
                    
            // Proposal Version Mappings
            CreateMap<ProposalVersion, ProposalVersionDto>()
                .ForMember(dest => dest.Summary, opt => 
                    opt.MapFrom(src => src.ContentSummary.Summary))
                .ForMember(dest => dest.KeySections, opt => 
                    opt.MapFrom(src => src.ContentSummary.KeySections))
                .ForMember(dest => dest.BillingType, opt => 
                    opt.MapFrom(src => src.Pricing.BillingType))
                .ForMember(dest => dest.HourlyRate, opt => 
                    opt.MapFrom(src => src.Pricing.HourlyRate != null ? src.Pricing.HourlyRate.Amount : (decimal?)null))
                .ForMember(dest => dest.HourlyRateCurrency, opt => 
                    opt.MapFrom(src => src.Pricing.HourlyRate != null ? src.Pricing.HourlyRate.Currency : null))
                .ForMember(dest => dest.FixedPrice, opt => 
                    opt.MapFrom(src => src.Pricing.FixedPrice))
                .ForMember(dest => dest.Currency, opt => 
                    opt.MapFrom(src => src.Pricing.Currency))
                .ForMember(dest => dest.EstimatedHours, opt => 
                    opt.MapFrom(src => src.Pricing.EstimatedHours))
                .ForMember(dest => dest.PaymentTerms, opt => 
                    opt.MapFrom(src => src.Pricing.PaymentTerms))
                .ForMember(dest => dest.DocumentId, opt => 
                    opt.MapFrom(src => src.DocumentId.Value))
                .ForMember(dest => dest.FormattedPrice, opt => 
                    opt.MapFrom(src => src.Pricing.GetFormattedPrice()))
                .ForMember(dest => dest.CreatedByName, opt => opt.Ignore()); // Will be set by service
                
            // Value Object Mappings for Proposal Version
            CreateMap<CreateProposalVersionDto, ProposalContentSummary>()
                .ConstructUsing(src => new ProposalContentSummary(
                    src.Summary,
                    src.KeySections));
                    
            CreateMap<CreateProposalVersionDto, PricingStructure>()
                .ConstructUsing(src => {
                    if (src.BillingType == Enums.BillingType.Hourly && src.HourlyRate.HasValue && !string.IsNullOrEmpty(src.HourlyRateCurrency))
                    {
                        return PricingStructure.CreateHourlyRate(
                            new HourlyRate(src.HourlyRate.Value, src.HourlyRateCurrency),
                            src.EstimatedHours,
                            src.PaymentTerms);
                    }
                    else if (src.BillingType == Enums.BillingType.FixedPrice && src.FixedPrice.HasValue && !string.IsNullOrEmpty(src.FixedPriceCurrency))
                    {
                        return PricingStructure.CreateFixedPrice(
                            src.FixedPrice.Value,
                            src.FixedPriceCurrency,
                            src.PaymentTerms);
                    }
                    return null;
                });
                
            CreateMap<CreateProposalVersionDto, DocumentId>()
                .ConstructUsing(src => new DocumentId(src.DocumentId));
                
            // Upwork Mappings
            CreateMap<UpworkEngagement, UpworkEngagementDto>()
                .ForMember(dest => dest.IsActive, opt => 
                    opt.MapFrom(src => src.IsActive()))
                .ForMember(dest => dest.IsCompleted, opt => 
                    opt.MapFrom(src => src.IsCompleted()))
                .ForMember(dest => dest.IsPending, opt => 
                    opt.MapFrom(src => src.IsPending()))
                .ForMember(dest => dest.LinkedProjectName, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.LinkedClientName, opt => opt.Ignore()); // Will be set by service
                
            CreateMap<UpworkCredentials, UpworkCredentialsDto>()
                .ForMember(dest => dest.IsTokenExpired, opt => 
                    opt.MapFrom(src => src.IsTokenExpired()));
                    
            CreateMap<UpworkCredentialsDto, UpworkCredentials>()
                .ConstructUsing(src => new UpworkCredentials(
                    src.ApiKey,
                    src.ApiSecret,
                    src.AccessToken,
                    src.RefreshToken,
                    src.TokenExpiration));
                    
            // Client Portal Mappings
            CreateMap<FreelanceProject, ClientPortalProjectDto>()
                .ForMember(dest => dest.CompletionPercentage, opt => opt.Ignore()); // Will be calculated in service
                
            CreateMap<ProjectMilestone, ClientPortalMilestoneDto>()
                .ForMember(dest => dest.Title, opt => 
                    opt.MapFrom(src => src.Details.Title))
                .ForMember(dest => dest.IsOverdue, opt => 
                    opt.MapFrom(src => src.IsOverdue()));
                    
            CreateMap<Proposal, ClientPortalProposalDto>()
                .ForMember(dest => dest.IsExpired, opt => 
                    opt.MapFrom(src => src.IsExpired()))
                .ForMember(dest => dest.DocumentId, opt => 
                    opt.MapFrom(src => src.CurrentVersionId.HasValue ? 
                        src.Versions.FirstOrDefault(v => v.Id == src.CurrentVersionId.Value)?.DocumentId.Value : null))
                .ForMember(dest => dest.ProjectName, opt => opt.Ignore()); // Will be set by service
        }
    }
}
