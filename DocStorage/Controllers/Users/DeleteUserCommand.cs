namespace DocStorage.Api.Controllers.Users
{
    public class DeleteUserCommand
    {
        public Guid Id { get; }

        internal DeleteUserCommand(Guid id)
        {
            this.Id = id;
        }
    }
}
