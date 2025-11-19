using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Collaboration.Comments.ValueObjects
{
    public class MentionList : ValueObject
    {
        public IReadOnlyList<Guid> UserIds { get; private set; }
        public int Count => UserIds?.Count ?? 0;

        public const int MaxMentions = 10;

        private MentionList()
        {
            UserIds = new List<Guid>();
        }

        public MentionList(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                UserIds = new List<Guid>();
                return;
            }

            if (userIds.Count > MaxMentions)
            {
                throw new BusinessException("Comments:TooManyMentions",
                    $"Cannot mention more than {MaxMentions} users in a comment.")
                    .WithData("maxMentions", MaxMentions)
                    .WithData("actualMentions", userIds.Count);
            }

            // Remove duplicates
            UserIds = userIds.Distinct().ToList();
        }

        public static MentionList Empty() => new MentionList();

        public bool Contains(Guid userId) => UserIds.Contains(userId);

        public bool HasMentions => Count > 0;

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var userId in UserIds)
            {
                yield return userId;
            }
        }
    }
}
