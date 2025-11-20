namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object for details about a pitch presentation
    /// </summary>
    public class PresentationDetails : ValueObject
    {
        public DateTime PresentationDate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Location { get; private set; }
        public string MeetingFormat { get; private set; } // In-person, Virtual, etc.
        public List<string> Attendees { get; private set; }
        
        private PresentationDetails() 
        {
            Attendees = new List<string>();
        }
        
        public PresentationDetails(
            DateTime presentationDate, 
            TimeSpan duration, 
            string location, 
            string meetingFormat, 
            List<string> attendees)
        {
            PresentationDate = presentationDate;
            SetDuration(duration);
            Location = location;
            MeetingFormat = meetingFormat ?? throw new ArgumentNullException(nameof(meetingFormat));
            Attendees = attendees ?? new List<string>();
        }
        
        private void SetDuration(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be positive.");
            }
            
            Duration = duration;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PresentationDate;
            yield return Duration;
            yield return Location ?? string.Empty;
            yield return MeetingFormat;
            
            foreach (var attendee in Attendees)
            {
                yield return attendee;
            }
        }
    }
}
