namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when vesting schedule is invalid
    /// </summary>
    public class InvalidVestingScheduleException : CapTableDomainException
    {
        public InvalidVestingScheduleException(string reason)
            : base("CapTable:ErrorMessage:InvalidVestingSchedule")
        {
            WithData("reason", reason);
        }
    }
}
