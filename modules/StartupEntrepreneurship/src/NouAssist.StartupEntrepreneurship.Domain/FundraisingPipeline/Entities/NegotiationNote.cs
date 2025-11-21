using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing a negotiation note for a term sheet
    /// </summary>
    public class NegotiationNote : Entity<Guid>, IHasCreationTime
    {
        public Guid TermSheetId { get; private set; }
        public Guid? RelatedClauseId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime CreationTime { get; set; }
        
        protected NegotiationNote() { }
        
        internal NegotiationNote(
            Guid id,
            Guid termSheetId,
            string title,
            string content,
            Guid? relatedClauseId = null) : base(id)
        {
            TermSheetId = termSheetId;
            SetTitle(title);
            SetContent(content);
            RelatedClauseId = relatedClauseId;
            CreationTime = DateTime.Now;
        }
        
        public void Update(
            string title,
            string content,
            Guid? relatedClauseId = null)
        {
            SetTitle(title);
            SetContent(content);
            RelatedClauseId = relatedClauseId;
        }
        
        private void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.");
            }
            
            Title = title;
        }
        
        private void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty.");
            }
            
            Content = content;
        }
    }
}
