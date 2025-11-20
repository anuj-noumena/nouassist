namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Entities
{
    /// <summary>
    /// Represents a role that can be assigned to users with a collection of permissions.
    /// </summary>
    public class Role : Entity<Guid>
    {
        public RoleName Name { get; private set; }
        public string Description { get; private set; }
        public bool IsSystemRole { get; private set; }
        
        private readonly List<Permission> _permissions;
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();
        
        protected Role()
        {
            _permissions = new List<Permission>();
        }
        
        internal Role(
            Guid id,
            RoleName name,
            string description,
            bool isSystemRole = false) : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SetDescription(description);
            IsSystemRole = isSystemRole;
            _permissions = new List<Permission>();
        }
        
        internal void SetDescription(string description)
        {
            Description = Check.NotNull(
                description,
                nameof(description),
                maxLength: CollaborationConsts.MaxRoleDescriptionLength
            );
        }
        
        /// <summary>
        /// Adds a permission to this role.
        /// </summary>
        internal void AddPermission(PermissionType permissionType, TargetResource resource)
        {
            if (_permissions.Any(p => p.Type == permissionType && p.Resource.Equals(resource)))
            {
                return; // Permission already exists
            }
            
            _permissions.Add(new Permission(permissionType, resource));
        }
        
        /// <summary>
        /// Removes a permission from this role.
        /// </summary>
        internal void RemovePermission(PermissionType permissionType, TargetResource resource)
        {
            var permission = _permissions.FirstOrDefault(
                p => p.Type == permissionType && p.Resource.Equals(resource));
                
            if (permission != null)
            {
                _permissions.Remove(permission);
            }
        }
        
        /// <summary>
        /// Checks if this role has the specified permission for the resource.
        /// </summary>
        public bool HasPermission(PermissionType permissionType, TargetResourceType resourceType, Guid resourceId)
        {
            return _permissions.Any(p => 
                p.Includes(permissionType) && 
                p.AppliesTo(resourceType, resourceId));
        }
        
        /// <summary>
        /// Checks if this role has the specified permission for any resource of the specified type.
        /// </summary>
        public bool HasPermissionForResourceType(PermissionType permissionType, TargetResourceType resourceType)
        {
            return _permissions.Any(p => 
                p.Includes(permissionType) && 
                p.Resource.AppliesTo(resourceType));
        }
    }
}
