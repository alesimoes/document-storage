namespace DocStorage.Api.Controllers.Groups
{
    public class RemoveUserFromGroupCommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

    }
}
