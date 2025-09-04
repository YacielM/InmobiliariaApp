using MySql.Data.MySqlClient;
 using Microsoft.Extensions.Configuration;
 namespace InmobiliariaApp.Data
 {
    public abstract class ConexionBase
    {
        protected readonly string _connectionString;
        public ConexionBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
 }