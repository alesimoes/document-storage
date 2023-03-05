namespace DocStorage.Domain.Groups
{
    public interface IGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
