using System;
using System.Collections.Generic;

namespace NouAssist.Search.Search.Dtos
{
    public class SearchAnalyticsDto
    {
        public int TotalSearches { get; set; }
        public double AverageResultCount { get; set; }
        public double AverageSearchDuration { get; set; }
        public double AverageQualityScore { get; set; }
        public double ClickThroughRate { get; set; }
        public int ZeroResultSearches { get; set; }
        public List<PopularQueryDto> TopQueries { get; set; }
    }

    public class PopularQueryDto
    {
        public string Query { get; set; }
        public int SearchCount { get; set; }
    }
}
