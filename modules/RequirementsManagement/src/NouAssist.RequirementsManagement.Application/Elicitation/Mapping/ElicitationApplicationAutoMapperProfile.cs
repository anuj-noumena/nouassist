using AutoMapper;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos;
using NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos;
using NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Services;
using NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;

namespace NouAssist.RequirementsManagement.Elicitation.Mapping
{
    public class ElicitationApplicationAutoMapperProfile : Profile
    {
        public ElicitationApplicationAutoMapperProfile()
        {
            // StakeholderProfile mappings
            CreateMap<StakeholderProfile, StakeholderProfileDto>();
            CreateMap<ContactInfo, ContactInfoDto>();
            CreateMap<InfluenceData, InfluenceDataDto>()
                .ForMember(dest => dest.InfluenceCategory, opt => opt.MapFrom(src => src.GetInfluenceCategory()))
                .ForMember(dest => dest.InterestCategory, opt => opt.MapFrom(src => src.GetInterestCategory()));

            // ElicitationSession mappings
            CreateMap<ElicitationSession, ElicitationSessionDto>()
                .ForMember(dest => dest.ArtifactsGenerated, opt => opt.MapFrom(src => src.GeneratedArtifacts.Count));
            CreateMap<SessionSchedule, SessionScheduleDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));
            CreateMap<ElicitationSessionParticipant, ElicitationSessionParticipantDto>();

            // Survey mappings
            CreateMap<ElicitationSession, SurveyDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsDeployed, opt => opt.MapFrom(src => src.SurveyDefinition != null && src.SurveyDefinition.IsDeployed))
                .ForMember(dest => dest.DeploymentDate, opt => opt.MapFrom(src => src.SurveyDefinition != null ? src.SurveyDefinition.DeploymentDate : null))
                .ForMember(dest => dest.Questionnaire, opt => opt.MapFrom(src => src.SurveyDefinition != null ? src.SurveyDefinition.Questionnaire : null))
                .ForMember(dest => dest.ResponseCount, opt => opt.MapFrom(src => src.SurveyDefinition != null ? src.SurveyDefinition.GetResponseCount() : 0))
                .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src => src.SurveyDefinition != null ? src.SurveyDefinition.GetCompletionRate() : 0.0))
                .ForMember(dest => dest.TargetStakeholderIds, opt => opt.MapFrom(src => src.Participants.Select(p => p.StakeholderId).ToList()));

            CreateMap<SurveyDefinition, SurveyDefinitionDto>()
                .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.Questionnaire.TotalQuestions))
                .ForMember(dest => dest.ResponseCount, opt => opt.MapFrom(src => src.GetResponseCount()))
                .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src => src.GetCompletionRate()));

            CreateMap<QuestionnaireStructure, QuestionnaireStructureDto>()
                .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.TotalQuestions));
            CreateMap<SurveyPage, SurveyPageDto>();
            CreateMap<SurveyQuestion, SurveyQuestionDto>();

            CreateMap<SurveyResponse, SurveyResponseDto>()
                .ForMember(dest => dest.ResponseTime, opt => opt.MapFrom(src => src.GetResponseTime()))
                .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.IsComplete(0))); // Will need total questions context

            CreateMap<ResponseData, ResponseDataDto>()
                .ForMember(dest => dest.ResponseTime, opt => opt.MapFrom(src => src.ResponseTime));

            CreateMap<SurveyAnalytics, SurveyAnalyticsDto>();

            // Interview mappings
            CreateMap<ElicitationSession, InterviewDto>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Schedule.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Schedule.EndTime))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Schedule.Location))
                .ForMember(dest => dest.IntervieweeId, opt => opt.MapFrom(src => src.Participants.FirstOrDefault() != null ? src.Participants.First().StakeholderId : Guid.Empty))
                .ForMember(dest => dest.Record, opt => opt.MapFrom(src => src.InterviewRecord));

            CreateMap<InterviewRecord, InterviewRecordDetailDto>()
                .ForMember(dest => dest.TranscriptText, opt => opt.MapFrom(src => src.TranscriptRecording.TranscriptText))
                .ForMember(dest => dest.RecordingPath, opt => opt.MapFrom(src => src.TranscriptRecording.RecordingPath))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.TranscriptRecording.FileName))
                .ForMember(dest => dest.FileSizeBytes, opt => opt.MapFrom(src => src.TranscriptRecording.FileSizeBytes))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.TranscriptRecording.MimeType))
                .ForMember(dest => dest.HasTranscript, opt => opt.MapFrom(src => src.TranscriptRecording.HasTranscript))
                .ForMember(dest => dest.HasRecording, opt => opt.MapFrom(src => src.TranscriptRecording.HasRecording));

            CreateMap<InterviewRecord, InterviewRecordDto>()
                .ForMember(dest => dest.HasTranscript, opt => opt.MapFrom(src => src.HasTranscript))
                .ForMember(dest => dest.HasRecording, opt => opt.MapFrom(src => src.HasRecording))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.TranscriptRecording.FileName))
                .ForMember(dest => dest.FileSizeBytes, opt => opt.MapFrom(src => src.TranscriptRecording.FileSizeBytes));

            // ElicitationArtifact mappings
            CreateMap<ElicitationArtifact, ElicitationArtifactDto>();
            CreateMap<StoryText, StoryTextDto>()
                .ForMember(dest => dest.FullText, opt => opt.MapFrom(src => src.ToString()));
            CreateMap<UseCaseModel, UseCaseModelDto>();
            CreateMap<UseCaseStep, UseCaseStepDto>();
            CreateMap<PrePostCondition, PrePostConditionDto>();
            CreateMap<EventDefinition, EventDefinitionDto>();
            CreateMap<IdeaTag, IdeaTagDto>();

            // BrainstormingIdea mappings
            CreateMap<BrainstormingIdea, BrainstormingIdeaDto>();

            // Session statistics mapping
            CreateMap<SessionStatistics, SessionStatisticsDto>();
        }
    }
}
