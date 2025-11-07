using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Tags.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel.Inbox
{
    [Authorize]
    public class InboxItemAppService : ApplicationService, IInboxItemAppService
    {
        private readonly IInboxItemRepository _inboxItemRepository;

        public InboxItemAppService(IInboxItemRepository inboxItemRepository)
        {
            _inboxItemRepository = inboxItemRepository;
        }

        public async Task<ListResultDto<InboxItemDto>> GetMyUnprocessedItemsAsync()
        {
            var userId = CurrentUser.GetId();
            var items = await _inboxItemRepository.GetUnprocessedByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<InboxItem>, List<InboxItemDto>>(items);
            return new ListResultDto<InboxItemDto>(dtos);
        }

        public async Task<int> GetMyUnprocessedCountAsync()
        {
            var userId = CurrentUser.GetId();
            return await _inboxItemRepository.CountUnprocessedByUserAsync(userId);
        }

        public async Task<InboxItemDto> CreateAsync(CreateInboxItemDto input)
        {
            var userId = CurrentUser.GetId();

            var itemType = !string.IsNullOrWhiteSpace(input.ItemType) 
                ? Enum.Parse<InboxItemType>(input.ItemType) 
                : InboxItemType.Other;

            var source = !string.IsNullOrWhiteSpace(input.Source) 
                ? Enum.Parse<InboxItemSource>(input.Source) 
                : InboxItemSource.Manual;

            var item = new InboxItem(
                GuidGenerator.Create(),
                userId,
                input.InboxContent,
                itemType,
                source,
                CurrentTenant.Id
            );

            await _inboxItemRepository.InsertAsync(item);
            return ObjectMapper.Map<InboxItem, InboxItemDto>(item);
        }

        public async Task<InboxItemDto> ProcessAsync(Guid id, ProcessInboxItemDto input)
        {
            var userId = CurrentUser.GetId();
            var item = await _inboxItemRepository.GetAsync(id);

            if (item.OwnerId != userId)
            {
                throw new UserFriendlyException("You can only process your own inbox items.");
            }

            item.MarkAsProcessed(
                userId,
                input.ProcessedToEntityType,
                input.ProcessedToEntityId
            );

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                item.Notes = input.Notes;
            }

            await _inboxItemRepository.UpdateAsync(item);
            return ObjectMapper.Map<InboxItem, InboxItemDto>(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _inboxItemRepository.DeleteAsync(id);
        }
    }
}
