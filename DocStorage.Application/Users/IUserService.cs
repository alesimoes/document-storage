using DocStorage.Application.Adapters;

namespace DocStorage.Application.Users
{
    public interface IUserService
    {
        public Task<User> Get(Guid id);
        public Task<User> Add(User user);
        public Task<User> Update(User user);
        public Task Delete(Guid id);
    }
}
