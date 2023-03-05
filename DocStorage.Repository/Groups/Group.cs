using DocStorage.Domain.Groups;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorage.Repository.Groups
{
    [Table("tb_group")]
    public class Group : EntityBase, IGroup
    {
        public Group()
        {
        }

        public Group(IGroup entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
