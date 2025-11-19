using System;
using System.Collections.Generic;

namespace NouAssist.FinancialManagement.AuditTrail.Dtos
{
    public class AuditStatisticsDto
    {
        public long TotalEntries { get; set; }
        public long TodayEntries { get; set; }
        public long ThisWeekEntries { get; set; }
        public long ThisMonthEntries { get; set; }
        public Dictionary<string, int> EntriesByEventType { get; set; }
        public Dictionary<string, int> EntriesBySeverity { get; set; }
        public List<TopUserActivityDto> TopUsers { get; set; }
    }

    public class TopUserActivityDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int ActivityCount { get; set; }
    }
}
