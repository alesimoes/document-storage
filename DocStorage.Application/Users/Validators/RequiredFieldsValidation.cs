using DocStorage.Application.Adapters;

namespace DocStorage.Application.Users.Validators
{
    public static class RequiredFieldsValidation
    {
        public static void Validate(this User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("User");
            }

            if (string.IsNullOrEmpty(entity.Username))
            {
                throw new ArgumentNullException("Username");
            }

            if (string.IsNullOrEmpty(entity.Password))
            {
                throw new ArgumentNullException("Password");
            }

        }
    }
}
