using NouAssist.Collaboration.Comments.Aggregates;
using NouAssist.Collaboration.Comments.Comments.Dtos;
using NouAssist.Collaboration.Comments.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.Collaboration.Comments
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CommentToCommentDtoMapper : MapperBase<Comment, CommentDto>
    {
        [MapperIgnoreTarget(nameof(CommentDto.AuthorName))]
        public override partial CommentDto Map(Comment source);

        public override partial void Map(Comment source, CommentDto destination);

        public override void AfterMap(Comment source, CommentDto destination)
        {
            destination.IsReply = source.IsReply;
            destination.HasReplies = source.HasReplies;
            destination.HasReactions = source.HasReactions;
            destination.HasMentions = source.HasMentions;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CommentReactionToCommentReactionDtoMapper : MapperBase<CommentReaction, CommentReactionDto>
    {
        [MapperIgnoreTarget(nameof(CommentReactionDto.UserName))]
        public override partial CommentReactionDto Map(CommentReaction source);

        public override partial void Map(CommentReaction source, CommentReactionDto destination);
    }
}
