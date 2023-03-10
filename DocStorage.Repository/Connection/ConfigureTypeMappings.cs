using DocStorage.Domain.ValueObjects;
using DocStorage.Repository.Documents;
using DocStorage.Repository.Groups;
using DocStorage.Repository.Users;
using Npgsql;

namespace DocStorage.Repository.Connection
{
    public class ConfigureTypeMappings
    {
        public ConfigureTypeMappings()
        {
            NpgsqlConnection.GlobalTypeMapper.MapComposite<User>("user_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Document>("document_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Group>("group_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<UserGroup>("user_group_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<DocumentAccess>("document_access_info");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>("public.user_role");
        }
    }
}
