using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Collaboration.Comments.ValueObjects
{
    public class CommentContent : ValueObject
    {
        public string Value { get; private set; }
        public int Length => Value?.Length ?? 0;

        public const int MaxLength = 5000;
        public const int MinLength = 1;

        private CommentContent() { }

        public CommentContent(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessException("Comments:ContentRequired",
                    "Comment content cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new BusinessException("Comments:ContentTooLong",
                    $"Comment content cannot exceed {MaxLength} characters.")
                    .WithData("maxLength", MaxLength)
                    .WithData("actualLength", value.Length);
            }

            Value = value.Trim();
        }

        public bool ContainsMention => Value.Contains("@");
        public bool ContainsLink => Value.Contains("http://") || Value.Contains("https://");

        public CommentContent Update(string newContent)
        {
            return new CommentContent(newContent);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
