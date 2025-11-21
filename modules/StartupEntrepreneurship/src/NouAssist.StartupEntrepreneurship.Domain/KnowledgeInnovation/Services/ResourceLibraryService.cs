using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services
{
    /// <summary>
    /// Service interface for resource library operations
    /// </summary>
    public interface IResourceLibraryService
    {
        /// <summary>
        /// Search for knowledge assets
        /// </summary>
        Task<List<KnowledgeAsset>> SearchKnowledgeAssetsAsync(
            string searchText,
            KnowledgeAssetType? assetType = null,
            List<string> tags = null,
            int maxResults = 10);

        /// <summary>
        /// Get recommended knowledge assets based on user's activity and interests
        /// </summary>
        Task<List<KnowledgeAsset>> GetRecommendedAssetsAsync(
            Guid userId,
            int maxResults = 5);

        /// <summary>
        /// Search for learning resources
        /// </summary>
        Task<List<LearningResource>> SearchLearningResourcesAsync(
            string searchText,
            ResourceType? resourceType = null,
            List<string> tags = null,
            int maxResults = 10);

        /// <summary>
        /// Get learning resource recommendations for user
        /// </summary>
        Task<List<LearningResource>> GetRecommendedLearningResourcesAsync(
            Guid userId,
            int maxResults = 5);

        /// <summary>
        /// Tag a knowledge asset or learning resource
        /// </summary>
        Task<List<string>> AddTagsAsync(
            string entityType,
            Guid entityId,
            List<string> tags);

        /// <summary>
        /// Get all available tags
        /// </summary>
        Task<List<string>> GetAllTagsAsync();

        /// <summary>
        /// Get most popular tags
        /// </summary>
        Task<List<string>> GetPopularTagsAsync(int maxTags = 20);
    }

    /// <summary>
    /// Implementation of resource library service
    /// </summary>
    public class ResourceLibraryService : DomainService, IResourceLibraryService
    {
        private readonly IKnowledgeAssetRepository _knowledgeAssetRepository;
        private readonly ILearningResourceRepository _learningResourceRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<ResourceLibraryService> _logger;

        public ResourceLibraryService(
            IKnowledgeAssetRepository knowledgeAssetRepository,
            ILearningResourceRepository learningResourceRepository,
            ITagRepository tagRepository,
            ILogger<ResourceLibraryService> logger)
        {
            _knowledgeAssetRepository = knowledgeAssetRepository;
            _learningResourceRepository = learningResourceRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public async Task<List<KnowledgeAsset>> SearchKnowledgeAssetsAsync(
            string searchText,
            KnowledgeAssetType? assetType = null,
            List<string> tags = null,
            int maxResults = 10)
        {
            try
            {
                var assets = await _knowledgeAssetRepository.SearchByTitleAsync(searchText, maxResults);

                // Filter by asset type if specified
                if (assetType.HasValue)
                {
                    var typeSpec = new KnowledgeAssetTypeSpecification(assetType.Value);
                    assets = assets.Where(a => typeSpec.IsSatisfiedBy(a)).ToList();
                }

                // Filter by tags if specified
                if (tags != null && tags.Any())
                {
                    assets = assets.Where(a => tags.All(t => a.Tags.Contains(t))).ToList();
                }

                return assets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching knowledge assets with text: {SearchText}", searchText);
                throw;
            }
        }

        public async Task<List<KnowledgeAsset>> GetRecommendedAssetsAsync(
            Guid userId,
            int maxResults = 5)
        {
            try
            {
                // In a real implementation, this would use more sophisticated recommendation
                // algorithms considering user behavior, preferences, and asset usage patterns
                
                // For now, implement a simple recommendation strategy:
                // 1. Get assets authored by the user
                var authorSpec = new KnowledgeAssetByAuthorSpecification(userId);
                var userAssets = await _knowledgeAssetRepository.GetListAsync(authorSpec);
                
                // 2. Collect tags from user's assets
                var userTags = userAssets
                    .SelectMany(a => a.Tags)
                    .GroupBy(t => t)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(5)
                    .ToList();
                
                // 3. Find other assets with these tags
                var recommendations = new List<KnowledgeAsset>();
                foreach (var tag in userTags)
                {
                    var tagSpec = new KnowledgeAssetWithTagSpecification(tag);
                    var assetsWithTag = await _knowledgeAssetRepository.GetListAsync(tagSpec);
                    
                    // Exclude user's own assets
                    assetsWithTag = assetsWithTag
                        .Where(a => a.Author.UserId != userId)
                        .ToList();
                    
                    recommendations.AddRange(assetsWithTag);
                }
                
                // 4. Return top N unique recommendations
                return recommendations
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .Take(maxResults)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommended assets for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<LearningResource>> SearchLearningResourcesAsync(
            string searchText,
            ResourceType? resourceType = null,
            List<string> tags = null,
            int maxResults = 10)
        {
            try
            {
                // Get published resources
                var publishedSpec = new PublishedLearningResourceSpecification();
                var resources = await _learningResourceRepository.GetListAsync(publishedSpec);
                
                // Filter by title search
                resources = resources
                    .Where(r => r.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                // Filter by resource type if specified
                if (resourceType.HasValue)
                {
                    var typeSpec = new LearningResourceTypeSpecification(resourceType.Value);
                    resources = resources.Where(r => typeSpec.IsSatisfiedBy(r)).ToList();
                }
                
                // Filter by tags if specified
                if (tags != null && tags.Any())
                {
                    resources = resources
                        .Where(r => tags.All(t => r.Tags.Contains(t)))
                        .ToList();
                }
                
                return resources.Take(maxResults).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching learning resources with text: {SearchText}", searchText);
                throw;
            }
        }

        public async Task<List<LearningResource>> GetRecommendedLearningResourcesAsync(
            Guid userId,
            int maxResults = 5)
        {
            try
            {
                // Get user's completed resources
                var completedSpec = new CompletedByUserSpecification(userId);
                var completedResources = await _learningResourceRepository.GetListAsync(completedSpec);
                
                // Collect tags from completed resources
                var userTags = completedResources
                    .SelectMany(r => r.Tags)
                    .GroupBy(t => t)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(5)
                    .ToList();
                
                // Get published resources with similar tags
                var publishedSpec = new PublishedLearningResourceSpecification();
                var publishedResources = await _learningResourceRepository.GetListAsync(publishedSpec);
                
                // Find resources with matching tags that user hasn't completed
                var recommendations = publishedResources
                    .Where(r => r.Tags.Any(t => userTags.Contains(t)))
                    .Where(r => !completedResources.Any(cr => cr.Id == r.Id))
                    .OrderByDescending(r => r.Tags.Count(t => userTags.Contains(t)))
                    .Take(maxResults)
                    .ToList();
                
                // If not enough recommendations, add some featured resources
                if (recommendations.Count < maxResults)
                {
                    var featuredSpec = new FeaturedLearningResourceSpecification();
                    var featuredResources = await _learningResourceRepository.GetListAsync(featuredSpec);
                    
                    // Add featured resources not already in recommendations
                    recommendations.AddRange(
                        featuredResources
                            .Where(r => !recommendations.Any(rec => rec.Id == r.Id))
                            .Where(r => !completedResources.Any(cr => cr.Id == r.Id))
                            .Take(maxResults - recommendations.Count)
                    );
                }
                
                return recommendations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommended learning resources for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<string>> AddTagsAsync(
            string entityType,
            Guid entityId,
            List<string> tags)
        {
            try
            {
                if (entityType.Equals("KnowledgeAsset", StringComparison.OrdinalIgnoreCase))
                {
                    var asset = await _knowledgeAssetRepository.GetAsync(entityId);
                    if (asset == null)
                    {
                        throw new EntityNotFoundException("KnowledgeAsset", entityId);
                    }
                    
                    // Create any new tags
                    foreach (var tag in tags)
                    {
                        await _tagRepository.CreateIfNotExistsAsync(tag, asset.TenantId);
                    }
                    
                    // Update asset tags
                    var updatedTags = asset.Tags.ToList();
                    foreach (var tag in tags)
                    {
                        if (!updatedTags.Contains(tag))
                        {
                            updatedTags.Add(tag);
                        }
                    }
                    
                    asset.UpdateDetails(
                        asset.Title,
                        asset.Description,
                        asset.AssetType,
                        updatedTags);
                    
                    await _knowledgeAssetRepository.UpdateAsync(asset);
                    return asset.Tags.ToList();
                }
                else if (entityType.Equals("LearningResource", StringComparison.OrdinalIgnoreCase))
                {
                    var resource = await _learningResourceRepository.GetAsync(entityId);
                    if (resource == null)
                    {
                        throw new EntityNotFoundException("LearningResource", entityId);
                    }
                    
                    // Create any new tags
                    foreach (var tag in tags)
                    {
                        await _tagRepository.CreateIfNotExistsAsync(tag, resource.TenantId);
                    }
                    
                    // Update resource tags
                    var updatedTags = resource.Tags.ToList();
                    foreach (var tag in tags)
                    {
                        if (!updatedTags.Contains(tag))
                        {
                            updatedTags.Add(tag);
                        }
                    }
                    
                    resource.UpdateDetails(
                        resource.Title,
                        resource.ResourceType,
                        resource.ResourceUrl,
                        resource.Description,
                        updatedTags);
                    
                    await _learningResourceRepository.UpdateAsync(resource);
                    return resource.Tags.ToList();
                }
                else
                {
                    throw new BusinessRuleViolationException($"Unsupported entity type: {entityType}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tags to {EntityType} with ID {EntityId}", entityType, entityId);
                throw;
            }
        }

        public async Task<List<string>> GetAllTagsAsync()
        {
            try
            {
                var tags = await _tagRepository.GetAllAsync();
                return tags.Select(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tags");
                throw;
            }
        }

        public async Task<List<string>> GetPopularTagsAsync(int maxTags = 20)
        {
            try
            {
                var tags = await _tagRepository.GetPopularTagsAsync(5, maxTags);
                return tags.Select(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting popular tags");
                throw;
            }
        }
    }
}
