using DocStorage.Application.Adapters;
using DocStorage.Domain.ValueObjects;

namespace DocStorage.Api.Controllers.Users
{
    public class UpdateUserCommand
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }

        internal User GetUser()
        {
            return new User
            {
                Id = Id,
                Role = Role
            };
        }
    }
}
