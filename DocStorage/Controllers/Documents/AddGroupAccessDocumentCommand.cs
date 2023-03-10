using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Documents
{
    public class AddGroupAccessDocumentCommand
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }

        public DocumentAccess GetDocumentAccess()
        {
            return new DocumentAccess
            {
                DocumentId = Id,
                EntityId = GroupId
            };
        }
    }
}
