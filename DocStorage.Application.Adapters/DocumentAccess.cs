using DocStorage.Domain.Document;

namespace DocStorage.Application.Adapters
{
    public class DocumentAccess : IDocumentAccess
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
    }
}
