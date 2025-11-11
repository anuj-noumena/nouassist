using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos
{
    public class CompleteSprintDto
    {
        [StringLength(5000)]
        public string RetrospectiveNotes { get; set; }
    }
}
