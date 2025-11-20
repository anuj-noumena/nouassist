namespace NouAssist.RequirementsManagement.ChangeManagement.Enums
{
    /// <summary>
    /// Represents the types of impact a change request may have
    /// </summary>
    public enum ImpactType
    {
        /// <summary>
        /// Impact on project cost/budget
        /// </summary>
        Cost = 0,

        /// <summary>
        /// Impact on project timeline
        /// </summary>
        Time = 1,

        /// <summary>
        /// Impact on quality attributes
        /// </summary>
        Quality = 2,

        /// <summary>
        /// Impact on project scope
        /// </summary>
        Scope = 3,

        /// <summary>
        /// Impact on resources allocation
        /// </summary>
        Resources = 4,

        /// <summary>
        /// Impact on risks
        /// </summary>
        Risk = 5
    }
}
