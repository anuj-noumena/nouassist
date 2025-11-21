using System;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for affected requirement reference
    /// </summary>
    public class AffectedRequirementDto
    {
        public Guid RequirementId { get; set; }
        public string Version { get; set; }
        public string DisplayName { get; set; }
    }
}
