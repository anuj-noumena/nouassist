using System;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing an interview transcript or notes
    /// </summary>
    public class InterviewTranscript : Entity<Guid>
    {
        public Guid InterviewId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public bool IsProcessed { get; private set; }

        protected InterviewTranscript() { }

        internal InterviewTranscript(
            Guid id,
            Guid interviewId,
            string content,
            DateTime createdAt,
            string createdBy) : base(id)
        {
            InterviewId = interviewId;
            SetContent(content);
            CreatedAt = createdAt;
            CreatedBy = Check.NotNullOrWhiteSpace(createdBy, nameof(createdBy));
            IsProcessed = false;
        }

        internal void UpdateContent(string content)
        {
            SetContent(content);
        }

        internal void MarkAsProcessed()
        {
            IsProcessed = true;
        }

        private void SetContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: ProductDiscoveryConsts.MaxTranscriptContentLength
            );
        }
    }
}
