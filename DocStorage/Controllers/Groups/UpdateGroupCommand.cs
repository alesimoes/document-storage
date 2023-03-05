using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Groups
{
    public class UpdateGroupCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        internal Group GetGroup()
        {
            return new Group
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
