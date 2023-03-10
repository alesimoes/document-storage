using DocStorage.Api.Attributes;
using DocStorage.Application.Adapters;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Groups
{
    public class UpdateGroupCommand
    {
        [GuidNotEmpty]
        public Guid Id { get; set; }

        [Required]
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
