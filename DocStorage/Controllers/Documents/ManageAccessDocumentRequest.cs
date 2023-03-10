using DocStorage.Api.Attributes;

namespace DocStorage.Api.Controllers.Documents
{
    public class ManageAccessDocumentRequest
    {
        [GuidNotEmpty]
        public Guid Id { get; set; }
    }
}
