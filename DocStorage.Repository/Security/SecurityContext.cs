namespace DocStorage.Repository.Security
{
    public class SecurityContext : ISecurityContext
    {
        public Guid UserId { get; set; }
    }
}
