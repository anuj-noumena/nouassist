using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCollections)]
    public class CollectionCaseAppService : ApplicationService, ICollectionCaseAppService
    {
        private readonly IRepository<CollectionCase, Guid> _collectionCaseRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public CollectionCaseAppService(
            IRepository<CollectionCase, Guid> collectionCaseRepository,
            ICustomerRepository customerRepository,
            IInvoiceRepository invoiceRepository)
        {
            _collectionCaseRepository = collectionCaseRepository;
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CollectionCaseDto> GetAsync(Guid id)
        {
            var collectionCase = await GetCaseWithDetailsAsync(id);
            return await MapToDtoAsync(collectionCase);
        }

        public async Task<PagedResultDto<CollectionCaseDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _collectionCaseRepository.GetQueryableAsync();

            // Filter to show only active cases by default
            queryable = queryable.Where(c => 
                c.Status != Enums.CollectionStatus.Closed &&
                c.Status != Enums.CollectionStatus.Paid);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(c => c.LastContactDate ?? DateTime.MinValue) // Prioritize cases not contacted
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var cases = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<CollectionCaseDto>();

            foreach (var collectionCase in cases)
            {
                var fullCase = await GetCaseWithDetailsAsync(collectionCase.Id);
                dtos.Add(await MapToDtoAsync(fullCase));
            }

            return new PagedResultDto<CollectionCaseDto>(totalCount, dtos);
        }

        public async Task<CollectionCaseDto> CreateAsync(CreateCollectionCaseDto input)
        {
            // Get customer and calculate overdue amount
            var customer = await _customerRepository.GetAsync(input.CustomerId);
            
            var overdueInvoices = await _invoiceRepository.GetOverdueInvoicesAsync();
            var customerOverdueInvoices = overdueInvoices
                .Where(i => i.CustomerId == input.CustomerId)
                .ToList();

            if (!customerOverdueInvoices.Any())
            {
                throw new Volo.Abp.UserFriendlyException(
                    "Customer has no overdue invoices. Cannot create collection case.");
            }

            var totalOverdue = customerOverdueInvoices.Sum(i => i.Balance.Amount);
            var currency = customerOverdueInvoices.First().Balance.Currency;

            var totalOverdueAmount = new Shared.ValueObjects.MonetaryValue(totalOverdue, currency);

            var collectionCase = new CollectionCase(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.CustomerId,
                totalOverdueAmount,
                input.AssignedTo);

            // Add initial activity
            var initialActivity = new CollectionActivity(
                collectionCase.Id,
                "System",
                $"Case created. {customerOverdueInvoices.Count} overdue invoices totaling {totalOverdue:C}",
                Clock.Now,
                CurrentUser.Id.Value);

            collectionCase.AddActivity(initialActivity);

            await _collectionCaseRepository.InsertAsync(collectionCase);

            return await MapToDtoAsync(collectionCase);
        }

        public async Task<CollectionCaseDto> AddActivityAsync(Guid caseId, AddCollectionActivityDto input)
        {
            var collectionCase = await GetCaseWithDetailsAsync(caseId);

            var activity = new CollectionActivity(
                caseId,
                input.ActivityType,
                input.Description,
                input.ActivityDate,
                CurrentUser.Id.Value,
                input.ContactedPerson,
                input.FollowUpDate);

            collectionCase.AddActivity(activity);

            await _collectionCaseRepository.UpdateAsync(collectionCase);

            return await MapToDtoAsync(collectionCase);
        }

        public async Task<CollectionCaseDto> UpdateStatusAsync(Guid caseId, UpdateCollectionStatusDto input)
        {
            var collectionCase = await GetCaseWithDetailsAsync(caseId);

            collectionCase.UpdateStatus(input.NewStatus);

            // Add status change activity
            var activity = new CollectionActivity(
                caseId,
                "StatusChange",
                $"Status updated to {input.NewStatus}",
                Clock.Now,
                CurrentUser.Id.Value);

            collectionCase.AddActivity(activity);

            await _collectionCaseRepository.UpdateAsync(collectionCase);

            return await MapToDtoAsync(collectionCase);
        }

        public async Task<CollectionCaseDto> AssignAsync(Guid caseId, Guid assigneeId)
        {
            var collectionCase = await GetCaseWithDetailsAsync(caseId);

            collectionCase.Assign(assigneeId);

            // Add assignment activity
            var activity = new CollectionActivity(
                caseId,
                "Assignment",
                $"Case reassigned to user {assigneeId}",
                Clock.Now,
                CurrentUser.Id.Value);

            collectionCase.AddActivity(activity);

            await _collectionCaseRepository.UpdateAsync(collectionCase);

            return await MapToDtoAsync(collectionCase);
        }

        private async Task<CollectionCase> GetCaseWithDetailsAsync(Guid id)
        {
            // In real implementation, use proper Include
            return await _collectionCaseRepository.GetAsync(id);
        }

        private async Task<CollectionCaseDto> MapToDtoAsync(CollectionCase collectionCase)
        {
            var dto = ObjectMapper.Map<CollectionCase, CollectionCaseDto>(collectionCase);

            // Load customer name
            var customer = await _customerRepository.GetAsync(collectionCase.CustomerId);
            dto.CustomerName = customer.Name;

            // Load assignee name (simplified - in real app, query user service)
            dto.AssignedToName = $"User-{collectionCase.AssignedTo}";

            return dto;
        }
    }
}
