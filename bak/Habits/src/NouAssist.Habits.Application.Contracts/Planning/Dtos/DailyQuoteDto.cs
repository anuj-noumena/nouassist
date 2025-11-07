using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Habits.Planning.Dtos
{
    public class DailyQuoteDto : EntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime? ViewedAt { get; set; }
        public bool HasBeenViewed { get; set; }
        public bool HasAuthor { get; set; }
    }
}
