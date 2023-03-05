using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Auth
{
    public class LoginCommand
    {
        public string Username { get; set; }
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
