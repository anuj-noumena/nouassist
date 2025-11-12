using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SharedKernel.Areas.Dtos
{
    public class AreaDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? Purpose { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastReviewedAt { get; set; }
        public bool IsArchived { get; set; }
        public bool HasParent { get; set; }
        public bool IsRootArea { get; set; }
    }

    public class CreateAreaDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? Purpose { get; set; }
    }

    public class UpdateAreaDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Purpose { get; set; }
    }

    public class AreaWithChildrenDto : AreaDto
    {
        public List<AreaDto> Children { get; set; } = new();
    }
}
