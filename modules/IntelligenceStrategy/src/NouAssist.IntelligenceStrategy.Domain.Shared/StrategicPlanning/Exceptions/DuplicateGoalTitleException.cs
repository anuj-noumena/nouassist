namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a goal with a duplicate title
    /// </summary>
    public class DuplicateGoalTitleException : StrategicPlanningDomainException
    {
        public DuplicateGoalTitleException(string title)
            : base("StrategicPlanning:ErrorMessage:DuplicateGoalTitle")
        {
            WithData("title", title);
        }
    }
}
