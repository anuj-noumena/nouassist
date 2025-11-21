using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing structured feedback data from user sessions
    /// </summary>
    public class FeedbackData : Entity<Guid>
    {
        public Guid FeedbackSessionId { get; private set; }
        public string Category { get; private set; }
        public string Question { get; private set; }
        public string Response { get; private set; }
        public FeedbackRating Rating { get; private set; }
        public List<FeedbackTag> Tags { get; private set; }
        public DateTime RecordedAt { get; private set; }

        protected FeedbackData() 
        { 
            Tags = new List<FeedbackTag>();
        }

        internal FeedbackData(
            Guid id,
            Guid feedbackSessionId,
            string category,
            string question,
            string response,
            FeedbackRating rating,
            DateTime recordedAt) : base(id)
        {
            FeedbackSessionId = feedbackSessionId;
            SetCategory(category);
            SetQuestion(question);
            SetResponse(response);
            Rating = rating;
            RecordedAt = recordedAt;
            Tags = new List<FeedbackTag>();
        }

        internal void UpdateResponse(string response)
        {
            SetResponse(response);
        }

        internal void UpdateRating(FeedbackRating rating)
        {
            Rating = rating ?? throw new ArgumentNullException(nameof(rating));
        }

        internal void AddTag(FeedbackTag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (!Tags.Any(t => t.Name == tag.Name))
            {
                Tags.Add(tag);
            }
        }

        internal void RemoveTag(string tagName)
        {
            Tags.RemoveAll(t => t.Name == tagName);
        }

        private void SetCategory(string category)
        {
            Category = Check.NotNullOrWhiteSpace(
                category,
                nameof(category),
                maxLength: 100
            );
        }

        private void SetQuestion(string question)
        {
            Question = Check.NotNullOrWhiteSpace(
                question,
                nameof(question),
                maxLength: 500
            );
        }

        private void SetResponse(string response)
        {
            Response = Check.NotNullOrWhiteSpace(
                response,
                nameof(response),
                maxLength: 2000
            );
        }
    }
}
