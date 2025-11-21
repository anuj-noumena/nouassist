using System;
using System.Collections.Generic;
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
    /// Service interface for mentor network operations
    /// </summary>
    public interface IMentorNetworkService
    {
        /// <summary>
        /// Create a new mentor profile
        /// </summary>
        Task<MentorProfile> CreateMentorProfileAsync(
            Guid id,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl,
            List<string> tags,
            Guid creatorId,
            Guid? tenantId = null);

        /// <summary>
        /// Update a mentor profile
        /// </summary>
        Task UpdateMentorProfileAsync(
            Guid mentorId,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl,
            List<string> tags);

        /// <summary>
        /// Add a relationship to a mentor profile
        /// </summary>
        Task AddRelationshipAsync(
            Guid mentorId,
            Guid relatedEntityId,
            string relatedEntityName,
            NetworkRelationshipType relationshipType,
            string description);

        /// <summary>
        /// Remove a relationship from a mentor profile
        /// </summary>
        Task RemoveRelationshipAsync(
            Guid mentorId,
            Guid relatedEntityId);

        /// <summary>
        /// Add advice record to a mentor profile
        /// </summary>
        Task<AdviceRecord> AddAdviceAsync(
            Guid mentorId,
            string topic,
            string content,
            List<string> tags);

        /// <summary>
        /// Get mentors by expertise
        /// </summary>
        Task<List<MentorProfile>> GetMentorsByExpertiseAsync(string expertiseKeyword);

        /// <summary>
        /// Get experienced mentors
        /// </summary>
        Task<List<MentorProfile>> GetExperiencedMentorsAsync(int minimumYears);

        /// <summary>
        /// Get mentors by tag
        /// </summary>
        Task<List<MentorProfile>> GetMentorsByTagAsync(string tag);

        /// <summary>
        /// Search for mentors
        /// </summary>
        Task<List<MentorProfile>> SearchMentorsAsync(
            string searchText,
            int maxResults = 10);
    }

    /// <summary>
    /// Implementation of mentor network service
    /// </summary>
    public class MentorNetworkService : DomainService, IMentorNetworkService
    {
        private readonly IMentorProfileRepository _mentorRepository;
        private readonly ILogger<MentorNetworkService> _logger;

        public MentorNetworkService(
            IMentorProfileRepository mentorRepository,
            ILogger<MentorNetworkService> logger)
        {
            _mentorRepository = mentorRepository;
            _logger = logger;
        }

        public async Task<MentorProfile> CreateMentorProfileAsync(
            Guid id,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl,
            List<string> tags,
            Guid creatorId,
            Guid? tenantId = null)
        {
            try
            {
                var mentor = MentorProfile.Create(
                    id,
                    name,
                    bio,
                    expertise,
                    experienceYears,
                    profilePictureUrl,
                    tags,
                    tenantId);
                
                mentor.CreatorId = creatorId;
                
                await _mentorRepository.InsertAsync(mentor);
                return mentor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating mentor profile for {Name}", name);
                throw;
            }
        }

        public async Task UpdateMentorProfileAsync(
            Guid mentorId,
            string name,
            string bio,
            string expertise,
            int experienceYears,
            string profilePictureUrl,
            List<string> tags)
        {
            try
            {
                var mentor = await _mentorRepository.GetAsync(mentorId);
                if (mentor == null)
                {
                    throw new EntityNotFoundException("MentorProfile", mentorId);
                }
                
                mentor.UpdateProfile(
                    name,
                    bio,
                    expertise,
                    experienceYears,
                    profilePictureUrl,
                    tags);
                
                await _mentorRepository.UpdateAsync(mentor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating mentor profile {MentorId}", mentorId);
                throw;
            }
        }

        public async Task AddRelationshipAsync(
            Guid mentorId,
            Guid relatedEntityId,
            string relatedEntityName,
            NetworkRelationshipType relationshipType,
            string description)
        {
            try
            {
                var mentor = await _mentorRepository.GetAsync(mentorId);
                if (mentor == null)
                {
                    throw new EntityNotFoundException("MentorProfile", mentorId);
                }
                
                var relationship = new NetworkRelationship(
                    relatedEntityId,
                    relatedEntityName,
                    relationshipType,
                    description,
                    DateTime.UtcNow);
                
                mentor.AddRelationship(relationship);
                await _mentorRepository.UpdateAsync(mentor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding relationship to mentor {MentorId}", mentorId);
                throw;
            }
        }

        public async Task RemoveRelationshipAsync(
            Guid mentorId,
            Guid relatedEntityId)
        {
            try
            {
                var mentor = await _mentorRepository.GetAsync(mentorId);
                if (mentor == null)
                {
                    throw new EntityNotFoundException("MentorProfile", mentorId);
                }
                
                mentor.RemoveRelationship(relatedEntityId);
                await _mentorRepository.UpdateAsync(mentor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing relationship from mentor {MentorId}", mentorId);
                throw;
            }
        }

        public async Task<AdviceRecord> AddAdviceAsync(
            Guid mentorId,
            string topic,
            string content,
            List<string> tags)
        {
            try
            {
                var mentor = await _mentorRepository.GetAsync(mentorId);
                if (mentor == null)
                {
                    throw new EntityNotFoundException("MentorProfile", mentorId);
                }
                
                var advice = mentor.AddAdvice(
                    topic,
                    content,
                    DateTime.UtcNow,
                    tags);
                
                await _mentorRepository.UpdateAsync(mentor);
                return advice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding advice to mentor {MentorId}", mentorId);
                throw;
            }
        }

        public async Task<List<MentorProfile>> GetMentorsByExpertiseAsync(string expertiseKeyword)
        {
            try
            {
                return await _mentorRepository.GetByExpertiseAsync(expertiseKeyword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mentors by expertise {ExpertiseKeyword}", expertiseKeyword);
                throw;
            }
        }

        public async Task<List<MentorProfile>> GetExperiencedMentorsAsync(int minimumYears)
        {
            try
            {
                return await _mentorRepository.GetExperiencedMentorsAsync(minimumYears);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting experienced mentors with minimum {MinimumYears} years", minimumYears);
                throw;
            }
        }

        public async Task<List<MentorProfile>> GetMentorsByTagAsync(string tag)
        {
            try
            {
                return await _mentorRepository.GetByTagAsync(tag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mentors by tag {Tag}", tag);
                throw;
            }
        }

        public async Task<List<MentorProfile>> SearchMentorsAsync(
            string searchText,
            int maxResults = 10)
        {
            try
            {
                var allMentors = await _mentorRepository.GetListAsync();
                
                return allMentors
                    .Where(m => 
                        m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        m.Expertise.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        m.Bio.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        m.Tags.Any(t => t.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                    .Take(maxResults)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching mentors with text {SearchText}", searchText);
                throw;
            }
        }
    }
}
