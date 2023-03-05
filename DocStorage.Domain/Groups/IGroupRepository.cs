
namespace DocStorage.Domain.Groups
{
    public interface IGroupRepository
    {
        public Task<IGroup> Get(Guid id);
        public Task Delete(Guid id);
        public Task<IGroup> Add(IGroup entity);
        public Task<IGroup> Update(IGroup entity);
        public Task AddUser(Guid id, Guid userId);
        public Task RemoveUser(Guid id, Guid userId);
    }
}
