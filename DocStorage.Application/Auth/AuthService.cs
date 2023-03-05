using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Application.Auth.Validators;
using DocStorage.Domain.Users;
using DocStorage.Service.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;


namespace DocStorage.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public AuthService(IJwtUtils jwtUtils, IMapper mapper, IUserRepository repository)
        {
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<AuthenticatedUser> Authenticate(User user)
        {
            var authenticatedUser = await _repository.AuthorizeUser(user);

            if (!BCryptNet.Verify(user.Password, authenticatedUser.Password))
            {
                throw new AuthenticationFailedException();
            }

            var userModel = _mapper.Map<User>(authenticatedUser);
            var jwtToken = _jwtUtils.GenerateJwtToken(userModel);

            return new AuthenticatedUser(jwtToken);
        }
    }
}
