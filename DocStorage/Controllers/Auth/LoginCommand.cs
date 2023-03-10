using DocStorage.Application.Adapters;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Auth
{
    public class LoginCommand
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }


        internal User GetUser()
        {
            return new User
            {
                Username = Username,
                Password = Password,
                Role = Domain.ValueObjects.Role.Manager
            };
        }
    }
}
