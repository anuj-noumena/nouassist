namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Data
{
    /// <summary>
    /// Seeds initial data for the Collaboration module.
    /// </summary>
    public class CollaborationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IUserAccessProfileRepository _userAccessProfileRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        
        public CollaborationDataSeedContributor(
            IUserAccessProfileRepository userAccessProfileRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _userAccessProfileRepository = userAccessProfileRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            // Create default roles and permissions for the admin user
            var adminUserId = context.TenantId == null
                ? CollaborationDataSeedConsts.HostAdminUserId
                : CollaborationDataSeedConsts.TenantAdminUserId;
                
            await CreateAdminAccessProfileAsync(adminUserId);
        }
        
        private async Task CreateAdminAccessProfileAsync(Guid adminUserId)
        {
            // Check if profile already exists
            var existingProfile = await _userAccessProfileRepository.GetByUserIdAsync(adminUserId);
            
            if (existingProfile != null)
            {
                return;
            }
            
            // Create admin profile with full permissions
            var profile = UserAccessProfile.Create(
                _guidGenerator.Create(),
                new UserID(adminUserId),
                _currentTenant.Id);
                
            // Create admin role
            var adminRole = profile.CreateRole(
                _guidGenerator.Create(),
                new RoleName("Admin"),
                "Administrator role with full access to all resources",
                true);
                
            // Add full control permission for all resource types
            foreach (TargetResourceType resourceType in Enum.GetValues(typeof(TargetResourceType)))
            {
                adminRole.AddPermission(
                    PermissionType.FullControl,
                    new TargetResource(resourceType));
            }
            
            await _userAccessProfileRepository.InsertAsync(profile);
        }
    }
}
