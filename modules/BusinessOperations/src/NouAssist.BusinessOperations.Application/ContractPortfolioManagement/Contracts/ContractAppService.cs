using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Localization;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Contracts.Dtos;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.Exceptions;
using NouAssist.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.ContractPortfolioManagement.Permissions;
using NouAssist.ContractPortfolioManagement.Repositories;
using NouAssist.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace NouAssist.ContractPortfolioManagement.Contracts;

[Authorize(ContractPortfolioManagementPermissions.Contract.Default)]
public class ContractAppService : ApplicationService, IContractAppService
{
    private readonly IContractRepository _contractRepository;
    private readonly IContractManager _contractManager;
    private readonly IClock _clock;
    private readonly ILogger<ContractAppService> _logger;

    public ContractAppService(
        IContractRepository contractRepository,
        IContractManager contractManager,
        IClock clock,
        ILogger<ContractAppService> logger)
    {
        _contractRepository = contractRepository;
        _contractManager = contractManager;
        _clock = clock;
        _logger = logger;
        
        LocalizationResource = typeof(ContractPortfolioManagementResource);
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<ContractDto> GetAsync(Guid id)
    {
        try
        {
            var contract = await _contractRepository.GetWithAllDetailsAsync(id);
            return ObjectMapper.Map<ContractAggregate, ContractDto>(contract);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<PagedResultDto<ContractListDto>> GetListAsync(GetContractListDto input)
    {
        try
        {
            var queryable = await _contractRepository.GetQueryableAsync();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(c => 
                    c.Title.Contains(input.Filter) || 
                    c.ReferenceNumber.Contains(input.Filter));
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(c => c.Status == input.Status.Value);
            }

            if (input.EffectiveDateMin.HasValue)
            {
                queryable = queryable.Where(c => c.EffectiveDate >= input.EffectiveDateMin.Value);
            }

            if (input.EffectiveDateMax.HasValue)
            {
                queryable = queryable.Where(c => c.EffectiveDate <= input.EffectiveDateMax.Value);
            }

            if (input.TotalValueMin.HasValue)
            {
                queryable = queryable.Where(c => c.TotalValue >= input.TotalValueMin.Value);
            }

            if (input.TotalValueMax.HasValue)
            {
                queryable = queryable.Where(c => c.TotalValue <= input.TotalValueMax.Value);
            }

            // Get total count
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Apply sorting and paging
            queryable = queryable
                .OrderBy(input.Sorting ?? "Title")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            // Get entities
            var contracts = await AsyncExecuter.ToListAsync(queryable);

            // Map to DTOs
            var contractDtos = ObjectMapper.Map<List<ContractAggregate>, List<ContractListDto>>(contracts);

            return new PagedResultDto<ContractListDto>(totalCount, contractDtos);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Create)]
    public async Task<ContractDto> CreateAsync(CreateContractDto input)
    {
        try
        {
            // Map DTOs to value objects
            var templateReference = ObjectMapper.Map<CreateContractTemplateReferenceDto, ContractTemplateReference>(input.TemplateReference);
            var documentId = ObjectMapper.Map<CreateDocumentIdDto, DocumentId>(input.MainDocument);
            
            // Use the domain service to create the contract
            var contract = await _contractManager.CreateContractAsync(
                input.Title,
                input.ReferenceNumber,
                input.Description,
                input.EffectiveDate,
                input.ExpirationDate,
                input.TotalValue,
                input.Currency,
                templateReference,
                documentId,
                CurrentTenant.Id);

            return ObjectMapper.Map<ContractAggregate, ContractDto>(contract);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task<ContractDto> UpdateAsync(Guid id, UpdateContractDto input)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(id);

            // Update basic details
            contract.UpdateBasicDetails(
                input.Title,
                input.ReferenceNumber,
                input.Description,
                input.EffectiveDate,
                input.ExpirationDate,
                input.TotalValue,
                input.Currency);

            await _contractRepository.UpdateAsync(contract);

            return ObjectMapper.Map<ContractAggregate, ContractDto>(
                await _contractRepository.GetWithAllDetailsAsync(id));
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _contractRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.ChangeStatus)]
    public async Task<ContractDto> ChangeStatusAsync(Guid id, ChangeContractStatusDto input)
    {
        try
        {
            var contract = await _contractManager.ChangeContractStatusAsync(id, input.NewStatus);
            return ObjectMapper.Map<ContractAggregate, ContractDto>(contract);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            
            if (bex is InvalidContractStatusTransitionException)
                throw new UserFriendlyException(
                    L["Contract:ErrorInvalidStatusTransition", bex.Data["currentStatus"], bex.Data["requestedStatus"]]);
                
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.CreateVersion)]
    public async Task<ContractVersionDto> CreateVersionAsync(Guid id, CreateContractVersionDto input)
    {
        try
        {
            var documentId = ObjectMapper.Map<CreateDocumentIdDto, DocumentId>(input.DocumentId);
            
            var contractVersion = await _contractManager.CreateContractVersionAsync(
                id,
                input.VersionLabel,
                input.ChangeReason,
                documentId);

            return ObjectMapper.Map<ContractVersion, ContractVersionDto>(contractVersion);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task<ContractPartyDto> AddPartyAsync(Guid id, CreateContractPartyDto input)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(id);

            var party = contract.AddParty(
                input.Name,
                input.Identifier,
                input.ContactInfo,
                input.PartyType);

            await _contractRepository.UpdateAsync(contract);

            return ObjectMapper.Map<ContractParty, ContractPartyDto>(party);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task DeletePartyAsync(Guid contractId, Guid partyId)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(contractId);

            contract.RemoveParty(partyId);

            await _contractRepository.UpdateAsync(contract);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", contractId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.AddAmendment)]
    public async Task<ContractAmendmentDto> AddAmendmentAsync(Guid id, CreateContractAmendmentDto input)
    {
        try
        {
            var amendmentDetails = new AmendmentDetails(
                input.Title,
                input.Description,
                input.EffectiveDate,
                input.AffectedSections);
                
            var documentId = ObjectMapper.Map<CreateDocumentIdDto, DocumentId>(input.DocumentId);
            
            var amendment = await _contractManager.AddContractAmendmentAsync(
                id,
                input.Title, 
                input.Description, 
                input.EffectiveDate, 
                input.AffectedSections, 
                documentId);

            return ObjectMapper.Map<ContractAmendment, ContractAmendmentDto>(amendment);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<List<ContractAmendmentDto>> GetAmendmentsAsync(Guid id)
    {
        try
        {
            var contract = await _contractRepository.GetWithAmendmentsAsync(id);
            return ObjectMapper.Map<List<ContractAmendment>, List<ContractAmendmentDto>>(contract.Amendments.ToList());
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task<ContractClauseDto> AddClauseAsync(Guid id, CreateContractClauseDto input)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(id);

            var clause = contract.AddClause(
                input.Title,
                input.Content,
                input.Reference,
                input.IsMandatory);

            await _contractRepository.UpdateAsync(contract);

            return ObjectMapper.Map<ContractClause, ContractClauseDto>(clause);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task DeleteClauseAsync(Guid contractId, Guid clauseId)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(contractId);

            contract.RemoveClause(clauseId);

            await _contractRepository.UpdateAsync(contract);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", contractId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task<ContractAlertDto> AddAlertAsync(Guid id, CreateContractAlertDto input)
    {
        try
        {
            var alert = await _contractManager.AddContractAlertAsync(
                id,
                input.Title,
                input.Description,
                input.AlertType,
                input.TriggerDate,
                input.DaysInAdvance,
                input.IsRecurring,
                input.Frequency);

            return ObjectMapper.Map<ContractAlert, ContractAlertDto>(alert);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.Update)]
    public async Task DeleteAlertAsync(Guid contractId, Guid alertId)
    {
        try
        {
            var contract = await _contractRepository.GetAsync(contractId);

            contract.RemoveAlert(alertId);

            await _contractRepository.UpdateAsync(contract);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", contractId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<List<ContractVersionDto>> GetVersionsAsync(Guid id)
    {
        try
        {
            var contract = await _contractRepository.GetWithVersionsAsync(id);
            return ObjectMapper.Map<List<ContractVersion>, List<ContractVersionDto>>(contract.Versions.ToList());
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Contract:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<List<ContractListDto>> GetExpiringContractsAsync(int daysThreshold)
    {
        try
        {
            var expiringContracts = await _contractManager.GetExpiringContractsAsync(
                daysThreshold, 
                CurrentTenant.Id);

            return ObjectMapper.Map<List<ContractAggregate>, List<ContractListDto>>(expiringContracts);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Contract.View)]
    public async Task<List<ContractListDto>> GetContractsNeedingAttentionAsync(int expirationDaysThreshold = 30)
    {
        try
        {
            var contracts = await _contractManager.GetContractsNeedingAttentionAsync(
                expirationDaysThreshold, 
                CurrentTenant.Id);

            return ObjectMapper.Map<List<ContractAggregate>, List<ContractListDto>>(contracts);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    protected async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
    {
        _logger.Log(logLevel, exception, 
            "Error in Contract Application Service: {ErrorMessage}", exception.Message);
        
        await Task.CompletedTask;
    }
}
