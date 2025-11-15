using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DocumentManagement.Dtos
{
    public class DocumentFolderDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentFolderId { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public List<Guid> DocumentIds { get; set; }
        public int DocumentCount { get; set; }
    }
}
