using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Notifications.Dtos
{
    public class UpdateSubscriptionRuleDto
    {
        [Required]
        public NotificationChannel Channel { get; set; }
    }
}
