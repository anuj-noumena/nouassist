using System;

namespace NouAssist.WorkManagement.Approval.Model;

public class ApprovalTrend
{
    public DateTime Date { get; set; }
    public int RequestsCreated { get; set; }
    public int RequestsCompleted { get; set; }
    public int RequestsApproved { get; set; }
    public int RequestsRejected { get; set; }
    public int RequestsCancelled { get; set; }
    public int PendingRequests { get; set; }
    public double AverageProcessingTimeHours { get; set; }
}