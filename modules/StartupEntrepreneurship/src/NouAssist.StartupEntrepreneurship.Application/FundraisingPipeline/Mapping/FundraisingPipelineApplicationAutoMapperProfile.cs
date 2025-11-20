using AutoMapper;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Mapping
{
    public class FundraisingPipelineApplicationAutoMapperProfile : Profile
    {
        public FundraisingPipelineApplicationAutoMapperProfile()
        {
            // Prospect mapping
            CreateMap<Prospect, ProspectDto>()
                .ForMember(dest => dest.FundraisingRoundName, opt => opt.Ignore());
            
            CreateMap<ContactInfo, ContactInfoDto>();
            CreateMap<PipelineStatus, PipelineStatusDto>();
            CreateMap<ProspectScore, ProspectScoreDto>();
            CreateMap<OpportunityForecastValue, OpportunityForecastDto>();
            
            CreateMap<InteractionLog, InteractionLogDto>();
            
            // CreateProspect DTO to value objects
            CreateMap<CreateContactInfoDto, ContactInfo>()
                .ConstructUsing(src => new ContactInfo(src.Email, src.Phone, src.Address));
            
            // UpdateProspect DTO to value objects
            CreateMap<UpdateContactInfoDto, ContactInfo>()
                .ConstructUsing(src => new ContactInfo(src.Email, src.Phone, src.Address));
            
            // FundraisingRound mapping
            CreateMap<FundraisingRound, FundraisingRoundDto>()
                .ForMember(dest => dest.ProspectCount, opt => opt.Ignore());
            
            CreateMap<PipelineStage, PipelineStageDto>()
                .ForMember(dest => dest.ProspectCount, opt => opt.Ignore());
            
            // PitchDeck mapping
            CreateMap<PitchDeck, PitchDeckDto>()
                .ForMember(dest => dest.FundraisingRoundName, opt => opt.Ignore())
                .ForMember(dest => dest.PresentationCount, opt => opt.MapFrom(src => src.Presentations.Count))
                .ForMember(dest => dest.FeedbackCount, opt => opt.MapFrom(src => src.Feedbacks.Count))
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore());
            
            CreateMap<PitchVersion, PitchVersionDto>();
            CreateMap<DeckContent, DeckContentDto>();
            
            CreateMap<PitchPresentation, PitchPresentationDto>()
                .ForMember(dest => dest.VersionName, opt => opt.Ignore())
                .ForMember(dest => dest.ProspectName, opt => opt.Ignore())
                .ForMember(dest => dest.ProspectCompany, opt => opt.Ignore());
            
            CreateMap<PresentationDetails, PresentationDetailsDto>();
            
            CreateMap<FeedbackRecord, FeedbackRecordDto>()
                .ForMember(dest => dest.ProspectName, opt => opt.Ignore());
            
            CreateMap<FeedbackData, FeedbackDataDto>();
            
            // CreatePitchVersion DTO to value objects
            CreateMap<CreateDeckContentDto, DeckContent>()
                .ConstructUsing(src => new DeckContent(
                    src.StorageUrl,
                    src.ThumbnailUrl,
                    src.SlideCount,
                    src.ContentHash,
                    src.SlideTopics ?? new List<string>()));
            
            // UpdatePitchVersion DTO to value objects
            CreateMap<UpdateDeckContentDto, DeckContent>()
                .ConstructUsing(src => new DeckContent(
                    src.StorageUrl,
                    src.ThumbnailUrl,
                    src.SlideCount,
                    src.ContentHash,
                    src.SlideTopics ?? new List<string>()));
            
            // CreatePresentation DTO to value objects
            CreateMap<CreatePresentationDetailsDto, PresentationDetails>()
                .ConstructUsing(src => new PresentationDetails(
                    src.PresentationDate,
                    src.Duration,
                    src.Location,
                    src.MeetingFormat,
                    src.Attendees ?? new List<string>()));
            
            // UpdatePresentation DTO to value objects
            CreateMap<UpdatePresentationDetailsDto, PresentationDetails>()
                .ConstructUsing(src => new PresentationDetails(
                    src.PresentationDate,
                    src.Duration,
                    src.Location,
                    src.MeetingFormat,
                    src.Attendees ?? new List<string>()));
            
            // CreateFeedback DTO to value objects
            CreateMap<CreateFeedbackDataDto, FeedbackData>()
                .ConstructUsing(src => new FeedbackData(
                    src.Sentiment,
                    src.Rating,
                    src.CategoryFeedback ?? new Dictionary<string, string>(),
                    src.StrengthPoints ?? new List<string>(),
                    src.ImprovementAreas ?? new List<string>()));
            
            // UpdateFeedback DTO to value objects
            CreateMap<UpdateFeedbackDataDto, FeedbackData>()
                .ConstructUsing(src => new FeedbackData(
                    src.Sentiment,
                    src.Rating,
                    src.CategoryFeedback ?? new Dictionary<string, string>(),
                    src.StrengthPoints ?? new List<string>(),
                    src.ImprovementAreas ?? new List<string>()));
            
            // PitchEffectivenessResult to DTO
            CreateMap<PitchEffectivenessResult, PitchEffectivenessDto>();
            
            // TermSheet mapping
            CreateMap<TermSheetOffer, TermSheetOfferDto>()
                .ForMember(dest => dest.ProspectName, opt => opt.Ignore())
                .ForMember(dest => dest.ProspectCompany, opt => opt.Ignore())
                .ForMember(dest => dest.FundraisingRoundName, opt => opt.Ignore());
            
            CreateMap<TermComparisonData, TermComparisonDataDto>();
            
            CreateMap<TermSheetClause, TermSheetClauseDto>();
            CreateMap<ClauseText, ClauseTextDto>();
            
            CreateMap<NegotiationNote, NegotiationNoteDto>()
                .ForMember(dest => dest.RelatedClauseName, opt => opt.Ignore());
            
            // CreateTermSheetOffer DTO to value objects
            CreateMap<CreateTermComparisonDataDto, TermComparisonData>()
                .ConstructUsing(src => new TermComparisonData(
                    src.PreMoneyValuation,
                    src.InvestmentAmount,
                    src.HasLiquidationPreference,
                    src.LiquidationMultiple,
                    src.HasParticipationRights,
                    src.HasProRataRights,
                    src.HasAntiDilutionProtection,
                    src.InvestorRights ?? new Dictionary<string, bool>()));
            
            // UpdateTermSheetOffer DTO to value objects
            CreateMap<UpdateTermComparisonDataDto, TermComparisonData>()
                .ConstructUsing(src => new TermComparisonData(
                    src.PreMoneyValuation,
                    src.InvestmentAmount,
                    src.HasLiquidationPreference,
                    src.LiquidationMultiple,
                    src.HasParticipationRights,
                    src.HasProRataRights,
                    src.HasAntiDilutionProtection,
                    src.InvestorRights ?? new Dictionary<string, bool>()));
            
            // CreateTermSheetClause DTO to value objects
            CreateMap<CreateClauseTextDto, ClauseText>()
                .ConstructUsing(src => new ClauseText(
                    src.Content,
                    src.IsNegotiable,
                    src.Category,
                    src.FormattedHtml));
            
            // UpdateTermSheetClause DTO to value objects
            CreateMap<UpdateClauseTextDto, ClauseText>()
                .ConstructUsing(src => new ClauseText(
                    src.Content,
                    src.IsNegotiable,
                    src.Category,
                    src.FormattedHtml));
            
            // TermSheetComparison domain results to DTOs
            CreateMap<TermSheetComparisonResult, TermSheetComparisonResultDto>();
            CreateMap<TermSheetSummary, TermSheetSummaryDto>();
            CreateMap<TermSheetRankingResult, TermSheetRankingResultDto>()
                .ForMember(dest => dest.RankedTermSheets, opt => 
                    opt.MapFrom(src => src.GetRankedTermSheets()))
                .ForMember(dest => dest.TopTermSheetId, opt => 
                    opt.MapFrom(src => src.GetTopTermSheet()));
        }
    }
}
