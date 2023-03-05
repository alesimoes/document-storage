
namespace DocStorage.Domain.Document
{
    public interface IDocumentAccessRepository
    {
        public Task AddUserAccess(IDocumentAccess documentAccess);
        public Task RemoveUserAccess(IDocumentAccess documentAccess);
        public Task AddGroupAccess(IDocumentAccess documentAccess);
        public Task RemoveGroupAccess(IDocumentAccess documentAccess);
    }
}
