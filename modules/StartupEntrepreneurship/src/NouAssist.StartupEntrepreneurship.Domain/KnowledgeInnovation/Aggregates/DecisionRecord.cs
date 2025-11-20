namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates
{
    /// <summary>
    /// Aggregate root for decision records in the decision journal
    /// </summary>
    [Table("AppDecisionRecords")]
    public class DecisionRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Title { get; private set; }
        public DecisionStatus Status { get; private set; }
        public DecisionRationale Rationale { get; private set; }
        public DateTime DecisionDate { get; private set; }
        public List<string> Tags { get; private set; }
        public Guid? TenantId { get; private set; }

        private DecisionContext _context;
        public DecisionContext Context => _context;

        private readonly List<DecisionAlternative> _alternatives;
        public IReadOnlyCollection<DecisionAlternative> Alternatives => _alternatives.AsReadOnly();

        private DecisionOutcome _outcome;
        public DecisionOutcome Outcome => _outcome;

        protected DecisionRecord()
        {
            _alternatives = new List<DecisionAlternative>();
            Tags = new List<string>();
        }

        public static DecisionRecord Create(
            Guid id,
            string title,
            DecisionContext context,
            List<string> tags = null,
            Guid? tenantId = null)
        {
            return new DecisionRecord(id, title, context, tags, tenantId);
        }

        private DecisionRecord(
            Guid id,
            string title,
            DecisionContext context,
            List<string> tags = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Status = DecisionStatus.Pending;
            Tags = tags ?? new List<string>();
            TenantId = tenantId;
            _alternatives = new List<DecisionAlternative>();
            DecisionDate = DateTime.UtcNow;
        }

        public void UpdateDetails(string title, List<string> tags)
        {
            SetTitle(title);
            Tags = tags ?? new List<string>();
        }

        public void UpdateContext(
            string background,
            string problemStatement,
            List<string> stakeholders,
            List<string> constraints)
        {
            _context.Update(background, problemStatement, stakeholders, constraints);
        }

        public DecisionAlternative AddAlternative(string title, string description, string prosCons)
        {
            if (Status != DecisionStatus.Pending)
            {
                throw new BusinessRuleViolationException(
                    "Cannot add alternatives after a decision has been made");
            }

            var alternative = new DecisionAlternative(Id, title, description, prosCons);
            _alternatives.Add(alternative);
            return alternative;
        }

        public void UpdateAlternative(string title, string description, string prosCons)
        {
            if (Status != DecisionStatus.Pending)
            {
                throw new BusinessRuleViolationException(
                    "Cannot update alternatives after a decision has been made");
            }

            var alternative = _alternatives.FirstOrDefault(a => a.Title == title);
            if (alternative == null)
            {
                throw new BusinessRuleViolationException($"Alternative with title '{title}' not found");
            }

            alternative.Update(title, description, prosCons);
        }

        public void MakeDecision(string chosenAlternativeTitle, DecisionRationale rationale, DateTime decisionDate)
        {
            if (Status != DecisionStatus.Pending)
            {
                throw new InvalidStatusTransitionException("DecisionRecord", Status, DecisionStatus.Made);
            }

            if (_alternatives.Count == 0)
            {
                throw new BusinessRuleViolationException("Cannot make a decision without alternatives");
            }

            var chosenAlternative = _alternatives.FirstOrDefault(a => a.Title == chosenAlternativeTitle);
            if (chosenAlternative == null)
            {
                throw new BusinessRuleViolationException($"Alternative with title '{chosenAlternativeTitle}' not found");
            }

            // Mark the chosen alternative
            foreach (var alternative in _alternatives)
            {
                alternative.MarkAsNotChosen();
            }
            chosenAlternative.MarkAsChosen();

            Rationale = rationale;
            DecisionDate = decisionDate;
            Status = DecisionStatus.Made;

            AddDistributedEvent(new DecisionRecordedEto
            {
                Id = Id,
                Title = Title,
                Status = Status,
                TenantId = TenantId,
                RecordedAt = decisionDate,
                RecordedBy = LastModifierId ?? CreatorId
            });
        }

        public void ImplementDecision(string description, DateTime implementationDate)
        {
            if (Status != DecisionStatus.Made)
            {
                throw new InvalidStatusTransitionException("DecisionRecord", Status, DecisionStatus.Implemented);
            }

            if (_outcome == null)
            {
                _outcome = new DecisionOutcome(Id, description, implementationDate);
            }
            else
            {
                _outcome.Update(description, implementationDate, _outcome.LessonsLearned);
            }

            Status = DecisionStatus.Implemented;
        }

        public void EvaluateOutcome(string description, string lessonsLearned, List<OutcomeImpact> impacts)
        {
            if (Status != DecisionStatus.Implemented)
            {
                throw new InvalidStatusTransitionException("DecisionRecord", Status, DecisionStatus.Evaluated);
            }

            if (_outcome == null)
            {
                throw new BusinessRuleViolationException("Cannot evaluate outcome without implementation details");
            }

            _outcome.Update(description, _outcome.ImplementationDate, lessonsLearned);
            
            _outcome.ClearImpacts();
            if (impacts != null)
            {
                foreach (var impact in impacts)
                {
                    _outcome.AddImpact(impact);
                }
            }

            Status = DecisionStatus.Evaluated;
        }

        public void ReverseDecision(string reason)
        {
            if (Status != DecisionStatus.Made && Status != DecisionStatus.Implemented && Status != DecisionStatus.Evaluated)
            {
                throw new InvalidStatusTransitionException("DecisionRecord", Status, DecisionStatus.Reversed);
            }

            // Add the reversal reason to the rationale
            var updatedReasoning = $"{Rationale.Reasoning}\n\nDECISION REVERSED: {reason}";
            Rationale = new DecisionRationale(
                updatedReasoning,
                Rationale.KeyFactors,
                Rationale.RisksIdentified,
                Rationale.AssumptionsMade);

            Status = DecisionStatus.Reversed;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                KnowledgeInnovationConsts.DecisionRecord.TitleMinLength,
                KnowledgeInnovationConsts.DecisionRecord.TitleMaxLength);
        }
    }
}
