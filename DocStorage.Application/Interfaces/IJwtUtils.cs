using DocStorage.Application.Adapters;

namespace DocStorage.Service.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
    }
}
