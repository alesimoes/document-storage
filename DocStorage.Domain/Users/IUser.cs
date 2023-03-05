using DocStorage.Domain.ValueObjects;

namespace DocStorage.Domain.Users
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
