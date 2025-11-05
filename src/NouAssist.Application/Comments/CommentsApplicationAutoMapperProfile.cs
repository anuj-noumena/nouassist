using AutoMapper;
using NouAssist.Comments.Aggregates;
using NouAssist.Comments.Dtos;
using NouAssist.Comments.Entities;

namespace NouAssist.Comments
{
    public class CommentsApplicationAutoMapperProfile : Profile
    {
        public CommentsApplicationAutoMapperProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.IsReply, opt => opt.MapFrom(src => src.IsReply))
                .ForMember(dest => dest.HasReplies, opt => opt.MapFrom(src => src.HasReplies))
                .ForMember(dest => dest.HasReactions, opt => opt.MapFrom(src => src.HasReactions))
                .ForMember(dest => dest.HasMentions, opt => opt.MapFrom(src => src.HasMentions))
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore());

            CreateMap<CommentReaction, CommentReactionDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
