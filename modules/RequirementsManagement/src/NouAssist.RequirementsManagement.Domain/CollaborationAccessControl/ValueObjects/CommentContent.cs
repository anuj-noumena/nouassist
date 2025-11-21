using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects
{
    /// <summary>
    /// Represents the content of a comment in a discussion thread.
    /// </summary>
    public class CommentContent : ValueObject
    {
        public string Value { get; private set; }
        public List<Guid> MentionedUserIds { get; private set; }

        private CommentContent()
        {
            MentionedUserIds = new List<Guid>();
        }

        public CommentContent(string value)
        {
            SetValue(value);
            MentionedUserIds = ExtractMentionedUsers(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: CollaborationConsts.MaxCommentContentLength
            );
        }

        /// <summary>
        /// Extracts user IDs that were mentioned in the comment (using @user syntax).
        /// </summary>
        private List<Guid> ExtractMentionedUsers(string content)
        {
            var mentionedUsers = new List<Guid>();
            
            // This is a simplified example. In a real implementation, you would use
            // a more sophisticated approach to extract user mentions.
            var regex = new System.Text.RegularExpressions.Regex(@"@([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})");
            var matches = regex.Matches(content);
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (Guid.TryParse(match.Groups[1].Value, out Guid userId))
                {
                    mentionedUsers.Add(userId);
                }
            }
            
            return mentionedUsers;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            foreach (var userId in MentionedUserIds.OrderBy(id => id))
            {
                yield return userId;
            }
        }
    }
}
