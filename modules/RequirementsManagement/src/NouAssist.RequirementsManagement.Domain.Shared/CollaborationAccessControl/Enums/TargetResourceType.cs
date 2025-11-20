namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Enums
{
    /// <summary>
    /// Defines the types of resources that can be targeted by permissions and notifications.
    /// </summary>
    public enum TargetResourceType
    {
        Requirement = 1,
        Baseline = 2,
        ChangeRequest = 3,
        ReviewCycle = 4,
        Project = 5,
        Feature = 6,
        TestCase = 7
    }
}
