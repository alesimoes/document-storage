using DocStorage.Application.Adapters;
using DocStorage.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Users
{
    public class AddUserCommand
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
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
