using DocStorage.Application.Adapters;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Groups
{
    public class AddGroupCommand
    {
        [Required]
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
