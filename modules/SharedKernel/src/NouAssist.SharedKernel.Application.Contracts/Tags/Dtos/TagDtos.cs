using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SharedKernel.Tags.Dtos
{
    public class TagDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
        public int UsageCount { get; set; }
        public bool IsPopular { get; set; }
    }

    public class CreateTagDto
    {
        public string Title { get; set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
    }
    
    public class UpdateTagDto
    {
        public string? Title { get; set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class TagCategoryDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }
    }

    public class CreateTagCategoryDto
    {
        public string Name { get; set; }
        public string? Color { get; set; }
    }

    public class UpdateTagCategoryDto
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
    }

}
