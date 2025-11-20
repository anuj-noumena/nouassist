using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures
{
    /// <summary>
    /// Application service interface for specification structure management
    /// </summary>
    public interface ISpecificationStructureAppService : IApplicationService
    {
        // CRUD operations
        Task<SpecificationStructureDto> GetAsync(Guid id);
        Task<PagedResultDto<SpecificationStructureDto>> GetListAsync(GetSpecificationStructureListDto input);
        Task<SpecificationStructureDto> CreateAsync(CreateSpecificationStructureDto input);
        Task<SpecificationStructureDto> UpdateAsync(Guid id, UpdateSpecificationStructureDto input);
        Task DeleteAsync(Guid id);

        // Section management
        Task<SpecificationSectionDto> AddSectionAsync(Guid structureId, CreateSpecificationSectionDto input);
        Task<SpecificationSectionDto> UpdateSectionAsync(Guid structureId, Guid sectionId, UpdateSpecificationSectionDto input);
        Task RemoveSectionAsync(Guid structureId, Guid sectionId);
        Task ReorderSectionsAsync(Guid structureId, List<Guid> sectionIds);

        // Hierarchy management
        Task<List<SpecificationSectionDto>> GetRootSectionsAsync(Guid structureId);
        Task<List<SpecificationSectionDto>> GetSubsectionsAsync(Guid structureId, Guid parentSectionId);

        // Document generation
        Task<string> FormatDocumentAsync(Guid structureId, List<Guid> requirementIds, string format = "html");
        Task<byte[]> GenerateDocumentAsync(GenerateDocumentDto input);

        // Templates and copies
        Task<SpecificationStructureDto> CloneAsync(Guid sourceId, string newName);
        Task<List<SpecificationStructureDto>> GetTemplatesAsync();
    }
}
