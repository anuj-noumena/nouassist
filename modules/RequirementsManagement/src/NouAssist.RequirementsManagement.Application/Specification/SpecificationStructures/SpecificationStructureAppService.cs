using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Interfaces.Services;
using NouAssist.RequirementsManagement.Specification.Permissions;
using NouAssist.RequirementsManagement.Specification.Repositories;
using NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using Volo.Abp.Timing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Guids;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures
{
    [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.Default)]
    public class SpecificationStructureAppService : ApplicationService, ISpecificationStructureAppService
    {
        private readonly ISpecificationStructureRepository _structureRepository;
        private readonly IRequirementRepository _requirementRepository;
        private readonly ISpecificationFormattingService _formattingService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public SpecificationStructureAppService(
            ISpecificationStructureRepository structureRepository,
            IRequirementRepository requirementRepository,
            ISpecificationFormattingService formattingService,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _structureRepository = structureRepository;
            _requirementRepository = requirementRepository;
            _formattingService = formattingService;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(RequirementSpecificationManagementResource);
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.View)]
        public virtual async Task<SpecificationStructureDto> GetAsync(Guid id)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var dto = ObjectMapper.Map<SpecificationStructure, SpecificationStructureDto>(structure);
                await EnrichDtoAsync(dto, structure);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.View)]
        public virtual async Task<PagedResultDto<SpecificationStructureDto>> GetListAsync(GetSpecificationStructureListDto input)
        {
            try
            {
                var query = await _structureRepository.GetQueryableAsync();

                // Apply filters
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    query = query.Where(s => s.Name.Contains(input.Filter) || s.Description.Contains(input.Filter));
                }

                if (!input.DocumentType.IsNullOrWhiteSpace())
                {
                    query = query.Where(s => s.DocumentType == input.DocumentType);
                }

                if (input.CreatorId.HasValue)
                {
                    query = query.Where(s => s.CreatorId == input.CreatorId.Value);
                }

                if (input.CreatedAfter.HasValue)
                {
                    query = query.Where(s => s.CreationTime >= input.CreatedAfter.Value);
                }

                if (input.CreatedBefore.HasValue)
                {
                    query = query.Where(s => s.CreationTime <= input.CreatedBefore.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var structures = new List<SpecificationStructure>();
                if (totalCount > 0)
                {
                    structures = await AsyncExecuter.ToListAsync(
                        query.OrderBy(input.Sorting ?? nameof(SpecificationStructure.Name))
                             .PageBy(input)
                    );
                }

                var dtos = ObjectMapper.Map<List<SpecificationStructure>, List<SpecificationStructureDto>>(structures);

                foreach (var dto in dtos)
                {
                    var structure = structures.First(s => s.Id == dto.Id);
                    await EnrichDtoAsync(dto, structure);
                }

                return new PagedResultDto<SpecificationStructureDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.Create)]
        public virtual async Task<SpecificationStructureDto> CreateAsync(CreateSpecificationStructureDto input)
        {
            try
            {
                var currentTenantId = _currentTenant.Id;

                var structure = SpecificationStructure.Create(
                    _guidGenerator.Create(),
                    input.Name,
                    input.DocumentType,
                    input.Description,
                    currentTenantId
                );

                await _structureRepository.InsertAsync(structure, autoSave: true);

                var dto = ObjectMapper.Map<SpecificationStructure, SpecificationStructureDto>(structure);
                await EnrichDtoAsync(dto, structure);

                Logger.LogInformation(
                    "Specification structure created successfully: {StructureId} by user {UserId}",
                    structure.Id,
                    _currentUser.Id);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.Update)]
        public virtual async Task<SpecificationStructureDto> UpdateAsync(Guid id, UpdateSpecificationStructureDto input)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                structure.UpdateInfo(input.Name, input.DocumentType, input.Description);
                await _structureRepository.UpdateAsync(structure, autoSave: true);

                var dto = ObjectMapper.Map<SpecificationStructure, SpecificationStructureDto>(structure);
                await EnrichDtoAsync(dto, structure);

                Logger.LogInformation(
                    "Specification structure updated successfully: {StructureId} by user {UserId}",
                    structure.Id,
                    _currentUser.Id);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                await _structureRepository.DeleteAsync(structure, autoSave: true);

                Logger.LogInformation(
                    "Specification structure deleted successfully: {StructureId} by user {UserId}",
                    structure.Id,
                    _currentUser.Id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.ManageSections)]
        public virtual async Task<SpecificationSectionDto> AddSectionAsync(Guid structureId, CreateSpecificationSectionDto input)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var section = structure.AddSection(input.SectionName, input.Content, input.Order, input.ParentSectionId);
                await _structureRepository.UpdateAsync(structure, autoSave: true);

                return ObjectMapper.Map<SpecificationSection, SpecificationSectionDto>(section);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.ManageSections)]
        public virtual async Task<SpecificationSectionDto> UpdateSectionAsync(Guid structureId, Guid sectionId, UpdateSpecificationSectionDto input)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var section = structure.Sections.FirstOrDefault(s => s.Id == sectionId);
                if (section == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:SectionNotFound")
                        .WithData("sectionId", sectionId);
                }

                section.UpdateContent(input.SectionName, input.Content);
                section.UpdateOrder(input.Order);

                await _structureRepository.UpdateAsync(structure, autoSave: true);

                return ObjectMapper.Map<SpecificationSection, SpecificationSectionDto>(section);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.ManageSections)]
        public virtual async Task RemoveSectionAsync(Guid structureId, Guid sectionId)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                structure.RemoveSection(sectionId);
                await _structureRepository.UpdateAsync(structure, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.ManageSections)]
        public virtual async Task ReorderSectionsAsync(Guid structureId, List<Guid> sectionIds)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                // Update section orders based on the provided list
                for (int i = 0; i < sectionIds.Count; i++)
                {
                    var section = structure.Sections.FirstOrDefault(s => s.Id == sectionIds[i]);
                    if (section != null)
                    {
                        section.UpdateOrder(i + 1);
                    }
                }

                await _structureRepository.UpdateAsync(structure, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.View)]
        public virtual async Task<List<SpecificationSectionDto>> GetRootSectionsAsync(Guid structureId)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var rootSections = structure.GetRootSections();
                return ObjectMapper.Map<List<SpecificationSection>, List<SpecificationSectionDto>>(rootSections.ToList());
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.View)]
        public virtual async Task<List<SpecificationSectionDto>> GetSubsectionsAsync(Guid structureId, Guid parentSectionId)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var subsections = structure.GetSubsections(parentSectionId);
                return ObjectMapper.Map<List<SpecificationSection>, List<SpecificationSectionDto>>(subsections.ToList());
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.GenerateDocuments)]
        public virtual async Task<string> FormatDocumentAsync(Guid structureId, List<Guid> requirementIds, string format = "html")
        {
            try
            {
                var structure = await _structureRepository.GetAsync(structureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var requirements = new List<Requirement>();
                if (requirementIds.Any())
                {
                    var query = await _requirementRepository.WithDetailsAsync();
                    requirements = await AsyncExecuter.ToListAsync(
                        query.Where(r => requirementIds.Contains(r.Id))
                    );
                }

                return await _formattingService.FormatSpecificationDocumentAsync(structure, requirements, format);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.GenerateDocuments)]
        public virtual async Task<byte[]> GenerateDocumentAsync(GenerateDocumentDto input)
        {
            try
            {
                var structure = await _structureRepository.GetAsync(input.StructureId, includeDetails: true);
                await CheckResourceAccessAsync(structure);

                var requirements = new List<Requirement>();
                if (input.RequirementIds.Any())
                {
                    var query = await _requirementRepository.WithDetailsAsync();
                    requirements = await AsyncExecuter.ToListAsync(
                        query.Where(r => input.RequirementIds.Contains(r.Id))
                    );
                }

                return await _formattingService.GenerateDocumentAsync(structure, requirements, input.Format);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.Create)]
        public virtual async Task<SpecificationStructureDto> CloneAsync(Guid sourceId, string newName)
        {
            try
            {
                var sourceStructure = await _structureRepository.GetAsync(sourceId, includeDetails: true);
                await CheckResourceAccessAsync(sourceStructure);

                var newStructure = SpecificationStructure.Create(
                    _guidGenerator.Create(),
                    newName,
                    sourceStructure.DocumentType,
                    $"Clone of {sourceStructure.Description}",
                    _currentTenant.Id
                );

                // Clone sections
                foreach (var section in sourceStructure.GetRootSections())
                {
                    await CloneSectionRecursivelyAsync(newStructure, section, null);
                }

                await _structureRepository.InsertAsync(newStructure, autoSave: true);

                var dto = ObjectMapper.Map<SpecificationStructure, SpecificationStructureDto>(newStructure);
                await EnrichDtoAsync(dto, newStructure);

                Logger.LogInformation(
                    "Specification structure cloned successfully: {SourceId} -> {NewId} by user {UserId}",
                    sourceId,
                    newStructure.Id,
                    _currentUser.Id);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.SpecificationStructures.View)]
        public virtual async Task<List<SpecificationStructureDto>> GetTemplatesAsync()
        {
            try
            {
                // You might want to mark certain structures as templates via a property
                // For now, returning structures with specific document types that are considered templates
                var templateTypes = new[] { "Template", "SRS Template", "Standard Template" };
                
                var query = await _structureRepository.GetQueryableAsync();
                var templates = await AsyncExecuter.ToListAsync(
                    query.Where(s => templateTypes.Contains(s.DocumentType))
                         .OrderBy(s => s.Name)
                );

                var dtos = ObjectMapper.Map<List<SpecificationStructure>, List<SpecificationStructureDto>>(templates);

                foreach (var dto in dtos)
                {
                    var structure = templates.First(s => s.Id == dto.Id);
                    await EnrichDtoAsync(dto, structure);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper methods
        private async Task CloneSectionRecursivelyAsync(SpecificationStructure targetStructure, SpecificationSection sourceSection, Guid? parentId)
        {
            var clonedSection = targetStructure.AddSection(
                sourceSection.SectionName,
                sourceSection.Content,
                sourceSection.Order,
                parentId
            );

            // Clone subsections
            var sourceStructure = await _structureRepository.GetAsync(sourceSection.StructureId, includeDetails: true);
            var subsections = sourceStructure.GetSubsections(sourceSection.Id);
            foreach (var subsection in subsections)
            {
                await CloneSectionRecursivelyAsync(targetStructure, subsection, clonedSection.Id);
            }
        }

        private async Task EnrichDtoAsync(SpecificationStructureDto dto, SpecificationStructure structure)
        {
            dto.SectionsCount = structure.Sections.Count;
            await Task.CompletedTask;
        }

        private async Task CheckResourceAccessAsync(SpecificationStructure structure)
        {
            if (structure.TenantId != _currentTenant.Id)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:TenantMismatch");
            }
            await Task.CompletedTask;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception,
                "Error in SpecificationStructureAppService: {ErrorMessage}. User: {UserId}, Tenant: {TenantId}",
                exception.Message,
                _currentUser.Id,
                _currentTenant.Id);

            await Task.CompletedTask;
        }
    }
}
