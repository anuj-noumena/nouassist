namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class GetUserAccessProfileListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
