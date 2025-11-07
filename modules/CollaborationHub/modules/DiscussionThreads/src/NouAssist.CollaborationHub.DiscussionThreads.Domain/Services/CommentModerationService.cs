using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NouAssist.CollaborationHub.DiscussionThreads.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.CollaborationHub.DiscussionThreads.Services
{
    /// <summary>
    /// Domain service for comment moderation and content filtering
    /// </summary>
    public class CommentModerationService : DomainService
    {
        private static readonly List<string> ProhibitedWords = new()
        {
            // Add prohibited words here
        };

        private static readonly Regex UrlPattern = new Regex(
            @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ModerationResult ModerateComment(Comment comment, ModerationRules rules)
        {
            var result = new ModerationResult
            {
                CommentId = comment.Id,
                IsApproved = true
            };

            // Check for prohibited content
            if (ContainsProhibitedWords(comment.Content))
            {
                result.IsApproved = false;
                result.Reasons.Add("Contains prohibited words");
                result.Severity = ModerationSeverity.High;
            }

            // Check for excessive URLs
            var urlCount = CountUrls(comment.Content);
            if (urlCount > rules.MaxUrlsPerComment)
            {
                result.IsApproved = false;
                result.Reasons.Add($"Contains too many URLs ({urlCount} > {rules.MaxUrlsPerComment})");
                result.Severity = ModerationSeverity.Medium;
            }

            // Check for excessive mentions
            if (comment.MentionedUserIds.Count > rules.MaxMentionsPerComment)
            {
                result.IsApproved = false;
                result.Reasons.Add($"Too many user mentions ({comment.MentionedUserIds.Count} > {rules.MaxMentionsPerComment})");
                result.Severity = ModerationSeverity.Low;
            }

            // Check for spam patterns
            if (IsLikelySpam(comment.Content))
            {
                result.IsApproved = false;
                result.Reasons.Add("Detected spam patterns");
                result.Severity = ModerationSeverity.High;
            }

            return result;
        }

        public List<Guid> ExtractMentionedUsers(string content)
        {
            var mentionPattern = new Regex(@"@(\w+)", RegexOptions.Compiled);
            var matches = mentionPattern.Matches(content);
            
            // In a real implementation, you would resolve usernames to user IDs
            // For now, return empty list
            return new List<Guid>();
        }

        public string SanitizeContent(string content)
        {
            // Remove HTML tags
            content = Regex.Replace(content, @"<[^>]+>", string.Empty);

            // Remove excessive whitespace
            content = Regex.Replace(content, @"\s+", " ");

            return content.Trim();
        }

        private bool ContainsProhibitedWords(string content)
        {
            var lowerContent = content.ToLower();
            return ProhibitedWords.Any(word => lowerContent.Contains(word.ToLower()));
        }

        private int CountUrls(string content)
        {
            return UrlPattern.Matches(content).Count;
        }

        private bool IsLikelySpam(string content)
        {
            // Check for repeated characters
            if (Regex.IsMatch(content, @"(.)\1{5,}"))
                return true;

            // Check for excessive capitalization
            var upperCount = content.Count(char.IsUpper);
            if (upperCount > content.Length * 0.7 && content.Length > 20)
                return true;

            return false;
        }
    }

    public class ModerationResult
    {
        public Guid CommentId { get; set; }
        public bool IsApproved { get; set; }
        public List<string> Reasons { get; set; } = new();
        public ModerationSeverity Severity { get; set; }
    }

    public class ModerationRules
    {
        public int MaxUrlsPerComment { get; set; } = 3;
        public int MaxMentionsPerComment { get; set; } = 10;
        public bool RequireApproval { get; set; }
        public List<string> ProhibitedWords { get; set; } = new();
    }

    public enum ModerationSeverity
    {
        Low,
        Medium,
        High
    }
}
