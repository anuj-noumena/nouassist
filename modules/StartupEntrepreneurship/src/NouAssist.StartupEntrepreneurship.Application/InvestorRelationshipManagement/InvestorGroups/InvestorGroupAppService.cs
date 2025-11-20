using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups
{
    /// <summary>
    /// Application service for investor group management
    /// </summary>
    [Authorize(IRMPermissions.Groups.Default)]
    public class InvestorGroupAppService : ApplicationService, IInvestorGroupAppService
    {
        private readonly IInvestorGroupRepository _groupRepository;
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public InvestorGroupAppService(
            IInvestorGroupRepository groupRepository,
            IInvestorProfileRepository investorRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _groupRepository = groupRepository;
            _investorRepository = investorRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<InvestorGroupDto> GetAsync(Guid id)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);
                return await MapToExtendedDtoAsync(group);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
            }
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<InvestorGroupDto> GetWithMembersAsync(Guid id)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);
                return await MapToExtendedDtoAsync(group, includeMembers: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
            }
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<PagedResultDto<InvestorGroupDto>> GetListAsync(GetInvestorGroupListDto input)
        {
            try
            {
                var queryable = await _groupRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    g => g.Name.Contains(input.Filter!) || g.Description.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.IsActive.HasValue, g => g.IsActive == input.IsActive.Value);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var items = new List<InvestorGroup>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        queryable.OrderBy(input.Sorting ?? nameof(InvestorGroup.Name))
                                .PageBy(input)
                    );
                }

                var itemDtos = new List<InvestorGroupDto>();
                foreach (var item in items)
                {
                    var dto = await MapToExtendedDtoAsync(item, includeMembers: input.IncludeMembers);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<InvestorGroupDto>(totalCount, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<PagedResultDto<InvestorGroupDto>> GetActiveAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var groups = await _groupRepository.GetActiveGroupsAsync();

                var itemDtos = new List<InvestorGroupDto>();
                foreach (var group in groups.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var dto = await MapToExtendedDtoAsync(group);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<InvestorGroupDto>(groups.Count, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.Create)]
        public async Task<InvestorGroupDto> CreateAsync(CreateInvestorGroupDto input)
        {
            try
            {
                // Validate name uniqueness
                if (!await _groupRepository.IsNameUniqueAsync(input.Name))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:InvalidOperation", "Group name already exists"]);
                }

                var id = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                var group = new InvestorGroup(
                    id,
                    input.Name,
                    input.Description,
                    currentTenantId
                );

                await _groupRepository.InsertAsync(group, autoSave: true);

                return await MapToExtendedDtoAsync(group);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.Update)]
        public async Task<InvestorGroupDto> UpdateAsync(Guid id, UpdateInvestorGroupDto input)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                // Validate name uniqueness
                if (!await _groupRepository.IsNameUniqueAsync(input.Name, id))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:InvalidOperation", "Group name already exists"]);
                }

                group.UpdateName(input.Name);
                group.UpdateDescription(input.Description);

                await _groupRepository.UpdateAsync(group, autoSave: true);

                return await MapToExtendedDtoAsync(group);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                await _groupRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.ManageMembers)]
        public async Task AddInvestorAsync(Guid id, AddInvestorToGroupDto input)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                // Verify investor exists and belongs to same tenant
                var investor = await _investorRepository.GetAsync(input.InvestorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                group.AddInvestor(input.InvestorId);
                await _groupRepository.UpdateAsync(group, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group or Investor"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.ManageMembers)]
        public async Task RemoveInvestorAsync(Guid id, RemoveInvestorFromGroupDto input)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                group.RemoveInvestor(input.InvestorId);
                await _groupRepository.UpdateAsync(group, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<PagedResultDto<InvestorGroupMemberDto>> GetMembersAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                var investors = await _investorRepository.GetByIdsAsync(group.InvestorIds.ToList());

                var memberDtos = new List<InvestorGroupMemberDto>();
                foreach (var investor in investors.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var memberDto = new InvestorGroupMemberDto
                    {
                        InvestorId = investor.Id,
                        Name = investor.FullName,
                        Email = investor.ContactDetails.Email,
                        Type = investor.Type,
                        Status = investor.Status,
                        CompanyName = investor.CompanyName,
                        TotalInvestment = investor.GetTotalInvestedAmount(),
                        RelationshipScore = investor.RelationshipScore.Score
                    };
                    memberDtos.Add(memberDto);
                }

                return new PagedResultDto<InvestorGroupMemberDto>(group.InvestorIds.Count, memberDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.Update)]
        public async Task ActivateAsync(Guid id)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                group.Activate();
                await _groupRepository.UpdateAsync(group, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Groups.Update)]
        public async Task DeactivateAsync(Guid id)
        {
            try
            {
                var group = await _groupRepository.GetAsync(id);

                // Check tenant consistency
                if (group.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                }

                group.Deactivate();
                await _groupRepository.UpdateAsync(group, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Group"]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null)
        {
            return await _groupRepository.IsNameUniqueAsync(name, excludeId);
        }

        [Authorize(IRMPermissions.Groups.View)]
        public async Task<PagedResultDto<InvestorGroupDto>> GetGroupsForInvestorAsync(Guid investorId, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var groups = await _groupRepository.GetGroupsForInvestorAsync(investorId);

                var groupDtos = new List<InvestorGroupDto>();
                foreach (var group in groups.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var dto = await MapToExtendedDtoAsync(group);
                    groupDtos.Add(dto);
                }

                return new PagedResultDto<InvestorGroupDto>(groups.Count, groupDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper Methods
        private async Task<InvestorGroupDto> MapToExtendedDtoAsync(InvestorGroup group, bool includeMembers = false)
        {
            var dto = ObjectMapper.Map<InvestorGroup, InvestorGroupDto>(group);

            if (includeMembers)
            {
                var investors = await _investorRepository.GetByIdsAsync(group.InvestorIds.ToList());

                foreach (var investor in investors)
                {
                    var memberDto = new InvestorGroupMemberDto
                    {
                        InvestorId = investor.Id,
                        Name = investor.FullName,
                        Email = investor.ContactDetails.Email,
                        Type = investor.Type,
                        Status = investor.Status,
                        CompanyName = investor.CompanyName,
                        TotalInvestment = investor.GetTotalInvestedAmount(),
                        RelationshipScore = investor.RelationshipScore.Score
                    };
                    dto.Investors.Add(memberDto);
                }
            }

            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in InvestorGroupAppService: {ErrorMessage}", exception.Message);
        }
    }
}
