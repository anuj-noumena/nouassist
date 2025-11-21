using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Events;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates
{
    /// <summary>
    /// Represents a prioritization activity where stakeholders vote on requirements
    /// using various prioritization techniques.
    /// </summary>
    public class PrioritizationActivity : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public PrioritizationMethod Method { get; private set; }
        public ActivityStatus Status { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool AllowModification { get; private set; }
        public string? Instructions { get; private set; }

        // Collections
        private readonly List<Guid> _participantIds;
        private readonly List<Guid> _requirementIds;
        private readonly List<StakeholderVote> _votes;

        public IReadOnlyCollection<Guid> ParticipantIds => _participantIds.AsReadOnly();
        public IReadOnlyCollection<Guid> RequirementIds => _requirementIds.AsReadOnly();
        public IReadOnlyCollection<StakeholderVote> Votes => _votes.AsReadOnly();

        protected PrioritizationActivity()
        {
            _participantIds = new List<Guid>();
            _requirementIds = new List<Guid>();
            _votes = new List<StakeholderVote>();
        }

        public static PrioritizationActivity Create(
            Guid id,
            string name,
            string description,
            PrioritizationMethod method,
            Guid? tenantId,
            string? instructions = null)
        {
            var activity = new PrioritizationActivity
            {
                Id = id,
                TenantId = tenantId,
                Method = method,
                Status = ActivityStatus.Draft,
                AllowModification = true,
                Instructions = instructions
            };

            activity.SetName(name);
            activity.SetDescription(description);

            return activity;
        }

        public void AddParticipants(IEnumerable<Guid> participantIds)
        {
            if (Status != ActivityStatus.Draft)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotModifyActiveActivity");
            }

            foreach (var participantId in participantIds)
            {
                if (!_participantIds.Contains(participantId))
                {
                    _participantIds.Add(participantId);
                }
            }
        }

        public void RemoveParticipant(Guid participantId)
        {
            if (Status != ActivityStatus.Draft)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotModifyActiveActivity");
            }

            _participantIds.Remove(participantId);
            
            // Remove votes from this participant
            var participantVotes = _votes.Where(v => v.StakeholderId == participantId).ToList();
            foreach (var vote in participantVotes)
            {
                _votes.Remove(vote);
            }
        }

        public void AddRequirements(IEnumerable<Guid> requirementIds)
        {
            if (Status != ActivityStatus.Draft)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotModifyActiveActivity");
            }

            foreach (var requirementId in requirementIds)
            {
                if (!_requirementIds.Contains(requirementId))
                {
                    _requirementIds.Add(requirementId);
                }
            }
        }

        public void RemoveRequirement(Guid requirementId)
        {
            if (Status != ActivityStatus.Draft)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotModifyActiveActivity");
            }

            _requirementIds.Remove(requirementId);
            
            // Remove votes for this requirement
            var requirementVotes = _votes.Where(v => v.RequirementId == requirementId).ToList();
            foreach (var vote in requirementVotes)
            {
                _votes.Remove(vote);
            }
        }

        public void Start(DateTime startDate)
        {
            if (Status != ActivityStatus.Draft)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:ActivityAlreadyStarted");
            }

            if (!_participantIds.Any())
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:NoParticipants");
            }

            if (!_requirementIds.Any())
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:NoRequirements");
            }

            Status = ActivityStatus.In_Progress;
            StartDate = startDate;
            AllowModification = false;
        }

        public void StartVoting()
        {
            if (Status != ActivityStatus.In_Progress)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidStatusForVoting");
            }

            Status = ActivityStatus.Voting;
        }

        public void SubmitVote(
            Guid stakeholderId,
            Guid requirementId,
            VoteValue vote,
            string? comment,
            DateTime votedAt)
        {
            if (Status != ActivityStatus.Voting)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:ActivityNotInVotingStatus");
            }

            if (!_participantIds.Contains(stakeholderId))
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:NotAuthorizedToVote");
            }

            if (!_requirementIds.Contains(requirementId))
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:RequirementNotInActivity");
            }

            // Check for existing vote
            var existingVote = _votes.FirstOrDefault(v => 
                v.StakeholderId == stakeholderId && v.RequirementId == requirementId);

            if (existingVote != null)
            {
                if (!AllowModification)
                {
                    throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:DuplicateVote");
                }
                
                existingVote.UpdateVote(vote, comment);
            }
            else
            {
                var newVote = new StakeholderVote(
                    GuidGenerator.Create(),
                    Id,
                    requirementId,
                    stakeholderId,
                    vote,
                    comment,
                    votedAt
                );
                _votes.Add(newVote);
            }
        }

        public void Complete(DateTime endDate)
        {
            if (Status != ActivityStatus.Voting)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidStatusForCompletion");
            }

            Status = ActivityStatus.Completed;
            EndDate = endDate;

            // Publish completion event
            AddDistributedEvent(new PrioritizationActivityCompletedEto
            {
                Id = Id,
                Name = Name,
                Method = Method,
                ParticipantCount = _participantIds.Count,
                RequirementCount = _requirementIds.Count,
                TenantId = TenantId,
                CompletionTime = endDate
            });
        }

        public void Cancel()
        {
            if (Status == ActivityStatus.Completed)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:CannotCancelCompletedActivity");
            }

            Status = ActivityStatus.Cancelled;
        }

        public Dictionary<Guid, RankScore> CalculateResults()
        {
            if (Status != ActivityStatus.Completed)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:ActivityNotCompleted");
            }

            var results = new Dictionary<Guid, RankScore>();

            switch (Method)
            {
                case PrioritizationMethod.MoSCoW:
                    results = CalculateMoSCoWResults();
                    break;
                case PrioritizationMethod.WSJF:
                case PrioritizationMethod.Numeric_Scoring:
                    results = CalculateNumericResults();
                    break;
                default:
                    throw new InvalidPrioritizationMethodException(Method);
            }

            return results;
        }

        private Dictionary<Guid, RankScore> CalculateMoSCoWResults()
        {
            var requirementScores = new Dictionary<Guid, List<decimal>>();

            // Group votes by requirement
            foreach (var requirementId in _requirementIds)
            {
                requirementScores[requirementId] = _votes
                    .Where(v => v.RequirementId == requirementId)
                    .Select(v => v.Vote.NumericValue)
                    .ToList();
            }

            // Calculate average scores and ranking
            var results = new Dictionary<Guid, RankScore>();
            var sortedRequirements = requirementScores
                .Select(kvp => new
                {
                    RequirementId = kvp.Key,
                    AverageScore = kvp.Value.Any() ? kvp.Value.Average() : 0,
                    VoteCount = kvp.Value.Count,
                    Variance = CalculateVariance(kvp.Value)
                })
                .OrderByDescending(x => x.AverageScore)
                .ToList();

            for (int i = 0; i < sortedRequirements.Count; i++)
            {
                var req = sortedRequirements[i];
                var confidence = CalculateConfidence(req.Variance, req.VoteCount);
                
                results[req.RequirementId] = new RankScore(
                    req.AverageScore,
                    i + 1,
                    confidence,
                    req.VoteCount
                );
            }

            return results;
        }

        private Dictionary<Guid, RankScore> CalculateNumericResults()
        {
            var requirementScores = new Dictionary<Guid, List<decimal>>();

            // Group votes by requirement
            foreach (var requirementId in _requirementIds)
            {
                requirementScores[requirementId] = _votes
                    .Where(v => v.RequirementId == requirementId)
                    .Select(v => v.Vote.NumericValue)
                    .ToList();
            }

            // Calculate weighted average scores
            var results = new Dictionary<Guid, RankScore>();
            var sortedRequirements = requirementScores
                .Select(kvp => new
                {
                    RequirementId = kvp.Key,
                    WeightedScore = CalculateWeightedScore(kvp.Value),
                    VoteCount = kvp.Value.Count,
                    Variance = CalculateVariance(kvp.Value)
                })
                .OrderByDescending(x => x.WeightedScore)
                .ToList();

            for (int i = 0; i < sortedRequirements.Count; i++)
            {
                var req = sortedRequirements[i];
                var confidence = CalculateConfidence(req.Variance, req.VoteCount);
                
                results[req.RequirementId] = new RankScore(
                    req.WeightedScore,
                    i + 1,
                    confidence,
                    req.VoteCount
                );
            }

            return results;
        }

        private static decimal CalculateWeightedScore(List<decimal> scores)
        {
            if (!scores.Any()) return 0;
            
            // Simple average for now, could be enhanced with stakeholder weights
            return scores.Average();
        }

        private static decimal CalculateVariance(List<decimal> scores)
        {
            if (scores.Count <= 1) return 0;
            
            var mean = scores.Average();
            var variance = scores.Select(x => (double)Math.Pow((double)(x - mean), 2)).Average();
            return (decimal)variance;
        }

        private static decimal CalculateConfidence(decimal variance, int voteCount)
        {
            // Simple confidence calculation: higher vote count and lower variance = higher confidence
            if (voteCount == 0) return 0;
            
            var normalizedVoteCount = Math.Min(1.0m, voteCount / 10.0m); // Normalize to 0-1
            var normalizedVariance = Math.Min(1.0m, variance / 4.0m); // Normalize variance
            
            return normalizedVoteCount * (1 - normalizedVariance);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalysisConsts.MaxPrioritizationActivityNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: AnalysisConsts.MaxPrioritizationActivityDescriptionLength
            );
        }

        public bool HasVoted(Guid stakeholderId, Guid requirementId)
        {
            return _votes.Any(v => v.StakeholderId == stakeholderId && v.RequirementId == requirementId);
        }

        public decimal GetVotingProgress()
        {
            if (!_participantIds.Any() || !_requirementIds.Any())
                return 0;

            var totalExpectedVotes = _participantIds.Count * _requirementIds.Count;
            var actualVotes = _votes.Count;
            
            return (decimal)actualVotes / totalExpectedVotes;
        }

        public List<StakeholderVote> GetVotesForRequirement(Guid requirementId)
        {
            return _votes.Where(v => v.RequirementId == requirementId).ToList();
        }

        public List<StakeholderVote> GetVotesFromStakeholder(Guid stakeholderId)
        {
            return _votes.Where(v => v.StakeholderId == stakeholderId).ToList();
        }
    }
}
