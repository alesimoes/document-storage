namespace DocStorage.Api.Controllers.Documents
{
    public class FileMetaData
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
