using DocStorage.Application.Adapters;
using DocStorage.Domain.ValueObjects;

namespace DocStorage.Api.Controllers.Users
{
    public class AddUserCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        internal User GetUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = Username,
                Password = Password,
                Role = Role
            };
        }
    }
}
