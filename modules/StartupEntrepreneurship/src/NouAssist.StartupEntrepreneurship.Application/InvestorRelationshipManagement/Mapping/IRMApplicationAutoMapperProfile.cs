using AutoMapper;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Mapping
{
    /// <summary>
    /// AutoMapper profile for IRM application layer mappings
    /// </summary>
    public class IRMApplicationAutoMapperProfile : Profile
    {
        public IRMApplicationAutoMapperProfile()
        {
            // --- Investor Profile Mappings ---
            CreateMap<InvestorProfile, InvestorProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactDetails.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.ContactDetails.Phone))
                .ForMember(dest => dest.LinkedIn, opt => opt.MapFrom(src => src.ContactDetails.LinkedIn))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.ContactDetails.Website))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ContactDetails.Address))
                .ForMember(dest => dest.PreferredChannel, opt => opt.MapFrom(src => src.CommunicationPreferences.PreferredChannel))
                .ForMember(dest => dest.UpdateFrequency, opt => opt.MapFrom(src => src.CommunicationPreferences.UpdateFrequency))
                .ForMember(dest => dest.AllowMarketing, opt => opt.MapFrom(src => src.CommunicationPreferences.AllowMarketing))
                .ForMember(dest => dest.CommunicationTags, opt => opt.MapFrom(src => src.CommunicationPreferences.Tags))
                .ForMember(dest => dest.RelationshipScore, opt => opt.MapFrom(src => src.RelationshipScore.Score))
                .ForMember(dest => dest.RelationshipScoreCalculatedAt, opt => opt.MapFrom(src => src.RelationshipScore.LastCalculated))
                .ForMember(dest => dest.RelationshipScoreNotes, opt => opt.MapFrom(src => src.RelationshipScore.Notes));

            CreateMap<InvestorInteraction, InvestorInteractionDto>();

            CreateMap<InvestmentRecord, InvestmentRecordDto>()
                .ForMember(dest => dest.InvestedAmount, opt => opt.MapFrom(src => src.Commitment.InvestedAmount))
                .ForMember(dest => dest.CommittedAmount, opt => opt.MapFrom(src => src.Commitment.CommittedAmount))
                .ForMember(dest => dest.OwnershipPercentage, opt => opt.MapFrom(src => src.Commitment.OwnershipPercentage))
                .ForMember(dest => dest.InvestmentRound, opt => opt.MapFrom(src => src.Commitment.InvestmentRound))
                .ForMember(dest => dest.InvestmentDate, opt => opt.MapFrom(src => src.Commitment.InvestmentDate));

            // --- Investor Update Mappings ---
            CreateMap<InvestorUpdate, InvestorUpdateDto>();

            CreateMap<UpdateSection, UpdateSectionDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Content))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Content.ContentType))
                .ForMember(dest => dest.ContentMetadata, opt => opt.MapFrom(src => src.Content.Metadata));

            CreateMap<MetricSnapshot, MetricSnapshotDto>()
                .ForMember(dest => dest.MetricName, opt => opt.MapFrom(src => src.Metric.Name))
                .ForMember(dest => dest.MetricValue, opt => opt.MapFrom(src => src.Metric.Value))
                .ForMember(dest => dest.MetricUnit, opt => opt.MapFrom(src => src.Metric.Unit))
                .ForMember(dest => dest.MetricPreviousValue, opt => opt.MapFrom(src => src.Metric.PreviousValue))
                .ForMember(dest => dest.MetricTargetValue, opt => opt.MapFrom(src => src.Metric.TargetValue));

            CreateMap<DistributionLog, DistributionLogDto>();

            CreateMap<EngagementRecord, EngagementRecordDto>();

            // --- Due Diligence Data Room Mappings ---
            CreateMap<DueDiligenceDataRoom, DueDiligenceDataRoomDto>()
                .ForMember(dest => dest.HasPasswordAccess, opt => opt.Ignore()); // Calculated based on user context

            CreateMap<SecureDocument, SecureDocumentDto>()
                .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => src.Metadata.FileName))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.Metadata.MimeType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Metadata.FileSize))
                .ForMember(dest => dest.FileHash, opt => opt.MapFrom(src => src.Metadata.Hash))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.Metadata.UploadedAt))
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom(src => src.Metadata.UploadedBy))
                .ForMember(dest => dest.UploadedByName, opt => opt.Ignore()) // Calculated by looking up user
                .ForMember(dest => dest.CanAccess, opt => opt.Ignore()) // Calculated based on user context
                .ForMember(dest => dest.CanDownload, opt => opt.Ignore()) // Calculated based on user context
                .ForMember(dest => dest.AccessCount, opt => opt.MapFrom(src => src.AccessLogs.Count))
                .ForMember(dest => dest.LastAccessDate, opt => opt.MapFrom(src => 
                    src.AccessLogs.Count > 0 ? src.AccessLogs.Max(log => log.AccessedAt) : (DateTime?)null))
                .ForMember(dest => dest.DownloadUrl, opt => opt.Ignore()); // Generated by controller

            // --- Investor Group Mappings ---
            CreateMap<InvestorGroup, InvestorGroupDto>()
                .ForMember(dest => dest.InvestorCount, opt => opt.MapFrom(src => src.InvestorIds.Count));

            // --- Analytics Mappings ---
            CreateMap<InvestorEngagementReport, InvestorEngagementReportDto>();
            CreateMap<TopInvestorEngagement, TopInvestorEngagementDto>();
            CreateMap<InvestorPerformanceReport, InvestorPerformanceReportDto>();
            CreateMap<UpdateEffectivenessReport, UpdateEffectivenessReportDto>();
            CreateMap<UpdatePerformance, UpdatePerformanceDto>();
            CreateMap<InvestorTrend, InvestorTrendDto>()
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction.ToString()));

            // --- Network Intelligence Mappings ---
            CreateMap<NetworkMap, NetworkMapDto>();
            CreateMap<NetworkNode, NetworkNodeDto>();
            CreateMap<NetworkEdge, NetworkEdgeDto>()
                .ForMember(dest => dest.ConnectionType, opt => opt.MapFrom(src => src.Type.ToString()));
            CreateMap<NetworkStatistics, NetworkStatisticsDto>();
            CreateMap<InvestorConnection, InvestorConnectionDto>()
                .ForMember(dest => dest.ConnectionType, opt => opt.MapFrom(src => src.ConnectionType.ToString()));
            CreateMap<NetworkInsights, NetworkInsightsDto>();
            CreateMap<InvestorCluster, InvestorClusterDto>();
            CreateMap<KeyConnector, KeyConnectorDto>();
            CreateMap<InvestorInfluence, InvestorInfluenceDto>();

            // --- Share Link Mappings ---
            CreateMap<SecureShareLink, SecureShareLinkDto>();
        }
    }
}
