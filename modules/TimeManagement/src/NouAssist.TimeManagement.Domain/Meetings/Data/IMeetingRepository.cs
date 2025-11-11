using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.TimeManagement.Meetings.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.TimeManagement.Meetings.Data
{
    public interface IMeetingRepository : IRepository<Meeting, Guid>
    {
        Task<Meeting> GetWithAttendeesAsync(Guid id);
        Task<Meeting> GetWithFullDetailsAsync(Guid id);
        Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId);
        Task<List<Meeting>> GetMeetingsForDateAsync(DateTime date, Guid userId);
        Task<List<Meeting>> GetMeetingsByProjectAsync(Guid projectId);
        Task<List<Meeting>> GetRecurringSeriesAsync(Guid seriesId);
        Task<List<Meeting>> GetListAsync(ISpecification<Meeting> spec);
    }
}
