using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class ChangeClientStatusDto
    {
        [Required]
        public ClientStatus NewStatus { get; set; }
    }
}
