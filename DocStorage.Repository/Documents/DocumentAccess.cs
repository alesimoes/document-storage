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
            this.GroupId = entity.GroupId;
            this.UserId = entity.UserId;
        }

        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
    }
}
