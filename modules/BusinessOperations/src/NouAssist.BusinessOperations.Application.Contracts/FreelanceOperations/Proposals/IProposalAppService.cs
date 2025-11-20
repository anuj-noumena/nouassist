using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals
{
    public interface IProposalAppService : IApplicationService
    {
        // Proposal CRUD
        Task<ProposalDto> GetAsync(Guid id);
        Task<PagedResultDto<ProposalDto>> GetListAsync(GetProposalListDto input);
        Task<ProposalDto> CreateAsync(CreateProposalDto input);
        Task<ProposalDto> UpdateAsync(Guid id, UpdateProposalDto input);
        Task DeleteAsync(Guid id);
        
        // Proposal Version operations
        Task<ProposalVersionDto> GetVersionAsync(Guid id);
        Task<List<ProposalVersionDto>> GetVersionsAsync(Guid proposalId);
        Task<ProposalVersionDto> AddVersionAsync(Guid proposalId, CreateProposalVersionDto input);
        Task<ProposalVersionDto> SetCurrentVersionAsync(Guid proposalId, Guid versionId);
        
        // Proposal status operations
        Task<ProposalDto> SendAsync(Guid id, SendProposalDto input);
        Task<ProposalDto> MarkAsViewedAsync(Guid id);
        Task<ProposalDto> AcceptAsync(Guid id);
        Task<ProposalDto> RejectAsync(Guid id);
        Task<ProposalDto> MarkAsNeedsRevisionAsync(Guid id);
        Task<ProposalDto> MarkAsExpiredAsync(Guid id);
        
        // Query methods
        Task<List<ProposalDto>> GetProposalsByClientAsync(Guid clientId);
        Task<List<ProposalDto>> GetProposalsByProjectAsync(Guid projectId);
        Task<List<ProposalDto>> GetProposalsNearingExpirationAsync(int daysUntilExpiration = 3);
    }
}
