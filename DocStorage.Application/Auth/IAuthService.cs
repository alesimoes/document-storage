using DocStorage.Application.Adapters;

namespace DocStorage.Application.Auth
{
    public interface IAuthService
    {
        Task<AuthenticatedUser> Authenticate(User user);
    }
}
