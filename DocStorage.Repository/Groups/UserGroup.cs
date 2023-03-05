namespace DocStorage.Repository.Groups
{

    public class UserGroup : EntityBase
    {
        public UserGroup()
        {
        }

        public UserGroup(Guid groupId, Guid userId)
        {
            this.Id = Guid.NewGuid();
            this.GroupId = groupId;
            this.UserId = userId;
        }

        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }

    }
}
