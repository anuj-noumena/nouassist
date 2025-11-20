namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for client portal operations
    /// </summary>
    public interface IClientPortalService
    {
        /// <summary>
        /// Gets project progress for client portal display
        /// </summary>
        Task<Dictionary<string, object>> GetProjectProgressAsync(Guid projectId, Guid clientId);
        
        /// <summary>
        /// Records client interaction with project in portal
        /// </summary>
        Task RecordClientProjectInteractionAsync(Guid projectId, Guid clientId, string interactionType);
        
        /// <summary>
        /// Gets client's proposals for display in portal
        /// </summary>
        Task<List<Dictionary<string, object>>> GetClientProposalsAsync(Guid clientId);
        
        /// <summary>
        /// Records when a client views a proposal
        /// </summary>
        Task RecordProposalViewedAsync(Guid proposalId, Guid clientId);
        
        /// <summary>
        /// Records client response to a proposal
        /// </summary>
        Task RecordProposalResponseAsync(Guid proposalId, Guid clientId, bool accepted);
    }
}
