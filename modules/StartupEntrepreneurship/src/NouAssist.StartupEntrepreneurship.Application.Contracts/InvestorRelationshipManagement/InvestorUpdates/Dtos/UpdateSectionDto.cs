using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO representing an update section
    /// </summary>
    public class UpdateSectionDto : EntityDto<Guid>
    {
        public Guid UpdateId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, object> ContentMetadata { get; set; } = new();
        public int Order { get; set; }
        public bool IsVisible { get; set; }
    }
}
