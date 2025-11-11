using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Reviews.Aggregates;
using NouAssist.Reviews.Data;
using NouAssist.Reviews.Enums;
using NouAssist.Reviews.Permissions;
using NouAssist.Reviews.Templates.Dtos;
using NouAssist.Reviews.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Reviews.Templates
{
    [Authorize(ReviewsPermissions.Templates.ViewSystem)]
    public class ReviewTemplateAppService : ApplicationService, IReviewTemplateAppService
    {
        private readonly IReviewTemplateRepository _templateRepository;

        public ReviewTemplateAppService(IReviewTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<ReviewTemplateDto> GetAsync(Guid id)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsSystem && template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to view this template");
            }

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        public async Task<PagedResultDto<ReviewTemplateDto>> GetListAsync(GetTemplateListDto input)
        {
            var ownerId = CurrentUser.GetId();
            var templates = await _templateRepository.GetTemplatesForOwnerAsync(ownerId, includeSystem: true);

            // Apply filters
            if (input.ReviewType.HasValue)
            {
                templates = templates.Where(t => t.ReviewType == input.ReviewType.Value).ToList();
            }

            if (input.IsSystem.HasValue)
            {
                templates = templates.Where(t => t.IsSystem == input.IsSystem.Value).ToList();
            }

            var totalCount = templates.Count;

            var pagedTemplates = templates
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var dtos = ObjectMapper.Map<List<ReviewTemplate>, List<ReviewTemplateDto>>(pagedTemplates);

            return new PagedResultDto<ReviewTemplateDto>(totalCount, dtos);
        }

        [Authorize(ReviewsPermissions.Templates.Create)]
        public async Task<ReviewTemplateDto> CreateAsync(CreateReviewTemplateDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var template = new ReviewTemplate(
                GuidGenerator.Create(),
                input.ReviewType,
                input.Title,
                ownerId,
                CurrentTenant.Id
            );

            template.Description = input.Description;
            template.RecommendedFrequency = input.RecommendedFrequency;
            template.UpdateEstimatedTime(input.EstimatedMinutes ?? 0);

            await _templateRepository.InsertAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        [Authorize(ReviewsPermissions.Templates.Edit)]
        public async Task<ReviewTemplateDto> UpdateAsync(Guid id, UpdateReviewTemplateDto input)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (template.IsSystem)
            {
                throw new BusinessException("Reviews:CannotEditSystemTemplate", 
                    "Cannot edit system templates");
            }

            if (template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this template");
            }

            template.UpdateTitle(input.Title);
            template.Description = input.Description;
            template.RecommendedFrequency = input.RecommendedFrequency;
            
            if (input.EstimatedMinutes.HasValue)
            {
                template.UpdateEstimatedTime(input.EstimatedMinutes.Value);
            }

            await _templateRepository.UpdateAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        [Authorize(ReviewsPermissions.Templates.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (template.IsSystem)
            {
                throw new BusinessException("Reviews:CannotDeleteSystemTemplate", 
                    "Cannot delete system templates");
            }

            if (template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to delete this template");
            }

            await _templateRepository.DeleteAsync(id);
        }

        public async Task<ReviewTemplateDto> GetByReviewTypeAsync(ReviewType reviewType)
        {
            var ownerId = CurrentUser.GetId();
            var template = await _templateRepository.GetByReviewTypeAsync(reviewType, ownerId);

            if (template == null)
            {
                return null;
            }

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        public async Task<ListResultDto<ReviewTemplateDto>> GetSystemTemplatesAsync()
        {
            var templates = await _templateRepository.GetSystemTemplatesAsync();
            var dtos = ObjectMapper.Map<List<ReviewTemplate>, List<ReviewTemplateDto>>(templates);

            return new ListResultDto<ReviewTemplateDto>(dtos);
        }

        public async Task<ListResultDto<ReviewTemplateDto>> GetCustomTemplatesAsync()
        {
            var ownerId = CurrentUser.GetId();
            var templates = await _templateRepository.GetCustomTemplatesAsync(ownerId);
            var dtos = ObjectMapper.Map<List<ReviewTemplate>, List<ReviewTemplateDto>>(templates);

            return new ListResultDto<ReviewTemplateDto>(dtos);
        }

        [Authorize(ReviewsPermissions.Templates.Edit)]
        public async Task<ReviewTemplateDto> AddChecklistStepAsync(Guid id, AddChecklistStepDto input)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsSystem && template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this template");
            }

            var step = new ReviewChecklistStep(input.Step, input.Title, input.Items, input.Description);
            template.AddChecklistStep(step);

            await _templateRepository.UpdateAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        [Authorize(ReviewsPermissions.Templates.Edit)]
        public async Task<ReviewTemplateDto> RemoveChecklistStepAsync(Guid id, int stepNumber)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsSystem && template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this template");
            }

            template.RemoveChecklistStep(stepNumber);

            await _templateRepository.UpdateAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        [Authorize(ReviewsPermissions.Templates.Edit)]
        public async Task<ReviewTemplateDto> AddReflectionPromptAsync(Guid id, string prompt)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsSystem && template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this template");
            }

            template.AddReflectionPrompt(prompt);

            await _templateRepository.UpdateAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }

        [Authorize(ReviewsPermissions.Templates.Edit)]
        public async Task<ReviewTemplateDto> RemoveReflectionPromptAsync(Guid id, string prompt)
        {
            var template = await _templateRepository.GetAsync(id);
            
            if (!template.IsSystem && template.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Reviews:NotOwner", "You don't have permission to edit this template");
            }

            template.RemoveReflectionPrompt(prompt);

            await _templateRepository.UpdateAsync(template);

            return ObjectMapper.Map<ReviewTemplate, ReviewTemplateDto>(template);
        }
    }
}
