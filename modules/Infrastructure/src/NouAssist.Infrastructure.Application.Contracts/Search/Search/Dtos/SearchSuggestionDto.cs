namespace NouAssist.Infrastructure.Search.Search.Dtos
{
    public class SearchSuggestionDto
    {
        public string Query { get; set; }
        public string Source { get; set; }
        public double Score { get; set; }
        public int? UseCount { get; set; }
        public string Category { get; set; }
    }
}
