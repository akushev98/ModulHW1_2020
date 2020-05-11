using Npgsql;

namespace WebApplication.Services
{
    public static class Connection
    {
        public static NpgsqlConnection CreateConnection()
        {
            var connection =
                new NpgsqlConnection(
                    $"server=localhost;database=MyWebApi.Dev;userid=postgres;password=modulpass20;Pooling=false;");
            return connection;
        }
    }
}