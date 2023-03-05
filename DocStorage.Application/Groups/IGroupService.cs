using DocStorage.Application.Adapters;

namespace DocStorage.Application.Groups
{
    public interface IGroupService
    {
        public Task<Group> Add(Group group);
        public Task<Group> Update(Group group);
        public Task Delete(Guid id);
        public Task<Group> Get(Guid id);
        public Task AddUser(Guid id, Guid userId);
        public Task RemoveUser(Guid id, Guid userId);
    }
}
