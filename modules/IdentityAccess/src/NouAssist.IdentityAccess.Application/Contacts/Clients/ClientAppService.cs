using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.Clients.Dtos;
using NouAssist.IdentityAccess.Contacts.Data;
using NouAssist.IdentityAccess.Contacts.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Contacts.Clients
{
    [Authorize(ContactsPermissions.Clients.Default)]
    public class ClientAppService : ApplicationService, IClientAppService
    {
        private readonly IClientRepository _clientRepository;

        public ClientAppService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to view this client");
            }

            return await MapToGetOutputDtoAsync(client);
        }

        public async Task<PagedResultDto<ClientDto>> GetListAsync(GetClientListDto input)
        {
            var ownerId = CurrentUser.GetId();
            var queryable = await _clientRepository.GetQueryableAsync();

            // Filter by owner
            queryable = queryable.Where(c => c.OwnerId == ownerId);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(c =>
                    c.Name.Contains(input.Filter) ||
                    c.LegalName.Contains(input.Filter) ||
                    c.Email.Contains(input.Filter));
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(c => c.Status == input.Status.Value);
            }

            if (input.IsActive.HasValue)
            {
                queryable = queryable.Where(c => c.IsActive == input.IsActive.Value);
            }

            if (input.NeedsReview == true)
            {
                queryable = queryable.Where(c => c.NextReviewDate.HasValue && c.NextReviewDate.Value <= DateTime.UtcNow);
            }

            if (!string.IsNullOrWhiteSpace(input.Industry))
            {
                queryable = queryable.Where(c => c.Industry == input.Industry);
            }

            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                queryable = queryable.Where(c => c.Tags.Contains(input.Tag));
            }

            var totalCount = queryable.Count();

            // Apply sorting
            queryable = !string.IsNullOrWhiteSpace(input.Sorting)
                ? queryable.OrderBy(input.Sorting)
                : queryable.OrderBy(c => c.Name);

            // Apply paging
            var clients = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var clientDtos = new List<ClientDto>();
            foreach (var client in clients)
            {
                clientDtos.Add(await MapToGetOutputDtoAsync(client));
            }

            return new PagedResultDto<ClientDto>(totalCount, clientDtos);
        }

        [Authorize(ContactsPermissions.Clients.Create)]
        public async Task<ClientDto> CreateAsync(CreateClientDto input)
        {
            var ownerId = CurrentUser.GetId();

            var client = new Client(
                GuidGenerator.Create(),
                ownerId,
                input.Name,
                CurrentTenant.Id,
                input.Email,
                input.Phone
            );

            client.LegalName = input.LegalName;
            client.Industry = input.Industry;
            client.Website = input.Website;
            client.PrimaryContactId = input.PrimaryContactId;

            await _clientRepository.InsertAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.Edit)]
        public async Task<ClientDto> UpdateAsync(Guid id, UpdateClientDto input)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.SetName(input.Name);
            client.LegalName = input.LegalName;
            client.Industry = input.Industry;
            client.Website = input.Website;
            client.PrimaryContactId = input.PrimaryContactId;
            client.Email = input.Email;
            client.Phone = input.Phone;
            client.NextReviewDate = input.NextReviewDate;

            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to delete this client");
            }

            await _clientRepository.DeleteAsync(id);
        }

        [Authorize(ContactsPermissions.Clients.ManageStatus)]
        public async Task<ClientDto> ActivateAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.Activate();
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.ManageStatus)]
        public async Task<ClientDto> DeactivateAsync(Guid id, string reason = null)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.Deactivate(reason);
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.ManageStatus)]
        public async Task<ClientDto> PutOnHoldAsync(Guid id, string reason = null)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.PutOnHold(reason);
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.ManageStatus)]
        public async Task<ClientDto> MarkAsLostAsync(Guid id, string reason = null)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.MarkAsLost(reason);
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.ManageStatus)]
        public async Task<ClientDto> ConvertToPayingAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.ConvertToPaying();
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.Edit)]
        public async Task<ClientDto> RecordContactAsync(Guid id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.RecordContact();
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.Edit)]
        public async Task<ClientDto> UpdateBalanceAsync(Guid id, decimal amount)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.UpdateBalance(amount);
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        [Authorize(ContactsPermissions.Clients.Edit)]
        public async Task<ClientDto> AddTagAsync(Guid id, string tag)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client.OwnerId != CurrentUser.GetId())
            {
                throw new BusinessException("Clients:NotOwner", "You don't have permission to edit this client");
            }

            client.AddTag(tag);
            await _clientRepository.UpdateAsync(client);

            return await MapToGetOutputDtoAsync(client);
        }

        // Private helper methods
        private async Task<ClientDto> MapToGetOutputDtoAsync(Client client)
        {
            var dto = ObjectMapper.Map<Client, ClientDto>(client);
            // TODO: Load primary contact name if PrimaryContactId is set
            return dto;
        }
    }
}
