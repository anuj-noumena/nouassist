using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Notifications.ValueObjects
{
    public class NotificationMessage : ValueObject
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public const int MaxTitleLength = 200;
        public const int MaxBodyLength = 1000;

        private NotificationMessage() { }

        public NotificationMessage(string title, string? body = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessException("Notification:TitleRequired",
                    "Notification title is required.");
            }

            if (title.Length > MaxTitleLength)
            {
                throw new BusinessException("Notification:TitleTooLong",
                    $"Notification title cannot exceed {MaxTitleLength} characters.")
                    .WithData("maxLength", MaxTitleLength);
            }

            if (body != null && body.Length > MaxBodyLength)
            {
                throw new BusinessException("Notification:BodyTooLong",
                    $"Notification body cannot exceed {MaxBodyLength} characters.")
                    .WithData("maxLength", MaxBodyLength);
            }

            Title = title.Trim();
            Body = body?.Trim();
        }

        public bool HasBody => !string.IsNullOrWhiteSpace(Body);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Body ?? string.Empty;
        }
    }
}
