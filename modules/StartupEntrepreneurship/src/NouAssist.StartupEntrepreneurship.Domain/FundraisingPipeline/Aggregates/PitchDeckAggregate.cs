using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates
{
    /// <summary>
    /// Aggregate root representing a pitch deck with versions and presentations
    /// </summary>
    public class PitchDeck : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid? FundraisingRoundId { get; private set; }
        
        private readonly List<PitchVersion> _versions;
        public IReadOnlyCollection<PitchVersion> Versions => _versions.AsReadOnly();
        
        private readonly List<PitchPresentation> _presentations;
        public IReadOnlyCollection<PitchPresentation> Presentations => _presentations.AsReadOnly();
        
        private readonly List<FeedbackRecord> _feedbacks;
        public IReadOnlyCollection<FeedbackRecord> Feedbacks => _feedbacks.AsReadOnly();
        
        protected PitchDeck()
        {
            _versions = new List<PitchVersion>();
            _presentations = new List<PitchPresentation>();
            _feedbacks = new List<FeedbackRecord>();
        }
        
        /// <summary>
        /// Factory method to create a new pitch deck
        /// </summary>
        public static PitchDeck Create(
            Guid id,
            string title,
            string description = null,
            Guid? fundraisingRoundId = null,
            Guid? tenantId = null)
        {
            return new PitchDeck(id, title, description, fundraisingRoundId, tenantId);
        }
        
        private PitchDeck(
            Guid id,
            string title,
            string description = null,
            Guid? fundraisingRoundId = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            Description = description;
            FundraisingRoundId = fundraisingRoundId;
            TenantId = tenantId;
            
            _versions = new List<PitchVersion>();
            _presentations = new List<PitchPresentation>();
            _feedbacks = new List<FeedbackRecord>();
        }
        
        /// <summary>
        /// Updates the basic information of the pitch deck
        /// </summary>
        public void UpdateBasicInfo(string title, string description = null, Guid? fundraisingRoundId = null)
        {
            SetTitle(title);
            Description = description;
            FundraisingRoundId = fundraisingRoundId;
        }
        
        /// <summary>
        /// Adds a new version to the pitch deck
        /// </summary>
        public PitchVersion AddVersion(
            string versionName,
            DeckContent content,
            string changeNotes = null)
        {
            var versionNumber = _versions.Count + 1;
            
            var version = new PitchVersion(
                new Guid(),
                Id,
                versionName,
                versionNumber,
                content,
                changeNotes);
            
            _versions.Add(version);
            return version;
        }
        
        /// <summary>
        /// Updates an existing version of the pitch deck
        /// </summary>
        public void UpdateVersion(
            Guid versionId,
            string versionName,
            DeckContent content,
            string changeNotes = null)
        {
            var version = _versions.FirstOrDefault(v => v.Id == versionId);
            if (version == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PitchVersionNotFound")
                    .WithData("id", versionId);
            }
            
            version.Update(versionName, content, changeNotes);
        }
        
        /// <summary>
        /// Adds a new presentation record
        /// </summary>
        public PitchPresentation AddPresentation(
            Guid versionId,
            Guid prospectId,
            PresentationDetails details)
        {
            // Verify the version exists
            var version = _versions.FirstOrDefault(v => v.Id == versionId);
            if (version == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PitchVersionNotFound")
                    .WithData("id", versionId);
            }
            
            var presentation = new PitchPresentation(
                new Guid(),
                Id,
                versionId,
                prospectId,
                details);
            
            _presentations.Add(presentation);
            return presentation;
        }
        
        /// <summary>
        /// Updates an existing presentation record
        /// </summary>
        public void UpdatePresentation(
            Guid presentationId,
            Guid versionId,
            Guid prospectId,
            PresentationDetails details)
        {
            var presentation = _presentations.FirstOrDefault(p => p.Id == presentationId);
            if (presentation == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PresentationNotFound")
                    .WithData("id", presentationId);
            }
            
            // Verify the version exists
            var version = _versions.FirstOrDefault(v => v.Id == versionId);
            if (version == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PitchVersionNotFound")
                    .WithData("id", versionId);
            }
            
            presentation.Update(versionId, prospectId, details);
        }
        
        /// <summary>
        /// Adds feedback for a presentation
        /// </summary>
        public FeedbackRecord AddFeedback(
            Guid presentationId,
            FeedbackData feedback,
            string notes = null)
        {
            // Verify the presentation exists
            var presentation = _presentations.FirstOrDefault(p => p.Id == presentationId);
            if (presentation == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PresentationNotFound")
                    .WithData("id", presentationId);
            }
            
            var feedbackRecord = new FeedbackRecord(
                new Guid(),
                Id,
                presentationId,
                feedback,
                notes);
            
            _feedbacks.Add(feedbackRecord);
            return feedbackRecord;
        }
        
        /// <summary>
        /// Updates existing feedback
        /// </summary>
        public void UpdateFeedback(
            Guid feedbackId,
            FeedbackData feedback,
            string notes = null)
        {
            var feedbackRecord = _feedbacks.FirstOrDefault(f => f.Id == feedbackId);
            if (feedbackRecord == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:FeedbackNotFound")
                    .WithData("id", feedbackId);
            }
            
            feedbackRecord.Update(feedback, notes);
        }
        
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: FundraisingPipelineConsts.PitchDeck.MaxTitleLength);
        }
    }
}
