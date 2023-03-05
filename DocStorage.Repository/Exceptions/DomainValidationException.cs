namespace DocStorage.Repository.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string? message) : base(message)
        {
        }
    }
}
