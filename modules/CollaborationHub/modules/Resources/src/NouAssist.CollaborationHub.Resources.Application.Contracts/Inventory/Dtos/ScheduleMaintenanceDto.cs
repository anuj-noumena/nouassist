using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.Resources.Inventory.Dtos
{
    public class ScheduleMaintenanceDto
    {
        [Required]
        public DateTime MaintenanceDate { get; set; }

        public string Notes { get; set; }
    }
}
