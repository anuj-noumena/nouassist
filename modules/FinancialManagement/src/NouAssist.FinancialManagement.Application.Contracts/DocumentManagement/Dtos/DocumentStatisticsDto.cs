using System.Collections.Generic;

namespace NouAssist.DocumentManagement.Dtos
{
    public class DocumentStatisticsDto
    {
        public long TotalDocuments { get; set; }
        public long TotalSize { get; set; }
        public Dictionary<string, int> DocumentsByType { get; set; }
        public Dictionary<string, int> DocumentsByStatus { get; set; }
        public int DocumentsPendingOCR { get; set; }
        public int ArchivedDocuments { get; set; }
    }
}
