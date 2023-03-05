using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Application.Groups.Validators;
using DocStorage.Domain.Groups;

namespace DocStorage.Application.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _repository;

        public GroupService(IMapper mapper, IGroupRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Group> Get(Guid id)
        {
            var currentGroup = await _repository.Get(id);
            return _mapper.Map<Group>(currentGroup);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task<Group> Add(Group group)
        {
            group.Validate();
            var newGroup = await _repository.Add(group);
            return _mapper.Map<Group>(newGroup);
        }

        public async Task<Group> Update(Group group)
        {
            group.Validate();
            var updatedGroup = await _repository.Update(group);
            return _mapper.Map<Group>(updatedGroup);
        }

        public async Task AddUser(Guid id, Guid userId)
        {
            await _repository.AddUser(id, userId);
        }

        public async Task RemoveUser(Guid id, Guid userId)
        {
            await _repository.RemoveUser(id, userId);
        }
    }
}