using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Application.Documents.Validators;
using DocStorage.Domain.Document;

namespace DocStorage.Application.Documents
{
    public class DocumentAccessService : IDocumentAccessService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentAccessRepository _repository;

        public DocumentAccessService(IMapper mapper, IDocumentAccessRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task AddUserAccess(DocumentAccess entity)
        {
            entity.Validate();
            await _repository.AddUserAccess(entity);
        }

        public async Task RemoveUserAccess(DocumentAccess entity)
        {
            entity.Validate();
            await _repository.RemoveUserAccess(entity);
        }

        public async Task AddGroupAccess(DocumentAccess entity)
        {
            entity.Validate();
            await _repository.AddGroupAccess(entity);
        }

        public async Task RemoveGroupAccess(DocumentAccess entity)
        {
            entity.Validate();
            await _repository.RemoveGroupAccess(entity);
        }
    }
}