using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Documents
{
    public class AddUserAccessDocumentCommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public DocumentAccess GetDocumentAccess()
        {
            return new DocumentAccess
            {
                DocumentId = Id,
                EntityId = UserId
            };
        }
    }
}
