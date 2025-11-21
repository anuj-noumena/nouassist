using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services
{
    /// <summary>
    /// Service interface for innovation pipeline operations
    /// </summary>
    public interface IInnovationPipelineService
    {
        /// <summary>
        /// Submit a new idea to the pipeline
        /// </summary>
        Task<Idea> SubmitIdeaAsync(
            Guid id,
            string title,
            string description,
            IdeaSource source,
            List<string> tags,
            string impactStatement,
            int priority,
            Guid creatorId,
            Guid? tenantId = null);

        /// <summary>
        /// Start evaluation process for an idea
        /// </summary>
        Task StartIdeaEvaluationAsync(Guid ideaId);

        /// <summary>
        /// Add an evaluation to an idea
        /// </summary>
        Task<IdeaEvaluation> EvaluateIdeaAsync(
            Guid ideaId,
            Guid evaluatorId,
            string evaluatorName,
            string comments,
            bool isApproved,
            List<EvaluationScore> scores);

        /// <summary>
        /// Approve an idea
        /// </summary>
        Task ApproveIdeaAsync(Guid ideaId);

        /// <summary>
        /// Reject an idea
        /// </summary>
        Task RejectIdeaAsync(Guid ideaId);

        /// <summary>
        /// Start implementation of an idea
        /// </summary>
        Task StartIdeaImplementationAsync(Guid ideaId);

        /// <summary>
        /// Mark an idea as implemented
        /// </summary>
        Task MarkIdeaImplementedAsync(Guid ideaId);

        /// <summary>
        /// Archive an idea
        /// </summary>
        Task ArchiveIdeaAsync(Guid ideaId);

        /// <summary>
        /// Get ideas by status
        /// </summary>
        Task<List<Idea>> GetIdeasByStatusAsync(IdeaStatus status);

        /// <summary>
        /// Get ideas by creator
        /// </summary>
        Task<List<Idea>> GetIdeasByCreatorAsync(Guid creatorId);

        /// <summary>
        /// Get high priority ideas
        /// </summary>
        Task<List<Idea>> GetHighPriorityIdeasAsync();

        /// <summary>
        /// Get ideas ready for evaluation
        /// </summary>
        Task<List<Idea>> GetIdeasForEvaluationAsync();
    }

    /// <summary>
    /// Implementation of innovation pipeline service
    /// </summary>
    public class InnovationPipelineService : DomainService, IInnovationPipelineService
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly ILogger<InnovationPipelineService> _logger;

        public InnovationPipelineService(
            IIdeaRepository ideaRepository,
            ILogger<InnovationPipelineService> logger)
        {
            _ideaRepository = ideaRepository;
            _logger = logger;
        }

        public async Task<Idea> SubmitIdeaAsync(
            Guid id,
            string title,
            string description,
            IdeaSource source,
            List<string> tags,
            string impactStatement,
            int priority,
            Guid creatorId,
            Guid? tenantId = null)
        {
            try
            {
                var idea = Idea.Create(
                    id,
                    title,
                    description,
                    source,
                    tags,
                    impactStatement,
                    priority,
                    tenantId);
                
                idea.CreatorId = creatorId;
                
                // Submit the idea immediately instead of leaving it as draft
                idea.Submit();
                
                await _ideaRepository.InsertAsync(idea);
                return idea;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting idea with title: {Title}", title);
                throw;
            }
        }

        public async Task StartIdeaEvaluationAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.StartEvaluation();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting evaluation for idea {IdeaId}", ideaId);
                throw;
            }
        }

        public async Task<IdeaEvaluation> EvaluateIdeaAsync(
            Guid ideaId,
            Guid evaluatorId,
            string evaluatorName,
            string comments,
            bool isApproved,
            List<EvaluationScore> scores)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                var evaluation = idea.AddEvaluation(
                    evaluatorId,
                    evaluatorName,
                    comments,
                    isApproved,
                    DateTime.UtcNow);
                
                if (scores != null && scores.Any())
                {
                    foreach (var score in scores)
                    {
                        idea.AddEvaluationScore(evaluatorId, score);
                    }
                }
                
                await _ideaRepository.UpdateAsync(idea);
                return evaluation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating idea {IdeaId} by evaluator {EvaluatorId}", 
                    ideaId, evaluatorId);
                throw;
            }
        }

        public async Task ApproveIdeaAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.Approve();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving idea {IdeaId}", ideaId);
                throw;
            }
        }

        public async Task RejectIdeaAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.Reject();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting idea {IdeaId}", ideaId);
                throw;
            }
        }

        public async Task StartIdeaImplementationAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.StartImplementation();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting implementation for idea {IdeaId}", ideaId);
                throw;
            }
        }

        public async Task MarkIdeaImplementedAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.MarkAsImplemented();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking idea {IdeaId} as implemented", ideaId);
                throw;
            }
        }

        public async Task ArchiveIdeaAsync(Guid ideaId)
        {
            try
            {
                var idea = await _ideaRepository.GetAsync(ideaId);
                if (idea == null)
                {
                    throw new EntityNotFoundException("Idea", ideaId);
                }
                
                idea.Archive();
                await _ideaRepository.UpdateAsync(idea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error archiving idea {IdeaId}", ideaId);
                throw;
            }
        }

        public async Task<List<Idea>> GetIdeasByStatusAsync(IdeaStatus status)
        {
            try
            {
                return await _ideaRepository.GetByStatusAsync(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ideas with status {Status}", status);
                throw;
            }
        }

        public async Task<List<Idea>> GetIdeasByCreatorAsync(Guid creatorId)
        {
            try
            {
                return await _ideaRepository.GetByCreatorAsync(creatorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ideas for creator {CreatorId}", creatorId);
                throw;
            }
        }

        public async Task<List<Idea>> GetHighPriorityIdeasAsync()
        {
            try
            {
                return await _ideaRepository.GetHighPriorityIdeasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting high priority ideas");
                throw;
            }
        }

        public async Task<List<Idea>> GetIdeasForEvaluationAsync()
        {
            try
            {
                return await _ideaRepository.GetEvaluableIdeasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ideas for evaluation");
                throw;
            }
        }
    }
}
