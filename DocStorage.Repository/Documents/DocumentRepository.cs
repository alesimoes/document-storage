using Dapper;
using DocStorage.Domain.Document;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Security;
using System.Data;

namespace DocStorage.Repository.Documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IConnectionFactory _connection;
        private readonly ISecurityContext _securityService;

        public DocumentRepository(IConnectionFactory connection, ISecurityContext securityService)
        {
            _connection = connection;
            _securityService = securityService;
        }

        public async Task<IDocument> Get(Guid id)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", id, DbType.Guid);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Document>("fn_get_document_by_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }

        public async Task<IDocument> Add(IDocument entity)
        {
            using (var connectionDb = _connection.Connection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@document_info", new Document(entity), DbType.Object);
                parameters.Add("current_user_id", _securityService.UserId, dbType: DbType.Guid);
                var result = await connectionDb.QueryFirstOrDefaultAsync<Document>("fn_insert_document",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                result.Validate();
                return result;
            }
        }
    }
}
