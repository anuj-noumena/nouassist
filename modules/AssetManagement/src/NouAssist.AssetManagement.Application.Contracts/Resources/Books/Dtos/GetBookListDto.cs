using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.AssetManagement.Resources.Books.Dtos
{
    public class GetBookListDto : PagedAndSortedResultRequestDto
    {
        public ReadingStatus? ReadingStatus { get; set; }
        public bool? IsFavorite { get; set; }
        public string Author { get; set; }
        public string SeriesName { get; set; }
        public string CollectionName { get; set; }
        public string SearchTerm { get; set; }
    }
}
