using MySql.Data.MySqlClient;

namespace CraftingTools.Persistence;

public static class Connect
{
    public static void Try()
    {
        var connectionString = "server=10.0.0.79;port=23306;userid=root;password=q2uQQbrf8rW4";
        MySqlConnection? connection = default;
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

            var command = new MySqlCommand(cmdText: "create database test_DB", connection);
            command.ExecuteNonQuery();
        }
        finally
        {
            connection?.Close();
        }
    }
}
