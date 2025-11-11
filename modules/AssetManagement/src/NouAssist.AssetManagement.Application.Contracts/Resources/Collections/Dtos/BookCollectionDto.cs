using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.AssetManagement.Resources.Collections.Dtos
{
    public class BookCollectionDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BookCount { get; set; }
        public List<string> Tags { get; set; }
        public int? SortOrder { get; set; }
        public bool IsPublic { get; set; }
    }
}
