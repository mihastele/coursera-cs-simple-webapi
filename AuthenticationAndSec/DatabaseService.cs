using Microsoft.Data.Sqlite;

namespace AuthenticationAndSec;

public class DatabaseService : IDisposable
{
    private readonly SqliteConnection _connection;
    
    public DatabaseService(string connectionString)
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        InitializeDatabase();
    }
    
    private void InitializeDatabase()
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL,
            Email TEXT NOT NULL,
            PasswordHash TEXT NOT NULL,
            Role TEXT NOT NULL DEFAULT 'User'
        )
        """;
        command.ExecuteNonQuery();
    }
    
    public void AddUser(string username, string email, string password)
    {
        var passwordHash = SecurityHelper.HashPassword(password);
        
        using var command = _connection.CreateCommand();
        command.CommandText = """
        INSERT INTO Users (Username, Email, PasswordHash, Role)
        VALUES (@username, @email, @passwordHash, 'User')
        """;
        
        command.Parameters.AddWithValue("@username", SecurityHelper.SanitizeInput(username));
        command.Parameters.AddWithValue("@email", SecurityHelper.SanitizeInput(email));
        command.Parameters.AddWithValue("@passwordHash", passwordHash);
        
        command.ExecuteNonQuery();
    }
    
    public void AddAdminUser(string username, string email, string password)
    {
        var passwordHash = SecurityHelper.HashPassword(password);
        
        using var command = _connection.CreateCommand();
        command.CommandText = """
        INSERT INTO Users (Username, Email, PasswordHash, Role)
        VALUES (@username, @email, @passwordHash, 'Admin')
        """;
        
        command.Parameters.AddWithValue("@username", SecurityHelper.SanitizeInput(username));
        command.Parameters.AddWithValue("@email", SecurityHelper.SanitizeInput(email));
        command.Parameters.AddWithValue("@passwordHash", passwordHash);
        
        command.ExecuteNonQuery();
    }
    
    public bool VerifyLogin(string username, string password)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
        SELECT PasswordHash FROM Users 
        WHERE Username = @username
        """;
        
        command.Parameters.AddWithValue("@username", SecurityHelper.SanitizeInput(username));
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var storedHash = reader.GetString(0);
            return SecurityHelper.VerifyPassword(password, storedHash);
        }
        
        return false;
    }
    
    public string GetUserRole(string username)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
        SELECT Role FROM Users 
        WHERE Username = @username
        """;
        
        command.Parameters.AddWithValue("@username", SecurityHelper.SanitizeInput(username));
        
        using var reader = command.ExecuteReader();
        return reader.Read() ? reader.GetString(0) : null;
    }
    
    public bool IsUserAdmin(string username)
    {
        var role = GetUserRole(username);
        return role == "Admin";
    }
    
    public void Dispose()
    {
        _connection?.Dispose();
    }
}
