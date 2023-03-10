using DocStorage.Domain.Document;

namespace DocStorage.Repository.Documents
{
    public class DocumentAccessRepository : IDocumentAccessRepository
    {
        private RepositoryContext _context;

        public DocumentAccessRepository(RepositoryContext context)
        {
            _context = context;
            _context.Schema = "app_documents";
        }

        public async Task AddGroupAccess(IDocumentAccess documentAccess)
        {
            await _context.Execute("add_document_group", new DocumentAccess(documentAccess));
        }

        public async Task AddUserAccess(IDocumentAccess documentAccess)
        {
            await _context.Execute("add_document_user", new DocumentAccess(documentAccess));
        }

        public async Task RemoveGroupAccess(IDocumentAccess documentAccess)
        {
            await _context.Execute("remove_document_group", new DocumentAccess(documentAccess));
        }

        public async Task RemoveUserAccess(IDocumentAccess documentAccess)
        {
            await _context.Execute("remove_document_user", new DocumentAccess(documentAccess));
        }
    }
}
