using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos
{
    /// <summary>
    /// DTO for querying document versions
    /// </summary>
    public class GetDocumentVersionsDto : PagedResultRequestDto
    {
        /// <summary>
        /// ID of the document
        /// </summary>
        public Guid DocumentId { get; set; }
    }
}
