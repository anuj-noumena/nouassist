using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SharedKernel.Logs.Dtos
{
    public class LogActivityDto : EntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? Description { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime OccurredAt { get; set; }
    }

    public class CreateLogActivityDto
    {
        public string Action { get; set; }
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? Description { get; set; }
    }

    public class GetLogActivityListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? EntityType { get; set; }
    }
}
