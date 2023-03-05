using DocStorage.Application.Adapters;

namespace DocStorage.Api.Controllers.Groups
{
    public class AddGroupCommand
    {
        public string Name { get; set; }

        internal Group GetGroup()
        {
            return new Group
            {
                Id = Guid.NewGuid(),
                Name = Name
            };
        }
    }
}
