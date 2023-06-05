using System.Data;
using System.Data.SqlClient;

namespace EmployeeServices.Context
{
    public class DapperContext
    {
        public readonly IConfiguration _db;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _db = configuration;
            _connectionString = _db.GetConnectionString("demoWithoutMVCContext");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
