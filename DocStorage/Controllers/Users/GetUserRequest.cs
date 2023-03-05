namespace DocStorage.Api.Controllers.Users
{
    public class GetUserRequest
    {
        public Guid Id { get; }

        internal GetUserRequest(Guid id)
        {
            this.Id = id;
        }
    }
}
