namespace NouAssist.DocumentManagement.Dtos
{
    public class DocumentDownloadDto
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
