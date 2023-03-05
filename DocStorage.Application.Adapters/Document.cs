using DocStorage.Domain.Document;

namespace DocStorage.Application.Adapters
{
    public class Document : IDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime PostedDate { get; set; }
        public string Filename { get; set; }
    }
}
