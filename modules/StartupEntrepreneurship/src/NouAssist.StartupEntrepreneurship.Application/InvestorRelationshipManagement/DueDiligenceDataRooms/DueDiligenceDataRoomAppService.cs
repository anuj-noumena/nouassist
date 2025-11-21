using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Localization;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms
{
    /// <summary>
    /// Application service for due diligence data room management
    /// </summary>
    [Authorize(IRMPermissions.DataRooms.Default)]
    public class DueDiligenceDataRoomAppService : ApplicationService, IDueDiligenceDataRoomAppService
    {
        private readonly IDueDiligenceDataRoomRepository _dataRoomRepository;
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ISecureDocumentSharingService _documentSharingService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public DueDiligenceDataRoomAppService(
            IDueDiligenceDataRoomRepository dataRoomRepository,
            IInvestorProfileRepository investorRepository,
            ISecureDocumentSharingService documentSharingService,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _dataRoomRepository = dataRoomRepository;
            _investorRepository = investorRepository;
            _documentSharingService = documentSharingService;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<DueDiligenceDataRoomDto> GetAsync(Guid id)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);
                return await MapToExtendedDtoAsync(dataRoom);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<DueDiligenceDataRoomDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetWithDocumentsAsync(id);
                return await MapToExtendedDtoAsync(dataRoom, includeDetails: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<PagedResultDto<DueDiligenceDataRoomDto>> GetListAsync(GetDueDiligenceDataRoomListDto input)
        {
            try
            {
                var queryable = await _dataRoomRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    dr => dr.Name.Contains(input.Filter!) || dr.Description.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.IsActive.HasValue, dr => dr.IsActive == input.IsActive.Value);
                queryable = queryable.WhereIf(input.RequirePasswordAccess.HasValue, 
                    dr => dr.RequirePasswordAccess == input.RequirePasswordAccess.Value);
                queryable = queryable.WhereIf(input.ExpiringBefore.HasValue, 
                    dr => dr.ExpiryDate.HasValue && dr.ExpiryDate <= input.ExpiringBefore.Value);

                // Filter for accessible only if requested
                if (input.AccessibleOnly)
                {
                    // This would require more complex logic to check user access
                    // For now, we'll skip this filter or implement it based on business rules
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var items = new List<DueDiligenceDataRoom>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        queryable.OrderBy(input.Sorting ?? nameof(DueDiligenceDataRoom.Name))
                                .PageBy(input)
                    );
                }

                var itemDtos = new List<DueDiligenceDataRoomDto>();
                foreach (var item in items)
                {
                    var dto = await MapToExtendedDtoAsync(item, includeStatistics: input.IncludeStatistics);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<DueDiligenceDataRoomDto>(totalCount, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<PagedResultDto<DueDiligenceDataRoomDto>> GetAccessibleAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                // For now, return active data rooms
                // In a real implementation, this would check user access permissions
                var dataRooms = await _dataRoomRepository.GetActiveDataRoomsAsync();

                var itemDtos = new List<DueDiligenceDataRoomDto>();
                foreach (var dataRoom in dataRooms.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var dto = await MapToExtendedDtoAsync(dataRoom);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<DueDiligenceDataRoomDto>(dataRooms.Count, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.Create)]
        public async Task<DueDiligenceDataRoomDto> CreateAsync(CreateDueDiligenceDataRoomDto input)
        {
            try
            {
                var id = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                var dataRoom = DueDiligenceDataRoom.Create(
                    id,
                    input.Name,
                    input.Description,
                    input.ExpiryDate,
                    input.RequirePasswordAccess,
                    input.AccessPassword,
                    currentTenantId
                );

                await _dataRoomRepository.InsertAsync(dataRoom, autoSave: true);

                return await MapToExtendedDtoAsync(dataRoom);
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

        [Authorize(IRMPermissions.DataRooms.Update)]
        public async Task<DueDiligenceDataRoomDto> UpdateAsync(Guid id, UpdateDueDiligenceDataRoomDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.UpdateName(input.Name);
                dataRoom.UpdateDescription(input.Description);
                dataRoom.SetExpiryDate(input.ExpiryDate);
                dataRoom.SetPasswordAccess(input.RequirePasswordAccess, input.AccessPassword);

                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);

                return await MapToExtendedDtoAsync(dataRoom);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                await _dataRoomRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageAccess)]
        public async Task GrantAccessAsync(Guid id, GrantAccessDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                // Verify investor exists
                var investor = await _investorRepository.GetAsync(input.InvestorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                dataRoom.GrantAccess(input.InvestorId, input.AccessLevel);
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageAccess)]
        public async Task RevokeAccessAsync(Guid id, RevokeAccessDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.RevokeAccess(input.InvestorId);
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<PagedResultDto<DataRoomInvestorAccessDto>> GetInvestorAccessAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                var accessList = new List<DataRoomInvestorAccessDto>();

                foreach (var kvp in dataRoom.InvestorAccess.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var investor = await _investorRepository.FindAsync(kvp.Key);
                    if (investor != null)
                    {
                        var accessDto = new DataRoomInvestorAccessDto
                        {
                            InvestorId = kvp.Key,
                            InvestorName = investor.FullName,
                            InvestorEmail = investor.ContactDetails.Email,
                            AccessLevel = kvp.Value,
                            GrantedDate = DateTime.UtcNow, // This should come from audit data
                            LastAccessDate = null, // This would come from access logs
                            AccessCount = 0 // This would come from access logs
                        };
                        accessList.Add(accessDto);
                    }
                }

                return new PagedResultDto<DataRoomInvestorAccessDto>(
                    dataRoom.InvestorAccess.Count,
                    accessList
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<bool> CanInvestorAccessAsync(Guid id, Guid investorId)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    return false;
                }

                return dataRoom.CanInvestorAccess(investorId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                return false;
            }
        }

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task<PagedResultDto<SecureDocumentDto>> GetDocumentsAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetWithDocumentsAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                var documents = dataRoom.Documents
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.Name)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var documentDtos = new List<SecureDocumentDto>();
                foreach (var document in documents)
                {
                    var dto = await MapDocumentToDtoAsync(document);
                    documentDtos.Add(dto);
                }

                return new PagedResultDto<SecureDocumentDto>(
                    dataRoom.Documents.Count(d => d.IsActive),
                    documentDtos
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageDocuments)]
        public async Task<SecureDocumentDto> UploadDocumentAsync(Guid id, UploadDocumentDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetWithDocumentsAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                // In a real implementation, you would handle file upload here
                // For now, we'll create a mock document metadata
                var documentMetadata = new DocumentMetadata(
                    input.Name + ".pdf", // Mock filename
                    "application/pdf",
                    1024 * 1024, // 1MB mock size
                    Guid.NewGuid().ToString("N"), // Mock hash
                    _clock.Now,
                    _currentUser.Id ?? Guid.Empty
                );

                // Mock storage path - in real implementation this would be actual file storage
                var storagePath = $"/storage/datarooms/{id}/{Guid.NewGuid():N}.pdf";

                dataRoom.AddDocument(
                    input.Name,
                    input.Type,
                    documentMetadata,
                    storagePath,
                    input.Description
                );

                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);

                // Return the newly added document
                var newDocument = dataRoom.Documents.OrderByDescending(d => d.CreationTime).First();
                return await MapDocumentToDtoAsync(newDocument);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageDocuments)]
        public async Task<SecureDocumentDto> UpdateDocumentAsync(Guid id, Guid documentId, UpdateDocumentDto input)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetWithDocumentsAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.UpdateDocument(documentId, input.Name, input.Description);
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);

                // Return the updated document
                var updatedDocument = dataRoom.Documents.First(d => d.Id == documentId);
                return await MapDocumentToDtoAsync(updatedDocument);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:Document"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageDocuments)]
        public async Task DeleteDocumentAsync(Guid id, Guid documentId)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetWithDocumentsAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.RemoveDocument(documentId);
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:Document"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.ManageAccess)]
        public async Task<SecureShareLinkDto> GenerateShareLinkAsync(Guid id, GenerateShareLinkDto input)
        {
            try
            {
                var shareLink = await _documentSharingService.GenerateShareLinkAsync(
                    id,
                    input.DocumentId,
                    input.InvestorId,
                    input.ExpiryDate,
                    input.AccessLevel
                );

                return ObjectMapper.Map<SecureShareLink, SecureShareLinkDto>(shareLink);
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

        [Authorize(IRMPermissions.DataRooms.View)]
        public async Task LogDocumentAccessAsync(Guid id, Guid documentId, string action)
        {
            try
            {
                var currentUserId = _currentUser.Id ?? Guid.Empty;
                
                await _documentSharingService.LogDocumentAccessAsync(
                    id,
                    documentId,
                    currentUserId,
                    action
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                // Don't throw for logging errors to avoid disrupting user experience
            }
        }

        [Authorize(IRMPermissions.DataRooms.ViewAnalytics)]
        public async Task<PagedResultDto<DocumentAccessSummaryDto>> GetDocumentAccessReportAsync(Guid id, GenerateReportDto input)
        {
            try
            {
                var accessSummaries = await _documentSharingService.GetDocumentAccessReportAsync(
                    id,
                    input.FromDate,
                    input.ToDate
                );

                var summaryDtos = new List<DocumentAccessSummaryDto>();
                foreach (var summary in accessSummaries)
                {
                    var dto = new DocumentAccessSummaryDto
                    {
                        DocumentId = summary.DocumentId,
                        DocumentName = summary.DocumentName,
                        TotalAccesses = summary.TotalAccesses,
                        UniqueViewers = summary.UniqueViewers,
                        LastAccessed = summary.LastAccessed,
                        InvestorAccesses = summary.InvestorAccesses.Select(ia => new InvestorAccessSummaryDto
                        {
                            InvestorId = ia.InvestorId,
                            InvestorName = ia.InvestorName,
                            AccessCount = ia.AccessCount,
                            LastAccessed = ia.LastAccessed,
                            Actions = ia.Actions
                        }).ToList()
                    };
                    summaryDtos.Add(dto);
                }

                return new PagedResultDto<DocumentAccessSummaryDto>(summaryDtos.Count, summaryDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.Update)]
        public async Task ActivateAsync(Guid id)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.Activate();
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.DataRooms.Update)]
        public async Task DeactivateAsync(Guid id)
        {
            try
            {
                var dataRoom = await _dataRoomRepository.GetAsync(id);

                // Check tenant consistency
                if (dataRoom.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                }

                dataRoom.Deactivate();
                await _dataRoomRepository.UpdateAsync(dataRoom, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:DataRoom"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper Methods
        private async Task<DueDiligenceDataRoomDto> MapToExtendedDtoAsync(DueDiligenceDataRoom dataRoom, bool includeDetails = false, bool includeStatistics = true)
        {
            var dto = ObjectMapper.Map<DueDiligenceDataRoom, DueDiligenceDataRoomDto>(dataRoom);

            if (includeStatistics)
            {
                dto.DocumentCount = dataRoom.Documents.Count(d => d.IsActive);
                dto.InvestorCount = dataRoom.InvestorAccess.Count;
                dto.TotalDocumentSize = dataRoom.Documents.Where(d => d.IsActive).Sum(d => d.Metadata.FileSize);
                dto.LastAccessDate = dataRoom.Documents
                    .Where(d => d.IsActive && d.AccessLogs.Any())
                    .SelectMany(d => d.AccessLogs)
                    .OrderByDescending(log => log.AccessedAt)
                    .FirstOrDefault()?.AccessedAt;
            }

            // Check if current user can access password (simplified logic)
            dto.HasPasswordAccess = !dataRoom.RequirePasswordAccess || 
                                   await AuthorizationService.IsGrantedAsync(IRMPermissions.DataRooms.ManageAccess);

            if (includeDetails)
            {
                // Map documents
                foreach (var document in dataRoom.Documents.Where(d => d.IsActive))
                {
                    var documentDto = await MapDocumentToDtoAsync(document);
                    dto.Documents.Add(documentDto);
                }

                // Map investor access
                foreach (var kvp in dataRoom.InvestorAccess)
                {
                    var investor = await _investorRepository.FindAsync(kvp.Key);
                    if (investor != null)
                    {
                        var accessDto = new DataRoomInvestorAccessDto
                        {
                            InvestorId = kvp.Key,
                            InvestorName = investor.FullName,
                            InvestorEmail = investor.ContactDetails.Email,
                            AccessLevel = kvp.Value,
                            GrantedDate = DateTime.UtcNow, // This should come from audit data
                            LastAccessDate = null, // This would come from access logs
                            AccessCount = 0 // This would come from access logs
                        };
                        dto.InvestorAccess.Add(accessDto);
                    }
                }
            }

            return dto;
        }

        private async Task<SecureDocumentDto> MapDocumentToDtoAsync(SecureDocument document)
        {
            var dto = ObjectMapper.Map<SecureDocument, SecureDocumentDto>(document);

            // Set access permissions based on current user context
            // This is simplified - in reality, you'd check actual user permissions
            dto.CanAccess = true;
            dto.CanDownload = await AuthorizationService.IsGrantedAsync(IRMPermissions.DataRooms.View);

            // Set upload user name (simplified - you'd look this up from user service)
            dto.UploadedByName = "User"; // This should be looked up from user service

            // Generate download URL (simplified - this would be generated by controller or file service)
            if (dto.CanDownload)
            {
                dto.DownloadUrl = $"/api/irm/datarooms/{document.DataRoomId}/documents/{document.Id}/download";
            }

            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in DueDiligenceDataRoomAppService: {ErrorMessage}", exception.Message);
        }
    }
}
