namespace DocStorage.Api.Controllers.Groups
{
    public class DeleteGroupCommand
    {
        public Guid Id { get; set; }

        public DeleteGroupCommand(Guid id)
        {
            this.Id = id;
        }

    }
}
