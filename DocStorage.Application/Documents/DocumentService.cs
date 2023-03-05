using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Application.Documents.Validators;
using DocStorage.Domain.Document;

namespace DocStorage.Application.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _repository;

        public DocumentService(IMapper mapper, IDocumentRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Document> Get(Guid id)
        {
            var currentDocument = await _repository.Get(id);
            return _mapper.Map<Document>(currentDocument);
        }

        public async Task<Document> Add(Document document)
        {
            document.Validate();
            var newDocument = await _repository.Add(document);
            return _mapper.Map<Document>(newDocument);
        }
    }
}