using DocStorage.Domain.Users;

namespace DocStorage.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private RepositoryContext _context;

        public UserRepository(RepositoryContext context)
        {
            _context = context;
            _context.Schema = "app_users";
        }

        public async Task<IUser> Get(Guid id)
        {
            return await _context.Execute("get_user_by_id", new User { Id = id });
        }

        public async Task<IUser> Add(IUser entity)
        {
            return await _context.Execute("insert_user", new User(entity));
        }

        public async Task<IUser> Update(IUser entity)
        {
            return await _context.Execute("update_user", new User(entity));
        }

        public async Task Delete(Guid id)
        {
            await _context.Execute("delete_user", new User { Id = id });
        }

        public async Task<IUser> AuthorizeUser(IUser entity)
        {
            return await _context.Execute("authorize_user", new User(entity), true);
        }
    }
}
