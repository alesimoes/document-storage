

namespace DocStorage.Domain.Document
{
    public interface IDocumentRepository
    {
        public Task<IDocument> Add(IDocument entity);
        public Task<IDocument> Get(Guid id);
    }
}
