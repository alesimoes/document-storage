using Dapper;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Security;
using System.Data;

namespace DocStorage.Repository
{
    public class RepositoryContext
    {
        private IConnectionFactory _connection;
        private ISecurityContext _securityContext;

        public RepositoryContext(IConnectionFactory connection, ISecurityContext securityContext)
        {
            _connection = connection;
            _securityContext = securityContext;
            Schema = "public";
        }

        public string Schema { get; internal set; }

        public async Task<T> Execute<T>(string function, T parameter, bool insecureContext = false) where T : EntityBase
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add(parameter.Mapping, parameter, dbType: DbType.Object);

                if (!insecureContext)
                {
                    parameters.Add("current_user_id", _securityContext.UserId, dbType: DbType.Guid);
                }

                var result = await connectionDb.QueryFirstOrDefaultAsync<T>($"{Schema}.{function}",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
