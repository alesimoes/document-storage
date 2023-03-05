using DocStorage.Application.Adapters;

namespace DocStorage.Application.Documents
{
    public interface IDocumentService
    {
        public Task<Document> Add(Document model);
        public Task<Document> Get(Guid id);
    }
}
