using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class FreelanceClientDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public ClientStatus Status { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; }
        public DateTime? RelationshipStartDate { get; set; }
        public bool HasOverdueFollowUps { get; set; }
        public int ProjectCount { get; set; }
    }
}
