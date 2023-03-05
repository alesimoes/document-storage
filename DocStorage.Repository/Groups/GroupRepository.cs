using Dapper;
using DocStorage.Domain.Groups;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Security;
using System.Data;

namespace DocStorage.Repository.Groups
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly ISecurityContext _securityService;

        public GroupRepository(IConnectionFactory connection, ISecurityContext securityService)
        {
            _connection = connection;
            _securityService = securityService;
        }
        public async Task<IGroup> Get(Guid id)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", id, dbType: DbType.Guid);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Group>("fn_get_group_by_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task<IGroup> Add(IGroup entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("group_info", new Group(entity), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Group>("fn_insert_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task<IGroup> Update(IGroup entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("group_info", new Group(entity), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Group>("fn_update_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task Delete(Guid id)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", id, dbType: DbType.Guid);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Group>("fn_delete_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task AddUser(Guid id, Guid userId)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_group_info", new UserGroup(id, userId), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<UserGroup>("fn_add_user_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task RemoveUser(Guid id, Guid userId)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_group_info", new UserGroup(id, userId), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<UserGroup>("fn_delete_user_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }
    }
}
