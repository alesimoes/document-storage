using DocStorage.Domain.Users;
using DocStorage.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocStorage.Repository.Users
{
    [Table("user")]
    public class User : EntityBase, IUser
    {
        public User()
        {

        }

        public User(IUser user)
        {
            this.Id = user.Id;
            this.Role = user.Role;
            this.Password = user.Password;
            this.Username = user.Username;
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public Role Role { get; set; }

        public override string Mapping => "user_info";

    }
}
