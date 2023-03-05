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
            NpgsqlConnection.GlobalTypeMapper.MapComposite<User>("tp_user_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Document>("tp_document_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Group>("tp_group_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<UserGroup>("tp_user_group_info");
            NpgsqlConnection.GlobalTypeMapper.MapComposite<DocumentAccess>("tp_document_access_info");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>("tp_user_role");
        }
    }
}
