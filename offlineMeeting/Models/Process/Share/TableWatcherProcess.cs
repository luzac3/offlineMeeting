using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Share;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public abstract class TableWatcherProcess: DBContext
{
    private readonly string _connectionString;
    private readonly string _sqlCommand;

    public TableWatcherProcess(string sqlCommand)
    {
        _connectionString = DbConnect.GetDBConnectionString();
        _sqlCommand = sqlCommand;
        SqlDependency.Start(_connectionString);
    }

    protected void RegisterDependency(SqlNotificationEventArgs? e = null)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        //using var command = new SqlCommand("SELECT [列1], [列2], ... FROM [OrderTable]", connection
        using var command = new SqlCommand(_sqlCommand, connection);

        var dependency = new SqlDependency(command);
        dependency.OnChange += OnSqlDependencyChange;

        // 実行して依存関係をセット
        using (var reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                GetReader(reader, e);
            }
            reader.Close();
        }
    }

    protected abstract void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e);

    protected abstract void GetReader(SqlDataReader reader, SqlNotificationEventArgs? e);
}