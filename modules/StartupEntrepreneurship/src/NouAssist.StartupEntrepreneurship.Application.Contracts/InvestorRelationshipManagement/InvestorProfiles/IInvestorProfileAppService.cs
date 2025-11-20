using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles
{
    /// <summary>
    /// Application service interface for investor profile management
    /// </summary>
    public interface IInvestorProfileAppService : IApplicationService
    {
        // CRUD Operations
        Task<InvestorProfileDto> GetAsync(Guid id);
        Task<PagedResultDto<InvestorProfileDto>> GetListAsync(GetInvestorProfileListDto input);
        Task<InvestorProfileDto> CreateAsync(CreateInvestorProfileDto input);
        Task<InvestorProfileDto> UpdateAsync(Guid id, UpdateInvestorProfileDto input);
        Task DeleteAsync(Guid id);

        // Interaction Management
        Task<PagedResultDto<InvestorInteractionDto>> GetInteractionsAsync(Guid investorId, PagedAndSortedResultRequestDto input);
        Task<InvestorInteractionDto> AddInteractionAsync(Guid investorId, CreateInvestorInteractionDto input);
        Task DeleteInteractionAsync(Guid investorId, Guid interactionId);

        // Investment Management
        Task<PagedResultDto<InvestmentRecordDto>> GetInvestmentsAsync(Guid investorId, PagedAndSortedResultRequestDto input);
        Task<InvestmentRecordDto> AddInvestmentAsync(Guid investorId, CreateInvestmentRecordDto input);
        Task ActivateInvestmentAsync(Guid investorId, Guid investmentId);
        Task DeactivateInvestmentAsync(Guid investorId, Guid investmentId);

        // Relationship Management
        Task UpdateRelationshipScoreAsync(Guid id, UpdateRelationshipScoreDto input);
        Task<int> RecalculateRelationshipScoreAsync(Guid id);
        Task UpdateStatusAsync(Guid id, InvestorStatus status);

        // Validation
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);
    }
}
