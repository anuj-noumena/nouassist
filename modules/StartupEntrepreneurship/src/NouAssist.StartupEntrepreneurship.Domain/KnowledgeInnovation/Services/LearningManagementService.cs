using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services
{
    /// <summary>
    /// Service interface for learning management operations
    /// </summary>
    public interface ILearningManagementService
    {
        /// <summary>
        /// Track user's progress on a learning resource
        /// </summary>
        Task<UserProgress> TrackProgressAsync(
            Guid resourceId,
            Guid userId,
            int progressPercentage,
            DateTime accessTime);

        /// <summary>
        /// Mark a learning resource as completed by user
        /// </summary>
        Task<UserProgress> MarkResourceCompletedAsync(
            Guid resourceId,
            Guid userId,
            DateTime completionTime);

        /// <summary>
        /// Mark a learning resource as abandoned by user
        /// </summary>
        Task MarkResourceAbandonedAsync(
            Guid resourceId,
            Guid userId,
            DateTime abandonTime);

        /// <summary>
        /// Get user's learning progress across all resources
        /// </summary>
        Task<List<UserProgress>> GetUserProgressAsync(Guid userId);

        /// <summary>
        /// Get resources in progress for user
        /// </summary>
        Task<List<LearningResource>> GetInProgressResourcesAsync(Guid userId);

        /// <summary>
        /// Get completed resources for user
        /// </summary>
        Task<List<LearningResource>> GetCompletedResourcesAsync(Guid userId);

        /// <summary>
        /// Get learning stats for user
        /// </summary>
        Task<LearningStats> GetUserLearningStatsAsync(Guid userId);
    }

    /// <summary>
    /// Learning statistics for a user
    /// </summary>
    public class LearningStats
    {
        public int TotalResourcesStarted { get; set; }
        public int ResourcesCompleted { get; set; }
        public int ResourcesInProgress { get; set; }
        public int ResourcesAbandoned { get; set; }
        public TimeSpan EstimatedLearningTime { get; set; }
        public List<string> TopLearningCategories { get; set; }

        public LearningStats()
        {
            TopLearningCategories = new List<string>();
        }
    }

    /// <summary>
    /// Implementation of learning management service
    /// </summary>
    public class LearningManagementService : DomainService, ILearningManagementService
    {
        private readonly ILearningResourceRepository _learningResourceRepository;
        private readonly ILogger<LearningManagementService> _logger;

        public LearningManagementService(
            ILearningResourceRepository learningResourceRepository,
            ILogger<LearningManagementService> logger)
        {
            _learningResourceRepository = learningResourceRepository;
            _logger = logger;
        }

        public async Task<UserProgress> TrackProgressAsync(
            Guid resourceId,
            Guid userId,
            int progressPercentage,
            DateTime accessTime)
        {
            try
            {
                var resource = await _learningResourceRepository.GetAsync(resourceId);
                if (resource == null)
                {
                    throw new EntityNotFoundException("LearningResource", resourceId);
                }

                var progress = resource.TrackUserProgress(userId, progressPercentage, accessTime);
                await _learningResourceRepository.UpdateAsync(resource);
                
                return progress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking progress for user {UserId} on resource {ResourceId}", 
                    userId, resourceId);
                throw;
            }
        }

        public async Task<UserProgress> MarkResourceCompletedAsync(
            Guid resourceId,
            Guid userId,
            DateTime completionTime)
        {
            try
            {
                var resource = await _learningResourceRepository.GetAsync(resourceId);
                if (resource == null)
                {
                    throw new EntityNotFoundException("LearningResource", resourceId);
                }

                resource.MarkResourceCompleted(userId, completionTime);
                await _learningResourceRepository.UpdateAsync(resource);
                
                return resource.GetUserProgress(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking resource {ResourceId} as completed for user {UserId}", 
                    resourceId, userId);
                throw;
            }
        }

        public async Task MarkResourceAbandonedAsync(
            Guid resourceId,
            Guid userId,
            DateTime abandonTime)
        {
            try
            {
                var resource = await _learningResourceRepository.GetAsync(resourceId);
                if (resource == null)
                {
                    throw new EntityNotFoundException("LearningResource", resourceId);
                }

                resource.MarkResourceAbandoned(userId, abandonTime);
                await _learningResourceRepository.UpdateAsync(resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking resource {ResourceId} as abandoned for user {UserId}", 
                    resourceId, userId);
                throw;
            }
        }

        public async Task<List<UserProgress>> GetUserProgressAsync(Guid userId)
        {
            try
            {
                var spec = new CompletedByUserSpecification(userId);
                var completedResources = await _learningResourceRepository.GetListAsync(spec);
                
                var progress = new List<UserProgress>();
                foreach (var resource in completedResources)
                {
                    var userProgress = resource.GetUserProgress(userId);
                    if (userProgress != null)
                    {
                        progress.Add(userProgress);
                    }
                }
                
                var inProgressResources = await _learningResourceRepository.GetInProgressByUserAsync(userId);
                foreach (var resource in inProgressResources)
                {
                    var userProgress = resource.GetUserProgress(userId);
                    if (userProgress != null && !progress.Any(p => p.LearningResourceId == resource.Id))
                    {
                        progress.Add(userProgress);
                    }
                }
                
                return progress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting progress for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<LearningResource>> GetInProgressResourcesAsync(Guid userId)
        {
            try
            {
                return await _learningResourceRepository.GetInProgressByUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting in-progress resources for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<LearningResource>> GetCompletedResourcesAsync(Guid userId)
        {
            try
            {
                return await _learningResourceRepository.GetCompletedByUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting completed resources for user {UserId}", userId);
                throw;
            }
        }

        public async Task<LearningStats> GetUserLearningStatsAsync(Guid userId)
        {
            try
            {
                var stats = new LearningStats();
                
                var progress = await GetUserProgressAsync(userId);
                
                stats.TotalResourcesStarted = progress.Count;
                stats.ResourcesCompleted = progress.Count(p => p.Status == CompletionStatus.Completed);
                stats.ResourcesInProgress = progress.Count(p => p.Status == CompletionStatus.InProgress);
                stats.ResourcesAbandoned = progress.Count(p => p.Status == CompletionStatus.Abandoned);
                
                // Calculate estimated learning time
                var completedResources = await GetCompletedResourcesAsync(userId);
                var estimatedMinutes = completedResources.Sum(r => r.Description.EstimatedDurationMinutes);
                stats.EstimatedLearningTime = TimeSpan.FromMinutes(estimatedMinutes);
                
                // Get top learning categories (from tags)
                var allTags = completedResources
                    .SelectMany(r => r.Tags)
                    .GroupBy(t => t)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(5)
                    .ToList();
                
                stats.TopLearningCategories = allTags;
                
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting learning stats for user {UserId}", userId);
                throw;
            }
        }
    }
}
