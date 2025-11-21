using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Localization;
using NouAssist.RequirementsManagement.Elicitation.Permissions;
using NouAssist.RequirementsManagement.Elicitation.Services;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts
{
    [Authorize(ElicitationPermissions.ElicitationArtifacts.Default)]
    public class ElicitationArtifactAppService : ApplicationService, IElicitationArtifactAppService
    {
        private readonly IRepository<ElicitationArtifact, Guid> _artifactRepository;
        private readonly IElicitationArtifactManager _artifactManager;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly ILogger<ElicitationArtifactAppService> _logger;

        public ElicitationArtifactAppService(
            IRepository<ElicitationArtifact, Guid> artifactRepository,
            IElicitationArtifactManager artifactManager,
            ICurrentUser currentUser,
            IClock clock,
            ILogger<ElicitationArtifactAppService> logger)
        {
            _artifactRepository = artifactRepository;
            _artifactManager = artifactManager;
            _currentUser = currentUser;
            _clock = clock;
            _logger = logger;
            LocalizationResource = typeof(ElicitationResource);
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.View)]
        public virtual async Task<ElicitationArtifactDto> GetAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.View)]
        public virtual async Task<PagedResultDto<ElicitationArtifactDto>> GetListAsync(GetElicitationArtifactListDto input)
        {
            try
            {
                var queryable = await _artifactRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Title.Contains(input.Filter!) || x.Description.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value);
                queryable = queryable.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
                queryable = queryable.WhereIf(input.SessionId.HasValue, x => x.SessionId == input.SessionId.Value);
                queryable = queryable.WhereIf(input.CreatedByStakeholderId.HasValue, x => x.CreatedByStakeholderId == input.CreatedByStakeholderId.Value);

                if (input.CreatedAfter.HasValue)
                    queryable = queryable.Where(x => x.CreationTime >= input.CreatedAfter.Value);
                if (input.CreatedBefore.HasValue)
                    queryable = queryable.Where(x => x.CreationTime <= input.CreatedBefore.Value);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var artifacts = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? nameof(ElicitationArtifact.CreationTime) + " DESC")
                             .PageBy(input)
                );

                return new PagedResultDto<ElicitationArtifactDto>(
                    totalCount,
                    ObjectMapper.Map<List<ElicitationArtifact>, List<ElicitationArtifactDto>>(artifacts)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving artifact list");
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Create)]
        public virtual async Task<ElicitationArtifactDto> CreateAsync(CreateElicitationArtifactDto input)
        {
            try
            {
                var artifact = await _artifactManager.CreateAsync(
                    input.Title,
                    input.Description,
                    input.Type,
                    input.SessionId,
                    input.CreatedByStakeholderId
                );

                _logger.LogInformation("Artifact created: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating artifact: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating artifact: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Create)]
        public virtual async Task<ElicitationArtifactDto> CreateUserStoryAsync(CreateUserStoryDto input)
        {
            try
            {
                var storyText = new StoryText(
                    input.StoryText.Persona,
                    input.StoryText.Goal,
                    input.StoryText.Benefit
                );

                var artifact = await _artifactManager.CreateUserStoryAsync(
                    input.Title,
                    input.Description,
                    input.SessionId,
                    input.CreatedByStakeholderId,
                    storyText
                );

                _logger.LogInformation("User story created: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating user story: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user story: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Create)]
        public virtual async Task<ElicitationArtifactDto> CreateUseCaseAsync(CreateUseCaseDto input)
        {
            try
            {
                var steps = input.UseCaseModel.Steps.Select(s => 
                    new UseCaseStep(s.StepNumber, s.Actor, s.Description)).ToList();

                var preCondition = input.UseCaseModel.PreCondition != null 
                    ? new PrePostCondition(input.UseCaseModel.PreCondition.Description, input.UseCaseModel.PreCondition.IsMandatory)
                    : null;

                var postCondition = input.UseCaseModel.PostCondition != null 
                    ? new PrePostCondition(input.UseCaseModel.PostCondition.Description, input.UseCaseModel.PostCondition.IsMandatory)
                    : null;

                var useCaseModel = new UseCaseModel(
                    input.UseCaseModel.Name,
                    input.UseCaseModel.Actors,
                    steps,
                    preCondition,
                    postCondition
                );

                var artifact = await _artifactManager.CreateUseCaseAsync(
                    input.Title,
                    input.Description,
                    input.SessionId,
                    input.CreatedByStakeholderId,
                    useCaseModel
                );

                _logger.LogInformation("Use case created: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating use case: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating use case: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Create)]
        public virtual async Task<ElicitationArtifactDto> CreateEventDefinitionAsync(CreateEventDefinitionDto input)
        {
            try
            {
                var eventDefinition = new EventDefinition(
                    input.EventDefinition.Name,
                    input.EventDefinition.Description,
                    input.EventDefinition.Trigger,
                    input.EventDefinition.Actors,
                    input.EventDefinition.Properties
                );

                var artifact = await _artifactManager.CreateEventDefinitionAsync(
                    input.Title,
                    input.Description,
                    input.SessionId,
                    input.CreatedByStakeholderId,
                    eventDefinition
                );

                _logger.LogInformation("Event definition created: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error creating event definition: {Title}", input.Title);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event definition: {Title}", input.Title);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> UpdateAsync(Guid id, UpdateElicitationArtifactDto input)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);

                // Check if artifact can be updated
                if (artifact.Status == ArtifactStatus.Approved || artifact.Status == ArtifactStatus.Implemented)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:CannotUpdateApprovedArtifact"]);
                }

                // Update properties using reflection (since setters are private)
                var titleProperty = typeof(ElicitationArtifact).GetProperty("Title", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?? typeof(ElicitationArtifact).GetProperty("Title");
                titleProperty?.SetValue(artifact, input.Title);

                var descProperty = typeof(ElicitationArtifact).GetProperty("Description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?? typeof(ElicitationArtifact).GetProperty("Description");
                descProperty?.SetValue(artifact, input.Description);

                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact updated: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found for update: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);

                // Check if artifact can be deleted
                if (artifact.Status == ArtifactStatus.Implemented)
                {
                    throw new UserFriendlyException(L["Elicitation:ErrorMessage:CannotDeleteImplementedArtifact"]);
                }

                await _artifactRepository.DeleteAsync(id);
                _logger.LogInformation("Artifact deleted: {ArtifactId}", id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found for deletion: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> SubmitForReviewAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.SubmitForReview();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact submitted for review: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error submitting artifact for review: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting artifact for review: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Approve)]
        public virtual async Task<ElicitationArtifactDto> ApproveAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.Approve();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact approved: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error approving artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Reject)]
        public virtual async Task<ElicitationArtifactDto> RejectAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.Reject();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact rejected: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error rejecting artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Approve)]
        public virtual async Task<ElicitationArtifactDto> MarkAsImplementedAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.MarkAsImplemented();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact marked as implemented: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error marking artifact as implemented: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking artifact as implemented: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> MarkAsObsoleteAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.MarkAsObsolete();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact marked as obsolete: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking artifact as obsolete: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> ReturnToDraftAsync(Guid id)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.ReturnToDraft();
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Artifact returned to draft: {Title} ({Id})", artifact.Title, artifact.Id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error returning artifact to draft: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning artifact to draft: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> AddTagAsync(Guid id, IdeaTagDto tagDto)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                var tag = new IdeaTag(tagDto.Name, tagDto.Color, tagDto.Description);
                artifact.AddTag(tag);
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Tag added to artifact: {TagName} -> {ArtifactId}", tag.Name, id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business error adding tag to artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L[bex.Code ?? "Elicitation:ErrorMessage:DefaultError"]);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tag to artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.Update)]
        public virtual async Task<ElicitationArtifactDto> RemoveTagAsync(Guid id, string tagName)
        {
            try
            {
                var artifact = await _artifactRepository.GetAsync(id);
                artifact.RemoveTag(tagName);
                await _artifactRepository.UpdateAsync(artifact);

                _logger.LogInformation("Tag removed from artifact: {TagName} -> {ArtifactId}", tagName, id);
                return ObjectMapper.Map<ElicitationArtifact, ElicitationArtifactDto>(artifact);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Artifact not found: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:ArtifactNotFound", id]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing tag from artifact: {ArtifactId}", id);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.View)]
        public virtual async Task<ListResultDto<ElicitationArtifactDto>> GetApprovedArtifactsAsync()
        {
            try
            {
                var artifacts = await _artifactManager.GetApprovedArtifactsAsync();
                return new ListResultDto<ElicitationArtifactDto>(
                    ObjectMapper.Map<List<ElicitationArtifact>, List<ElicitationArtifactDto>>(artifacts)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving approved artifacts");
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(ElicitationPermissions.ElicitationArtifacts.View)]
        public virtual async Task<ListResultDto<ElicitationArtifactDto>> GetArtifactsBySessionAsync(Guid sessionId)
        {
            try
            {
                var artifacts = await _artifactManager.GetArtifactsBySessionAsync(sessionId);
                return new ListResultDto<ElicitationArtifactDto>(
                    ObjectMapper.Map<List<ElicitationArtifact>, List<ElicitationArtifactDto>>(artifacts)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving artifacts by session: {SessionId}", sessionId);
                throw new UserFriendlyException(L["Elicitation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
