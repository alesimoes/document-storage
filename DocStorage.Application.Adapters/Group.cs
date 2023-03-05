using DocStorage.Domain.Groups;

namespace DocStorage.Application.Adapters
{
    public class Group : IGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
