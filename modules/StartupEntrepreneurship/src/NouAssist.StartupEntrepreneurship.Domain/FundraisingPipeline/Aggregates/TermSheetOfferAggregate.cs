namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates
{
    /// <summary>
    /// Aggregate root representing a term sheet offer from an investor
    /// </summary>
    public class TermSheetOffer : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public Guid ProspectId { get; private set; }
        public Guid? FundraisingRoundId { get; private set; }
        public DateTime ReceivedDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }
        public TermSheetStatus Status { get; private set; }
        public TermComparisonData TermData { get; private set; }
        
        private readonly List<TermSheetClause> _clauses;
        public IReadOnlyCollection<TermSheetClause> Clauses => _clauses.AsReadOnly();
        
        private readonly List<NegotiationNote> _negotiationNotes;
        public IReadOnlyCollection<NegotiationNote> NegotiationNotes => _negotiationNotes.AsReadOnly();
        
        protected TermSheetOffer()
        {
            _clauses = new List<TermSheetClause>();
            _negotiationNotes = new List<NegotiationNote>();
        }
        
        /// <summary>
        /// Factory method to create a new term sheet offer
        /// </summary>
        public static TermSheetOffer Create(
            Guid id,
            string title,
            Guid prospectId,
            DateTime receivedDate,
            TermComparisonData termData,
            DateTime? expirationDate = null,
            Guid? fundraisingRoundId = null,
            Guid? tenantId = null)
        {
            return new TermSheetOffer(
                id,
                title,
                prospectId,
                receivedDate,
                termData,
                expirationDate,
                fundraisingRoundId,
                tenantId);
        }
        
        private TermSheetOffer(
            Guid id,
            string title,
            Guid prospectId,
            DateTime receivedDate,
            TermComparisonData termData,
            DateTime? expirationDate = null,
            Guid? fundraisingRoundId = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            ProspectId = prospectId;
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
            FundraisingRoundId = fundraisingRoundId;
            TermData = termData ?? throw new ArgumentNullException(nameof(termData));
            TenantId = tenantId;
            
            Status = TermSheetStatus.Received;
            _clauses = new List<TermSheetClause>();
            _negotiationNotes = new List<NegotiationNote>();
            
            AddDistributedEvent(new TermSheetReceivedEto
            {
                Id = Id,
                TenantId = TenantId,
                ProspectId = ProspectId,
                // Note: In a real implementation, we would query the prospect name and company
                // from a repository or domain service
                ProspectName = "", 
                ProspectCompany = "",
                OfferedAmount = TermData.InvestmentAmount,
                Valuation = TermData.PreMoneyValuation
            });
        }
        
        /// <summary>
        /// Updates the basic information of the term sheet
        /// </summary>
        public void UpdateBasicInfo(
            string title,
            DateTime receivedDate,
            TermComparisonData termData,
            DateTime? expirationDate = null,
            Guid? fundraisingRoundId = null)
        {
            SetTitle(title);
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
            FundraisingRoundId = fundraisingRoundId;
            TermData = termData ?? throw new ArgumentNullException(nameof(termData));
        }
        
        /// <summary>
        /// Updates the status of the term sheet
        /// </summary>
        public void UpdateStatus(TermSheetStatus newStatus)
        {
            ValidateStatusTransition(newStatus);
            Status = newStatus;
        }
        
        /// <summary>
        /// Adds a clause to the term sheet
        /// </summary>
        public TermSheetClause AddClause(
            string name,
            ClauseText text,
            string category,
            int order,
            string notes = null)
        {
            if (_clauses.Any(c => c.Name == name))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:DuplicateClauseName")
                    .WithData("name", name);
            }
            
            var clause = new TermSheetClause(
                GuidGenerator.Create(),
                Id,
                name,
                text,
                category,
                order,
                notes);
            
            _clauses.Add(clause);
            return clause;
        }
        
        /// <summary>
        /// Updates an existing clause
        /// </summary>
        public void UpdateClause(
            Guid clauseId,
            string name,
            ClauseText text,
            string category,
            int order,
            string notes = null)
        {
            var clause = _clauses.FirstOrDefault(c => c.Id == clauseId);
            if (clause == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:ClauseNotFound")
                    .WithData("id", clauseId);
            }
            
            // Check for duplicate name
            if (_clauses.Any(c => c.Name == name && c.Id != clauseId))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:DuplicateClauseName")
                    .WithData("name", name);
            }
            
            clause.Update(name, text, category, order, notes);
        }
        
        /// <summary>
        /// Removes a clause from the term sheet
        /// </summary>
        public void RemoveClause(Guid clauseId)
        {
            var clause = _clauses.FirstOrDefault(c => c.Id == clauseId);
            if (clause == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:ClauseNotFound")
                    .WithData("id", clauseId);
            }
            
            _clauses.Remove(clause);
        }
        
        /// <summary>
        /// Adds a negotiation note
        /// </summary>
        public NegotiationNote AddNegotiationNote(
            string title,
            string content,
            Guid? relatedClauseId = null)
        {
            // Verify the related clause exists if specified
            if (relatedClauseId.HasValue && !_clauses.Any(c => c.Id == relatedClauseId.Value))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:ClauseNotFound")
                    .WithData("id", relatedClauseId.Value);
            }
            
            var note = new NegotiationNote(
                GuidGenerator.Create(),
                Id,
                title,
                content,
                relatedClauseId);
            
            _negotiationNotes.Add(note);
            return note;
        }
        
        /// <summary>
        /// Updates an existing negotiation note
        /// </summary>
        public void UpdateNegotiationNote(
            Guid noteId,
            string title,
            string content,
            Guid? relatedClauseId = null)
        {
            var note = _negotiationNotes.FirstOrDefault(n => n.Id == noteId);
            if (note == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:NegotiationNoteNotFound")
                    .WithData("id", noteId);
            }
            
            // Verify the related clause exists if specified
            if (relatedClauseId.HasValue && !_clauses.Any(c => c.Id == relatedClauseId.Value))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:ClauseNotFound")
                    .WithData("id", relatedClauseId.Value);
            }
            
            note.Update(title, content, relatedClauseId);
        }
        
        /// <summary>
        /// Removes a negotiation note
        /// </summary>
        public void RemoveNegotiationNote(Guid noteId)
        {
            var note = _negotiationNotes.FirstOrDefault(n => n.Id == noteId);
            if (note == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:NegotiationNoteNotFound")
                    .WithData("id", noteId);
            }
            
            _negotiationNotes.Remove(note);
        }
        
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: FundraisingPipelineConsts.TermSheet.MaxTitleLength);
        }
        
        private void ValidateStatusTransition(TermSheetStatus newStatus)
        {
            // Implement status transition rules
            // This is simplified; in a real implementation, you might have more complex rules
            if (Status == TermSheetStatus.Accepted || 
                Status == TermSheetStatus.Rejected || 
                Status == TermSheetStatus.Expired)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:InvalidTermSheetStatusTransition")
                    .WithData("currentStatus", Status)
                    .WithData("newStatus", newStatus);
            }
        }
    }
}
