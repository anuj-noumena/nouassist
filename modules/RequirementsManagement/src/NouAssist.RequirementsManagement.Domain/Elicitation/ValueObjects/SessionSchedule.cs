namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for session scheduling information
    /// </summary>
    public class SessionSchedule : ValueObject
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Location { get; private set; }
        public string TimeZone { get; private set; }

        private SessionSchedule() { }

        public SessionSchedule(DateTime startTime, DateTime endTime, string location = null, string timeZone = "UTC")
        {
            SetSchedule(startTime, endTime);
            SetLocation(location);
            TimeZone = timeZone ?? "UTC";
        }

        private void SetSchedule(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
            {
                throw new BusinessException("Elicitation:Validation:InvalidValue")
                    .WithData("propertyName", "EndTime");
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        private void SetLocation(string location)
        {
            if (!string.IsNullOrEmpty(location) && location.Length > ElicitationConsts.MaxLocationLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Location")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxLocationLength);
            }

            Location = location ?? string.Empty;
        }

        public TimeSpan Duration => EndTime - StartTime;

        public bool IsOverlapping(SessionSchedule other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartTime;
            yield return EndTime;
            yield return Location ?? string.Empty;
            yield return TimeZone;
        }
    }
}
