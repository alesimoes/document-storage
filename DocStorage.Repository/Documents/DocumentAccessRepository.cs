using Dapper;
using DocStorage.Domain.Document;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Security;
using System.Data;

namespace DocStorage.Repository.Documents
{
    public class DocumentAccessRepository : IDocumentAccessRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly ISecurityContext _securityService;

        public DocumentAccessRepository(IConnectionFactory connection, ISecurityContext securityService)
        {
            _connection = connection;
            _securityService = securityService;
        }


        public async Task AddGroupAccess(IDocumentAccess documentAccess)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("document_access_info", new DocumentAccess(documentAccess), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<DocumentAccess>("fn_add_document_access_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task AddUserAccess(IDocumentAccess documentAccess)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("document_access_info", new DocumentAccess(documentAccess), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<DocumentAccess>("fn_add_document_access_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task RemoveGroupAccess(IDocumentAccess documentAccess)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("document_access_info", new DocumentAccess(documentAccess), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<DocumentAccess>("fn_remove_document_access_group",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }

        public async Task RemoveUserAccess(IDocumentAccess documentAccess)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("document_access_info", new DocumentAccess(documentAccess), dbType: DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<DocumentAccess>("fn_remove_document_access_user",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
            }
        }
    }
}
