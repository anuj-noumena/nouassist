using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ContractPortfolioManagement.Contracts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ContractPortfolioManagement.Contracts;

/// <summary>
/// Application service interface for Contract management
/// </summary>
public interface IContractAppService : IApplicationService
{
    /// <summary>
    /// Gets contract details by ID
    /// </summary>
    Task<ContractDto> GetAsync(Guid id);

    /// <summary>
    /// Gets a paged list of contracts
    /// </summary>
    Task<PagedResultDto<ContractListDto>> GetListAsync(GetContractListDto input);

    /// <summary>
    /// Creates a new contract
    /// </summary>
    Task<ContractDto> CreateAsync(CreateContractDto input);

    /// <summary>
    /// Updates an existing contract
    /// </summary>
    Task<ContractDto> UpdateAsync(Guid id, UpdateContractDto input);

    /// <summary>
    /// Deletes a contract
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Changes the status of a contract
    /// </summary>
    Task<ContractDto> ChangeStatusAsync(Guid id, ChangeContractStatusDto input);

    /// <summary>
    /// Creates a new version of a contract
    /// </summary>
    Task<ContractVersionDto> CreateVersionAsync(Guid id, CreateContractVersionDto input);

    /// <summary>
    /// Adds a party to a contract
    /// </summary>
    Task<ContractPartyDto> AddPartyAsync(Guid id, CreateContractPartyDto input);

    /// <summary>
    /// Removes a party from a contract
    /// </summary>
    Task DeletePartyAsync(Guid contractId, Guid partyId);

    /// <summary>
    /// Adds an amendment to a contract
    /// </summary>
    Task<ContractAmendmentDto> AddAmendmentAsync(Guid id, CreateContractAmendmentDto input);

    /// <summary>
    /// Gets amendments for a contract
    /// </summary>
    Task<List<ContractAmendmentDto>> GetAmendmentsAsync(Guid id);

    /// <summary>
    /// Adds a clause to a contract
    /// </summary>
    Task<ContractClauseDto> AddClauseAsync(Guid id, CreateContractClauseDto input);

    /// <summary>
    /// Removes a clause from a contract
    /// </summary>
    Task DeleteClauseAsync(Guid contractId, Guid clauseId);

    /// <summary>
    /// Adds an alert to a contract
    /// </summary>
    Task<ContractAlertDto> AddAlertAsync(Guid id, CreateContractAlertDto input);

    /// <summary>
    /// Removes an alert from a contract
    /// </summary>
    Task DeleteAlertAsync(Guid contractId, Guid alertId);

    /// <summary>
    /// Gets versions of a contract
    /// </summary>
    Task<List<ContractVersionDto>> GetVersionsAsync(Guid id);

    /// <summary>
    /// Gets contracts that will expire within specified days
    /// </summary>
    Task<List<ContractListDto>> GetExpiringContractsAsync(int daysThreshold);

    /// <summary>
    /// Gets contracts needing attention (pending approval, expiring soon, etc.)
    /// </summary>
    Task<List<ContractListDto>> GetContractsNeedingAttentionAsync(int expirationDaysThreshold = 30);
}
