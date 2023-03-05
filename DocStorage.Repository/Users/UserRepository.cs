using Dapper;
using DocStorage.Domain.Users;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Security;
using System.Data;

namespace DocStorage.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly ISecurityContext _securityService;

        public UserRepository(IConnectionFactory connection, ISecurityContext securityService)
        {
            _connection = connection;
            _securityService = securityService;
        }

        public async Task<IUser> Get(Guid id)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", id, dbType: DbType.Guid);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<User>("fn_get_user_by_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task<IUser> Add(IUser entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_info", new User(entity), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<User>("fn_insert_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task<IUser> Update(IUser entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_info", new User(entity), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<User>("fn_update_user",
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
                parameters.Add("id", id, DbType.Guid);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<User>("fn_delete_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task<IUser> AuthorizeUser(IUser entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_info", new User(entity), dbType: DbType.Object);
                var result = await connectionDb.QueryFirstOrDefaultAsync<User>("fn_authorize_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }
    }
}
