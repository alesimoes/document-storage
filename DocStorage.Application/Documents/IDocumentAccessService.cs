using DocStorage.Application.Adapters;

namespace DocStorage.Application.Documents
{
    public interface IDocumentAccessService
    {
        public Task AddUserAccess(DocumentAccess entity);
        public Task RemoveUserAccess(DocumentAccess entity);
        public Task AddGroupAccess(DocumentAccess entity);
        public Task RemoveGroupAccess(DocumentAccess entity);
    }
}
