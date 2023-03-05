using DocStorage.Repository.Exceptions;

namespace DocStorage.Repository
{
    public class EntityBase
    {
        string message;
        public string Message { get => message; set => message = value; }

        public void Validate()
        {
            HandleErrors.Validate(message);
        }

    }
}
