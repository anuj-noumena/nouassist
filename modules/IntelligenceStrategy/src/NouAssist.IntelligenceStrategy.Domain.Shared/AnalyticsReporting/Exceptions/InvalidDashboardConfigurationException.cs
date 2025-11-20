using System;

namespace NouAssist.Analytics.Exceptions
{
    /// <summary>
    /// Exception thrown when a dashboard configuration is invalid
    /// </summary>
    public class InvalidDashboardConfigurationException : AnalyticsDomainException
    {
        public Guid DashboardId { get; }
        
        public InvalidDashboardConfigurationException(Guid dashboardId, string reason)
            : base("Analytics:ErrorMessage:InvalidDashboardConfiguration")
        {
            DashboardId = dashboardId;
            WithData("dashboardId", dashboardId);
            WithData("reason", reason);
        }
    }
}
