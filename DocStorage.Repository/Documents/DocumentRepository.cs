using DocStorage.Domain.Document;

namespace DocStorage.Repository.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private RepositoryContext _context;

        public DocumentRepository(RepositoryContext context)
        {
            _context = context;
            _context.Schema = "app_documents";
        }

        public async Task<IDocument> Get(Guid id)
        {
            return await _context.Execute("get_document_by_id", new Document { Id = id });
        }

        public async Task<IDocument> Add(IDocument entity)
        {
            return await _context.Execute("insert_document", new Document(entity));
        }
    }
}
