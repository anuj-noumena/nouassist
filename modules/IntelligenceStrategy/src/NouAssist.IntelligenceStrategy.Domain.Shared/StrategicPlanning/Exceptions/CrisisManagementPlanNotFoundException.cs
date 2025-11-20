namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions
{
    /// <summary>
    /// Exception thrown when a crisis management plan is not found
    /// </summary>
    public class CrisisManagementPlanNotFoundException : StrategicPlanningDomainException
    {
        public CrisisManagementPlanNotFoundException(Guid planId)
            : base("StrategicPlanning:ErrorMessage:CrisisManagementPlanNotFound")
        {
            WithData("planId", planId);
        }
    }
}
