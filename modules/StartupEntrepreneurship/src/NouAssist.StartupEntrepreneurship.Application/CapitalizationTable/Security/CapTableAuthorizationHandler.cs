using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Security
{
    /// <summary>
    /// Custom authorization handler for cap table specific permissions
    /// </summary>
    public class CapTableAuthorizationHandler : ITransientDependency
    {
        private readonly ICapTableRepository _capTableRepository;
        private readonly ISecurityHolderRepository _securityHolderRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;

        public CapTableAuthorizationHandler(
            ICapTableRepository capTableRepository,
            ISecurityHolderRepository securityHolderRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant)
        {
            _capTableRepository = capTableRepository;
            _securityHolderRepository = securityHolderRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
        }

        /// <summary>
        /// Checks if the current user can access a specific cap table
        /// </summary>
        public async Task<bool> CanAccessCapTableAsync(Guid capTableId)
        {
            if (!_currentUser.IsAuthenticated)
                return false;

            // Super admin can access everything
            if (_currentUser.IsInRole("admin"))
                return true;

            try
            {
                var capTable = await _capTableRepository.GetAsync(capTableId);
                
                // Check tenant ownership
                if (capTable.TenantId != _currentTenant.Id)
                    return false;

                // Additional checks could include:
                // - User is a security holder in this cap table
                // - User has specific roles related to this company
                // - User is an advisor or board member

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the current user can modify a specific security holder
        /// </summary>
        public async Task<bool> CanModifySecurityHolderAsync(Guid securityHolderId)
        {
            if (!_currentUser.IsAuthenticated)
                return false;

            // Super admin and company admin can modify any holder
            if (_currentUser.IsInRole("admin") || _currentUser.IsInRole("company_admin"))
                return true;

            try
            {
                var securityHolder = await _securityHolderRepository.GetAsync(securityHolderId);
                
                // Check tenant ownership
                if (securityHolder.TenantId != _currentTenant.Id)
                    return false;

                // User can modify their own information
                if (securityHolder.ExternalReference == _currentUser.Id?.ToString())
                    return true;

                // Additional business rules could be applied here

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the current user can view sensitive cap table information
        /// </summary>
        public async Task<bool> CanViewSensitiveInformationAsync(Guid capTableId)
        {
            if (!await CanAccessCapTableAsync(capTableId))
                return false;

            // Only specific roles can view sensitive information like valuations, full ownership details
            if (_currentUser.IsInRole("admin") || 
                _currentUser.IsInRole("company_admin") || 
                _currentUser.IsInRole("founder") ||
                _currentUser.IsInRole("investor"))
                return true;

            return false;
        }
    }
}
