using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Search.Search.Dtos
{
    public class SavedSearchDto : CreationAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public string Filters { get; set; }
        public int UsageCount { get; set; }
        public DateTime? LastUsedAt { get; set; }
    }
}
