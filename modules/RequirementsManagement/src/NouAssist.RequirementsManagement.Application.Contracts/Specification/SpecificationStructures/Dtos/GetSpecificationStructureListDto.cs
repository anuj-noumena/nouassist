using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    /// <summary>
    /// DTO for filtering and sorting specification structures list
    /// </summary>
    public class GetSpecificationStructureListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public Guid? CreatorId { get; set; }
    }
}
