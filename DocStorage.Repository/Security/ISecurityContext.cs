namespace DocStorage.Repository.Security
{
    public interface ISecurityContext
    {
        public Guid UserId { get; set; }
    }
}
