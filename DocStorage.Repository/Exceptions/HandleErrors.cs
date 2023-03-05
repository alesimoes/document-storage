namespace DocStorage.Repository.Exceptions
{
    internal class HandleErrors
    {
        public static void Validate(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                throw new DomainValidationException(Resources.Messages.ResourceManager.GetString(error));
            }
        }
    }
}
