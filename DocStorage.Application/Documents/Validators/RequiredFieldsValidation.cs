using DocStorage.Application.Adapters;

namespace DocStorage.Application.Documents.Validators
{
    public static class RequiredFieldsValidation
    {
        public static void Validate(this Document entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Document");
            }

            if (string.IsNullOrEmpty(entity.Filename))
            {
                throw new ArgumentNullException("Filename");
            }
        }

        public static void Validate(this DocumentAccess entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Document");
            }

            if (entity.DocumentId == Guid.Empty)
            {
                throw new ArgumentNullException("DocumentId");
            }

            if (entity.EntityId == Guid.Empty)
            {
                throw new ArgumentNullException("UserId or GroupId");
            }
        }
    }
}
