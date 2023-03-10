using DocStorage.Api.Attributes;

namespace DocStorage.Api.Controllers.Groups
{
    public class ManageUserGroupRequest
    {
        [GuidNotEmpty]
        public Guid Id { get; set; }

    }
}
