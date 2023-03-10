using DocStorage.Api.Attributes;
using DocStorage.Application.Adapters;
using DocStorage.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Controllers.Users
{
    public class UpdateUserCommand
    {
        [GuidNotEmpty]
        public Guid Id { get; set; }

        [Required]
        public Role Role { get; set; }

        internal User GetUser()
        {
            return new User
            {
                Id = Id,
                Role = Role
            };
        }
    }
}
