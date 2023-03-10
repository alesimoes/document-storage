using DocStorage.Domain.Groups;

namespace DocStorage.Repository.Groups
{
    public class GroupRepository : IGroupRepository
    {
        private RepositoryContext _context;

        public GroupRepository(RepositoryContext context)
        {
            _context = context;
            _context.Schema = "app_users";
        }

        public async Task<IGroup> Get(Guid id)
        {
            return await _context.Execute("get_group_by_id", new Group { Id = id });
        }

        public async Task<IGroup> Add(IGroup entity)
        {
            return await _context.Execute("insert_group", new Group(entity));
        }

        public async Task<IGroup> Update(IGroup entity)
        {
            return await _context.Execute("update_group", new Group(entity));
        }

        public async Task Delete(Guid id)
        {
            await _context.Execute("delete_group", new Group { Id = id });
        }

        public async Task AddUser(Guid id, Guid userId)
        {
            await _context.Execute("add_user_group", new UserGroup(id, userId));
        }

        public async Task RemoveUser(Guid id, Guid userId)
        {
            await _context.Execute("delete_user_group", new UserGroup(id, userId));
        }
    }
}
