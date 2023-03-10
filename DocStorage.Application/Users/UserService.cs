using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Application.Auth;
using DocStorage.Application.Users.Validators;
using DocStorage.Domain.Users;

namespace DocStorage.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public UserService(IMapper mapper, IUserRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<User> Add(User user)
        {
            user.Validate();
            var hash = Security.HashPassword(user.Password);
            user.Password = hash;

            var newUser = await _repository.Add(user);
            return _mapper.Map<User>(newUser);
        }

        public async Task<User> Update(User user)
        {
            var updatedUser = await _repository.Update(user);
            return _mapper.Map<User>(updatedUser);
        }

        public async Task<User> Get(Guid id)
        {
            var currentUser = await _repository.Get(id);
            return _mapper.Map<User>(currentUser);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }
    }
}