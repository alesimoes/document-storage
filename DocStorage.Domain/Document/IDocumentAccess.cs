namespace DocStorage.Domain.Document
{
    public interface IDocumentAccess
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid? EntityId { get; set; }
    }
}
