using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing feedback for a pitch presentation
    /// </summary>
    public class FeedbackRecord : Entity<Guid>, IHasCreationTime
    {
        public Guid PitchDeckId { get; private set; }
        public Guid PresentationId { get; private set; }
        public FeedbackData Feedback { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreationTime { get; set; }
        
        protected FeedbackRecord() { }
        
        internal FeedbackRecord(
            Guid id,
            Guid pitchDeckId,
            Guid presentationId,
            FeedbackData feedback,
            string notes = null) : base(id)
        {
            PitchDeckId = pitchDeckId;
            PresentationId = presentationId;
            Feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
            Notes = notes;
            CreationTime = DateTime.Now;
        }
        
        public void Update(FeedbackData feedback, string notes = null)
        {
            Feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
            Notes = notes;
        }
    }
}
