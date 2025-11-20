using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal
{
    public interface IClientPortalAppService : IApplicationService
    {
        // Project operations
        Task<ClientPortalProjectDto> GetProjectProgressAsync(Guid projectId);
        Task RecordProjectInteractionAsync(Guid projectId, ProjectInteractionDto input);
        
        // Proposal operations
        Task<List<ClientPortalProposalDto>> GetClientProposalsAsync();
        Task RecordProposalViewedAsync(Guid proposalId);
        Task<ClientPortalProposalDto> RespondToProposalAsync(Guid proposalId, ProposalResponseDto input);
    }
}
