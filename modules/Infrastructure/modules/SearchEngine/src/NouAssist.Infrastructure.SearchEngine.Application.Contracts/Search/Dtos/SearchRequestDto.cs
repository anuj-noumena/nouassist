using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Infrastructure.SearchEngine.Enums;

namespace NouAssist.Infrastructure.SearchEngine.Search.Dtos
{
    public class SearchRequestDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(500)]
        public string Query { get; set; }

        public List<SearchableEntityType> EntityTypes { get; set; }
        public Dictionary<string, object> Filters { get; set; }
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
        public SearchSortBy SortBy { get; set; } = SearchSortBy.Relevance;
    }
}
