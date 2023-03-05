using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Documents
{
    public class AddDocumentCommand
    {
        public AddDocumentCommand(FileMetaData metaData)
        {
            this.Id = Guid.NewGuid();
            this.Name = metaData.Name;
            this.Description = metaData.Description;
            this.Category = metaData.Category;
        }

        public Document GetDocument()
        {
            return new Document
            {
                Id = Id,
                Description = Description,
                Category = Category,
                Filename = Filename,
                Name = Name,
                PostedDate = DateTime.Now.ToUniversalTime(),
            };
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Filename { get; set; }
    }
}
