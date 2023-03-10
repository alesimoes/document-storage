using DocStorage.Domain.Document;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorage.Repository.Documents
{
    [Table("document")]
    public class Document : EntityBase, IDocument
    {
        public Document()
        {
        }

        public Document(IDocument document)
        {
            Id = document.Id;
            Name = document.Name;
            Description = document.Description;
            Category = document.Category;
            Filename = document.Filename;
            PostedDate = document.PostedDate;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Filename { get; set; }
        public DateTime PostedDate { get; set; }

        public override string Mapping => "document_info";
    }
}
