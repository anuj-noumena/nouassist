using System.Collections.Generic;
using AutoMapper;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.MentorProfiles.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Mapping
{
    public class KnowledgeInnovationApplicationAutoMapperProfile : Profile
    {
        public KnowledgeInnovationApplicationAutoMapperProfile()
        {
            // KnowledgeAsset mappings
            CreateMap<KnowledgeAsset, KnowledgeAssetDto>()
                .ForMember(dest => dest.CurrentContent, opt => opt.MapFrom(src => 
                    src.ContentVersions.Count > 0 ? src.CurrentContent : null));
            
            CreateMap<KnowledgeAsset, KnowledgeAssetListDto>();
            CreateMap<AssetContent, AssetContentDto>();
            CreateMap<AuthorInfo, AuthorInfoDto>();
            
            CreateMap<CreateKnowledgeAssetDto, KnowledgeAsset>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ContentVersions, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => new AuthorInfo(
                    src.Author.UserId,
                    src.Author.Name,
                    src.Author.Title,
                    src.Author.AvatarUrl
                )));
            
            CreateMap<CreateAuthorInfoDto, AuthorInfo>()
                .ConstructUsing(src => new AuthorInfo(
                    src.UserId,
                    src.Name,
                    src.Title,
                    src.AvatarUrl
                ));

            // LearningResource mappings
            CreateMap<LearningResource, LearningResourceDto>();
            CreateMap<LearningResource, LearningResourceListDto>()
                .ForMember(dest => dest.EstimatedDurationMinutes, opt => 
                    opt.MapFrom(src => src.Description.EstimatedDurationMinutes))
                .ForMember(dest => dest.UserCompletionStatus, opt => opt.Ignore());
            
            CreateMap<LearningModuleDescription, LearningModuleDescriptionDto>();
            CreateMap<UserProgress, UserProgressDto>();
            
            CreateMap<CreateLearningResourceDto, LearningResource>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserProgressRecords, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new LearningModuleDescription(
                    src.Description.Overview,
                    src.Description.LearningObjectives,
                    src.Description.PrerequisiteKnowledge,
                    src.Description.EstimatedDurationMinutes
                )));
            
            CreateMap<CreateLearningModuleDescriptionDto, LearningModuleDescription>()
                .ConstructUsing(src => new LearningModuleDescription(
                    src.Overview,
                    src.LearningObjectives,
                    src.PrerequisiteKnowledge,
                    src.EstimatedDurationMinutes
                ));
            
            CreateMap<UpdateLearningModuleDescriptionDto, LearningModuleDescription>()
                .ConstructUsing(src => new LearningModuleDescription(
                    src.Overview,
                    src.LearningObjectives,
                    src.PrerequisiteKnowledge,
                    src.EstimatedDurationMinutes
                ));

            // Idea mappings
            CreateMap<Idea, IdeaDto>();
            CreateMap<Idea, IdeaListDto>()
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Source.SourceName))
                .ForMember(dest => dest.EvaluationCount, opt => opt.MapFrom(src => src.Evaluations.Count));
            
            CreateMap<IdeaSource, IdeaSourceDto>();
            CreateMap<IdeaEvaluation, IdeaEvaluationDto>();
            CreateMap<EvaluationScore, EvaluationScoreDto>();
            
            CreateMap<CreateIdeaDto, Idea>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Evaluations, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => new IdeaSource(
                    src.Source.SourceName,
                    src.Source.SourceDescription,
                    src.Source.SourcedDate
                )));
            
            CreateMap<CreateIdeaSourceDto, IdeaSource>()
                .ConstructUsing(src => new IdeaSource(
                    src.SourceName,
                    src.SourceDescription,
                    src.SourcedDate
                ));
            
            CreateMap<CreateEvaluationScoreDto, EvaluationScore>()
                .ConstructUsing(src => new EvaluationScore(
                    src.CriteriaName,
                    src.Score,
                    src.Justification
                ));

            // DecisionRecord mappings
            CreateMap<DecisionRecord, DecisionRecordDto>();
            CreateMap<DecisionRecord, DecisionRecordListDto>()
                .ForMember(dest => dest.AlternativesCount, opt => opt.MapFrom(src => src.Alternatives.Count))
                .ForMember(dest => dest.HasOutcome, opt => opt.MapFrom(src => src.Outcome != null));
            
            CreateMap<DecisionContext, DecisionContextDto>();
            CreateMap<DecisionRationale, DecisionRationaleDto>();
            CreateMap<DecisionAlternative, DecisionAlternativeDto>();
            CreateMap<DecisionOutcome, DecisionOutcomeDto>();
            CreateMap<OutcomeImpact, OutcomeImpactDto>();
            
            CreateMap<CreateDecisionRecordDto, DecisionRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DecisionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Rationale, opt => opt.Ignore())
                .ForMember(dest => dest.Context, opt => opt.Ignore())
                .ForMember(dest => dest.Alternatives, opt => opt.Ignore())
                .ForMember(dest => dest.Outcome, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()));
            
            CreateMap<CreateDecisionContextDto, DecisionContext>()
                .ForMember(dest => dest.DecisionRecordId, opt => opt.Ignore())
                .ForMember(dest => dest.ContextCapturedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Stakeholders, opt => opt.MapFrom(src => src.Stakeholders ?? new List<string>()))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Constraints ?? new List<string>()));
            
            CreateMap<AddAlternativeDto, DecisionAlternative>()
                .ForMember(dest => dest.DecisionRecordId, opt => opt.Ignore())
                .ForMember(dest => dest.IsChosen, opt => opt.Ignore());
            
            CreateMap<CreateOutcomeImpactDto, OutcomeImpact>()
                .ConstructUsing(src => new OutcomeImpact(
                    src.Area,
                    src.Description,
                    src.Magnitude
                ));

            // MentorProfile mappings
            CreateMap<MentorProfile, MentorProfileDto>();
            CreateMap<MentorProfile, MentorProfileListDto>()
                .ForMember(dest => dest.RelationshipsCount, opt => opt.MapFrom(src => src.Relationships.Count))
                .ForMember(dest => dest.AdviceCount, opt => opt.MapFrom(src => src.AdviceRecords.Count));
            
            CreateMap<NetworkRelationship, NetworkRelationshipDto>();
            CreateMap<AdviceRecord, AdviceRecordDto>();
            
            CreateMap<CreateMentorProfileDto, MentorProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Relationships, opt => opt.Ignore())
                .ForMember(dest => dest.AdviceRecords, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()));
            
            CreateMap<AddRelationshipDto, NetworkRelationship>()
                .ConstructUsing(src => new NetworkRelationship(
                    src.RelatedEntityId,
                    src.RelatedEntityName,
                    src.RelationshipType,
                    src.Description,
                    DateTime.UtcNow
                ));
        }
    }
}
