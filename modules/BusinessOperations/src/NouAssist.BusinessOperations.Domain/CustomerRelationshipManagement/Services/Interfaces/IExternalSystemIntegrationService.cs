using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Service for integrating with external CRM systems
    /// </summary>
    public interface IExternalSystemIntegrationService
    {
        /// <summary>
        /// Exports a lead to an external system
        /// </summary>
        Task<string> ExportLeadAsync(Lead lead, string externalSystemName);
        
        /// <summary>
        /// Exports a contact to an external system
        /// </summary>
        Task<string> ExportContactAsync(Contact contact, string externalSystemName);
        
        /// <summary>
        /// Imports leads from an external system
        /// </summary>
        Task<int> ImportLeadsAsync(string externalSystemName, DateTime? modifiedSince = null);
        
        /// <summary>
        /// Imports contacts from an external system
        /// </summary>
        Task<int> ImportContactsAsync(string externalSystemName, DateTime? modifiedSince = null);
    }
}
