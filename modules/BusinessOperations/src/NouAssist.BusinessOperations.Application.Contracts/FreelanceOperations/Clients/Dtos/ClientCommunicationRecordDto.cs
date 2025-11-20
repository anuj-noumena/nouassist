using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class ClientCommunicationRecordDto : EntityDto<Guid>
    {
        public Guid ClientId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool RequiresFollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public bool IsFollowUpOverdue { get; set; }
    }
}
