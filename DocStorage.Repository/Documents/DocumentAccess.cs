using DocStorage.Domain.Document;

namespace DocStorage.Repository.Documents
{
    public class DocumentAccess : EntityBase, IDocumentAccess
    {
        public DocumentAccess()
        {
        }

        public DocumentAccess(IDocumentAccess entity)
        {
            this.Id = entity.Id;
            this.DocumentId = entity.DocumentId;
            this.EntityId = entity.EntityId;
        }

        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid? EntityId { get; set; }

        public override string Mapping => "document_access_info";
    }
}
