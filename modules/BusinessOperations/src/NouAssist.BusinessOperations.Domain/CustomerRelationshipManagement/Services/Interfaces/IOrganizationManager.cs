namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing organizations
    /// </summary>
    public interface IOrganizationManager
    {
        /// <summary>
        /// Creates a new organization
        /// </summary>
        Task<Organization> CreateAsync(
            string name,
            ContactInfo contactInfo,
            OrganizationInfo details,
            string registrationNumber = null,
            Guid? parentOrganizationId = null);
            
        /// <summary>
        /// Updates an existing organization
        /// </summary>
        Task<Organization> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            OrganizationInfo details = null,
            string registrationNumber = null);
            
        /// <summary>
        /// Sets the parent organization
        /// </summary>
        Task<Organization> SetParentOrganizationAsync(
            Guid id,
            Guid parentId);
            
        /// <summary>
        /// Removes the parent organization
        /// </summary>
        Task<Organization> RemoveParentOrganizationAsync(
            Guid id);
            
        /// <summary>
        /// Adds a tag to an organization
        /// </summary>
        Task<Organization> AddTagAsync(
            Guid id,
            string tag);
            
        /// <summary>
        /// Removes a tag from an organization
        /// </summary>
        Task<Organization> RemoveTagAsync(
            Guid id,
            string tag);
            
        /// <summary>
        /// Activates an organization
        /// </summary>
        Task<Organization> ActivateAsync(
            Guid id);
            
        /// <summary>
        /// Deactivates an organization
        /// </summary>
        Task<Organization> DeactivateAsync(
            Guid id);
            
        /// <summary>
        /// Gets the contacts of an organization
        /// </summary>
        Task<List<Contact>> GetContactsAsync(
            Guid id,
            bool primaryOnly = false,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null);
    }
}
