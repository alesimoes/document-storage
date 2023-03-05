using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Repository.Contracts
{
    public interface IConnectionFactory
    {
        IDbConnection Connection();
        void CreateInitialDatabase();
    }
}
