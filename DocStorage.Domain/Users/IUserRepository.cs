namespace DocStorage.Domain.Users
{
    public interface IUserRepository
    {
        public Task<IUser> Get(Guid id);
        public Task Delete(Guid id);
        public Task<IUser> Add(IUser entity);
        public Task<IUser> Update(IUser entity);
        public Task<IUser> AuthorizeUser(IUser entity);
    }
}
