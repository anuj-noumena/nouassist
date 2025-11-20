namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Integration
{
    /// <summary>
    /// Interface for looking up users from the identity system
    /// </summary>
    public interface IIdentityUserLookupService
    {
        /// <summary>
        /// Finds a user by their ID
        /// </summary>
        Task<IdentityUserDto> FindByIdAsync(Guid id);
    }

    /// <summary>
    /// Simple DTO for identity user information
    /// </summary>
    public class IdentityUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
