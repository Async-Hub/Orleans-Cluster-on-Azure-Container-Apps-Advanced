using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ShoppingApp.SiloHost.MicrosoftSqlServer;

public class SqlDatabaseInitializer
{
    private readonly string _connectionString;

    public SqlDatabaseInitializer(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public void Run()
    {
        if(IsTableExist("OrleansQuery")) return;

        var assembly = Assembly.GetExecutingAssembly();
        const string sqlServerMainSqlFileName =
            "ShoppingApp.SiloHost.MicrosoftSqlServer.SQLServer-Main.sql";
        const string sqlServerPersistenceSqlFileName =
            "ShoppingApp.SiloHost.MicrosoftSqlServer.SQLServer-Persistence.sql";

        using var stream1 = assembly.GetManifestResourceStream(sqlServerMainSqlFileName);
        using var stream2 = assembly.GetManifestResourceStream(sqlServerPersistenceSqlFileName);

        if (stream1 == null)
        {
            throw new InvalidOperationException("Can't read SQLServer-Main.sql file as embedded resource.");
        }

        if (stream2 == null)
        {
            throw new InvalidOperationException("Can't read SQLServer-Persistence.sql file as embedded resource.");
        }

        using var reader1 = new StreamReader(stream1);
        using var reader2 = new StreamReader(stream2);
        var sqlServerMainSql = reader1.ReadToEnd();
        var sqlServerPersistenceSql = reader2.ReadToEnd();

        using var sqlConnection = new SqlConnection(_connectionString);
        SqlTransaction? sqlTransaction = null;

        try
        {
            sqlConnection.Open();
            var sqlCommand1 = new SqlCommand(sqlServerMainSql, sqlConnection);
            sqlCommand1.ExecuteNonQuery();

            sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.Serializable);
            var sqlCommand2 = new SqlCommand(sqlServerPersistenceSql, sqlConnection, sqlTransaction);
            sqlCommand2.ExecuteNonQuery();
            sqlTransaction.Commit();
        }
        catch(Exception)
        {
            sqlTransaction?.Rollback();
            throw;
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    private bool IsTableExist(string tableName)
    {
        var sql = $"SELECT CASE WHEN OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL THEN 1 ELSE 0 END";
        using var sqlConnection = new SqlConnection(_connectionString);
        var sqlCommand = new SqlCommand(sql, sqlConnection);
        try
        {
            sqlConnection.Open();
            return (int)sqlCommand.ExecuteScalar() == 1;
        }
        finally
        {
            sqlConnection.Close();
        }
    }
}