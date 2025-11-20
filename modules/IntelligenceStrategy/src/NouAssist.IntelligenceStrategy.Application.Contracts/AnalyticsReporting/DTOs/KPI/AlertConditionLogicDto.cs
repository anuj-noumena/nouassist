namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for alert condition logic
    /// </summary>
    public class AlertConditionLogicDto
    {
        public string ConditionExpression { get; set; }
        
        public int ConsecutiveBreaches { get; set; }
        
        public int SuppressMinutes { get; set; }
        
        public bool NotifyOnBackToNormal { get; set; }
    }
}
