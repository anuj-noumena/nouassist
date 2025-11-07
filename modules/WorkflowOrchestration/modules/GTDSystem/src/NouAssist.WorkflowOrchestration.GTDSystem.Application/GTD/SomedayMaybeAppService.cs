using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Data;
using NouAssist.WorkflowOrchestration.GTDSystem.Dtos;
using NouAssist.WorkflowOrchestration.GTDSystem.Entities;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using NouAssist.WorkflowOrchestration.GTDSystem.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkflowOrchestration.GTDSystem.GTD
{
    [Authorize(GtdPermissions.SomedayMaybe.Default)]
    public class SomedayMaybeAppService : ApplicationService, ISomedayMaybeAppService
    {
        private readonly ISomedayMaybeRepository _somedayMaybeRepository;
        private readonly IRepository<SomedayMaybeReview, Guid> _reviewRepository;

        public SomedayMaybeAppService(
            ISomedayMaybeRepository somedayMaybeRepository,
            IRepository<SomedayMaybeReview, Guid> reviewRepository)
        {
            _somedayMaybeRepository = somedayMaybeRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<SomedayMaybeItemDto> GetAsync(Guid id)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        public async Task<PagedResultDto<SomedayMaybeItemDto>> GetListAsync(GetSomedayMaybeListDto input)
        {
            var userId = CurrentUser.GetId();
            var queryable = await _somedayMaybeRepository.GetQueryableAsync();
            
            var query = queryable.Where(s => s.OwnerId == userId);

            if (input.Status.HasValue)
                query = query.Where(s => s.Status == input.Status.Value);

            if (!string.IsNullOrWhiteSpace(input.Category))
                query = query.Where(s => s.Category == input.Category);

            if (input.NeedsReview == true)
            {
                var reviewThreshold = Clock.Now.AddDays(-90);
                query = query.Where(s => 
                    s.Status == SomedayMaybeStatus.Active &&
                    (!s.LastReviewedAt.HasValue || s.LastReviewedAt.Value < reviewThreshold));
            }

            var totalCount = await AsyncExecuter.CountAsync(query);
            
            query = query
                .OrderBy(input.Sorting ?? "CreationTime DESC")
                .PageBy(input.SkipCount, input.MaxResultCount);

            var items = await AsyncExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<SomedayMaybeItem>, List<SomedayMaybeItemDto>>(items);

            return new PagedResultDto<SomedayMaybeItemDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<SomedayMaybeItemDto>> GetItemsNeedingReviewAsync()
        {
            var userId = CurrentUser.GetId();
            var items = await _somedayMaybeRepository.GetNeedingReviewAsync(userId);
            var dtos = ObjectMapper.Map<List<SomedayMaybeItem>, List<SomedayMaybeItemDto>>(items);
            return new ListResultDto<SomedayMaybeItemDto>(dtos);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Create)]
        public async Task<SomedayMaybeItemDto> CreateAsync(CreateSomedayMaybeItemDto input)
        {
            var userId = CurrentUser.GetId();

            var item = new SomedayMaybeItem(
                GuidGenerator.Create(),
                userId,
                input.Title,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                item.Description = input.Description;

            if (!string.IsNullOrWhiteSpace(input.Motivation))
                item.Motivation = input.Motivation;

            if (!string.IsNullOrWhiteSpace(input.Category))
                item.Category = input.Category;

            await _somedayMaybeRepository.InsertAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Update)]
        public async Task<SomedayMaybeItemDto> UpdateAsync(Guid id, UpdateSomedayMaybeItemDto input)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                item.Title = input.Title;

            if (input.Description != null)
                item.Description = input.Description;

            if (input.Notes != null)
                item.Notes = input.Notes;

            if (input.WhyNotNow != null)
                item.UpdateWhyNotNow(input.WhyNotNow);

            await _somedayMaybeRepository.UpdateAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _somedayMaybeRepository.DeleteAsync(id);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Promote)]
        public async Task<SomedayMaybeItemDto> PromoteAsync(Guid id, PromoteSomedayMaybeDto input)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);
            item.Promote(input.PromotedToType, input.PromotedToId);
            await _somedayMaybeRepository.UpdateAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Update)]
        public async Task<SomedayMaybeItemDto> ArchiveAsync(Guid id)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);
            item.Archive();
            await _somedayMaybeRepository.UpdateAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Update)]
        public async Task<SomedayMaybeItemDto> ActivateAsync(Guid id)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);
            item.Activate();
            await _somedayMaybeRepository.UpdateAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Update)]
        public async Task<SomedayMaybeItemDto> PutOnHoldAsync(Guid id)
        {
            var item = await _somedayMaybeRepository.GetAsync(id);
            item.PutOnHold();
            await _somedayMaybeRepository.UpdateAsync(item);
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }

        [Authorize(GtdPermissions.SomedayMaybe.Review)]
        public async Task<SomedayMaybeItemDto> ReviewAsync(Guid id, ReviewSomedayMaybeDto input)
        {
            var userId = CurrentUser.GetId();
            var item = await _somedayMaybeRepository.GetAsync(id);
            
            item.Review();

            // Create review record
            var review = new SomedayMaybeReview(
                GuidGenerator.Create(),
                id,
                userId
            );

            if (!string.IsNullOrWhiteSpace(input.Decision))
                review.Decision = input.Decision;

            if (input.InterestLevel.HasValue)
                review.SetInterestLevel(input.InterestLevel.Value);

            if (input.ShouldPromote == true)
                review.RecommendPromotion();

            if (!string.IsNullOrWhiteSpace(input.Notes))
                review.Notes = input.Notes;

            await _reviewRepository.InsertAsync(review);
            await _somedayMaybeRepository.UpdateAsync(item);
            
            return ObjectMapper.Map<SomedayMaybeItem, SomedayMaybeItemDto>(item);
        }
    }
}
