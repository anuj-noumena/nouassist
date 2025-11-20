namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for change rationale
    /// </summary>
    public class ChangeRationaleDto : EntityDto<Guid>
    {
        public string RationaleText { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
