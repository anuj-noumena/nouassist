namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing contacts
    /// </summary>
    public interface IContactManager
    {
        /// <summary>
        /// Creates a new contact
        /// </summary>
        Task<Contact> CreateAsync(
            string name,
            ContactInfo contactInfo,
            string jobTitle = null,
            Guid? organizationId = null,
            bool isPrimaryContact = false,
            string notes = null);
            
        /// <summary>
        /// Updates an existing contact
        /// </summary>
        Task<Contact> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            string jobTitle = null,
            string notes = null);
            
        /// <summary>
        /// Assigns a contact to an organization
        /// </summary>
        Task<Contact> AssignToOrganizationAsync(
            Guid id,
            Guid organizationId,
            bool isPrimaryContact = false);
            
        /// <summary>
        /// Removes a contact from an organization
        /// </summary>
        Task<Contact> RemoveFromOrganizationAsync(
            Guid id);
            
        /// <summary>
        /// Adds a contact to a group
        /// </summary>
        Task<Contact> AddToGroupAsync(
            Guid id,
            Guid groupId);
            
        /// <summary>
        /// Removes a contact from a group
        /// </summary>
        Task<Contact> RemoveFromGroupAsync(
            Guid id,
            Guid groupId);
            
        /// <summary>
        /// Adds a tag to a contact
        /// </summary>
        Task<Contact> AddTagAsync(
            Guid id,
            string tag);
            
        /// <summary>
        /// Removes a tag from a contact
        /// </summary>
        Task<Contact> RemoveTagAsync(
            Guid id,
            string tag);
            
        /// <summary>
        /// Activates a contact
        /// </summary>
        Task<Contact> ActivateAsync(
            Guid id);
            
        /// <summary>
        /// Deactivates a contact
        /// </summary>
        Task<Contact> DeactivateAsync(
            Guid id);
    }
}
