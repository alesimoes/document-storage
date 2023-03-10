using DocStorage.Domain.Users;
using DocStorage.Domain.ValueObjects;

namespace DocStorage.Application.Adapters
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}