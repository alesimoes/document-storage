using DocStorage.Repository.Contracts;
using Npgsql;
using System.Data;

namespace DocStorage.Repository.Connection
{
    public class DefaultPostgreConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public DefaultPostgreConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new NpgsqlConnection(_connectionString);
        }

        public void CreateInitialDatabase()
        {
            using (var db = Connection())
            {
                //db.Execute(Database.createstructure);
                //db.Execute(Database.create_users);
                //db.Execute(Database.create_group);
                //db.Execute(Database.create_documents);
                //db.Execute(Database.create_document_access);
            }
        }
    }
}
