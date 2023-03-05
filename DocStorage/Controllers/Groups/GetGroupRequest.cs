namespace DocStorage.Api.Controllers.Groups
{
    public class GetGroupRequest
    {
        public Guid Id { get; set; }

        public GetGroupRequest(Guid id)
        {
            this.Id = id;
        }

    }
}
