namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums
{
    /// <summary>
    /// Represents the type of trigger for personal automation
    /// </summary>
    public enum AutomationTriggerType
    {
        /// <summary>
        /// Trigger based on time (scheduled)
        /// </summary>
        TimeBased = 0,
        
        /// <summary>
        /// Trigger based on an event
        /// </summary>
        EventBased = 1,
        
        /// <summary>
        /// Trigger based on a condition
        /// </summary>
        ConditionBased = 2,
        
        /// <summary>
        /// Trigger based on manual activation
        /// </summary>
        Manual = 3,
        
        /// <summary>
        /// Trigger based on location
        /// </summary>
        LocationBased = 4
    }
}
