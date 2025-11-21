using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents an evaluation of an idea
    /// </summary>
    public class IdeaEvaluation : Entity
    {
        public Guid IdeaId { get; private set; }
        public Guid EvaluatorId { get; private set; }
        public string EvaluatorName { get; private set; }
        public DateTime EvaluationDate { get; private set; }
        public string Comments { get; private set; }
        public bool IsApproved { get; private set; }

        private readonly List<EvaluationScore> _scores;
        public IReadOnlyCollection<EvaluationScore> Scores => _scores.AsReadOnly();

        protected IdeaEvaluation()
        {
            _scores = new List<EvaluationScore>();
        }

        internal IdeaEvaluation(
            Guid ideaId, 
            Guid evaluatorId, 
            string evaluatorName, 
            string comments, 
            bool isApproved, 
            DateTime evaluationDate)
        {
            IdeaId = ideaId;
            EvaluatorId = evaluatorId;
            EvaluatorName = Check.NotNullOrWhiteSpace(evaluatorName, nameof(evaluatorName));
            Comments = comments;
            IsApproved = isApproved;
            EvaluationDate = evaluationDate;
            _scores = new List<EvaluationScore>();
        }

        internal void AddScore(EvaluationScore score)
        {
            if (_scores.Any(s => s.CriteriaName == score.CriteriaName))
            {
                throw new BusinessRuleViolationException($"Score for criteria '{score.CriteriaName}' already exists");
            }

            _scores.Add(score);
        }

        internal void UpdateApprovalStatus(bool isApproved, string comments)
        {
            IsApproved = isApproved;
            Comments = comments;
        }

        public override object[] GetKeys()
        {
            return new object[] { IdeaId, EvaluatorId };
        }
    }
}
