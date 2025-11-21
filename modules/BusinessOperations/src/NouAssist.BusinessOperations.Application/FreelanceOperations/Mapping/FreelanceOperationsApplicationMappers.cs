using System;
using System.Linq;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.BusinessOperations.FreelanceOperations.Mapping
{
    /* Freelance Project Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FreelanceProjectToFreelanceProjectDtoMapper : MapperBase<FreelanceProject, FreelanceProjectDto>
    {
        [MapperIgnoreTarget(nameof(FreelanceProjectDto.ClientName))]
        public override partial FreelanceProjectDto Map(FreelanceProject source);

        public override partial void Map(FreelanceProject source, FreelanceProjectDto destination);

        public override void AfterMap(FreelanceProject source, FreelanceProjectDto destination)
        {
            destination.HourlyRate = source.HourlyRate?.Amount;
            destination.Currency = source.HourlyRate?.Currency ?? source.Currency;
            destination.ScopeDescription = source.Scope.Description;
            destination.ScopeDeliverables = source.Scope.Deliverables;
            destination.ScopeIsFlexible = source.Scope.IsFlexible;
            destination.TotalHours = source.GetTotalHours();
            destination.TotalBillableHours = source.GetTotalBillableHours();
            destination.TotalAmount = source.CalculateTotalAmount();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TimeEntryToTimeEntryDtoMapper : MapperBase<TimeEntry, TimeEntryDto>
    {
        public override partial TimeEntryDto Map(TimeEntry source);

        public override partial void Map(TimeEntry source, TimeEntryDto destination);

        public override void AfterMap(TimeEntry source, TimeEntryDto destination)
        {
            destination.FormattedDuration = source.GetFormattedDuration();
            destination.DurationInHours = source.GetDurationInHours();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectMilestoneToMilestoneDtoMapper : MapperBase<ProjectMilestone, MilestoneDto>
    {
        public override partial MilestoneDto Map(ProjectMilestone source);

        public override partial void Map(ProjectMilestone source, MilestoneDto destination);

        public override void AfterMap(ProjectMilestone source, MilestoneDto destination)
        {
            destination.Title = source.Details.Title;
            destination.Description = source.Details.Description;
            destination.Price = source.Details.Price;
            destination.Currency = source.Details.Currency;
            destination.AcceptanceCriteria = source.Details.AcceptanceCriteria;
            destination.IsOverdue = source.IsOverdue();
        }
    }

    /* Freelance Client Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FreelanceClientToFreelanceClientDtoMapper : MapperBase<FreelanceClient, FreelanceClientDto>
    {
        [MapperIgnoreTarget(nameof(FreelanceClientDto.ProjectCount))]
        public override partial FreelanceClientDto Map(FreelanceClient source);

        public override partial void Map(FreelanceClient source, FreelanceClientDto destination);

        public override void AfterMap(FreelanceClient source, FreelanceClientDto destination)
        {
            destination.Email = source.ContactInfo.Email;
            destination.Phone = source.ContactInfo.Phone;
            destination.Website = source.ContactInfo.Website;
            destination.Address = source.ContactInfo.Address;
            destination.HasOverdueFollowUps = source.HasOverdueFollowUps();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ClientCommunicationRecordToClientCommunicationRecordDtoMapper : MapperBase<ClientCommunicationRecord, ClientCommunicationRecordDto>
    {
        [MapperIgnoreTarget(nameof(ClientCommunicationRecordDto.UserName))]
        [MapperIgnoreTarget(nameof(ClientCommunicationRecordDto.ProjectName))]
        public override partial ClientCommunicationRecordDto Map(ClientCommunicationRecord source);

        public override partial void Map(ClientCommunicationRecord source, ClientCommunicationRecordDto destination);

        public override void AfterMap(ClientCommunicationRecord source, ClientCommunicationRecordDto destination)
        {
            destination.Timestamp = source.CommunicationDetails.Timestamp;
            destination.Type = source.CommunicationDetails.Type;
            destination.Subject = source.CommunicationDetails.Subject;
            destination.Content = source.CommunicationDetails.Content;
            destination.UserId = source.CommunicationDetails.UserId;
            destination.IsFollowUpOverdue = source.RequiresFollowUp && source.FollowUpDate.HasValue && source.FollowUpDate.Value < DateTime.UtcNow;
        }
    }

    /* Proposal Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProposalToProposalDtoMapper : MapperBase<Proposal, ProposalDto>
    {
        [MapperIgnoreTarget(nameof(ProposalDto.ClientName))]
        [MapperIgnoreTarget(nameof(ProposalDto.ProjectName))]
        [MapperIgnoreTarget(nameof(ProposalDto.CurrentVersion))]
        public override partial ProposalDto Map(Proposal source);

        public override partial void Map(Proposal source, ProposalDto destination);

        public override void AfterMap(Proposal source, ProposalDto destination)
        {
            destination.TemplateName = source.TemplateReference.TemplateName;
            destination.TemplateVersion = source.TemplateReference.Version;
            destination.IsExpired = source.IsExpired();
            destination.FormattedStatus = source.Status.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProposalVersionToProposalVersionDtoMapper : MapperBase<ProposalVersion, ProposalVersionDto>
    {
        [MapperIgnoreTarget(nameof(ProposalVersionDto.CreatedByName))]
        public override partial ProposalVersionDto Map(ProposalVersion source);

        public override partial void Map(ProposalVersion source, ProposalVersionDto destination);

        public override void AfterMap(ProposalVersion source, ProposalVersionDto destination)
        {
            destination.Summary = source.ContentSummary.Summary;
            destination.KeySections = source.ContentSummary.KeySections;
            destination.BillingType = source.Pricing.BillingType;
            destination.HourlyRate = source.Pricing.HourlyRate?.Amount;
            destination.HourlyRateCurrency = source.Pricing.HourlyRate?.Currency;
            destination.FixedPrice = source.Pricing.FixedPrice;
            destination.Currency = source.Pricing.Currency;
            destination.EstimatedHours = source.Pricing.EstimatedHours;
            destination.PaymentTerms = source.Pricing.PaymentTerms;
            destination.DocumentId = source.DocumentId.Value;
            destination.FormattedPrice = source.Pricing.GetFormattedPrice();
        }
    }

    /* Upwork Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UpworkEngagementToUpworkEngagementDtoMapper : MapperBase<UpworkEngagement, UpworkEngagementDto>
    {
        [MapperIgnoreTarget(nameof(UpworkEngagementDto.LinkedProjectName))]
        [MapperIgnoreTarget(nameof(UpworkEngagementDto.LinkedClientName))]
        public override partial UpworkEngagementDto Map(UpworkEngagement source);

        public override partial void Map(UpworkEngagement source, UpworkEngagementDto destination);

        public override void AfterMap(UpworkEngagement source, UpworkEngagementDto destination)
        {
            destination.IsActive = source.IsActive();
            destination.IsCompleted = source.IsCompleted();
            destination.IsPending = source.IsPending();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UpworkCredentialsToUpworkCredentialsDtoMapper : TwoWayMapperBase<UpworkCredentials, UpworkCredentialsDto>
    {
        public override partial UpworkCredentialsDto Map(UpworkCredentials source);

        public override partial void Map(UpworkCredentials source, UpworkCredentialsDto destination);

        public override partial UpworkCredentials ReverseMap(UpworkCredentialsDto source);

        public override partial void ReverseMap(UpworkCredentialsDto source, UpworkCredentials destination);

        public override void AfterMap(UpworkCredentials source, UpworkCredentialsDto destination)
        {
            destination.IsTokenExpired = source.IsTokenExpired();
        }
    }

    /* Client Portal Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FreelanceProjectToClientPortalProjectDtoMapper : MapperBase<FreelanceProject, ClientPortalProjectDto>
    {
        [MapperIgnoreTarget(nameof(ClientPortalProjectDto.CompletionPercentage))]
        public override partial ClientPortalProjectDto Map(FreelanceProject source);

        public override partial void Map(FreelanceProject source, ClientPortalProjectDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectMilestoneToClientPortalMilestoneDtoMapper : MapperBase<ProjectMilestone, ClientPortalMilestoneDto>
    {
        public override partial ClientPortalMilestoneDto Map(ProjectMilestone source);

        public override partial void Map(ProjectMilestone source, ClientPortalMilestoneDto destination);

        public override void AfterMap(ProjectMilestone source, ClientPortalMilestoneDto destination)
        {
            destination.Title = source.Details.Title;
            destination.IsOverdue = source.IsOverdue();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProposalToClientPortalProposalDtoMapper : MapperBase<Proposal, ClientPortalProposalDto>
    {
        [MapperIgnoreTarget(nameof(ClientPortalProposalDto.ProjectName))]
        public override partial ClientPortalProposalDto Map(Proposal source);

        public override partial void Map(Proposal source, ClientPortalProposalDto destination);

        public override void AfterMap(Proposal source, ClientPortalProposalDto destination)
        {
            destination.IsExpired = source.IsExpired();
            destination.DocumentId = source.CurrentVersionId.HasValue
                ? source.Versions.FirstOrDefault(v => v.Id == source.CurrentVersionId.Value)?.DocumentId.Value
                : null;
        }
    }
}
