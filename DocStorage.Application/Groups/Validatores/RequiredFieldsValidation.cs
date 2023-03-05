using DocStorage.Application.Adapters;

namespace DocStorage.Application.Groups.Validators
{
    public static class RequiredFieldsValidation
    {
        public static void Validate(this Group entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Group");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                throw new ArgumentNullException("Name");
            }
        }
    }
}
