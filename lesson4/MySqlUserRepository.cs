using System;
using System.Collections.Generic;
using MySqlConnector;

namespace lesson4;

public class MySqlUserRepository : IUserRepository
{
    // Docker'daki MySQL'e bağlanmak için gereken bilgiler
    private readonly string _connectionString = "Server=localhost;Port=3306;User ID=root;Password=mysecretpassword;Database=mysqldemo";

    public MySqlUserRepository() 
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var command = new MySqlCommand("CREATE TABLE IF NOT EXISTS Users (Id INT AUTO_INCREMENT PRIMARY KEY, Name TEXT NOT NULL)", connection);
        command.ExecuteNonQuery();
    }

    public void Add(User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var command = new MySqlCommand("INSERT INTO Users (Name) VALUES (@name)", connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.ExecuteNonQuery();
        Console.WriteLine($"[MYSQL] {user.Name} MySQL veritabanına eklendi!");
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var command = new MySqlCommand("SELECT Id, Name FROM Users", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1) });
        }
        return users;
    }
}
