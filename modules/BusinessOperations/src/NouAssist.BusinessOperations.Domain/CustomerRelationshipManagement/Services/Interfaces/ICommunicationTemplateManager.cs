using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing communication templates
    /// </summary>
    public interface ICommunicationTemplateManager
    {
        /// <summary>
        /// Creates a new communication template
        /// </summary>
        Task<CommunicationTemplate> CreateAsync(
            string code,
            string name,
            InteractionType type,
            string subject,
            string content,
            string description = null,
            string category = null);
            
        /// <summary>
        /// Updates an existing template
        /// </summary>
        Task<CommunicationTemplate> UpdateAsync(
            Guid id,
            string name = null,
            string subject = null,
            string content = null,
            string description = null,
            string category = null);
            
        /// <summary>
        /// Activates a template
        /// </summary>
        Task<CommunicationTemplate> ActivateAsync(
            Guid id);
            
        /// <summary>
        /// Deactivates a template
        /// </summary>
        Task<CommunicationTemplate> DeactivateAsync(
            Guid id);
            
        /// <summary>
        /// Gets a template by code
        /// </summary>
        Task<CommunicationTemplate> GetByCodeAsync(
            string code);
            
        /// <summary>
        /// Renders a template with the provided variables
        /// </summary>
        Task<(string Subject, string Content)> RenderTemplateAsync(
            Guid id,
            Dictionary<string, string> variables);
            
        /// <summary>
        /// Renders a template by code with the provided variables
        /// </summary>
        Task<(string Subject, string Content)> RenderTemplateByCodeAsync(
            string code,
            Dictionary<string, string> variables);
    }
}
