using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Data;
using NouAssist.FinancialManagement.TaxManagement.Enums;
using NouAssist.FinancialManagement.TaxManagement.Permissions;
using NouAssist.FinancialManagement.TaxManagement.TaxProfiles.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.FinancialManagement.TaxManagement.TaxProfiles
{
    [Authorize(TaxManagementPermissions.TaxProfiles.Default)]
    public class TaxProfileAppService : ApplicationService, ITaxProfileAppService
    {
        private readonly ITaxProfileRepository _taxProfileRepository;

        public TaxProfileAppService(ITaxProfileRepository taxProfileRepository)
        {
            _taxProfileRepository = taxProfileRepository;
        }

        public async Task<TaxProfileDto> GetMyProfileAsync()
        {
            var userId = CurrentUser.GetId();
            var profile = await _taxProfileRepository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                // Create default profile
                profile = new TaxProfile(
                    GuidGenerator.Create(),
                    userId,
                    CurrentTenant.Id,
                    TaxFilingStatus.Single
                );
                await _taxProfileRepository.InsertAsync(profile);
            }

            return ObjectMapper.Map<TaxProfile, TaxProfileDto>(profile);
        }

        [Authorize(TaxManagementPermissions.TaxProfiles.Manage)]
        public async Task<TaxProfileDto> CreateOrUpdateAsync(CreateOrUpdateTaxProfileDto input)
        {
            var userId = CurrentUser.GetId();
            var profile = await _taxProfileRepository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                profile = new TaxProfile(
                    GuidGenerator.Create(),
                    userId,
                    CurrentTenant.Id,
                    input.DefaultFilingStatus
                );
                await _taxProfileRepository.InsertAsync(profile);
            }
            else
            {
                profile.UpdateFilingStatus(input.DefaultFilingStatus);
                profile.UpdateDependents(input.NumberOfDependents);
                profile.UpdateSettings(input.IsItemizer, input.IsSelfEmployed);
                
                if (!string.IsNullOrWhiteSpace(input.TaxPreparer))
                {
                    profile.SetTaxPreparer(input.TaxPreparer);
                }

                await _taxProfileRepository.UpdateAsync(profile);
            }

            return ObjectMapper.Map<TaxProfile, TaxProfileDto>(profile);
        }
    }
}
