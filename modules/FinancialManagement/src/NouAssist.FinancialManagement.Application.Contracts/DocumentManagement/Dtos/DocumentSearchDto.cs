using System;
using NouAssist.FinancialManagement.DocumentManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.DocumentManagement.Dtos
{
    public class DocumentSearchDto : PagedAndSortedResultRequestDto
    {
        public string SearchTerm { get; set; }
        public DocumentType? Type { get; set; }
        public DocumentStatus? Status { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public DateTime? UploadedAfter { get; set; }
        public DateTime? UploadedBefore { get; set; }
    }
}
