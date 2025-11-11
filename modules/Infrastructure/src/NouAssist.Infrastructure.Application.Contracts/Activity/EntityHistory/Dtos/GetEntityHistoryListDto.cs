using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Infrastructure.Activity.EntityHistory.Dtos
{
    public class GetEntityHistoryListDto
    {
        [Required]
        public string EntityType { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        public int MaxResultCount { get; set; } = 50;
    }
}
